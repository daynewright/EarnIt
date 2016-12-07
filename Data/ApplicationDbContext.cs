using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ModelBuilder.Entity<Child>()
            .Property(b => b.DateCreated)
            .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

            ModelBuilder.Entity<Event>()
            .Property(b => b.DateCreated)
            .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

            ModelBuilder.Entity<EventState>()
            .Property(b => b.DateCreated)
            .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

            ModelBuilder.Entity<Reward>()
            .Property(b => b.DateCreated)
            .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");
        }
    }
}
