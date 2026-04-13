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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(31, 31, 31);
            // Esto hace que el formulario siempre abra centrado
            this.StartPosition = FormStartPosition.CenterScreen;
            // Hace que Enter dispare el botón de ingresar
            this.AcceptButton = btnIngresar;
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (usuario == "" || password == "")
            {
                MessageBox.Show("Debe ingresar usuario y contraseña");
                return;
            }

            string passwordEncriptada = EncriptarPassword(password);

            Conexion conexionDB = new Conexion();

            try
            {
                using (var conn = conexionDB.ObtenerConexion())
                {
                    string query = "SELECT password FROM usuarios WHERE usuario = @usuario";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);

                        var resultado = cmd.ExecuteScalar();

                        if (resultado == null)
                        {
                            MessageBox.Show("El usuario no existe");
                            return;
                        }

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
                MessageBox.Show("Error al iniciar sesión: " + ex.Message);
            }
        }
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

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
