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
            try
            {
                if (!User.IsInRole("admin"))
                {
                    throw new Exception("Error");
                }

                var allBus = _context.TbBus.Where(e => e.State == true).ToList();

                return Ok(new { mensaje = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetBusById(int id)
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

                var busEntity = _context.TbBus.Find(id);

                if (busEntity is null)
                {
                    throw new Exception("El numero de boleta no existe");
                }

                return Ok(new { mensaje = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddBus(Bus model)
        {
            try
            {
                if (model is null)
                {
                    throw new Exception("");
                }

                var busEntity = new Bus()
                {
                    Placa = model.Placa,
                    Model = model.Model,
                    NumColumns = model.NumColumns,
                    NumRows = model.NumRows
                };

                _context.TbBus.Add(busEntity);
                _context.SaveChanges();

                return Ok(new { mensaje = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult UpdateBus(int id, Bus bus)
        {
            try
            {
                if (!User.IsInRole("admin"))
                {
                    throw new Exception("Error");
                }

                var busEntity = _context.TbBus.Find(id);
                if (busEntity is null)
                {
                    throw new Exception("");
                }

                busEntity.Placa = bus.Placa;
                busEntity.Model = bus.Model;
                busEntity.NumColumns = bus.NumColumns;
                busEntity.NumRows = bus.NumRows;

                _context.SaveChanges();

                return Ok(new { mensaje = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
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

            busEntity.Seating = addAsiento;

            return View(busEntity);
        }

        [HttpPost]
        public IActionResult AddBusSeats(List<string> asiento, int idBus)
        {
            try
            {
                if (asiento == null || !asiento.Any())
                {
                    throw new Exception("La lista de asientos no puede estar vacía.");
                }

                if (idBus <= 0)
                {
                    throw new Exception("El ID del bus no es válido.");
                }

                var existingSeats = _context.TbSeating
                    .Where(e => e.IdBus == idBus)
                    .Select(e => new { e.Column, e.Row })
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

                        _context.SaveChanges();
                    }
                }

                return Ok(new { mensaje = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }
    }
}
