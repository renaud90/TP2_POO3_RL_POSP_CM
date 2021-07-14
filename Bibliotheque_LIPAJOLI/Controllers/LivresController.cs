﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bibliotheque_LIPAJOLI.Data;
using Bibliotheque_LIPAJOLI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bibliotheque_LIPAJOLI.Controllers
{
    public class LivresController : Controller
    {
        private readonly BibliothequeContext _context;
        private readonly IConfiguration _config;

        public LivresController(BibliothequeContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: Livres
        public async Task<IActionResult> Index()
        {
            return View(await _context.Livres.ToListAsync());
        }

        // GET: Livres/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livre = await _context.Livres
                .FirstOrDefaultAsync(m => m.CodeLivre == id);
            if (livre == null)
            {
                return NotFound();
            }

            return View(livre);
        }

        // GET: Livres/Create
        public IActionResult Create()
        {
            ViewBag.categories = _config.GetSection("Bibliotheque:Categories").Get<List<string>>();
            ViewBag.auteurs = _config.GetSection("Bibliotheque:Auteurs").Get<List<string>>();
            return View();
        }

        // POST: Livres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Isbn10,Isbn13,Titre,Quantite,Prix")] Livre livre, string categorie, string[] auteurs)
        {
            if (_context.Livres.FirstOrDefault(_ => _.CodeLivre.Substring(0, 3).ToUpper() == categorie.Substring(0, 3).ToUpper()) == null)
            {
                livre.CodeLivre = categorie.Substring(0, 3).ToUpper() + "001";
            }
            else
            {
                var valeurMaxCategorie = _context.Livres.Where(_ => _.Categorie == categorie)
                                                        .Select(_ => Int32.Parse(_.CodeLivre.Substring(4, 3)))
                                                        .ToList()
                                                        .Max();
                var nouvelleValeurMaxCategorie = (valeurMaxCategorie + 1).ToString();
                if(nouvelleValeurMaxCategorie.Length == 1)
                {
                    nouvelleValeurMaxCategorie = "00" + nouvelleValeurMaxCategorie;
                }else if(nouvelleValeurMaxCategorie.Length == 2)
                {
                    nouvelleValeurMaxCategorie = "0" + nouvelleValeurMaxCategorie;
                }
                livre.CodeLivre = categorie.Substring(0, 3).ToUpper() + nouvelleValeurMaxCategorie;
            }
            if(categorie == null)
            {
                ModelState.AddModelError("Categorie", "Ce champ est requis.");
                return View(livre);
            }
            if (auteurs.Length == 0)
            {
                ModelState.AddModelError("Auteurs", "Ce champ est requis.");
                return View(livre);
            }
            livre.Categorie = categorie;
            foreach(var auteur in auteurs)
            {
                livre.Auteurs += auteur;
                livre.Auteurs += ",";
            }
            //Enlever la virgule à la fin
            livre.Auteurs = livre.Auteurs.Remove(livre.Auteurs.Length - 1);

            
            if (ModelState.IsValid)
            {
                _context.Add(livre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.categories = _config.GetSection("Bibliotheque:Categories").Get<List<string>>();
            ViewBag.auteurs = _config.GetSection("Bibliotheque:Auteurs").Get<List<string>>();
            return View(livre);
        }

        // GET: Livres/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livre = await _context.Livres.FindAsync(id);
            if (livre == null)
            {
                return NotFound();
            }
            return View(livre);
        }

        // POST: Livres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CodeLivre,Isbn10,Isbn13,Titre,Categorie,Quantite,Prix,Auteurs")] Livre livre)
        {
            if (id != livre.CodeLivre)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivreExists(livre.CodeLivre))
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
            return View(livre);
        }

        // GET: Livres/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livre = await _context.Livres
                .FirstOrDefaultAsync(m => m.CodeLivre == id);
            if (livre == null)
            {
                return NotFound();
            }

            return View(livre);
        }

        // POST: Livres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var livre = await _context.Livres.FindAsync(id);
            _context.Livres.Remove(livre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LivreExists(string id)
        {
            return _context.Livres.Any(e => e.CodeLivre == id);
        }
    }
}
