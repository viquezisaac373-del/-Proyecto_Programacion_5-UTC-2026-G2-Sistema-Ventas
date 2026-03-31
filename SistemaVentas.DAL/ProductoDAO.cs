using MySqlConnector;
using Sistema_Completo_De_Ventas;
using System;
using System.Collections.Generic;
using System.Data;
using SistemaVentas.DAL;


// Clase repositorio encargada de manejar las operaciones CRUD de productos
// (Create, Read, Update, Delete) en la base de datos
public static class ProductoDAO
{


    // OBTENER PRODUCTOS (READ)
    // Este método consulta la base de datos y devuelve todos los productos
    // almacenados en la tabla "productos"
    public static List<Producto> ObtenerProductos()
    {
        // Lista donde se almacenarán los productos obtenidos de la base de datos
        List<Producto> productos = new List<Producto>();

        // Se crea el objeto encargado de abrir la conexión a la base de datos
        Conexion conexionDB = new Conexion();

        try
        {
            // Se abre la conexión a MySQL
            // using asegura que la conexión se cierre automáticamente
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                // Consulta SQL para obtener todos los productos
                string query = "SELECT codigo, nombre, precio, stock, descuento FROM productos";

                // Se crea el comando SQL
                using (MySqlCommand cmd = new MySqlCommand(query, conn))

                // Se ejecuta la consulta y se obtiene un lector de datos
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    // Se recorre cada fila del resultado
                    while (reader.Read())
                    {
                        // Se leen los valores de cada columna
                        string codigo = reader.GetString("codigo");
                        string nombre = reader.GetString("nombre");
                        decimal precio = reader.GetDecimal("precio");
                        int stock = reader.GetInt32("stock");

                        // Si la columna descuento es NULL se usa 0
                        int indexDescuento = reader.GetOrdinal("descuento");
                        decimal descuento = reader.IsDBNull(indexDescuento) ? 0 : reader.GetDecimal(indexDescuento);

                        Producto producto;

                        // Si el producto tiene descuento se crea como ProductoPromocion
                        if (descuento > 0)
                        {
                            producto = new ProductoPromocion(codigo, nombre, precio, stock, descuento);
                        }
                        else
                        {
                            // Si no tiene descuento se crea como Producto normal
                            producto = new Producto(codigo, nombre, precio, stock);
                        }

                        // Se agrega el producto a la lista
                        productos.Add(producto);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Manejo de errores en caso de fallo en la consulta
            Console.WriteLine("Error al obtener productos: " + ex.Message);
        }

        // Retorna la lista de productos obtenidos
        return productos;
    }



    // INSERTAR PRODUCTO (CREATE)
    // Inserta un nuevo producto en la base de datos
    public static void InsertarProducto(Producto producto)
    {
        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                // Consulta SQL para insertar un producto
                string query = @"INSERT INTO productos 
                             (codigo, nombre, precio, stock, descuento)
                             VALUES (@codigo, @nombre, @precio, @stock, @descuento)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Se asignan los valores de los parámetros
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);

                    // Por defecto el descuento es 0
                    decimal descuento = 0;

                    // Si el producto es una promoción se obtiene el descuento
                    if (producto is ProductoPromocion promo)
                        descuento = promo.Descuento;

                    cmd.Parameters.AddWithValue("@descuento", descuento);

                    // Ejecuta el INSERT en la base de datos
                    cmd.ExecuteNonQuery();
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
    // Modifica los datos de un producto existente
    public static void ActualizarProducto(Producto producto)
    {
        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                // Consulta SQL para actualizar un producto
                string query = @"UPDATE productos
                             SET nombre=@nombre, precio=@precio, stock=@stock, descuento=@descuento
                             WHERE codigo=@codigo";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Parámetros del producto a actualizar
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);

                    decimal descuento = 0;

                    // Si el producto tiene promoción se obtiene el descuento
                    if (producto is ProductoPromocion promo)
                        descuento = promo.Descuento;

                    cmd.Parameters.AddWithValue("@descuento", descuento);

                    // Ejecuta el UPDATE
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
    // Elimina un producto de la base de datos usando su código
    public static void EliminarProducto(string codigo)
    {
        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                // Consulta SQL para eliminar un producto
                string query = "DELETE FROM productos WHERE codigo=@codigo";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Se pasa el código del producto a eliminar
                    cmd.Parameters.AddWithValue("@codigo", codigo);

                    // Ejecuta el DELETE
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al eliminar producto: " + ex.Message);
        }
    }

    // ACTUALIZAR SOLO STOCK (UPDATE específico)
    // Este método reduce el stock de un producto en la base de datos
    // Se utiliza cuando se realiza una venta
    public static void ActualizarStock(string codigo, int nuevoStock)
    {
        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                // Consulta SQL para actualizar únicamente el stock
                string query = "UPDATE productos SET stock = @stock WHERE codigo = @codigo";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Se asignan los valores
                    cmd.Parameters.AddWithValue("@stock", nuevoStock);
                    cmd.Parameters.AddWithValue("@codigo", codigo);

                    // Ejecuta el UPDATE
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al actualizar stock: " + ex.Message);
        }
    }

    public static List<Producto> ObtenerBajoStock(int umbral)
    {
        List<Producto> lista = new List<Producto>();
        Conexion conexionDB = new Conexion();

        using (MySqlConnection conn = conexionDB.ObtenerConexion())
        {
            string query = @"SELECT codigo, nombre, precio, stock 
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
                            reader.GetString("codigo"),
                            reader.GetString("nombre"),
                            reader.GetDecimal("precio"),
                            reader.GetInt32("stock")
                        ));
                    }
                }
            }
        }

        return lista;
    }

}
