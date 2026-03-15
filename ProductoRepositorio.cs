using MySqlConnector;
using Sistema_Completo_De_Ventas;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.Json;

// Clase encargada de realizar operaciones CRUD de productos en la base de datos
// Aquí se manejan consultas SQL para insertar y obtener productos desde MySQL
public static class ProductoRepositorio
{
    // Obtiene todos los productos almacenados en la base de datos
    // y los convierte en objetos Producto o ProductoPromocion
    public static List<Producto> ObtenerProductos()
    {
        List<Producto> productos = new List<Producto>();
        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                string query = "SELECT codigo, nombre, precio, stock, descuento FROM productos";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string codigo = reader.GetString("codigo");
                        string nombre = reader.GetString("nombre");
                        decimal precio = reader.GetDecimal("precio");
                        int stock = reader.GetInt32("stock");
                        decimal descuento = reader.IsDBNull("descuento") ? 0 : reader.GetDecimal("descuento");

                        Producto producto = descuento > 0
                            ? new ProductoPromocion(codigo, nombre, precio, stock, descuento)
                            : new Producto(codigo, nombre, precio, stock);

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

    // Método que inserta un producto en la base de datos
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

    // Nuevo método: Exportar productos a JSON
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
