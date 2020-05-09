using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CosmeticsShopBusinessLogic.ViewModels
{
   public class CosmeticViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название косметики")]
        public string CosmeticName { get; set; }
        [DisplayName("Цена")]
        public decimal Price { get; set; }
        [DisplayName("Дата выпуска")]
        public DateTime ReleaseDate { get; set; }
        [DisplayName("Дата окончания")]
        public DateTime ExpiryDate { get; set; }
    }
}
