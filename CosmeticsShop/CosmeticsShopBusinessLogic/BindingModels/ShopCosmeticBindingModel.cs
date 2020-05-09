using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsShopBusinessLogic.BindingModels
{
   public class ShopCosmeticBindingModel
    {
        public int? Id { get; set; }
        public int ShopId { get; set; }
        public int CosmeticId { get; set; }
        public string CosmeticName { get; set; }
        public int Count { get; set; }
        public DateTime ReceipDate { get; set; }
    }
}
