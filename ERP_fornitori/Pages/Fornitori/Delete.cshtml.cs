using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FornitoriERP.Models;
using FornitoriERP.Services;

namespace FornitoriERP.Pages.Fornitori
{
    public class DeleteModel : PageModel
    {
        private readonly IFornitoreService _service;
        private readonly ILogger<DeleteModel> _logger;

        public Fornitore Fornitore { get; set; } = new();

        public DeleteModel(IFornitoreService service, ILogger<DeleteModel> logger)
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Fornitore con ID {Id} non trovato", id);
                    return NotFound();
                }

                _logger.LogInformation("Fornitore eliminato (ID: {Id})", id);
                TempData["SuccessMessage"] = "Fornitore eliminato con successo!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nell'eliminazione del fornitore");
                ModelState.AddModelError("", "Errore nell'eliminazione del fornitore. Riprovare.");
                return Page();
            }
        }
    }
}
