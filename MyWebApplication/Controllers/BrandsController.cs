using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using MyWebApplication;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace MyWebApplication.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class BrandsController : Controller
    {
        private readonly guitar_shopContext _context;

        public BrandsController(guitar_shopContext context)
        {
            _context = context;
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyName(string name)
        {
            var brands = (from b in _context.Brands where b.Name == name select b).ToList();
            if (brands.Count > 0)
            {
                return Json($"Name {name} is already exists.");
            }
            else
            {
                return Json(true);
            }
        }

        // GET: Brands
        public async Task<IActionResult> Index()
        {
            return View(await _context.Brands.ToListAsync());
        }

        // GET: Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brands = await _context.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brands == null)
            {
                return NotFound();
            }

            //return View(brands);
            return RedirectToAction("Index", "Guitars", new { id = brands.Id, name = brands.Name });
        }

        // GET: Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Country")] Brands brands)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brands);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brands);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brands = await _context.Brands.FindAsync(id);
            if (brands == null)
            {
                return NotFound();
            }
            return View(brands);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Country")] Brands brands)
        {
            if (id != brands.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brands);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandsExists(brands.Id))
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
            return View(brands);
        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brands = await _context.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brands == null)
            {
                return NotFound();
            }

            return View(brands);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brands = await _context.Brands.FindAsync(id);
            var guitars = _context.Guitars.Where(g => g.BrandId == id);
            foreach (Guitars g in guitars)
            {
                _context.Guitars.Remove(g);
            }
            _context.Brands.Remove(brands);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandsExists(int id)
        {
            return _context.Brands.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Brands newbrand;
                                var b = (from br in _context.Brands where 
                                         br.Name.Contains(worksheet.Name) select br).ToList();
                                if (b.Count > 0)
                                {
                                    newbrand = b[0];
                                }
                                else
                                {
                                    newbrand = new Brands();
                                    newbrand.Name = worksheet.Name;
                                    newbrand.Country = "Ukraine";
                                    //додати в контекст
                                    _context.Brands.Add(newbrand);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed())
                                {
                                    try
                                    {
                                        Guitars guitar = new Guitars();
                                        var guitars = (from g in _context.Guitars where 
                                                       g.Name.Contains(row.Cell(1).Value.ToString()) select g).ToList();
                                        if (guitars.Count > 0)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            guitar.Name = row.Cell(1).Value.ToString();
                                            guitar.Brand = newbrand;
                                            guitar.Cost = Convert.ToInt32(row.Cell(2).Value);
                                            guitar.Year = Convert.ToInt32(row.Cell(3).Value);
                                            guitar.Info = "from excel";
                                            //у разі наявності автора знайти його, у разі відсутності - додати

                                            Forms form;
                                            var forms = (from f in _context.Forms where 
                                                         f.Name.Contains(row.Cell(4).Value.ToString()) select f).ToList();
                                            if (forms.Count > 0)
                                            {
                                                form = forms[0];
                                            }
                                            else
                                            {
                                                form = new Forms();
                                                form.Name = row.Cell(4).Value.ToString();
                                                _context.Forms.Add(form);
                                            }
                                            guitar.Form = form;

                                            Materials material;
                                            var materials = (from m in _context.Materials where 
                                                             m.Name.Contains(row.Cell(5).Value.ToString()) select m).ToList();
                                            if (materials.Count > 0)
                                            {
                                                material = materials[0];
                                            }
                                            else
                                            {
                                                material = new Materials();
                                                material.Name = row.Cell(5).Value.ToString();
                                                _context.Materials.Add(material);
                                            }
                                            guitar.Material = material;

                                            Types type;
                                            var types = (from t in _context.Types where 
                                                         t.Name.Contains(row.Cell(6).Value.ToString()) select t).ToList();
                                            if (types.Count > 0)
                                            {
                                                type = types[0];
                                            }
                                            else
                                            {
                                                type = new Types();
                                                type.Name = row.Cell(6).Value.ToString();
                                                _context.Types.Add(type);
                                            }
                                            guitar.Type = type;

                                            _context.Guitars.Add(guitar);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        //logging самостійно :)
                                        throw new Exception("Text with context data", e);
                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var brands = _context.Brands.Include("Guitars").ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ (писати лише вибрані)
                foreach (var b in brands)
                {
                    var worksheet = workbook.Worksheets.Add(b.Name);

                    worksheet.Cell("A1").Value = "Name";
                    worksheet.Cell("B1").Value = "Price";
                    worksheet.Cell("C1").Value = "Year";
                    worksheet.Cell("D1").Value = "Form";
                    worksheet.Cell("E1").Value = "Material";
                    worksheet.Cell("F1").Value = "Type";
                    worksheet.Cell("G1").Value = "Info";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var guitars = b.Guitars.ToList();

                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < guitars.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = guitars[i].Name;
                        worksheet.Cell(i + 2, 2).Value = guitars[i].Cost;
                        worksheet.Cell(i + 2, 3).Value = guitars[i].Year;
                        var form = _context.Forms.Where(f => f.Id == guitars[i].FormId).ToString();
                        worksheet.Cell(i + 2, 4).Value = _context.Forms.Where(f => f.Id == guitars[i].FormId).ToList()[0].Name;
                        worksheet.Cell(i + 2, 5).Value =  _context.Materials.Where(m => m.Id == guitars[i].MaterialId).ToList()[0].Name;
                        worksheet.Cell(i + 2, 6).Value = _context.Types.Where(t => t.Id == guitars[i].TypeId).ToList()[0].Name;
                        worksheet.Cell(i + 2, 7).Value = guitars[i].Info;
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"GuitarShop{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

    }
}
