using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Configuration;

namespace Bibliotheque_LIPAJOLI.Models
{
    public class Emprunt
    {
        [Required, ForeignKey("Livre")]
        public string CodeLivre { get; set; }
        [Required, ForeignKey("Usager")]
        public string NumAbonne { get; set; }
        [Required]
        public DateTime DateEmprunt { get; set; }
        public DateTime DateRetour { get; set; }

        public Livre Livre { get; set; }
        public Usager Usager { get; set; }

    }
}
