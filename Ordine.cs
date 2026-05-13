using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLIENTI.Models
{
    public class Ordine
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public DateTime DataOrdine { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Totale { get; set; }

        public StatoOrdine Stato { get; set; } = StatoOrdine.Preventivo;

        public string NumeroTracking { get; set; }

        public ICollection<DettaglioOrdine> DettagliOrdine { get; set; } = new List<DettaglioOrdine>();
    }

    public enum StatoOrdine
    {
        Preventivo,
        InLavorazione,
        Spedito,
        Consegnato
    }
}