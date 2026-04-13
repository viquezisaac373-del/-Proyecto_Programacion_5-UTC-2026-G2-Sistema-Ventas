using Sistema_Completo_De_Ventas;
using SistemaVentas.BLL;
using SistemaVentas.DAL;
using SistemaVentas.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

public static class VentaUI
{
    public static void RealizarVenta(
        List<ClienteDTO> clientes,           
        Dictionary<int, ClienteDTO> clientesPorId,  
        List<Producto> productos,
        Dictionary<string, Producto> productosPorCodigo,
        List<Venta> ventas)
    {
        Console.Clear();
        Console.WriteLine("--- MÓDULO DE VENTAS ---");

        if (clientes.Count == 0 || productos.Count == 0)
        {
            Console.WriteLine("Debe haber clientes y productos.");
            Console.ReadKey();
            return;
        }

        // Mostrar clientes
        foreach (var c in clientes.OrderBy(c => c.Id))
            Console.WriteLine($"{c.Id} - {c.Nombre}");

        Console.Write("\nID ClienteDTO: ");
        int idCliente = int.Parse(Console.ReadLine()!);

        if (!clientesPorId.TryGetValue(idCliente, out var cliente))
        {
            Console.WriteLine("ClienteDTO no encontrado.");
            Console.ReadKey();
            return;
        }

        Venta venta = new Venta(ventas.Count + 1, cliente);

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"ClienteDTO: {cliente.Nombre}");

            foreach (var p in productos.OrderBy(p => p.Codigo))
                Console.WriteLine($"{p.Codigo} | {p.Nombre} | Stock: {p.Stock}");

            Console.Write("\nCodigo (o 'fin'): ");
            string? codigo = Console.ReadLine();

            if (codigo == "fin") break;

            if (codigo == null || !productosPorCodigo.TryGetValue(codigo, out var prod))
            {
                Console.WriteLine("No existe.");
                Console.ReadKey();
                continue;
            }

            Console.Write("Cantidad: ");
            int cantidad = int.Parse(Console.ReadLine()!);

            venta.AgregarProducto(prod, cantidad);

            // ACTUALIZAR STOCK EN BD
            ProductoDAO.ActualizarStock(prod.Codigo, prod.Stock);

            Console.WriteLine("Agregado.");
            Console.ReadKey();
        }

        if (venta.Detalles.Count == 0)
        {
            Console.WriteLine("Venta cancelada.");
            Console.ReadKey();
            return;
        }

        Console.Clear();
        venta.MostrarFactura();

        ventas.Add(venta);

        // GUARDAR EN BD
        VentaService service = new VentaService();

        var ventaDTO = new VentaDTO
        {
            ClienteId = venta.Cliente.Id,
            Fecha = venta.Fecha
        };

        var detallesDTO = venta.Detalles.Select(d => new VentaDetalleDTO
        {
            CodigoP = d.Producto.Codigo,
            Cantidad = d.Cantidad,
            PrecioUnitario = d.PrecioUnitario
        }).ToList();

        try
        {
            int idGenerado = service.RegistrarVenta(ventaDTO, detallesDTO);
            // Actualizar el ID real en memoria
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