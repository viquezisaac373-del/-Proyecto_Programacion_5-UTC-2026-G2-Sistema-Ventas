using System;
using System.Drawing;
using System.Windows.Forms;
using MySqlConnector;
using System.Security.Cryptography;
using System.Text;
using Sistema_Completo_De_Ventas;
using SistemaVentas.DTO;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    public partial class FrmLogin : Form
    {
        private int intentosFallidos = 0;
        private const int MAX_INTENTOS = 3;

        public FrmLogin()
        {
            InitializeComponent();
            // Ya no llamamos a ConfigurarEstiloPro() porque el 
            // nuevo diseño está completamente integrado en el Designer.
        }

        // --- LOS EVENTOS VACÍOS NECESARIOS POR EL DISEÑADOR ---
        private void FrmLogin_Load(object? sender, EventArgs e) { }
        private void txtUsuario_TextChanged(object? sender, EventArgs e) { }
        private void label1_Click(object? sender, EventArgs e) { }

        // --- LÓGICA DE INGRESO ---
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (intentosFallidos >= MAX_INTENTOS)
            {
                MessageBox.Show("Demasiados intentos. Reinicie la aplicación.", "Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            string user = txtUsuario.Text.Trim();
            string pass = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Complete los campos por favor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Conexion conexionDB = new Conexion();
                using (var conn = conexionDB.ObtenerConexion())
                {
                    string sql = @"SELECT u.id, u.usuario, u.password, u.id_rol, r.nombre as nombre_rol 
                                 FROM usuarios u 
                                 INNER JOIN roles r ON u.id_rol = r.id 
                                 WHERE u.usuario = @u";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", user);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string passBD = reader["password"]?.ToString() ?? "";
                                string passIngresada = EncriptarSHA256(pass);

                                if (passBD == passIngresada)
                                {
                                    UsuarioDTO dto = new UsuarioDTO
                                    {
                                        Id = Convert.ToInt32(reader["id"]),
                                        Usuario = reader["usuario"]?.ToString() ?? "",
                                        IdRol = Convert.ToInt32(reader["id_rol"]),
                                        NombreRol = reader["nombre_rol"]?.ToString() ?? ""
                                    };

                                    FrmPrincipal principal = new FrmPrincipal(dto);
                                    principal.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    RegistrarError();
                                }
                            }
                            else
                            {
                                RegistrarError();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RegistrarError()
        {
            intentosFallidos++;
            MessageBox.Show($"Usuario o contraseña incorrectos. Intento {intentosFallidos}/{MAX_INTENTOS}", "Error de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // --- ENCRIPTACIÓN DE CONTRASEÑA ---
        private string EncriptarSHA256(string texto)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}