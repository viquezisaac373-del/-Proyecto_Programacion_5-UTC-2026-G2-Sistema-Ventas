using MySqlConnector;
using Sistema_Completo_De_Ventas;
using System;
using System.Collections.Generic;
using System.Data;

// Clase encargada de realizar operaciones CRUD de productos en la base de datos
// Aquí se manejan consultas SQL para insertar y obtener productos desde MySQL
public static class ProductoRepositorio
{
    // Obtiene todos los productos almacenados en la base de datos
    // y los convierte en objetos Producto o ProductoPromocion
    public static List<Producto> ObtenerProductos()
    {
        // Lista donde se almacenarán los productos obtenidos de la base de datos
        List<Producto> productos = new List<Producto>();

        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                // Consulta SQL para obtener todos los productos
                string query = "SELECT codigo, nombre, precio, stock, descuento FROM productos";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    // Se ejecuta la consulta y se recorre el resultado
                    // para crear objetos Producto en memoria
                    while (reader.Read())
                    {
                        // Se leen los datos del producto desde la base de datos
                        // Si el producto tiene descuento se crea como ProductoPromocion
                        // en caso contrario se crea como Producto normal

                        string codigo = reader.GetString("codigo");
                        string nombre = reader.GetString("nombre");
                        decimal precio = reader.GetDecimal("precio");
                        int stock = reader.GetInt32("stock");
                        decimal descuento = reader.IsDBNull("descuento") ? 0 : reader.GetDecimal("descuento");

                        Producto producto;

                        if (descuento > 0)
                        {
                            producto = new ProductoPromocion(codigo, nombre, precio, stock, descuento);
                        }
                        else
                        {
                            producto = new Producto(codigo, nombre, precio, stock);
                        }
                        // Se agrega el producto a la lista que será retornada
                        productos.Add(producto);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al obtener productos: " + ex.Message);
        }
        // Retorna la lista de productos obtenidos desde la base de datos
        return productos;
    }

    // Método que inserta un producto en la base de datos
    public static void InsertarProducto(Producto producto)
    {
        // Se crea una instancia de la clase que maneja la conexión a la base de datos
        Conexion conexionDB = new Conexion();

        try
        {
            // Se obtiene una conexión abierta a la base de datos
            // using asegura que la conexión se cierre automáticamente al terminar
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                // Consulta SQL para insertar un producto en la tabla "productos"
                // Los valores se pasan mediante parámetros para evitar SQL Injection
                string query = @"INSERT INTO productos 
                             (codigo, nombre, precio, stock, descuento)
                             VALUES (@codigo, @nombre, @precio, @stock, @descuento)";

                // Se crea el comando SQL asociado a la conexión
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Se asignan los valores de los parámetros usando los datos del objeto producto
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);

                    // Variable para guardar el descuento (por defecto 0)
                    decimal descuento = 0;

                    // Si el producto es de tipo ProductoPromocion,
                    // se obtiene el descuento específico de ese tipo de producto
                    if (producto is ProductoPromocion promo)
                        descuento = promo.Descuento;

                    // Se agrega el parámetro de descuento al comando SQL
                    cmd.Parameters.AddWithValue("@descuento", descuento);

                    // Ejecuta la consulta INSERT en la base de datos
                    cmd.ExecuteNonQuery();
                }
            }

            // Mensaje que confirma que el producto se guardó correctamente
            Console.WriteLine("Producto guardado en la base de datos.");
        }
        catch (Exception ex)
        {
            // Captura cualquier error durante la inserción y muestra el mensaje en consola
            Console.WriteLine("Error al insertar producto: " + ex.Message);
        }
    }

}