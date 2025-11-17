using Backend.DataAccess;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;



namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioDAL _usuarioDAL;
        private readonly JwtSettings _jwtSettings;

        public AuthController(UsuarioDAL usuarioDAL, IOptions<JwtSettings> jwtSettings)
        {
            _usuarioDAL = usuarioDAL;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel loginModel)
        {
            if (loginModel == null || string.IsNullOrWhiteSpace(loginModel.Usuario) || string.IsNullOrWhiteSpace(loginModel.Clave))
            {
                return BadRequest("El nombre de usuario y la contraseña son obligatorios.");
            }

            // Cambio aquí: se pasan los valores directamente
            var usuario = await _usuarioDAL.AutenticarUsuarioAsync(loginModel.Usuario, loginModel.Clave);

            if (usuario == null)
            {
                return Unauthorized("Credenciales incorrectas.");
            }

            // Generar el token
            var token = GenerarToken(usuario);

            return Ok(new
            {
                usuario.Usuario,
                usuario.Rol,
                usuario.Sexo,
                Token = token
            });
        }

        private string GenerarToken(UsuarioDTO usuario)
        {
            // Asegurarse de que los valores no sean nulos
            var usuarioNombre = usuario.Usuario ?? string.Empty;
            var usuarioRol = usuario.Rol ?? string.Empty;

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, usuarioNombre),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("Rol", usuarioRol)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.TokenLifetimeMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [Authorize]
        [HttpGet("validar")]
        public IActionResult ValidarToken()
        {
            return Ok(new { mensaje = "Token válido" });
        }
    }
}
