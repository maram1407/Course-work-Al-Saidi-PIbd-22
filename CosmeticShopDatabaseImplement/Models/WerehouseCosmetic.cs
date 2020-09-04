using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CosmeticShopDatabaseImplement.Models
{
    public class WerehouseCosmetic
    {
        public int Id { get; set; }
        public int WerehouseId { get; set; }
        public int CosmeticId { get; set; }
        [Required]
        public int Free { get; set; }
        [Required]
        public int Reserved { get; set; }
        public virtual Werehouse Werehouse { get; set; }
        public virtual Cosmetic Cosmetic { get; set; }
    }
}