using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;
using MyProjectAPI_Travel.Models.DTO;

namespace MyProjectAPI_Travel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "admin,manager")]
    public class TrabajadorController : ControllerBase
    {
        private readonly MyProjectTravelContext _context;

        public TrabajadorController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        [Authorize(Roles = "admin")]
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

        [HttpGet("{id:int}")]
        [Authorize(Roles = "admin")]
        public IActionResult GetTrabajadorById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { mensaje = "No se ingreso el ID." });
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

        [HttpPost("add")]
        [Authorize(Roles = "admin")]
        public IActionResult AddTrabajador(Worker model)
        {
            try
            {
                //if (!User.IsInRole("admin"))
                //{
                //    return Ok("No tienes acceso");
                //}

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var trabajadorEntity = new Worker()
                {
                    IdWrk = model.IdWrk,
                    Role = model.Role,
                    Salary = model.Salary,
                    Availability = false,
                    State = true
                };

                _context.TbWorkers.Add(trabajadorEntity);
                _context.SaveChanges();

                return Ok(new { mensaje = "Se agrego el un nuevo trabajador" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al ingresar el nuevo trabajador.", error = ex.Message });
            }
        }

        [HttpPut("update/{id:int}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateTrabajador(int id, Worker model)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { mensaje = "No se ingreso el ID." });
                }

                var trabajadorEntity = _context.TbWorkers.Find(id);

                if (trabajadorEntity is null)
                {
                    return BadRequest(new { mensaje = "No se pudo obtener datos del trabajador." });
                }

                trabajadorEntity.Salary = model.Salary;

                _context.SaveChanges();

                return Ok(new { mensaje = "El trabajador se actualizo correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el usuario.", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id:int}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteTrabajador(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { mensaje = "No se ingreso el ID." });
            }
            var trabajadorEntity = _context.TbWorkers.Find(id);
            if (trabajadorEntity is null)
                return NotFound();
            trabajadorEntity.State = false;
            _context.SaveChanges();
            return Ok();
        }
    }
}
