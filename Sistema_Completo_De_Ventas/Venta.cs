using System;
using System.Collections.Generic;
using System.Linq;
using SistemaVentas.DTO;

namespace Sistema_Completo_De_Ventas
{
    // Clase que representa el detalle de una venta (producto + cantidad)
    public class VentaDetalle
    {
        // Producto asociado al detalle
        public Producto Producto { get; set; } = null!;

        // Cantidad del producto en la venta
        public int Cantidad { get; set; }

        // Precio unitario del producto (considera descuentos si aplica)
        public decimal PrecioUnitario => Producto.CalcularPrecioVenta();

        // Subtotal del producto (precio * cantidad)
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }

    // Clase que representa una venta completa
    public class Venta
    {
        // Identificador de la venta
        public int IdVenta { get; set; }

        // Fecha en que se realiza la venta
        public DateTime Fecha { get; set; }

        // Cliente asociado a la venta (DTO)
        public ClienteDTO Cliente { get; set; }

        // Lista de productos vendidos
        public List<VentaDetalle> Detalles { get; set; }

        // Constructor de la venta
        public Venta(int id, ClienteDTO cliente)
        {
            IdVenta = id;
            Fecha = DateTime.Now; // Asigna la fecha actual
            Cliente = cliente;
            Detalles = new List<VentaDetalle>();
        }

        // Método para agregar un producto a la venta
        public void AgregarProducto(Producto producto, int cantidad)
        {
            // Valida que la cantidad sea válida
            if (cantidad <= 0)
            {
                Console.WriteLine("Cantidad inválida.");
                return;
            }

            // Verifica si hay suficiente stock
            if (producto.Stock >= cantidad)
            {
                // Reduce el stock del producto
                producto.ReducirStock(cantidad);

                // Busca si el producto ya existe en la venta
                var existente = Detalles.FirstOrDefault(d => d.Producto.Codigo == producto.Codigo);

                if (existente != null)
                    existente.Cantidad += cantidad; // Aumenta la cantidad
                else
                    // Agrega un nuevo detalle
                    Detalles.Add(new VentaDetalle { Producto = producto, Cantidad = cantidad });

                Console.WriteLine($"Agregado: {producto.Nombre} x {cantidad}");
            }
            else
            {
                // Mensaje si no hay suficiente stock
                Console.WriteLine($"Error: No hay suficiente stock de {producto.Nombre}");
            }
        }

        // Calcula el total de la venta sumando los subtotales
        public decimal CalcularTotal() => Detalles.Sum(d => d.Subtotal);

        // Muestra la factura en consola
        public void MostrarFactura()
        {
            Console.WriteLine("\n---------------- FACTURA ----------------");
            Console.WriteLine($"Venta #: {IdVenta} | Fecha: {Fecha}");

            // Muestra datos del cliente
            Console.WriteLine($"Cliente: {Cliente.Id} | {Cliente.Nombre} | {Cliente.Correo} | Tel: {Cliente.Telefono}");
            Console.WriteLine("-----------------------------------------");

            // Recorre los detalles y muestra cada producto
            foreach (var item in Detalles)
                Console.WriteLine(
                    $"- {item.Producto.Codigo} | {item.Producto.Nombre} (x{item.Cantidad}) " +
                    $"@ {item.PrecioUnitario:C} = {item.Subtotal:C}"
                );

            Console.WriteLine("-----------------------------------------");

            // Muestra el total final
            Console.WriteLine($"TOTAL A PAGAR: {CalcularTotal():C}");
            Console.WriteLine("-----------------------------------------\n");
        }
    }
}