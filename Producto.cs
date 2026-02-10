namespace SistemaVentas
{
    public class Producto
    {
        // Propiedades modernas (C# standard)
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        public Producto(string codigo, string nombre, decimal precio, int stock)
        {
            Codigo = codigo;
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
        }

        // Método virtual para permitir polimorfismo en el cálculo
        public virtual decimal CalcularPrecioVenta()
        {
            return Precio;
        }

        public void ReducirStock(int cantidad)
        {
            if (Stock >= cantidad) Stock -= cantidad;
            else Console.WriteLine("Advertencia: Stock insuficiente.");
        }
    }

    // --- Ejemplo de Polimorfismo: Producto con IVA reducido o Promoción ---
    public class ProductoPromocion : Producto
    {
        public decimal Descuento { get; set; }

        public ProductoPromocion(string codigo, string nombre, decimal precio, int stock, decimal descuento)
            : base(codigo, nombre, precio, stock)
        {
            Descuento = descuento;
        }

        // Aquí aplicamos Polimorfismo: El cálculo es diferente al del padre
        public override decimal CalcularPrecioVenta()
        {
            return Precio - (Precio * Descuento);
        }
    }
}