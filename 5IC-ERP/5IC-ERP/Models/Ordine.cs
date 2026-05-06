namespace _5IC_ERP.Models
{
    public class Ordine
    {
        public int IdOrdine { get; set; }
        public int IdFornitore { get; set; }
        public DateTime DataOrdine { get; set; }
        public StatoOrdine StatoOrdine { get; set; }
    }
    public enum StatoOrdine
    {
        InElaborazione, Spedito, Consegnato
    }
}
