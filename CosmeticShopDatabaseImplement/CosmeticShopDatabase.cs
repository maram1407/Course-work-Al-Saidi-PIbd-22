using CosmeticShopDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace CosmeticShopDatabaseImplement
{
    class CosmeticShopDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-G18CFSK\SQLEXPRESS;Initial Catalog=CosmeticDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
                //optionsBuilder.UseSqlServer(@"Data Source=LAPTOP-IMFQ926R\SQLEXPRESS;Initial Catalog=RestaurantDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Cosmetic> Cosmetics { set; get; }
        public virtual DbSet<Set> Sets { set; get; }
        public virtual DbSet<SetCosmetic> SetCosmetics { set; get; }
        public virtual DbSet<Order> Orders { set; get; }
        public virtual DbSet<Werehouse> Werehouses { set; get; }
        public virtual DbSet<WerehouseCosmetic> WerehouseCosmetics { set; get; }
        public virtual DbSet<Request> Requests { set; get; }
        public virtual DbSet<RequestCosmetic> RequestCosmetics { set; get; }
        public virtual DbSet<Supplier> Suppliers { set; get; }
    }
}
