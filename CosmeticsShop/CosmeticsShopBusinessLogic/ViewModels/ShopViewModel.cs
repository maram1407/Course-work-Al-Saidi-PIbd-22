using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace CosmeticsShopBusinessLogic.ViewModels
{
   public class ShopViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название магазина")]
        public string ShopName { get; set; }
    }
}
