using System.Globalization;
using System.Text;

namespace Bibliotheques.MVC.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Méthode d'extension pour obtenir la version d'une chaîne de caractère sans les accents / signes diacritiques.
        /// </summary>
        /// <param name="texte">La chaîne de caractère à modifier</param>
        /// <returns>La chaîne sans aucun signe diacritiques.</returns>
        public static string EnleverSymbolesDiacritiques(this string texte) 
        {
            var texteNormalise = texte.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var caractere in texteNormalise.EnumerateRunes())
            {
                var unicodeCategory = Rune.GetUnicodeCategory(caractere);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(caractere);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}