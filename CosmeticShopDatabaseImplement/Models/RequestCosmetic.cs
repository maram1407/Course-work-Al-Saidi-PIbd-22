using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CosmeticShopDatabaseImplement.Models
{
    public class RequestCosmetic
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int CosmeticId { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public bool Inres { get; set; }
        public virtual Request Request { get; set; }
        public virtual Cosmetic Cosmetic { get; set; }
    }
}