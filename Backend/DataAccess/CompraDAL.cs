using Backend.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;

namespace Backend.DataAccess
{
    public class CompraDAL
    {
        private readonly string _connectionString;

        public CompraDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public CompraResponseDTO RegistrarCompra(
            string proveedor,
            string usuario,
            string numeroFactura,
            string productos,
            string cantidades,
            string preciosCompra,
            string preciosVenta,
            string codigosLote,
            string fechasEntrada,
            string fechasVencimiento)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("sp_RegistrarCompra", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NombreProveedor", proveedor);
                    cmd.Parameters.AddWithValue("@NombreUsuario", usuario);
                    cmd.Parameters.AddWithValue("@NumFactura", numeroFactura);
                    cmd.Parameters.AddWithValue("@Productos", productos);
                    cmd.Parameters.AddWithValue("@Cantidades", cantidades);
                    cmd.Parameters.AddWithValue("@PreciosCompra", preciosCompra);
                    cmd.Parameters.AddWithValue("@PreciosVenta", preciosVenta);
                    cmd.Parameters.AddWithValue("@CodigosLote", codigosLote);

                    cmd.Parameters.AddWithValue("@FechasEntrada", ConvertirFechasIso(fechasEntrada));
                    cmd.Parameters.AddWithValue("@FechasVencimiento", ConvertirFechasIso(fechasVencimiento));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CompraResponseDTO
                            {
                                Mensaje = reader["Mensaje"].ToString(),
                                NumeroFactura = reader["NumeroFactura"].ToString()
                            };
                        }
                    }
                }
            }

            return new CompraResponseDTO
            {
                Mensaje = "No se pudo registrar la compra",
                NumeroFactura = numeroFactura
            };
        }

        // NUEVO método para llamar al procedimiento BuscarProductosActivos
        public List<Dictionary<string, object>> BuscarProductosActivos(string termino)
        {
            var productos = new List<Dictionary<string, object>>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("BuscarProductosActivos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Termino", termino ?? (object)DBNull.Value);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var producto = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                producto[reader.GetName(i)] = reader.GetValue(i);
                            }

                            productos.Add(producto);
                        }
                    }
                }
            }

            return productos;
        }

        public List<string> BuscarProveedoresPorRazonSocial(string termino)
        {
            var proveedores = new List<string>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("Buscar_Proveedores", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Termino", termino ?? (object)DBNull.Value);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            proveedores.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return proveedores;
        }

        private string ConvertirFechasIso(string fechasCsv)
        {
            if (string.IsNullOrEmpty(fechasCsv)) return "";

            var fechas = fechasCsv.Split(',');
            var fechasIso = fechas
                .Select(f =>
                {
                    if (DateTime.TryParse(f, out DateTime dt))
                        return dt.ToString("yyyy-MM-ddTHH:mm:ss");
                    return f;
                })
                .ToArray();

            return string.Join(",", fechasIso);
        }
    }
}
