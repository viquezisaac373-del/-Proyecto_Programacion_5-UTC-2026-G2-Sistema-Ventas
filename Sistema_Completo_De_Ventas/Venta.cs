using System;
using System.Collections.Generic;
using System.Linq;
using SistemaVentas.DTO; 

namespace Sistema_Completo_De_Ventas
{
    public class VentaDetalle
    {
        public Producto Producto { get; set; } = null!; 
        public int Cantidad { get; set; }
        public decimal PrecioUnitario => Producto.CalcularPrecioVenta();
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }

    public class Venta
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public ClienteDTO Cliente { get; set; } // ✅ Ahora usa ClienteDTO
        public List<VentaDetalle> Detalles { get; set; }

        public Venta(int id, ClienteDTO cliente)
        {
            IdVenta = id;
            Fecha = DateTime.Now;
            Cliente = cliente;
            Detalles = new List<VentaDetalle>();
        }

        public void AgregarProducto(Producto producto, int cantidad) 
        {
            if (cantidad <= 0)
            {
                Console.WriteLine("Cantidad inválida.");
                return;
            }

            if (producto.Stock >= cantidad)
            {
                producto.ReducirStock(cantidad);

                var existente = Detalles.FirstOrDefault(d => d.Producto.Codigo == producto.Codigo);
                if (existente != null)
                    existente.Cantidad += cantidad;
                else
                    Detalles.Add(new VentaDetalle { Producto = producto, Cantidad = cantidad });

                Console.WriteLine($"Agregado: {producto.Nombre} x {cantidad}");
            }
            else
            {
                Console.WriteLine($"Error: No hay suficiente stock de {producto.Nombre}");
            }
        }

        public decimal CalcularTotal() => Detalles.Sum(d => d.Subtotal);

        public void MostrarFactura()
        {
            Console.WriteLine("\n---------------- FACTURA ----------------");
            Console.WriteLine($"Venta #: {IdVenta} | Fecha: {Fecha}");
            Console.WriteLine($"Cliente: {Cliente.Id} | {Cliente.Nombre} | {Cliente.Correo} | Tel: {Cliente.Telefono}");
            Console.WriteLine("-----------------------------------------");

            foreach (var item in Detalles)
                Console.WriteLine(
                    $"- {item.Producto.Codigo} | {item.Producto.Nombre} (x{item.Cantidad}) " +
                    $"@ {item.PrecioUnitario:C} = {item.Subtotal:C}"
                );

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine($"TOTAL A PAGAR: {CalcularTotal():C}");
            Console.WriteLine("-----------------------------------------\n");
        }
    }
}