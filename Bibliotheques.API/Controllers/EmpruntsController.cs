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
        [HttpGet("{id}")]
        public async Task<Emprunt> Get(int id)
        {
            return await _crudService.ObtenirEmpruntParId(id);
        }

        // POST api/<EmpruntController>
        [HttpPost]
        public async Task<ActionResult<Emprunt>> Post([FromBody] Emprunt emprunt)
        {
            await _crudService.AjouterEmprunt(emprunt);
            return CreatedAtAction(nameof(Post), new { id = emprunt.Id}, emprunt);
        }

        // PUT api/<EmpruntController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Emprunt>> Put(int id, [FromBody] Emprunt emprunt)
        {
            await _crudService.ModifierEmprunt(emprunt);
            return NoContent();
        }

        // DELETE api/<EmpruntController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Emprunt>> Delete(int id)
        {
            await _crudService.EffacerEmprunt(id);
            return NoContent();
        }
    }
}
