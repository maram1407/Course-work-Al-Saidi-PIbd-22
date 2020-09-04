﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CosmeticShopBusinessLogic.BindingModels
{
    public class SetBindingModel
    {
        public int? Id { get; set; }
        public string SetName { get; set; }
        public decimal Price { get; set; }
        public Dictionary<int, (string, int)> SetCosmetics { get; set; }
    }
}
