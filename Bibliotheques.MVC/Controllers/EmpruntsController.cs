using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bibliotheques.MVC.Models;
using Bibliotheques.MVC.Proxies;
using Microsoft.Extensions.Configuration;
using System;

namespace Bibliotheques.MVC.Controllers
{
    public class EmpruntsController : Controller
    {
        private readonly IBibliothequeService _bibliothequeProxy;
        private readonly IConfiguration _config;

        public EmpruntsController(IBibliothequeService bibliothequeProxy, IConfiguration config)
        {
            _bibliothequeProxy = bibliothequeProxy;
            _config = config;
        }

        // GET: Emprunts
        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.JoursLocation = _config.GetValue<int>("Bibliotheque:JoursEmprunt");

            ViewData["DateRetourSortParam"] = string.IsNullOrEmpty(sortOrder) ? "dateRetour_desc" : "";
            ViewData["TitreLivreSortParam"] = sortOrder == "livreTitre" ? "livreTitre_desc" : "livreTitre";
            ViewData["DateEmpruntSortParam"] = sortOrder == "dateEmprunt" ? "dateEmprunt_desc" : "dateEmprunt";
            ViewData["NomAbonneSortParam"] = sortOrder == "nomAbonne" ? "nomAbonne_desc" : "nomAbonne";
            ViewData["CurrentFilter"] = searchString;

            var emprunts = await _bibliothequeProxy.ObtenirTousLesEmprunts();

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchStringToUpper = searchString.ToUpper();
                emprunts = emprunts.Where(s => s.Usager.Nom.ToUpper().Contains(searchStringToUpper)
                                             || s.Usager.Prenom.ToUpper().Contains(searchStringToUpper)
                                             || s.Usager.NumAbonne.ToUpper().Contains(searchStringToUpper)
                                             || s.Livre.Titre.ToUpper().Contains(searchStringToUpper)
                                             || s.Livre.Isbn13.ToUpper().Contains(searchStringToUpper)
                                             || s.Livre.Isbn10.ToUpper().Contains(searchStringToUpper));
            }

