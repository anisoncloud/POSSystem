using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Branch> Branches => Set<Branch>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // 👈 Required — configures Identity tables

            builder.Entity<AppUser>()
                .HasOne(u=>u.Branch)
                .WithMany(b=>b.Users)
                .HasForeignKey(u=>u.BranchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
