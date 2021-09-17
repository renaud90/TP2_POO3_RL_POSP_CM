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

        [HttpGet]
        public async Task<IEnumerable<Usager>> Get()
        {
            return await _crudService.ObtenirTousLesUsagers();
        }
        
        [HttpGet("{id:int}")]
        public async Task<Usager> Get(int id)
        {
            var usagers = await _crudService.ObtenirTousLesUsagers();
            return usagers.FirstOrDefault(_ => _.Id == id);
        }
    }
}