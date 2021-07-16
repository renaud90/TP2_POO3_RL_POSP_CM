using System.Linq;
using Bibliotheque_LIPAJOLI.Data;
using Bibliotheque_LIPAJOLI.Extensions;
using Bibliotheque_LIPAJOLI.Models;

namespace Bibliotheque_LIPAJOLI.Services
{
    public class GenerateurCodeUsager : IGenerateurCodeUsager
    {
        private readonly BibliothequeContext _context;

        public GenerateurCodeUsager(BibliothequeContext context)
        {
            _context = context;
        }
        
        public string GenererCodeUsager(Usager usager)
        {
            string codeFinal;
            var dernierUsager = _context.Usagers
                .ToList()
                .OrderByDescending(ObtenirValeurNumeroAbonne)
                .FirstOrDefault();

            if (dernierUsager == null)
            {
                codeFinal = ObtenirLettresCodeUsager(usager) + "0001";
            }
            else
            {
                int codeChiffres = ObtenirValeurNumeroAbonne(dernierUsager) + 1;

                string codeChiffresString = codeChiffres.ToString("D4"); 
                codeFinal = ObtenirLettresCodeUsager(usager) + codeChiffresString;
            }

            return codeFinal;
        }
        
        private int ObtenirValeurNumeroAbonne(Usager usager)
        {
            return int.Parse(usager.NumAbonne.Substring(4, 4));
        }
        
        private string ObtenirLettresCodeUsager(Usager usager)
        {
            string codeLettresPrenom = CodeLettresNom(usager.Prenom);
            string codeLettresNom = CodeLettresNom(usager.Nom);

            string codeLettres = codeLettresNom + codeLettresPrenom;

            return codeLettres.ToUpper().EnleverSymbolesDiacritiques();
        }
        
        private string CodeLettresNom(string nom)
        {
            string codeLettresNom;
            if (nom.Contains('-'))
            {
                int indexTiret = nom.IndexOf("-");

                string codeNomCompose = nom.Substring(indexTiret + 1, 1);

                codeLettresNom = nom.Substring(0, 1) + codeNomCompose;
            }
            else
            {
                codeLettresNom = nom.Substring(0, 2);
            }

            return codeLettresNom;
        }
    }
}