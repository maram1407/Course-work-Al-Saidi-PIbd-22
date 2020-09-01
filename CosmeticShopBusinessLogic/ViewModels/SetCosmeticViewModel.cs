using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace CosmeticShopBusinessLogic.ViewModels
{
    public class SetCosmeticViewModel
    {
        public int Id { get; set; }
        public int CosmeticId { get; set; }
        public int SetId { get; set; }
        [DisplayName("Количество")]
        public int Count { get; set; }
    }
}
