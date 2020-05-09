using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticsShopBusinessLogic.BindingModels;
using CosmeticsShopBusinessLogic.ViewModels;

namespace CosmeticsShopBusinessLogic.Interfaces
{
    interface ISupplierLogic
    {
        List<SupplierViewModel> Read(SupplierBindingModel model);
        void CreateOrUpdate(SupplierBindingModel model);
        void Delete(SupplierBindingModel model);
    }
}
