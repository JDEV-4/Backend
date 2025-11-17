using Backend.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Backend.DataAccess
{
    public class CategoriaDAL
    {
        private readonly string _connectionString;

        // Constructor que recibe el connection string
        public CategoriaDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<CategoriaDTO> ListarCategorias()
        {
            var categorias = new List<CategoriaDTO>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("listar_categorias", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categorias.Add(new CategoriaDTO
                            {
                                ID_Categoria = Convert.ToInt32(reader["ID_Categoria"]),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                Estado = reader["Estado"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar categorías: {ex.Message}");
                categorias = new List<CategoriaDTO>();
            }

            return categorias;
        }
    }
}
