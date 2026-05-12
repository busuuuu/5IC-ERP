using ERP_Ordini.Data;
using ERP_Ordini.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP_Ordini.Pages.Ordini
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context) => _context = context;

        public IList<Ordine> Ordini { get; set; }

        public async Task OnGetAsync()
        {
            Ordini = await _context.Ordini
                                   .Include(o => o.DettagliOrdine)
                                   .ToListAsync();
        }
    }
}