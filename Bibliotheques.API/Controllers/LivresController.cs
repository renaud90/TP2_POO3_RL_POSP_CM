using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bibliotheques.ApplicationCore.Entites;
using Bibliotheques.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotheques.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivresController : ControllerBase
    {
        private readonly IBibliothequeService _crudService;

        public LivresController(IBibliothequeService crudService)
        {
            _crudService = crudService;
        }

        // GET: api/Livres
        [HttpGet]
        public async Task<IEnumerable<Livre>> Get()
        {
            return await _crudService.ObtenirTousLesLivres();
        }

        // GET: api/Livres/5
        [HttpGet("{id:int}", Name = "Get")]
        public async Task<Livre> Get(int id)
        {
            var livres = await _crudService.ObtenirTousLesLivres();
            return livres.FirstOrDefault(_ => _.Id == id);
        }

    }
}
