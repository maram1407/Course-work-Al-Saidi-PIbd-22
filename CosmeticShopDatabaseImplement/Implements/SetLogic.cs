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
    public class SetLogic : ISetLogic
    {
        public void CreateOrUpdate(SetBindingModel model)
        {
            using (var context = new CosmeticShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Set element = context.Sets.FirstOrDefault(rec =>
                       rec.SetName == model.SetName && rec.Id != model.Id);
                        if (element != null)
                        {
                            throw new Exception("Уже есть изделие с таким названием");
                        }
                        if (model.Id.HasValue)
                        {
                            element = context.Sets.FirstOrDefault(rec => rec.Id ==
                           model.Id);
                            if (element == null)
                            {
                                throw new Exception("Элемент не найден");
                            }
                        }
                        else
                        {
                            element = new Set();
                            context.Sets.Add(element);
                        }
                        element.SetName = model.SetName;
                        element.Price = model.Price;
                        context.SaveChanges();
                        if (model.Id.HasValue)
                        {
                            var SetCosmetics = context.SetCosmetics.Where(rec
                           => rec.SetId == model.Id.Value).ToList();
                            // удалили те, которых нет в модели
                            context.SetCosmetics.RemoveRange(SetCosmetics.Where(rec =>
                            !model.SetCosmetics.ContainsKey(rec.CosmeticId)).ToList());
                            context.SaveChanges();
                            // обновили количество у существующих записей
                            foreach (var updateCosmetic in SetCosmetics)
                            {
                                updateCosmetic.Count =
                               model.SetCosmetics[updateCosmetic.CosmeticId].Item2;

                                model.SetCosmetics.Remove(updateCosmetic.CosmeticId);
                            }
                            context.SaveChanges();
                        }
                        // добавили новые
                        foreach (var pc in model.SetCosmetics)
                        {
                            context.SetCosmetics.Add(new SetCosmetic
                            {
                                SetId = element.Id,
                                CosmeticId = pc.Key,
                                Count = pc.Value.Item2
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
        public void Delete(SetBindingModel model)
        {
            using (var context = new CosmeticShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // удаяем записи по продуктам при удалении закуски
                        context.SetCosmetics.RemoveRange(context.SetCosmetics.Where(rec =>
                        rec.SetId == model.Id));
                        Set element = context.Sets.FirstOrDefault(rec => rec.Id
                        == model.Id);
                        if (element != null)
                        {
                            context.Sets.Remove(element);
                            context.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("Элемент не найден");
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
        public List<SetViewModel> Read(SetBindingModel model)
        {
            using (var context = new CosmeticShopDatabase())
            {
                return context.Sets
                .Where(rec => model == null || rec.Id == model.Id)
                .ToList()
                .Select(rec => new SetViewModel
                {
                    Id = rec.Id,
                    SetName = rec.SetName,
                    Price = rec.Price,
                    SetCosmetics = context.SetCosmetics
                                        .Include(recPC => recPC.Cosmetic)
                                        .Where(recPC => recPC.SetId == rec.Id)
                                        .ToDictionary(recPC => recPC.CosmeticId, recPC => (
                                            recPC.Cosmetic?.CosmeticName, recPC.Count
                                         ))
                }).ToList();
            }
        }

        public void SaveJsonSet(string folderName)
        {
            string fileName = $"{folderName}\\Set.json";
            using (var context = new CosmeticShopDatabase())
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(IEnumerable<Set>));
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    jsonFormatter.WriteObject(fs, context.Sets);
                }
            }
        }

        public void SaveJsonSetCosmetic(string folderName)
        {
            string fileName = $"{folderName}\\SetCosmetic.json";
            using (var context = new CosmeticShopDatabase())
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(IEnumerable<SetCosmetic>));
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    jsonFormatter.WriteObject(fs, context.SetCosmetics);
                }
            }
        }

        public void SaveXmlSet(string folderName)
        {
            string fileName = $"{folderName}\\Set.xml";
            using (var context = new CosmeticShopDatabase())
            {
                XmlSerializer fomatter = new XmlSerializer(typeof(DbSet<Set>));
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    fomatter.Serialize(fs, context.Sets);
                }
            }
        }

        public void SaveXmlSetCosmetic(string folderName)
        {
            string fileName = $"{folderName}\\SetCosmetic.xml";
            using (var context = new CosmeticShopDatabase())
            {
                XmlSerializer fomatter = new XmlSerializer(typeof(DbSet<SetCosmetic>));
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    fomatter.Serialize(fs, context.SetCosmetics);
                }
            }
        }
    }
}
