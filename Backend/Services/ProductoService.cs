using Backend.Models;
using Backend.DataAccess;
using System;

namespace Backend.Services
{
    public class ProductoService
    {
        private readonly ProductoDAL _productoDAL;

        public ProductoService(ProductoDAL productoDAL)
        {
            _productoDAL = productoDAL;
        }

        // =========================================
        // Crear producto (por nombre)
        // =========================================
        public ProductoResponseDTO CrearProducto(ProductoDTO producto)
        {
            // 🔹 Validaciones básicas
            if (producto == null)
                throw new ArgumentNullException(nameof(producto), "El producto no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(producto.NombreProducto))
                throw new ArgumentException("El nombre del producto no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(producto.NombreCategoria))
                throw new ArgumentException("Debe especificar la categoría del producto.");

            // 🔹 Llamar al DAL
            var response = _productoDAL.InsertarProducto(producto);

            // 🔹 Validar la respuesta del DAL
            if (response == null)
                return new ProductoResponseDTO
                {
                    Mensaje = "Error desconocido al insertar el producto.",
                    IdProducto = 0
                };

            return response;
        }


        // =========================================
        // Listar productos activos con paginación
        // =========================================
        public PagedResult<ProductoListadoDTO> ObtenerProductosActivos(PaginationParams pagination)
        {
            if (pagination == null)
                throw new ArgumentNullException(nameof(pagination));

            if (pagination.PageNumber <= 0)
                pagination.PageNumber = 1;

            if (pagination.PageSize <= 0)
                pagination.PageSize = 10;

            return _productoDAL.ListarProductosActivos(pagination);
        }
    }
}
