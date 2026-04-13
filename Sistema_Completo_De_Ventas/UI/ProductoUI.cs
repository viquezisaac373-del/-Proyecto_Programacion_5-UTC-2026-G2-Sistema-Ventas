using Sistema_Completo_De_Ventas;
using SistemaVentas.DAL;
using SistemaVentas.DTO;
using SistemaVentas.BLL;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ProductoUI
{
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

        string? op = Console.ReadLine();

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

    static void Registrar(List<Producto> productos, Dictionary<int, Producto> productosPorCodigo)
    {
        Console.Write("Codigo: ");
        int codigo = int.Parse(Console.ReadLine()!);

        if (productosPorCodigo.ContainsKey(codigo))
        {
            Console.WriteLine("Ya existe.");
            Console.ReadKey();
            return;
        }

        Console.Write("Nombre: ");
        string nombre = Console.ReadLine()!;

        Console.Write("Precio: ");
        decimal precio = decimal.Parse(Console.ReadLine()!);

        Console.Write("Stock: ");
        int stock = int.Parse(Console.ReadLine()!);

        Console.Write("Descripción: ");
        string descripcion = Console.ReadLine() ?? "";

        Console.Write("¿Tiene descuento? (s/n): ");
        string? resp = Console.ReadLine();

        Producto nuevo;

        if (resp?.ToLower() == "s")
        {
            Console.Write("Descuento: ");
            decimal desc = decimal.Parse(Console.ReadLine()!);
            nuevo = new ProductoPromocion(codigo, nombre, precio, stock, desc, descripcion);
        }
        else
        {
            nuevo = new Producto(codigo, nombre, precio, stock, descripcion);
        }

        productos.Add(nuevo);
        productosPorCodigo[codigo] = nuevo;

        ProductoDAO.InsertarProducto(nuevo);

        Console.WriteLine("Guardado.");
        Console.ReadKey();
    }

    static void Listar(List<Producto> productos)
    {
        var lista = productos.OrderBy(p => p.Nombre).ToList();

        foreach (var p in lista)
        {
            Console.WriteLine($"{p.Codigo} | {p.Nombre} | {p.Descripcion} | {p.Precio} | Stock: {p.Stock}");
        }

        Console.ReadKey();
    }

    static void Buscar(Dictionary<int, Producto> productosPorCodigo)
    {
        Console.Write("Codigo: ");
        int codigo = int.Parse(Console.ReadLine()!);

        if (productosPorCodigo.TryGetValue(codigo, out var p))
            Console.WriteLine($"{p.Nombre} - {p.Descripcion} - {p.Precio}");
        else
            Console.WriteLine("No encontrado");

        Console.ReadKey();
    }

    static void Editar(Dictionary<int, Producto> productosPorCodigo)
    {
        Console.Write("Codigo: ");
        int codigo = int.Parse(Console.ReadLine()!);

        if (!productosPorCodigo.TryGetValue(codigo, out var p))
        {
            Console.WriteLine("No existe");
            Console.ReadKey();
            return;
        }

        Console.Write("Nuevo nombre: ");
        p.Nombre = Console.ReadLine()!;

        Console.Write("Nueva descripción: ");
        p.Descripcion = Console.ReadLine() ?? "";

        Console.Write("Nuevo precio: ");
        p.Precio = decimal.Parse(Console.ReadLine()!);

        Console.Write("Nuevo stock: ");
        p.Stock = int.Parse(Console.ReadLine()!);

        ProductoDAO.ActualizarProducto(p);

        Console.WriteLine("Actualizado.");
        Console.ReadKey();
    }

    static void Eliminar(List<Producto> productos, Dictionary<int, Producto> productosPorCodigo, List<Venta> ventas)
    {
        Console.Write("Codigo: ");
        int codigo = int.Parse(Console.ReadLine()!);

        if (!productosPorCodigo.TryGetValue(codigo, out var p))
        {
            Console.WriteLine("No existe");
            Console.ReadKey();
            return;
        }

        productos.Remove(p);
        productosPorCodigo.Remove(codigo);

        ProductoDAO.EliminarProducto(codigo);

        Console.WriteLine("Eliminado.");
        Console.ReadKey();
    }

    static void ExportarProductosJSON()
    {
        try
        {
            var lista = ProductoDAO.ObtenerProductos();

            var listaDTO = lista.Select(p => new ProductoDTO
            {
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock
            }).ToList();

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