using Backend.Models;
using Backend.Models.MetricsModels;
using Backend.Services;
using Backend.Services.MetricsServices; // Importar IMetrics
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly CompraService _compraService;
        private readonly IMetrics _metricsService; // Métricas

        public CompraController(CompraService compraService, IMetrics metricsService)
        {
            _compraService = compraService;
            _metricsService = metricsService;
        }

        // Endpoint para buscar productos activos
        [HttpGet("buscar-productos")]
        public IActionResult BuscarProductos([FromQuery] string? termino)
        {
            try
            {
                termino = string.IsNullOrWhiteSpace(termino) ? null : termino.Trim();
                var productos = _compraService.BuscarProductosActivos(termino);
                return Ok(new { data = productos });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // Endpoint para buscar proveedores por razón social
        [HttpGet("buscar-proveedores")]
        public IActionResult BuscarProveedores([FromQuery] string? termino)
        {
            try
            {
                termino = string.IsNullOrWhiteSpace(termino) ? null : termino.Trim();
                var proveedores = _compraService.BuscarProveedoresPorRazonSocial(termino);
                return Ok(new { data = proveedores });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarCompra([FromBody] CompraDTO compraDTO)
        {
            try
            {
                var resultado = await _compraService.RegistrarCompraAsync(compraDTO);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }


    }
}
