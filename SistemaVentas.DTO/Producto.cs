using System;

namespace Sistema_Completo_De_Ventas
{
    public class Producto
    {
        public int Codigo { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; } 

        public decimal Precio { get; set; } // Precio base

        public int Stock { get; set; } // Cantidad en inventario

        // 🔹 PROPIEDAD VIRTUAL PARA DESCUENTO
        public virtual decimal Descuento { get; set; } = 0;


        // 🔹 CONSTRUCTOR ACTUALIZADO
        public Producto(int codigo, string nombre, decimal precio, int stock, string descripcion)
        {
            Codigo = codigo;
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
            Descripcion = descripcion;
        }


        // Precio sin descuento (producto normal)
        public virtual decimal CalcularPrecioVenta()
        {
            return Precio;
        }


        public void ReducirStock(int cantidad)
        {
            if (cantidad <= 0) return;

            if (Stock >= cantidad)
                Stock -= cantidad;
            else
                Console.WriteLine("Advertencia: Stock insuficiente.");
        }


        public void AumentarStock(int cantidad)
        {
            if (cantidad <= 0) return;

            Stock += cantidad;
        }
    }


    public class ProductoPromocion : Producto
    {
        public override decimal Descuento { get; set; }


        // 🔹 CONSTRUCTOR ACTUALIZADO
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


        // Aplica el descuento al precio
        public override decimal CalcularPrecioVenta()
        {
            return Precio - (Precio * Descuento / 100);
        }
    }
}