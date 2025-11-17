using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class UsuarioDTO
    {
        public int ID_Usuario { get; set; }
        public string? Usuario { get; set; }
        public string? Rol { get; set; }
        public string? Estado { get; set; }
        public string? Sexo { get; set; }

        [JsonIgnore]
        public string? Clave { get; set; }
    }
}
