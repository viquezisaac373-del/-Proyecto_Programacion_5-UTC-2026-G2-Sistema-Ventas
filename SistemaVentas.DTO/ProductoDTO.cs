using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.DTO
{
    // Clase DTO  para representar un producto
    // Se utiliza para transportar datos entre las capas del sistema (DAL, BLL, UI)
    public class ProductoDTO
    {
        // Código único del producto
        public int Codigo { get; set; }

        // Nombre del producto
        public string Nombre { get; set; }

        // Descripción del producto
        public string Descripcion { get; set; }

        // Precio base del producto
        public decimal Precio { get; set; }

        // Cantidad disponible en inventario
        public int Stock { get; set; }

        // Porcentaje de descuento aplicado al producto (si tiene)
        public decimal Descuento { get; set; }
    }
}