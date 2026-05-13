using ERP_Ordini.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP_Ordini.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Ordine> Ordini { get; set; }
        public DbSet<DettagliOrdine> DettagliOrdine { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ordine>()
                        .HasOne(o => o.DettagliOrdine)
                        .WithOne()
                        .HasForeignKey<Ordine>(o => o.IdDettagliOrdine)
                        .OnDelete(deleteBehavior:DeleteBehavior.Cascade);
        }
    }
}
