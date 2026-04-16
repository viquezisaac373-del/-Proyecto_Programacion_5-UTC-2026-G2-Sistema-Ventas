using System;
using System.Linq;
using System.Text;

namespace SistemaVentas.DTO
{
    // Clase DTO para representar el detalle de una venta
    // Contiene la información de cada producto vendido dentro de una venta
    public class VentaDetalleDTO
    {
        // ID de la venta a la que pertenece este detalle
        public int VentaId { get; set; }

        // Código del producto vendido
        public int CodigoP { get; set; }

        // Cantidad de unidades vendidas del producto
        public int Cantidad { get; set; }

        // Precio unitario del producto al momento de la venta
        public decimal PrecioUnitario { get; set; }
    }
}