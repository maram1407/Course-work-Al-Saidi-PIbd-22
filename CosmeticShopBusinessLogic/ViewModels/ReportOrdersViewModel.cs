using CosmeticShopBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmeticShopBusinessLogic.ViewModels
{
    public class ReportOrdersViewModel
    {
        public DateTime CreationDate { get; set; }
        public string SetName { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
        public Status Status { get; set; }
    }
}
