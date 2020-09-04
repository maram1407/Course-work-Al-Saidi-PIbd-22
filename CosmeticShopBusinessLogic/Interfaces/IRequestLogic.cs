using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmeticShopBusinessLogic.Interfaces
{
    public interface IRequestLogic
    {
        List<RequestViewModel> Read(RequestBindingModel model);
        void CreateOrUpdate(RequestBindingModel model);
        void Delete(RequestBindingModel model);
        void Reserve(ReserveCosmeticsBindingModel model);
        void SaveJsonRequest(string folderName);
        void SaveJsonRequestCosmetic(string folderName);
        void SaveXmlRequest(string folderName);
        void SaveXmlRequestCosmetic(string folderName);
    }
}