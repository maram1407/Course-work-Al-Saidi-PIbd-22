using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace CosmeticShopBusinessLogic.ViewModels
{
    public class CosmeticViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название косметики")]
        public string CosmeticName { get; set; }
        [DisplayName("Цена")]
        public decimal Price { get; set; }
    }
}