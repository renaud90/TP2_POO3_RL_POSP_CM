using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bibliotheque_LIPAJOLI.Models
{
    public class Livre
    {
        [Required, Key]
        public string CodeLivre { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public string Titre { get; set; }
        public string Categorie { get; set; }
        public int Quantite { get; set; }
        public double Prix { get; set; }
        public List<string> Auteurs { get; set; }
    }
}

