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
        private const string _bibliothequeApiUrl = "api/Emprunt";

        public BibliothequeServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Emprunt> ObtenirEmpruntParId(int id) {
            return await _httpClient.GetFromJsonAsync<Emprunt>(_bibliothequeApiUrl + id);
        }
        public async Task<IEnumerable<Emprunt>> ObtenirTousLesEmprunts() {
            return await _httpClient.GetFromJsonAsync<List<Emprunt>>(_bibliothequeApiUrl);
        }
        //Task<IEnumerable<Emprunt>> ObtenirListeEmprunts(Expression<Func<Emprunt, bool>> predicat) { }
        public async Task AjouterEmprunt(Emprunt emprunt) {

            StringContent content = new StringContent(JsonConvert.SerializeObject(emprunt), Encoding.UTF8, "application/json");

            await _httpClient.PostAsync(_bibliothequeApiUrl, content);
        }
        public async Task ModifierEmprunt(Emprunt emprunt) {
            StringContent content = new StringContent(JsonConvert.SerializeObject(emprunt), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync(_bibliothequeApiUrl, content);
        }
        public async Task EffacerEmprunt(int id) {
            await _httpClient.DeleteAsync(_bibliothequeApiUrl + id);
        }
    }
}
