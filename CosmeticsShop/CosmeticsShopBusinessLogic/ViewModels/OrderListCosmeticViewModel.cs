using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace CosmeticsShopBusinessLogic.ViewModels
{
   public class OrderListCosmeticViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название косметики")]
        public string CosmeticName { get; set; }
        [DisplayName("Цена")]
        public decimal Price { get; set; }
        [DisplayName("Количество")]
        public int Count { get; set; }
        [DisplayName("Сумма")]
        public decimal Sum { get; set; }
        public int CosmeticId { get; set; }
        public int OrderListId { get; set; }
    }
}
