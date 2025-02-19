using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;
using System.Security.Claims;

namespace MyProjectAPI_Travel.Controllers
{
    public class BoleteriaController : Controller
    {
        private readonly MyProjectTravelContext _context;

        public BoleteriaController(MyProjectTravelContext context)
        {
            _context = context;
        }


        public IActionResult PrevCreateBoleto(List<string> asientosSeleccionados, int idItn, int idBus, int monto)
        {
            if (asientosSeleccionados.Count == 0)
            {
                throw new Exception("Usted aun no a seleccionado asientos");
            }

            if (!User.Identity.IsAuthenticated)
            {
                throw new Exception("Usted aun no a iniciado secion.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new Exception("Ocurrio un problema con su usuario.");
            }

            var itinerario = _context.TbItineraries
                .Include(r => r.Origin)
                .Include(r => r.Destination)
                .Include(u => u.Bus).ThenInclude(t => t.Seating)
                .FirstOrDefault(e => e.IdItn == idItn);

            Ticket boto = new Ticket()
                {
                    IdUsr = userId,
                    IdWrk = 1,
                    Itinerary = itinerario,
                    IdBus = idBus,
                    Amount = monto
                };

            ViewBag.Asientosseleccionados = asientosSeleccionados;

            return View(boto);
        }

        [HttpPost]
        public IActionResult CreateBoleto(Ticket model, List<string> nombre, List<string> apellido, List<int> edad, List<string> tipoDocumento, List<string> numDocumento, List<string> asiento)
        {
            if (!User.Identity.IsAuthenticated)
            {
                throw new Exception("Usted aun no a iniciado secion.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new Exception("Ocurrio un problema con su usuario.");
            }

            for(int x = 0; asiento.Count > x; x++){
                var datos = asiento[x].Split('-');
                int columna = int.Parse(datos[0]);
                int fila = int.Parse(datos[1]);

                Ticket boto = new Ticket()
                {
                    IdUsr = userId,
                    IdWrk = 1,
                    IdItn = model.IdItn,
                    IdBus = model.IdBus,
                    Row = fila,
                    Column = columna,
                    Amount = model.Amount,
                    PaymentMethod = model.PaymentMethod,
                    UserName = nombre[x],
                    Lastname = apellido[x],
                    Age = edad[x].ToString(),
                    TypeDocument = tipoDocumento[x],
                    NumDocument = numDocumento[x]
                };

                _context.TbTickets.Add(boto);
            }

            return View();
        }
    }
}
