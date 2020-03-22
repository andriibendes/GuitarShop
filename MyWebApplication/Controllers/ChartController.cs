using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly guitar_shopContext _context;

        public ChartController(guitar_shopContext context)
        {
            _context = context;
        }

        [HttpGet("JsonDataBrands")]
        public JsonResult JsonDataBrands()
        {
            var brands = _context.Brands.Include(g => g.Guitars).ToList();
            List<object> cGuitar = new List<object>();
            cGuitar.Add(new[] { "Brand", "Count" });
            foreach (var b in brands)
            {
                cGuitar.Add( new object[] { b.Name, b.Guitars.Count() });
            }
            return new JsonResult(cGuitar);
        }

        [HttpGet("JsonDataPrice")]
        public JsonResult JsonDataPrice()
        {
            var brands = _context.Brands.Include(g => g.Guitars).ToList();
            List<object> cGuitar = new List<object>();
            cGuitar.Add(new[] { "Brand", "Average price" });

            foreach (var b in brands)
            {
                if (b.Guitars.Count == 0)
                {
                    continue;
                }
                int average = 0;
                foreach (var g in b.Guitars)
                {
                    average += g.Cost;
                }
                average /= b.Guitars.Count;
                cGuitar.Add(new object[] { b.Name, average });
            }
            return new JsonResult(cGuitar);
        }
    }
}