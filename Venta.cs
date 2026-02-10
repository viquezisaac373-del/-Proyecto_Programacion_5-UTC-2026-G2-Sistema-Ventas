using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaVentas
{
    public class VentaDetalle
    {
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => Producto.CalcularPrecioVenta() * Cantidad;
    }

    public class Venta
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public Cliente Cliente { get; set; }
        public List<VentaDetalle> Detalles { get; set; }

        public Venta(int id, Cliente cliente)
        {
            IdVenta = id;
            Fecha = DateTime.Now;
            Cliente = cliente;
            Detalles = new List<VentaDetalle>();
        }

        public void AgregarProducto(Producto producto, int cantidad)
        {
            if (producto.Stock >= cantidad)
            {
                producto.ReducirStock(cantidad);
                Detalles.Add(new VentaDetalle { Producto = producto, Cantidad = cantidad });
                Console.WriteLine($"Agregado: {producto.Nombre} x {cantidad}");
            }
            else
            {
                Console.WriteLine($"Error: No hay suficiente stock de {producto.Nombre}");
            }
        }

        public decimal CalcularTotal()
        {
            return Detalles.Sum(d => d.Subtotal);
        }

        public void MostrarFactura()
        {
            Console.WriteLine("\n---------------- FACTURA ----------------");
            Console.WriteLine($"Fecha: {Fecha}");
            Cliente.MostrarInformacion();
            Console.WriteLine("-----------------------------------------");

            foreach (var item in Detalles)
            {
                Console.WriteLine($"- {item.Producto.Nombre} (x{item.Cantidad}): ${item.Subtotal}");
            }

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine($"TOTAL A PAGAR: ${CalcularTotal()}");
            Console.WriteLine("-----------------------------------------\n");
        }
    }
}
