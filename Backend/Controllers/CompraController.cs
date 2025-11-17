using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly CompraService _compraService;

        public CompraController(CompraService compraService)
        {
            _compraService = compraService;
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
        public IActionResult RegistrarCompra([FromBody] CompraDTO compraDTO)
        {
            try
            {
                var resultado = _compraService.RegistrarCompra(compraDTO);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
