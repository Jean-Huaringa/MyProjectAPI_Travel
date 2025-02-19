using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace MyProjectAPI_Travel.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyProjectTravelContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();


        public AccountController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Filter", "Itinerario");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {

            var user = _context.TbUsers
                .Include(u => u.Worker)
                .FirstOrDefault(u => u.Mail == username);

            if (user == null)
            {
                Debug.WriteLine("Contraseña incorrecta.");
                ViewBag.Error = "Usuario o contraseña incorrectos";
                return View();
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                Debug.WriteLine("Contraseña incorrecta.");
                ViewBag.Error = "Usuario o contraseña incorrectos";
                return View();
            }
            var rol = "";

            if (user.Worker != null)
            {
                rol = user.Worker.Role;
            }
            else
            {
                rol = "invitado"; 
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUsr.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Mail),
                new Claim(ClaimTypes.Role, rol)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            return RedirectToAction("SearchForPassage", "Pasajes");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SearchForPassage", "Pasajes");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Filter", "Itinerario");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            var existingUser = _context.TbUsers.FirstOrDefault(u => u.Mail == model.Mail || u.NumDocument == model.NumDocument);
            if (existingUser != null)
            {
                ViewBag.Error = "El correo o DNI ya está registrado.";
                return View(model);
            }

            string passwordHash = _passwordHasher.HashPassword(model, model.Password);

            var newUser = new User()
            {
                UserName = model.UserName,
                Lastname = model.Lastname,
                Phone = model.Phone,
                Birthdate = model.Birthdate,
                TypeDocument = model.TypeDocument,
                NumDocument = model.NumDocument,
                Mail = model.Mail,
                Password = passwordHash
            };

            _context.TbUsers.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
        
        [HttpGet]
        public IActionResult UpdateUser()
        {
            var userEntity = VerificarSesion();

            return View(userEntity);
        }

        [HttpPost]
        public IActionResult UpdateUser(User model)
        {
            if (model == null)
            {
                return NotFound(new { message = "Hemos encontrado problemas con tu usuario" });
            }

            var userEntity = VerificarSesion();

            userEntity.UserName = model.UserName;
            userEntity.Lastname = model.Lastname;
            userEntity.Phone = model.Phone;

            _context.TbUsers.Update(userEntity);
            _context.SaveChanges();

            return RedirectToAction("SearchUser");
        }

        [HttpGet]
        public IActionResult UpdatePassword()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Filter", "Itinerario");
            }
            return View();
        }

        [HttpPost]
        public IActionResult UpdatePassword(string oldPassword, string newPassword)
        {
            var existingUser = VerificarSesion();

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.Password, oldPassword);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos";
                return View();
            }

            if (!string.IsNullOrEmpty(newPassword))
            {
                existingUser.Password = _passwordHasher.HashPassword(existingUser, newPassword);
            }

            _context.TbUsers.Update(existingUser);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult SearchUser()
        {
            try
            {
                var existingUser = VerificarSesion();
                return View(existingUser);
            }
            catch (Exception ex)
            {
                return RedirectToAction("SearchForPassage", "Pasajes");
            }
        }

        private User VerificarSesion()
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

            User usuarioEntity = _context.TbUsers.Find(userId);

            return usuarioEntity;
        }
    }
}
