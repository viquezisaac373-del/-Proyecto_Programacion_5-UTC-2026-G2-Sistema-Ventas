using MySqlConnector;
using Sistema_Completo_De_Ventas;
using SistemaVentas.DTO;
using System;
using System.Collections.Generic;

namespace SistemaVentas.DAL
{
    public class VentaDAO
    {
        private Conexion conexion = new Conexion();

        // ✅ GUARDAR VENTA
        public int GuardarVenta(VentaDTO venta)
        {
            try
            {
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    string query = @"INSERT INTO ventas (cliente_id, fecha, subtotal, impuesto, total)
                                     VALUES (@cliente, @fecha, @subtotal, @impuesto, @total);
                                     SELECT LAST_INSERT_ID();";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cliente", venta.ClienteId);
                        cmd.Parameters.AddWithValue("@fecha", venta.Fecha);
                        cmd.Parameters.AddWithValue("@subtotal", venta.Subtotal);
                        cmd.Parameters.AddWithValue("@impuesto", venta.Impuesto);
                        cmd.Parameters.AddWithValue("@total", venta.Total);

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar venta: " + ex.Message);
            }
        }

        // ✅ GUARDAR DETALLE
        public void GuardarDetalleVenta(VentaDetalleDTO detalle)
        {
            try
            {
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    string query = @"INSERT INTO venta_detalle
                                     (venta_id, producto_codigo, cantidad, precio)
                                     VALUES (@venta, @producto, @cantidad, @precio)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@venta", detalle.VentaId);

                        // 🔥 CORREGIDO: ahora es INT
                        cmd.Parameters.AddWithValue("@producto", detalle.CodigoP);

                        cmd.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                        cmd.Parameters.AddWithValue("@precio", detalle.PrecioUnitario);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar detalle de venta: " + ex.Message);
            }
        }

        // ✅ OBTENER DETALLES
        public List<VentaDetalleDTO> ObtenerDetallesPorVenta(int ventaId)
        {
            List<VentaDetalleDTO> lista = new List<VentaDetalleDTO>();

            try
            {
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    string query = @"SELECT venta_id, producto_codigo, cantidad, precio
                                     FROM venta_detalle
                                     WHERE venta_id = @ventaId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ventaId", ventaId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VentaDetalleDTO detalle = new VentaDetalleDTO
                                {
                                    VentaId = reader.GetInt32("venta_id"),

                                    // 🔥 CORREGIDO: INT en vez de string
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

        // ✅ OBTENER VENTAS
        public List<VentaDTO> ObtenerVentas()
        {
            List<VentaDTO> lista = new List<VentaDTO>();

            try
            {
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    string query = @"SELECT id, cliente_id, fecha, subtotal, impuesto, total FROM ventas";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
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

        // ✅ VENTAS POR DÍA
        public List<(DateTime Fecha, int Cantidad)> ObtenerVentasPorDia()
        {
            var lista = new List<(DateTime, int)>();

            using (var conn = conexion.ObtenerConexion())
            {
                string query = @"SELECT DATE(fecha) as fecha, COUNT(*) as cantidad
                                 FROM ventas
                                 GROUP BY DATE(fecha)
                                 ORDER BY fecha DESC";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
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

        // ✅ TOP PRODUCTOS (CORREGIDO A INT)
        public List<(int Codigo, int Cantidad)> ObtenerTopProductos()
        {
            var lista = new List<(int, int)>();

            using (var conn = conexion.ObtenerConexion())
            {
                string query = @"SELECT producto_codigo, SUM(cantidad) as total
                                 FROM venta_detalle
                                 GROUP BY producto_codigo
                                 ORDER BY total DESC
                                 LIMIT 10";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add((
                            reader.GetInt32("producto_codigo"), // 🔥 INT
                            reader.GetInt32("total")
                        ));
                    }
                }
            }

            return lista;
        }
    }
}