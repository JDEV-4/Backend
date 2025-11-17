using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class DevolucionDTO
    {
        [JsonIgnore]
        public int Id_Devolucion { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Factura { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public string EstadoProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public int Prioridad { get; set; }
    }
}
