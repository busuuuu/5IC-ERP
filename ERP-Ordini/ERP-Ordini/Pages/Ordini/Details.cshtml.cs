using ERP_Ordini.Data;
using ERP_Ordini.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ERP_Ordini.Pages.Ordini
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;
        public DetailsModel(AppDbContext context) => _context = context;

        public Ordine Ordine { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Ordine = await _context.Ordini
                                   .Include(o => o.DettagliOrdine)
                                   .FirstOrDefaultAsync(o => o.IdOrdine == id);
            if (Ordine == null) return NotFound();
            return Page();
        }
    }
}