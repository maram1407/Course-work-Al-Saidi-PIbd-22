using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace CosmeticsShopBusinessLogic.ViewModels
{
   public class SupplierViewModel
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        [DisplayName("ФИО")]
        public string SupplierFIO { get; set; }
        [DisplayName("Пароль")]
        public int Password { get; set; }
    }
}
