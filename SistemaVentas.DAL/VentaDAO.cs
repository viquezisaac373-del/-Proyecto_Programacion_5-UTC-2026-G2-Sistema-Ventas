using MySqlConnector;
using Sistema_Completo_De_Ventas;
using SistemaVentas.DTO;
using System;
using System.Collections.Generic;

namespace SistemaVentas.DAL
{
    // Clase DAO encargada de acceder a la base de datos para todo lo relacionado con ventas
    public class VentaDAO
    {
        // Instancia de la conexión a la base de datos
        private Conexion conexion = new Conexion();

        // Método para guardar una venta en la base de datos
        // Retorna el ID generado automáticamente
        public int GuardarVenta(VentaDTO venta)
        {
            try
            {
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    // Inserta la venta y obtiene el último ID insertado
                    string query = @"INSERT INTO ventas (cliente_id, fecha, subtotal, impuesto, total)
                                     VALUES (@cliente, @fecha, @subtotal, @impuesto, @total);
                                     SELECT LAST_INSERT_ID();";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Se envían los datos como parámetros (evita SQL Injection)
                        cmd.Parameters.AddWithValue("@cliente", venta.ClienteId);
                        cmd.Parameters.AddWithValue("@fecha", venta.Fecha);
                        cmd.Parameters.AddWithValue("@subtotal", venta.Subtotal);
                        cmd.Parameters.AddWithValue("@impuesto", venta.Impuesto);
                        cmd.Parameters.AddWithValue("@total", venta.Total);

                        // Ejecuta la consulta y retorna el ID generado
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al guardar venta: " + ex.Message);
            }
        }

        // Método para guardar el detalle de una venta (productos vendidos)
        public void GuardarDetalleVenta(VentaDetalleDTO detalle)
        {
            try
            {
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    // Inserta cada producto vendido en la tabla detalle
                    string query = @"INSERT INTO venta_detalle
                                     (venta_id, producto_codigo, cantidad, precio)
                                     VALUES (@venta, @producto, @cantidad, @precio)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Parámetros del detalle
                        cmd.Parameters.AddWithValue("@venta", detalle.VentaId);
                        cmd.Parameters.AddWithValue("@producto", detalle.CodigoP);
                        cmd.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                        cmd.Parameters.AddWithValue("@precio", detalle.PrecioUnitario);

                        // Ejecuta la inserción
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar detalle de venta: " + ex.Message);
            }
        }

        // Método para obtener los detalles de una venta específica
        public List<VentaDetalleDTO> ObtenerDetallesPorVenta(int ventaId)
        {
            List<VentaDetalleDTO> lista = new List<VentaDetalleDTO>();

            try
            {
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    // Consulta filtrada por ID de venta
                    string query = @"SELECT venta_id, producto_codigo, cantidad, precio
                                     FROM venta_detalle
                                     WHERE venta_id = @ventaId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ventaId", ventaId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Se recorren los resultados
                            while (reader.Read())
                            {
                                VentaDetalleDTO detalle = new VentaDetalleDTO
                                {
                                    VentaId = reader.GetInt32("venta_id"),
                                    CodigoP = reader.GetInt32("producto_codigo"),
                                    Cantidad = reader.GetInt32("cantidad"),
                                    PrecioUnitario = reader.GetDecimal("precio")
                                };

                                lista.Add(detalle);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener detalles de venta: " + ex.Message);
            }

            return lista;
        }

        // Método para obtener todas las ventas registradas
        public List<VentaDTO> ObtenerVentas()
        {
            List<VentaDTO> lista = new List<VentaDTO>();

            try
            {
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    // Consulta general de ventas
                    string query = @"SELECT id, cliente_id, fecha, subtotal, impuesto, total FROM ventas";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Se recorren los registros
                            while (reader.Read())
                            {
                                VentaDTO venta = new VentaDTO
                                {
                                    Id = reader.GetInt32("id"),
                                    ClienteId = reader.GetInt32("cliente_id"),
                                    Fecha = reader.GetDateTime("fecha"),
                                    Subtotal = reader.GetDecimal("subtotal"),
                                    Impuesto = reader.GetDecimal("impuesto"),
                                    Total = reader.GetDecimal("total")
                                };

                                lista.Add(venta);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener ventas: " + ex.Message);
            }

            return lista;
        }

        // Método que obtiene la cantidad de ventas agrupadas por día
        public List<(DateTime Fecha, int Cantidad)> ObtenerVentasPorDia()
        {
            var lista = new List<(DateTime, int)>();

            using (var conn = conexion.ObtenerConexion())
            {
                // Agrupa las ventas por fecha
                string query = @"SELECT DATE(fecha) as fecha, COUNT(*) as cantidad
                                 FROM ventas
                                 GROUP BY DATE(fecha)
                                 ORDER BY fecha DESC";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    // Se recorren los resultados
                    while (reader.Read())
                    {
                        lista.Add((
                            reader.GetDateTime("fecha"),
                            reader.GetInt32("cantidad")
                        ));
                    }
                }
            }

            return lista;
        }

        // Método que obtiene los productos más vendidos
        public List<(int Codigo, int Cantidad)> ObtenerTopProductos()
        {
            var lista = new List<(int, int)>();

            using (var conn = conexion.ObtenerConexion())
            {
                // Agrupa productos y suma la cantidad vendida
                string query = @"SELECT producto_codigo, SUM(cantidad) as total
                                 FROM venta_detalle
                                 GROUP BY producto_codigo
                                 ORDER BY total DESC
                                 LIMIT 10";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    // Se recorren los resultados
                    while (reader.Read())
                    {
                        lista.Add((
                            reader.GetInt32("producto_codigo"),
                            reader.GetInt32("total")
                        ));
                    }
                }
            }

            return lista;
        }
    }
}