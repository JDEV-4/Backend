using Backend.Models;
using Backend.DataAccess;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Backend.Services
{
    public class CompraService
    {
        private readonly CompraDAL _compraDAL;

        public CompraService(CompraDAL compraDAL)
        {
            _compraDAL = compraDAL;
        }

        public CompraResponseDTO RegistrarCompra(CompraDTO compra)
        {
            if (string.IsNullOrEmpty(compra.NumeroFactura))
                throw new ArgumentException("El número de factura es obligatorio.");

            if (string.IsNullOrEmpty(compra.Proveedor))
                throw new ArgumentException("El proveedor es obligatorio.");

            if (string.IsNullOrEmpty(compra.Usuario))
                throw new ArgumentException("El usuario es obligatorio.");

            if (compra.Productos.Count == 0)
                throw new ArgumentException("Debe agregar al menos un producto.");

            string productos = string.Join(",", compra.Productos);
            string cantidades = string.Join(",", compra.Cantidades);
            string preciosCompra = string.Join(",", compra.PreciosCompra);
            string preciosVenta = string.Join(",", compra.PreciosVenta);
            string codigosLote = string.Join(",", compra.CodigosLote);

            string fechasEntrada = string.Join(",", compra.FechasEntrada.Select(d => d.ToString("yyyy-MM-ddTHH:mm:ss")));
            string fechasVencimiento = string.Join(",", compra.FechasVencimiento.Select(d => d.ToString("yyyy-MM-ddTHH:mm:ss")));

            return _compraDAL.RegistrarCompra(
                compra.Proveedor,
                compra.Usuario,
                compra.NumeroFactura,
                productos,
                cantidades,
                preciosCompra,
                preciosVenta,
                codigosLote,
                fechasEntrada,
                fechasVencimiento
            );
        }

        // NUEVO método para buscar productos activos
        public List<Dictionary<string, object>> BuscarProductosActivos(string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                termino = null;

            return _compraDAL.BuscarProductosActivos(termino);
        }

        // NUEVO método para buscar proveedores por razón social
        public List<string> BuscarProveedoresPorRazonSocial(string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                termino = null;

            return _compraDAL.BuscarProveedoresPorRazonSocial(termino);
        }



        // DTO para respuesta de productos (opcional para el futuro)
        public class ProductosResponseDTO
        {
            public int TotalRecords { get; set; }
            public List<Dictionary<string, object>> Productos { get; set; }
        }
    }
}


