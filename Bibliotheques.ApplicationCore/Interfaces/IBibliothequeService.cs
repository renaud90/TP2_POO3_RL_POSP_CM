using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bibliotheques.ApplicationCore.Entites;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotheques.ApplicationCore.Interfaces
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