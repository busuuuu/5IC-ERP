using ERP_Ordini.Data;
using ERP_Ordini.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ERP_Ordini.Pages.Ordini
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        public EditModel(AppDbContext context) => _context = context;

        [BindProperty]
        public Ordine Ordine { get; set; }

        // Carica l'ordine e i dettagli per il form
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Ordine = await _context.Ordini
                                   .Include(o => o.DettagliOrdine)
                                   .FirstOrDefaultAsync(o => o.IdOrdine == id);
            if (Ordine == null) return NotFound();

            return Page();
        }

        // Salva modifiche ordine e dettagli
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Modifica Ordine
            _context.Attach(Ordine).State = EntityState.Modified;

            // Modifica DettagliOrdine
            if (Ordine.DettagliOrdine != null)
            {
                _context.Attach(Ordine.DettagliOrdine).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdineExists(Ordine.IdOrdine))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToPage("Index");
        }

        private bool OrdineExists(int id)
        {
            return _context.Ordini.AnyAsync(e => e.IdOrdine == id).Result;
        }
    }
}