using System;

namespace Sistema_Completo_De_Ventas
{
    public class Producto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } = "";
        public decimal Precio { get; set; } // Identificadores de codigo, nombre, descripcion, precio
        public int Stock { get; set; } // Cantidad disponible en inventario

        public Producto(string codigo, string nombre, decimal precio, int stock)
        {
            Codigo = codigo;
            Nombre = nombre;
            Precio = precio; // Asigna el código recibido, precio base y nombre recibido
            Stock = stock; // Asigna la cantidad inicial en inventario
        }

        public virtual decimal CalcularPrecioVenta()
        {
            return Precio; // En el producto normal, el precio de venta es igual al precio base
        }

        public void ReducirStock(int cantidad) // Disminuye el inventario cuando se realiza una venta
        {
            if (cantidad <= 0) return; // Evita procesar cantidades inválidas

            if (Stock >= cantidad) Stock -= cantidad; // Resta la cantidad si hay suficiente inventario
            else Console.WriteLine("Advertencia: Stock insuficiente."); // Muestra advertencia si no hay suficiente stock
        }

        public void AumentarStock(int cantidad) // Incrementa el inventario, por ejemplo en reposiciones
        {
            if (cantidad <= 0) return;
            Stock += cantidad; // Suma la cantidad al inventario actual
        }
    }

    public class ProductoPromocion : Producto // Clase derivada que representa un producto con descuento
    {
        public decimal Descuento { get; set; } // Porcentaje de descuento aplicado al precio base

        public ProductoPromocion(string codigo, string nombre, decimal precio, int stock, decimal descuento)
            : base(codigo, nombre, precio, stock) // Llama al constructor de la clase base Producto
        {
            Descuento = descuento; // Asigna el descuento específico para este producto
        }

        public override decimal CalcularPrecioVenta() // Sobrescribe el cálculo del precio para aplicar descuento
        {
            return Precio - (Precio * Descuento); // Devuelve el precio final restando el porcentaje de descuento
        }
    }
}