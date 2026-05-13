using CLIENTI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CLIENTI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clienti { get; set; }
        public DbSet<Ordine> Ordini { get; set; }
        public DbSet<DettaglioOrdine> DettagliOrdine { get; set; }
        public DbSet<Articolo> Articoli { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DettaglioOrdine>()
                .HasOne(d => d.Ordine)
                .WithMany(o => o.DettagliOrdine)
                .HasForeignKey(d => d.OrdineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ordine>()
                .HasOne(o => o.Cliente)
                .WithMany(c => c.Ordini)
                .HasForeignKey(o => o.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Dati di esempio
            modelBuilder.Entity<Articolo>().HasData(
                new Articolo { Id = 1, Codice = "GW-001", Descrizione = "Gateway Domotico PRO", Tipo = TipoArticolo.ProdottoFinito, Giacenza = 10, Prezzo = 199.99M },
                new Articolo { Id = 2, Codice = "SN-001", Descrizione = "Sensore Temperatura WiFi", Tipo = TipoArticolo.ProdottoFinito, Giacenza = 50, Prezzo = 49.99M }
            );

            modelBuilder.Entity<Cliente>().HasData(
                new Cliente { Id = 1, UserId = "test-user-id", RagioneSociale = "Tech Corp", Indirizzo = "Via Roma 1", Citta = "Milano", CAP = "20100", Telefono = "02-123456", Email = "cliente@test.com" }
            );
        }
    }
}