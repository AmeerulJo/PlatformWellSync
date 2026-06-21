using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlatformWellSync.Models;

namespace PlatformWellSync.Data
{
    internal class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Platform> Platforms => Set<Platform>();

        public DbSet<Well> Wells => Set<Well>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Platform>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Well>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Well>()
                .HasOne(x => x.Platform)
                .WithMany(x => x.Wells)
                .HasForeignKey(x => x.PlatformId);
        }
    }
}
