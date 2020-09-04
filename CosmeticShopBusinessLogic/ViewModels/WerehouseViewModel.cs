using CosmeticShopBusinessLogic.BindingModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CosmeticShopBusinessLogic.ViewModels
{
    public class WerehouseViewModel
    {
        [DisplayName("Номер склада")]
        public int Id { get; set; }
        [DisplayName("Название склад")]
        public string WerehouseName { get; set; }
        [DisplayName("Вместимость")]
        public int Capacity { get; set; }
        [DisplayName("Тип склада")]
        public string Type { get; set; }
        public Dictionary<int, (string, int, int)> Cosmetics { get; set; }
    }
}