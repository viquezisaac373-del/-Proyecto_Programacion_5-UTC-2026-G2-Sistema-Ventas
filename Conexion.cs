using MySqlConnector;

namespace Sistema_Completo_De_Ventas 
{
    public class Conexion // Clase encargada de administrar la conexión a la base de datos
    {
        private string cadenaConexion =
            "server=localhost;database=facturadb;user=root;password=;"; // Cadena de conexión con servidor, base de datos, usuario y contraseña

        public MySqlConnection ObtenerConexion() // Método que crea y devuelve una conexión abierta
        {
            MySqlConnection conexion = new MySqlConnection(cadenaConexion); // Se instancia la conexión usando la cadena definida
            conexion.Open(); // Se abre la conexión con el servidor MySQL
            return conexion; // Se retorna la conexión abierta para su uso en consultas
        }
    }
}