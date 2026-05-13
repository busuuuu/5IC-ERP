using Microsoft.EntityFrameworkCore;
using FornitoriERP.Models;

namespace FornitoriERP.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Fornitore> Fornitori { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fornitore>(entity =>
            {
                entity.HasKey(e => e.IdFornitore);
                entity.ToTable("5ic_fornitori");

                entity.Property(e => e.IdFornitore)
                    .HasColumnName("id_fornitore")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.NomeFornitore)
                    .HasColumnName("nome_fornitore")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.ContattoFornitore)
                    .HasColumnName("contatto_fornitore")
                    .HasMaxLength(255);

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Fornitore>().HasData(
                new Fornitore { IdFornitore = 1, NomeFornitore = "Acme Srl", ContattoFornitore = "Mario Rossi", Telefono = "02 1234567", Email = "mario.rossi@acme.it" },
                new Fornitore { IdFornitore = 2, NomeFornitore = "TechSupply Spa", ContattoFornitore = "Giulia Bianchi", Telefono = "06 7654321", Email = "g.bianchi@techsupply.it" },
                new Fornitore { IdFornitore = 3, NomeFornitore = "LogisticaPro", ContattoFornitore = "Luca Verdi", Telefono = "011 9988776", Email = "luca@logisticapro.com" }
            );
        }
    }
}
