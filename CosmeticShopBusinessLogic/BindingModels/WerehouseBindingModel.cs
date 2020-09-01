using System;
using System.Collections.Generic;
using System.Text;

namespace CosmeticShopBusinessLogic.BindingModels
{
    public class WerehouseBindingModel
    {
        public int? Id { get; set; }
        public int SupplierId { get; set; }
        public string WerehouseName { get; set; }
        public int Capacity { get; set; }
        public string Type { get; set; }
    }
}