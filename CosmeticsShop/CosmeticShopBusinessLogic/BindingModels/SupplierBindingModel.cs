using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsShopBusinessLogic.BindingModels
{
  public class SupplierBindingModel
    {
        public int? Id { get; set; }
        public int ShopId { get; set; }
        public string SupplierFIO { get; set; }
        public int Password { get; set; }
    }
}
