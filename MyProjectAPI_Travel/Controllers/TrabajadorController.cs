using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;

namespace MyProjectAPI_Travel.Controllers
{
    [Authorize(Roles = "admin")]
    public class TrabajadorController : Controller
    {
        private readonly MyProjectTravelContext _context;

        public TrabajadorController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllTrabajador()
        {
            try
            {
                if (!User.IsInRole("admin"))
                {
                    throw new Exception("Error");
                }

                var allTrabajador = _context.TbWorkers.Where(e => e.State == true).ToList();

                return Ok(new { mensaje = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetTrabajadorById(int id)
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

                var trabajadorEntity = _context.TbWorkers.Find(id);

                if (trabajadorEntity is null)
                {
                    throw new Exception("El numero de boleta no existe");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddTrabajador(Worker model)
        {
            try
            {
                if (!User.IsInRole("admin"))
                {
                    throw new Exception("Error");
                }

                if (model is null)
                {
                    throw new Exception("");
                }

                var trabajadorEntity = new Worker()
                {
                    Salary = model.Salary
                };

                _context.TbWorkers.Add(trabajadorEntity);
                _context.SaveChanges();

                return View(trabajadorEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al ingresar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult UpdateTrabajador(int id, Worker model)
        {
            try
            {
                if (!User.IsInRole("admin"))
                {
                    throw new Exception("Error");
                }

                var trabajadorEntity = _context.TbWorkers.Find(id);

                if (trabajadorEntity is null)
                {
                    throw new Exception("");
                }

                trabajadorEntity.Salary = model.Salary;

                _context.SaveChanges();

                return Ok(new { mensaje = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteTrabajador(int id)
        {
            var trabajadorEntity = _context.TbWorkers.Find(id);
            if (trabajadorEntity is null)
                return NotFound();
            _context.TbWorkers.Remove(trabajadorEntity);
            _context.SaveChanges();
            return Ok();
        }
    }
}
