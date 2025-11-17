using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteServices _clienteServices;

        // Constructor que recibe el servicio de clientes
        public ClienteController(ClienteServices clienteServices)
        {
            _clienteServices = clienteServices;
        }

        // POST: api/cliente
        // Método para agregar un cliente
        [HttpPost]
        public IActionResult AgregarCliente([FromBody] Cliente cliente)
        {
            try
            {
                if (cliente == null)
                    return BadRequest("Los datos del cliente no pueden ser nulos.");

                // Llamamos al servicio para agregar el cliente
                _clienteServices.AgregarCliente(cliente.Nombre, cliente.Apellido, cliente.Telefono);
                return CreatedAtAction(nameof(ObtenerCliente), new { id = cliente.ID_Cliente }, cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/cliente
        // Método para listar todos los clientes
        [HttpGet]
        public IActionResult ObtenerClientes()
        {
            try
            {
                var clientes = _clienteServices.ObtenerClientes();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/cliente/{id}
        [HttpPut("{id}")]
        public IActionResult ActualizarCliente(int id, [FromBody] Cliente cliente)
        {
            try
            {
                if (cliente == null)
                    return BadRequest("Los datos del cliente no pueden ser nulos.");

                if (id != cliente.ID_Cliente)
                    return BadRequest("El ID del cliente no coincide.");

                _clienteServices.ActualizarCliente(id, cliente.Nombre, cliente.Apellido, cliente.Telefono);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        // DELETE: api/cliente/{id}
        [HttpDelete("{id}")]
        public IActionResult EliminarCliente(int id)
        {
            try
            {
                _clienteServices.EliminarCliente(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        // GET: api/cliente/{id}
        [HttpGet("{id}")]
        public IActionResult ObtenerCliente(int id)
        {
            try
            {
                var cliente = _clienteServices.ObtenerClientes().Find(c => c.ID_Cliente == id);
                if (cliente == null)
                {
                    return NotFound("Cliente no encontrado.");
                }
                return Ok(cliente); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }
    }
}
