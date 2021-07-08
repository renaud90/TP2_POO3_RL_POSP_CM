﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bibliotheque_LIPAJOLI.Data;
using Bibliotheque_LIPAJOLI.Models;

namespace Bibliotheque_LIPAJOLI.Controllers
{
    public class UsagersController : Controller
    {
        private readonly BibliothequeContext _context;

        public UsagersController(BibliothequeContext context)
        {
            _context = context;
        }

        // GET: Usagers
        //public async Task<IActionResult> Index()
        //{
        //    var usagers = _context.Usagers
        //         .Include(c => c.Emprunts)
        //         .AsNoTracking();
        //    return View(await usagers.ToListAsync());
        //}
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["StatutSortParm"] = sortOrder == "statut" ? "statut_desc" : "statut";
            ViewData["DefaillanceSortParm"] = sortOrder == "def" ? "def_desc" : "def";
            ViewData["CurrentFilter"] = searchString;

            var students = from s in _context.Usagers
                           select s;


            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Nom.Contains(searchString)
                                       || s.Prenom.Contains(searchString) || s.Nom.ToLower().Contains(searchString) || s.Prenom.ToLower().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.Nom);
                    break;
                case "statut":
                    students = students.OrderBy(s => s.Statut);
                    break;
                case "statut_desc":
                    students = students.OrderByDescending(s => s.Statut);
                    break;
                case "def_desc":
                    students = students.OrderByDescending(s => s.Defaillance);
                    break;
                case "def":
                    students = students.OrderBy(s => s.Defaillance);
                    break;
                default:
                    students = students.OrderBy(s => s.Nom);
                    break;
            }
            return View(await students.AsNoTracking().ToListAsync());
        }

        // GET: Usagers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usager = await _context.Usagers
                .Include(c => c.Emprunts)
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
        public async Task<IActionResult> Create([Bind("NumAbonne,Nom,Prenom,Statut,Email,Defaillance")] Usager usager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usager = await _context.Usagers
                .FirstOrDefaultAsync(m => m.NumAbonne == id);
            if (usager == null)
            {
                return NotFound();
            }

            return View(usager);
        }

        // POST: Usagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var usager = await _context.Usagers.FindAsync(id);
            _context.Usagers.Remove(usager);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsagerExists(string id)
        {
            return _context.Usagers.Any(e => e.NumAbonne == id);
        }
    }
}