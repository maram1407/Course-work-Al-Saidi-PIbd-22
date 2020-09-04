using System;
using System.Collections.Generic;
using System.Text;

namespace CosmeticShopBusinessLogic.BindingModels
{
    public class CreateRequestBindingModel
    {
        public int? SupplierId { get; set; }
        public Dictionary<int, (string, int)> Cosmetics { get; set; }
    }
}