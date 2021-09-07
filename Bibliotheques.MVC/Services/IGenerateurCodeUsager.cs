using Bibliotheques.ApplicationCore.Entites;

namespace Bibliotheques.MVC.Services
{
    public interface IGenerateurCodeUsager
    {
        /// <summary>
        /// Permet de générer un code d'usager automatiquement en fournissant un usager en paramètre.
        /// </summary>
        /// <param name="usager">L'usager dont ont génère le code</param>
        /// <returns>Le code usager généré, sous forme de chaîne de caractère.</returns>
        string GenererCodeUsager(Usager usager);
    }
}