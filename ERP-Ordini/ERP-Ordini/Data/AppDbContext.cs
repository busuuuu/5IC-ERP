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
}//whats wrong here:
// Unable to create a 'DbContext' of type ''. The exception 'The entity type 'Ordine' requires a primary key to be defined. If you intended to use a keyless entity type, call 'HasNoKey' in 'OnModelCreating'. For more information on keyless entity types, see https://go.microsoft.com/fwlink/?linkid=2141943.' was thrown while attempting to create an instance. For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728
// The error indicates that the 'Ordine' entity does not have a primary key defined. In Entity Framework Core, every entity must have a primary key to uniquely identify each record. To fix this issue, you need to ensure that the 'Ordine' class has a property that is marked as the primary key, typically using the [Key] attribute or by convention (e.g., a property named 'Id' or 'OrdineId'). If 'Ordine' is meant to be a keyless entity, you should call 'HasNoKey' in the 'OnModelCreating' method for that entity.
// it has a [Key] data annotation. why does it not work?
// If the 'Ordine' class has a [Key] data annotation, but you are still encountering this error, there could be a few reasons why Entity Framework Core is not recognizing it:
// 1. **Namespace Issues**: Ensure that the 'Ordine' class is in the correct namespace and that it is being referenced correctly in the 'AppDbContext'.
