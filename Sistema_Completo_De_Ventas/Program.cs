using MySqlConnector;
using Sistema_Completo_De_Ventas;
using SistemaVentas.BLL;
using SistemaVentas.DTO;
using SistemaVentas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    // Ambos usan el mismo tipo: SistemaVentas.DTO.ClienteDTO
    static List<ClienteDTO> clientes = new List<ClienteDTO>();
    static List<Producto> productos = new List<Producto>();
    static List<Venta> ventas = new List<Venta>();

    static Dictionary<int, ClienteDTO> clientesPorId = new Dictionary<int, ClienteDTO>();
    static Dictionary<string, Producto> productosPorCodigo =
        new Dictionary<string, Producto>(StringComparer.OrdinalIgnoreCase);

    static void Main(string[] args)
    {
        CargarDatosDePrueba();
        ReconstruirDiccionarios();

        ConectarADb();
        Console.WriteLine("Presione una tecla para iniciar");
        Console.ReadLine();

        bool continuar = true;
        while (continuar)
        {
            Console.Clear();
            Console.WriteLine("=== SISTEMA DE VENTAS ===");
            Console.WriteLine("1. Menu Clientes");
            Console.WriteLine("2. Menu Productos");
            Console.WriteLine("3. Vender (Facturacion)");
            Console.WriteLine("4. Reportes (LINQ)");
            Console.WriteLine("5. Salir");
            Console.Write("Elige una opcion: ");

            string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    ClienteUI.Menu(clientes, clientesPorId, ventas); break;
                case "2":
                    ProductoUI.Menu(productos, productosPorCodigo, ventas); break;
                case "3":
                    VentaUI.RealizarVenta(clientes, clientesPorId, productos, productosPorCodigo, ventas); break;
                case "4":
                    ReporteUI.Menu(); break;
                case "5":
                    continuar = false;
                    Console.WriteLine("Saliendo del sistema...");
                    break;
                default:
                    Console.WriteLine("Esa opcion no existe. Presiona Enter.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void CargarDatosDePrueba()
    {
        var clienteService = new ClienteService();
        clientes = clienteService.ObtenerClientes(); // Devuelve List<ClienteDTO> ✅
        clientesPorId = clientes.ToDictionary(c => c.Id, c => c); // ✅ mismo tipo

        productos = ProductoDAO.ObtenerProductos();
        productosPorCodigo = productos.ToDictionary(
            p => p.Codigo, p => p, StringComparer.OrdinalIgnoreCase);

        Console.WriteLine("Datos cargados desde la base de datos.");
    }

    static void ReconstruirDiccionarios()
    {
        clientesPorId = clientes.ToDictionary(c => c.Id, c => c);
        productosPorCodigo = productos.ToDictionary(
            p => p.Codigo, p => p, StringComparer.OrdinalIgnoreCase);
    }

    static void ConectarADb()
    {
        Conexion conexionDB = new Conexion();
        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                Console.WriteLine("Conexión exitosa a la base de datos");
                string query = "SELECT NOW();";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    var resultado = cmd.ExecuteScalar();
                    Console.WriteLine("Fecha del servidor: " + resultado);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    static int LeerEntero(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? txt = Console.ReadLine();
            if (int.TryParse(txt, out int v)) return v;
            Console.WriteLine("Valor inválido, intenta de nuevo.");
        }
    }

    static decimal LeerDecimal(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? txt = Console.ReadLine();
            if (decimal.TryParse(txt, out decimal v)) return v;
            Console.WriteLine("Valor inválido, intenta de nuevo.");
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

    static string? LeerTextoOpcional(string mensaje)
    {
        Console.Write(mensaje);
        return Console.ReadLine();
    }
}