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
        /// Premet l'obtention et le retour d'une liste de tous les livres de la Biblioth�que Lipajoli
        /// </summary>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="200">Liste compl�te des livres de la biblioth�que Lipajoli trouv�e et retourn�e</response>
        /// <response code="404">Liste compl�te des livres de la biblioth�que Lipajoli introuvable</response>
        /// <response code="500">Oups! Le service demand� est indisponible pour le moment</response>
        // GET: api/Livres
        [HttpGet]
        public async Task<IEnumerable<Livre>> Get()
        {
            return await _crudService.ObtenirTousLesLivres();
        }

        /// <summary>
        /// Premet l'obtention et le retour des informations d'un livre sp�cifique, cibl� par l'id pass� en param�tre
        /// </summary>
        /// <param name="id">id du livre � retourner</param>
        /// <returns></returns>
        /// <remarks>Pas de remarques</remarks>  
        /// <response code="200">Le livre sp�cifi� a �t� trouv� et retourn�</response>
        /// <response code="404">Livre introuvable pour l'id specifi�</response>
        /// <response code="500">Oups! Le service demand� est indisponible pour le moment</response>
        // GET: api/Livres/5
        [HttpGet("{id:int}", Name = "Get")]
        public async Task<Livre> Get(int id)
        {
            var livres = await _crudService.ObtenirTousLesLivres();
            return livres.FirstOrDefault(_ => _.Id == id);
        }

    }
}
