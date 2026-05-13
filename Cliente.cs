using System.ComponentModel.DataAnnotations;

namespace CLIENTI.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Ragione Sociale")]
        public string RagioneSociale { get; set; }

        [Required]
        public string Indirizzo { get; set; }

        [Required]
        public string Citta { get; set; }

        [Required]
        public string CAP { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public ICollection<Ordine> Ordini { get; set; } = new List<Ordine>();
    }
}