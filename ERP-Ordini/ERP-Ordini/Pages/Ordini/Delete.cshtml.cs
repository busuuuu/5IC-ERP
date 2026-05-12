using ERP_Ordini.Data;
using ERP_Ordini.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ERP_Ordini.Pages.Ordini
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;
        public DeleteModel(AppDbContext context) => _context = context;

        [BindProperty]
        public Ordine Ordine { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Ordine = await _context.Ordini
                                   .Include(o => o.DettagliOrdine)
                                   .FirstOrDefaultAsync(o => o.IdOrdine == id);
            if (Ordine == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Ordine = await _context.Ordini
                                   .Include(o => o.DettagliOrdine)
                                   .FirstOrDefaultAsync(o => o.IdOrdine == id);
            if (Ordine != null)
            {
                _context.DettagliOrdine.Remove(Ordine.DettagliOrdine);
                _context.Ordini.Remove(Ordine);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}