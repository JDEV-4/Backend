using Backend.DataAccess;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioDAL _usuarioDAL;

        public UsuarioController(UsuarioDAL usuarioDAL)
        {
            _usuarioDAL = usuarioDAL;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<List<UsuarioDTO>>> ObtenerUsuarios()
        {
            try
            {
                var usuarios = await _usuarioDAL.ObtenerUsuariosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener los usuarios: {ex.Message}");
            }
        }

        // POST: api/Usuario
        [HttpPost]
        public async Task<IActionResult> InsertarUsuario([FromBody] Usuarios nuevoUsuario)
        {
            try
            {
                if (nuevoUsuario == null)
                    return BadRequest("Los datos del usuario son requeridos.");

                if (string.IsNullOrWhiteSpace(nuevoUsuario.UsuarioName) ||
                    string.IsNullOrWhiteSpace(nuevoUsuario.UsuarioClave) ||
                    string.IsNullOrWhiteSpace(nuevoUsuario.Rol) ||
                    string.IsNullOrWhiteSpace(nuevoUsuario.Estado) ||
                    string.IsNullOrWhiteSpace(nuevoUsuario.Sexo))
                {
                    return BadRequest("Todos los campos son obligatorios, incluyendo Sexo.");
                }

                int idRol = nuevoUsuario.Rol.ToLower() switch
                {
                    "administrador" => 1,
                    "vendedor" => 2,
                    _ => throw new ArgumentException("Rol no válido. Solo se permiten: Administrador, Vendedor.")
                };

                string estado = nuevoUsuario.Estado.ToLower() switch
                {
                    "activo" => "Activo",
                    "inactivo" => "Inactivo",
                    _ => throw new ArgumentException("Estado no válido. Solo se permiten: Activo o Inactivo.")
                };

                string sexo = nuevoUsuario.Sexo.ToLower() switch
                {
                    "hombre" => "H",
                    "mujer" => "M",
                    _ => throw new ArgumentException("Sexo inválido. Solo se permite Hombre o Mujer.")
                };

                string hashedPassword = PasswordHelper.EncryptPassword(nuevoUsuario.UsuarioClave);

                await _usuarioDAL.InsertarUsuarioAsync(
                    nuevoUsuario.UsuarioName,
                    hashedPassword,
                    nuevoUsuario.Rol,
                    estado,
                    sexo
                );

                return Ok(new { mensaje = "Usuario insertado correctamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al insertar usuario: {ex.Message}");
            }
        }

        // PUT: api/Usuario/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] Usuarios usuarioActualizado)
        {
            try
            {
                if (usuarioActualizado == null)
                    return BadRequest("Los datos del usuario son requeridos.");

                if (string.IsNullOrWhiteSpace(usuarioActualizado.UsuarioName) ||
                    string.IsNullOrWhiteSpace(usuarioActualizado.Rol) ||
                    string.IsNullOrWhiteSpace(usuarioActualizado.Estado) ||
                    string.IsNullOrWhiteSpace(usuarioActualizado.Sexo))
                {
                    return BadRequest("Usuario, rol, estado y sexo son obligatorios.");
                }

                int idRol = usuarioActualizado.Rol.ToLower() switch
                {
                    "administrador" => 1,
                    "vendedor" => 2,
                    _ => throw new ArgumentException("Rol no válido.")
                };

                bool estado = usuarioActualizado.Estado.ToLower() switch
                {
                    "activo" => true,
                    "inactivo" => false,
                    _ => throw new ArgumentException("Estado no válido.")
                };

                string sexo = usuarioActualizado.Sexo.ToLower() switch
                {
                    "hombre" => "H",
                    "mujer" => "M",
                    _ => throw new ArgumentException("Sexo no válido. Solo se permite Hombre o Mujer.")
                };

                string? password = usuarioActualizado.UsuarioClave;
                if (!string.IsNullOrWhiteSpace(password) && password.Length < 8)
                    return BadRequest("La contraseña debe tener al menos 8 caracteres si se proporciona.");

                await _usuarioDAL.ActualizarUsuarioAsync(
                    id,
                    usuarioActualizado.UsuarioName,
                    string.IsNullOrWhiteSpace(password) ? null : PasswordHelper.EncryptPassword(password),
                    idRol,
                    estado,
                    sexo
                );

                return Ok("Usuario actualizado correctamente.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar usuario: {ex.Message}");
            }
        }

        // PUT: api/Usuario/{id}/estado
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstadoUsuario(int id, [FromQuery] bool activo)
        {
            try
            {
                await _usuarioDAL.CambiarEstadoUsuarioAsync(id, activo);
                string mensaje = activo ? "Usuario activado correctamente." : "Usuario desactivado correctamente.";
                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al cambiar el estado del usuario: {ex.Message}");
            }
        }
    }
}
