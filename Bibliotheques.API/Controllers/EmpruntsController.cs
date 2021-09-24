using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bibliotheques.ApplicationCore.Entites;
using Bibliotheques.ApplicationCore.Interfaces;

namespace Bibliotheques.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpruntsController : ControllerBase
    {
        private readonly IBibliothequeService _crudService;

        public EmpruntsController(IBibliothequeService crudService)
        {
            _crudService = crudService;
        }

        /// <summary>
        /// Premet l'obtention et le retour d'une liste de tous les emprunts ayant été effectués à la Bibliothèque Lipajoli
        /// </summary>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="200">Liste complète des emprunts effectués à la bibliothèque Lipajoli trouvée et retournée</response>
        /// <response code="404">Liste complète des emprunts effectués à la bibliothèque Lipajoli introuvable</response>
        /// <response code="500">Oups! Le service demandé est indisponible pour le moment</response>
        // GET: api/<EmpruntController>
        [HttpGet]
        public async Task<IEnumerable<Emprunt>> Get()
        {
            return await _crudService.ObtenirTousLesEmprunts();
        }

        /// <summary>
        /// Premet l'obtention et le retour des informations d'un emprunt spécifique, ciblé par l'id passé en paramètre
        /// </summary>
        /// <param name="id">id de l'emprunt à retourner</param>
        /// <returns></returns>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="200">L'emprunt spécifié a été trouvé et retourné</response>
        /// <response code="404">Emprunt introuvable pour l'id specifié</response>
        /// <response code="500">Oups! Le service demandé est indisponible pour le moment</response>
        // GET api/<EmpruntController>/5
        [HttpGet("{id:int}")]
        public async Task<Emprunt> Get(int id)
        {
            return await _crudService.ObtenirEmpruntParId(id);
        }

        /// <summary>
        /// Création d'un nouvel emprunt respectant les normes de validation
        /// </summary>
        /// <param name="emprunt">Nouvel emprunt auquel les valeurs de champs selectionnées par l'utilisateur seront assignées</param>
        /// <returns></returns>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="200">Nouvel emprunt créé et ajouté</response>
        /// <response code="404">Impossible de procéder à la création d'un nouvel emprunt avec l'id specifié</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // POST api/<EmpruntController>
        [HttpPost]
        public async Task<ActionResult<Emprunt>> Post([FromBody] Emprunt emprunt)
        {
            if (emprunt == null)
                return BadRequest("Erreur de création : il n'y a aucun emprunt à créer.");
            
            if (await EmpruntSimilaireExiste(emprunt))
                return BadRequest("Erreur de création : Un emprunt similaire existe déjà et n'a pas été retourné.");
            
            if (!emprunt.Usager.PeutEmprunter)
                return BadRequest("Erreur de création : Cet usager a accumulé trop de défaillances.");
            
            if (emprunt.Livre.Quantite == 0)
                return BadRequest("Erreur de création : Le livre n'est plus disponible.");

            if (!ModelState.IsValid)
                return BadRequest("Erreur de création : L'emprunt n'est pas conforme.");
            
            await _crudService.AjouterEmprunt(emprunt);
            return CreatedAtAction(nameof(Post), new { id = emprunt.Id }, emprunt);
        }

        /// <summary>
        /// Permet la modification des informations d'un emprunt existant
        /// </summary>
        /// <param name="id">id de l'emprunt à modifier</param>
        /// <param name="emprunt">emprunt à modifier</param>
        /// <param name="retard">valeur booléenne signifiant que la date 
        /// limite de retour du livre emprunté ne doit pas être dépassée</param>
        /// <returns></returns>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="200">Emprunt cible trouvé et mofifié</response>
        /// <response code="404">Emprunt introuvable pour l'id specifié</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // PUT api/<EmpruntController>/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Emprunt>> Put(int id, [FromBody] Emprunt emprunt, bool retard = false)
        {
            if (emprunt == null || emprunt.Id != id)
                return BadRequest("Requête invalide : L'emprunt n'est pas le même que celui de l'identifiant/est nul.");

            var empruntAModifier = await _crudService.ObtenirEmpruntParId(emprunt.Id);

            if (empruntAModifier == null)
                return NotFound();
            
            if (!ModelState.IsValid || emprunt.DateRetour == DateTime.MinValue) 
                return BadRequest("Requête invalide : L'emprunt n'est pas conforme.");
            
            await _crudService.ModifierEmprunt(emprunt, retard);
            
            return NoContent();

        }

        /// <summary>
        /// Permet la suppression d'un emprunt existant, si le livre concerné n'a pas déjà été rendu à la bibliothèque
        /// </summary>
        /// <param name="id">id de l'emprunt à supprimer</param>
        /// <returns></returns>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="200">Emprunt cible trouvé et supprimé</response>
        /// <response code="404">Emprunt introuvable pour l'id specifié</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // DELETE api/<EmpruntController>/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Emprunt>> Delete(int id)
        {
            var empruntASupprimer = await _crudService.ObtenirEmpruntParId(id);

            if (empruntASupprimer == null)
                return NotFound();

            if (empruntASupprimer.DateRetour != DateTime.MinValue)
                return BadRequest("Suppression impossible : Le livre de l'emprunt à déjà été retourné.");
            
            await _crudService.EffacerEmprunt(id);
            return NoContent();
        }

        /// <summary>
        /// Compare un emprunt aux emprunts préexistants dans la base de données et retourne une valeur booléenne 
        /// </summary>
        /// <param name="emprunt">emprunt devant être comparé et validé</param>
        /// <returns></returns>
        private async Task<bool> EmpruntSimilaireExiste(Emprunt emprunt)
        {
            var emprunts = await _crudService.ObtenirTousLesEmprunts();

            return emprunts.Any(e =>
                e.UsagerId == emprunt.UsagerId &&
                e.LivreId == emprunt.LivreId &&
                e.DateRetour == DateTime.MinValue);
        }
    }
}
