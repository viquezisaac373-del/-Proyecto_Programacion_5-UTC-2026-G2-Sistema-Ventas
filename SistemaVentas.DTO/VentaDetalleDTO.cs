using System;
using System.Linq;
using System.Text;

namespace SistemaVentas.DTO
{
    public class VentaDetalleDTO
    {
        public int VentaId { get; set; }
        public int CodigoP { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}