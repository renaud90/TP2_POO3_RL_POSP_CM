using System.Collections.Generic;
using System.Threading.Tasks;
using Bibliotheques.MVC.Models;

namespace Bibliotheques.MVC.Proxies
{
    public interface IBibliothequeService
    {
        Task<Emprunt> ObtenirEmpruntParId(int id);
        Task<IEnumerable<Emprunt>> ObtenirTousLesEmprunts();
        Task AjouterEmprunt(Emprunt emprunt);
        Task ModifierEmprunt(Emprunt emprunt);
        Task EffacerEmprunt(int id);
        Task<IEnumerable<Livre>> ObtenirTousLesLivres();
        Task<IEnumerable<Usager>> ObtenirTousLesUsagers();
    }
}
