using Microsoft.AspNetCore.Mvc.RazorPages;
using FornitoriERP.Models;
using FornitoriERP.Services;

namespace FornitoriERP.Pages.Fornitori
{
    public class IndexModel : PageModel
    {
        private readonly IFornitoreService _service;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public IEnumerable<Fornitore> Fornitori { get; set; } = new List<Fornitore>();

        public IndexModel(IFornitoreService service, ILogger<IndexModel> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            try
            {
                Fornitori = await _service.GetAllAsync(SearchTerm);
                _logger.LogInformation("Caricati {Count} fornitori", Fornitori.Count());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel caricamento dei fornitori");
                Fornitori = new List<Fornitore>();
            }
        }
    }
}
