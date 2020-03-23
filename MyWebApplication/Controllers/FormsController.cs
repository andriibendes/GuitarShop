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
    public class FormsController : Controller
    {
        private readonly guitar_shopContext _context;

        public FormsController(guitar_shopContext context)
        {
            _context = context;
        }

        public IActionResult VerifyName(string name)
        {
            var forms = (from f in _context.Forms where f.Name == name select f).ToList();
            if (forms.Count > 0)
            {
                return Json($"Name {name} is already exists.");
            }
            else
            {
                return Json(true);
            }
        }

        // GET: Forms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Forms.ToListAsync());
        }

        // GET: Forms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forms = await _context.Forms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (forms == null)
            {
                return NotFound();
            }

            return View(forms);
        }

        // GET: Forms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Forms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Forms forms)
        {
            if (ModelState.IsValid)
            {
                _context.Add(forms);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(forms);
        }

        // GET: Forms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forms = await _context.Forms.FindAsync(id);
            if (forms == null)
            {
                return NotFound();
            }
            return View(forms);
        }

        // POST: Forms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Forms forms)
        {
            if (id != forms.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forms);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FormsExists(forms.Id))
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
            return View(forms);
        }

        // GET: Forms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forms = await _context.Forms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (forms == null)
            {
                return NotFound();
            }

            return View(forms);
        }

        // POST: Forms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var forms = await _context.Forms.FindAsync(id);
            _context.Forms.Remove(forms);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FormsExists(int id)
        {
            return _context.Forms.Any(e => e.Id == id);
        }
    }
}
