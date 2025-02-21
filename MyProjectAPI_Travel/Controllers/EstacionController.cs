using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;
using MyProjectAPI_Travel.Models.DTO;

namespace MyProjectAPI_Travel.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstacionController : ControllerBase
{
    private readonly MyProjectTravelContext _context;

    public EstacionController(MyProjectTravelContext context)
    {
        _context = context;
    }

    [HttpGet("all")]
    public IActionResult GetAllEstacion()
    {
        try
        {
            var estaciones = _context.TbStations
                .Where(e => e.State == true)
                .Select(e => new
                {
                    e.IdStn,
                    e.City,
                    e.Street,
                    e.Pseudonym
                })
                .ToList();

            return Ok(estaciones);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al obtener las estaciones.", error = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult GetEstacionById(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new { mensaje = "No se ingreso el ID." });
            }

            var estacion = _context.TbStations
                .Where(e => e.IdStn == id && e.State == true)
                .Select(e => new
                {
                    e.IdStn,
                    e.City,
                    e.Street,
                    e.Pseudonym
                })
                .FirstOrDefault();

            if (estacion == null)
            {
                return NotFound(new { mensaje = "Estación no encontrada." });
            }

            return Ok(estacion);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al obtener la estación.", error = ex.Message });
        }
    }

    [HttpPost("add")]
    [Authorize(Roles = "admin")]
    public IActionResult AddEstacion([FromBody] Station model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var estacion = new Station
            {
                City = model.City,
                Street = model.Street,
                Pseudonym = model.Pseudonym,
                State = true
            };

            _context.TbStations.Add(estacion);
            _context.SaveChanges();

            return Ok(new { mensaje = "Estación agregada correctamente." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al agregar la estación.", error = ex.Message });
        }
    }

    [HttpPut("update/{id:int}")]
    [Authorize(Roles = "admin")]
    public IActionResult UpdateEstacion(int id, [FromBody] Station model)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new { mensaje = "No se ingreso el ID." });
            }

            var estacion = _context.TbStations.Find(id);

            if (estacion == null)
            {
                return NotFound(new { mensaje = "Estación no encontrada." });
            }

            estacion.City = model.City;
            estacion.Street = model.Street;
            estacion.Pseudonym = model.Pseudonym;

            _context.SaveChanges();

            return Ok(new { mensaje = "Estación actualizada correctamente." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al actualizar la estación.", error = ex.Message });
        }
    }

    [HttpDelete("delete/{id:int}")]
    [Authorize(Roles = "admin")]
    public IActionResult DeleteEstacion(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new { mensaje = "No se ingreso el ID." });
            }

            var estacion = _context.TbStations.Find(id);

            if (estacion == null)
                return NotFound(new { mensaje = "Estación no encontrada." });

            estacion.State = false;

            _context.SaveChanges();

            return Ok(new { mensaje = "Estación eliminada correctamente." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al eliminar la estación.", error = ex.Message });
        }
    }
}
