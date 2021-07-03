using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace Bibliotheque_LIPAJOLI.Models
{
    public class Emprunt
    {
        [Required]
        public string CodeLivre { get; set; }
        [Required]
        public string NumAbonne { get; set; }
        [Required]
        public DateTime DateEmprunt { get; set; }
        public DateTime DateRetour { get; set; }

        public DateTime ObtenirDateLimite(IConfiguration config)
        {
            var nbJoursLocation = config.GetSection("Bibliotheque")
                .GetValue<int>("JoursLocation");

            return DateEmprunt.AddDays(nbJoursLocation);
        }
    }
}
