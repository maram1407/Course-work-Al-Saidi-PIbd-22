using System;
using System.Collections.Generic;
using System.Text;

namespace CosmeticShopBusinessLogic.BindingModels
{
    public class SetCosmeticBindingModel
    {
        public int? Id { get; set; }
        public int SetId { get; set; }
        public int CosmeticId { get; set; }
        public int Count { get; set; }
    }
}
