using Sistema_Completo_De_Ventas; // Importa las clases del sistema de ventas (por ejemplo la clase Cliente)
using System;
using System.Collections.Generic;
using System.Linq; // Necesario para usar OrderBy y ThenBy

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
            .OrderBy(c => c.Nombre) // Orden principal por nombre
            .ThenBy(c => c.Id)      // Si hay nombres iguales, ordena por Id
            .ToList();              // Convierte el resultado a lista

        // Verifica si la lista está vacía
        if (ordenados.Count == 0)
        {
            Console.WriteLine("No hay clientes.");
        }
        else
        {
            // Recorre la lista de clientes ordenados
            foreach (var c in ordenados)
                c.MostrarInformacion(); // Llama al método que muestra la información del cliente
        }

        // Mensaje para que el usuario pueda leer la información antes de regresar al menú
        Console.WriteLine("\nPresiona una tecla para volver...");
        Console.ReadKey(); // Espera a que el usuario presione una tecla
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
}