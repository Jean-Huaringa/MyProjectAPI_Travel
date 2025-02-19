using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;

namespace MyProjectAPI_Travel.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsuarioController : Controller
    {
        private readonly MyProjectTravelContext _context;

        public UsuarioController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllUsuario()
        {
            var allUsuario = _context.TbUsers.ToList();
            return View(allUsuario);
        }

        [HttpGet]
        public IActionResult GetUsuarioById(int id)
        {
            var UsuarioEntity = _context.TbUsers.Find(id);
            if (UsuarioEntity is null)
                return View(new User());
            return View(UsuarioEntity);
        }

        [HttpGet]
        public IActionResult AddUsuario()
        {
            try
            {
                return View(new User());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al ingresar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddUsuario(User model)
        {
            try
            {
                var UsuarioEntity = new User()
                {
                    UserName = model.UserName,
                    Lastname = model.Lastname,
                    Phone = model.Phone,
                    Birthdate = model.Birthdate,
                    TypeDocument = model.TypeDocument,
                    NumDocument = model.NumDocument,
                    Mail = model.Mail,
                    Password = model.Password
                };

                _context.TbUsers.Add(UsuarioEntity);
                _context.SaveChanges();

                return View(UsuarioEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al ingresar el bus.", error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult UpdateUsuario(int id)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Home", "User");
            }
            try
            {
                var UsuarioEntity = _context.TbUsers.Find(id);
                if (UsuarioEntity is null)
                    return NotFound();

                _context.SaveChanges();

                return View(UsuarioEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult UpdateUsuario(int id, User model)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Home", "User");
            }
            try
            {
                var UsuarioEntity = _context.TbUsers.Find(id);
                if (UsuarioEntity is null)
                    return NotFound();

                UsuarioEntity.UserName = model.UserName;
                UsuarioEntity.Lastname = model.Lastname;
                UsuarioEntity.Phone = model.Phone;
                UsuarioEntity.Birthdate = model.Birthdate;
                UsuarioEntity.TypeDocument = model.TypeDocument;
                UsuarioEntity.NumDocument = model.NumDocument;
                UsuarioEntity.Mail = model.Mail;
                UsuarioEntity.Password = model.Password;

                _context.SaveChanges();

                return View(UsuarioEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el bus.", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteUsuario(int id)
        {
            var UsuarioEntity = _context.TbUsers.Find(id);
            if (UsuarioEntity is null)
                return NotFound();
            _context.TbUsers.Remove(UsuarioEntity);
            _context.SaveChanges();
            return Ok();
        }
    }
}
