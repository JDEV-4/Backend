namespace Backend.Models
{
    public class DevolucionRequest
    {
        public string Usuario { get; set; } = string.Empty;
        public string NumeroFactura { get; set; } = string.Empty;
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = string.Empty;  
        public int Prioridad { get; set; } = 1;
    }
}
