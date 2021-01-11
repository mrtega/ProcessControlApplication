using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProcessControlApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessControlApplication.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Models.Action> Actions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationRole>().HasIndex(x => x.ParentRoleId).IsUnique();
            builder.Entity<ApplicationRole>().HasIndex(x => x.ChildRoleId).IsUnique();
            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = 1,
                    Name = "StoreKeeper",
                    ChildRoleId = 2,
                    NormalizedName = "STOREKEEPER"
                },
                new ApplicationRole
                {
                    Id = 2,
                    Name = "Supervisor",
                    ChildRoleId = 3,
                    ParentRoleId = 1,
                    NormalizedName = "SUPERVISOR"
                },
                new ApplicationRole
                {
                    Id = 3,
                    Name = "ProductManager",
                    ChildRoleId = 4,
                    ParentRoleId = 2,
                    NormalizedName = "PRODUCTMANAGER"
                },
                new ApplicationRole
                {
                    Id = 4,
                    Name = "Manager",
                    ParentRoleId = 3,
                    NormalizedName = "MANAGER"
                }
                );
        }
    }
}