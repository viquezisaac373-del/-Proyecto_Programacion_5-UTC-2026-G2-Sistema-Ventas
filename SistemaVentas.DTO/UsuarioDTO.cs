using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaVentas.DTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public int IdRol { get; set; }
    }
}
