using MySqlConnector;
using Sistema_Completo_De_Ventas;
using System;
using System.Collections.Generic;
using System.Data;
using SistemaVentas.DAL;

// Clase repositorio encargada de manejar las operaciones CRUD de productos
public static class ProductoDAO
{

    // OBTENER PRODUCTOS (READ)
    public static List<Producto> ObtenerProductos()
    {
        List<Producto> productos = new List<Producto>();
        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                string query = "SELECT codigo, nombre, descripcion, precio, stock, descuento FROM productos";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int codigo = reader.GetInt32("codigo");
                        string nombre = reader.GetString("nombre");

                        string descripcion = reader.IsDBNull(
                            reader.GetOrdinal("descripcion"))
                            ? ""
                            : reader.GetString("descripcion");

                        decimal precio = reader.GetDecimal("precio");
                        int stock = reader.GetInt32("stock");

                        int indexDescuento = reader.GetOrdinal("descuento");
                        decimal descuento = reader.IsDBNull(indexDescuento)
                            ? 0
                            : reader.GetDecimal(indexDescuento);

                        Producto producto;

                        if (descuento > 0)
                        {
                            producto = new ProductoPromocion(
                                codigo,
                                nombre,
                                precio,
                                stock,
                                descuento,
                                descripcion
                            );
                        }
                        else
                        {
                            producto = new Producto(
                                codigo,
                                nombre,
                                precio,
                                stock,
                                descripcion
                            );
                        }

                        productos.Add(producto);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al obtener productos: " + ex.Message);
        }

        return productos;
    }


    // INSERTAR PRODUCTO (CREATE)
    public static void InsertarProducto(Producto producto)
    {
        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                string query = @"INSERT INTO productos 
                 (nombre, descripcion, precio, stock, descuento)
                 VALUES (@nombre, @descripcion, @precio, @stock, @descuento);
                 SELECT LAST_INSERT_ID();";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);

                    decimal descuento = producto.Descuento;
                    cmd.Parameters.AddWithValue("@descuento", descuento);

                    int idGenerado = Convert.ToInt32(cmd.ExecuteScalar());
                    producto.Codigo = idGenerado;
                }
            }

            Console.WriteLine("Producto guardado en la base de datos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al insertar producto: " + ex.Message);
        }
    }


    // ACTUALIZAR PRODUCTO (UPDATE)
    public static void ActualizarProducto(Producto producto)
    {
        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                string query = @"UPDATE productos
                             SET nombre=@nombre,
                                 descripcion=@descripcion,
                                 precio=@precio,
                                 stock=@stock,
                                 descuento=@descuento
                             WHERE codigo=@codigo";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);

                    decimal descuento = producto.Descuento;
                    cmd.Parameters.AddWithValue("@descuento", descuento);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al actualizar producto: " + ex.Message);
        }
    }


    // ELIMINAR PRODUCTO (DELETE)
    public static void EliminarProducto(int codigo)
    {
        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                string query = "DELETE FROM productos WHERE codigo=@codigo";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@codigo", codigo);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al eliminar producto: " + ex.Message);
        }
    }


    // ACTUALIZAR SOLO STOCK
    public static void ActualizarStock(int codigo, int nuevoStock)
    {
        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                string query = "UPDATE productos SET stock = @stock WHERE codigo = @codigo";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@stock", nuevoStock);
                    cmd.Parameters.AddWithValue("@codigo", codigo);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al actualizar stock: " + ex.Message);
        }
    }


    public static bool TieneVentas(int codigo)
    {
        using (MySqlConnection conn = new Conexion().ObtenerConexion())
        {
            string query = "SELECT COUNT(*) FROM venta_detalle WHERE producto_codigo = @codigo";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@codigo", codigo);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
    }


    // 🔧 CORREGIDO — ORDEN DE PARÁMETROS
    public static List<Producto> ObtenerBajoStock(int umbral)
    {
        List<Producto> lista = new List<Producto>();
        Conexion conexionDB = new Conexion();

        using (MySqlConnection conn = conexionDB.ObtenerConexion())
        {
            string query = @"SELECT codigo, nombre, descripcion, precio, stock 
                         FROM productos 
                         WHERE stock <= @umbral";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@umbral", umbral);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Producto(
                            reader.GetInt32("codigo"),
                            reader.GetString("nombre"),
                            reader.GetDecimal("precio"),   
                            reader.GetInt32("stock"),      
                            reader.IsDBNull(
                            reader.GetOrdinal("descripcion"))
                            ? ""
                            : reader.GetString("descripcion") 
                        ));
                    }
                }
            }
        }

        return lista;
    }

}