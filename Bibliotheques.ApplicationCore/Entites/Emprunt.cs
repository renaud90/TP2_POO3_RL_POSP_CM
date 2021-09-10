using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bibliotheques.ApplicationCore.Entites
{
    public class Emprunt : BaseEntite
    {
        [Required, ForeignKey("Livre")]
        public int LivreId { get; set; }
        
        [Required, ForeignKey("Usager")]
        public int UsagerId { get; set; }
        
        [Required]
        public DateTime DateEmprunt { get; set; }
        public DateTime DateRetour { get; set; }

        public virtual Livre Livre { get; set; }
        public virtual Usager Usager { get; set; }

    }
}
