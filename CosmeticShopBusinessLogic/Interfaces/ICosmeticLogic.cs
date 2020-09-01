using System;
using System.Collections.Generic;
using System.Text;
using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.ViewModels;

namespace CosmeticShopBusinessLogic.Interfaces
{
    public interface ICosmeticLogic
    {
        List<CosmeticViewModel> Read(CosmeticBindingModel model);
        void CreateOrUpdate(CosmeticBindingModel model);
        void Delete(CosmeticBindingModel model);
        void SaveJsonCosmetic(string folderName);
        void SaveXmlCosmetic(string folderName);
    }
}
