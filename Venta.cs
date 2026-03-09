using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaVentas
{
    public class VentaDetalle
    {
        public Producto Producto { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario => Producto.CalcularPrecioVenta();
        public decimal Subtotal => PrecioUnitario * Cantidad; // Define el producto vendido, la cantidad y calcula automáticamente el precio unitario y el subtotal de la línea
    }

    public class Venta
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public Cliente Cliente { get; set; }
        public List<VentaDetalle> Detalles { get; set; } // Propiedades que identifican la venta, registran su fecha, el cliente asociado y almacenan sus líneas de detalle

        public Venta(int id, Cliente cliente)
        {
            IdVenta = id;
            Fecha = DateTime.Now;
            Cliente = cliente;
            Detalles = new List<VentaDetalle>(); // Inicializa la venta asignando id, fecha actual, cliente y creando la lista de detalles
        }

        public void AgregarProducto(Producto producto, int cantidad)
        {
            if (cantidad <= 0)
            {
                Console.WriteLine("Cantidad inválida."); // Detiene el proceso si la cantidad no es válida
                return;
            }

            if (producto.Stock >= cantidad)
            {
                producto.ReducirStock(cantidad); // Reduce inventario al momento de agregar el producto a la venta

                // Si el producto ya estaba en la venta, se acumula la cantidad en la misma línea para no duplicar filas
                var existente = Detalles.FirstOrDefault(d => d.Producto.Codigo == producto.Codigo);
                if (existente != null)
                {
                    existente.Cantidad += cantidad; // Suma la cantidad adicional al detalle existente
                }
                else
                {
                    Detalles.Add(new VentaDetalle { Producto = producto, Cantidad = cantidad }); // Crea un nuevo detalle si no existía
                }

                Console.WriteLine($"Agregado: {producto.Nombre} x {cantidad}"); // Confirma lo agregado
            }
            else
            {
                Console.WriteLine($"Error: No hay suficiente stock de {producto.Nombre}"); // Informa si el inventario no alcanza
            }
        }

        public decimal CalcularTotal()
        {
            return Detalles.Sum(d => d.Subtotal); // Suma los subtotales de cada detalle para obtener el total de la venta
        }

        public void MostrarFactura()
        {
            Console.WriteLine("\n---------------- FACTURA ----------------"); 
            Console.WriteLine($"Venta #: {IdVenta} | Fecha: {Fecha}"); 
            Cliente.MostrarInformacion(); 
            Console.WriteLine("-----------------------------------------"); 

            foreach (var item in Detalles)
            {
                Console.WriteLine(
                    $"- {item.Producto.Codigo} | {item.Producto.Nombre} (x{item.Cantidad}) " +
                    $"@ {item.PrecioUnitario:C} = {item.Subtotal:C}" // Formato monetario con :C para precio unitario y subtotal
                );
            }

            Console.WriteLine("-----------------------------------------"); 
            Console.WriteLine($"TOTAL A PAGAR: {CalcularTotal():C}"); 
            Console.WriteLine("-----------------------------------------\n"); 
        }
    }
}