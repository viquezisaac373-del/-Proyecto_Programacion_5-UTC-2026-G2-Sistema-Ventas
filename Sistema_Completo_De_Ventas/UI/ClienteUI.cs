using Sistema_Completo_De_Ventas;
using SistemaVentas.DAL;
using SistemaVentas.DTO;
using SistemaVentas.BLL;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ClienteUI
{
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

        string? op = Console.ReadLine();
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

    static void Registrar(List<ClienteDTO> clientes, Dictionary<int, ClienteDTO> clientesPorId)
    {
        int id;
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

        string nombre = LeerTexto("Nombre: ");
        string correo = LeerTexto("Correo: ");
        string telefono = LeerTexto("Teléfono: ");

        var cliente = new ClienteDTO { Id = id, Nombre = nombre, Correo = correo, Telefono = telefono };

        clientes.Add(cliente);
        clientesPorId[id] = cliente;
        ClienteDAO.InsertarCliente(cliente);

        Console.WriteLine("Cliente registrado.");
        Console.ReadKey();
    }

    static void Listar()
    {
        Console.Clear();
        Console.WriteLine("--- LISTA DE CLIENTES ---");
        var lista = ClienteDAO.ObtenerClientes();
        if (lista.Count == 0) Console.WriteLine("No hay clientes.");
        else foreach (var c in lista.OrderBy(c => c.Id))
            Console.WriteLine($"{c.Id} | {c.Nombre} | {c.Correo} | {c.Telefono}");
        Console.ReadKey();
    }

    static void Buscar(Dictionary<int, ClienteDTO> clientesPorId)
    {
        int id = LeerEntero("ID a buscar: ");
        if (clientesPorId.TryGetValue(id, out var c))
            Console.WriteLine($"{c.Id} | {c.Nombre} | {c.Correo} | {c.Telefono}");
        else Console.WriteLine("No encontrado.");
        Console.ReadKey();
    }

    static void Editar(Dictionary<int, ClienteDTO> clientesPorId)
    {
        int id = LeerEntero("ID a editar: ");
        if (!clientesPorId.TryGetValue(id, out var c))
        {
            Console.WriteLine("No existe."); Console.ReadKey(); return;
        }
        Console.WriteLine($"Actual: {c.Nombre} | {c.Correo} | {c.Telefono}");
        string nuevoNombre = LeerTextoOpcional("Nuevo nombre (Enter para dejar igual): ");
        string nuevoCorreo = LeerTextoOpcional("Nuevo correo (Enter para dejar igual): ");
        string nuevoTel = LeerTextoOpcional("Nuevo teléfono (Enter para dejar igual): ");
        if (!string.IsNullOrWhiteSpace(nuevoNombre)) c.Nombre = nuevoNombre;
        if (!string.IsNullOrWhiteSpace(nuevoCorreo)) c.Correo = nuevoCorreo;
        if (!string.IsNullOrWhiteSpace(nuevoTel)) c.Telefono = nuevoTel;
        ClienteDAO.ActualizarCliente(c);
        Console.WriteLine("Actualizado."); Console.ReadKey();
    }

    static void Eliminar(List<ClienteDTO> clientes, Dictionary<int, ClienteDTO> clientesPorId, List<Venta> ventas)
    {
        int id = LeerEntero("ID a eliminar: ");
        if (!clientesPorId.TryGetValue(id, out var c))
        {
            Console.WriteLine("No existe."); Console.ReadKey(); return;
        }
        bool tieneVentas = ventas.Any(v => v.Cliente.Id == id);
        if (tieneVentas)
        {
            Console.WriteLine("No se puede eliminar: tiene ventas."); Console.ReadKey(); return;
        }
        clientes.Remove(c);
        clientesPorId.Remove(id);
        ClienteDAO.EliminarCliente(id);
        Console.WriteLine("Eliminado."); Console.ReadKey();
    }

    static void ExportarClientesJSON()
    {
        try
        {
            var lista = ClienteDAO.ObtenerClientes(); // ya devuelve List<ClienteDTO>
            JsonHelper.ExportarClientes(lista);
            Console.WriteLine("Clientes exportados a JSON correctamente.");
        }
        catch (Exception ex) { Console.WriteLine("Error al exportar: " + ex.Message); }
        Console.ReadKey();
    }

    static int LeerEntero(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            if (int.TryParse(Console.ReadLine(), out int v)) return v;
            Console.WriteLine("Valor inválido.");
        }
    }

    static string LeerTexto(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? txt = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(txt)) return txt.Trim();
            Console.WriteLine("No puede estar vacío.");
        }
    }

    static string LeerTextoOpcional(string mensaje)
    {
        Console.Write(mensaje);
        return Console.ReadLine() ?? "";
    }
}