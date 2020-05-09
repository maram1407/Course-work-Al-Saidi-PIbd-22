using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using CosmeticsShopBusinessLogic.Enums; 
namespace CosmeticsShopBusinessLogic.ViewModels
{
   public class OrderViewModel
    {
        public int Id { get; set; }
        [DisplayName("Дата создание")]
        public DateTime CreationDate { get; set; }
        [DisplayName("Сумма")]
        public decimal Sum { get; set; }
        [DisplayName("Дата завершения")]
        public DateTime CompletionDate { get; set; }
        [DisplayName("Статус")]
        public Status Status { get; set; }
        
    }
}
