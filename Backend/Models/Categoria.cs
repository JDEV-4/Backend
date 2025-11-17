using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Categoria
    {
        [JsonIgnore]
        public int ID_Categoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;

        [JsonIgnore] // para que no se muestre en la respuesta JSON
        public bool EstadoBool => Estado == "Activo";
    }
}
