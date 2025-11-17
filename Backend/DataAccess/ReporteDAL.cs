using Backend.Models;
using System.Data;
using System.Data.SqlClient;

namespace Backend.DataAccess
{
    public class ReporteDAL
    {
        private readonly string connectionString;

        public ReporteDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<ReporteCompra> ObtenerReporteCompras(DateTime? fechaInicio, DateTime? fechaFin, string razonSocial)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_ReporteCompras", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FechaInicio", (object)fechaInicio ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FechaFin", (object)fechaFin ?? DBNull.Value);
                    command.Parameters.AddWithValue("@RazonSocial", (object)razonSocial ?? DBNull.Value);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    List<ReporteCompra> reportes = new List<ReporteCompra>();
                    while (reader.Read())
                    {
                        reportes.Add(new ReporteCompra
                        {
                            ID_Compra = Convert.ToInt32(reader["ID_Compra"]),
                            Proveedor = reader["Proveedor"].ToString(),
                            Usuario = reader["Usuario"].ToString(),
                            Num_Factura = reader["Num_Factura"].ToString(),
                            FechaCompra = Convert.ToDateTime(reader["FechaCompra"]),
                            Subtotal = Convert.ToDecimal(reader["Subtotal"]),
                            IVA = Convert.ToDecimal(reader["IVA"]),
                            Total = Convert.ToDecimal(reader["Total"])
                        });
                    }
                    return reportes;
                }
            }
        }

        public List<ReporteVenta> ObtenerReporteVentas(DateTime? fechaInicio, DateTime? fechaFin)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GenerarReporteVentas", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FechaInicio", (object)fechaInicio ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FechaFin", (object)fechaFin ?? DBNull.Value);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    List<ReporteVenta> reportes = new List<ReporteVenta>();
                    while (reader.Read())
                    {
                        reportes.Add(new ReporteVenta
                        {
                            IDVenta = Convert.ToInt32(reader["IDVenta"]),
                            Vendedor = reader["Vendedor"].ToString(),
                            NumeroFactura = reader["NumeroFactura"].ToString(),
                            FechaVenta = Convert.ToDateTime(reader["FechaVenta"]),
                            Producto = reader["Producto"].ToString(),
                            Cantidad = Convert.ToInt32(reader["Cantidad"]),
                            Subtotal = Convert.ToDecimal(reader["Subtotal"]),
                            IVA = Convert.ToDecimal(reader["IVA"]),
                            Total = Convert.ToDecimal(reader["Total"])
                        });
                    }

                    return reportes;
                }
            }
        }
    }
}
