using Backend.DataAccess;
using Backend.Models;

namespace Backend.Services
{
    public class ReporteService
    {
        private readonly ReporteDAL _reporteDAL;

        public ReporteService(string connectionString)
        {
            _reporteDAL = new ReporteDAL(connectionString);
        }

        // Método para obtener el reporte de compras
        public List<ReporteCompra> ObtenerReporteCompras(DateTime? fechaInicio, DateTime? fechaFin, string razonSocial)
        {
            return _reporteDAL.ObtenerReporteCompras(fechaInicio, fechaFin, razonSocial);
        }

        // Método para obtener el reporte de ventas
        public List<ReporteVenta> ObtenerReporteVentas(DateTime? fechaInicio, DateTime? fechaFin)
        {
            return _reporteDAL.ObtenerReporteVentas(fechaInicio, fechaFin);
        }
    }
}
