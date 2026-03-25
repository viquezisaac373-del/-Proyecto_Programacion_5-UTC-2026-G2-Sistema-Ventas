using MySqlConnector;
using Sistema_Completo_De_Ventas;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.Json;

// Clase encargada de realizar operaciones CRUD de productos en la base de datos
public static class ProductoRepositorio
{
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
                    while (reader.Read())
                    {
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

    public static void InsertarProducto(Producto producto)
    {
        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                string query = @"INSERT INTO productos 
                             (codigo, nombre, precio, stock, descuento)
                             VALUES (@codigo, @nombre, @precio, @stock, @descuento)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);

                    decimal descuento = 0;

                    if (producto is ProductoPromocion promo)
                        descuento = promo.Descuento;

                    cmd.Parameters.AddWithValue("@descuento", descuento);

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

    // EXPORTAR PRODUCTOS A JSON
    public static void ExportarProductosJSON(List<Producto> productos)
    {
        try
        {
            string json = JsonSerializer.Serialize(productos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("productos.json", json);
            Console.WriteLine("Productos exportados a productos.json");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al exportar productos a JSON: " + ex.Message);
        }
    }
}