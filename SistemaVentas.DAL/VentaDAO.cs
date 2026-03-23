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

        public void GuardarDetalleVenta(VentaDetalleDTO detalle)
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

        public List<VentaDetalleDTO> ObtenerDetallesPorVenta(int ventaId)
        {
            // Lista donde se guardarán los resultados
            List<VentaDetalleDTO> lista = new List<VentaDetalleDTO>();

            // Abrimos conexión a la base de datos
            using (MySqlConnection conn = conexion.ObtenerConexion())
            {
                // Consulta SQL para traer los detalles de la venta
                string query = @"SELECT venta_id, producto_codigo, cantidad, precio
                         FROM venta_detalle
                         WHERE venta_id = @ventaId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Parámetro para evitar SQL Injection
                    cmd.Parameters.AddWithValue("@ventaId", ventaId);

                    // Ejecutamos la consulta
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Recorremos cada fila obtenida
                        while (reader.Read())
                        {
                            // Creamos objeto DTO y lo llenamos con los datos
                            VentaDetalleDTO detalle = new VentaDetalleDTO
                            {
                                VentaId = reader.GetInt32("venta_id"),
                                CodigoProducto = reader.GetString("producto_codigo"),
                                Cantidad = reader.GetInt32("cantidad"),
                                PrecioUnitario = reader.GetDecimal("precio")
                            };

                            // Agregamos a la lista
                            lista.Add(detalle);
                        }
                    }
                }
            }

            // Retornamos todos los detalles encontrados
            return lista;
        }

        public List<VentaDTO> ObtenerVentas()
        {
            List<VentaDTO> lista = new List<VentaDTO>();

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

            return lista;
        }


    }
}