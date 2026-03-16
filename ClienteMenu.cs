using Sistema_Completo_De_Ventas; // Importa las clases del sistema de ventas (por ejemplo la clase Cliente)
using System;
using System.Collections.Generic;
using System.Linq; // Necesario para usar OrderBy y ThenBy
<<<<<<< HEAD
using System.IO;
using System.Text.Json;
=======
>>>>>>> a9a4d7d71e3e90e78210a8e738477de1843b3fc4

// Clase estática que contiene funciones relacionadas con el menú de clientes
public static class ClienteMenu
{
    // Método que recibe una lista de clientes y los muestra ordenados en pantalla
    public static void ListarClientesOrdenados(List<Cliente> clientes)
    {
<<<<<<< HEAD
        Console.WriteLine("\n--- LISTADO DE CLIENTES ---");

        var ordenados = clientes
            .OrderBy(c => c.Nombre)
            .ThenBy(c => c.Id)
            .ToList();

=======
        // Título del listado
        Console.WriteLine("\n--- LISTADO DE CLIENTES ---");

        // Ordena la lista de clientes primero por Nombre y luego por Id
        var ordenados = clientes
            .OrderBy(c => c.Nombre) // Orden principal por nombre
            .ThenBy(c => c.Id)      // Si hay nombres iguales, ordena por Id
            .ToList();              // Convierte el resultado a lista

        // Verifica si la lista está vacía
>>>>>>> a9a4d7d71e3e90e78210a8e738477de1843b3fc4
        if (ordenados.Count == 0)
        {
            Console.WriteLine("No hay clientes.");
        }
        else
        {
<<<<<<< HEAD
            foreach (var c in ordenados)
                c.MostrarInformacion();
        }

        Console.WriteLine("\nPresiona una tecla para volver...");
        Console.ReadKey();
    }

=======
            // Recorre la lista de clientes ordenados
            foreach (var c in ordenados)
                c.MostrarInformacion(); // Llama al método que muestra la información del cliente
        }

        // Mensaje para que el usuario pueda leer la información antes de regresar al menú
        Console.WriteLine("\nPresiona una tecla para volver...");
        Console.ReadKey(); // Espera a que el usuario presione una tecla
    }
    
>>>>>>> a9a4d7d71e3e90e78210a8e738477de1843b3fc4
    // LISTAR CLIENTES DESDE LA BASE DE DATOS
    public static void ListarClientesDB()
    {
        Console.WriteLine("\n--- CLIENTES REGISTRADOS EN LA BASE DE DATOS ---");

<<<<<<< HEAD
=======
        // Se obtienen los clientes desde el repositorio (MySQL)
>>>>>>> a9a4d7d71e3e90e78210a8e738477de1843b3fc4
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
<<<<<<< HEAD

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
=======
}
>>>>>>> a9a4d7d71e3e90e78210a8e738477de1843b3fc4
