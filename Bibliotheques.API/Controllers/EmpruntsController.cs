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

        // GET: api/<EmpruntController>
        [HttpGet]
        public async Task<IEnumerable<Emprunt>> Get()
        {
            return await _crudService.ObtenirTousLesEmprunts();
        }

        // GET api/<EmpruntController>/5
        [HttpGet("{id:int}")]
        public async Task<Emprunt> Get(int id)
        {
            return await _crudService.ObtenirEmpruntParId(id);
        }

        // POST api/<EmpruntController>
        [HttpPost]
        public async Task<ActionResult<Emprunt>> Post([FromBody] Emprunt emprunt)
        {
            if (emprunt == null)
                return BadRequest("Erreur de création : il n'y a aucun emprunt à créer.");
            
            if (EmpruntExiste(emprunt))
                return BadRequest("Erreur de création : L'emprunt existe déja.");
            
            if (!emprunt.Usager.PeutEmprunter)
                return BadRequest("Erreur de création : Cet usager a accumulé trop de défaillances.");
            
            if (emprunt.Livre.Quantite == 0)
                return BadRequest("Erreur de création : Le livre n'est plus disponible.");

            if (!ModelState.IsValid)
                return BadRequest("Erreur de création : L'emprunt n'est pas conforme.");
            
            await _crudService.AjouterEmprunt(emprunt);
            return CreatedAtAction(nameof(Post), new { id = emprunt.Id }, emprunt);
        }

        // PUT api/<EmpruntController>/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Emprunt>> Put(int id, [FromBody] Emprunt emprunt)//, bool retard = false)
        {
            if (emprunt == null || emprunt.Id != id)
                return BadRequest("Requête invalide : L'emprunt n'est pas le même que celui de l'identifiant/est nul.");

            if (!EmpruntExiste(emprunt))
                return NotFound();
            
            if (!ModelState.IsValid || emprunt.DateRetour == DateTime.MinValue) 
                return BadRequest("Requête invalide : L'emprunt n'est pas conforme.");

            await _crudService.ModifierEmprunt(emprunt);//, retard);
            
            return NoContent();

        }

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
        
        private bool EmpruntExiste(Emprunt emprunt)
        {
            var empruntDansRepo = _crudService.ObtenirEmpruntParId(emprunt.Id).Result;

            return empruntDansRepo != null;
        }
    }
}
