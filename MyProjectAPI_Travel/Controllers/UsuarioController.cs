using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProjectAPI_Travel.Data;
using MyProjectAPI_Travel.Models;
using MyProjectAPI_Travel.Models.DTO;

namespace MyProjectAPI_Travel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly MyProjectTravelContext _context;

        public UsuarioController(MyProjectTravelContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        [Authorize(Roles = "admin")]
        public IActionResult GetAllUsuario()
        {
            try
            {
                var allUsuario = _context.TbUsers.Where(e => e.State == true).ToList();

                return Ok(allUsuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "admin")]
        public IActionResult GetUsuarioById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { mensaje = "No se ingreso el ID." });
                }

                var UsuarioEntity = _context.TbUsers.Find(id);

                if (UsuarioEntity is null)
                {
                    throw new Exception("El numero de boleta no existe");
                }
                return Ok(UsuarioEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "", error = ex.Message });
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "admin")]
        public IActionResult AddUsuario([FromBody] UserDTO model)
        {
            try
            {
                if (model is null)
                {
                    return BadRequest(new { mensaje = "El objeto proporcionado es nulo." });
                }

                var UsuarioEntity = new User()
                {
                    UserName = model.UserName,
                    Lastname = model.Lastname,
                    Phone = model.Phone,
                    Birthdate = model.Birthdate,
                    TypeDocument = model.TypeDocument,
                    NumDocument = model.NumDocument,
                    Mail = model.Mail
                };

                _context.TbUsers.Add(UsuarioEntity);
                _context.SaveChanges();

                return Ok(new { mensaje = "Se agrego un nuevo usuario." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al ingresar el usuario.", error = ex.Message });
            }
        }

        [HttpPut("update/{id:int}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateUsuario(int id, [FromBody] UserDTO model)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { mensaje = "No se ingreso el ID." });
                }

                var UsuarioEntity = _context.TbUsers.Find(id);

                if (UsuarioEntity is null)
                {
                    return BadRequest(new { mensaje = "No se pudo obtener datos del usuario." });
                }

                UsuarioEntity.UserName = model.UserName;
                UsuarioEntity.Lastname = model.Lastname;
                UsuarioEntity.Phone = model.Phone;
                UsuarioEntity.Birthdate = model.Birthdate;
                UsuarioEntity.TypeDocument = model.TypeDocument;
                UsuarioEntity.NumDocument = model.NumDocument;
                UsuarioEntity.Mail = model.Mail;

                _context.SaveChanges();

                return Ok(new { mensaje = "El usuario se actualizo correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el bus.", error = ex.Message });
            }
        }

        [HttpDelete("delete/{id:int}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteUsuario(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { mensaje = "No se ingreso el ID." });
            }

            var UsuarioEntity = _context.TbUsers.Find(id);
            if (UsuarioEntity is null)
                return NotFound(new { mensaje = "Usuario no encontrado." });
            UsuarioEntity.State = false;
            _context.SaveChanges();
            return Ok(new { mensaje = "Usuario eliminado correctamente." });
        }
    }
}
