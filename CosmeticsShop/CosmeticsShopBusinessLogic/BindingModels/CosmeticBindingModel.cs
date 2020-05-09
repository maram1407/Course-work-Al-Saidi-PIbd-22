﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsShopBusinessLogic.BindingModels
{
  public  class CosmeticBindingModel
    {
        public int? Id { get; set; }
        public string CosmeticName { get; set; }
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
