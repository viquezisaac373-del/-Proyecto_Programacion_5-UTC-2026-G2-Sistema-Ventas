using System;
using System.Collections.Generic;
using SistemaVentas;

class Program
{
    // Usamos listas estáticas para guardar los datos mientras corre el programa
    // porque todavía no usamos Base de Datos.
    static List<Cliente> clientes = new List<Cliente>();
    static List<Producto> productos = new List<Producto>();
    static List<Venta> ventas = new List<Venta>();

    static void Main(string[] args)
    {
        // Cargo algunos datos de prueba para no tener que escribir todo cada vez que pruebo (opcional, se puede comentar)
        CargarDatosDePrueba();

        bool continuar = true;
        while (continuar)
        {
            Console.Clear();
            Console.WriteLine("=== SISTEMA DE VENTAS ===");
            Console.WriteLine("1. Menu Clientes");
            Console.WriteLine("2. Menu Productos");
            Console.WriteLine("3. Vender (Facturacion)");
            Console.WriteLine("4. Ver Reporte de Ventas");
            Console.WriteLine("5. Salir");
            Console.Write("Elige una opcion: ");

            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    MenuClientes();
                    break;
                case "2":
                    MenuProductos();
                    break;
                case "3":
                    RealizarVenta();
                    break;
                case "4":
                    ListarVentas();
                    break;
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

    static void MenuClientes()
    {
        Console.Clear();
        Console.WriteLine("--- CLIENTES ---");
        Console.WriteLine("1. Agregar Nuevo Cliente");
        Console.WriteLine("2. Ver Todos los Clientes");
        Console.WriteLine("3. Regresar");
        Console.Write("Opcion: ");
        
        string op = Console.ReadLine();
        if (op == "1") 
        {
            RegistrarCliente();
        }
        else if (op == "2")
        {
            ListarClientes();
            Console.WriteLine("\nPresiona una tecla para volver...");
            Console.ReadKey();
        }
    }

    static void MenuProductos()
    {
        Console.Clear();
        Console.WriteLine("--- PRODUCTOS ---");
        Console.WriteLine("1. Agregar Nuevo Producto");
        Console.WriteLine("2. Ver Inventario");
        Console.WriteLine("3. Regresar");
        Console.Write("Opcion: ");

        string op = Console.ReadLine();
        
        switch (op)
        {
            case "1": RegistrarProducto(); break;
            case "2": ListarProductos(); Console.ReadKey(); break;
        }
    }

    // --- FUNCIONES PARA AGREGAR DATOS ---

    static void RegistrarCliente()
    {
        Console.WriteLine("\nIngrese los datos del cliente:");
        Console.Write("ID (Numerico): ");
        // Uso int.Parse asumiendo que el usuario pone numeros validos
        int id = int.Parse(Console.ReadLine()); 
        
        Console.Write("Nombre Completo: ");
        string nombre = Console.ReadLine();
        
        Console.Write("Email: ");
        string correo = Console.ReadLine();
        
        Console.Write("Telefono: ");
        string telefono = Console.ReadLine();

        // Creo el objeto Cliente y lo guardo en la lista
        Cliente nuevoCliente = new Cliente(id, nombre, correo, telefono);
        clientes.Add(nuevoCliente);
        
        Console.WriteLine("¡Cliente guardado! Presiona tecla.");
        Console.ReadKey();
    }

    static void ListarClientes()
    {
        Console.WriteLine("\n--- LISTADO DE CLIENTES ---");
        // Recorro la lista con un foreach para mostrar cada uno
        foreach (var c in clientes)
        {
            c.MostrarInformacion(); // Esto usa el metodo override de Cliente
        }
    }

    static void RegistrarProducto()
    {
        Console.WriteLine("\nIngrese datos del producto:");
        Console.Write("Codigo (ej. P001): ");
        string codigo = Console.ReadLine();
        
        Console.Write("Nombre: ");
        string nombre = Console.ReadLine();
        
        Console.Write("Precio: ");
        decimal precio = decimal.Parse(Console.ReadLine());
        
        Console.Write("Stock Inicial: ");
        int stock = int.Parse(Console.ReadLine());

        // Aca pregunto si es promocion para usar Polimorfismo
        Console.Write("¿Este producto tiene descuento? (s/n): ");
        string respuesta = Console.ReadLine();

        if (respuesta == "s" || respuesta == "S")
        {
            Console.Write("Ingrese el descuento (ej. 0.10 para 10%): ");
            decimal desc = decimal.Parse(Console.ReadLine());
            
            // Instancio ProductoPromocion (Clase Hija)
            ProductoPromocion pPromocion = new ProductoPromocion(codigo, nombre, precio, stock, desc);
            productos.Add(pPromocion);
        }
        else
        {
            // Instancio Producto normal (Clase Padre)
            Producto pNormal = new Producto(codigo, nombre, precio, stock);
            productos.Add(pNormal);
        }
        
        Console.WriteLine("Producto agregado correctamente.");
        Console.ReadKey();
    }

    static void ListarProductos()
    {
        Console.WriteLine("\n--- INVENTARIO ---");
        foreach (var p in productos)
        {
            // Muestro info basica. TODO: Ver como mostrar si es promocion o no en el listado
            Console.WriteLine($"COD: {p.Codigo} | {p.Nombre} | Precio: {p.Precio} | Stock: {p.Stock}");
        }
        Console.WriteLine("\nPresiona tecla para continuar...");
    }

    static void RealizarVenta()
    {
        Console.Clear();
        Console.WriteLine("--- MÓDULO DE VENTAS ---");
        
        // 1. Mostrar clientes para saber el ID
        ListarClientes();
        Console.Write("\nEscribe el ID del Cliente que va a comprar: ");
        int idCliente = int.Parse(Console.ReadLine());

        // Busco el cliente en la lista
        Cliente clienteEncontrado = null;
        foreach(var c in clientes) {
            if(c.Id == idCliente) {
                clienteEncontrado = c;
                break;
            }
        }

        if (clienteEncontrado == null)
        {
            Console.WriteLine("No encontre ese cliente. Volviendo...");
            Console.ReadKey();
            return;
        }

        // Creo la venta con ID automatico (cantidad de ventas + 1)
        Venta nuevaVenta = new Venta(ventas.Count + 1, clienteEncontrado);

        // Ciclo para agregar productos a la venta
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Venta para: {clienteEncontrado.Nombre}");
            ListarProductos(); // Muestro que hay disponible
            
            Console.Write("\nEscribe el CODIGO del producto (o escribe 'fin' para terminar): ");
            string codigo = Console.ReadLine();
            
            if (codigo == "fin") break;

            // Busco el producto
            Producto prodSeleccionado = null;
            foreach(var p in productos) {
                if(p.Codigo == codigo) {
                    prodSeleccionado = p;
                    break; // Ya lo encontre, salgo del foreach
                }
            }

            if (prodSeleccionado != null)
            {
                Console.Write($"Cuantos '{prodSeleccionado.Nombre}' quieres vender? ");
                int cantidad = int.Parse(Console.ReadLine());
                
                // Agrego a la venta (la logica de stock esta en la clase Venta)
                nuevaVenta.AgregarProducto(prodSeleccionado, cantidad);
                Console.WriteLine("Agregado. Presiona tecla.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Ese codigo no existe. Intenta de nuevo.");
                Console.ReadKey();
            }
        }

        // Al terminar, mostramos la factura y guardamos
        Console.Clear();
        nuevaVenta.MostrarFactura();
        ventas.Add(nuevaVenta); // Guardo en el historial
        
        Console.WriteLine("Venta finalizada y guardada.");
        Console.ReadKey();
    }

    static void ListarVentas()
    {
        Console.WriteLine("\n--- REPORTE DE VENTAS DEL DIA ---");
        if (ventas.Count == 0) {
            Console.WriteLine("Aun no hay ventas registradas.");
        }

        foreach (var v in ventas)
        {
            Console.WriteLine($"Venta #{v.IdVenta} | Cliente: {v.Cliente.Nombre} | Total: ${v.CalcularTotal()}");
        }
        Console.WriteLine("\nPresiona tecla para volver...");
        Console.ReadKey();
    }

    static void CargarDatosDePrueba()
    {
        // Datos 'quemados' para probar rapido
        clientes.Add(new Cliente(1, "Juan Perez", "juan@mail.com", "8888-8888"));
        clientes.Add(new Cliente(2, "Maria Lopez", "maria@test.com", "9999-0000"));
        
        productos.Add(new Producto("P01", "Coca Cola", 1500, 20));
        // Este tiene descuento del 10%
        productos.Add(new ProductoPromocion("P02", "Papas Fritas", 1000, 10, 0.10m)); 
    }
}