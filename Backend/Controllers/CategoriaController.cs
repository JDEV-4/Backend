using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        // Inyectamos el servicio desde DI
        public CategoriaController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet("listar")]
        public ActionResult<IEnumerable<CategoriaDTO>> ListarCategorias()
        {
            var categorias = _categoriaService.ListarCategorias();

            if (categorias == null || categorias.Count == 0)
                return NotFound("No se encontraron categorías.");

            return Ok(categorias);
        }
    }
}
