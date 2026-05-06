using erp_corriere.Data;
using erp_corriere.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace erp_corriere.Pages
{
    public class CreaModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreaModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Spedizione Spedizione { get; set; } = new();

        public void OnGet()
        {
            Spedizione.DataSpedizione = DateTime.Today;
            Spedizione.Stato = "In preparazione";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Spedizioni.Add(Spedizione);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
