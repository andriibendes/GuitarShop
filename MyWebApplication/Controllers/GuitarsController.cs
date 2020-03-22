using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebApplication;

namespace MyWebApplication.Controllers
{
    public class GuitarsController : Controller
    {
        private readonly guitar_shopContext _context;

        public GuitarsController(guitar_shopContext context)
        {
            _context = context;
        }

        public IActionResult VerifyName(string name)
        {
            var guitars = (from g in _context.Guitars where g.Name == name select g).ToList();
            if (guitars.Count > 0)
            {
                return Json($"Name {name} is already exsists.");
            }
            else
            {
                return Json(true);
            }
        }

        // GET: Guitars
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null)
            {
                var guitar_shop = _context.Guitars.Include(g => g.Brand).Include(g => g.Form).Include(g => g.Material).Include(g => g.Type);
                return View(await guitar_shop.ToListAsync());
            }
            ViewBag.BrandId = id;
            ViewBag.BrandName = name;
            var guitar_shopContext = _context.Guitars.Where(g => g.BrandId == id).Include(g => g.Brand).Include(g => g.Form).Include(g => g.Material).Include(g => g.Type);
            return View(await guitar_shopContext.ToListAsync());
        }

        // GET: Guitars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guitars = await _context.Guitars
                .Include(g => g.Brand)
                .Include(g => g.Form)
                .Include(g => g.Material)
                .Include(g => g.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (guitars == null)
            {
                return NotFound();
            }

            return View(guitars);
        }

        // GET: Guitars/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            ViewData["FormId"] = new SelectList(_context.Forms, "Id", "Name");
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Name");
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Name");
            return View();
        }

        // POST: Guitars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Info,Cost,Year,FormId,MaterialId,TypeId,BrandId")] Guitars guitars)
        {
            if (ModelState.IsValid)
            {
                _context.Add(guitars);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", guitars.BrandId);
            ViewData["FormId"] = new SelectList(_context.Forms, "Id", "Name", guitars.FormId);
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Name", guitars.MaterialId);
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Name", guitars.TypeId);
            return View(guitars);
        }

        // GET: Guitars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guitars = await _context.Guitars.FindAsync(id);
            if (guitars == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", guitars.BrandId);
            ViewData["FormId"] = new SelectList(_context.Forms, "Id", "Name", guitars.FormId);
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Name", guitars.MaterialId);
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Name", guitars.TypeId);
            return View(guitars);
        }

        // POST: Guitars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Info,Cost,Year,FormId,MaterialId,TypeId,BrandId")] Guitars guitars)
        {
            if (id != guitars.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guitars);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuitarsExists(guitars.Id))
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", guitars.BrandId);
            ViewData["FormId"] = new SelectList(_context.Forms, "Id", "Name", guitars.FormId);
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Name", guitars.MaterialId);
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Name", guitars.TypeId);
            return View(guitars);
        }

        // GET: Guitars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guitars = await _context.Guitars
                .Include(g => g.Brand)
                .Include(g => g.Form)
                .Include(g => g.Material)
                .Include(g => g.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (guitars == null)
            {
                return NotFound();
            }

            return View(guitars);
        }

        // POST: Guitars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var guitars = await _context.Guitars.FindAsync(id);
            _context.Guitars.Remove(guitars);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuitarsExists(int id)
        {
            return _context.Guitars.Any(e => e.Id == id);
        }
    }
}
