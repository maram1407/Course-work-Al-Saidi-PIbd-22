using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmeticShopBusinessLogic.Interfaces
{
    public interface ISupplierLogic
    {
        List<SupplierViewModel> Read(SupplierBindingModel model);
        void CreateOrUpdate(SupplierBindingModel model);
        void Delete(SupplierBindingModel model);
        void SaveJsonSupplier(string folderName);
        void SaveXmlSupplier(string folderName);
    }
}