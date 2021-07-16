﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bibliotheque_LIPAJOLI.Data;
using Bibliotheque_LIPAJOLI.Extensions;
using Bibliotheque_LIPAJOLI.Models;

namespace Bibliotheque_LIPAJOLI.Controllers
{
    public class UsagersController : Controller
    {
        private readonly BibliothequeContext _context;
        private readonly IConfiguration _config;

        public UsagersController(BibliothequeContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
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
            return View(await usagers.AsNoTracking().ToListAsync());
        }

        // GET: Usagers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.JoursLocation = _config.GetValue<int>("Bibliotheque:JoursEmprunt");

            var usager = await _context.Usagers
                .Include(c => c.Emprunts)
                .ThenInclude(e =>e.Livre)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.NumAbonne == id);

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
            CreerCodeUsager(usager);

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
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Edit(string id, [Bind("NumAbonne,Nom,Prenom,Statut,Email,Defaillance")] Usager usager)
        {
            if (id != usager.NumAbonne)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsagerExists(usager.NumAbonne))
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
        public async Task<IActionResult> Delete(string id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usager = await _context.Usagers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.NumAbonne == id);

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
        public async Task<IActionResult> DeleteConfirmed(string id)
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
        
        private bool UsagerExists(string id)
        {
            return _context.Usagers.Any(e => e.NumAbonne == id);
        }

        private string ObtenirLettresCodeUsager(Usager usager)
        {
            string codeLettresPrenom = CodeLettresNom(usager.Prenom);
            string codeLettresNom = CodeLettresNom(usager.Nom);

            string codeLettres = codeLettresNom + codeLettresPrenom;

            return codeLettres.ToUpper().EnleverSymbolesDiacritiques();
        }
        
        private static string CodeLettresNom(string nom)
        {
            string codeLettresNom;
            if (nom.Contains('-'))
            {
                int indexTiret = nom.IndexOf("-");

                string codeNomCompose = nom.Substring(indexTiret + 1, 1);

                codeLettresNom = nom.Substring(0, 1) + codeNomCompose;
            }
            else
            {
                codeLettresNom = nom.Substring(0, 2);
            }

            return codeLettresNom;
        }

        private void CreerCodeUsager(Usager usager)
        {
            string codeFinal;
            var dernierUsager = _context.Usagers
                .ToList()
                .OrderByDescending(ObtenirValeurNumeroAbonne)
                .FirstOrDefault();

            if (dernierUsager == null)
            {
                codeFinal = ObtenirLettresCodeUsager(usager) + "0001";
            }
            else
            {
                int codeChiffres = ObtenirValeurNumeroAbonne(dernierUsager) + 1;

                string codeChiffresString = codeChiffres.ToString("D4"); 
                codeFinal = ObtenirLettresCodeUsager(usager) + codeChiffresString;
            }

            usager.NumAbonne = codeFinal;
        }
        
        private static int ObtenirValeurNumeroAbonne(Usager usager)
        {
            return int.Parse(usager.NumAbonne.Substring(4, 4));
        }


    }
}
