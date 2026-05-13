using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FornitoriERP.Models;
using FornitoriERP.Services;

namespace FornitoriERP.Pages.Fornitori
{
    public class CreateModel : PageModel
    {
        private readonly IFornitoreService _service;
        private readonly ILogger<CreateModel> _logger;

        [BindProperty]
        public Fornitore Fornitore { get; set; } = new();

        public CreateModel(IFornitoreService service, ILogger<CreateModel> logger)
        {
            _service = service;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var created = await _service.CreateAsync(Fornitore);
                _logger.LogInformation("Fornitore creato: {Nome} (ID: {Id})", created.NomeFornitore, created.IdFornitore);
                TempData["SuccessMessage"] = $"Fornitore '{created.NomeFornitore}' creato con successo!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nella creazione del fornitore");
                ModelState.AddModelError("", "Errore nella creazione del fornitore. Riprovare.");
                return Page();
            }
        }
    }
}
