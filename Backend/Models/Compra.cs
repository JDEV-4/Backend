namespace Backend.Models
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public string Proveedor { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal Subtotal { get; set; }
        public string Ubicacion { get; set; } = string.Empty;   
    }
}
