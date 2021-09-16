using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bibliotheques.ApplicationCore.Entites;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotheques.ApplicationCore.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntite
    {
        Task<T> ObtenirParIdAsync(int id);
        Task<IEnumerable<T>> ObtenirToutAsync();
        Task<IEnumerable<T>> ObtenirListeAsync(Expression<Func<T, bool>> predicat);
        Task AjouterAsync(T entite);
        Task SupprimerAsync(T entite);
        Task ModifierAsync(T entite);
    }
}