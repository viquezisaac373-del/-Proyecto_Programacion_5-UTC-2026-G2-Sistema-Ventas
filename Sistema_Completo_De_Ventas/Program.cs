using Sistema_Completo_De_Ventas;
using MySqlConnector;
using System;
using System.Windows.Forms;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
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