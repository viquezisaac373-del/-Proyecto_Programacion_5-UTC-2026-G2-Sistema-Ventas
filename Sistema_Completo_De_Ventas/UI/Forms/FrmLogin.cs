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
    // Formulario de inicio de sesión con límite de intentos y encriptación SHA256
    public partial class FrmLogin : Form
    {
        private int intentosFallidos = 0;        // Contador de intentos fallidos
        private const int MAX_INTENTOS = 3;      // Límite máximo antes de bloquear el acceso

        public FrmLogin()
        {
            InitializeComponent();
            // Permite iniciar sesión presionando Enter desde el campo de contraseña
            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) btnIngresar_Click(s, e); };
        }

        // Eventos requeridos por el diseñador, sin lógica adicional
        private void FrmLogin_Load(object? sender, EventArgs e) { }
        private void txtUsuario_TextChanged(object? sender, EventArgs e) { }
        private void label1_Click(object? sender, EventArgs e) { }

        // Valida credenciales contra la BD y abre el sistema si son correctas
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            // Si se superó el límite de intentos, se bloquea el acceso
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
                    // Se busca el usuario junto con su rol para construir el DTO de sesión
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
                                // La contraseña ingresada se encripta para compararla con la almacenada
                                string passIngresada = EncriptarSHA256(pass);

                                if (passBD == passIngresada)
                                {
                                    // Credenciales correctas: se crea el DTO y se abre el sistema
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
                                else { RegistrarError(); }
                            }
                            else { RegistrarError(); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Incrementa el contador de fallos y notifica al usuario cuántos intentos le quedan
        private void RegistrarError()
        {
            intentosFallidos++;
            MessageBox.Show($"Usuario o contraseña incorrectos. Intento {intentosFallidos}/{MAX_INTENTOS}", "Error de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // Convierte el texto a un hash SHA256 en formato hexadecimal.
        // Las contraseñas nunca se comparan en texto plano, siempre en su forma encriptada.
        private string EncriptarSHA256(string texto)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash) sb.Append(b.ToString("x2")); // Convierte cada byte a hexadecimal
                return sb.ToString();
            }
        }

        // Cierra la aplicación al presionar la X del formulario
        private void btnCerrar_Click(object? sender, EventArgs e) => Application.Exit();

        // Efecto visual: la X cambia de color al pasar el mouse por encima
        private void btnCerrar_MouseEnter(object? sender, EventArgs e) => btnCerrar.ForeColor = Color.White;
        private void btnCerrar_MouseLeave(object? sender, EventArgs e) => btnCerrar.ForeColor = Color.DimGray;
    }
}