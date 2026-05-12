using System.ComponentModel.DataAnnotations;

namespace ERP_Ordini.Models
{
    public class DettagliOrdine
    {
        [Key]
        public int Id { get; set; }

        public DateTime DataOrdine { get; set; }

        public string IdCliente { get; set; }

        //public List<string> Prodotti
    }
}
