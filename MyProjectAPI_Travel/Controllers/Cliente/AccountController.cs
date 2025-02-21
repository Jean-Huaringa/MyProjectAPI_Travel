using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;
using MyProjectAPI_Travel.Models.DTO;
using System.Security.Claims;

namespace MyProjectAPI_Travel.Controllers.Cliente
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly MyProjectTravelContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public AccountController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                var user = _context.TbUsers
                    .Include(u => u.Worker)
                    .FirstOrDefault(u => u.Mail == model.Username);

                if (user == null)
                {
                    return BadRequest(new { message = "Usuario o contraseña incorrectos" });
                }

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                if (passwordVerificationResult != PasswordVerificationResult.Success)
                {
                    return BadRequest(new { message = "Usuario o contraseña incorrectos" });
                }

                var role = user.Worker?.Role ?? "invitado";

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


                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Sesión cerrada exitosamente" });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] UserDTO model)
        {
            try
            {

                var existingUser = _context.TbUsers.FirstOrDefault(u => u.Mail == model.Mail || u.NumDocument == model.NumDocument);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "El correo o DNI ya está registrado" });
                }

                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    throw new Exception("La contraseña no puede estar vacía");
                }

                var newUser = new User
                {
                    UserName = model.UserName,
                    Lastname = model.Lastname,
                    Phone = model.Phone,
                    Birthdate = model.Birthdate,
                    TypeDocument = model.TypeDocument,
                    NumDocument = model.NumDocument,
                    Mail = model.Mail,
                    State = true,
                    Ban = false
                };

                string passwordHash = _passwordHasher.HashPassword(newUser, model.Password);

                newUser.Password = passwordHash;

                _context.TbUsers.Add(newUser);
                _context.SaveChanges();

                return Ok(new { message = "Registro exitoso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("update-user")]
        [Authorize]
        public IActionResult UpdateUser([FromBody] UserDTO model)
        {
            try
            {
                var userEntity = VerificarSesion();

                userEntity.UserName = model.UserName;
                userEntity.Lastname = model.Lastname;
                userEntity.Phone = model.Phone;

                _context.TbUsers.Update(userEntity);
                _context.SaveChanges();

                return Ok(new { message = "Usuario actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("update-password")]
        [Authorize]
        public IActionResult UpdatePassword([FromBody] PasswordUpdateRequest model)
        {
            try
            {
                var existingUser = VerificarSesion();

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.Password, model.OldPassword);

                if (passwordVerificationResult != PasswordVerificationResult.Success)
                {
                    return BadRequest(new { message = "Contraseña actual incorrecta" });
                }

                existingUser.Password = _passwordHasher.HashPassword(existingUser, model.NewPassword);

                _context.TbUsers.Update(existingUser);
                _context.SaveChanges();

                return Ok(new { message = "Contraseña actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("get-user")]
        [Authorize]
        public IActionResult GetUser()
        {
            try
            {
                var userEntity = VerificarSesion();

                return Ok(userEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        private User VerificarSesion()
        {
            if (!User.Identity.IsAuthenticated)
            {
                throw new Exception("Usted no ha iniciado sesión.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new Exception("Ocurrió un problema con su sesión.");
            }

            var userEntity = _context.TbUsers.Find(userId);

            if (userEntity == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            return userEntity;
        }
    }

    // Modelos adicionales para solicitudes específicas
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class PasswordUpdateRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
