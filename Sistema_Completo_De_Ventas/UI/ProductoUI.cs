using Sistema_Completo_De_Ventas;
using SistemaVentas.DAL;
using SistemaVentas.DTO;
using SistemaVentas.BLL;
using System;
using System.Collections.Generic;
using System.Linq;

// Clase estática que maneja la interfaz de usuario para productos
public static class ProductoUI
{
    // Menú principal de productos
    public static void Menu(List<Producto> productos, Dictionary<int, Producto> productosPorCodigo, List<Venta> ventas)
    {
        Console.Clear();
        Console.WriteLine("--- PRODUCTOS ---");
        Console.WriteLine("1. Agregar");
        Console.WriteLine("2. Listar");
        Console.WriteLine("3. Buscar");
        Console.WriteLine("4. Editar");
        Console.WriteLine("5. Eliminar");
        Console.WriteLine("6. Exportar a JSON");
        Console.WriteLine("7. Regresar");

        // Lee la opción del usuario
        string? op = Console.ReadLine();

        // Ejecuta la acción según la opción
        switch (op)
        {
            case "1": Registrar(productos, productosPorCodigo); break;
            case "2": Listar(productos); break;
            case "3": Buscar(productosPorCodigo); break;
            case "4": Editar(productosPorCodigo); break;
            case "5": Eliminar(productos, productosPorCodigo, ventas); break;
            case "6": ExportarProductosJSON(); break;
        }
    }

    // Método para registrar un nuevo producto
    static void Registrar(List<Producto> productos, Dictionary<int, Producto> productosPorCodigo)
    {
        Console.Write("Codigo: ");
        int codigo = int.Parse(Console.ReadLine()!);

        // Verifica si el código ya existe
        if (productosPorCodigo.ContainsKey(codigo))
        {
            Console.WriteLine("Ya existe.");
            Console.ReadKey();
            return;
        }

        // Solicita datos del producto
        Console.Write("Nombre: ");
        string nombre = Console.ReadLine()!;

        Console.Write("Precio: ");
        decimal precio = decimal.Parse(Console.ReadLine()!);

        Console.Write("Stock: ");
        int stock = int.Parse(Console.ReadLine()!);

        Console.Write("Descripción: ");
        string descripcion = Console.ReadLine() ?? "";

        // Pregunta si el producto tiene descuento
        Console.Write("¿Tiene descuento? (s/n): ");
        string? resp = Console.ReadLine();

        Producto nuevo;

        // Si tiene descuento, crea un ProductoPromocion
        if (resp?.ToLower() == "s")
        {
            Console.Write("Descuento: ");
            decimal desc = decimal.Parse(Console.ReadLine()!);
            nuevo = new ProductoPromocion(codigo, nombre, precio, stock, desc, descripcion);
        }
        else
        {
            // Producto normal
            nuevo = new Producto(codigo, nombre, precio, stock, descripcion);
        }

        // Guarda en memoria
        productos.Add(nuevo);
        productosPorCodigo[codigo] = nuevo;

        // Guarda en base de datos
        ProductoDAO.InsertarProducto(nuevo);

        Console.WriteLine("Guardado.");
        Console.ReadKey();
    }

    // Método para listar productos
    static void Listar(List<Producto> productos)
    {
        // Ordena los productos por nombre
        var lista = productos.OrderBy(p => p.Nombre).ToList();

        // Muestra cada producto
        foreach (var p in lista)
        {
            Console.WriteLine($"{p.Codigo} | {p.Nombre} | {p.Descripcion} | {p.Precio} | Stock: {p.Stock}");
        }

        Console.ReadKey();
    }

    // Método para buscar un producto por código
    static void Buscar(Dictionary<int, Producto> productosPorCodigo)
    {
        Console.Write("Codigo: ");
        int codigo = int.Parse(Console.ReadLine()!);

        // Busca en el diccionario
        if (productosPorCodigo.TryGetValue(codigo, out var p))
            Console.WriteLine($"{p.Nombre} - {p.Descripcion} - {p.Precio}");
        else
            Console.WriteLine("No encontrado");

        Console.ReadKey();
    }

    // Método para editar un producto existente
    static void Editar(Dictionary<int, Producto> productosPorCodigo)
    {
        Console.Write("Codigo: ");
        int codigo = int.Parse(Console.ReadLine()!);

        // Verifica si existe
        if (!productosPorCodigo.TryGetValue(codigo, out var p))
        {
            Console.WriteLine("No existe");
            Console.ReadKey();
            return;
        }

        // Solicita nuevos datos
        Console.Write("Nuevo nombre: ");
        p.Nombre = Console.ReadLine()!;

        Console.Write("Nueva descripción: ");
        p.Descripcion = Console.ReadLine() ?? "";

        Console.Write("Nuevo precio: ");
        p.Precio = decimal.Parse(Console.ReadLine()!);

        Console.Write("Nuevo stock: ");
        p.Stock = int.Parse(Console.ReadLine()!);

        // Actualiza en base de datos
        ProductoDAO.ActualizarProducto(p);

        Console.WriteLine("Actualizado.");
        Console.ReadKey();
    }

    // Método para eliminar un producto
    static void Eliminar(List<Producto> productos, Dictionary<int, Producto> productosPorCodigo, List<Venta> ventas)
    {
        Console.Write("Codigo: ");
        int codigo = int.Parse(Console.ReadLine()!);

        // Verifica si existe
        if (!productosPorCodigo.TryGetValue(codigo, out var p))
        {
            Console.WriteLine("No existe");
            Console.ReadKey();
            return;
        }

        // Elimina de memoria
        productos.Remove(p);
        productosPorCodigo.Remove(codigo);

        // Elimina de base de datos
        ProductoDAO.EliminarProducto(codigo);

        Console.WriteLine("Eliminado.");
        Console.ReadKey();
    }

    // Método para exportar productos a JSON
    static void ExportarProductosJSON()
    {
        try
        {
            // Obtiene productos desde la base de datos
            var lista = ProductoDAO.ObtenerProductos();

            // Convierte a DTO
            var listaDTO = lista.Select(p => new ProductoDTO
            {
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock
            }).ToList();

            // Exporta usando JsonHelper
            JsonHelper.ExportarProductos(listaDTO);

            Console.WriteLine("Productos exportados a JSON correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al exportar: " + ex.Message);
        }

        Console.ReadKey();
    }
}