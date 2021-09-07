using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bibliotheques.ApplicationCore.Entites;

namespace Bibliotheques.ApplicationCore.Interfaces
{
    public interface IBibliothequeService
    {
        Task<Emprunt> ObtenirEmpruntParId(int id);
        Task<IEnumerable<Emprunt>> ObtenirTousLesEmprunts();
        Task<IEnumerable<Emprunt>> ObtenirListeEmprunts(Expression<Func<Emprunt, bool>> predicat);
        Task AjouterEmprunt(Emprunt emprunt);
        Task ModifierEmprunt(Emprunt emprunt);
    }
}