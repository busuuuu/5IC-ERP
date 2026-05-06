using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErpPreventivi.Data;
using ErpPreventivi.Models;

namespace ErpPreventivi.Controllers;

public class PreventiviController : Controller
{
    private readonly AppDbContext _db;

    public PreventiviController(AppDbContext db)
    {
        _db = db;
    }

    // ───────────────────────────────────────────────
    // GET /Preventivi  →  Lista di tutti i preventivi
    // ───────────────────────────────────────────────
    public async Task<IActionResult> Index(string? cerca, StatoPreventivo? stato)
    {
        var query = _db.Preventivi.Include(p => p.Righe).AsQueryable();

        // Filtro ricerca per cliente o numero
        if (!string.IsNullOrWhiteSpace(cerca))
            query = query.Where(p => p.Cliente.Contains(cerca) || p.Numero.Contains(cerca));

        // Filtro per stato
        if (stato.HasValue)
            query = query.Where(p => p.Stato == stato.Value);

        ViewBag.Cerca = cerca;
        ViewBag.StatoFiltro = stato;

        var lista = await query.OrderByDescending(p => p.DataCreazione).ToListAsync();
        return View(lista);
    }

    // ───────────────────────────────────────────────
    // GET /Preventivi/Details/5  →  Dettaglio
    // ───────────────────────────────────────────────
    public async Task<IActionResult> Details(int id)
    {
        var preventivo = await _db.Preventivi
            .Include(p => p.Righe)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (preventivo == null) return NotFound();
        return View(preventivo);
    }

    // ───────────────────────────────────────────────
    // GET /Preventivi/Create  →  Form creazione
    // ───────────────────────────────────────────────
    public IActionResult Create()
    {
        // Genera automaticamente il numero preventivo
        var anno = DateTime.Today.Year;
        var count = _db.Preventivi.Count(p => p.DataCreazione.Year == anno) + 1;
        var preventivo = new Preventivo
        {
            Numero = $"PREV-{anno}-{count:D4}",
            DataCreazione = DateTime.Today,
            DataScadenza = DateTime.Today.AddDays(30)
        };
        return View(preventivo);
    }

    // ───────────────────────────────────────────────
    // POST /Preventivi/Create  →  Salvataggio
    // ───────────────────────────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Preventivo preventivo,
        List<string> rigaDescrizione,
        List<decimal> rigaQuantita,
        List<decimal> rigaPrezzo)
    {
        // Costruisce le righe dal form
        preventivo.Righe = CreaRighe(rigaDescrizione, rigaQuantita, rigaPrezzo);

        if (!ModelState.IsValid)
            return View(preventivo);

        _db.Preventivi.Add(preventivo);
        await _db.SaveChangesAsync();

        TempData["Successo"] = $"Preventivo {preventivo.Numero} creato con successo!";
        return RedirectToAction(nameof(Index));
    }

    // ───────────────────────────────────────────────
    // GET /Preventivi/Edit/5  →  Form modifica
    // ───────────────────────────────────────────────
    public async Task<IActionResult> Edit(int id)
    {
        var preventivo = await _db.Preventivi
            .Include(p => p.Righe)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (preventivo == null) return NotFound();
        return View(preventivo);
    }

    // ───────────────────────────────────────────────
    // POST /Preventivi/Edit/5  →  Salvataggio modifica
    // ───────────────────────────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Preventivo preventivo,
        List<string> rigaDescrizione,
        List<decimal> rigaQuantita,
        List<decimal> rigaPrezzo)
    {
        if (id != preventivo.Id) return BadRequest();

        if (!ModelState.IsValid)
        {
            preventivo.Righe = CreaRighe(rigaDescrizione, rigaQuantita, rigaPrezzo);
            return View(preventivo);
        }

        // Rimuove le vecchie righe e le sostituisce
        var righeVecchie = _db.PreventivoRighe.Where(r => r.PreventivoId == id);
        _db.PreventivoRighe.RemoveRange(righeVecchie);

        preventivo.Righe = CreaRighe(rigaDescrizione, rigaQuantita, rigaPrezzo);

        _db.Update(preventivo);
        await _db.SaveChangesAsync();

        TempData["Successo"] = $"Preventivo {preventivo.Numero} aggiornato!";
        return RedirectToAction(nameof(Index));
    }

    // ───────────────────────────────────────────────
    // POST /Preventivi/Delete/5  →  Eliminazione
    // ───────────────────────────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var preventivo = await _db.Preventivi.FindAsync(id);
        if (preventivo == null) return NotFound();

        _db.Preventivi.Remove(preventivo);
        await _db.SaveChangesAsync();

        TempData["Successo"] = $"Preventivo {preventivo.Numero} eliminato.";
        return RedirectToAction(nameof(Index));
    }

    // ───────────────────────────────────────────────
    // POST /Preventivi/CambiaStato  →  Cambio stato rapido
    // ───────────────────────────────────────────────
    [HttpPost]
    public async Task<IActionResult> CambiaStato(int id, StatoPreventivo nuovoStato)
    {
        var preventivo = await _db.Preventivi.FindAsync(id);
        if (preventivo == null) return NotFound();

        preventivo.Stato = nuovoStato;
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id });
    }

    // ─── Helper privato ───────────────────────────
    private static List<PreventivoRiga> CreaRighe(
        List<string> descrizioni, List<decimal> quantita, List<decimal> prezzi)
    {
        var righe = new List<PreventivoRiga>();
        for (int i = 0; i < descrizioni.Count; i++)
        {
            if (!string.IsNullOrWhiteSpace(descrizioni[i]))
            {
                righe.Add(new PreventivoRiga
                {
                    Descrizione = descrizioni[i],
                    Quantita = i < quantita.Count ? quantita[i] : 1,
                    PrezzoUnitario = i < prezzi.Count ? prezzi[i] : 0
                });
            }
        }
        return righe;
    }
}
