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
    public class BrController : ControllerBase
    {
        private readonly guitar_shopContext _context;

        public BrController(guitar_shopContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
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
    }
}