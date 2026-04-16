using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySqlConnector;
using System.Security.Cryptography;
using System.Text;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    // Formulario de inicio de sesión
    public partial class FrmLogin : Form
    {
        // Constructor del formulario
        public FrmLogin()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(31, 31, 31);
            // Esto hace que el formulario siempre abra centrado
            this.StartPosition = FormStartPosition.CenterScreen;
            // Hace que Enter dispare el botón de ingresar
            this.AcceptButton = btnIngresar;
        }

        // Evento que se ejecuta al cargar el formulario
        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        // Evento del label (no se está utilizando)
        private void label1_Click(object sender, EventArgs e)
        {

        }

        // Evento del botón Ingresar
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            // Obtener datos ingresados por el usuario
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Validar que no estén vacíos
            if (usuario == "" || password == "")
            {
                MessageBox.Show("Debe ingresar usuario y contraseña");
                return;
            }

            // Encriptar la contraseña ingresada
            string passwordEncriptada = EncriptarPassword(password);

            // Crear conexión a la base de datos
            Conexion conexionDB = new Conexion();

            try
            {
                using (var conn = conexionDB.ObtenerConexion())
                {
                    // Consulta para obtener la contraseña del usuario
                    string query = "SELECT password FROM usuarios WHERE usuario = @usuario";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        // Parámetro para evitar SQL Injection
                        cmd.Parameters.AddWithValue("@usuario", usuario);

                        // Ejecutar consulta y obtener resultado
                        var resultado = cmd.ExecuteScalar();

                        // Verificar si el usuario existe
                        if (resultado == null)
                        {
                            MessageBox.Show("El usuario no existe");
                            return;
                        }

                        // Obtener la contraseña almacenada en la BD
                        string passwordBD = resultado.ToString();

                        if (passwordBD != passwordEncriptada)
                        {
                            MessageBox.Show("Contraseña incorrecta");
                            return;
                        }

                        // LOGIN CORRECTO
                        FrmPrincipal frm = new FrmPrincipal();
                        frm.Show();

                        this.Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Error al iniciar sesión: " + ex.Message);
            }
        }

        // Método para encriptar la contraseña usando SHA256
        private string EncriptarPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(
                    Encoding.UTF8.GetBytes(password)
                );

                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        // Evento cuando cambia el texto del usuario (no se usa actualmente)
        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
