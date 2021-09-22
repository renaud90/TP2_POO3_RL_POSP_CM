using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bibliotheques.ApplicationCore.Entites;
using Bibliotheques.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Bibliotheques.ApplicationCore.Services
{
    public class BibliothequeService : IBibliothequeService
    {
        private readonly IAsyncRepository<Emprunt> _empruntsRepository;
        private readonly IAsyncRepository<Livre> _livresRepository;
        private readonly IAsyncRepository<Usager> _usagersRepository;

        public BibliothequeService(IAsyncRepository<Emprunt> empruntsRepository, IAsyncRepository<Livre> livresRepository, IAsyncRepository<Usager> usagersRepository)
        {
            _empruntsRepository = empruntsRepository;
            _livresRepository = livresRepository;
            _usagersRepository = usagersRepository;
        }

        public async Task<Emprunt> ObtenirEmpruntParId(int id)
        {
            return await _empruntsRepository
                .ObtenirParIdAsync(id);
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
            var livre = await _livresRepository.ObtenirParIdAsync(emprunt.LivreId);
            livre.Quantite -= 1;
            await _empruntsRepository.AjouterAsync(emprunt);
            await _livresRepository.ModifierAsync(livre);
        }

        public async Task ModifierEmprunt(Emprunt emprunt)
        {
            var empruntExiste = await EmpruntExiste(emprunt);
            if (emprunt == null || !empruntExiste)
                return;

            await _empruntsRepository.ModifierAsync(emprunt);
        }

        public async Task EffacerEmprunt(int id)
        {
            Emprunt emprunt = await _empruntsRepository.ObtenirParIdAsync(id);
            await _empruntsRepository.SupprimerAsync(emprunt);
        }

        public async Task<IEnumerable<Livre>> ObtenirTousLesLivres()
        {
            return await _livresRepository.ObtenirToutAsync();
        }
        
        public async Task<IEnumerable<Usager>> ObtenirTousLesUsagers()
        {
            return await _usagersRepository.ObtenirToutAsync();
        }

        private async Task<bool> EmpruntExiste(Emprunt emprunt)
        {
            var empruntDansRepo = await _empruntsRepository.ObtenirParIdAsync(emprunt.Id);

            return empruntDansRepo != null;
        }
        
    }
}