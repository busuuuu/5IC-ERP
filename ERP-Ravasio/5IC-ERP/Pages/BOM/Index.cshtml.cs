using _5IC_ERP.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace _5IC_ERP.Pages.BOM
{
    public class IndexModel : PageModel
    {
        public List<DistintaBaseItem> Riga { get; set; } = new();

        public void OnGet()
        {
            Riga = new List<DistintaBaseItem>
            {
                new DistintaBaseItem
                {
                    Id = 1,
                    Componente = "Case",
                    Quantita = 1,
                    Giacenza = 15
                },

                new DistintaBaseItem
                {
                    Id = 2,
                    Componente = "Scheda Madre",
                    Quantita = 1,
                    Giacenza = 9
                },

                new DistintaBaseItem
                {
                    Id = 3,
                    Componente = "Antenna",
                    Quantita = 3,
                    Giacenza = 50
                }
            };
        }
    }
}