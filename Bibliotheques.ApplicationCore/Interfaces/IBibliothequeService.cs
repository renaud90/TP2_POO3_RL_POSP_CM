using System.Collections.Generic;
using System.Threading.Tasks;
using Bibliotheques.ApplicationCore.Entites;

namespace Bibliotheques.ApplicationCore.Interfaces
{
    public interface IBibliothequeService
    {
        Task<Emprunt> ObtenirEmpruntParId(int id);
        Task<IEnumerable<Emprunt>> ObtenirTousLesEmprunts();
        Task AjouterEmprunt(Emprunt emprunt);
        Task ModifierEmprunt(Emprunt emprunt, bool enRetard);//, bool estEnRetard);
        Task EffacerEmprunt(int id);
        Task<IEnumerable<Livre>> ObtenirTousLesLivres();
        Task<IEnumerable<Usager>> ObtenirTousLesUsagers();
    }
}