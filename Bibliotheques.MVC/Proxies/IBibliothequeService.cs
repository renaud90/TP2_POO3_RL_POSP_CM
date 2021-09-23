using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bibliotheques.MVC.Models;

namespace Bibliotheques.MVC.Proxies
{
    public interface IBibliothequeService
    {
        Task<Emprunt> ObtenirEmpruntParId(int id);
        Task<IEnumerable<Emprunt>> ObtenirTousLesEmprunts();
        Task<HttpResponseMessage> AjouterEmprunt(Emprunt emprunt);
        Task<HttpResponseMessage> ModifierEmprunt(Emprunt emprunt);
        Task<HttpResponseMessage> EffacerEmprunt(int id);
        Task<IEnumerable<Livre>> ObtenirTousLesLivres();
        Task<IEnumerable<Usager>> ObtenirTousLesUsagers();
    }
}
