using Backend.DataAccess;
using Backend.Models;
using Backend.Models.MetricsModels;
using Backend.Services.MetricsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class CompraService
    {
        private readonly CompraDAL _compraDAL;
        private readonly IMetrics _metricsService;

        public CompraService(CompraDAL compraDAL, IMetrics metricsService)
        {
            _compraDAL = compraDAL;
            _metricsService = metricsService;
        }

        public async Task<CompraResponseDTO> RegistrarCompraAsync(CompraDTO compra)
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

            try
            {
                // Registrar la compra en SQL Server
                var resultado = _compraDAL.RegistrarCompra(
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

                // Métrica de transacción exitosa
                var metric = new TransactionMetric
                {
                    TransactionName = "Compra Registrada",
                    UserId = compra.Usuario,
                    Success = true,
                    Amount = compra.PreciosCompra.Sum(),
                    ItemsCount = compra.Productos.Count
                };

                await _metricsService.RecordEventAsync<TransactionMetric>(metric);

                return resultado;
            }
            catch (Exception ex)
            {
                // Métrica de transacción fallida
                var metric = new TransactionMetric
                {
                    TransactionName = "Compra Fallida",
                    UserId = compra.Usuario,
                    Success = false,
                    Amount = compra.PreciosCompra.Sum(),
                    ItemsCount = compra.Productos.Count
                };

                await _metricsService.RecordEventAsync<TransactionMetric>(metric);

                throw; // re-lanzar excepción para que el controller lo maneje
            }
        }

        // Métodos existentes de búsqueda de productos y proveedores
        public List<Dictionary<string, object>> BuscarProductosActivos(string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                termino = null;

            return _compraDAL.BuscarProductosActivos(termino);
        }

        public List<string> BuscarProveedoresPorRazonSocial(string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                termino = null;

            return _compraDAL.BuscarProveedoresPorRazonSocial(termino);
        }

        // DTO opcional para respuesta de productos
        public class ProductosResponseDTO
        {
            public int TotalRecords { get; set; }
            public List<Dictionary<string, object>> Productos { get; set; }
        }
    }
}
