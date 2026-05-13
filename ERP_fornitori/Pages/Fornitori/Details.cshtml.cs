using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FornitoriERP.Models;
using FornitoriERP.Services;

namespace FornitoriERP.Pages.Fornitori
{
    public class DetailsModel : PageModel
    {
        private readonly IFornitoreService _service;
        private readonly ILogger<DetailsModel> _logger;

        public Fornitore Fornitore { get; set; } = new();

        public DetailsModel(IFornitoreService service, ILogger<DetailsModel> logger)
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
    }
}
