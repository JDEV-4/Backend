using Backend.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Backend.DataAccess
{
    public class VentaDAL
    {
        private readonly string _connectionString;

        public VentaDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ClienteDTO> BuscarClientes(string termino)
        {
            var clientes = new List<ClienteDTO>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_BuscarClientes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Termino", termino ?? (object)DBNull.Value);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientes.Add(new ClienteDTO
                            {
                                IdCliente = Convert.ToInt32(reader["ID_Cliente"]),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString()
                            });
                        }
                    }
                }
            }

            return clientes;
        }


        public VentaResponseDTO RegistrarVenta(string nombreCliente, string apellidoCliente, string nombreUsuario, string productos, string cantidades)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("sp_CrearVenta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NombreCliente", nombreCliente);
                    cmd.Parameters.AddWithValue("@ApellidoCliente", apellidoCliente);
                    cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                    cmd.Parameters.AddWithValue("@Productos", productos);
                    cmd.Parameters.AddWithValue("@Cantidades", cantidades);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new VentaResponseDTO
                            {
                                Mensaje = reader["Mensaje"].ToString(),
                                NumeroFactura = reader["NumeroFactura"].ToString(),
                                IdVenta = Convert.ToInt32(reader["IdVenta"])
                            };
                        }
                    }
                }
            }

            return new VentaResponseDTO
            {
                Mensaje = "No se pudo registrar la venta.",
                NumeroFactura = null,
                IdVenta = 0
            };
        }
    }
}
