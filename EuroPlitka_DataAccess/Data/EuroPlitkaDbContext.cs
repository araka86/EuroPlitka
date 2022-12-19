using EuroPlitka_Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EuroPlitka_DataAccess.Data
{
    public class EuroPlitkaDbContext: IdentityDbContext
    {

        public EuroPlitkaDbContext(DbContextOptions<EuroPlitkaDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<AplicationUser> AplicationUser { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Basket> Baskets { get; set; }


        public DbSet<Culture> Cultures { get; set; }
        public DbSet<Resource> Resources { get; set; }
    
        public DbSet<Europlitkaview> Views { get; set; }

        public DbSet<Pagefille> Pagefilles { get; set; }


    }
}
