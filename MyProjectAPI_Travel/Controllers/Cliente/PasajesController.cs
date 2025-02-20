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

        [HttpPost("filter-for-passage")]
        public IActionResult SearchForPassage(int? idOrigen, int? idDestino, DateTime? fechaInicio)
        {

            var filteredItineraries = _context.TbItineraries
                .AsNoTracking()
                .Where(i => i.State == true &&
                            (!idOrigen.HasValue || i.IdOrigin == idOrigen) &&
                            (!idDestino.HasValue || i.IdDestination == idDestino) &&
                            (!fechaInicio.HasValue || EF.Functions.DateDiffDay(i.StartDate, fechaInicio) == 0)
                            )
                .Include(r => r.Origin)
                .Include(r => r.Destination)
                .ToList();


            return Ok(filteredItineraries);
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
                .FirstOrDefault(it => it.IdItn == id);


            if (EstacionEntity == null)
            {
                return NotFound();
            }

            return Ok(EstacionEntity);
        }

        [HttpPost("buy-ticket/")]
        public IActionResult BuyTicket([FromBody] TicketDTO model)
        {
            try
            {
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
                    NumDocument = model.NumDocument
                };

                _context.TbTickets.Add(ticketEntity);
                _context.SaveChanges();

                return Ok(new { mensaje = "El boleto se registro con exito" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }
    }
}
