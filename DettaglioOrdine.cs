using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLIENTI.Models
{
    public class DettaglioOrdine
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Ordine")]
        public int OrdineId { get; set; }
        public Ordine Ordine { get; set; }

        [ForeignKey("Articolo")]
        public int ArticoloId { get; set; }
        public Articolo Articolo { get; set; }

        public int Quantita { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrezzoUnitario { get; set; }
    }
}