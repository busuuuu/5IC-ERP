using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CLIENTI.Data;
using CLIENTI.Models;

namespace CLIENTI.Pages.Clienti
{
    public class DettaglioOrdineModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DettaglioOrdineModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Ordine? Ordine { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Ordine = await _context.Ordini
                .Include(o => o.Cliente)
                .Include(o => o.DettagliOrdine)
                    .ThenInclude(d => d.Articolo)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (Ordine == null)
                return NotFound();

            return Page();
        }
    }
}