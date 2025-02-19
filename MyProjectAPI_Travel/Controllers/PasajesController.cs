using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;

namespace MyProjectAPI_Travel.Controllers
{
    public class PasajesController : Controller
    {
        private readonly MyProjectTravelContext _context;

        public PasajesController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SearchForPassage()
        {
            var estaciones = _context.TbStations.Where(e => e.State == true).ToList();
            ViewBag.Estaciones = new SelectList(estaciones, "Id", "Ciudad");
            return View(new List<Itinerary>());
        }

        [HttpPost]
        public IActionResult SearchForPassage(int? idOrigen, int? idDestino, DateTime? fechaInicio)
        {
            var estaciones = _context.TbStations.Where(e => e.State == true).ToList();
            ViewBag.Estaciones = new SelectList(estaciones, "Id", "Ciudad");

            if (!idOrigen.HasValue && !idDestino.HasValue && !fechaInicio.HasValue)
            {
                ViewBag.Mensaje = "Por favor ingrese datos";
                return View(new List<Itinerary>());
            }

            var filteredItineraries = _context.TbItineraries
                .AsNoTracking()
                .Where(i => (i.State == true) &&
                            (!idOrigen.HasValue || i.IdOrigin == idOrigen) &&
                            (!idDestino.HasValue || i.IdDestination == idDestino) &&
                            (!fechaInicio.HasValue || EF.Functions.DateDiffDay(i.StartDate, fechaInicio) == 0)
                            )
                .Include(r => r.Origin)
                .Include(r => r.Destination)
                .ToList();


            return View(filteredItineraries);
        }

        [HttpGet]
        public IActionResult SearchForInformationPassage(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            var estacion = _context.TbItineraries
                .Include(r => r.Origin)
                .Include(r => r.Destination)
                .Include(u => u.Bus).ThenInclude(t => t.Seating)
                .FirstOrDefault(e => e.IdItn == id && e.State == true);

            var busEntity = _context.TbBus.Find(estacion.IdBus);

            var existingSeats = _context.TbSeating.Where(e => e.IdBus == estacion.IdBus).ToList();

            if (estacion == null)
            {
                return NotFound();
            }

            ViewBag.Asientos = existingSeats;
            return View(estacion);
        }
    }
}
