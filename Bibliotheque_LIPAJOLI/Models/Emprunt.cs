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

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime DateEmprunt { get; set; }

        public DateTime DateRetour { get; set; }

        public Livre Livre { get; set; }

        public Usager Usager { get; set; }

        public DateTime ObtenirDateLimite(IConfiguration config)
        {
            var nbJoursLocation = config.GetSection("Bibliotheque:JoursLocations")
                .Get<int>();

            return DateEmprunt.AddDays(nbJoursLocation);
        }
    }
}
