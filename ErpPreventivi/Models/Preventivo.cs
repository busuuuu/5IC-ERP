using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpPreventivi.Models;

public enum StatoPreventivo
{
    Bozza,
    Inviato,
    Accettato,
    Rifiutato
}

public class Preventivo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Il numero preventivo è obbligatorio")]
    [Display(Name = "N° Preventivo")]
    public string Numero { get; set; } = string.Empty;

    [Required(ErrorMessage = "Il cliente è obbligatorio")]
    [Display(Name = "Cliente")]
    public string Cliente { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email non valida")]
    [Display(Name = "Email Cliente")]
    public string? EmailCliente { get; set; }

    [Display(Name = "Data Creazione")]
    [DataType(DataType.Date)]
    public DateTime DataCreazione { get; set; } = DateTime.Today;

    [Display(Name = "Data Scadenza")]
    [DataType(DataType.Date)]
    public DateTime? DataScadenza { get; set; }

    [Display(Name = "Note")]
    public string? Note { get; set; }

    [Display(Name = "Stato")]
    public StatoPreventivo Stato { get; set; } = StatoPreventivo.Bozza;

    // Relazione con le righe
    public List<PreventivoRiga> Righe { get; set; } = new();

    // Proprietà calcolata: totale imponibile
    [NotMapped]
    [Display(Name = "Imponibile")]
    public decimal Imponibile => Righe.Sum(r => r.Totale);

    // Proprietà calcolata: IVA (22%)
    [NotMapped]
    [Display(Name = "IVA (22%)")]
    public decimal Iva => Math.Round(Imponibile * 0.22m, 2);

    // Proprietà calcolata: totale con IVA
    [NotMapped]
    [Display(Name = "Totale")]
    public decimal TotaleConIva => Imponibile + Iva;
}
