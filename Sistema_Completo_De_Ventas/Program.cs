using Sistema_Completo_De_Ventas;
using MySqlConnector;
using System;
using System.Windows.Forms;
using System.Globalization; // Necesario para manejar configuraciones regionales (moneda, idioma)
using System.Threading;     // Necesario para aplicar la cultura al hilo principal de la aplicación

// Clase principal que sirve como punto de entrada del sistema de ventas
class Program
{
    // Método Main: es el primero en ejecutarse al iniciar la aplicación.
    // [STAThread] es requerido por WinForms para manejar correctamente la interfaz gráfica.
    [STAThread]
    static void Main(string[] args)
    {
        // Se configura la cultura regional de Costa Rica para que el sistema
        // muestre precios y formatos numéricos en colones (₡)
        CultureInfo culturaCR = new CultureInfo("es-CR");
        culturaCR.NumberFormat.CurrencySymbol = "₡";

        // Se aplica la cultura al hilo actual, afectando tanto los datos
        // como los textos mostrados en la interfaz
        Thread.CurrentThread.CurrentCulture = culturaCR;
        Thread.CurrentThread.CurrentUICulture = culturaCR;

        // Se verifica que la conexión a la base de datos esté disponible antes de continuar
        ConectarADb();

        // Se inicializa WinForms con la configuración del proyecto (DPI, estilos visuales, etc.)
        ApplicationConfiguration.Initialize();

        // Se lanza la aplicación mostrando el formulario de inicio de sesión
        Application.Run(new Sistema_Completo_De_Ventas.UI.Forms.FrmLogin());
    }

    // Método que verifica si la conexión a la base de datos es exitosa al arrancar el sistema.
    // Si falla, muestra un mensaje de error crítico al usuario.
    static void ConectarADb()
    {
        Conexion conexionDB = new Conexion();
        try
        {
            using (var conn = conexionDB.ObtenerConexion())
            {
                // Se ejecuta una consulta mínima solo para confirmar que la BD responde
                string query = "SELECT NOW();";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.ExecuteScalar();
                }
            }
        }
        catch (Exception ex)
        {
            // Si la conexión falla, se notifica al usuario y se evita que el sistema
            // continúe cargando sin acceso a los datos
            MessageBox.Show(
                "Falló la conexión a la base de datos: " + ex.Message,
                "Error Crítico",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
}