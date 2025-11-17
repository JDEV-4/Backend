using Backend.Models;
using Backend.DataAccess;

namespace Backend.Services
{
    public class UsuarioServices
    {
        private readonly UsuarioDAL _usuarioDAL;

        // Cambiado para inyectar UsuarioDAL directamente
        public UsuarioServices(UsuarioDAL usuarioDAL)
        {
            _usuarioDAL = usuarioDAL;
        }

        // ... resto del código igual ...
    }
}
