namespace Backend.Models
{
    public class VentaDTO
    {
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string NombreUsuario { get; set; }
        public List<string> Productos { get; set; }
        public List<int> Cantidades { get; set; }
    }

    public class VentaResponseDTO
    {
        public string Mensaje { get; set; }
        public string NumeroFactura { get; set; }
        public int IdVenta { get; set; }
    }
}
