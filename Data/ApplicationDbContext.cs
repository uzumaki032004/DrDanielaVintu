using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DrDanielaVintu.Models;

namespace DrDanielaVintu.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Service>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,2)");

            builder.Entity<SubscriptionPlan>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
