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
    public class MusiciansController : Controller
    {
        private readonly guitar_shopContext _context;

        public MusiciansController(guitar_shopContext context)
        {
            _context = context;
        }

        // GET: Musicians
        public async Task<IActionResult> Index()
        {
            return View(await _context.Musicians.ToListAsync());
        }

        // GET: Musicians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicians = await _context.Musicians
                .FirstOrDefaultAsync(m => m.Id == id);
            if (musicians == null)
            {
                return NotFound();
            }

            //return View(musicians);
            return RedirectToAction("Index", "Guitars", new { id = musicians.Id, name = musicians.Name });
        }

        // GET: Musicians/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Musicians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Musicians musicians)
        {
            if (ModelState.IsValid)
            {
                _context.Add(musicians);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(musicians);
        }

        // GET: Musicians/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicians = await _context.Musicians.FindAsync(id);
            if (musicians == null)
            {
                return NotFound();
            }
            return View(musicians);
        }

        // POST: Musicians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Musicians musicians)
        {
            if (id != musicians.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(musicians);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusiciansExists(musicians.Id))
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
            return View(musicians);
        }

        // GET: Musicians/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicians = await _context.Musicians
                .FirstOrDefaultAsync(m => m.Id == id);
            if (musicians == null)
            {
                return NotFound();
            }

            return View(musicians);
        }

        // POST: Musicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musicians = await _context.Musicians.FindAsync(id);
            _context.Musicians.Remove(musicians);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MusiciansExists(int id)
        {
            return _context.Musicians.Any(e => e.Id == id);
        }
    }
}
