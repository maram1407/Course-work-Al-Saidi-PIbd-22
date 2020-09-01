using Microsoft.EntityFrameworkCore;
using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.Enums;
using CosmeticShopBusinessLogic.Interfaces;
using CosmeticShopBusinessLogic.ViewModels;
using CosmeticShopDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace CosmeticShopDatabaseImplement.Implements
{
    public class RequestLogic : IRequestLogic
    {
        public void CreateOrUpdate(RequestBindingModel model)
        {
            using (var context = new CosmeticShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Request request;
                        if (model.Id.HasValue)
                        {
                            request = context.Requests.FirstOrDefault(rec => rec.Id == model.Id);
                            if (request == null)
                            {
                                throw new Exception("Заявка не найдена");
                            }
                            var requestCosmetics = context.RequestCosmetics
                                .Where(rec => rec.RequestId == model.Id.Value).ToList();
                            context.RequestCosmetics.RemoveRange(requestCosmetics.Where(rec =>
                                !model.Cosmetics.ContainsKey(rec.CosmeticId)).ToList());
                            foreach (var updCosmetic in requestCosmetics)
                            {
                                updCosmetic.Count = model.Cosmetics[updCosmetic.CosmeticId].Item2;
                                updCosmetic.Inres = model.Cosmetics[updCosmetic.CosmeticId].Item3;
                                model.Cosmetics.Remove(updCosmetic.CosmeticId);
                            }
                            request.CompletionDate = model.CompletionDate;
                            context.SaveChanges();
                        }
                        else
                        {
                            request = new Request();
                            context.Requests.Add(request);
                        }
                        request.SupplierId = model.SupplierId;
                        request.Sum = model.Sum;
                        request.CreationDate = model.CreationDate;
                        request.Status = model.Status;
                        context.SaveChanges();
                        foreach (var Cosmetic in model.Cosmetics)
                        {
                            context.RequestCosmetics.Add(new RequestCosmetic
                            {
                                RequestId = request.Id,
                                CosmeticId = Cosmetic.Key,
                                Count = Cosmetic.Value.Item2,
                                Inres = false
                            });
                            context.SaveChanges();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(RequestBindingModel model)
        {
            if (model.Status != RequestStatus.Выполняется)
            {
                using (var context = new CosmeticShopDatabase())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            context.RequestCosmetics.RemoveRange(context
                                .RequestCosmetics.Where(rec => rec.RequestId == model.Id));
                            Request request = context.Requests.FirstOrDefault(rec => rec.Id == model.Id);
                            if (request != null)
                            {
                                context.Requests.Remove(request);
                                context.SaveChanges();
                            }
                            else
                            {
                                throw new Exception("Заявка не найдена");
                            }
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Заявку невозможно удалить. Заявка в процессе");
            }
        }

        public List<RequestViewModel> Read(RequestBindingModel model)
        {
            using (var context = new CosmeticShopDatabase())
            {
                return context.Requests
                    .Include(rec => rec.Supplier)
                    .Where(rec => model == null || rec.Id == model.Id || (rec.SupplierId == model.SupplierId) && (model.DateFrom == null && model.DateTo == null ||
                    rec.CompletionDate >= model.DateFrom && rec.CompletionDate <= model.DateTo && rec.Status == RequestStatus.Готова))
                    .ToList()
                    .Select(rec => new RequestViewModel
                    {
                        Id = rec.Id,
                        SupplierFIO = rec.Supplier.SupplierFIO,
                        SupplierId = rec.SupplierId,
                        CompletionDate = rec.CompletionDate,
                        CreationDate = rec.CreationDate,
                        Status = rec.Status,
                        Cosmetics = context.RequestCosmetics
                            .Include(recRF => recRF.Cosmetic)
                            .Where(recRF => recRF.RequestId == rec.Id)
                            .ToDictionary(recRF => recRF.CosmeticId, recRF =>
                            (recRF.Cosmetic?.CosmeticName, recRF.Count, recRF.Inres)),
                        Sum = Decimal.Round(context.RequestCosmetics
                            .Include(recRF => recRF.Cosmetic)
                            .Where(recRF => recRF.RequestId == rec.Id)
                            .Sum(recRF => recRF.Cosmetic.Price * recRF.Count), 2)
                    })
                    .ToList();
            }
        }
        public void Reserve(ReserveCosmeticsBindingModel model)
        {
            using (var context = new CosmeticShopDatabase())
            {
                var requestCosmetics = context.RequestCosmetics.FirstOrDefault(rec =>
                rec.RequestId == model.RequestId && rec.CosmeticId == model.CosmeticId);
                if (requestCosmetics == null)
                {
                    throw new Exception("Продукта нет в заявке");
                }
                requestCosmetics.Inres = true;
                context.SaveChanges();
            }
        }

        public void SaveJsonRequest(string folderName)
        {
            string fileName = $"{folderName}\\Request.json";
            using (var context = new CosmeticShopDatabase())
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(IEnumerable<Request>));
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    jsonFormatter.WriteObject(fs, context.Requests);
                }
            }
        }

        public void SaveJsonRequestCosmetic(string folderName)
        {
            string fileName = $"{folderName}\\RequestFood.json";
            using (var context = new CosmeticShopDatabase())
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(IEnumerable<RequestCosmetic>));
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    jsonFormatter.WriteObject(fs, context.RequestCosmetics);
                }
            }
        }

        public void SaveXmlRequest(string folderName)
        {
            string fileNameDop = $"{folderName}\\Request.xml";
            using (var context = new CosmeticShopDatabase())
            {
                XmlSerializer fomatterXml = new XmlSerializer(typeof(DbSet<Request>));
                using (FileStream fs = new FileStream(fileNameDop, FileMode.Create))
                {
                    fomatterXml.Serialize(fs, context.Requests);
                }
            }
        }

        public void SaveXmlRequestCosmetic(string folderName)
        {
            string fileNameDop = $"{folderName}\\RequestCosmetic.xml";
            using (var context = new CosmeticShopDatabase())
            {
                XmlSerializer fomatterXml = new XmlSerializer(typeof(DbSet<RequestCosmetic>));
                using (FileStream fs = new FileStream(fileNameDop, FileMode.Create))
                {
                    fomatterXml.Serialize(fs, context.RequestCosmetics);
                }
            }
        }
    }
}