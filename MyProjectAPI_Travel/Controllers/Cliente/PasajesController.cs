using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;
using MyProjectAPI_Travel.Models.DTO;

namespace MyProjectAPI_Travel.Controllers.Cliente
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasajesController : ControllerBase
    {
        private readonly MyProjectTravelContext _context;

        public PasajesController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpGet("all/station")]
        public ActionResult<IEnumerable<object>> GetAllEstacion()
        {
            try
            {
                var estaciones = _context.TbStations
                    .Where(e => e.State)
                    .Select(e => new
                    {
                        e.IdStn,
                        e.City,
                        e.Street,
                        e.Pseudonym
                    })
                    .ToList();

                if (!estaciones.Any())
                {
                    return NotFound("No hay estaciones disponibles.");
                }

                return Ok(estaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener las estaciones.", error = ex.Message });
            }
        }

        [HttpGet("filter-for-passage")]
        public IActionResult SearchForPassage([FromQuery] int? idOrigen, [FromQuery] int? idDestino, [FromQuery] DateTime? fechaInicio)
        {
            try
            {
                var filteredItineraries = _context.TbItineraries
                    .AsNoTracking()
                    .Where(i => i.State &&
                                (!idOrigen.HasValue || i.IdOrigin == idOrigen) &&
                                (!idDestino.HasValue || i.IdDestination == idDestino) &&
                                (!fechaInicio.HasValue || EF.Functions.DateDiffDay(i.StartDate, fechaInicio) == 0))
                    .Include(i => i.Origin)
                    .Include(i => i.Destination)
                    .ToList();

                if (!filteredItineraries.Any())
                {
                    return NotFound("No se encontraron pasajes con los filtros proporcionados.");
                }

                return Ok(filteredItineraries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al filtrar pasajes.", error = ex.Message });
            }
        }

        [HttpGet("search-for-information-passage/{id:int}")]
        public IActionResult SearchForInformationPassage(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            var EstacionEntity = _context.TbItineraries
                .Include(it => it.Origin)
                .Include(it => it.Destination)
                .Include(it => it.Bus).ThenInclude(b => b.Seating)
                .FirstOrDefault(it => it.IdItn == id);

            if (EstacionEntity == null)
            {
                return NotFound("No se encontró información del pasaje.");
            }

            return Ok(EstacionEntity);
        }

        [HttpPost("purchase")]
        public IActionResult BuyTicket([FromBody] TicketDTO model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var ticketEntity = new Ticket()
                {
                    IdUsr = model.IdUsr,
                    IdWrk = model.IdWrk,
                    IdItn = model.IdItn,
                    IdBus = model.IdBus,
                    Row = model.Row,
                    Column = model.Column,
                    Amount = model.Amount,
                    PaymentMethod = model.PaymentMethod,
                    UserName = model.UserName,
                    Lastname = model.Lastname,
                    Age = model.Age,
                    TypeDocument = model.TypeDocument,
                    NumDocument = model.NumDocument,
                    State = true
                };

                _context.TbTickets.Add(ticketEntity);
                _context.SaveChanges();

                return Ok(new { mensaje = "El boleto se registró con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Hubo un error al registrar el boleto.", error = ex.Message });
            }
        }
    }
}
