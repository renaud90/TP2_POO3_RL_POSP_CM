using System.ComponentModel.DataAnnotations;

namespace Bibliotheque_LIPAJOLI.Models
{
    public class Livre
    {
        [Required(ErrorMessage = "Ce champ est requis."), Key]
        [RegularExpression(@"^[A-Z]{3}[0-9]{3}", ErrorMessage = "Le code doit avoir le format MMM555.")]
        public string CodeLivre { get; set; }
        [Required(ErrorMessage = "Ce champ est requis.")]
        [RegularExpression(@"^[0-9]-[0-9]{5}-[0 - 9]{ 3}-[0 - 9]$", ErrorMessage = "La valeur doit avoir le format 5-55555-555-5.")]
        public string Isbn10 { get; set; }
        [Required(ErrorMessage = "Ce champ est requis")]
        [RegularExpression(@"^[0 - 9]{3}-[0-9]-[0-9]{5}-[0 - 9]{ 3}-[0 - 9]$", ErrorMessage = "La valeur doit avoir le format 555-5-55555-555-5.")]
        public string Isbn13 { get; set; }
        [Required(ErrorMessage = "Ce champ est requis.")]
        [MaxLength(200, ErrorMessage = "La titre doit comporter un maximum de 200 caractères.")]
        public string Titre { get; set; }
        [Required(ErrorMessage = "Ce champ est requis.")]
        public string Categorie { get; set; }
        [Required(ErrorMessage = "Ce champ est requis.")] 
        [Range(0, int.MaxValue, ErrorMessage = "La quantité doit être positive.")]
        public int Quantite { get; set; } = 0;
        [Required(ErrorMessage = "Ce champ est requis.")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être un nombre postitif.")]
        public double Prix { get; set; } = 0;
        [Required(ErrorMessage = "Ce champ est requis.")]
        [MinLength(1, ErrorMessage = "Le livre doit posséder au moins un auteur.")]
        public string Auteurs { get; set; }
    }
}

