﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebApplication;
using Microsoft.AspNetCore.Authorization;

namespace MyWebApplication.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class MaterialsController : Controller
    {
        private readonly guitar_shopContext _context;

        public MaterialsController(guitar_shopContext context)
        {
            _context = context;
        }

        public IActionResult VerifyName(string name)
        {
            var materials = (from m in _context.Materials where m.Name == name select m).ToList();
            if (materials.Count > 0)
            {
                return Json($"Name {name} is already exists.");
            }
            else
            {
                return Json(true);
            }
        }

        // GET: Materials
        public async Task<IActionResult> Index()
        {
            return View(await _context.Materials.ToListAsync());
        }

        // GET: Materials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materials = await _context.Materials
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materials == null)
            {
                return NotFound();
            }

            return View(materials);
        }

        // GET: Materials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Materials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Materials materials)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materials);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(materials);
        }

        // GET: Materials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materials = await _context.Materials.FindAsync(id);
            if (materials == null)
            {
                return NotFound();
            }
            return View(materials);
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Materials materials)
        {
            if (id != materials.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materials);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialsExists(materials.Id))
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
            return View(materials);
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materials = await _context.Materials
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materials == null)
            {
                return NotFound();
            }

            return View(materials);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materials = await _context.Materials.FindAsync(id);
            _context.Materials.Remove(materials);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialsExists(int id)
        {
            return _context.Materials.Any(e => e.Id == id);
        }
    }
}
