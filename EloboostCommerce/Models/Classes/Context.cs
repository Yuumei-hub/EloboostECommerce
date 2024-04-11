using Iyzipay.Model;
using Microsoft.EntityFrameworkCore;

namespace EloboostCommerce.Models.Classes
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=LAPTOP-9SC1960C\\SQLEXPRESS;" +
                "database=EloboostCommerceDB;" +
                "integrated security=true;");
        }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PayWithIyzico> PayWithIyzicos { get; set; }

    }
}