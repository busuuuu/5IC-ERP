using System.ComponentModel.DataAnnotations;

namespace ERP_Ordini.Models
{
    public class Ordine
    {
        [Key]
        public int IdOrdine { get; set; }
        public StatoOrdine Stato { get; set; }

        public int IdDettagliOrdine { get; set; }   
        public DettagliOrdine DettagliOrdine { get; set; }
    }
    public enum StatoOrdine
    {
        InElaborazione, Spedito, Consegnato
    }
}
