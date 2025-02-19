using Microsoft.AspNetCore.Mvc;

namespace MyProjectAPI_Travel.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult MensajeError()
        {
            return View();
        }
    }
}
