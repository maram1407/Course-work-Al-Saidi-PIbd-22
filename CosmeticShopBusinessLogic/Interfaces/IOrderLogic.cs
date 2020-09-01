using System;
using System.Collections.Generic;
using System.Text;
using CosmeticShopBusinessLogic.BindingModels;
using CosmeticShopBusinessLogic.ViewModels;

namespace CosmeticShopBusinessLogic.Interfaces
{
    public interface IOrderLogic
    {
        List<OrderViewModel> Read(OrderBindingModel model);
        void CreateOrUpdate(OrderBindingModel model);
        void Delete(OrderBindingModel model);
        void SaveJsonOrder(string folderName);
        void SaveXmlOrder(string folderName);
    }
}
