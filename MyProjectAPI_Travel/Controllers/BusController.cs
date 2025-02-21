using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;
using MyProjectAPI_Travel.Models.DTO;

namespace MyProjectAPI_Travel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusController : ControllerBase
    {
        private readonly MyProjectTravelContext _context;

        public BusController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        //[Authorize(Roles = "admin")]
        public IActionResult GetAllBus()
        {
            try
            {
                var allBus = _context.TbBus.Where(e => e.State == true).ToList();

                return Ok(allBus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBusById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { mensaje = "No se ingreso el ID." });
                }

                var busEntity = _context.TbBus.Find(id);

                if (busEntity is null)
                {
                    return BadRequest(new { mensaje = "El numero de boleta no existe" });
                }

                return Ok(busEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpPost("add")]
        //[Authorize(Roles = "admin")]
        public IActionResult AddBus([FromBody] Bus model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var busEntity = new Bus()
                {
                    Placa = model.Placa,
                    Model = model.Model,
                    NumColumns = model.NumColumns,
                    NumRows = model.NumRows,
                    State = true
                };

                _context.TbBus.Add(busEntity);
                _context.SaveChanges();

                return Ok(new { mensaje = "El bus se ingreso correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpPut("update/{id:int}")]
        //[Authorize(Roles = "admin")]
        public IActionResult UpdateBus(int id, [FromBody] Bus bus)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { mensaje = "No se ingreso el ID." });
                }

                var busEntity = _context.TbBus.Find(id);

                if (busEntity is null)
                {
                    return BadRequest(new { mensaje = "El numero de boleta no existe" });
                }

                busEntity.Placa = bus.Placa;
                busEntity.Model = bus.Model;
                busEntity.NumColumns = bus.NumColumns;
                busEntity.NumRows = bus.NumRows;

                _context.SaveChanges();

                return Ok(new { mensaje = "El bus se actualizo correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id:int}")]
        //[Authorize(Roles = "admin")]
        public IActionResult DeleteBus(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { mensaje = "No se ingreso el ID." });
            }
            var busEntity = _context.TbBus.Find(id);
            if (busEntity is null)
                return BadRequest(new { mensaje = "No se encontro el bus" });
            busEntity.State = false;
            _context.SaveChanges();
            return Ok(new { mensaje = "Estación eliminada correctamente." });
        }

    }
}
