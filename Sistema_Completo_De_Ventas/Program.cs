using MySqlConnector;
using Sistema_Completo_De_Ventas;
using SistemaVentas.BLL;
using SistemaVentas.DTO;
using SistemaVentas.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

class Program
{
    // Ambos usan el mismo tipo: SistemaVentas.DTO.ClienteDTO
    static List<ClienteDTO> clientes = new List<ClienteDTO>();
    static List<Producto> productos = new List<Producto>();
    static List<Venta> ventas = new List<Venta>();

    static Dictionary<int, ClienteDTO> clientesPorId = new Dictionary<int, ClienteDTO>();
    static Dictionary<string, Producto> productosPorCodigo =
        new Dictionary<string, Producto>(StringComparer.OrdinalIgnoreCase);

    [STAThread]
    static void Main(string[] args)
    {
        CargarDatosDePrueba();
        ReconstruirDiccionarios();

        ConectarADb();
        // Console.WriteLine("Presione una tecla para iniciar");
        // Console.ReadLine();

        ApplicationConfiguration.Initialize();
        Application.Run(new Sistema_Completo_De_Ventas.UI.Forms.FrmPrincipal());
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