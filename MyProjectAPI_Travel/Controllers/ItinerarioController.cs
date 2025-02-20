using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;

namespace MyProjectAPI_Travel.Controllers
{
    //[Route("Itinerario")]
    public class ItinerarioController : Controller
    {
        private readonly MyProjectTravelContext _context;

        public ItinerarioController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllItinerario()
        {
            var allItinerario = _context.TbItineraries
                .Include(r => r.Origin)
                .Include(d => d.Destination)
                .Include(t => t.Worker).ThenInclude(t => t.Users)
                .Include(b => b.Bus)
                .ToList();

            return View(allItinerario);
        }

        [HttpGet]
        public IActionResult GetItinerarioById(int id)
        {
            if (id <= 0)
            {
                Console.WriteLine("El objeto es null.");
            }

            var ItinerarioEntity = _context.TbItineraries
                .Include(r => r.Origin)
                .Include(d => d.Destination)
                .Include(t => t.Worker).ThenInclude(t => t.Users)
                .Include(b => b.Bus)
                .FirstOrDefault(e => e.IdItn == id && e.State == true);

            if (ItinerarioEntity is null)
                return View(new Itinerary());

            return View(ItinerarioEntity);
        }

        [HttpGet]
        public IActionResult AddItinerario()
        {
            try
            {
                var estaciones = _context.TbStations.Where(e => e.State == true).ToList();
                var bus = _context.TbBus.Where(e => e.State == true).ToList();
                var trabajador = _context.TbWorkers
                    .Include(t => t.Users)
                    .Where(e => e.State == true && e.Role == "seller").ToList();


                ViewBag.Estaciones = new SelectList(estaciones, "Id", "Ciudad");
                ViewBag.Bus = new SelectList(bus, "Id", "Placa");
                ViewBag.Trabajador = new SelectList(trabajador, "Id", "Usuario.Nombre");

                return View(new Itinerary());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al ingresar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddItinerario(Itinerary model)
        {
            try
            {
                if (model is null)
                {
                    Console.WriteLine("El objeto es null.");
                }

                var ItinerarioEntity = new Itinerary()
                {
                    IdOrigin = model.IdOrigin,
                    IdDestination = model.IdDestination,
                    IdWrk = model.IdWrk,
                    IdBus = model.IdBus,
                    StartDate = model.StartDate,
                    ArrivalDate = model.ArrivalDate,
                    Price = model.Price
                };

                _context.TbItineraries.Add(ItinerarioEntity);
                _context.SaveChanges();

                return View(ItinerarioEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al ingresar el bus.", error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult UpdateItinerario(int id)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Home", "User");
            }
            try
            {
                if (id <= 0)
                {
                    Console.WriteLine("El objeto es null.");
                }

                var ItinerarioEntity = _context.TbItineraries
                .Include(r => r.Origin)
                .Include(d => d.Destination)
                .Include(t => t.Worker).ThenInclude(t => t.Users)
                .Include(b => b.Bus)
                .FirstOrDefault(e => e.IdItn == id && e.State == true);

                var estaciones = _context.TbStations.Where(e => e.State == true).ToList();
                var bus = _context.TbBus.Where(e => e.State == true).ToList();
                var trabajador = _context.TbWorkers
                    .Include(t => t.Users)
                    .Where(e => e.State == true && e.Role == "seller").ToList();

                if (ItinerarioEntity is null)
                    return NotFound();

                _context.SaveChanges();

                ViewBag.Estaciones = new SelectList(estaciones, "Id", "Ciudad");
                ViewBag.Bus = new SelectList(bus, "Id", "Placa");
                ViewBag.Trabajador = new SelectList(trabajador, "Id", "Usuario.Nombre");

                return View(ItinerarioEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult UpdateItinerario(int id, Itinerary model)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Home", "User");
            }
            try
            {
                if(id <= 0)
                {
                    Console.WriteLine("El objeto es null.");
                }
                if (model is null)
                {
                    Console.WriteLine("El objeto es null.");
                }

                var ItinerarioEntity = _context.TbItineraries.Find(id);
                if (ItinerarioEntity is null)
                    return NotFound();

                ItinerarioEntity.IdOrigin = model.IdOrigin;
                ItinerarioEntity.IdDestination = model.IdDestination;
                ItinerarioEntity.IdWrk = model.IdWrk;
                ItinerarioEntity.IdBus = model.IdBus;
                ItinerarioEntity.StartDate = model.StartDate;
                ItinerarioEntity.ArrivalDate = model.ArrivalDate;
                ItinerarioEntity.Price = model.Price;

                _context.SaveChanges();

                return View(ItinerarioEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el bus.", error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteItinerario(int id)
        {
            if (id <= 0)
            {
                Console.WriteLine("El objeto es null.");
            }

            var ItinerarioEntity = _context.TbItineraries.Find(id);
            if (ItinerarioEntity is null)
                return NotFound();
            _context.TbItineraries.Remove(ItinerarioEntity);
            _context.SaveChanges();
            return Ok();
        }

    }
}
