using Sistema_Completo_De_Ventas;
using MySqlConnector;
using System;
using System.Windows.Forms;
using System.Globalization; // 1. Agregado para poder cambiar a Colones
using System.Threading;     // 2. Agregado para poder cambiar a Colones

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        // --- INICIO DE CONFIGURACIÓN DE MONEDA (COSTA RICA) ---
        CultureInfo culturaCR = new CultureInfo("es-CR");
        culturaCR.NumberFormat.CurrencySymbol = "₡";

        Thread.CurrentThread.CurrentCulture = culturaCR;
        Thread.CurrentThread.CurrentUICulture = culturaCR;
        // --- FIN DE CONFIGURACIÓN ---

        ConectarADb();

        ApplicationConfiguration.Initialize();

        // ARRANCA EN LOGIN
        Application.Run(new Sistema_Completo_De_Ventas.UI.Forms.FrmLogin());
    }

    static void ConectarADb()
    {
        Conexion conexionDB = new Conexion();

        try
        {
            using (var conn = conexionDB.ObtenerConexion())
            {
                string query = "SELECT NOW();";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.ExecuteScalar();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Falló la conexión a la base de datos: " + ex.Message,
                "Error Crítico",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
}