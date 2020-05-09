using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticsShopBusinessLogic.Enums;

namespace CosmeticsShopBusinessLogic.BindingModels
{
  public  class OrderBindingModel
    {
        public int? Id { get; set; }
        public decimal Sum { get; set; }
        public Status Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public int SupplierId { get; set; }
        
    }
}
