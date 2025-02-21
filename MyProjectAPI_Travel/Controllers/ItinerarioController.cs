using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;
using MyProjectAPI_Travel.Models.DTO;

namespace MyProjectAPI_Travel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItinerarioController : ControllerBase
    {
        private readonly MyProjectTravelContext _context;

        public ItinerarioController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        //[Authorize(Roles = "admin")]
        public IActionResult GetAllItinerarios()
        {
            var allItinerarios = _context.TbItineraries.Where(e => e.State == true).ToList();

            return Ok(allItinerarios);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetItinerarioById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { mensaje = "No se ingreso el ID." });
            }

            var itinerarioEntity = _context.TbItineraries
                .Include(r => r.Origin)
                .Include(d => d.Destination)
                .Include(t => t.Worker).ThenInclude(t => t.User)
                .Include(b => b.Bus)
                .FirstOrDefault(e => e.IdItn == id && e.State == true);

            if (itinerarioEntity is null)
                return NotFound(new { mensaje = "Itinerario no encontrado." });

            return Ok(itinerarioEntity);
        }

        [HttpPost("add")]
        [Authorize(Roles = "admin")]
        public IActionResult AddItinerario([FromBody] Itinerary model)
        {
            try
            {
                if (model is null)
                {
                    return BadRequest(new { mensaje = "El objeto proporcionado es nulo." });
                }

                var itinerarioEntity = new Itinerary()
                {
                    IdOrigin = model.IdOrigin,
                    IdDestination = model.IdDestination,
                    IdWrk = model.IdWrk,
                    IdBus = model.IdBus,
                    StartDate = model.StartDate,
                    ArrivalDate = model.ArrivalDate,
                    Price = model.Price,
                    Availability = true,
                    State = true
                };

                _context.TbItineraries.Add(itinerarioEntity);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetItinerarioById), new { id = itinerarioEntity.IdItn }, itinerarioEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al agregar el itinerario.", error = ex.Message });
            }
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateItinerario(int id, [FromBody] Itinerary model)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { mensaje = "No se ingreso el ID." });
                }

                if (model is null)
                {
                    return BadRequest(new { mensaje = "El objeto proporcionado es nulo." });
                }

                var itinerarioEntity = _context.TbItineraries.Find(id);
                if (itinerarioEntity is null)
                    return NotFound(new { mensaje = "Itinerario no encontrado." });

                itinerarioEntity.IdOrigin = model.IdOrigin;
                itinerarioEntity.IdDestination = model.IdDestination;
                itinerarioEntity.IdWrk = model.IdWrk;
                itinerarioEntity.IdBus = model.IdBus;
                itinerarioEntity.StartDate = model.StartDate;
                itinerarioEntity.ArrivalDate = model.ArrivalDate;
                itinerarioEntity.Price = model.Price;
                itinerarioEntity.Availability = true;

                _context.SaveChanges();

                return Ok(new { mensaje = "Itinerario actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el itinerario.", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteItinerario(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { mensaje = "No se ingreso el ID." });
            }

            var itinerarioEntity = _context.TbItineraries.Find(id);
            if (itinerarioEntity is null)
                return NotFound(new { mensaje = "Itinerario no encontrado." });

            itinerarioEntity.State = false;

            _context.SaveChanges();

            return Ok(new { mensaje = "Itinerario eliminado correctamente." });
        }
    }
}
