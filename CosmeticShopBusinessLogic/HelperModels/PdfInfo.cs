using CosmeticShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace CosmeticShopBusinessLogic.HelperModels
{
    class PdfInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<ReportCosmeticViewModel> Cosmetics { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime DateFrom { get; set; }
    }
}