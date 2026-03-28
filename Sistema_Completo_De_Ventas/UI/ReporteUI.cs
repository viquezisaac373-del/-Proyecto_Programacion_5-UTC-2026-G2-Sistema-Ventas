using Sistema_Completo_De_Ventas;
using SistemaVentas.BLL;
using SistemaVentas.DAL;
using System;
using System.Collections.Generic;

public static class ReporteUI
{
    public static void Menu()
    {
        Console.Clear();
        Console.WriteLine("--- REPORTES ---");
        Console.WriteLine("1. Ventas por día");
        Console.WriteLine("2. Top productos");
        Console.WriteLine("3. Ventas por cliente");
        Console.WriteLine("4. Bajo stock");
        Console.WriteLine("5. Regresar");

        string? op = Console.ReadLine();

        switch (op)
        {
            case "1": VentasPorDia(); break;
            case "2": TopProductos(); break;
            case "3": VentasPorCliente(); break;
            case "4": BajoStock(); break;
        }
    }

    static void VentasPorDia()
    {
        Console.Clear();
        Console.WriteLine("--- VENTAS POR DÍA ---");

        VentaService service = new VentaService();
        var datos = service.ObtenerVentasPorDia();

        if (datos.Count == 0)
        {
            Console.WriteLine("No hay ventas.");
        }
        else
        {
            foreach (var d in datos)
            {
                Console.WriteLine($"{d.Fecha.ToShortDateString()} | Ventas: {d.Cantidad}");
            }
        }

        Console.ReadKey();
    }

    static void TopProductos()
    {
        Console.Clear();
        Console.WriteLine("--- TOP PRODUCTOS ---");

        VentaService service = new VentaService();
        var top = service.ObtenerTopProductos();

        if (top.Count == 0)
        {
            Console.WriteLine("No hay ventas.");
        }
        else
        {
            foreach (var p in top)
            {
                Console.WriteLine($"{p.Codigo} | Vendidos: {p.Cantidad}");
            }
        }

        Console.ReadKey();
    }

    static void VentasPorCliente()
    {
        Console.Clear();
        Console.WriteLine("--- VENTAS POR CLIENTE ---");

        VentaService service = new VentaService();
        var ventas = service.ObtenerVentas();

        if (ventas.Count == 0)
        {
            Console.WriteLine("No hay ventas.");
        }
        else
        {
            var resultado = ventas
                .GroupBy(v => v.ClienteId)
                .Select(g => new
                {
                    ClienteId = g.Key,
                    Cantidad = g.Count()
                })
                .OrderByDescending(x => x.Cantidad);

            foreach (var c in resultado)
            {
                Console.WriteLine($"Cliente ID: {c.ClienteId} | Ventas: {c.Cantidad}");
            }
        }

        Console.ReadKey();
    }

    static void BajoStock()
    {
        Console.Clear();
        Console.WriteLine("--- BAJO STOCK ---");

        Console.Write("Umbral: ");
        int umbral = int.Parse(Console.ReadLine()!);

        var lista = ProductoDAO.ObtenerBajoStock(umbral);

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