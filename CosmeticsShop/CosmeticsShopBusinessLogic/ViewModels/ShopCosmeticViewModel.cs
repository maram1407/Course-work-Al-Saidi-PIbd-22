using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CosmeticsShopBusinessLogic.ViewModels
{
    public class ShopCosmeticViewModel
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int CosmeticId { get; set; }
        [DisplayName("Количество")]
        public int Count { get; set; }
        [DisplayName("Название косметики")]
        public string CosmeticName { get; set; }
        [DisplayName("Дата получения")]
        public DateTime ReceipDate { get; set; }
    }
}
