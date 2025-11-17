using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _productoService;
        private readonly CategoriaService _categoriaService;

        public ProductoController(ProductoService productoService, CategoriaService categoriaService)
        {
            _productoService = productoService;
            _categoriaService = categoriaService;
        }

        // =========================================
        // Endpoint para listar categorías
        // =========================================
        [HttpGet("categorias")]
        public ActionResult<IEnumerable<CategoriaDTO>> ListarCategorias()
        {
            var categorias = _categoriaService.ListarCategorias();

            if (categorias == null || categorias.Count == 0)
                return NotFound("No se encontraron categorías.");

            return Ok(categorias);
        }

        // =========================================
        // Crear producto
        // =========================================
        [HttpPost("crear")]
        public IActionResult CrearProducto([FromBody] ProductoDTO producto)
        {
            if (producto == null)
                return BadRequest(new { mensaje = "El objeto producto no puede ser nulo." });

            try
            {
                var resultado = _productoService.CrearProducto(producto);

                if (resultado == null)
                    return StatusCode(500, new { mensaje = "Error al insertar el producto." });

                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        // =========================================
        // Listar productos activos con paginación
        // =========================================
        [HttpGet("activos")]
        public IActionResult ListarProductosActivos([FromQuery] PaginationParams pagination)
        {
            try
            {
                var resultado = _productoService.ObtenerProductosActivos(pagination);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }
    }
}
