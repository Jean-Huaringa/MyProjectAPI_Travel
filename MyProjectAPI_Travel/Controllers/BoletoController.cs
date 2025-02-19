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
            var allBoleto = _context.TbTickets.ToList();
            return View(allBoleto);
        }

        [HttpGet]
        public IActionResult GetBoletoById(int id)
        {
            var BoletoEntity = _context.TbTickets.Find(id);
            if (BoletoEntity is null)
                return View(new Ticket());
            return View(BoletoEntity);
        }

        [HttpGet]
        public IActionResult AddBoleto()
        {
            try
            {
                return View(new Ticket());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al ingresar el bus.", error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult UpdateBoleto(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Home", "User");
            }
            try
            {
                var BoletoEntity = _context.TbTickets.Find(id);
                if (BoletoEntity is null)
                    return NotFound();

                _context.SaveChanges();

                return View(BoletoEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult UpdateBoleto(int id, Ticket model)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Home", "User");
            }
            try
            {
                var BoletoEntity = _context.TbTickets.Find(id);
                if (BoletoEntity is null)
                    return NotFound();

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
                return StatusCode(500, new { mensaje = "Error al actualizar el bus.", error = ex.Message });
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
