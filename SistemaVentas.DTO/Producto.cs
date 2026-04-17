using System;

namespace Sistema_Completo_De_Ventas
{
    // Clase base que representa un producto general del sistema
    public class Producto
    {
        // Código único que identifica el producto
        public int Codigo { get; set; }

        // Nombre del producto
        public string Nombre { get; set; }

        // Descripción detallada del producto
        public string Descripcion { get; set; } 

        // Precio base del producto sin descuentos
        public decimal Precio { get; set; }

        // Cantidad disponible en inventario
        public int Stock { get; set; }

        // Propiedad virtual que permite aplicar descuentos en clases derivadas
        public virtual decimal Descuento { get; set; } = 0;

        // Constructor que inicializa los atributos del producto
        public Producto(int codigo, string nombre, decimal precio, int stock, string descripcion)
        {
            Codigo = codigo;
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
            Descripcion = descripcion;
        }

        // Método virtual que calcula el precio de venta (sin descuento por defecto)
        public virtual decimal CalcularPrecioVenta()
        {
            return Precio;
        }

        // Método para reducir el stock al realizar una venta
        public void ReducirStock(int cantidad)
        {
            // Valida que la cantidad sea mayor a cero
            if (cantidad <= 0) return;

            // Verifica si hay suficiente stock antes de reducir
            if (Stock >= cantidad)
                Stock -= cantidad;
            else
                Console.WriteLine("Advertencia: Stock insuficiente.");
        }

        // Método para aumentar el stock (por ejemplo, al reabastecer)
        public void AumentarStock(int cantidad)
        {
            // Valida que la cantidad sea mayor a cero
            if (cantidad <= 0) return;

            Stock += cantidad;
        }
    }

    // Clase derivada que representa productos en promoción
    public class ProductoPromocion : Producto
    {
        // Sobrescribe la propiedad descuento para aplicar promociones específicas
        public override decimal Descuento { get; set; }

        // Constructor que incluye el descuento
        public ProductoPromocion(
            int codigo,
            string nombre,
            decimal precio,
            int stock,
            decimal descuento,
            string descripcion
        )
        : base(codigo, nombre, precio, stock, descripcion)
        {
            Descuento = descuento;
        }

        // Sobrescribe el método para calcular el precio con descuento aplicado
        public override decimal CalcularPrecioVenta()
        {
            return Precio - (Precio * Descuento / 100);
        }
    }
}