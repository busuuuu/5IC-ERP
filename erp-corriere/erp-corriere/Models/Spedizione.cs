using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace erp_corriere.Models
{
    public class Spedizione
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Il numero spedizione è obbligatorio")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Il numero spedizione deve contenere solo numeri")]
        [Display(Name = "N° Spedizione")]
        public string Numero { get; set; } = "";

        [Required(ErrorMessage = "Il destinatario è obbligatorio")]
        [Display(Name = "Destinatario")]
        public string Destinatario { get; set; } = "";

        [Display(Name = "Indirizzo")]
        public string Indirizzo { get; set; } = "";

        [Display(Name = "Città")]
        public string Citta { get; set; } = "";

        [Display(Name = "CAP")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Il CAP deve essere di 5 cifre")]
        public string? Cap { get; set; }

        [Required(ErrorMessage = "Seleziona un corriere")]
        [Display(Name = "Corriere")]
        public string Corriere { get; set; } = "";

        [Display(Name = "Stato")]
        public string Stato { get; set; } = "";

        [Display(Name = "Data Spedizione")]
        public DateTime DataSpedizione { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Arrivo Previsto")]
        public DateTime? DataArrivoPrevista { get; set; }

        [Display(Name = "Peso (kg)")]
        [Range(0.01, 9999.99, ErrorMessage = "Il peso deve essere tra 0.01 e 9999.99 kg")]
        [Column(TypeName = "decimal(8,2)")]
        public decimal? PesoKg { get; set; }

        [Display(Name = "Dimensioni (LxAxP cm)")]
        public string? Dimensioni { get; set; }

        [Display(Name = "Costo Netto (€)")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? CostoNetto { get; set; }

        [Display(Name = "IVA (%)")]
        public int? AliquotaIva { get; set; } = 22;

        [Display(Name = "Note")]
        public string? Note { get; set; }

        // Campi calcolati (non mappati su DB)
        [NotMapped]
        public decimal CostoIva => CostoNetto.HasValue && AliquotaIva.HasValue
            ? Math.Round(CostoNetto.Value * AliquotaIva.Value / 100, 2)
            : 0;

        [NotMapped]
        public decimal CostoTotale => CostoNetto.HasValue ? CostoNetto.Value + CostoIva : 0;
    }
}
