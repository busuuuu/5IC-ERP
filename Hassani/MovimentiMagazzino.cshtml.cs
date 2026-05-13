using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TechLogixERP.Pages
{
    public class MovimentoMagazzinoModel
    {
        public string Id { get; set; } = "";
        public string Data { get; set; } = "";
        public string Tipo { get; set; } = "";       // CARICO, SCARICO, RETTIFICA, RESO
        public string Articolo { get; set; } = "";
        public int Quantita { get; set; }
        public string Causale { get; set; } = "";
        public string Riferimento { get; set; } = "";
        public string Operatore { get; set; } = "";
    }

    public class AlertSottoscorta
    {
        public string Articolo { get; set; } = "";
        public int Scorta { get; set; }
        public int Minimo { get; set; }
    }

    public class ChartDay
    {
        public string Giorno { get; set; } = "";
        public int Carichi { get; set; }
        public int Scarichi { get; set; }
    }

    public class TopArticolo
    {
        public string Nome { get; set; } = "";
        public int Movimenti { get; set; }
        public int Percentuale { get; set; }
    }

    public class MovimentiMagazzinoModel : PageModel
    {
        // ── Dati esposti alla View ──────────────────────────────────────────
        public List<MovimentoMagazzinoModel> Movimenti { get; set; } = new();
        public List<AlertSottoscorta> AlertSottoscorte { get; set; } = new();
        public List<ChartDay> ChartData { get; set; } = new();
        public List<TopArticolo> TopArticoli { get; set; } = new();

        // KPI
        public int MovimentiOggi { get; set; }
        public int CarichiMese { get; set; }
        public int SarichiMese { get; set; }
        public int NumSottoscorta { get; set; }

        // ── Bind del form Nuovo Movimento ──────────────────────────────────
        [BindProperty] public string NuovoTipo { get; set; } = "CARICO";
        [BindProperty] public string NuovoArticolo { get; set; } = "";
        [BindProperty] public int NuovoQty { get; set; }
        [BindProperty] public string NuovaData { get; set; } = "";
        [BindProperty] public string NuovaCausale { get; set; } = "";
        [BindProperty] public string NuovoRiferimento { get; set; } = "";
        [BindProperty] public string NuoveNote { get; set; } = "";

        // Messaggio di feedback dopo il POST
        public string? ToastMessage { get; set; }
        public bool ToastIsError { get; set; }

        // ── GET ─────────────────────────────────────────────────────────────
        public void OnGet()
        {
            CaricaDati();
        }

        // ── POST (Nuovo Movimento) ──────────────────────────────────────────
        public IActionResult OnPost()
        {
            CaricaDati();

            if (string.IsNullOrWhiteSpace(NuovoArticolo) ||
                NuovoQty <= 0 ||
                string.IsNullOrWhiteSpace(NuovaData) ||
                string.IsNullOrWhiteSpace(NuovaCausale))
            {
                ToastMessage = "⚠️ Compila tutti i campi obbligatori";
                ToastIsError = true;
                return Page();
            }

            int qtaEffettiva = NuovoTipo == "SCARICO" ? -Math.Abs(NuovoQty) : Math.Abs(NuovoQty);

            var nuovo = new MovimentoMagazzinoModel
            {
                Id         = $"MOV-{Movimenti.Count + 1:D4}",
                Data       = NuovaData,
                Tipo       = NuovoTipo,
                Articolo   = NuovoArticolo,
                Quantita   = qtaEffettiva,
                Causale    = NuovaCausale,
                Riferimento = string.IsNullOrWhiteSpace(NuovoRiferimento) ? "—" : NuovoRiferimento,
                Operatore  = "M.Hassani"
            };

            // In un progetto reale qui salveresti su DB:
            //   _db.Movimenti.Add(nuovo);
            //   await _db.SaveChangesAsync();

            Movimenti.Insert(0, nuovo);
            MovimentiOggi++;

            ToastMessage = "✅ Movimento registrato con successo!";
            ToastIsError = false;

            return Page();
        }

        // ── Dati di esempio (da sostituire con accesso al DB) ───────────────
        private void CaricaDati()
        {
            Movimenti = new List<MovimentoMagazzinoModel>
            {
                new() { Id="MOV-0012", Data="2026-04-22", Tipo="CARICO",    Articolo="PCB-MAIN-01 — Scheda Madre",     Quantita=+50, Causale="Acquisto fornitore",  Riferimento="ODA-2026-041", Operatore="M.Hassani" },
                new() { Id="MOV-0011", Data="2026-04-22", Tipo="SCARICO",   Articolo="ANT-EXT-03 — Antenna Esterna",   Quantita=-15, Causale="Assemblaggio prod.",  Riferimento="PRD-2026-009", Operatore="M.Hassani" },
                new() { Id="MOV-0010", Data="2026-04-21", Tipo="SCARICO",   Articolo="GW-DOM-01 — Gateway Domotico",   Quantita=-8,  Causale="Vendita B2B",         Riferimento="ODV-2026-022", Operatore="Boffelli"  },
                new() { Id="MOV-0009", Data="2026-04-21", Tipo="CARICO",    Articolo="CASE-STD-02 — Case Standard",    Quantita=+30, Causale="Acquisto fornitore",  Riferimento="ODA-2026-040", Operatore="M.Hassani" },
                new() { Id="MOV-0008", Data="2026-04-20", Tipo="RETTIFICA", Articolo="SEN-TEMP-01 — Sensore Temp.",    Quantita=+2,  Causale="Inventario fisico",   Riferimento="INV-2026-004", Operatore="Galizzi"   },
                new() { Id="MOV-0007", Data="2026-04-19", Tipo="SCARICO",   Articolo="PSU-5V-01 — Alimentatore 5V",   Quantita=-20, Causale="Assemblaggio prod.",  Riferimento="PRD-2026-008", Operatore="M.Hassani" },
                new() { Id="MOV-0006", Data="2026-04-18", Tipo="RESO",      Articolo="SEN-HUM-02 — Sensore Umidità",  Quantita=+5,  Causale="Reso cliente",        Riferimento="RES-2026-003", Operatore="Boffelli"  },
                new() { Id="MOV-0005", Data="2026-04-17", Tipo="CARICO",    Articolo="ANT-EXT-03 — Antenna Esterna",  Quantita=+100,Causale="Acquisto fornitore",  Riferimento="ODA-2026-038", Operatore="M.Hassani" },
                new() { Id="MOV-0004", Data="2026-04-16", Tipo="SCARICO",   Articolo="CAB-USB-01 — Cavo USB-C",       Quantita=-40, Causale="Vendita B2C",         Riferimento="ODV-2026-019", Operatore="Benigni"   },
                new() { Id="MOV-0003", Data="2026-04-15", Tipo="CARICO",    Articolo="SEN-TEMP-01 — Sensore Temp.",   Quantita=+75, Causale="Acquisto fornitore",  Riferimento="ODA-2026-037", Operatore="M.Hassani" },
                new() { Id="MOV-0002", Data="2026-04-14", Tipo="RETTIFICA", Articolo="CASE-STD-02 — Case Standard",   Quantita=-3,  Causale="Danno magazzino",     Riferimento="INV-2026-003", Operatore="Galizzi"   },
                new() { Id="MOV-0001", Data="2026-04-08", Tipo="CARICO",    Articolo="GW-DOM-01 — Gateway Domotico",  Quantita=+20, Causale="Acquisto fornitore",  Riferimento="ODA-2026-035", Operatore="M.Hassani" },
            };

            AlertSottoscorte = new List<AlertSottoscorta>
            {
                new() { Articolo="PSU-5V-01 — Alimentatore 5V",  Scorta=4,  Minimo=10 },
                new() { Articolo="CAB-USB-01 — Cavo USB-C",      Scorta=7,  Minimo=20 },
                new() { Articolo="SEN-HUM-02 — Sensore Umidità", Scorta=12, Minimo=15 },
            };

            ChartData = new List<ChartDay>
            {
                new() { Giorno="Mer", Carichi=8,  Scarichi=5  },
                new() { Giorno="Gio", Carichi=3,  Scarichi=12 },
                new() { Giorno="Ven", Carichi=15, Scarichi=6  },
                new() { Giorno="Sab", Carichi=0,  Scarichi=0  },
                new() { Giorno="Dom", Carichi=0,  Scarichi=0  },
                new() { Giorno="Lun", Carichi=10, Scarichi=4  },
                new() { Giorno="Mar", Carichi=50, Scarichi=15 },
            };

            TopArticoli = new List<TopArticolo>
            {
                new() { Nome="Scheda Madre",      Movimenti=8, Percentuale=80 },
                new() { Nome="Antenna Esterna",   Movimenti=6, Percentuale=60 },
                new() { Nome="Gateway Domotico",  Movimenti=5, Percentuale=50 },
                new() { Nome="Sensore Temp.",      Movimenti=4, Percentuale=40 },
            };

            // KPI calcolati
            string oggi = DateTime.Today.ToString("yyyy-MM-dd");
            MovimentiOggi  = Movimenti.Count(m => m.Data == oggi);
            CarichiMese    = Movimenti.Where(m => m.Tipo == "CARICO").Sum(m => m.Quantita);
            SarichiMese    = Math.Abs(Movimenti.Where(m => m.Tipo == "SCARICO").Sum(m => m.Quantita));
            NumSottoscorta = AlertSottoscorte.Count;
        }
    }
}
