using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bibliotheques.MVC.Models
{
    public class Emprunt
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey("Livre")]
        public int LivreId { get; set; }
        
        [Required, ForeignKey("Usager")]
        public int UsagerId { get; set; }
        
        [Required]
        [Display(Name = "Date de l'emprunt")]
        public DateTime DateEmprunt { get; set; }

        [Display(Name = "Date de retour")]
        public DateTime DateRetour { get; set; }

        public virtual Livre Livre { get; set; }

        public virtual Usager Usager { get; set; }

    }
}
