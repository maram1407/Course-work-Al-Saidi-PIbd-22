using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsShopBusinessLogic.BindingModels
{
  public  class OrderListCosmeticBindingModel
    {
        public int? Id { get; set; }
        public string CosmeticName { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public int CosmeticId { get; set; }
        public int OrderListId { get; set; }
    }
}
