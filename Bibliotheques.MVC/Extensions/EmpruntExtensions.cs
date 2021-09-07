using System;
using Bibliotheques.ApplicationCore.Entites;

namespace Bibliotheques.MVC.Extensions
{
    public static class EmpruntExtensions
    {
        public static DateTime ObtenirDateLimite(this Emprunt emprunt, int nbJoursLocation)
        {
            return emprunt.DateEmprunt.AddDays(nbJoursLocation);
        }
    }
}