using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmeticShopBusinessLogic.Interfaces
{
    public interface IWerehouseLogic
    {
        List<WerehouseViewModel> Read(WerehouseBindingModel model);
        void CreateOrUpdate(WerehouseBindingModel model);
        void Delete(WerehouseBindingModel model);
        void AddCosmetic(RequestCosmeticBindingModel model);
        void ReserveCosmetics(RequestCosmeticBindingModel model);
        List<WerehouseAvailableViewModel> GetWerehouseAvailable(RequestCosmeticBindingModel model);
        void SaveJsonWerehouse(string folderName);
        void SaveJsonWerehouseCosmetic(string folderName);
        void SaveXmlWerehouse(string filderName);
        void SaveXmlWerehouseCosmetic(string filderName);
    }
}