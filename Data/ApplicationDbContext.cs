using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EarnIt.Models;

namespace EarnIt.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Child> Child { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<EventPoint> EventPoint { get; set; }
        public DbSet<Reward> Reward { get; set; }
        public DbSet<RewardEarned> RewardEarned { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Child>()
            .Property(b => b.DateCreated)
            .HasDefaultValueSql("now()");

            builder.Entity<Event>()
            .Property(b => b.DateCreated)
            .HasDefaultValueSql("now()");

            builder.Entity<EventPoint>()
            .Property(b => b.DateCreated)
            .HasDefaultValueSql("now()");

            builder.Entity<Reward>()
            .Property(b => b.DateCreated)
            .HasDefaultValueSql("now()");

            builder.Entity<RewardEarned>()
            .Property(b => b.DateEarned)
            .HasDefaultValueSql("now()");
        }
    }
}
