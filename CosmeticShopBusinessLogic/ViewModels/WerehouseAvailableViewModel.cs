using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CosmeticShopBusinessLogic.ViewModels
{
    public class WerehouseAvailableViewModel
    {
        [DisplayName("Склад")]
        public string WerehouseName { get; set; }
        public int WerehouseId { get; set; }
        [DisplayName("Количество")]
        public int Count { get; set; }
    }
}