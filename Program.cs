// Program.cs
using System;
using SistemaVentas;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== SISTEMA DE VENTAS - SEMANA 4 ===");

        // 1. Creación de Objetos (Semana 2-3)
        Cliente cliente1 = new Cliente(1, "Juan Perez", "juan@mail.com", "555-1234");

        // Producto normal
        Producto laptop = new Producto("P001", "Laptop Dell", 1000m, 10);

        // Producto con polimorfismo (tiene 10% de descuento)
        Producto mouse = new ProductoPromocion("P002", "Mouse Gamer", 50m, 20, 0.10m);

        // 2. Simulación de Venta (Semana 4)
        Venta ventaActual = new Venta(1001, cliente1);

        // Agregamos productos
        ventaActual.AgregarProducto(laptop, 1); // Precio: 1000
        ventaActual.AgregarProducto(mouse, 2);  // Precio: 45 x 2 = 90 (aplica descuento)

        // 3. Imprimir Factura
        ventaActual.MostrarFactura();

        // Verificamos Stock restante
        Console.WriteLine($"Stock restante Laptop: {laptop.Stock}");
        Console.WriteLine($"Stock restante Mouse: {mouse.Stock}");

        Console.ReadLine();
    }
}