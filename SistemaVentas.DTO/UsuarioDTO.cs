using System;

namespace SistemaVentas.DTO
{
    // Esta clase es la que transporta la información del usuario entre las capas.
    // La propiedad 'Clave' es fundamental para que el DAO y el Service compilen.
    public class UsuarioDTO
    {
        // El ID del usuario en la base de datos
        public int IdUsuario { get; set; }

        // El nombre que se usa para el Login
        public string NombreUsuario { get; set; } = string.Empty;

        // PROPIEDAD FUNDAMENTAL: Aquí se almacena la contraseña.
        // Al agregar esta línea, se corrigen los errores CS1061 y CS0117.
        public string Clave { get; set; } = string.Empty;

        // El rol (Administrador, Cajero, Contador)
        public string Rol { get; set; } = string.Empty;
    }
}