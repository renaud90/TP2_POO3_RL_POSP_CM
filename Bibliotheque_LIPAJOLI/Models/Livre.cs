using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bibliotheque_LIPAJOLI.Models
{
    public class Livre
    {
        [Required, Key]
        public string CodeLivre { get; set; }
        public string Isbn10 { get; set; }
        [Required]
        public string Isbn13 { get; set; }
        [Required]
        public string Titre { get; set; }
        [Required]
        public string Categorie { get; set; }
        [Required] 
        public int Quantite { get; set; } = 0;
        [Required] 
        public double Prix { get; set; } = 0;
        [Required]
        public string Auteurs { get; set; }
    }
}

