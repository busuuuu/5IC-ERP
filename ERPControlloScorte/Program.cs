using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(o => o.RoutePrefix = string.Empty);
app.UseCors();

// === Inizializzazione DB ===
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// ============================================================
// ENDPOINTS - PRODOTTI
// ============================================================

// Lista prodotti (con filtro opzionale per sotto-scorta)
app.MapGet("/api/prodotti", async (AppDbContext db, bool? sottoScorta) =>
{
    var query = db.Prodotti.AsQueryable();
    if (sottoScorta == true)
        query = query.Where(p => p.QuantitaDisponibile < p.ScortaMinima);

    var prodotti = await query.OrderBy(p => p.Nome).ToListAsync();
    return Results.Ok(prodotti);
});

// Singolo prodotto
app.MapGet("/api/prodotti/{id}", async (AppDbContext db, int id) =>
{
    var p = await db.Prodotti.FindAsync(id);
    return p is null ? Results.NotFound("Prodotto non trovato") : Results.Ok(p);
});

// Crea prodotto
app.MapPost("/api/prodotti", async (AppDbContext db, Prodotto prodotto) =>
{
    prodotto.DataCreazione = DateTime.UtcNow;
    db.Prodotti.Add(prodotto);
    await db.SaveChangesAsync();
    return Results.Created($"/api/prodotti/{prodotto.Id}", prodotto);
});

// Aggiorna prodotto
app.MapPut("/api/prodotti/{id}", async (AppDbContext db, int id, Prodotto aggiornato) =>
{
    var p = await db.Prodotti.FindAsync(id);
    if (p is null) return Results.NotFound("Prodotto non trovato");

    p.Nome = aggiornato.Nome;
    p.Descrizione = aggiornato.Descrizione;
    p.Categoria = aggiornato.Categoria;
    p.PrezzoUnitario = aggiornato.PrezzoUnitario;
    p.ScortaMinima = aggiornato.ScortaMinima;
    p.PuntoRiordino = aggiornato.PuntoRiordino;
    p.UnitaMisura = aggiornato.UnitaMisura;
    await db.SaveChangesAsync();
    return Results.Ok(p);
});

// Elimina prodotto
app.MapDelete("/api/prodotti/{id}", async (AppDbContext db, int id) =>
{
    var p = await db.Prodotti.FindAsync(id);
    if (p is null) return Results.NotFound("Prodotto non trovato");
    db.Prodotti.Remove(p);
    await db.SaveChangesAsync();
    return Results.Ok("Prodotto eliminato");
});

// ============================================================
// ENDPOINTS - MOVIMENTI (Carico / Scarico)
// ============================================================

// Lista movimenti (filtro opzionale per prodotto)
app.MapGet("/api/movimenti", async (AppDbContext db, int? prodottoId) =>
{
    var query = db.Movimenti.Include(m => m.Prodotto).AsQueryable();
    if (prodottoId.HasValue)
        query = query.Where(m => m.ProdottoId == prodottoId);

    return Results.Ok(await query.OrderByDescending(m => m.Data).ToListAsync());
});

