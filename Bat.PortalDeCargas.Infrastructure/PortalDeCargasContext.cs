using Bat.PortalDeCargas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bat.PortalDeCargas.Infrastructure
{
    public class PortalDeCargasContext:DbContext
    {
        public PortalDeCargasContext(DbContextOptions<PortalDeCargasContext> options)
            :base(options){}

       public DbSet<Dimension> Dimension { get; set; }
       public DbSet<AppUser> Users { get; set; }
       public DbSet<DimensionDomain> DimensionDomain { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Dimension>().HasKey("DimensionId");
            modelBuilder.Entity<User>().HasKey("UserId");
            modelBuilder.Entity<Dimension>().HasOne(u => u.CreatedUser);
            modelBuilder.Entity<Dimension>().HasOne(u => u.UpdatedUser);*/
        }

    }
}
