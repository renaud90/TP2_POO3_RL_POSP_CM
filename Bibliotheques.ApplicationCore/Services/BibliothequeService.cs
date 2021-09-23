using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bibliotheques.ApplicationCore.Entites;
using Bibliotheques.ApplicationCore.Interfaces;

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
            var emprunt = await _empruntsRepository.ObtenirParIdAsync(id);
            emprunt.Livre = await _livresRepository.ObtenirParIdAsync(emprunt.LivreId);
            emprunt.Usager = await _usagersRepository.ObtenirParIdAsync(emprunt.UsagerId);

            return emprunt;
        }

        public async Task<IEnumerable<Emprunt>> ObtenirTousLesEmprunts()
        {
            var emprunts = await _empruntsRepository.ObtenirToutAsync();
            foreach (var emprunt in emprunts)
            {
                emprunt.Livre = await _livresRepository.ObtenirParIdAsync(emprunt.LivreId);
                emprunt.Usager = await _usagersRepository.ObtenirParIdAsync(emprunt.UsagerId);
            }

            return emprunts;
        }

        public async Task<IEnumerable<Emprunt>> ObtenirListeEmprunts(Expression<Func<Emprunt, bool>> predicat)
        {
            var emprunts = await _empruntsRepository.ObtenirListeAsync(predicat);
            
            foreach (var emprunt in emprunts)
            {
                emprunt.Livre = await _livresRepository.ObtenirParIdAsync(emprunt.LivreId);
                emprunt.Usager = await _usagersRepository.ObtenirParIdAsync(emprunt.UsagerId);
            }

            return emprunts;
        }

        public async Task AjouterEmprunt(Emprunt emprunt)
        {
            emprunt.Livre = null;
            emprunt.Usager = null;
            
            await _empruntsRepository.AjouterAsync(emprunt);
            
            var livre = await _livresRepository.ObtenirParIdAsync(emprunt.LivreId);
            livre.Quantite--;
            await _livresRepository.ModifierAsync(livre);
         
        }

        public async Task ModifierEmprunt(Emprunt emprunt, bool enRetard)//, bool estEnRetard)
        {
            emprunt.Livre.Quantite++;
            
            if (enRetard)
                emprunt.Usager.Defaillance++;
            
            await _empruntsRepository.ModifierAsync(emprunt);
        }

        public async Task EffacerEmprunt(int id)
        {
            var emprunt = await _empruntsRepository.ObtenirParIdAsync(id);
            await _empruntsRepository.SupprimerAsync(emprunt);

            var livre = await _livresRepository.ObtenirParIdAsync(emprunt.LivreId);
            livre.Quantite++;
            await _livresRepository.ModifierAsync(emprunt.Livre);
        }

        public async Task<IEnumerable<Livre>> ObtenirTousLesLivres()
        {
            var livres = await _livresRepository.ObtenirToutAsync();

            foreach (var livre in livres)
            {
                livre.Emprunts = (await _empruntsRepository.ObtenirListeAsync(e => e.LivreId == livre.Id)).ToList();
            }

            return livres;
        }
        
        public async Task<IEnumerable<Usager>> ObtenirTousLesUsagers()
        {
            var usagers = await _usagersRepository.ObtenirToutAsync();

            foreach (var usager in usagers)
            {
                usager.Emprunts = (await _empruntsRepository.ObtenirListeAsync(e => e.UsagerId == usager.Id)).ToList();
            }

            return usagers;
        }

    }
}