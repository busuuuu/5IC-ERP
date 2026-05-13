using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CLIENTI.Data;
using CLIENTI.Models;

namespace CLIENTI.Pages.Clienti
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Cliente? Cliente { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Cliente = await _context.Clienti
                .Include(c => c.Ordini.OrderByDescending(o => o.DataOrdine))
                    .ThenInclude(o => o.DettagliOrdine)
                        .ThenInclude(d => d.Articolo)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Cliente == null)
                return NotFound();

            return Page();
        }
    }
}