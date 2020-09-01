using CosmeticShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CosmeticShopBusinessLogic.HelperModels
{
    public class WordInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<SetViewModel> Sets { get; set; }
        public List<WerehouseViewModel> Werehouses { get; set; }
        public int RequestId { get; set; }
        public string SupplierFIO { get; set; }
        public Dictionary<int, (string, int, bool)> RequestCosmetics { get; set; }
        public List<ReportSetCosmeticViewModel> SetCosmetics { get; set; }
        public List<ReportOrdersViewModel> Orders { get; set; }
    }
}