namespace Backend.Models
{
    public class Usuarios
    {
        public string UsuarioName { get; set; } = string.Empty;
        public string UsuarioClave { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Estado { get; set; }  = string.Empty;
        public string Sexo { get; set; } = string.Empty;
    }
}
