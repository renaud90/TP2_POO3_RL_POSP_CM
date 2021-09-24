using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bibliotheques.ApplicationCore.Entites;
using Bibliotheques.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotheques.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsagersController : ControllerBase
    {
        private readonly IBibliothequeService _crudService;

        public UsagersController(IBibliothequeService crudService)
        {
            _crudService = crudService;
        }

        /// <summary>
        /// Premet l'obtention et le retour d'une liste de tous les usagers de la Bibliothèque Lipajoli
        /// </summary>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="200">Liste complète des usagers de la bibliothèque Lipajoli trouvée et retournée</response>
        /// <response code="404">Liste complète des usagers de la bibliothèque Lipajoli introuvable</response>
        /// <response code="500">Oups! Le service demandé est indisponible pour le moment</response>
        // GET: api/Usagers
        [HttpGet]
        public async Task<IEnumerable<Usager>> Get()
        {
            return await _crudService.ObtenirTousLesUsagers();
        }

        /// <summary>
        /// Premet l'obtention et le retour des informations d'un usager spécifique, ciblé par l'id passé en paramètre
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="200">L'usager spécifié a été trouvé et retourné</response>
        /// <response code="404">Usager introuvable pour l'id specifié</response>
        /// <response code="500">Oups! Le service demandé est indisponible pour le moment</response>
        [HttpGet("{id:int}")]
        public async Task<Usager> Get(int id)
        {
            var usagers = await _crudService.ObtenirTousLesUsagers();
            return usagers.FirstOrDefault(_ => _.Id == id);
        }
    }
}