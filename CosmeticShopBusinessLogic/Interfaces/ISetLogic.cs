using System;
using System.Collections.Generic;
using System.Text;
using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.ViewModels;

namespace CosmeticShopBusinessLogic.Interfaces
{
    public interface ISetLogic
    {
        List<SetViewModel> Read(SetBindingModel model);
        void CreateOrUpdate(SetBindingModel model);
        void Delete(SetBindingModel model);
        void SaveJsonSet(string folderName);
        void SaveXmlSet(string folderName);
        void SaveJsonSetCosmetic(string folderName);
        void SaveXmlSetCosmetic(string folderName);
    }
}
