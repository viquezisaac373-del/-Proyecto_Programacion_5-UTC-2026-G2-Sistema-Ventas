using MySqlConnector;
namespace SistemaVentas
{ 
    public class Conexion
    {
        private string cadenaConexion =
            "server=localhost;database=facturadb;user=root;password=;"; // en "facturadb" utilizar el mismo nombre que le pusieron a la base de datos 
        public MySqlConnection ObtenerConexion()
        {
            MySqlConnection conexion = new MySqlConnection(cadenaConexion);
            conexion.Open();
            return conexion;
        }
    }
}