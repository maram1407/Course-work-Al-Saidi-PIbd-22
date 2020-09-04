using System;
using System.Collections.Generic;
using System.Text;

namespace CosmeticShopBusinessLogic.BindingModels
{
    public class CosmeticBindingModel
    {
        public int? Id { get; set; }
        public string CosmeticName { get; set; }
        public decimal Price { get; set; }
    }
}