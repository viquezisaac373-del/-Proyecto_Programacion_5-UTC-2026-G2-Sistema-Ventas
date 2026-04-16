using MySqlConnector;
using Sistema_Completo_De_Ventas;
using System;
using System.Collections.Generic;
using System.Data;
using SistemaVentas.DAL;

// Clase DAO encargada de manejar las operaciones CRUD de productos
public static class ProductoDAO
{
    // OBTENER PRODUCTOS (READ)
    public static List<Producto> ObtenerProductos()
    {
        List<Producto> productos = new List<Producto>();
        Conexion conexionDB = new Conexion();

        try
        {
            // Abre conexión a la base de datos
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                string query = "SELECT codigo, nombre, descripcion, precio, stock, descuento FROM productos";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    // Recorre los registros obtenidos
                    while (reader.Read())
                    {
                        int codigo = reader.GetInt32("codigo");
                        string nombre = reader.GetString("nombre");

                        // Manejo de valores nulos en descripción
                        string descripcion = reader.IsDBNull(
                            reader.GetOrdinal("descripcion"))
                            ? ""
                            : reader.GetString("descripcion");

                        decimal precio = reader.GetDecimal("precio");
                        int stock = reader.GetInt32("stock");

                        // Obtiene el descuento (puede ser nulo)
                        int indexDescuento = reader.GetOrdinal("descuento");
                        decimal descuento = reader.IsDBNull(indexDescuento)
                            ? 0
                            : reader.GetDecimal(indexDescuento);

                        Producto producto;

                        // Si tiene descuento, crea ProductoPromocion
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
                            // Producto normal
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
                // Inserta producto y obtiene el ID generado
                string query = @"INSERT INTO productos 
                 (nombre, descripcion, precio, stock, descuento)
                 VALUES (@nombre, @descripcion, @precio, @stock, @descuento);
                 SELECT LAST_INSERT_ID();";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Parámetros
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);

                    decimal descuento = producto.Descuento;
                    cmd.Parameters.AddWithValue("@descuento", descuento);

                    // Ejecuta y obtiene el ID generado
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
                // Consulta SQL para actualizar producto
                string query = @"UPDATE productos
                             SET nombre=@nombre,
                                 descripcion=@descripcion,
                                 precio=@precio,
                                 stock=@stock,
                                 descuento=@descuento
                             WHERE codigo=@codigo";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Parámetros
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);

                    decimal descuento = producto.Descuento;
                    cmd.Parameters.AddWithValue("@descuento", descuento);

                    // Ejecuta actualización
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

                    // Ejecuta eliminación
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al eliminar producto: " + ex.Message);
        }
    }

    // ACTUALIZAR SOLO EL STOCK
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

                    // Ejecuta actualización de stock
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al actualizar stock: " + ex.Message);
        }
    }

    // Verifica si un producto tiene ventas asociadas
    public static bool TieneVentas(int codigo)
    {
        using (MySqlConnection conn = new Conexion().ObtenerConexion())
        {
            string query = "SELECT COUNT(*) FROM venta_detalle WHERE producto_codigo = @codigo";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@codigo", codigo);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                // Retorna true si tiene ventas
                return count > 0;
            }
        }
    }

    // Obtiene productos con stock menor o igual al umbral
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
                    // Recorre productos con bajo stock
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