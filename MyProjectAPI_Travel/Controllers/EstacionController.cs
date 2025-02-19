using Microsoft.AspNetCore.Mvc;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;

namespace MyProjectAPI_Travel.Controllers
{
    public class EstacionController : Controller
    {
        private readonly MyProjectTravelContext _context;

        public EstacionController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllEstacion()
        {
            var allEstacion = _context.TbStations.ToList();
            return View(allEstacion);
        }

        [HttpGet]
        public IActionResult GetEstacionById(int id)
        {
            var estacionEntity = _context.TbStations.Find(id);
            if (estacionEntity is null)
                return View(new Station());
            return View(estacionEntity);
        }

        [HttpGet]
        public IActionResult AddEstacion()
        {
            try
            {
                return View(new Station());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al ingresar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddEstacion(Station model)
        {
            try
            {
                var estacionEntity = new Station()
                {
                    City = model.City,
                    Street = model.Street,
                    Pseudonym = model.Pseudonym
                };

                _context.TbStations.Add(estacionEntity);
                _context.SaveChanges();

                return View(estacionEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al ingresar el bus.", error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult UpdateEstacion(int id)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Home", "User");
            }
            try
            {
                var estacionEntity = _context.TbStations.Find(id);
                if (estacionEntity is null)
                    return NotFound();

                _context.SaveChanges();

                return View(estacionEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult UpdateEstacion(int id, Station model)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Home", "User");
            }
            try
            {
                var estacionEntity = _context.TbStations.Find(id);
                if (estacionEntity is null)
                    return NotFound();

                estacionEntity.City = model.City;
                estacionEntity.Street = model.Street;
                estacionEntity.Pseudonym = model.Pseudonym;

                _context.SaveChanges();

                return View(estacionEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteEstacion(int id)
        {
            var estacionEntity = _context.TbStations.Find(id);
            if (estacionEntity is null)
                return NotFound();
            _context.TbStations.Remove(estacionEntity);
            _context.SaveChanges();
            return Ok();
        }
    }
}
