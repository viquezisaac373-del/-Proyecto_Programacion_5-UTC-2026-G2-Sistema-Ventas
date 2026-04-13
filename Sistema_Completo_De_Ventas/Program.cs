using System;
using System.Windows.Forms;
using Sistema_Completo_De_Ventas.UI.Forms; // Referencia a la ubicación de FrmLogin
using SistemaVentas.DAL; // Referencia a donde reside la clase Conexion
using MySqlConnector;

namespace Sistema_Completo_De_Ventas
{
    // Clase principal que arranca la ejecución del software
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Inicialización necesaria para aplicaciones Windows Forms
            ApplicationConfiguration.Initialize();

            // 1. Verificación preventiva de la base de datos.
            // Esto evita que el usuario vea errores extraños si el servidor MySQL no está activo.
            if (ProbarConexionAlArranque())
            {
                // 2. CAMBIO ESTRATÉGICO:
                // Ahora el programa inicia con FrmLogin.
                // Si las credenciales son válidas, FrmLogin se encargará de abrir FrmPrincipal.
                Application.Run(new FrmLogin());
            }
            else
            {
                // Si la conexión falla, cerramos la aplicación para proteger la integridad del sistema.
                Application.Exit();
            }
        }

        /// <summary>
        /// Intenta establecer una conexión breve con la base de datos para asegurar disponibilidad.
        /// </summary>
        private static bool ProbarConexionAlArranque()
        {
            Conexion conexionDB = new Conexion();
            try
            {
                using (var conn = conexionDB.ObtenerConexion())
                {
                    // Si logramos abrir la conexión sin excepciones, el servidor está listo.
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Mensaje informativo para el usuario final en caso de fallo técnico.
                MessageBox.Show(
                    "Error Crítico: No se pudo conectar con el servidor de base de datos.\n\n" +
                    "Asegúrese de que MySQL esté encendido y la base de datos 'facturadb' configurada.\n\n" +
                    "Detalle: " + ex.Message,
                    "Fallo de Conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
        }
    }
}