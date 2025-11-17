namespace Backend.Models
{
    public class ReporteCompra
    {
        public int ID_Compra { get; set; }
        public string Proveedor { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Num_Factura { get; set; } = string.Empty;
        public DateTime FechaCompra { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
    }
}
