using Sistema_Completo_De_Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

// Clase estática que contiene funciones relacionadas con el menú de clientes
public static class ClienteMenu
{
    // Método que recibe una lista de clientes y los muestra ordenados en pantalla
    public static void ListarClientesOrdenados(List<Cliente> clientes)
    {
        // Título del listado
        Console.WriteLine("\n--- LISTADO DE CLIENTES ---");

        // Ordena la lista de clientes primero por Nombre y luego por Id
        var ordenados = clientes
            .OrderBy(c => c.Nombre)
            .ThenBy(c => c.Id)
            .ToList();

        // Verifica si la lista está vacía
        if (ordenados.Count == 0)
        {
            Console.WriteLine("No hay clientes.");
        }
        else
        {
            // Recorre la lista de clientes ordenados
            foreach (var c in ordenados)
                c.MostrarInformacion();
        }

        // Mensaje para que el usuario pueda leer la información antes de regresar al menú
        Console.WriteLine("\nPresiona una tecla para volver...");
        Console.ReadKey();
    }

    // LISTAR CLIENTES DESDE LA BASE DE DATOS
    public static void ListarClientesDB()
    {
        Console.WriteLine("\n--- CLIENTES REGISTRADOS EN LA BASE DE DATOS ---");

        // Se obtienen los clientes desde el repositorio (MySQL)
        var clientes = ClienteRepositorio.ObtenerClientes();

        if (clientes.Count == 0)
        {
            Console.WriteLine("No hay clientes registrados.");
        }
        else
        {
            foreach (var c in clientes)
            {
                c.MostrarInformacion();
            }
        }

        Console.WriteLine("\nPresiona una tecla para continuar...");
        Console.ReadKey();
    }

    // EXPORTAR CLIENTES A JSON
    public static void ExportarClientesJSON(List<Cliente> clientes)
    {
        try
        {
            string json = JsonSerializer.Serialize(clientes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("clientes.json", json);
            Console.WriteLine("Clientes exportados a clientes.json");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al exportar clientes: " + ex.Message);
        }
    }
}