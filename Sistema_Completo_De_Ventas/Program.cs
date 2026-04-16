using Sistema_Completo_De_Ventas;
using MySqlConnector;
using System;
using System.Windows.Forms;

// Clase principal del programa
class Program
{
    // Método principal de entrada de la aplicación
    [STAThread]
    static void Main(string[] args)
    {
        // Intenta conectar a la base de datos antes de iniciar la aplicación
        ConectarADb();

        // Inicializa la configuración de la aplicación (WinForms)
        ApplicationConfiguration.Initialize();

        // Inicia la aplicación mostrando el formulario de login
        Application.Run(new Sistema_Completo_De_Ventas.UI.Forms.FrmLogin());
    }

    // Método que prueba la conexión a la base de datos
    static void ConectarADb()
    {
        // Crea una instancia de la clase de conexión
        Conexion conexionDB = new Conexion();

        try
        {
            // Abre la conexión usando la clase Conexion
            using (var conn = conexionDB.ObtenerConexion())
            {
                // Consulta simple para verificar que la conexión funciona
                string query = "SELECT NOW();";

                // Crea el comando SQL
                using (var cmd = new MySqlCommand(query, conn))
                {
                    // Ejecuta la consulta (no devuelve resultados visibles)
                    cmd.ExecuteScalar();
                }
            }
        }
        catch (Exception ex)
        {
            // Muestra un mensaje de error si falla la conexión
            MessageBox.Show(
                "Falló la conexión a la base de datos: " + ex.Message,
                "Error Crítico",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
}