using System.Data;
using System.Data.SqlClient;
using Backend.Models;


namespace Backend.DataAccess
{
    public class ClienteDAL
    {
        private readonly string _connectionString;

        public ClienteDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para insertar un cliente
        public void InsertarCliente(string nombre, string apellido, string telefono)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("insertar_cliente", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Apellido", apellido);
                cmd.Parameters.AddWithValue("@Telefono", telefono);

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // Capturar el error específico generado por el procedimiento almacenado
                    if (ex.Message.Contains("Ya existe un cliente con el mismo nombre"))
                    {
                        throw new InvalidOperationException("Ya existe un cliente con el mismo nombre.");
                    }
                    throw; // Re-lanzar la excepción si no es un error esperado
                }
            }
        }


        // Método para listar clientes
        public List<Cliente> ListarClientes()
        {
            List<Cliente> clientes = new List<Cliente>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("listar_clientes", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Cliente cliente = new Cliente
                        {
                            ID_Cliente = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Telefono = reader.GetString(3)
                        };
                        clientes.Add(cliente);
                    }
                }
            }

            return clientes;
        }

        // Método para actualizar un cliente
        public void ActualizarCliente(int idCliente, string nombre, string apellido, string telefono)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("actualizar_cliente", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_Cliente", idCliente);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Apellido", apellido);
                cmd.Parameters.AddWithValue("@Telefono", telefono);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Método para eliminar un cliente
        public void EliminarCliente(int idCliente)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("eliminar_cliente", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_Cliente", idCliente);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
