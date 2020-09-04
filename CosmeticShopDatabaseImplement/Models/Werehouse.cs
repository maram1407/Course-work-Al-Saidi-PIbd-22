using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CosmeticShopDatabaseImplement.Models
{
    public class Werehouse
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Заполните поле")]
        [DisplayName("Название")]
        public string WerehouseName { get; set; }
        [Required(ErrorMessage = "Заполните поле")]
        [DisplayName("Вместимость")]
        public int Capacity { get; set; }
        [Required(ErrorMessage = "Заполните поле")]
        [DisplayName("Тип")]
        public string Type { get; set; }
        [ForeignKey("WerehouseId")]
        public virtual List<WerehouseCosmetic> WerehouseCosmetics { get; set; }
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}