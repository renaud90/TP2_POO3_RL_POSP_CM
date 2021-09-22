using Bibliotheques.ApplicationCore.Entites;
using Bibliotheques.ApplicationCore.Interfaces;
using Bibliotheques.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bibliotheques.Infrastructure.Repositories
{
    public class EmpruntAsyncRepository: IAsyncRepository<Emprunt> //where T: BaseEntite
    {
        private readonly BibliothequeContext _context;
        public EmpruntAsyncRepository(BibliothequeContext context)
        {
            _context = context;
        }
        
        public async Task<Emprunt> ObtenirParIdAsync(int id) {
            return await _context.Set<Emprunt>()
                .FirstOrDefaultAsync(_ => _.Id == id);            
        }

        public async Task<IEnumerable<Emprunt>> ObtenirToutAsync()
        {
            return await _context.Set<Emprunt>()
                .ToListAsync();
        }

        public async Task<IEnumerable<Emprunt>> ObtenirListeAsync(Expression<Func<Emprunt, bool>> predicate)
        {
            return await _context.Set<Emprunt>()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task AjouterAsync(Emprunt entite) 
        {
            _context.Set<Emprunt>().Add(entite);
            await _context.SaveChangesAsync();
        }

        public async Task SupprimerAsync(Emprunt entite) 
        {
            
            _context.Set<Emprunt>().Remove(entite);
            await _context.SaveChangesAsync();
        }

        public async Task ModifierAsync(Emprunt entite)
        {
            _context.Update(entite);
            await _context.SaveChangesAsync();
        }
    }
}
