namespace _5IC_ERP.Models
{
    public class DistintaBaseItem
    {
        public int Id { get; set; }

        public string Prodotto { get; set; } = "";

        public string Componente { get; set; } = "";

        public int Quantita { get; set; }

        public int Giacenza { get; set; }
    }
}