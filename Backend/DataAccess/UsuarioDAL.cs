using Backend.Models;
using Backend.Services;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Backend.DataAccess
{
    public class UsuarioDAL
    {
        private readonly string _connectionString;

        public UsuarioDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<UsuarioDTO>> ObtenerUsuariosAsync()
        {
            var usuarios = new List<UsuarioDTO>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("obtener_usuarios", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                usuarios.Add(new UsuarioDTO
                {
                    ID_Usuario = reader.GetInt32(reader.GetOrdinal("ID_Usuario")),
                    Usuario = reader.GetString(reader.GetOrdinal("UsuarioName")),
                    Rol = reader.GetString(reader.GetOrdinal("Rol")),
                    Estado = reader.GetString(reader.GetOrdinal("Estado")),
                    Sexo = reader["Sexo"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Sexo")) : "Desconocido"
                });
            }

            return usuarios;
        }

        public async Task InsertarUsuarioAsync(string usuarioName, string hashedPassword, string nombreRol, string estado, string sexo)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("insertar_usuario", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@UsuarioName", usuarioName);
            command.Parameters.AddWithValue("@UsuarioClave", hashedPassword);
            command.Parameters.AddWithValue("@NombreRol", nombreRol);
            command.Parameters.AddWithValue("@EstadoDescripcion", estado);
            command.Parameters.AddWithValue("@Sexo", sexo);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<UsuarioDTO?> AutenticarUsuarioAsync(string usuarioName, string clave)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("AutenticarUsuario2", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Usuario", usuarioName);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var usuario = new UsuarioDTO
                {
                    ID_Usuario = Convert.ToInt32(reader["ID_Usuario"]),
                    Usuario = reader["Usuario"]?.ToString() ?? "",
                    Clave = reader["UsuarioClave"]?.ToString() ?? "",
                    Rol = reader["Rol"]?.ToString() ?? "",
                    Sexo = reader["Sexo"]?.ToString() ?? "",
                    Estado = reader["Estado"]?.ToString() ?? ""
                };

                if (usuario.Clave != null && PasswordHelper.VerifyPassword(clave, usuario.Clave))
                {
                    usuario.Clave = null;
                    return usuario;
                }
            }

            return null;
        }

        public async Task ActualizarUsuarioAsync(int idUsuario, string usuarioName, string? usuarioClave, int idRol, bool estado, string sexo)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("actualizar_usuario", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@ID_Usuario", idUsuario);
            command.Parameters.AddWithValue("@UsuarioName", usuarioName);
            command.Parameters.AddWithValue("@Id_Rol", idRol);
            command.Parameters.AddWithValue("@Estado", estado);
            command.Parameters.AddWithValue("@Sexo", sexo);

            if (string.IsNullOrWhiteSpace(usuarioClave))
                command.Parameters.AddWithValue("@UsuarioClave", DBNull.Value);
            else
                command.Parameters.AddWithValue("@UsuarioClave", usuarioClave);

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Cambia el estado de un usuario (Activo / Inactivo)
        /// </summary>
        /// <param name="idUsuario">ID del usuario</param>
        /// <param name="estado">true = Activo, false = Inactivo</param>
        public async Task CambiarEstadoUsuarioAsync(int idUsuario, bool estado)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("actualizar_estado_usuario", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@ID_Usuario", idUsuario);
            command.Parameters.AddWithValue("@Estado", estado);

            await command.ExecuteNonQueryAsync();
        }
    }
}
