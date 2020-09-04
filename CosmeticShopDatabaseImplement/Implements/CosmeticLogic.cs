using Microsoft.EntityFrameworkCore;
using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.Interfaces;
using CosmeticShopBusinessLogic.ViewModels;
using CosmeticShopDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

namespace CosmeticShopDatabaseImplement.Implements
{
    public class CosmeticLogic : ICosmeticLogic
    {
        public void CreateOrUpdate(CosmeticBindingModel model)
        {
            using (var context = new CosmeticShopDatabase())
            {
                Cosmetic element = context.Cosmetics.FirstOrDefault(rec =>
               rec.CosmeticName == model.CosmeticName && rec.Id != model.Id);
                if (element != null)
                {
                    throw new Exception("Уже есть продукт с таким названием");
                }
                if (model.Id.HasValue)
                {
                    element = context.Cosmetics.FirstOrDefault(rec => rec.Id ==
                   model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                }
                else
                {
                    element = new Cosmetic();
                    context.Cosmetics.Add(element);
                }
                element.CosmeticName = model.CosmeticName;
                element.Price = model.Price;
                context.SaveChanges();
            }
        }
        public void Delete(CosmeticBindingModel model)
        {
            using (var context = new CosmeticShopDatabase())
            {
                Cosmetic element = context.Cosmetics.FirstOrDefault(rec => rec.Id ==
               model.Id);
                if (element != null)
                {
                    context.Cosmetics.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }
        public List<CosmeticViewModel> Read(CosmeticBindingModel model)
        {
            using (var context = new CosmeticShopDatabase())
            {
                return context.Cosmetics
                .Where(rec => model == null || rec.Id == model.Id)
                .Select(rec => new CosmeticViewModel
                {
                    Id = rec.Id,
                    CosmeticName = rec.CosmeticName,
                    Price = rec.Price
                })
                .ToList();
            }
        }

        public void SaveJsonCosmetic(string folderName)
        {
            string fileName = $"{folderName}\\Cosmetic.json";
            using (var context = new CosmeticShopDatabase())
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(IEnumerable<Cosmetic>));
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    jsonFormatter.WriteObject(fs, context.Cosmetics);
                }
            }
        }

        public void SaveXmlCosmetic(string folderName)
        {
            string fileName = $"{folderName}\\Cosmetic.xml";
            using (var context = new CosmeticShopDatabase())
            {
                XmlSerializer fomatter = new XmlSerializer(typeof(DbSet<Cosmetic>));
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    fomatter.Serialize(fs, context.Cosmetics);
                }
            }
        }
    }
}
