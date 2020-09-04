using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CosmeticShopDatabaseImplement.Models
{
    public class Cosmetic
    {
        public int Id { get; set; }
        [DisplayName("Название косметики")]
        [Required]
        public string CosmeticName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [ForeignKey("CosmeticId")]
        public virtual List<WerehouseCosmetic> WerehouseCosmetics { get; set; }
        [ForeignKey("CosmeticId")]
        public virtual List<RequestCosmetic> RequestCosmetics { get; set; }
    }
}