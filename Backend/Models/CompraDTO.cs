namespace Backend.Models
{
    public class CompraDTO
    {
        public string Proveedor { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public List<string> Productos { get; set; } = new();
        public List<int> Cantidades { get; set; } = new();
        public List<decimal> PreciosCompra { get; set; } = new();
        public List<decimal> PreciosVenta { get; set; } = new();
        public List<string> CodigosLote { get; set; } = new();
        public List<DateTime> FechasEntrada { get; set; } = new();
        public List<DateTime> FechasVencimiento { get; set; } = new();
        public string NumeroFactura { get; set; } = string.Empty;
    }
}
