using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bibliotheques.ApplicationCore.Entites;
using Bibliotheques.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Bibliotheques.MVC.Services;

namespace Bibliotheques.MVC.Controllers
{
    public class EmpruntsController : Controller
    {
        //private readonly IBibliothequeService _bibliothequeProxy;
        private readonly BibliothequeContext _context;
        private readonly IConfiguration _config;
        private readonly IGenerateurCodeUsager _generateurCodeUsager;

        public EmpruntsController(BibliothequeContext context, IConfiguration config, IGenerateurCodeUsager generateurCodeUsager)
        {
            //_bibliothequeProxy = bibliothequeService;
            _context = context;
            _config = config;
            _generateurCodeUsager = generateurCodeUsager;
        }

        // GET: Emprunts
        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.JoursLocation = _config.GetValue<int>("Bibliotheque:JoursEmprunt");

            ViewData["DateRetourSortParam"] = String.IsNullOrEmpty(sortOrder) ? "dateRetour_desc" : "";
            ViewData["TitreLivreSortParam"] = sortOrder == "livreTitre" ? "livreTitre_desc" : "livreTitre";
            ViewData["DateEmpruntSortParam"] = sortOrder == "dateEmprunt" ? "dateEmprunt_desc" : "dateEmprunt";
            ViewData["NomAbonneSortParam"] = sortOrder == "nomAbonne" ? "nomAbonne_desc" : "nomAbonne";
            ViewData["CurrentFilter"] = searchString;

            var emprunt = from s in _context.Emprunts
                          select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchStringToUpper = searchString.ToUpper();
                emprunt = emprunt.Where(s => s.Usager.Nom.ToUpper().Contains(searchStringToUpper)
                                             || s.Usager.Prenom.ToUpper().Contains(searchStringToUpper)
                                             || s.Usager.NumAbonne.ToUpper().Contains(searchStringToUpper)
                                             || s.Livre.Titre.ToUpper().Contains(searchStringToUpper)
                                             || s.Livre.Isbn13.ToUpper().Contains(searchStringToUpper)
                                             || s.Livre.Isbn10.ToUpper().Contains(searchStringToUpper));
            }

            switch (sortOrder)
            {
                case "livreTitre_desc":
                    emprunt = emprunt.OrderByDescending(s => s.Livre.Titre);
                    break;
                case "livreTitre":
                    emprunt = emprunt.OrderBy(s => s.Livre.Titre);
                    break;
                case "dateRetour_desc":
                    emprunt = emprunt.OrderByDescending(s => s.DateRetour);
                    break;
                case "dateEmprunt_desc":
                    emprunt = emprunt.OrderByDescending(s => s.DateEmprunt);
                    break;
                case "dateEmprunt":
                    emprunt = emprunt.OrderBy(s => s.DateEmprunt);
                    break;
                case "nomAbonne_desc":
                    emprunt = emprunt.OrderByDescending(s => s.Usager.Nom);
                    break;
                case "nomAbonne":
                    emprunt = emprunt.OrderBy(s => s.Usager.Nom);
                    break;
                default:
                    emprunt = emprunt.OrderBy(s => s.DateRetour);
                    break;
            }
            return View(await emprunt.ToListAsync());
        }

        // GET: Emprunts/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.JoursLocation = _config.GetValue<int>("Bibliotheque:JoursEmprunt");

            var emprunt = await _context.Emprunts
                .Include(e => e.Livre)
                .Include(e => e.Usager)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emprunt == null)
            {
                return NotFound();
            }

            return View(emprunt);
        }

        // GET: Emprunts/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["LivreId"] = new SelectList(_context.Livres, "Id", "Categorie");
            ViewData["UsagerId"] = new SelectList(_context.Usagers, "Id", "Email");
            return View();
        }

        // POST: Emprunts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LivreId,UsagerId,DateEmprunt,DateRetour,Id")] Emprunt emprunt)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(emprunt);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Échoue de la savegarde des données. Veuillez réessayer, si le probleme persiste " +
                    "contacter l'administrateur de votre système.");
            }

            ViewData["LivreId"] = new SelectList(_context.Livres, "Id", "Categorie", emprunt.LivreId);
            ViewData["UsagerId"] = new SelectList(_context.Usagers, "Id", "Email", emprunt.UsagerId);
            return View(emprunt);
        }

        // GET: Emprunts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.JoursLocation = _config.GetValue<int>("Bibliotheque:JoursEmprunt");

            var emprunt = await _context.Emprunts.FindAsync(id);

            //var emprunt = await _bibliothequeProxy.ObtenirEmpruntParId(id);
            if (emprunt == null)
            {
                return NotFound();
            }

            ViewData["LivreId"] = new SelectList(_context.Livres, "Id", "Categorie", emprunt.LivreId);
            ViewData["UsagerId"] = new SelectList(_context.Usagers, "Id", "Email", emprunt.UsagerId);

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
                    _context.Entry(emprunt).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
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
            ViewData["LivreId"] = new SelectList(_context.Livres, "Id", "Titre", emprunt.LivreId);
            ViewData["UsagerId"] = new SelectList(_context.Usagers, "Id", "Nom", emprunt.UsagerId);
            return View(emprunt);
        }

        // GET: Emprunts/Delete/5
        public async Task<IActionResult> Delete(int id, bool? saveChangesError = false)
        {
            ViewBag.JoursLocation = _config.GetValue<int>("Bibliotheque:JoursEmprunt");

            var emprunt = await _context.Emprunts
                .Include(e => e.Livre)
                .Include(e => e.Usager)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var emprunt = await _context.Emprunts.FindAsync(id);

            if (emprunt == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Emprunts.Remove(emprunt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }

        }

        private bool EmpruntExists(int id)
        {
            return _context.Emprunts.Any(e => e.Id == id);
        }
    }
}
