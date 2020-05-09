using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsShopBusinessLogic.BindingModels
{
     public class OrderListBindingModel
    {
        public int? Id { get; set; }
        public string OrderName { get; set; }
        public decimal Sum { get; set; }
        public int OrderId { get; set; }
    }
}
