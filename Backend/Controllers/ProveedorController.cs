using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Services;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorController : ControllerBase
    {
        private readonly ProveedorServices _proveedorServices;

        public ProveedorController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            _proveedorServices = new ProveedorServices(connectionString);
        }

        // GET: api/proveedor
        [HttpGet]
        public IActionResult Get()
        {
            var proveedores = _proveedorServices.ObtenerProveedores();
            return Ok(proveedores);
        }

        // POST: api/proveedor
        [HttpPost]
        public IActionResult Post([FromBody] Proveedor proveedor)
        {
            try
            {
                _proveedorServices.AgregarProveedor(proveedor);
                return Ok(new { message = "Proveedor agregado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        // PUT: api/proveedor
        [HttpPut]
        public IActionResult Put([FromBody] Proveedor proveedor)
        {
            try
            {
                _proveedorServices.ActualizarProveedor(proveedor);
                return Ok(new { message = "Proveedor actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/proveedor/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _proveedorServices.EliminarProveedor(id);
                return Ok(new { message = "Proveedor eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
