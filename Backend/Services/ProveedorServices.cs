using Backend.DataAccess;
using Backend.Models;

namespace Backend.Services
{
    public class ProveedorServices
    {
        private readonly ProveedorDAL _proveedorDAL;

        public ProveedorServices(string connectionString)
        {
            _proveedorDAL = new ProveedorDAL(connectionString);
        }

        public void AgregarProveedor(Proveedor proveedor)
        {
            if (string.IsNullOrWhiteSpace(proveedor.NumRuc))
                throw new ArgumentException("El RUC no puede estar vacío.");
            if (proveedor.NumRuc.Length != 11)
                throw new ArgumentException("El RUC debe tener 11 caracteres.");
            if (string.IsNullOrWhiteSpace(proveedor.RazonSocial))
                throw new ArgumentException("La razón social no puede estar vacía.");

            _proveedorDAL.InsertarProveedor(proveedor);
        }

        public List<Proveedor> ObtenerProveedores()
        {
            return _proveedorDAL.ListarProveedores();
        }

        public void ActualizarProveedor(Proveedor proveedor)
        {
            if (proveedor.ID_Proveedor <= 0)
                throw new ArgumentException("El ID del proveedor no es válido.");
            if (string.IsNullOrWhiteSpace(proveedor.Nombre))
                throw new ArgumentException("El nombre no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(proveedor.RazonSocial))
                throw new ArgumentException("La razón social no puede estar vacía.");

            _proveedorDAL.ActualizarProveedor(proveedor);
        }

        public void EliminarProveedor(int idProveedor)
        {
            if (idProveedor <= 0)
                throw new ArgumentException("El ID del proveedor no es válido.");

            _proveedorDAL.EliminarProveedor(idProveedor);
        }
    }
}
