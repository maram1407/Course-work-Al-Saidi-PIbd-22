using System;
using System.Collections.Generic;
using System.Text;

namespace CosmeticShopBusinessLogic.BindingModels
{
    public class RequestCosmeticBindingModel
    {
        public int WerehouseId { get; set; }
        public int CosmeticId { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
    }
}