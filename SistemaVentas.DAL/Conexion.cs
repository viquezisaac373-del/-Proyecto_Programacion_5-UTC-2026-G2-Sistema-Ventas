using MySqlConnector;
using System;

namespace Sistema_Completo_De_Ventas
{
    // Clase encargada de gestionar la conexión a la base de datos
    public class Conexion
    {
        // Cadena de conexión con los datos del servidor, base de datos, usuario y contraseña
        private string cadenaConexion =
            "server=localhost;database=facturadb;user=root;password=;";

        // Método que crea, abre y devuelve una conexión a MySQL
        public MySqlConnection ObtenerConexion()
        {
            try
            {
                // Crea una nueva instancia de conexión usando la cadena definida
                MySqlConnection conexion = new MySqlConnection(cadenaConexion);

                // Abre la conexión con el servidor de base de datos
                conexion.Open();

                // Retorna la conexión abierta para ser utilizada en otras clases (DAO)
                return conexion;
            }
            catch (Exception ex)
            {
                // Manejo de error en caso de fallo de conexión
                throw new Exception("Error al conectar con la base de datos: " + ex.Message);
            }
        }
    }
}