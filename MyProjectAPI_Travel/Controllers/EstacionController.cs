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
            try
            {
                if (!User.IsInRole("admin"))
                {
                    throw new Exception("Error");
                }

                var allEstacion = _context.TbStations.Where(e => e.State == true).ToList();

                return Ok(new { mensaje = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetEstacionById(int id)
        {
            try
            {
                if (!User.IsInRole("admin"))
                {
                    throw new Exception("Error");
                }

                if (id <= 0)
                {
                    throw new Exception("No se ingreso el numero de la boleta");
                }

                var estacionEntity = _context.TbStations.Find(id);

                if (estacionEntity is null)
                {
                    throw new Exception("El numero de boleta no existe");
                }

                return Ok(new { mensaje = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddEstacion(Station model)
        {
            try
            {
                if (model is null)
                {
                    throw new Exception("");
                }

                var estacionEntity = new Station()
                {
                    City = model.City,
                    Street = model.Street,
                    Pseudonym = model.Pseudonym
                };

                _context.TbStations.Add(estacionEntity);
                _context.SaveChanges();

                return Ok(new { mensaje = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al ingresar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult UpdateEstacion(int id, Station model)
        {
            if (!User.IsInRole("admin"))
            {
                throw new Exception("Error");
            }
            try
            {
                var estacionEntity = _context.TbStations.Find(id);

                if (estacionEntity is null)
                {
                    throw new Exception("");
                }

                estacionEntity.City = model.City;
                estacionEntity.Street = model.Street;
                estacionEntity.Pseudonym = model.Pseudonym;

                _context.SaveChanges();

                return Ok(new { mensaje = "" });
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
