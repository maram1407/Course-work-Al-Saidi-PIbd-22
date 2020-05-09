using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace CosmeticsShopBusinessLogic.ViewModels
{
    public class OrderListViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название заказа")]
        public string OrderName { get; set; }
        [DisplayName("Сумма")]
        public decimal Sum { get; set; }
        public int OrderId { get; set; }
    }
}
