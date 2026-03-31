using Sistema_Completo_De_Ventas;
using SistemaVentas.DTO;  
using SistemaVentas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ClienteMenu
{
    public static void ListarClientesOrdenados(List<ClienteDTO> clientes)
    {
        Console.WriteLine("\n--- LISTADO DE CLIENTES ---");

        var ordenados = clientes
            .OrderBy(c => c.Nombre)
            .ThenBy(c => c.Id)
            .ToList();

        if (ordenados.Count == 0)
        {
            Console.WriteLine("No hay clientes.");
        }
        else
        {
            foreach (var c in ordenados)
                Console.WriteLine($"Cliente: {c.Id} | {c.Nombre} | Correo: {c.Correo} | Tel: {c.Telefono}");
        }

        Console.WriteLine("\nPresiona una tecla para volver...");
        Console.ReadKey();
    }

    public static void ListarClientesDB()
    {
        Console.WriteLine("\n--- CLIENTES REGISTRADOS EN LA BASE DE DATOS ---");

        var clientes = ClienteDAO.ObtenerClientes();

        if (clientes.Count == 0)
        {
            Console.WriteLine("No hay clientes registrados.");
        }
        else
        {
            foreach (var c in clientes)
                Console.WriteLine($"Cliente: {c.Id} | {c.Nombre} | Correo: {c.Correo} | Tel: {c.Telefono}");
        }

        Console.WriteLine("\nPresiona una tecla para continuar...");
        Console.ReadKey();
    }
}