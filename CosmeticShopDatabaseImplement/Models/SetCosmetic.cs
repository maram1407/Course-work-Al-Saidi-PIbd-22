using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CosmeticShopDatabaseImplement.Models
{
    public class SetCosmetic
    {
        public int Id { get; set; }
        [Required]
        public int Count { get; set; }
        public int CosmeticId { get; set; }
        public int SetId { get; set; }
        public Cosmetic Cosmetic { get; set; }
        public Set Set { get; set; }
    }
}