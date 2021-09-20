using System;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Bibliotheques.MVC.Data;
using Bibliotheques.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bibliotheques.MVC.Services;

namespace Bibliotheques.MVC.Controllers
{
    public class UsagersController : Controller
    {
        private readonly BibliothequeContext _context;
        private readonly IConfiguration _config;
        private readonly IGenerateurCodeUsager _generateurCodeUsager;

        public UsagersController(BibliothequeContext context, IConfiguration config, IGenerateurCodeUsager generateurCodeUsager)
        {
            _context = context;
            _config = config;
            _generateurCodeUsager = generateurCodeUsager;
        }

        // GET: Usagers
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["StatutSortParm"] = sortOrder == "statut" ? "statut_desc" : "statut";
            ViewData["DefaillanceSortParm"] = sortOrder == "def" ? "def_desc" : "def";
            ViewData["NumSortParm"] = sortOrder == "num" ? "num_desc" : "num";
            ViewData["CurrentFilter"] = searchString;

            var usagers = from s in _context.Usagers
                           select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchStringToUpper = searchString.ToUpper();
                usagers = usagers.Where(s => s.Nom.ToUpper().Contains(searchStringToUpper)
                                             || s.Prenom.ToUpper().Contains(searchStringToUpper) 
                                             || s.NumAbonne.ToUpper().Contains(searchStringToUpper));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    usagers = usagers.OrderByDescending(s => s.Nom);
                    break;
                case "statut":
                    usagers = usagers.OrderBy(s => s.Statut);
                    break;
                case "statut_desc":
                    usagers = usagers.OrderByDescending(s => s.Statut);
                    break;
                case "def_desc":
                    usagers = usagers.OrderByDescending(s => s.Defaillance);
                    break;
                case "def":
                    usagers = usagers.OrderBy(s => s.Defaillance);
                    break;
                case "num_desc":
                    usagers = usagers.OrderByDescending(s => s.NumAbonne);
                    break;
                case "num":
                    usagers = usagers.OrderBy(s => s.NumAbonne);
                    break;
                default:
                    usagers = usagers.OrderBy(s => s.Nom);
                    break;
            }
            return View(await usagers.ToListAsync());
        }

        // GET: Usagers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.JoursLocation = _config.GetValue<int>("Bibliotheque:JoursEmprunt");

            var usager = await _context.Usagers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usager == null)
            {
                return NotFound();
            }

            return View(usager);
        }

        // GET: Usagers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom,Prenom,Statut,Email")] Usager usager)
        {
            var codeUsager = _generateurCodeUsager.GenererCodeUsager(usager);
            
            usager.NumAbonne = codeUsager;
            
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(usager);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Échoue de la savegarde des données. Veuillez réessayer, si le probleme persiste " +
                    "contacter l'administrateur de votre système.");
            }

            return View(usager);
        }

        // GET: Usagers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var usager = await _context.Usagers.FindAsync(id);
            if (usager == null)
            {
                return NotFound();
            }

            return View(usager);
        }

        // POST: Usagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumAbonne,Nom,Prenom,Statut,Email")] Usager usager)
        {
            if (id != usager.Id)
            {
                return NotFound();
            }

            usager.NumAbonne = _generateurCodeUsager.GenererCodeUsager(usager);
                  
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(usager).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsagerExists(usager.Id))
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
            
            return View(usager);
        }


        // GET: Usagers/Delete/5
        public async Task<IActionResult> Delete(int id, bool? saveChangesError = false)
        {
            var usager = await _context.Usagers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usager == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Échoue de la supression. Veuillez réessayer, si le probleme persiste " +
                    "contacter l'administrateur de votre système.";
            }

            return View(usager);
        }

        // POST: Usagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usager = await _context.Usagers.FindAsync(id);

            if(usager == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Usagers.Remove(usager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
        
        private bool UsagerExists(int id)
        {
            return _context.Usagers.Any(e => e.Id == id);
        }
        
    }
}
