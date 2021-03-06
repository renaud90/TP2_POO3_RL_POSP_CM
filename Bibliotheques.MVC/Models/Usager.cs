using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Bibliotheques.MVC.Models
{
    public enum Statut
    {
        Enseignant,
        [Display(Name = "Étudiant")]
        Etudiant
    }

    public class Usager
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Numéro d'abonné", Prompt = "Entrez le numéro d'abonné")]
        public string NumAbonne { get; set; } 

        [Required(ErrorMessage = "Veuillez renseigner ce champ")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Ce champ doit comporter au moins 2 caractères et est limité à 30")]
        [RegularExpression(@"^[A-zÀ-ú\-]*$", ErrorMessage = "Ce champ ne doit comporter que des lettres")]
        [Display(Name = "Nom de famille", Prompt = "Entrez le nom de famille de l'abonné")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Veuillez renseigner ce champ")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Ce champ doit comporter au moins 2 caractères et est limité à 20")]
        [RegularExpression(@"^[A-zÀ-ú\-]*$", ErrorMessage = "Ce champ ne doit comporter que des lettres")]
        [Display(Name = "Prénom", Prompt = "Entrez le prénom de l'abonné")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Veuillez renseigner ce champ")]
        public Statut Statut { get; set; }

        [Required(ErrorMessage = "Veuillez renseigner ce champ")]
        [Display(Name = "Courriel", Prompt = "Entrez votre adresse courriel")]
        [DataType(DataType.EmailAddress, ErrorMessage ="Veuillez entrer une adresse courriel valide")]
        public string Email { get; set; }

        //Sera incremente automatiquement dans la gestion des emprunts.
        //TP2.2: Dans Controller ajouter loop qui compte les insatnces dont DateTime.Now>DateEmprunt+this.ObtenirJoursEmprunt()
        [Display(Name = "Nombre de défaillance(s)")]
        public int Defaillance { get; set; } = 0;
        
        [JsonIgnore]
        public virtual ICollection<Emprunt> Emprunts { get; set; }
        
        public bool PeutEmprunter => Defaillance < 3;
        
    }

}