            switch (sortOrder)
            {
                case "livreTitre_desc":
                    emprunts = emprunts.OrderByDescending(s => s.Livre.Titre);
                    break;
                case "livreTitre":
                    emprunts = emprunts.OrderBy(s => s.Livre.Titre);
                    break;
                case "dateRetour_desc":
                    emprunts = emprunts.OrderByDescending(s => s.DateRetour);
                    break;
                case "dateEmprunt_desc":
                    emprunts = emprunts.OrderByDescending(s => s.DateEmprunt);
                    break;
                case "dateEmprunt":
                    emprunts = emprunts.OrderBy(s => s.DateEmprunt);
                    break;
                case "nomAbonne_desc":
                    emprunts = emprunts.OrderByDescending(s => s.Usager.Nom);
                    break;
                case "nomAbonne":
                    emprunts = emprunts.OrderBy(s => s.Usager.Nom);
                    break;
                default:
                    emprunts = emprunts.OrderBy(s => s.DateRetour);
                    break;
            }
            return View(emprunts);
        }

        // GET: Emprunts/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.JoursLocation = _config.GetValue<int>("Bibliotheque:JoursEmprunt");

            var emprunt = await _bibliothequeProxy.ObtenirEmpruntParId(id);
            if (emprunt == null)
            {
                return NotFound();
            }

            return View(emprunt);
        }

        // GET: Emprunts/Create
        [HttpGet]
        public async Task<IActionResult> Create(string? erreurLivre, string? erreurUsager, int? userId, int? livreId)
        {
            ViewBag.Livres = (await _bibliothequeProxy.ObtenirTousLesLivres()).ToList().OrderBy(_ => _.CodeLivre);
            ViewBag.Usagers = (await _bibliothequeProxy.ObtenirTousLesUsagers()).ToList().OrderBy(_ => _.NumAbonne);
            ViewBag.ErreurLivre = erreurLivre;
            ViewBag.ErreurUsager = erreurUsager;
            ViewBag.livreId = livreId;
            ViewBag.userId = userId;
            return View();
        }

        // POST: Emprunts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int LivreId, int UsagerId)
        {
            var usager = (await _bibliothequeProxy.ObtenirTousLesUsagers()).FirstOrDefault(_ => _.Id == UsagerId);
            var livre = (await _bibliothequeProxy.ObtenirTousLesLivres()).FirstOrDefault(_ => _.Id == LivreId);
            if(usager.Emprunts != null)
            {
                foreach (var e in usager.Emprunts)
                {
                    if (e.LivreId == LivreId || e.DateRetour == DateTime.MinValue)
                    {
                        return RedirectToAction(nameof(Create), new {erreurLivre= "Ce livre est déjà prêté à cet usager.", erreurUsager="Cet usager a déjà ce livre en sa possession.", userId=UsagerId, livreId=LivreId });
                    }
                }
            }
            
            var emprunt = new Emprunt() { LivreId = LivreId, UsagerId = UsagerId, DateEmprunt = DateTime.Now, DateRetour = DateTime.MinValue };
            try
            {
                await _bibliothequeProxy.AjouterEmprunt(emprunt);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Échoue de la savegarde des données. Veuillez réessayer, si le probleme persiste " +
                    "contacter l'administrateur de votre système.");
            }

            ViewData["LivreId"] = new SelectList(await _bibliothequeProxy.ObtenirTousLesLivres(), "Id", "Categorie", emprunt.LivreId);
            ViewData["UsagerId"] = new SelectList(await _bibliothequeProxy.ObtenirTousLesUsagers(), "Id", "Email", emprunt.UsagerId);
            return View(emprunt);
        }

        // GET: Emprunts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.JoursLocation = _config.GetValue<int>("Bibliotheque:JoursEmprunt");

            var emprunt = await _bibliothequeProxy.ObtenirEmpruntParId(id);

            //var emprunt = await _bibliothequeProxy.ObtenirEmpruntParId(id);
            if (emprunt == null)
            {
                return NotFound();
            }

            ViewData["LivreId"] = new SelectList(await _bibliothequeProxy.ObtenirTousLesLivres(), "Id", "Categorie", emprunt.LivreId);
            ViewData["UsagerId"] = new SelectList(await _bibliothequeProxy.ObtenirTousLesUsagers(), "Id", "Email", emprunt.UsagerId);
            ViewBag.NbJoursLimite = Int32.Parse(_config.GetSection("Bibliotheque").GetSection("JoursEmprunt").Value);
            return View(emprunt);
        }

        // POST: Emprunts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LivreId,UsagerId,DateEmprunt,DateRetour,Id")] Emprunt emprunt)
        {
            if (id != emprunt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _bibliothequeProxy.ModifierEmprunt(emprunt);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpruntExists(emprunt.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LivreId"] = new SelectList(await _bibliothequeProxy.ObtenirTousLesLivres(), "Id", "Titre", emprunt.LivreId);
            ViewData["UsagerId"] = new SelectList(await _bibliothequeProxy.ObtenirTousLesUsagers(), "Id", "Nom", emprunt.UsagerId);
            return View(emprunt);
        }

        // GET: Emprunts/Delete/5
        public async Task<IActionResult> Delete(int id, bool? saveChangesError = false)
        {
            ViewBag.JoursLocation = _config.GetValue<int>("Bibliotheque:JoursEmprunt");

            var emprunt = await _bibliothequeProxy.ObtenirEmpruntParId(id);
            if (emprunt == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Échoue de la supression. Veuillez réessayer, si le probleme persiste " +
                    "contacter l'administrateur de votre système.";
            }

            return View(emprunt);
        }

        // POST: Emprunts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emprunt = await _bibliothequeProxy.ObtenirEmpruntParId(id);

            if (emprunt == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                await _bibliothequeProxy.EffacerEmprunt(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }

        }

        private bool EmpruntExists(int id)
        {
            return _bibliothequeProxy.ObtenirEmpruntParId(id) != null;
        }
    }
}
