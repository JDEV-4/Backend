namespace Backend.Models
{
    public class ProductoListadoDTO
    {
        public string Producto { get; set; }
        public string Descripcion { get; set; }
        public string EstadoProducto { get; set; }
        public string Categoria { get; set; }
        public int Existencia { get; set; }
        public decimal Precio_Compra { get; set; }
        public decimal Precio_Venta { get; set; }
        public string Lote { get; set; }
        public string Fecha_Entrada { get; set; }
        public string Fecha_Vencimiento { get; set; }
        public string EstadoStock { get; set; }
    }

}
