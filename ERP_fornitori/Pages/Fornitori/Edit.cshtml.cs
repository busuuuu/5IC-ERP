using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FornitoriERP.Models;
using FornitoriERP.Services;

namespace FornitoriERP.Pages.Fornitori
{
    public class EditModel : PageModel
    {
        private readonly IFornitoreService _service;
        private readonly ILogger<EditModel> _logger;

        [BindProperty]
        public Fornitore Fornitore { get; set; } = new();

        public EditModel(IFornitoreService service, ILogger<EditModel> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var fornitore = await _service.GetByIdAsync(id);
            if (fornitore == null)
            {
                _logger.LogWarning("Fornitore con ID {Id} non trovato", id);
                return NotFound();
            }

            Fornitore = fornitore;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var updated = await _service.UpdateAsync(Fornitore.IdFornitore, Fornitore);
                if (updated == null)
                {
                    _logger.LogWarning("Fornitore con ID {Id} non trovato", Fornitore.IdFornitore);
                    return NotFound();
                }

                _logger.LogInformation("Fornitore aggiornato: {Nome} (ID: {Id})", updated.NomeFornitore, updated.IdFornitore);
                TempData["SuccessMessage"] = $"Fornitore '{updated.NomeFornitore}' aggiornato con successo!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nell'aggiornamento del fornitore");
                ModelState.AddModelError("", "Errore nell'aggiornamento del fornitore. Riprovare.");
                return Page();
            }
        }
    }
}
