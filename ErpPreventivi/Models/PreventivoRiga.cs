using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpPreventivi.Models;

public class PreventivoRiga
{
    public int Id { get; set; }

    // Chiave esterna verso Preventivo
    public int PreventivoId { get; set; }
    public Preventivo? Preventivo { get; set; }

    [Required(ErrorMessage = "La descrizione è obbligatoria")]
    [Display(Name = "Descrizione")]
    public string Descrizione { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "La quantità deve essere maggiore di 0")]
    [Display(Name = "Quantità")]
    public decimal Quantita { get; set; } = 1;

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Il prezzo non può essere negativo")]
    [Display(Name = "Prezzo Unitario (€)")]
    public decimal PrezzoUnitario { get; set; }

    // Totale riga calcolato
    [NotMapped]
    [Display(Name = "Totale Riga")]
    public decimal Totale => Quantita * PrezzoUnitario;
}
