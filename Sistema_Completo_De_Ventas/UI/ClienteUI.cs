using Sistema_Completo_De_Ventas;
using SistemaVentas.DAL;
using SistemaVentas.DTO;
using SistemaVentas.BLL;
using System;
using System.Collections.Generic;
using System.Linq;

// Clase estática que maneja la interfaz de usuario para clientes
public static class ClienteUI
{
    // Muestra el menú principal de clientes
    public static void Menu(List<ClienteDTO> clientes, Dictionary<int, ClienteDTO> clientesPorId, List<Venta> ventas)
    {
        Console.Clear();
        Console.WriteLine("--- CLIENTES ---");
        Console.WriteLine("1. Agregar");
        Console.WriteLine("2. Listar");
        Console.WriteLine("3. Buscar");
        Console.WriteLine("4. Editar");
        Console.WriteLine("5. Eliminar");
        Console.WriteLine("6. Exportar a JSON");
        Console.WriteLine("7. Regresar");
        Console.Write("Opción: ");

        // Lee la opción seleccionada
        string? op = Console.ReadLine();

        // Ejecuta la acción según la opción
        switch (op)
        {
            case "1": Registrar(clientes, clientesPorId); break;
            case "2": Listar(); break;
            case "3": Buscar(clientesPorId); break;
            case "4": Editar(clientesPorId); break;
            case "5": Eliminar(clientes, clientesPorId, ventas); break;
            case "6": ExportarClientesJSON(); break;
        }
    }

    // Método para registrar un nuevo cliente
    static void Registrar(List<ClienteDTO> clientes, Dictionary<int, ClienteDTO> clientesPorId)
    {
        int id;

        // Solicita el ID hasta que sea válido y no exista
        while (true)
        {
            Console.Write("ID: ");
            if (int.TryParse(Console.ReadLine(), out id))
            {
                if (!clientesPorId.ContainsKey(id)) break;
                Console.WriteLine("Ya existe un cliente con ese ID.");
            }
            else Console.WriteLine("Valor inválido.");
        }

        // Solicita los datos del cliente
        string nombre = LeerTexto("Nombre: ");
        string correo = LeerTexto("Correo: ");
        string telefono = LeerTexto("Teléfono: ");

        // Crea el objeto cliente
        var cliente = new ClienteDTO { Id = id, Nombre = nombre, Correo = correo, Telefono = telefono };

        // Agrega a las colecciones en memoria
        clientes.Add(cliente);
        clientesPorId[id] = cliente;

        // Guarda en la base de datos
        ClienteDAO.InsertarCliente(cliente);

        Console.WriteLine("Cliente registrado.");
        Console.ReadKey();
    }

    // Método para listar todos los clientes
    static void Listar()
    {
        Console.Clear();
        Console.WriteLine("--- LISTA DE CLIENTES ---");

        // Obtiene clientes desde la base de datos
        var lista = ClienteDAO.ObtenerClientes();

        if (lista.Count == 0)
            Console.WriteLine("No hay clientes.");
        else
            // Muestra clientes ordenados por ID
            foreach (var c in lista.OrderBy(c => c.Id))
                Console.WriteLine($"{c.Id} | {c.Nombre} | {c.Correo} | {c.Telefono}");

        Console.ReadKey();
    }

    // Método para buscar un cliente por ID
    static void Buscar(Dictionary<int, ClienteDTO> clientesPorId)
    {
        int id = LeerEntero("ID a buscar: ");

        // Busca en el diccionario
        if (clientesPorId.TryGetValue(id, out var c))
            Console.WriteLine($"{c.Id} | {c.Nombre} | {c.Correo} | {c.Telefono}");
        else
            Console.WriteLine("No encontrado.");

        Console.ReadKey();
    }

    // Método para editar un cliente existente
    static void Editar(Dictionary<int, ClienteDTO> clientesPorId)
    {
        int id = LeerEntero("ID a editar: ");

        // Verifica si existe el cliente
        if (!clientesPorId.TryGetValue(id, out var c))
        {
            Console.WriteLine("No existe.");
            Console.ReadKey();
            return;
        }

        // Muestra datos actuales
        Console.WriteLine($"Actual: {c.Nombre} | {c.Correo} | {c.Telefono}");

        // Solicita nuevos datos (opcionales)
        string nuevoNombre = LeerTextoOpcional("Nuevo nombre (Enter para dejar igual): ");
        string nuevoCorreo = LeerTextoOpcional("Nuevo correo (Enter para dejar igual): ");
        string nuevoTel = LeerTextoOpcional("Nuevo teléfono (Enter para dejar igual): ");

        // Actualiza solo si se ingresan valores
        if (!string.IsNullOrWhiteSpace(nuevoNombre)) c.Nombre = nuevoNombre;
        if (!string.IsNullOrWhiteSpace(nuevoCorreo)) c.Correo = nuevoCorreo;
        if (!string.IsNullOrWhiteSpace(nuevoTel)) c.Telefono = nuevoTel;

        // Guarda cambios en la base de datos
        ClienteDAO.ActualizarCliente(c);

        Console.WriteLine("Actualizado.");
        Console.ReadKey();
    }

    // Método para eliminar un cliente
    static void Eliminar(List<ClienteDTO> clientes, Dictionary<int, ClienteDTO> clientesPorId, List<Venta> ventas)
    {
        int id = LeerEntero("ID a eliminar: ");

        // Verifica si existe
        if (!clientesPorId.TryGetValue(id, out var c))
        {
            Console.WriteLine("No existe.");
            Console.ReadKey();
            return;
        }

        // Verifica si el cliente tiene ventas asociadas
        bool tieneVentas = ventas.Any(v => v.Cliente.Id == id);

        if (tieneVentas)
        {
            Console.WriteLine("No se puede eliminar: tiene ventas.");
            Console.ReadKey();
            return;
        }

        // Elimina de memoria
        clientes.Remove(c);
        clientesPorId.Remove(id);

        // Elimina de la base de datos
        ClienteDAO.EliminarCliente(id);

        Console.WriteLine("Eliminado.");
        Console.ReadKey();
    }

    // Método para exportar clientes a JSON
    static void ExportarClientesJSON()
    {
        try
        {
            // Obtiene clientes desde la base de datos
            var lista = ClienteDAO.ObtenerClientes();

            // Llama al helper para exportar
            JsonHelper.ExportarClientes(lista);

            Console.WriteLine("Clientes exportados a JSON correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al exportar: " + ex.Message);
        }

        Console.ReadKey();
    }

    // Método auxiliar para leer números enteros válidos
    static int LeerEntero(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            if (int.TryParse(Console.ReadLine(), out int v)) return v;
            Console.WriteLine("Valor inválido.");
        }
    }

    // Método auxiliar para leer texto obligatorio
    static string LeerTexto(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? txt = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(txt))
                return txt.Trim();

            Console.WriteLine("No puede estar vacío.");
        }
    }

    // Método auxiliar para leer texto opcional
    static string LeerTextoOpcional(string mensaje)
    {
        Console.Write(mensaje);
        return Console.ReadLine() ?? "";
    }
}