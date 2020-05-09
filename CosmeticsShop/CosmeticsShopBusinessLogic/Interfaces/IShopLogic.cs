using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticsShopBusinessLogic.BindingModels;
using CosmeticsShopBusinessLogic.ViewModels;

namespace CosmeticsShopBusinessLogic.Interfaces
{
    interface IShopLogic
    {
        List<ShopViewModel> GetList();
        ShopViewModel GetElement(int id);
        void AddElement(ShopBindingModel model);
        void UpdElement(ShopBindingModel model);
        void DelElement(int id);
        void FillShop(ShopCosmeticBindingModel model);
    }
}
