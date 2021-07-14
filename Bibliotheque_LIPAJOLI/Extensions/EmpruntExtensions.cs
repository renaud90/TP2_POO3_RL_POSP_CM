using System;
using Bibliotheque_LIPAJOLI.Models;

namespace Bibliotheque_LIPAJOLI.Extensions
{
    public static class EmpruntExtensions
    {
        public static DateTime ObtenirDateLimite(this Emprunt emprunt, int nbJoursLocation)
        {
            return emprunt.DateEmprunt.AddDays(nbJoursLocation);
        }
    }
}