using erp_corriere.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace erp_corriere.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Spedizione> Spedizioni { get; set; }
    }
}

