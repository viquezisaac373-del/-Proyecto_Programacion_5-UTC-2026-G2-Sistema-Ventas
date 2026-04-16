namespace SistemaVentas.DTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int IdRol { get; set; }

        // Esta propiedad extra nos sirve para mostrar el texto en pantalla (ej: "Administrador")
        public string NombreRol { get; set; } = string.Empty;
    }
}