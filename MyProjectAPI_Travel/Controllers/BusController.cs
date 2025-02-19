using Microsoft.AspNetCore.Mvc;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;

namespace MyProjectAPI_Travel.Controllers
{
    public class BusController : Controller
    {
        private readonly MyProjectTravelContext _context;

        public BusController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllBus()
        {
            var allBus = _context.TbBus.ToList();
            return View(allBus);
        }

        [HttpGet]
        public IActionResult GetBusById(int id)
        {
            var busEntity = _context.TbBus.Find(id);
            if (busEntity is null)
                return View(new Bus());
            return View(busEntity);
        }

        [HttpGet]
        public IActionResult AddBus()
        {
            try
            {
                return View(new Bus());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al ingresar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddBus(Bus bus)
        {
            try
            {
                var busEntity = new Bus()
                {
                    Placa = bus.Placa,
                    Model = bus.Model,
                    NumColumns = bus.NumColumns,
                    NumRows = bus.NumRows
                };

                _context.TbBus.Add(busEntity);
                _context.SaveChanges();

                return View(busEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al ingresar el bus.", error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult UpdateBus(int id)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Error", "MensajeError");
            }
            try
            {
                var busEntity = _context.TbBus.Find(id);
                if (busEntity is null)
                    return NotFound();

                _context.SaveChanges();

                return View(busEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult UpdateBus(int id, Bus bus)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Home", "User");
            }
            try
            {
                var busEntity = _context.TbBus.Find(id);
                if (busEntity is null)
                    return NotFound();
                busEntity.Placa = bus.Placa;
                busEntity.Model = bus.Model;
                busEntity.NumColumns = bus.NumColumns;
                busEntity.NumRows = bus.NumRows;

                _context.SaveChanges();

                return View(busEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteBus(int id)
        {
            var busEntity = _context.TbBus.Find(id);
            if (busEntity is null)
                return NotFound();
            _context.TbBus.Remove(busEntity);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetSeatsByBus(int id)
        {
            var busEntity = _context.TbBus.Find(id);
            if (busEntity == null)
                return View(new Bus());

            var existingSeats = _context.TbSeating
                .Where(e => e.IdBus == id)
                .Select(e => new { e.Column, e.Row })
                .ToHashSet();

            List<Seat> addAsiento = new List<Seat>();

            for (int col = 0; col < busEntity.NumColumns; col++)
            {
                for (int row = 0; row < busEntity.NumRows; row++)
                {
                    bool exists = existingSeats.Contains(new { Column = col, Row = row });

                    addAsiento.Add(new Seat
                    {
                        Column = col,
                        Row = row,
                        IdBus = id,
                        State = exists,
                        Type = "1"
                    });
                }
            }

            ViewBag.Asientos = addAsiento;
            return View(busEntity);
        }

        [HttpPost]
        public IActionResult AddBusSeats(List<string> asiento, int idBus)
        {
            if (asiento == null || !asiento.Any())
            {
                return BadRequest("La lista de asientos no puede estar vacía.");
            }

            if (idBus <= 0)
            {
                return BadRequest("El ID del bus no es válido.");
            }


            var existingSeats = _context.TbSeating
                .Where(e => e.IdBus == idBus)
                .Select(e => new { e.Column, e.Row})
                .ToHashSet();

            foreach (var item in asiento)
            {
                var datos = item.Split('-');
                int columna = int.Parse(datos[0]);
                int fila = int.Parse(datos[1]);

                bool exists = existingSeats.Contains(new { Column = columna, Row = fila });

                if (!exists)
                {
                    Seat asi = new Seat()
                    {
                        Column = columna,
                        Row = fila,
                        IdBus = idBus,
                        Type = "1"
                    };

                    _context.TbSeating.Add(asi); 
                }

                _context.SaveChanges();
            }

            return RedirectToAction("SearchForPassage", "Pasajes");
        }
    }
}