// Registra movimento (carico o scarico)
app.MapPost("/api/movimenti", async (AppDbContext db, Movimento mov) =>
{
    var prodotto = await db.Prodotti.FindAsync(mov.ProdottoId);
    if (prodotto is null)
        return Results.NotFound("Prodotto non trovato");

    // Aggiorna quantità
    if (mov.Tipo == "Carico")
    {
        prodotto.QuantitaDisponibile += mov.Quantita;
    }
    else if (mov.Tipo == "Scarico")
    {
        if (prodotto.QuantitaDisponibile < mov.Quantita)
            return Results.BadRequest($"Quantità insufficiente. Disponibile: {prodotto.QuantitaDisponibile}");
        prodotto.QuantitaDisponibile -= mov.Quantita;
    }
    else
    {
        return Results.BadRequest("Tipo deve essere 'Carico' o 'Scarico'");
    }

    mov.Data = DateTime.UtcNow;
    db.Movimenti.Add(mov);
    await db.SaveChangesAsync();

    // Genera alert se sotto scorta minima
    if (prodotto.QuantitaDisponibile < prodotto.ScortaMinima)
    {
        var alertEsistente = await db.Alert
            .AnyAsync(a => a.ProdottoId == prodotto.Id && !a.Risolto);

        if (!alertEsistente)
        {
            db.Alert.Add(new Alert
            {
                ProdottoId = prodotto.Id,
                Messaggio = $"Scorta bassa: {prodotto.Nome} ha {prodotto.QuantitaDisponibile} {prodotto.UnitaMisura} (minimo: {prodotto.ScortaMinima})",
                Data = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }
    }

    return Results.Created($"/api/movimenti", mov);
});

// ============================================================
// ENDPOINTS - ALERT SCORTE
// ============================================================

// Alert attivi
app.MapGet("/api/alert", async (AppDbContext db) =>
{
    var alert = await db.Alert
        .Include(a => a.Prodotto)
        .Where(a => !a.Risolto)
        .OrderByDescending(a => a.Data)
        .ToListAsync();
    return Results.Ok(alert);
});

// Risolvi alert
app.MapPut("/api/alert/{id}/risolvi", async (AppDbContext db, int id) =>
{
    var a = await db.Alert.FindAsync(id);
    if (a is null) return Results.NotFound("Alert non trovato");
    a.Risolto = true;
    a.DataRisoluzione = DateTime.UtcNow;
    await db.SaveChangesAsync();
    return Results.Ok("Alert risolto");
});

// Verifica manuale tutte le scorte
app.MapPost("/api/alert/verifica", async (AppDbContext db) =>
{
    var prodotti = await db.Prodotti
        .Where(p => p.QuantitaDisponibile < p.ScortaMinima)
        .ToListAsync();

    int nuoviAlert = 0;
    foreach (var p in prodotti)
    {
        var esiste = await db.Alert.AnyAsync(a => a.ProdottoId == p.Id && !a.Risolto);
        if (!esiste)
        {
            db.Alert.Add(new Alert
            {
                ProdottoId = p.Id,
                Messaggio = p.QuantitaDisponibile <= 0
                    ? $"ESAURITO: {p.Nome}"
                    : $"Scorta bassa: {p.Nome} ({p.QuantitaDisponibile}/{p.ScortaMinima} {p.UnitaMisura})",
                Data = DateTime.UtcNow
            });
            nuoviAlert++;
        }
    }
    await db.SaveChangesAsync();
    return Results.Ok($"Verifica completata: {nuoviAlert} nuovi alert generati");
});

// ============================================================
// ENDPOINT - DASHBOARD
// ============================================================

app.MapGet("/api/dashboard", async (AppDbContext db) =>
{
    var prodotti = await db.Prodotti.ToListAsync();
    return Results.Ok(new
    {
        TotaleProdotti = prodotti.Count,
        SottoScortaMinima = prodotti.Count(p => p.QuantitaDisponibile < p.ScortaMinima && p.QuantitaDisponibile > 0),
        Esauriti = prodotti.Count(p => p.QuantitaDisponibile <= 0),
        ValoreMagazzino = prodotti.Sum(p => p.QuantitaDisponibile * p.PrezzoUnitario),
        AlertAttivi = await db.Alert.CountAsync(a => !a.Risolto),
        ProdottiCritici = prodotti
            .Where(p => p.QuantitaDisponibile < p.ScortaMinima)
            .OrderBy(p => p.QuantitaDisponibile)
            .Select(p => new { p.Codice, p.Nome, p.QuantitaDisponibile, p.ScortaMinima })
            .ToList()
    });
});

app.Run();

// ============================================================
// MODELLI (Entity Framework)
// ============================================================

public class Prodotto
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Codice { get; set; } = "";

    [Required, StringLength(200)]
    public string Nome { get; set; } = "";

    [StringLength(500)]
    public string? Descrizione { get; set; }

    [StringLength(50)]
    public string Categoria { get; set; } = "";

    [StringLength(20)]
    public string UnitaMisura { get; set; } = "pz";

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrezzoUnitario { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal QuantitaDisponibile { get; set; }

    /// <summary>Soglia sotto la quale scatta l'allarme</summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal ScortaMinima { get; set; }

    /// <summary>Quantità ideale da riordinare</summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal PuntoRiordino { get; set; }

    public DateTime DataCreazione { get; set; } = DateTime.UtcNow;
}

public class Movimento
{
    public int Id { get; set; }
    public int ProdottoId { get; set; }

    [Required]
    public string Tipo { get; set; } = ""; // "Carico" o "Scarico"

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantita { get; set; }

    [StringLength(500)]
    public string? Causale { get; set; }

    [StringLength(100)]
    public string? Operatore { get; set; }

    public DateTime Data { get; set; } = DateTime.UtcNow;

    [ForeignKey("ProdottoId")]
    public Prodotto? Prodotto { get; set; }
}

public class Alert
{
    public int Id { get; set; }
    public int ProdottoId { get; set; }
    public string Messaggio { get; set; } = "";
    public DateTime Data { get; set; } = DateTime.UtcNow;
    public bool Risolto { get; set; } = false;
    public DateTime? DataRisoluzione { get; set; }

    [ForeignKey("ProdottoId")]
    public Prodotto? Prodotto { get; set; }
}

// ============================================================
// DATABASE CONTEXT
// ============================================================

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

    public DbSet<Prodotto> Prodotti => Set<Prodotto>();
    public DbSet<Movimento> Movimenti => Set<Movimento>();
    public DbSet<Alert> Alert => Set<Alert>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Prodotto>().HasIndex(p => p.Codice).IsUnique();

        // Dati di esempio
        mb.Entity<Prodotto>().HasData(
            new Prodotto { Id = 1, Codice = "MAT-001", Nome = "Bulloni M8x30", Categoria = "Ferramenta", UnitaMisura = "pz", PrezzoUnitario = 0.15m, QuantitaDisponibile = 5000, ScortaMinima = 500, PuntoRiordino = 2000, DataCreazione = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Prodotto { Id = 2, Codice = "MAT-002", Nome = "Lamiera acciaio 2mm", Categoria = "Lamiere", UnitaMisura = "pz", PrezzoUnitario = 85m, QuantitaDisponibile = 12, ScortaMinima = 5, PuntoRiordino = 20, DataCreazione = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Prodotto { Id = 3, Codice = "MAT-003", Nome = "Olio lubrificante", Categoria = "Lubrificanti", UnitaMisura = "lt", PrezzoUnitario = 4.50m, QuantitaDisponibile = 80, ScortaMinima = 40, PuntoRiordino = 100, DataCreazione = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Prodotto { Id = 4, Codice = "MAT-004", Nome = "Guarnizioni gomma", Categoria = "Guarnizioni", UnitaMisura = "pz", PrezzoUnitario = 0.80m, QuantitaDisponibile = 50, ScortaMinima = 100, PuntoRiordino = 500, DataCreazione = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Prodotto { Id = 5, Codice = "MAT-005", Nome = "Vernice RAL 7035", Categoria = "Vernici", UnitaMisura = "lt", PrezzoUnitario = 12m, QuantitaDisponibile = 0, ScortaMinima = 10, PuntoRiordino = 30, DataCreazione = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
