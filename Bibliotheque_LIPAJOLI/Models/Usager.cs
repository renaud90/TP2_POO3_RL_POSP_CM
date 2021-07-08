using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bibliotheque_LIPAJOLI.Models
{
    public enum Statut
    {
        Enseignant,
        Etudiant
    }

    public class Usager
    {

        [Required, Key] 
        public string NumAbonne { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Ce champ doit comporter au moins 2 caractères et est limité à 30")]
        [RegularExpression(@"^[A-zÀ-ú]*$", ErrorMessage = "Ce champ ne doit comporter que des lettres")]
        [Display(Name = "Nom de famille", Prompt = "Entrez votre nom de famille")]
        public string Nom { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Ce champ doit comporter au moins 2 caractères et est limité à 20")]
        [RegularExpression(@"^[A-zÀ-ú]*$", ErrorMessage = "Ce champ ne doit comporter que des lettres")]
        [Display(Name = "Prénom", Prompt = "Entrez votre prénom")]
        public string Prenom { get; set; }

        [Required]
        public Statut Statut { get; set; }

        [Required]
        [Display(Name = "Courriel", Prompt = "Entrez votre adresse courriel")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Nombre de défaillance(s)", Prompt = "0")]
        [RegularExpression(@"^[0-9]*$"/*, ErrorMessage = "Ce champ ne doit comporter que des chiffres" //*/)]
        public int Defaillance { get; set; } = 0;

        public ICollection<Emprunt> Emprunts { get; set; }

        public bool PeutEmprunter => Defaillance < 3;



    }

}
