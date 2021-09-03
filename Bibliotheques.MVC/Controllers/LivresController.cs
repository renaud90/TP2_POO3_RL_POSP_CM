using System;
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
        public async Task<IActionResult> Index(string chaineDeRecherche, string champAFiltrer)
        {
            var categoriesFiltre = new List<string>() {"Auteur", "Categorie", "Titre"};

            ViewBag.ChaineDeRecherche = chaineDeRecherche;
            ViewBag.CategoriesFiltre = new SelectList(categoriesFiltre, champAFiltrer);

            var chaineDeRechercheMajuscules = chaineDeRecherche?.ToUpper();

            List<Livre> livresAAfficher;
            switch (champAFiltrer)
            {
                case "Titre":
                    livresAAfficher = await _context.Livres.Where(_ => _.Titre.ToUpper().Contains(chaineDeRechercheMajuscules)).ToListAsync();
                    break;
                case "Categorie":
                    livresAAfficher = await _context.Livres.Where(_ => _.Categorie.ToUpper().Contains(chaineDeRechercheMajuscules)).ToListAsync();
                    break;
                case "Auteur":
                    livresAAfficher = await _context.Livres.Where(_ => _.Auteurs.ToUpper().Contains(chaineDeRechercheMajuscules)).ToListAsync();
                    break;
                default:
                    livresAAfficher = await _context.Livres.ToListAsync();
                    break;
            }
                
            return View(livresAAfficher);
        }

        // GET: Livres/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livre = await _context.Livres
                .Include(l => l.Emprunts)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CodeLivre == id)
                ;
            if (livre == null)
            {
                return NotFound();
            }

            return View(livre);
        }

        // GET: Livres/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_config.GetSection("Bibliotheque:Categories").Get<List<string>>());
            ViewBag.Auteurs = new SelectList(_config.GetSection("Bibliotheque:Auteurs").Get<List<string>>());
            return View();
        }

        // POST: Livres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Isbn10,Isbn13,Titre,Categorie,Quantite,Prix")] Livre livre, string[] auteurs)
        {
            if (_context.Livres.FirstOrDefault(_ => _.Categorie == livre.Categorie) == null)
            {
                if (livre.Categorie.Contains("-"))
                {
                    var mots = livre.Categorie.Split("-");
                    livre.CodeLivre = (mots[0].Substring(0, 1) + mots[1].Substring(0, 2)).ToUpper() + "001";
                }else
                {
                    livre.CodeLivre = livre.Categorie.Substring(0, 3).ToUpper() + "001";
                }
            }
            else
            {
                var valeurMaxCategorie = _context.Livres.Where(_ => _.Categorie == livre.Categorie)
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

                if (livre.Categorie.Contains("-"))
                {
                    var mots = livre.Categorie.Split("-");
                    livre.CodeLivre = (mots[0].Substring(0, 1) + mots[1].Substring(0, 2)).ToUpper() + nouvelleValeurMaxCategorie;
                }
                else
                {
                    livre.CodeLivre = livre.Categorie.Substring(0, 3).ToUpper() + nouvelleValeurMaxCategorie;
                }
            }

            ViewBag.Categories = new SelectList(_config.GetSection("Bibliotheque:Categories").Get<List<string>>());
            ViewBag.Auteurs = new SelectList(_config.GetSection("Bibliotheque:Auteurs").Get<List<string>>());

            if (auteurs.Length == 0)
            {
                ModelState.AddModelError("Auteurs", "Ce champ est requis.");
                return View(livre);
            }

            foreach(var auteur in auteurs)
            {
                livre.Auteurs += auteur;
                livre.Auteurs += ", ";
            }
            //Enlever la virgule à la fin
            livre.Auteurs = livre.Auteurs.Remove(livre.Auteurs.Length - 2);

            if (ModelState.IsValid)
            {
                _context.Livres.Add(livre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

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
            
            ViewBag.Categories = new SelectList(_config.GetSection("Bibliotheque:Categories").Get<List<string>>());
            ViewBag.Auteurs = new SelectList(_config.GetSection("Bibliotheque:Auteurs").Get<List<string>>());
            
            return View(livre);
        }

        // POST: Livres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CodeLivre,Isbn10,Isbn13,Titre,Categorie,Quantite,Prix")] Livre livre, string[] auteurs)
        {
            if (id != livre.CodeLivre)
            {
                return NotFound();
            }
            
            ViewBag.Categories = new SelectList(_config.GetSection("Bibliotheque:Categories").Get<List<string>>());
            ViewBag.Auteurs = new SelectList(_config.GetSection("Bibliotheque:Auteurs").Get<List<string>>());
            
            if (auteurs.Length == 0)
            {
                ModelState.AddModelError("Auteurs", "Ce champ est requis.");
                return View(livre);
            }

            //On vide le champ avant de le renseigner avec les changements
            livre.Auteurs = "";
            foreach (var auteur in auteurs)
            {
                livre.Auteurs += auteur;
                livre.Auteurs += ", ";
            }
            //Enlever la virgule à la fin
            livre.Auteurs = livre.Auteurs.Remove(livre.Auteurs.Length - 2);

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
