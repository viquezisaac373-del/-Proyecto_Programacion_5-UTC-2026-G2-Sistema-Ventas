using Sistema_Completo_De_Ventas;
using SistemaVentas.BLL;
using SistemaVentas.DAL;
using System;
using System.Collections.Generic;

// Clase estática que maneja la interfaz de usuario para reportes
public static class ReporteUI
{
    // Menú principal de reportes
    public static void Menu()
    {
        Console.Clear();
        Console.WriteLine("--- REPORTES ---");
        Console.WriteLine("1. Ventas por día");
        Console.WriteLine("2. Top productos");
        Console.WriteLine("3. Ventas por cliente");
        Console.WriteLine("4. Bajo stock");
        Console.WriteLine("5. Regresar");

        // Lee la opción seleccionada
        string? op = Console.ReadLine();

        // Ejecuta la opción elegida
        switch (op)
        {
            case "1": VentasPorDia(); break;
            case "2": TopProductos(); break;
            case "3": VentasPorCliente(); break;
            case "4": BajoStock(); break;
        }
    }

    // Muestra la cantidad de ventas agrupadas por día
    static void VentasPorDia()
    {
        Console.Clear();
        Console.WriteLine("--- VENTAS POR DÍA ---");

        // Instancia del servicio de ventas
        VentaService service = new VentaService();

        // Obtiene datos agrupados por fecha
        var datos = service.ObtenerVentasPorDia();

        // Verifica si hay datos
        if (datos.Count == 0)
        {
            Console.WriteLine("No hay ventas.");
        }
        else
        {
            // Muestra cada registro
            foreach (var d in datos)
            {
                Console.WriteLine($"{d.Fecha.ToShortDateString()} | Ventas: {d.Cantidad}");
            }
        }

        Console.ReadKey();
    }

    // Muestra los productos más vendidos
    static void TopProductos()
    {
        Console.Clear();
        Console.WriteLine("--- TOP PRODUCTOS ---");

        // Instancia del servicio
        VentaService service = new VentaService();

        // Obtiene los productos más vendidos
        var top = service.ObtenerTopProductos();

        if (top.Count == 0)
        {
            Console.WriteLine("No hay ventas.");
        }
        else
        {
            // Muestra código y cantidad vendida
            foreach (var p in top)
            {
                Console.WriteLine($"{p.Codigo} | Vendidos: {p.Cantidad}");
            }
        }

        Console.ReadKey();
    }

    // Muestra la cantidad de ventas por cliente
    static void VentasPorCliente()
    {
        Console.Clear();
        Console.WriteLine("--- VENTAS POR CLIENTE ---");

        VentaService service = new VentaService();

        // Obtiene todas las ventas
        var ventas = service.ObtenerVentas();

        if (ventas.Count == 0)
        {
            Console.WriteLine("No hay ventas.");
        }
        else
        {
            // Agrupa las ventas por cliente
            var resultado = ventas
                .GroupBy(v => v.ClienteId)
                .Select(g => new
                {
                    ClienteId = g.Key,
                    Cantidad = g.Count()
                })
                .OrderByDescending(x => x.Cantidad);

            // Muestra resultados
            foreach (var c in resultado)
            {
                Console.WriteLine($"ClienteDTO ID: {c.ClienteId} | Ventas: {c.Cantidad}");
            }
        }

        Console.ReadKey();
    }

    // Muestra productos con bajo stock según un umbral
    static void BajoStock()
    {
        Console.Clear();
        Console.WriteLine("--- BAJO STOCK ---");

        // Solicita el valor mínimo de stock
        Console.Write("Umbral: ");
        int umbral = int.Parse(Console.ReadLine()!);

        // Obtiene productos con stock bajo
        var lista = ProductoDAO.ObtenerBajoStock(umbral);

        if (lista.Count == 0)
        {
            Console.WriteLine("No hay productos.");
        }
        else
        {
            // Muestra los productos con bajo stock
            foreach (var p in lista)
            {
                Console.WriteLine($"{p.Codigo} | {p.Nombre} | Stock: {p.Stock}");
            }
        }

        Console.ReadKey();
    }
}