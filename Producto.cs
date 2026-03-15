using System;

namespace Sistema_Completo_De_Ventas
{
    public class Producto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } = "";
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        public Producto(string codigo, string nombre, decimal precio, int stock)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(codigo))
                throw new Exception("El código no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre no puede estar vacío.");

            if (precio <= 0)
                throw new Exception("El precio debe ser mayor que cero.");

            if (stock < 0)
                throw new Exception("El stock no puede ser negativo.");

            Codigo = codigo;
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
        }

        public virtual decimal CalcularPrecioVenta()
        {
            return Precio;
        }

        public void ReducirStock(int cantidad)
        {
            if (cantidad <= 0) return;

            if (Stock >= cantidad) Stock -= cantidad;
            else Console.WriteLine("Advertencia: Stock insuficiente.");
        }

        public void AumentarStock(int cantidad)
        {
            if (cantidad <= 0) return;
            Stock += cantidad;
        }
    }

    public class ProductoPromocion : Producto
    {
        public decimal Descuento { get; set; }

        public ProductoPromocion(string codigo, string nombre, decimal precio, int stock, decimal descuento)
            : base(codigo, nombre, precio, stock)
        {
            if (descuento < 0 || descuento > 1)
                throw new Exception("El descuento debe estar entre 0 y 1 (ejemplo: 0.2 = 20%).");

            Descuento = descuento;
        }

        public override decimal CalcularPrecioVenta()
        {
            return Precio - (Precio * Descuento); // Devuelve el precio final restando el porcentaje de descuento
        }
    }
}
