using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;

namespace MyProjectAPI_Travel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatingController : ControllerBase
    {
        private readonly MyProjectTravelContext _context;

        public SeatingController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        [Authorize(Roles = "admin")]
        public IActionResult AllSeating()
        {
            try
            {
                var allSeating = _context.TbSeating
                    .Where(e => e.State == true).ToList();

                return Ok(allSeating);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Problemas con el asiento", error = ex.Message });
            }
        }

        [HttpPost("add")]
        public IActionResult AddSeatinngByBus(int idBus)
        {
            try
            {
                if (idBus <= 0)
                {
                    return NotFound(new { mensaje = "Ingrese un ID" });
                }

                var busEntity = _context.TbBus.Find(idBus);

                if (busEntity == null)
                    return NotFound(new { mensaje = "Bus no encontrado." });

                var existingSeats = _context.TbSeating
                    .Where(e => e.IdBus == idBus)
                    .Select(e => new { e.Column, e.Row })
                    .ToHashSet();

                for (int col = 0; col < busEntity.NumColumns; col++)
                {
                    for (int row = 0; row < busEntity.NumRows; row++)
                    {
                        bool exists = existingSeats.Contains(new { Column = col, Row = row });

                        if (!exists)
                        {
                            Seat asi = new Seat()
                            {
                                Column = col,
                                Row = row,
                                IdBus = idBus,
                                State = true,
                                Type = "1",
                                Busy = false
                            };

                            _context.TbSeating.Add(asi);

                            _context.SaveChanges();
                        }
                    }
                }

                return Ok(new { mensaje = "Se ingresaron los asientos" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Problemas con el asiento", error = ex.Message });
            }
        }

        [HttpGet("take-seat/{idBus:int}/{row:int}/{column:int}")]
        public IActionResult TakeSeat(int idBus, int row, int column)
        {
            try
            {
                if (idBus <= 0 || row <= 0 || column <= 0)
                {
                    return BadRequest(new { mensaje = "Faltan identificadores válidos" });
                }

                var asiento = _context.TbSeating
                    .FirstOrDefault(e => e.IdBus == idBus && e.Row == row && e.Column == column && e.State == true);

                if (asiento == null)
                {
                    return NotFound(new { mensaje = "El asiento no existe o no está disponible" });
                }

                asiento.Busy = !asiento.Busy;

                _context.SaveChanges();

                return Ok(new
                {
                    mensaje = "El estado del asiento se actualizó correctamente",
                    asiento = new
                    {
                        asiento.IdBus,
                        asiento.Row,
                        asiento.Column,
                        asiento.Type,
                        asiento.Busy
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Problemas con el asiento", error = ex.Message });
            }
        }

    }
}
