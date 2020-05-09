using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticsShopBusinessLogic.BindingModels;
using CosmeticsShopBusinessLogic.ViewModels;

namespace CosmeticsShopBusinessLogic.Interfaces
{
    interface ICosmeticLogic
    {
        List<CosmeticViewModel> Read(CosmeticBindingModel model);
        void CreateOrUpdate(CosmeticBindingModel model);
        void Delete(CosmeticBindingModel model);
    }
}
