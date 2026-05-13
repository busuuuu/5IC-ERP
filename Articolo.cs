using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLIENTI.Models
{
    public class Articolo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Codice { get; set; }

        [Required]
        public string Descrizione { get; set; }

        public TipoArticolo Tipo { get; set; }

        public int Giacenza { get; set; }

        public int ScortaMinima { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Prezzo { get; set; }
    }

    public enum TipoArticolo
    {
        Componente,
        ProdottoFinito
    }
}