using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class DetalleVenta
    {
        [JsonPropertyName("Producto")]
        public string ProductoNombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int Cantidades { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal => Cantidad * Precio;
    }
}
