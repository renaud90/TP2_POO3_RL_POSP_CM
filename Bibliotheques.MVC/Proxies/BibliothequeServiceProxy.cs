using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Bibliotheques.MVC.Models;
using Microsoft.Extensions.Configuration;


namespace Bibliotheques.MVC.Proxies
{
    public class BibliothequeServiceProxy : IBibliothequeService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private const string _empruntsApiUrl = "api/Emprunts/";
        private const string _livresApiUrl = "api/Livres/";
        private const string _usagersApiUrl = "api/Usagers/";

        public BibliothequeServiceProxy(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<Emprunt> ObtenirEmpruntParId(int id) 
        {
            return await _httpClient.GetFromJsonAsync<Emprunt>(_empruntsApiUrl + id);
        }
        
        public async Task<IEnumerable<Emprunt>> ObtenirTousLesEmprunts() 
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Emprunt>>(_empruntsApiUrl);
        }
        
        public async Task<HttpResponseMessage> AjouterEmprunt(Emprunt emprunt) {

            StringContent content = new StringContent(JsonConvert.SerializeObject(emprunt), Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync(_empruntsApiUrl, content);            
        }
        public async Task<HttpResponseMessage> ModifierEmprunt(Emprunt emprunt) 
        {
            var nbJoursEmprunts = _config.GetSection("Bibliotheque:JoursEmprunt").Get<int>();
            StringContent content = new StringContent(JsonConvert.SerializeObject(emprunt), Encoding.UTF8, "application/json");
            
            if (emprunt.DateEmprunt.AddDays(nbJoursEmprunts) >= DateTime.Today)
                return await _httpClient.PutAsync(_empruntsApiUrl + emprunt.Id, content);
            
            var query = new Dictionary<string, string> { ["retard"] = "true" }; 
            return await _httpClient.PutAsync(QueryHelpers.AddQueryString(_empruntsApiUrl + emprunt.Id, query) , content);

        }
        public async Task<HttpResponseMessage> EffacerEmprunt(int id) {
            return await _httpClient.DeleteAsync(_empruntsApiUrl + id);
        }
        
        public async Task<IEnumerable<Livre>> ObtenirTousLesLivres()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Livre>>(_livresApiUrl);
        }

        public async Task<IEnumerable<Usager>> ObtenirTousLesUsagers()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Usager>>(_usagersApiUrl);
        }
    }
}
