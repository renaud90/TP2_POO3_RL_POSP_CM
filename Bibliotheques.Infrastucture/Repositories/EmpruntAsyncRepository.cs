using Bibliotheques.ApplicationCore.Entites;
using Bibliotheques.ApplicationCore.Interfaces;
using Bibliotheques.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheques.Infrastructure.Repositories
{
    public class AsyncRepository<T>: IAsyncRepository<T> where T: BaseEntite
    {
        private readonly BibliothequeContext _context;
        public AsyncRepository(BibliothequeContext context)
        {
            _context = context;
        }

        public async Task<T> ObtenirParIdAsync(int id) {
            return await _context.Set<T>().FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<IEnumerable<T>> ObtenirToutAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> ObtenirListeAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task AjouterAsync(T entite) 
        {
            await _context.Set<T>().AddAsync(entite);
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
