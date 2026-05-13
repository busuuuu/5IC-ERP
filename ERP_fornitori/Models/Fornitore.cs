using System.ComponentModel.DataAnnotations;

namespace FornitoriERP.Models
{
    public class Fornitore
    {
        public int IdFornitore { get; set; }

        [Required(ErrorMessage = "Il nome fornitore è obbligatorio")]
        [StringLength(255, ErrorMessage = "Max 255 caratteri")]
        [Display(Name = "Nome Fornitore")]
        public string NomeFornitore { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Max 255 caratteri")]
        [Display(Name = "Contatto")]
        public string? ContattoFornitore { get; set; }

        [StringLength(50, ErrorMessage = "Max 50 caratteri")]
        [Phone(ErrorMessage = "Numero di telefono non valido")]
        [Display(Name = "Telefono")]
        public string? Telefono { get; set; }

        [StringLength(100, ErrorMessage = "Max 100 caratteri")]
        [EmailAddress(ErrorMessage = "Email non valida")]
        [Display(Name = "Email")]
        public string? Email { get; set; }
    }
}
