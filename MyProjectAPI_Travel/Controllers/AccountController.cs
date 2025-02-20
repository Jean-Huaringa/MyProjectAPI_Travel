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
            try
            {
                var user = _context.TbUsers
                    .Include(u => u.Worker)
                    .FirstOrDefault(u => u.Mail == username);

                if (user == null)
                {
                    throw new Exception("Usuario o contraseña incorrectos");
                }

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

                if (passwordVerificationResult != PasswordVerificationResult.Success)
                {
                    throw new Exception("Usuario o contraseña incorrectos");
                    throw new Exception("");
                }

                //Aqui se usara el operador (operador null-coalescing) donde si el primer parametro obtiene null se accedera al siguiente
                var role = user.Worker?.Role ?? "invitado"; // user.Worker?.Role aqui si worker es null rebotara null y se accedera al siguiente parametro 

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUsr.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Mail),
                new Claim(ClaimTypes.Role, role)
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SearchForPassage", "Pasajes");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    throw new Exception("A ocurrido un problema");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            try
            {
                var existingUser = _context.TbUsers.FirstOrDefault(u => u.Mail == model.Mail || u.NumDocument == model.NumDocument);
                if (existingUser != null)
                {
                    throw new Exception("El correo o DNI ya está registrado");
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

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
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
            try 
            {
                if (model == null)
                {
                    throw new Exception("Hemos encontrado problemas con tu usuario");
                }

                var userEntity = VerificarSesion();

                userEntity.UserName = model.UserName;
                userEntity.Lastname = model.Lastname;
                userEntity.Phone = model.Phone;

                _context.TbUsers.Update(userEntity);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet]
        public IActionResult UpdatePassword()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    throw new Exception("A ocurrido un problema");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPost]
        public IActionResult UpdatePassword(string oldPassword, string newPassword)
        {
            try
            {
                var existingUser = VerificarSesion();

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.Password, oldPassword);

                if (passwordVerificationResult != PasswordVerificationResult.Success)
                {
                    throw new Exception("Usuario o contraseña incorrectos");
                }

                if (!string.IsNullOrEmpty(newPassword))
                {
                    existingUser.Password = _passwordHasher.HashPassword(existingUser, newPassword);
                }

                _context.TbUsers.Update(existingUser);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
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
                return NotFound(ex);
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
