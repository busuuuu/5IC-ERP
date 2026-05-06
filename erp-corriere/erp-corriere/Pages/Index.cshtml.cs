using erp_corriere.Data;
using erp_corriere.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace erp_corriere.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Spedizione> Spedizioni { get; set; } = new();

        public async Task OnGetAsync()
        {
            Spedizioni = await _context.Spedizioni
                .OrderByDescending(s => s.Id)
                .ToListAsync();
        }
    }
    
}
