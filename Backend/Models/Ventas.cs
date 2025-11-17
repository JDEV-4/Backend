using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Ventas
    {
        [JsonPropertyName("Usuario")]
        public string UsuarioName { get; set; } = string.Empty;

        [JsonPropertyName("Cliente")]
        public string ClienteNombre { get; set; } = string.Empty;

        [JsonIgnore]
        public decimal Subtotal { get; set; }

        [JsonIgnore]
        public decimal IVA { get; set; }

        [JsonIgnore]
        public decimal Total { get; set; }
        public List<DetalleVenta> Detalles { get; set; }
    }
}
