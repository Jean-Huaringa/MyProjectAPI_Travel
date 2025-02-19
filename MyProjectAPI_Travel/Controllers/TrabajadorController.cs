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
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Error", "MensajeError");
            }
            var allTrabajador= _context.TbWorkers.ToList();
            return View(allTrabajador);
        }

        [HttpGet]
        public IActionResult GetTrabajadorById(int id)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Error", "MensajeError");
            }
            var trabajadorEntity = _context.TbWorkers.Find(id);
            if (trabajadorEntity is null)
                return View(new Worker());
            return View(trabajadorEntity);
        }

        [HttpPost]
        public IActionResult AddTrabajador(Worker model)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Error", "MensajeError");
            }
            try
            {
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

        [HttpGet]
        public IActionResult UpdateTrabajador(int id)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Error", "MensajeError");
            }
            try
            {
                var trabajadorEntity = _context.TbWorkers.Find(id);
                if (trabajadorEntity is null)
                    return NotFound();

                _context.SaveChanges();

                return View(trabajadorEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult UpdateTrabajador(int id, Worker model)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Error", "MensajeError");
            }
            try
            {
                var trabajadorEntity = _context.TbWorkers.Find(id);
                if (trabajadorEntity is null)
                    return NotFound();

                trabajadorEntity.Salary = model.Salary;

                _context.SaveChanges();

                return View(trabajadorEntity);
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
