using erp_corriere.Data;
using erp_corriere.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace erp_corriere.Pages
{
    public class EliminaModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EliminaModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Spedizione Spedizione { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var spedizione = await _context.Spedizioni.FindAsync(id);

            if (spedizione == null)
                return NotFound();

            Spedizione = spedizione;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var spedizione = await _context.Spedizioni.FindAsync(Spedizione.Id);

            if (spedizione != null)
            {
                _context.Spedizioni.Remove(spedizione);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}
