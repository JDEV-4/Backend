using Backend.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Backend.DataAccess
{
    public class ProductoDAL
    {
        private readonly string _connectionString;

        public ProductoDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ============================================
        // MÉTODO: Insertar producto (por nombre)
        // ============================================
        public ProductoResponseDTO InsertarProducto(ProductoDTO producto)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_InsertarProducto", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento
                    cmd.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DescripcionProducto", producto.DescripcionProducto ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NombreCategoria", producto.NombreCategoria ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estado", producto.Estado);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ProductoResponseDTO
                            {
                                Mensaje = reader["Mensaje"].ToString(),
                                IdProducto = Convert.ToInt32(reader["IdProducto"])
                            };
                        }
                        else
                        {
                            return new ProductoResponseDTO
                            {
                                Mensaje = "No se recibió respuesta del procedimiento almacenado.",
                                IdProducto = 0
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string mensaje;

                if (ex.Message.Contains("Ya existe un producto"))
                    mensaje = "No se puede agregar. Ya existe un producto con ese nombre.";
                else if (ex.Message.Contains("categoría especificada no existe"))
                    mensaje = "La categoría indicada no existe.";
                else
                    mensaje = $"Error SQL: {ex.Message}";

                return new ProductoResponseDTO
                {
                    Mensaje = mensaje,
                    IdProducto = 0
                };
            }
            catch (Exception ex)
            {
                return new ProductoResponseDTO
                {
                    Mensaje = $"Error general: {ex.Message}",
                    IdProducto = 0
                };
            }
        }


        // ============================================
        // MÉTODO: Listar productos activos con paginación
        // ============================================
        public PagedResult<ProductoListadoDTO> ListarProductosActivos(PaginationParams pagination)
        {
            var result = new PagedResult<ProductoListadoDTO>
            {
                PageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber,
                PageSize = pagination.PageSize < 1 ? 10 : pagination.PageSize,
                Items = new List<ProductoListadoDTO>()
            };

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("ListarProductosActivos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento
                    cmd.Parameters.AddWithValue("@PageNumber", result.PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", result.PageSize);

                    // Parámetro de salida
                    var totalRecordsParam = new SqlParameter("@TotalRecords", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(totalRecordsParam);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Items.Add(new ProductoListadoDTO
                            {
                                Producto = reader["Producto"]?.ToString() ?? "",
                                Descripcion = reader["Descripcion"]?.ToString() ?? "",
                                EstadoProducto = reader["EstadoProducto"]?.ToString() ?? "",
                                Categoria = reader["Categoria"]?.ToString() ?? "",
                                Existencia = reader["Existencia"] != DBNull.Value ? Convert.ToInt32(reader["Existencia"]) : 0,
                                Precio_Compra = reader["Precio_Compra"] != DBNull.Value ? Convert.ToDecimal(reader["Precio_Compra"]) : 0,
                                Precio_Venta = reader["Precio_Venta"] != DBNull.Value ? Convert.ToDecimal(reader["Precio_Venta"]) : 0,
                                Lote = reader["Lote"]?.ToString() ?? "",
                                Fecha_Entrada = reader["Fecha_Entrada"]?.ToString() ?? "",
                                Fecha_Vencimiento = reader["Fecha_Vencimiento"]?.ToString() ?? "",
                                EstadoStock = reader["EstadoStock"]?.ToString() ?? "Agotado"
                            });
                        }
                    }

                    // Obtener total de registros desde el parámetro OUTPUT
                    result.TotalRecords = totalRecordsParam.Value != DBNull.Value
                                          ? Convert.ToInt32(totalRecordsParam.Value)
                                          : 0;
                }
            }
            catch (Exception ex)
            {
                result.Items = new List<ProductoListadoDTO>();
                result.TotalRecords = 0;
                Console.WriteLine($"Error en ListarProductosActivos: {ex.Message}\n{ex.StackTrace}");
            }

            return result;
        }

    }
}
