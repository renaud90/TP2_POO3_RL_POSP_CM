using Bibliotheques.ApplicationCore.Entites;
using Bibliotheques.ApplicationCore.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheques.MVC.Proxies
{
    public class BibliothequeServiceProxy : IBibliothequeService
    {
        private readonly HttpClient _httpClient;
        private const string _empruntsApiUrl = "api/Emprunts/";
        private const string _livresApiUrl = "api/Livres/";
        private const string _usagersApiUrl = "api/Usagers/";

        public BibliothequeServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Emprunt> ObtenirEmpruntParId(int id) {
            return await _httpClient.GetFromJsonAsync<Emprunt>(_empruntsApiUrl + id);
        }
        
        public async Task<IEnumerable<Emprunt>> ObtenirTousLesEmprunts() {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Emprunt>>(_empruntsApiUrl);
        }
        
        public async Task AjouterEmprunt(Emprunt emprunt) {

            StringContent content = new StringContent(JsonConvert.SerializeObject(emprunt), Encoding.UTF8, "application/json");

            await _httpClient.PostAsync(_empruntsApiUrl, content);
        }
        public async Task ModifierEmprunt(Emprunt emprunt) {
            StringContent content = new StringContent(JsonConvert.SerializeObject(emprunt), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync(_empruntsApiUrl, content);
        }
        public async Task EffacerEmprunt(int id) {
            await _httpClient.DeleteAsync(_empruntsApiUrl + id);
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
