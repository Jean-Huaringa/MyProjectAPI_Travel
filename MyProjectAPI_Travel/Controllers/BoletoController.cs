using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;
using MyProjectAPI_Travel.Models.DTO;

namespace MyProjectAPI_Travel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoletoController : ControllerBase
    {
        private readonly MyProjectTravelContext _context;

        public BoletoController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        //[Authorize(Roles = "admin")]
        public IActionResult GetAllBoleto()
        {
            try
            {
                var allBoleto = _context.TbTickets.Where(e => e.State == true).ToList();

                return Ok(allBoleto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        //[Authorize(Roles = "admin")]
        public IActionResult GetBoletoById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { mensaje = "No se ingreso el ID." });
                }

                var BoletoEntity = _context.TbTickets.Find(id);

                if (BoletoEntity is null)
                {
                    return BadRequest(new { mensaje = "El numero de boleta no existe." });
                }

                return Ok(BoletoEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpPost("add")]
        public IActionResult AddTicket([FromBody] TicketDTO model)
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
                    NumDocument = model.NumDocument,
                    State = true
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

        [HttpPut("update/{id:int}")]
        //[Authorize(Roles = "admin")]
        public IActionResult UpdateBoleto(int id, [FromBody] TicketDTO model)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { mensaje = "No se ingreso el ID." });
                }

                var BoletoEntity = _context.TbTickets.Find(id);

                if (BoletoEntity is null)
                {
                    return BadRequest(new { mensaje = "No se encontro el numero de boleta" });
                }

                BoletoEntity.IdUsr = model.IdUsr;
                BoletoEntity.IdWrk = model.IdWrk;
                BoletoEntity.IdItn = model.IdItn;
                BoletoEntity.IdBus = model.IdBus;
                BoletoEntity.Row = model.Row;
                BoletoEntity.Column = model.Column;
                BoletoEntity.PaymentMethod = model.PaymentMethod;
                BoletoEntity.Amount = model.Amount;
                BoletoEntity.UserName = model.UserName;
                BoletoEntity.Lastname = model.Lastname;
                BoletoEntity.Age = model.Age;
                BoletoEntity.TypeDocument = model.TypeDocument;
                BoletoEntity.NumDocument = model.NumDocument;

                _context.SaveChanges();

                return Ok(new { mensaje = "Boletto actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id:int}")]
        //[Authorize(Roles = "admin")]
        public IActionResult DeleteBoleto(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { mensaje = "No se ingreso el ID." });
            }
            var BoletoEntity = _context.TbTickets.Find(id);
            if (BoletoEntity is null)
                return BadRequest(new { mensaje = "No se encontro el numero de boleta" });
            BoletoEntity.State = false;
            _context.SaveChanges();
            return Ok(new { mensaje = "Boleto eliminada correctamente." });
        }

    }
}
