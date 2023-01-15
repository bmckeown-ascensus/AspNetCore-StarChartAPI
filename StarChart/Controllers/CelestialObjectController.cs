﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var CelestialObject = _context.CelestialObjects.Find(id);
            if (CelestialObject == null)
            {
                return NotFound();
            }
            CelestialObject.Satellites = _context.CelestialObjects
                .Where(objs => objs.OrbitedObjectId == id).ToList();
            return Ok(CelestialObject);
        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(objs => objs.Name == name).ToList();
            if (!celestialObjects.Any())
            {
                return NotFound();
            }
            foreach (var obj in celestialObjects)
            {
                obj.Satellites = _context.CelestialObjects
                    .Where(objs => objs.OrbitedObjectId == obj.Id).ToList();
            }
            return Ok(celestialObjects);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach (var obj in celestialObjects)
            {
                obj.Satellites = _context.CelestialObjects
                    .Where(objs => objs.OrbitedObjectId == obj.Id).ToList();
            }
            return Ok(celestialObjects);
        }

    }
}
