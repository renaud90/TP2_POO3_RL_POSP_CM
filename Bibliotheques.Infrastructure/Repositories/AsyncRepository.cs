using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bibliotheques.ApplicationCore.Entites;
using Bibliotheques.ApplicationCore.Interfaces;
using Bibliotheques.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bibliotheques.Infrastructure.Repositories
{
    public class AsyncRepository<T> : IAsyncRepository<T> where T : BaseEntite
    {
        private readonly BibliothequeContext _context;

        public AsyncRepository(BibliothequeContext context)
        {
            _context = context;
        }

        public async Task<T> ObtenirParIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> ObtenirToutAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> ObtenirListeAsync(Expression<Func<T, bool>> predicat)
        {
            return await _context.Set<T>()
                .Where(predicat)
                .ToListAsync();
        }

        public async Task AjouterAsync(T entite)
        {
            _context.Set<T>().Add(entite);
            await _context.SaveChangesAsync();
        }


        public async Task SupprimerAsync(T entite)
        {
            _context.Set<T>().Remove(entite);
            await _context.SaveChangesAsync();
        }

        public async Task ModifierAsync(T entite)
        {
            _context.Set<T>().Update(entite);
            await _context.SaveChangesAsync();
        }
        
    }
}
