using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Bibliotheques.MVC.Models
{
    public class Livre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Code du livre")]
        public string CodeLivre { get; set; }
        
        [Required(ErrorMessage = "Ce champ est requis.")]
        [RegularExpression(@"^[0-9]-[0-9]{5}-[0-9]{3}-[0-9]", ErrorMessage = "La valeur doit avoir le format 5-55555-555-5.")]
        [DisplayName("n° ISBN10")]
        public string Isbn10 { get; set; }
        
        [Required(ErrorMessage = "Ce champ est requis")]
        [RegularExpression(@"^[0-9]{3}-[0-9]-[0-9]{5}-[0-9]{3}-[0-9]", ErrorMessage = "La valeur doit avoir le format 555-5-55555-555-5.")]
        [Display(Name = "n° ISBN13", Prompt = "Entrez le code ISBN du livre")]
        public string Isbn13 { get; set; }
        
        [Required(ErrorMessage = "Ce champ est requis.")]
        [MaxLength(200, ErrorMessage = "La titre doit comporter un maximum de 200 caractères.")]
        [Display(Prompt="Entrez le titre du livre")]
        public string Titre { get; set; }
        
        [DisplayName("Catégorie")]
        [Required(ErrorMessage = "Ce champ est requis.")]
        public string Categorie { get; set; }
        
        [Required(ErrorMessage = "Ce champ est requis.")] 
        [Range(0, int.MaxValue, ErrorMessage = "La quantité doit être positive.")]
        [DisplayName("Quantité")]
        public int Quantite { get; set; }

        [Required(ErrorMessage = "Ce champ est requis.")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être un nombre postitif.")]
        public double Prix { get; set; }
        
        [MinLength(1, ErrorMessage = "Le livre doit posséder au moins un auteur.")]
        public string Auteurs { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Emprunt> Emprunts { get; set; }
    }
}

