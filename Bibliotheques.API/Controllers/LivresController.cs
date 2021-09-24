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

        /// <summary>
        /// Premet l'obtention et le retour d'une liste de tous les livres de la Bibliothèque Lipajoli
        /// </summary>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="200">Liste complète des livres de la bibliothèque Lipajoli trouvée et retournée</response>
        /// <response code="404">Liste complète des livres de la bibliothèque Lipajoli introuvable</response>
        /// <response code="500">Oups! Le service demandé est indisponible pour le moment</response>
        // GET: api/Livres
        [HttpGet]
        public async Task<IEnumerable<Livre>> Get()
        {
            return await _crudService.ObtenirTousLesLivres();
        }

        /// <summary>
        /// Premet l'obtention et le retour des informations d'un livre spécifique, ciblé par l'id passé en paramètre
        /// </summary>
        /// <param name="id">id du livre à retourner</param>
        /// <returns></returns>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="200">Le livre spécifié a été trouvé et retourné</response>
        /// <response code="404">Livre introuvable pour l'id specifié</response>
        /// <response code="500">Oups! Le service demandé est indisponible pour le moment</response>
        // GET: api/Livres/5
        [HttpGet("{id:int}", Name = "Get")]
        public async Task<Livre> Get(int id)
        {
            var livres = await _crudService.ObtenirTousLesLivres();
            return livres.FirstOrDefault(_ => _.Id == id);
        }

    }
}
