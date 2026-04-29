using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using _5IC_ERP.Models;

namespace _5IC_ERP.Pages.BOM
{
    public class VerificaModel : PageModel
    {
        [BindProperty]
        public int QuantitaDaProdurre { get; set; } = 1;

        public string Risultato { get; set; } = "";

        public bool ProduzioneOk { get; set; }

        public List<string> Dettagli { get; set; } = new();

        public void OnGet()
        {
        }

        public void OnPost()
        {
            var lista = new List<DistintaBaseItem>()
            {
                new DistintaBaseItem { Componente="Case", Quantita=1, Giacenza=15 },
                new DistintaBaseItem { Componente="Scheda Madre", Quantita=1, Giacenza=9 },
                new DistintaBaseItem { Componente="Antenna", Quantita=3, Giacenza=50 }
            };

            bool ok = true;

            foreach (var item in lista)
            {
                int richiesta = item.Quantita * QuantitaDaProdurre;

                if (item.Giacenza >= richiesta)
                    Dettagli.Add(item.Componente + " disponibile");
                else
                {
                    ok = false;
                    int mancanti = richiesta - item.Giacenza;
                    Dettagli.Add(item.Componente + " mancanti: " + mancanti);
                }
            }

            ProduzioneOk = ok;
            Risultato = ok ? "Produzione possibile" : "Produzione NON possibile";
        }
    }
}