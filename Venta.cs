using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace Sistema_Completo_De_Ventas
{
    public class CantidadInvalidaException : Exception
    {
        public CantidadInvalidaException(string mensaje) : base(mensaje) { }
    }

    public class StockInsuficienteException : Exception
    {
        public StockInsuficienteException(string mensaje) : base(mensaje) { }
    }

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
        public Cliente Cliente { get; set; }
        public List<VentaDetalle> Detalles { get; set; }

        public Venta(int id, Cliente cliente)
        {
            IdVenta = id;
            Fecha = DateTime.Now;
            Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));
            Detalles = new List<VentaDetalle>();
        }

        public void AgregarProducto(Producto producto, int cantidad)
        {
            if (cantidad <= 0)
                throw new CantidadInvalidaException("La cantidad debe ser mayor a cero.");

            if (producto.Stock >= cantidad)
            {
                producto.ReducirStock(cantidad);
                var existente = Detalles.FirstOrDefault(d => d.Producto.Codigo == producto.Codigo);
                if (existente != null)
                    existente.Cantidad += cantidad;
                else
                    Detalles.Add(new VentaDetalle { Producto = producto, Cantidad = cantidad });
            }
            else
            {
                throw new StockInsuficienteException($"No hay suficiente stock de {producto.Nombre}");
            }
        }

        public decimal CalcularTotal() => Detalles.Sum(d => d.Subtotal);

        public string GenerarFactura()
        {
            var factura = new System.Text.StringBuilder();
            factura.AppendLine("---------------- FACTURA ----------------");
            factura.AppendLine($"Venta #: {IdVenta} | Fecha: {Fecha}");
            factura.AppendLine($"Cliente: {Cliente.Nombre} | Correo: {Cliente.Correo}");
            factura.AppendLine("-----------------------------------------");

            foreach (var item in Detalles)
            {
                factura.AppendLine(
                    $"- {item.Producto.Codigo} | {item.Producto.Nombre} (x{item.Cantidad}) " +
                    $"@ {item.PrecioUnitario:C} = {item.Subtotal:C}"
                );
            }

            factura.AppendLine("-----------------------------------------");
            factura.AppendLine($"TOTAL A PAGAR: {CalcularTotal():C}");
            factura.AppendLine("-----------------------------------------");
            return factura.ToString();
        }

        public void MostrarFactura()
        {
            Console.WriteLine(GenerarFactura());
            GuardarFacturaArchivo();
        }

        public void GuardarFacturaArchivo()
        {
            try
            {
                File.WriteAllText($"factura_{IdVenta}.txt", GenerarFactura());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar factura: " + ex.Message);
            }
        }

        public void GuardarVentaJSON()
        {
            try
            {
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText($"venta_{IdVenta}.json", json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar venta en JSON: " + ex.Message);
            }
        }

        public static Venta LeerVentaJSON(string archivo)
        {
            try
            {
                string contenido = File.ReadAllText(archivo);
                return JsonSerializer.Deserialize<Venta>(contenido)!;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer venta desde JSON: " + ex.Message);
                return null!;
            }
        }
    }
}
