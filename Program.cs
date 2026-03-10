using System;
using System.Collections.Generic;
using System.Linq;
using SistemaVentas;
using MySqlConnector;

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

        bool continuar = true;
        while (continuar)
        {

            // Console.Clear(); // Comentado para comprobar conexion con la base de datos
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
                    MenuClientes();
                    break;
                case "2":
                    MenuProductos();
                    break;
                case "3":
                    RealizarVenta();
                    break;
                case "4":
                    MenuReportes();
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

    // MENU CLIENTES (CRUD)
    static void MenuClientes()
    {
        Console.Clear();
        Console.WriteLine("--- CLIENTES ---");
        Console.WriteLine("1. Agregar Nuevo Cliente");
        Console.WriteLine("2. Ver Todos los Clientes (OrderBy)");
        Console.WriteLine("3. Buscar Cliente (Dictionary)");
        Console.WriteLine("4. Editar Cliente");
        Console.WriteLine("5. Eliminar Cliente");
        Console.WriteLine("6. Regresar");
        Console.Write("Opcion: ");

        string? op = Console.ReadLine();

        switch (op)
        {
            case "1": RegistrarCliente(); break;
            case "2": ListarClientesOrdenados(); break;
            case "3": BuscarCliente(); break;
            case "4": EditarCliente(); break;
            case "5": EliminarCliente(); break;
        } // Enruta la acción del usuario a la operación CRUD correspondiente de clientes
    }

    static void RegistrarCliente()
    {
        Console.WriteLine("\nIngrese los datos del cliente:");
        int id = LeerEntero("ID (Numerico): ");

        if (clientesPorId.ContainsKey(id))
        {
            Console.WriteLine("Error: Ya existe un cliente con ese ID.");
            Console.ReadKey();
            return;
        }

        string nombre = LeerTexto("Nombre Completo: ");
        string correo = LeerTexto("Email: ");
        string telefono = LeerTexto("Telefono: ");

        Cliente nuevoCliente = new Cliente(id, nombre, correo, telefono);
        clientes.Add(nuevoCliente);
        clientesPorId[id] = nuevoCliente; // Valida ID único y registra el cliente en lista y diccionario para mantenerlos sincronizados

        Console.WriteLine("¡Cliente guardado! Presiona tecla.");
        Console.ReadKey();
    }

    static void ListarClientesOrdenados()
    {
        Console.WriteLine("\n--- LISTADO DE CLIENTES ---");

        var ordenados = clientes            // Se utiliza LINQ para ordenar la lista de clientes primero por el nombre (orden alfabético)
            .OrderBy(c => c.Nombre)         // y, en caso de que existan nombres iguales, se ordena por el ID.
            .ThenBy(c => c.Id)              // El método ToList() convierte el resultado en una nueva lista ya ordenada,
            .ToList();                      // sin modificar la lista original.

        if (ordenados.Count == 0)
        {
            Console.WriteLine("No hay clientes.");
        }
        else
        {
            foreach (var c in ordenados)
                c.MostrarInformacion();
        }

        Console.WriteLine("\nPresiona una tecla para volver...");
        Console.ReadKey();
    }

    static void BuscarCliente()
    {
        Console.WriteLine("\n--- BUSCAR CLIENTE ---");
        int id = LeerEntero("ID del cliente: ");

        if (clientesPorId.TryGetValue(id, out var cliente))
            cliente.MostrarInformacion();
        else
            Console.WriteLine("No encontrado."); // Busca por ID usando diccionario para evitar recorridos completos de la lista

        Console.WriteLine("\nPresiona una tecla para volver...");
        Console.ReadKey();
    }

    static void EditarCliente()
    {
        Console.WriteLine("\n--- EDITAR CLIENTE ---");
        int id = LeerEntero("ID del cliente: ");

        if (!clientesPorId.TryGetValue(id, out var cliente))
        {
            Console.WriteLine("No existe ese cliente.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"Actual: {cliente.Nombre} | {cliente.Correo} | {cliente.Telefono}");
        string? nuevoNombre = LeerTextoOpcional("Nuevo nombre (Enter para dejar igual): ");
        string? nuevoCorreo = LeerTextoOpcional("Nuevo correo (Enter para dejar igual): ");
        string? nuevoTel = LeerTextoOpcional("Nuevo telefono (Enter para dejar igual): ");

        if (!string.IsNullOrWhiteSpace(nuevoNombre)) cliente.Nombre = nuevoNombre.Trim();
        if (!string.IsNullOrWhiteSpace(nuevoCorreo)) cliente.Correo = nuevoCorreo.Trim();
        if (!string.IsNullOrWhiteSpace(nuevoTel)) cliente.Telefono = nuevoTel.Trim(); // Actualiza solo los campos ingresados, manteniendo los existentes si el usuario presiona Enter

        Console.WriteLine("Cliente actualizado. Presiona tecla.");
        Console.ReadKey();
    }

    static void EliminarCliente()
    {
        Console.WriteLine("\n--- ELIMINAR CLIENTE ---");
        int id = LeerEntero("ID del cliente: ");

        if (!clientesPorId.TryGetValue(id, out var cliente))
        {
            Console.WriteLine("No existe ese cliente.");
            Console.ReadKey();
            return;
        }

        bool tieneVentas = ventas.Any(v => v.Cliente.Id == id);
        if (tieneVentas)
        {
            Console.WriteLine("No se puede eliminar: el cliente tiene ventas registradas.");
            Console.ReadKey();
            return;
        }

        clientes.Remove(cliente);
        clientesPorId.Remove(id); // Elimina cliente de ambas estructuras, bloqueando la operación si hay ventas asociadas

        Console.WriteLine("Cliente eliminado. Presiona tecla.");
        Console.ReadKey();
    }

    // MENU PRODUCTOS (CRUD)

    static void MenuProductos()
    {
        Console.Clear();
        Console.WriteLine("--- PRODUCTOS ---");
        Console.WriteLine("1. Agregar Nuevo Producto");
        Console.WriteLine("2. Ver Inventario (OrderBy)");
        Console.WriteLine("3. Buscar Producto (Dictionary)");
        Console.WriteLine("4. Editar Producto");
        Console.WriteLine("5. Eliminar Producto");
        Console.WriteLine("6. Regresar");
        Console.Write("Opcion: ");

        string? op = Console.ReadLine();

        switch (op)
        {
            case "1": RegistrarProducto(); break;
            case "2": ListarProductosOrdenados(); break;
            case "3": BuscarProducto(); break;
            case "4": EditarProducto(); break;
            case "5": EliminarProducto(); break;
        } // Enruta la acción del usuario a la operación CRUD correspondiente de productos
    }

    static void RegistrarProducto()
    {
        Console.WriteLine("\nIngrese datos del producto:");
        string codigo = LeerTexto("Codigo (ej. P001): ");

        if (productosPorCodigo.ContainsKey(codigo))
        {
            Console.WriteLine("Error: Ya existe un producto con ese codigo.");
            Console.ReadKey();
            return;
        }

        string nombre = LeerTexto("Nombre: ");
        decimal precio = LeerDecimal("Precio: ");
        int stock = LeerEntero("Stock Inicial: ");

        Console.Write("¿Este producto tiene descuento? (s/n): ");
        string? respuesta = Console.ReadLine();

        Producto nuevo;

        if (respuesta?.Trim().Equals("s", StringComparison.OrdinalIgnoreCase) == true)
        {
            decimal desc = LeerDecimal("Ingrese el descuento (ej. 0.10 para 10%): ");
            nuevo = new ProductoPromocion(codigo, nombre, precio, stock, desc);
        }
        else
        {
            nuevo = new Producto(codigo, nombre, precio, stock);
        }

        productos.Add(nuevo);
        productosPorCodigo[codigo] = nuevo; // Valida código único, crea el producto (con o sin promoción) y lo registra en lista y diccionario

        Console.WriteLine("Producto agregado correctamente.");
        Console.ReadKey();
    }

    static void ListarProductosOrdenados()
    {
        Console.WriteLine("\n--- INVENTARIO ---");

        var ordenados = productos
            .OrderBy(p => p.Nombre)
            .ThenBy(p => p.Codigo)
            .ToList(); // Ordena el inventario para mostrarlo de forma estable y legible

        foreach (var p in ordenados)
        {
            Console.WriteLine($"COD: {p.Codigo} | {p.Nombre} | Precio: {p.Precio} | Stock: {p.Stock}");
        }

        Console.WriteLine("\nPresiona tecla para continuar...");
        Console.ReadKey();
    }

    static void BuscarProducto()
    {
        Console.WriteLine("\n--- BUSCAR PRODUCTO ---");
        string codigo = LeerTexto("Codigo: ");

        if (productosPorCodigo.TryGetValue(codigo, out var p))
            Console.WriteLine($"COD: {p.Codigo} | {p.Nombre} | Precio: {p.Precio} | Stock: {p.Stock}");
        else
            Console.WriteLine("No encontrado."); // Busca por código usando diccionario para encontrar el producto sin recorrer toda la lista

        Console.WriteLine("\nPresiona tecla para continuar...");
        Console.ReadKey();
    }

    static void EditarProducto()
    {
        Console.WriteLine("\n--- EDITAR PRODUCTO ---");
        string codigo = LeerTexto("Codigo del producto: ");

        if (!productosPorCodigo.TryGetValue(codigo, out var p))
        {
            Console.WriteLine("No existe ese producto.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"Actual: {p.Nombre} | Precio: {p.Precio} | Stock: {p.Stock}");
        string? nuevoNombre = LeerTextoOpcional("Nuevo nombre (Enter para dejar igual): ");
        string? nuevoPrecio = LeerTextoOpcional("Nuevo precio (Enter para dejar igual): ");
        string? nuevoStock = LeerTextoOpcional("Nuevo stock (Enter para dejar igual): ");

        if (!string.IsNullOrWhiteSpace(nuevoNombre)) p.Nombre = nuevoNombre.Trim();
        if (decimal.TryParse(nuevoPrecio, out var pr) && pr >= 0) p.Precio = pr;
        if (int.TryParse(nuevoStock, out var st) && st >= 0) p.Stock = st; // Aplica cambios solo si el usuario ingresa valores válidos, manteniendo el resto sin modificar

        Console.WriteLine("Producto actualizado. Presiona tecla.");
        Console.ReadKey();
    }

    static void EliminarProducto()
    {
        Console.WriteLine("\n--- ELIMINAR PRODUCTO ---");
        string codigo = LeerTexto("Codigo a eliminar: ");

        if (!productosPorCodigo.TryGetValue(codigo, out var p))
        {
            Console.WriteLine("No existe ese producto.");
            Console.ReadKey();
            return;
        }

        bool seVendió = ventas.Any(v => v.Detalles.Any(d => d.Producto.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase)));
        if (seVendió)
        {
            Console.WriteLine("No se puede eliminar: el producto ya aparece en ventas registradas.");
            Console.ReadKey();
            return;
        }

        productos.Remove(p);
        productosPorCodigo.Remove(codigo); // Elimina producto de ambas estructuras, bloqueando si ya fue utilizado en alguna venta

        Console.WriteLine("Producto eliminado. Presiona tecla.");
        Console.ReadKey();
    }

    // VENTAS

    static void RealizarVenta()
    {
        Console.Clear();
        Console.WriteLine("--- MÓDULO DE VENTAS ---");

        if (clientes.Count == 0 || productos.Count == 0)
        {
            Console.WriteLine("Debes tener al menos 1 cliente y 1 producto para vender.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nClientes:");
        foreach (var c in clientes.OrderBy(c => c.Id))
            Console.WriteLine($"- {c.Id}: {c.Nombre}"); // Muestra el catálogo de clientes ordenado por ID para facilitar la selección

        int idCliente = LeerEntero("\nEscribe el ID del Cliente que va a comprar: ");

        if (!clientesPorId.TryGetValue(idCliente, out var clienteEncontrado))
        {
            Console.WriteLine("No encontre ese cliente. Volviendo...");
            Console.ReadKey();
            return;
        }

        Venta nuevaVenta = new Venta(ventas.Count + 1, clienteEncontrado); // Crea la venta con ID incremental usando el total actual de ventas

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Venta para: {clienteEncontrado.Nombre}");

            Console.WriteLine("\n--- INVENTARIO ---");
            foreach (var p in productos.OrderBy(p => p.Codigo))
                Console.WriteLine($"COD: {p.Codigo} | {p.Nombre} | Precio: {p.Precio} | Stock: {p.Stock}"); // Muestra inventario ordenado por código para seleccionar productos

            Console.Write("\nEscribe el CODIGO del producto (o escribe 'fin' para terminar): ");
            string? codigo = Console.ReadLine();

            if (string.Equals(codigo, "fin", StringComparison.OrdinalIgnoreCase))
                break;

            if (string.IsNullOrWhiteSpace(codigo) || !productosPorCodigo.TryGetValue(codigo, out var prodSeleccionado))
            {
                Console.WriteLine("Ese codigo no existe. Intenta de nuevo.");
                Console.ReadKey();
                continue;
            }

            int cantidad = LeerEntero($"Cuantos '{prodSeleccionado.Nombre}' quieres vender? ");
            nuevaVenta.AgregarProducto(prodSeleccionado, cantidad); // Agrega el detalle a la venta y ajusta stock según la validación del método

            Console.WriteLine("Agregado. Presiona cualquier tecla.");
            Console.ReadKey();
        }

        Console.Clear();
        if (nuevaVenta.Detalles.Count == 0)
        {
            Console.WriteLine("Venta cancelada (no se agregó valor).");
            Console.ReadKey();
            return;
        }

        nuevaVenta.MostrarFactura();
        ventas.Add(nuevaVenta); // Confirma la venta agregándola al registro solo si tiene al menos un detalle

        Console.WriteLine("Venta finalizada y guardada.");
        Console.ReadKey();
    }

    // REPORTES (Semana 6 LINQ)

    static void MenuReportes()
    {
        Console.Clear();
        Console.WriteLine("--- REPORTES (LINQ) ---");
        Console.WriteLine("1. Ver Reporte de Ventas (igual al anterior)");
        Console.WriteLine("2. Top productos más vendidos");
        Console.WriteLine("3. Ventas por cliente");
        Console.WriteLine("4. Productos con bajo stock");
        Console.WriteLine("5. Regresar");
        Console.Write("Opcion: ");

        string? op = Console.ReadLine();

        switch (op)
        {
            case "1": ListarVentas(); break;
            case "2": ReporteTopProductos(); break;
            case "3": ReporteVentasPorCliente(); break;
            case "4": ReporteBajoStock(); break;
        } // Enruta a la generación del reporte seleccionado
    }

    static void ListarVentas()
    {
        Console.WriteLine("\n--- REPORTE DE VENTAS DEL DIA ---");
        if (ventas.Count == 0)
        {
            Console.WriteLine("Aun no hay ventas registradas.");
        }

        foreach (var v in ventas)
        {
            Console.WriteLine($"Venta #{v.IdVenta} | Cliente: {v.Cliente.Nombre} | Total: {v.CalcularTotal()}");
        }

        Console.WriteLine("\nPresiona tecla para volver...");
        Console.ReadKey();
    }

    static void ReporteTopProductos()
    {
        Console.Clear();
        Console.WriteLine("--- TOP PRODUCTOS MÁS VENDIDOS ---");

        if (ventas.Count == 0)
        {
            Console.WriteLine("No hay ventas.");
            Console.ReadKey();
            return;
        }

        var top = ventas
            .SelectMany(v => v.Detalles)                        // Genera el ranking de los productos más vendidos.
            .GroupBy(d => d.Producto.Codigo)                    // Primero consolida todos los detalles de todas las ventas,
            .Select(g => new                                    // luego agrupa por código de producto,
            {                                                   // calcula la cantidad total vendida y el total generado,
                Codigo = g.Key,                                 // los ordena de mayor a menor según cantidad,
                Nombre = g.First().Producto.Nombre,             // y finalmente toma los 10 productos más vendidos.
                Cantidad = g.Sum(x => x.Cantidad),
                Total = g.Sum(x => x.Subtotal)
            })
            .OrderByDescending(x => x.Cantidad)
            .Take(10)
            .ToList(); // Consolida detalles de todas las ventas y calcula ranking por cantidad vendida (máximo 10)

        foreach (var item in top)
            Console.WriteLine($"{item.Codigo} | {item.Nombre} | Cantidad: {item.Cantidad} | Total vendido: {item.Total}");

        Console.WriteLine("\nEnter...");
        Console.ReadKey();
    }

    static void ReporteVentasPorCliente()
    {
        Console.Clear();
        Console.WriteLine("--- VENTAS POR CLIENTE ---");

        if (ventas.Count == 0)
        {
            Console.WriteLine("No hay ventas.");
            Console.ReadKey();
            return;
        }

        var porCliente = ventas
            .GroupBy(v => v.Cliente.Id)
            .Select(g => new                                    // Genera un reporte de ventas por cliente:
            {                                                   // agrupa las ventas por ID de cliente, cuenta cuántas ventas hizo cada uno,
                ClienteId = g.Key,                              // suma el total gastado acumulado y ordena el resultado de mayor a menor por monto.
                Nombre = g.First().Cliente.Nombre,
                CantVentas = g.Count(),
                Total = g.Sum(v => v.CalcularTotal())
            })
            .OrderByDescending(x => x.Total)
            .ToList(); // Agrupa ventas por cliente para contar ventas y sumar totales, ordenando por el monto acumulado

        foreach (var c in porCliente)
            Console.WriteLine($"Cliente {c.ClienteId} | {c.Nombre} | Ventas: {c.CantVentas} | Total: {c.Total}");

        Console.WriteLine("\nEnter...");
        Console.ReadKey();
    }

    static void ReporteBajoStock()
    {
        Console.Clear();
        Console.WriteLine("--- PRODUCTOS CON BAJO STOCK ---");

        int umbral = LeerEntero("Mostrar productos con stock <= : ");

        var bajos = productos
            .Where(p => p.Stock <= umbral)
            .OrderBy(p => p.Stock)
            .ThenBy(p => p.Nombre)
            .ToList(); // Filtra productos por umbral y los ordena para priorizar los más críticos

        if (bajos.Count == 0)
        {
            Console.WriteLine("No hay productos bajo ese umbral.");
        }
        else
        {
            foreach (var p in bajos)
                Console.WriteLine($"{p.Codigo} | {p.Nombre} | Stock: {p.Stock} | Precio venta: {p.CalcularPrecioVenta()}");
        }

        Console.WriteLine("\nEnter...");
        Console.ReadKey();
    }

    // DATOS DE PRUEBA, ** quitar hasta que se tenga la BD funcional!**

    static void CargarDatosDePrueba()
    {
        if (clientes.Count == 0)
        {
            clientes.Add(new Cliente(1, "Juan Perez", "juan@mail.com", "8888-8888"));
            clientes.Add(new Cliente(2, "Maria Lopez", "maria@test.com", "9999-0000"));
        }

        if (productos.Count == 0)
        {
            productos.Add(new Producto("P01", "Coca Cola", 1500m, 20));
            productos.Add(new ProductoPromocion("P02", "Papas Fritas", 1000m, 10, 0.10m));
            productos.Add(new Producto("P03", "Agua", 900m, 30));
        } // Inserta datos iniciales solo si las colecciones están vacías para no duplicar registros
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