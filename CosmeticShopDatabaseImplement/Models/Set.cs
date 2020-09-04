using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticShopDatabaseImplement.Models
{
    public class Set
    {
        public int Id { get; set; }
        [Required]
        public string SetName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [ForeignKey("SetId")]
        public virtual List<Order> Orders { get; set; }
        [ForeignKey("SetId")]
        public virtual List<SetCosmetic> SetCosmetics { get; set; }
    }
}
