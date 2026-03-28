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

        public int GuardarVenta(VentaDTO venta)
        {
            try
            {
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    string query = @"INSERT INTO ventas (cliente_id, fecha)
                                     VALUES (@cliente, @fecha);
                                     SELECT LAST_INSERT_ID();";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cliente", venta.ClienteId);
                        cmd.Parameters.AddWithValue("@fecha", venta.Fecha);

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar venta: " + ex.Message);
            }
        }

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
                        cmd.Parameters.AddWithValue("@producto", detalle.CodigoProducto);
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
                                    CodigoProducto = reader.GetString("producto_codigo"),
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

        public List<VentaDTO> ObtenerVentas()
        {
            List<VentaDTO> lista = new List<VentaDTO>();

            try
            {
                using (MySqlConnection conn = conexion.ObtenerConexion())
                {
                    string query = @"SELECT id, cliente_id, fecha FROM ventas";

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
                                    Fecha = reader.GetDateTime("fecha")
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

        public List<(string Codigo, int Cantidad)> ObtenerTopProductos()
        {
            var lista = new List<(string, int)>();

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
                            reader.GetString("producto_codigo"),
                            reader.GetInt32("total")
                        ));
                    }
                }
            }

            return lista;
        }
    }
}