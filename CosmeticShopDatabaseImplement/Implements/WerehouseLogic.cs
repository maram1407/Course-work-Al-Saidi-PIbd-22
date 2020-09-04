using Microsoft.EntityFrameworkCore;
using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.Interfaces;
using CosmeticShopBusinessLogic.ViewModels;
using CosmeticShopDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace CosmeticShopDatabaseImplement.Implements
{
    public class WerehouseLogic : IWerehouseLogic
    {
        public List<WerehouseViewModel> Read(WerehouseBindingModel model)
        {
            using (var context = new CosmeticShopDatabase())
            {
                return context.Werehouses
                .Where(rec => model == null || rec.Id == model.Id
                    || rec.SupplierId == model.SupplierId)
                .ToList()
                .Select(rec => new WerehouseViewModel
                {
                    Id = rec.Id,
                    WerehouseName = rec.WerehouseName,
                    Capacity = rec.Capacity,
                    Type = rec.Type,
                    Cosmetics = context.WerehouseCosmetics
                            .Include(recCC => recCC.Cosmetic)
                            .Where(recCC => recCC.WerehouseId == rec.Id)
                            .ToDictionary(recCC => recCC.CosmeticId, recCC =>
                            (recCC.Cosmetic?.CosmeticName, recCC.Free, recCC.Reserved))
                })
                    .ToList();
            }
        }

        public void CreateOrUpdate(WerehouseBindingModel model)
        {
            using (var context = new CosmeticShopDatabase())
            {
                Werehouse element = context.Werehouses.FirstOrDefault(rec => rec.WerehouseName == model.WerehouseName && rec.Id != model.Id);
                if (element != null)
                {
                    throw new Exception("Уже существует склад с таким названием");
                }
                if (model.Id.HasValue)
                {
                    element = context.Werehouses.FirstOrDefault(rec => rec.Id == model.Id);
                    int free = context.WerehouseCosmetics.Where(rec =>
                    rec.WerehouseId == model.Id).Sum(rec => rec.Free);
                    int res = context.WerehouseCosmetics.Where(rec =>
                    rec.WerehouseId == model.Id).Sum(rec => rec.Reserved);
                    if ((free + res) > model.Capacity)
                    {
                        throw new Exception("Вместимость не может быть меньше количества мкосметики в складе");
                    }
                    if (element == null)
                    {
                        throw new Exception("Склад не найден");
                    }
                }
                else
                {
                    element = new Werehouse();
                    context.Werehouses.Add(element);
                }
                element.SupplierId = model.SupplierId;
                element.WerehouseName = model.WerehouseName;
                element.Capacity = model.Capacity;
                element.Type = model.Type;
                context.SaveChanges();
            }
        }

        public void Delete(WerehouseBindingModel model)
        {
            using (var context = new CosmeticShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.WerehouseCosmetics.RemoveRange(context.WerehouseCosmetics.Where(rec => rec.WerehouseId == model.Id));
                        Werehouse element = context.Werehouses.FirstOrDefault(rec => rec.Id == model.Id);
                        if (element != null)
                        {
                            context.Werehouses.Remove(element);
                            context.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("Склад не найден");
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

        public List<WerehouseAvailableViewModel> GetWerehouseAvailable(RequestCosmeticBindingModel model)
        {
            using (var context = new CosmeticShopDatabase())
            {
                return context.WerehouseCosmetics
                .Include(rec => rec.Werehouse)
                .Where(rec => rec.CosmeticId == model.CosmeticId
                && rec.Free >= model.Count)
                .Select(rec => new WerehouseAvailableViewModel
                {
                    WerehouseId = rec.WerehouseId,
                    WerehouseName = rec.Werehouse.WerehouseName,
                    Count = rec.Free
                })
                .ToList();
            }
        }

        public void AddCosmetic(RequestCosmeticBindingModel model)
        {
            using (var context = new CosmeticShopDatabase())
            {
                var werehouseCosmetics = context.WerehouseCosmetics.FirstOrDefault(rec =>
                 rec.WerehouseId == model.WerehouseId && rec.CosmeticId == model.CosmeticId);
                var fridge = context.Werehouses.FirstOrDefault(rec => rec.Id == model.WerehouseId);

                int free = context.WerehouseCosmetics.Where(rec =>
                rec.WerehouseId == model.WerehouseId).Sum(rec => rec.Free);
                int res = context.WerehouseCosmetics.Where(rec =>
                rec.WerehouseId == model.WerehouseId).Sum(rec => rec.Reserved);
                if ((free + res + model.Count) > fridge.Capacity)
                {
                    throw new Exception("Недостаточно места в холодильнике");
                }
                if (werehouseCosmetics == null)
                {
                    context.WerehouseCosmetics.Add(new WerehouseCosmetic
                    {
                        WerehouseId = model.WerehouseId,
                        CosmeticId = model.CosmeticId,
                        Free = model.Count,
                        Reserved = 0
                    });
                }
                else
                {
                    werehouseCosmetics.Free += model.Count;
                }

                Cosmetic element = context.Cosmetics.FirstOrDefault(rec =>
                    rec.Id == model.CosmeticId);
                element.Price = model.Price;
                context.SaveChanges();
            }
        }

        public void ReserveCosmetics(RequestCosmeticBindingModel model)
        {
            using (var context = new CosmeticShopDatabase())
            {
                var werehouseCosmetics = context.WerehouseCosmetics.FirstOrDefault(rec =>
                rec.WerehouseId == model.WerehouseId && rec.CosmeticId == model.CosmeticId);
                if (werehouseCosmetics != null)
                {
                    if (werehouseCosmetics.Free >= model.Count)
                    {
                        werehouseCosmetics.Free -= model.Count;
                        werehouseCosmetics.Reserved += model.Count;
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Недостаточно косметики");
                    }
                }
                else
                {
                    throw new Exception("В складе не существует такой кометики");
                }
            }
        }

        public void SaveJsonWerehouse(string folderName)
        {
            string fileName = $"{folderName}\\Werehouse.json";
            using (var context = new CosmeticShopDatabase())
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(IEnumerable<WerehouseLogic>));
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    jsonFormatter.WriteObject(fs, context.Werehouses);
                }
            }
        }

        public void SaveJsonWerehouseCosmetic(string folderName)
        {
            string fileName = $"{folderName}\\WerehouseCosmetic.json";
            using (var context = new CosmeticShopDatabase())
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(IEnumerable<WerehouseCosmetic>));
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    jsonFormatter.WriteObject(fs, context.WerehouseCosmetics);
                }
            }
        }

        public void SaveXmlWerehouse(string folderName)
        {
            string fileNameDop = $"{folderName}\\Werehouse.xml";
            using (var context = new CosmeticShopDatabase())
            {
                XmlSerializer fomatterXml = new XmlSerializer(typeof(DbSet<WerehouseLogic>));
                using (FileStream fs = new FileStream(fileNameDop, FileMode.Create))
                {
                    fomatterXml.Serialize(fs, context.Werehouses);
                }
            }
        }

        public void SaveXmlWerehouseCosmetic(string folderName)
        {
            string fileNameDop = $"{folderName}\\WerehouseCosmetic.xml";
            using (var context = new CosmeticShopDatabase())
            {
                XmlSerializer fomatterXml = new XmlSerializer(typeof(DbSet<WerehouseCosmetic>));
                using (FileStream fs = new FileStream(fileNameDop, FileMode.Create))
                {
                    fomatterXml.Serialize(fs, context.WerehouseCosmetics);
                }
            }
        }
    }
}