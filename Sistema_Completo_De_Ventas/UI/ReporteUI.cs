using Sistema_Completo_De_Ventas;
using SistemaVentas.BLL;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ReporteUI
{
    public static void Menu(List<Venta> ventas, List<Producto> productos)
    {
        Console.Clear();
        Console.WriteLine("--- REPORTES ---");
        Console.WriteLine("1. Ventas del día");
        Console.WriteLine("2. Top productos");
        Console.WriteLine("3. Ventas por cliente");
        Console.WriteLine("4. Bajo stock");
        Console.WriteLine("5. Regresar");

        string? op = Console.ReadLine();

        switch (op)
        {
            case "1": ListarVentas(ventas); break;
            case "2": TopProductos(ventas); break;
            case "3": VentasPorCliente(); break;
            case "4": BajoStock(productos); break;
        }
    }

    static void ListarVentas(List<Venta> ventas)
    {
        Console.Clear();
        Console.WriteLine("--- VENTAS ---");

        if (ventas.Count == 0)
        {
            Console.WriteLine("No hay ventas.");
            Console.ReadKey();
            return;
        }

        // Instancia del servicio
        VentaService service = new VentaService();

        foreach (var v in ventas)
        {
            Console.WriteLine($"\nVenta #{v.IdVenta} | Cliente: {v.Cliente.Nombre} | Total: {v.CalcularTotal()}");

            //  TRAER DETALLES DESDE BD
            var detalles = service.ObtenerDetalles(v.IdVenta);

            foreach (var d in detalles)
            {
                Console.WriteLine($"   Producto: {d.CodigoProducto} | Cantidad: {d.Cantidad} | Precio: {d.PrecioUnitario}");
            }
        }

        Console.ReadKey();
    }

    static void TopProductos(List<Venta> ventas)
    {
        Console.Clear();
        Console.WriteLine("--- TOP PRODUCTOS ---");

        if (ventas.Count == 0)
        {
            Console.WriteLine("No hay ventas.");
            Console.ReadKey();
            return;
        }

        var top = ventas
            .SelectMany(v => v.Detalles)
            .GroupBy(d => d.Producto.Codigo)
            .Select(g => new
            {
                Codigo = g.Key,
                Nombre = g.First().Producto.Nombre,
                Cantidad = g.Sum(x => x.Cantidad),
                Total = g.Sum(x => x.Subtotal)
            })
            .OrderByDescending(x => x.Cantidad)
            .Take(10)
            .ToList();

        foreach (var item in top)
        {
            Console.WriteLine($"{item.Codigo} | {item.Nombre} | Cantidad: {item.Cantidad} | Total: {item.Total}");
        }

        Console.ReadKey();
    }

    static void VentasPorCliente()
    {
        Console.Clear();
        Console.WriteLine("--- VENTAS POR CLIENTE ---");

        VentaService service = new VentaService();

        // Traemos ventas desde BD
        var ventas = service.ObtenerVentas();

        if (ventas.Count == 0)
        {
            Console.WriteLine("No hay ventas.");
            Console.ReadKey();
            return;
        }

        // Agrupar por cliente
        var resultado = ventas
            .GroupBy(v => v.ClienteId)
            .Select(g => new
            {
                ClienteId = g.Key,
                Cantidad = g.Count()
            })
            .OrderByDescending(x => x.Cantidad)
            .ToList();

        foreach (var c in resultado)
        {
            Console.WriteLine($"Cliente ID: {c.ClienteId} | Ventas: {c.Cantidad}");
        }

        Console.ReadKey();
    }

    static void BajoStock(List<Producto> productos)
    {
        Console.Clear();
        Console.WriteLine("--- BAJO STOCK ---");

        Console.Write("Umbral: ");
        int umbral = int.Parse(Console.ReadLine()!);

        var lista = productos
            .Where(p => p.Stock <= umbral)
            .OrderBy(p => p.Stock)
            .ToList();

        if (lista.Count == 0)
        {
            Console.WriteLine("No hay productos.");
        }
        else
        {
            foreach (var p in lista)
            {
                Console.WriteLine($"{p.Codigo} | {p.Nombre} | Stock: {p.Stock}");
            }
        }

        Console.ReadKey();
    }
}