using MySqlConnector;
using Sistema_Completo_De_Ventas;
using SistemaVentas.BLL;
using SistemaVentas.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using SistemaVentas.DAL;



class Program
{
    static List<Cliente> clientes = new List<Cliente>();
    static List<Producto> productos = new List<Producto>();
    static List<Venta> ventas = new List<Venta>(); // Son listas comunes que almacena en memoria clientes, productos y ventas durante la ejecución

    static Dictionary<int, Cliente> clientesPorId = new Dictionary<int, Cliente>();
    static Dictionary<string, Producto> productosPorCodigo =
    new Dictionary<string, Producto>(StringComparer.OrdinalIgnoreCase); // Índices en memoria para buscar rápido por ID de cliente y por código de producto sin importar mayúsculas/minúsculas

    static void Main(string[] args)
    {
        CargarDatosDePrueba();
        ReconstruirDiccionarios(); // Carga datos iniciales y sincroniza los diccionarios con las listas

        // Se realiza una prueba de conexión a la base de datos al iniciar el sistema.
        // Esto permite verificar que la configuración de la base de datos es correcta
        // antes de que el usuario interactúe con el sistema.
        ConectarADb(); // Verifica conexión a BD en cada iteración del menú principal
        Console.WriteLine("Presione una tecla para iniciar"); 
        Console.ReadLine();

        bool continuar = true;
        while (continuar)
        {
           
            Console.Clear(); // Comentado para comprobar conexion con la base de datos
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
                    ProductoUI.Menu(productos, productosPorCodigo, ventas);
                    break;
                case "3":
                    VentaUI.RealizarVenta(clientes, clientesPorId, productos, productosPorCodigo, ventas);
                    break;
                case "4":
                    ReporteUI.Menu();
                    break;
                case "5":
                    continuar = false;
                    Console.WriteLine("Saliendo del sistema...");
                    break;
                default:
                    Console.WriteLine("Esa opcion no existe. Presiona Enter.");
                    Console.ReadKey();
                    break;
            } // Controla el flujo del sistema según la opción elegida en el menú principal
        }
    }

   


    // Carga clientes y productos desde la base de datos al iniciar el sistema
    // Esto permite llenar las listas en memoria con datos persistentes de MySQL
    static void CargarDatosDePrueba()
    {
        // Cargar clientes desde la base de datos
        clientes = ClienteDAO.ObtenerClientes();
        clientesPorId = clientes.ToDictionary(c => c.Id, c => c);

        // Cargar productos desde la base de datos
        productos = ProductoDAO.ObtenerProductos();
        // Se reconstruye el diccionario de productos para facilitar búsquedas rápidas por código
        productosPorCodigo = productos.ToDictionary(p => p.Codigo, p => p, StringComparer.OrdinalIgnoreCase);

        Console.WriteLine("Datos cargados desde la base de datos.");
    }

    static void ReconstruirDiccionarios()
    {
        clientesPorId = clientes.ToDictionary(c => c.Id, c => c);
        productosPorCodigo = productos.ToDictionary(p => p.Codigo, p => p, StringComparer.OrdinalIgnoreCase); // Reconstruye índices de búsqueda a partir de las listas actuales
    }

    // Método estático que se encarga de conectar a la base de datos
    static void ConectarADb()
    {
        // Se crea una instancia de la clase Conexion que contiene
        // la cadena de conexión y el método para abrir la conexión con MySQL
        Conexion conexionDB = new Conexion(); // Creamos una instancia de la clase para la conexión
        try
        {
            // using abre la conexión y la cierra automáticamente cuando termina el bloque
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                Console.WriteLine("Conexión exitosa a la base de datos");
                string query = "SELECT NOW();";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    var resultado = cmd.ExecuteScalar();
                    Console.WriteLine("Fecha del servidor: " + resultado);
                }
            } // Abre conexión, ejecuta una consulta simple y cierra recursos automáticamente con using
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message); // Reporta la causa del fallo de conexión o ejecución de consulta
        }
    }

    // INPUTS SEGUROS (para no caerse)
    static int LeerEntero(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? txt = Console.ReadLine();
            if (int.TryParse(txt, out int v)) return v;
            Console.WriteLine("Valor inválido, intenta de nuevo.");
        } // Repite hasta que el usuario ingrese un entero válido
    }

    static decimal LeerDecimal(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? txt = Console.ReadLine();
            if (decimal.TryParse(txt, out decimal v)) return v;
            Console.WriteLine("Valor inválido, intenta de nuevo.");
        } // Repite hasta que el usuario ingrese un decimal válido
    }

    static string LeerTexto(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? txt = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(txt)) return txt.Trim();
            Console.WriteLine("No puede estar vacío.");
        } // Repite hasta que el usuario ingrese texto no vacío
    }

    static string? LeerTextoOpcional(string mensaje)
    {
        Console.Write(mensaje);
        return Console.ReadLine(); // Permite devolver vacío/null para mantener valores anteriores en ediciones
    }
}