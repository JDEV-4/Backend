namespace Backend.Models
{
    public class Proveedor
    {
        public int ID_Proveedor { get; set; }
        public string NumRuc { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
    }
}
