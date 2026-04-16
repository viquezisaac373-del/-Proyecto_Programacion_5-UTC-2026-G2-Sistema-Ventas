using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.DTO
{
    // Clase DTO que representa una venta
    // Se usa para transportar los datos de la venta entre las capas del sistema
    public class VentaDTO
    {
        // Identificador único de la venta
        public int Id { get; set; }

        // ID del cliente que realizó la compra
        public int ClienteId { get; set; }

        // Fecha en la que se realizó la venta
        public DateTime Fecha { get; set; }

        // Subtotal de la venta (sin impuestos)
        public decimal Subtotal { get; set; }

        // Monto del impuesto aplicado a la venta
        public decimal Impuesto { get; set; }

        // Total final de la venta (subtotal + impuesto)
        public decimal Total { get; set; }
    }
}