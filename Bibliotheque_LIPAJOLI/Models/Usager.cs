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
        public string Nom { get; set; }
        [Required]
        public string Prenom { get; set; }
        [Required]
        public Statut Statut { get; set; }
        [Required]
        public string Email { get; set; }
        public int Defaillance { get; set; } = 0;

        public ICollection<Emprunt> Emprunts { get; set; }

        public bool PeutEmprunter => Defaillance < 3;
    }

}
