using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            CelestialObject obj = _context.CelestialObjects.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            obj.Satellites = _context.CelestialObjects.Where(u => u.OrbitedObjectId == id).ToList();
            return Ok(obj);
        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            IEnumerable<CelestialObject> objects = _context.CelestialObjects.Where(u => u.Name == name).ToList();

            if (objects.Count() == 0)
            {
                return NotFound();
            }

            foreach (var obj in objects)
            {
                obj.Satellites = _context.CelestialObjects.Where(u => u.OrbitedObjectId == obj.Id).ToList();
            }

            return Ok(objects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<CelestialObject> objects = _context.CelestialObjects;

            foreach (var obj in objects)
            {
                obj.Satellites = _context.CelestialObjects.Where(u => u.OrbitedObjectId == obj.Id).ToList();
            }

            return Ok(objects);
        }
    }
}
