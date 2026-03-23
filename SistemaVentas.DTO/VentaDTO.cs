using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.DTO
{
    public class VentaDTO
    {
        public int Id { get; set; } 
        public int ClienteId { get; set; }
        public DateTime Fecha { get; set; }
    }
}