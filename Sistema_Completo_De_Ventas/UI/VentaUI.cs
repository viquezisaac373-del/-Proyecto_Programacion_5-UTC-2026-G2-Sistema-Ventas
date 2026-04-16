using Sistema_Completo_De_Ventas;
using SistemaVentas.BLL;
using SistemaVentas.DAL;
using SistemaVentas.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

// Clase estática que maneja la interfaz de usuario por consola para el módulo de ventas
public static class VentaUI
{
    // Método principal del módulo de ventas.
    // Recibe las colecciones de datos en memoria para evitar consultas repetidas a la BD.
    // Los diccionarios permiten búsquedas rápidas por ID o código sin recorrer listas completas.
    public static void RealizarVenta(
        List<ClienteDTO> clientes,                  // Lista de clientes para mostrar en pantalla
        Dictionary<int, ClienteDTO> clientesPorId,  // Diccionario para buscar cliente por ID
        List<Producto> productos,                   // Lista de productos disponibles
        Dictionary<string, Producto> productosPorCodigo, // Diccionario para buscar producto por código
        List<Venta> ventas)                         // Lista de ventas registradas en la sesión
    {
        Console.Clear();
        Console.WriteLine("--- MÓDULO DE VENTAS ---");

        // Validación previa: no se puede realizar una venta si no hay clientes o productos cargados
        if (clientes.Count == 0 || productos.Count == 0)
        {
            Console.WriteLine("Debe haber clientes y productos.");
            Console.ReadKey();
            return;
        }

        // Se muestran los clientes ordenados por ID para facilitar la selección
        foreach (var c in clientes.OrderBy(c => c.Id))
            Console.WriteLine($"{c.Id} - {c.Nombre}");

        Console.Write("\nID ClienteDTO: ");
        int idCliente = int.Parse(Console.ReadLine()!);

        // Se busca el cliente en el diccionario; si no existe, se cancela la operación
        if (!clientesPorId.TryGetValue(idCliente, out var cliente))
        {
            Console.WriteLine("ClienteDTO no encontrado.");
            Console.ReadKey();
            return;
        }

        // Se crea una nueva venta asociada al cliente seleccionado.
        // El ID se genera en base a la cantidad de ventas actuales en memoria.
        Venta venta = new Venta(ventas.Count + 1, cliente);

        // Bucle principal para agregar productos a la venta.
        // El usuario puede escribir 'fin' para terminar de agregar productos.
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"ClienteDTO: {cliente.Nombre}");

            // Se muestran los productos disponibles ordenados por código
            foreach (var p in productos.OrderBy(p => p.Codigo))
                Console.WriteLine($"{p.Codigo} | {p.Nombre} | Stock: {p.Stock}");

            Console.Write("\nCodigo (o 'fin'): ");
            string? codigo = Console.ReadLine();

            // Si el usuario escribe 'fin', se termina de agregar productos
            if (codigo == "fin") break;

            // Se valida que el código ingresado exista en el diccionario de productos
            if (codigo == null || !productosPorCodigo.TryGetValue(codigo, out var prod))
            {
                Console.WriteLine("No existe.");
                Console.ReadKey();
                continue;
            }

            Console.Write("Cantidad: ");
            int cantidad = int.Parse(Console.ReadLine()!);

            // Se agrega el producto a la venta; internamente este método también descuenta el stock
            venta.AgregarProducto(prod, cantidad);

            // Se persiste el nuevo stock del producto en la base de datos inmediatamente
            ProductoDAO.ActualizarStock(prod.Codigo, prod.Stock);

            Console.WriteLine("Agregado.");
            Console.ReadKey();
        }

        // Si no se agregó ningún producto, se cancela la venta sin guardar nada
        if (venta.Detalles.Count == 0)
        {
            Console.WriteLine("Venta cancelada.");
            Console.ReadKey();
            return;
        }

        Console.Clear();

        // Se muestra el resumen de la factura antes de guardar
        venta.MostrarFactura();

        // Se agrega la venta a la lista en memoria de la sesión actual
        ventas.Add(venta);

        // --- PERSISTENCIA EN BASE DE DATOS ---

        VentaService service = new VentaService();

        // Se construye el DTO principal de la venta con los datos mínimos necesarios para la BD
        var ventaDTO = new VentaDTO
        {
            ClienteId = venta.Cliente.Id,
            Fecha = venta.Fecha
        };

        // Se convierten los detalles de la venta a DTOs para ser enviados a la capa de servicio
        var detallesDTO = venta.Detalles.Select(d => new VentaDetalleDTO
        {
            CodigoP = d.Producto.Codigo,
            Cantidad = d.Cantidad,
            PrecioUnitario = d.PrecioUnitario
        }).ToList();

        try
        {
            // Se registra la venta completa en BD y se obtiene el ID real generado por la base de datos
            int idGenerado = service.RegistrarVenta(ventaDTO, detallesDTO);

            // Se actualiza el objeto en memoria con el ID real asignado por la BD,
            // para mantener consistencia entre la sesión y la base de datos
            venta.IdVenta = idGenerado;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        Console.WriteLine("Venta guardada.");
        Console.ReadKey();
    }
}