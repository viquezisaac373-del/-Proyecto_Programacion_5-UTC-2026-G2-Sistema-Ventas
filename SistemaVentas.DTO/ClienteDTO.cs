using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.DTO
{
    // Clase DTO para representar un cliente
    // Se usa para transportar datos entre capas (DAL, BLL, UI)
    public class ClienteDTO
    {
        // Identificador único del cliente
        public int Id { get; set; }

        // Nombre del cliente
        public string Nombre { get; set; }

        // Correo electrónico del cliente
        public string Correo { get; set; }

        // Número de teléfono del cliente
        public string Telefono { get; set; }
    }
}