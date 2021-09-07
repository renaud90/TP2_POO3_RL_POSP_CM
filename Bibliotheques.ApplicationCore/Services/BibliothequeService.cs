using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bibliotheques.ApplicationCore.Entites;
using Bibliotheques.ApplicationCore.Interfaces;

namespace Bibliotheques.ApplicationCore.Services
{
    public class BibliothequeService : IBibliothequeService
    {
        private readonly IAsyncRepository<Emprunt> _empruntsRepository;

        public BibliothequeService(IAsyncRepository<Emprunt> empruntsRepository)
        {
            _empruntsRepository = empruntsRepository;
        }

        public async Task<Emprunt> ObtenirEmpruntParId(int id)
        {
            return await _empruntsRepository.ObtenirParIdAsync(id);
        }

        public async Task<IEnumerable<Emprunt>> ObtenirTousLesEmprunts()
        {
            return await _empruntsRepository.ObtenirToutAsync();
        }

        public async Task<IEnumerable<Emprunt>> ObtenirListeEmprunts(Expression<Func<Emprunt, bool>> predicat)
        {
            return await _empruntsRepository.ObtenirListeAsync(predicat);
        }

        public async Task AjouterEmprunt(Emprunt emprunt)
        {
            var empruntExiste = await EmpruntExiste(emprunt);
            if (emprunt == null || empruntExiste)
                return;

            await _empruntsRepository.AjouterAsync(emprunt);
        }

        public async Task ModifierEmprunt(Emprunt emprunt)
        {
            var empruntExiste = await EmpruntExiste(emprunt);
            if (emprunt == null || !empruntExiste)
                return;

            await _empruntsRepository.ModifierAsync(emprunt);
        }

        private async Task<bool> EmpruntExiste(Emprunt emprunt)
        {
            var empruntDansRepo = await _empruntsRepository.ObtenirParIdAsync(emprunt.Id);
            
            return empruntDansRepo != null;
        }
    }
}