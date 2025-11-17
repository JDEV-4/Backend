using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Services;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReporteController : ControllerBase
    {
        private readonly ReporteService _reporteService;

        // Constructor: obtenemos la cadena de conexión desde el archivo de configuración
        public ReporteController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            _reporteService = new ReporteService(connectionString);
        }

        // GET: api/reporte/compras
        [HttpGet("compras")]
        public IActionResult ObtenerReporteCompras([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin, [FromQuery] string razonSocial)
        {
            try
            {
                List<ReporteCompra> reporte = _reporteService.ObtenerReporteCompras(fechaInicio, fechaFin, razonSocial);
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/reporte/ventas
        [HttpGet("ventas")]
        public IActionResult ObtenerReporteVentas([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            try
            {
                List<ReporteVenta> reporte = _reporteService.ObtenerReporteVentas(fechaInicio, fechaFin);
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
