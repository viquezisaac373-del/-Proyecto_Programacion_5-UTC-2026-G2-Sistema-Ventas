using Sistema_Completo_De_Ventas;
using SistemaVentas.DTO;
using SistemaVentas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

// Clase estática que maneja las opciones del menú relacionadas con clientes
public static class ClienteMenu
{
    // Método para listar clientes ordenados por nombre y luego por ID
    public static void ListarClientesOrdenados(List<ClienteDTO> clientes)
    {
        Console.WriteLine("\n--- LISTADO DE CLIENTES ---");

        // Ordena la lista de clientes usando LINQ
        var ordenados = clientes
            .OrderBy(c => c.Nombre)   // Ordena por nombre
            .ThenBy(c => c.Id)        // Luego por ID
            .ToList();

        // Verifica si la lista está vacía
        if (ordenados.Count == 0)
        {
            Console.WriteLine("No hay clientes.");
        }
        else
        {
            // Recorre la lista y muestra cada cliente
            foreach (var c in ordenados)
                Console.WriteLine($"Cliente: {c.Id} | {c.Nombre} | Correo: {c.Correo} | Tel: {c.Telefono}");
        }

        Console.WriteLine("\nPresiona una tecla para volver...");
        Console.ReadKey();
    }

    // Método para obtener y mostrar clientes desde la base de datos
    public static void ListarClientesDB()
    {
        Console.WriteLine("\n--- CLIENTES REGISTRADOS EN LA BASE DE DATOS ---");

        // Llama al DAO para traer los clientes de la base de datos
        var clientes = ClienteDAO.ObtenerClientes();

        // Verifica si hay clientes registrados
        if (clientes.Count == 0)
        {
            Console.WriteLine("No hay clientes registrados.");
        }
        else
        {
            // Muestra los datos de cada cliente
            foreach (var c in clientes)
                Console.WriteLine($"Cliente: {c.Id} | {c.Nombre} | Correo: {c.Correo} | Tel: {c.Telefono}");
        }

        Console.WriteLine("\nPresiona una tecla para continuar...");
        Console.ReadKey();
    }
}