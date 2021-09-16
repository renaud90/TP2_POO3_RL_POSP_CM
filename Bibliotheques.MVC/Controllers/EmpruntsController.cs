using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bibliotheques.ApplicationCore.Entites;
using Bibliotheques.ApplicationCore.Interfaces;
using Bibliotheques.Infrastructure.Data;

namespace Bibliotheques.MVC.Controllers
{
    public class EmpruntsController : Controller
    {
        private readonly IBibliothequeService _bibliothequeProxy;
        private readonly BibliothequeContext _context;

        public EmpruntsController(IBibliothequeService bibliothequeService, BibliothequeContext context)
        {
            _bibliothequeProxy = bibliothequeService;
            _context = context;
        }

        // GET: Emprunts
        public async Task<IActionResult> Index()
        {
            return View((await _bibliothequeProxy.ObtenirTousLesEmprunts()).ToList());
        }

        // GET: Emprunts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var emprunt = await _bibliothequeProxy.ObtenirEmpruntParId(id);
            if (emprunt == null)
            {
                return NotFound();
            }

            return View(emprunt);
        }

        // GET: Emprunts/Create
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
            if (ModelState.IsValid)
            {
                await _bibliothequeProxy.AjouterEmprunt(emprunt);
                return RedirectToAction(nameof(Index));
            }
            ViewData["LivreId"] = new SelectList(_context.Livres, "Id", "CodeLivre", emprunt.LivreId);
            ViewData["UsagerId"] = new SelectList(_context.Usagers, "Id", "NumAbonne", emprunt.UsagerId);
            return View(emprunt);
        }

        // GET: Emprunts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            var emprunt = await _bibliothequeProxy.ObtenirEmpruntParId(id);
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
                    await _bibliothequeProxy.ModifierEmprunt(emprunt);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_bibliothequeProxy.ObtenirEmpruntParId(emprunt.Id) == null)
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
            ViewData["LivreId"] = new SelectList(_context.Livres, "Id", "Categorie", emprunt.LivreId);
            ViewData["UsagerId"] = new SelectList(_context.Usagers, "Id", "Email", emprunt.UsagerId);
            return View(emprunt);
        }

        // GET: Emprunts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var emprunt = await _bibliothequeProxy.ObtenirEmpruntParId(id);
            if (emprunt == null)
            {
                return NotFound();
            }

            return View(emprunt);
        }

        // POST: Emprunts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bibliothequeProxy.EffacerEmprunt(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
