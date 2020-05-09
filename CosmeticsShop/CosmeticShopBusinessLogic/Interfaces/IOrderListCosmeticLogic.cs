using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticsShopBusinessLogic.BindingModels;
using CosmeticsShopBusinessLogic.ViewModels;

namespace CosmeticsShopBusinessLogic.Interfaces
{
    interface IOrderListCosmeticLogic
    {
        List<OrderListCosmeticViewModel> Read(OrderListCosmeticBindingModel model);
        void CreateOrUpdate(OrderListCosmeticBindingModel model);
        void Delete(OrderListCosmeticBindingModel model);
    }
}
