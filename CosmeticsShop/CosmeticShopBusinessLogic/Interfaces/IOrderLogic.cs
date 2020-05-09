using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticsShopBusinessLogic.BindingModels;
using CosmeticsShopBusinessLogic.ViewModels;

namespace CosmeticsShopBusinessLogic.Interfaces
{
    interface IOrderLogic
    {
        List<OrderViewModel> Read(OrderBindingModel model);
        void CreateOrUpdate(OrderBindingModel model);
        void Delete(OrderBindingModel model);
    }
}
