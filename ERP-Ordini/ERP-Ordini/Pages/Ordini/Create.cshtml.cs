using ERP_Ordini.Data;
using ERP_Ordini.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ERP_Ordini.Pages.Ordini
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        public CreateModel(AppDbContext context) => _context = context;

        [BindProperty]
        public Ordine Ordine { get; set; }

        public void OnGet()
        {
            // Inizializza Ordine e DettagliOrdine per il binding nel form
            Ordine = new Ordine
            {
                DettagliOrdine = new DettagliOrdine
                {
                    DataOrdine = System.DateTime.Today
                }
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Salva prima i dettagli ordine
            _context.DettagliOrdine.Add(Ordine.DettagliOrdine);
            await _context.SaveChangesAsync();

            // Collega i dettagli all'ordine
            Ordine.IdDettagliOrdine = Ordine.DettagliOrdine.Id;

            _context.Ordini.Add(Ordine);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}