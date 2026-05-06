using Microsoft.EntityFrameworkCore;
using ErpPreventivi.Models;

namespace ErpPreventivi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Preventivo> Preventivi { get; set; }
    public DbSet<PreventivoRiga> PreventivoRighe { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Preventivo → Righe: cancellazione a cascata
        modelBuilder.Entity<PreventivoRiga>()
            .HasOne(r => r.Preventivo)
            .WithMany(p => p.Righe)
            .HasForeignKey(r => r.PreventivoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configurazione decimali per SQLite
        modelBuilder.Entity<PreventivoRiga>()
            .Property(r => r.Quantita)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<PreventivoRiga>()
            .Property(r => r.PrezzoUnitario)
            .HasColumnType("decimal(18,2)");
    }
}
