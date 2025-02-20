using Microsoft.AspNetCore.Mvc;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;

namespace MyProjectAPI_Travel.Controllers
{
    public class BoletoController : Controller
    {
        private readonly MyProjectTravelContext _context;

        public BoletoController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllBoleto()
        {
            try
            {
                if (!User.IsInRole("admin"))
                {
                    throw new Exception("Error");
                }

                var allBoleto = _context.TbTickets.Where(e => e.State == true).ToList();

                return Ok(new { mensaje = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetBoletoById(int id)
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

                var BoletoEntity = _context.TbTickets.Find(id);

                if (BoletoEntity is null)
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
        public IActionResult UpdateBoleto(int id, Ticket model)
        {
            try
            {
                if (!User.IsInRole("admin"))
                {
                    throw new Exception("Error");
                }

                var BoletoEntity = _context.TbTickets.Find(id);

                if (BoletoEntity is null)
                {
                    throw new Exception("El numero de boleta no existe");
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

                return View(BoletoEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteBoleto(int id)
        {
            var BoletoEntity = _context.TbTickets.Find(id);
            if (BoletoEntity is null)
                return NotFound();
            _context.TbTickets.Remove(BoletoEntity);
            _context.SaveChanges();
            return Ok();
        }
    }
}
