using System;
using System.Drawing;
using System.Windows.Forms;
using SistemaVentas.BLL;
using SistemaVentas.DTO;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    // Se utiliza 'partial' para asegurar compatibilidad con el archivo de diseño oculto de Visual Studio
    public partial class FrmLogin : Form
    {
        // Controles declarados a nivel de clase con guion bajo para evitar choques con el diseñador
        private TextBox _txtUser = null!;
        private TextBox _txtPass = null!;
        private Button _btnLogin = null!;
        private Label _lblUser = null!;
        private Label _lblPass = null!;
        private Label _lblTitulo = null!;

        public FrmLogin()
        {
            // ÚNICO constructor. Llama a la inicialización nativa y luego a la nuestra.
            ConstruirInterfaz();
        }

        // Método personalizado para dibujar la interfaz de forma segura
        private void ConstruirInterfaz()
        {
            // Limpiamos la ventana para asegurarnos de que no haya controles duplicados
            this.Controls.Clear();

            this._txtUser = new TextBox();
            this._txtPass = new TextBox();
            this._btnLogin = new Button();
            this._lblUser = new Label();
            this._lblPass = new Label();
            this._lblTitulo = new Label();

            this.SuspendLayout();

            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(350, 280);
            this.Text = "Autenticación";

            _lblTitulo.Text = "ACCESO AL SISTEMA";
            _lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            _lblTitulo.Location = new Point(0, 20);
            _lblTitulo.Size = new Size(350, 30);
            _lblTitulo.TextAlign = ContentAlignment.MiddleCenter;

            _lblUser.Text = "Nombre de Usuario:";
            _lblUser.Location = new Point(50, 65);
            _lblUser.AutoSize = true;

            _txtUser.Location = new Point(50, 85);
            _txtUser.Size = new Size(250, 25);
            _txtUser.BackColor = Color.FromArgb(30, 30, 30);
            _txtUser.ForeColor = Color.White;
            _txtUser.BorderStyle = BorderStyle.FixedSingle;

            _lblPass.Text = "Contraseña:";
            _lblPass.Location = new Point(50, 115);
            _lblPass.AutoSize = true;

            _txtPass.Location = new Point(50, 135);
            _txtPass.Size = new Size(250, 25);
            _txtPass.UseSystemPasswordChar = true;
            _txtPass.BackColor = Color.FromArgb(30, 30, 30);
            _txtPass.ForeColor = Color.White;
            _txtPass.BorderStyle = BorderStyle.FixedSingle;

            _btnLogin.Text = "Entrar";
            _btnLogin.Location = new Point(50, 180);
            _btnLogin.Size = new Size(250, 40);
            _btnLogin.BackColor = Color.FromArgb(0, 122, 204);
            _btnLogin.FlatStyle = FlatStyle.Flat;
            _btnLogin.FlatAppearance.BorderSize = 0;
            _btnLogin.Cursor = Cursors.Hand;
            _btnLogin.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            // Única suscripción al evento clic para evitar el error CS0111
            _btnLogin.Click += btnLogin_Click;

            this.Controls.Add(_lblTitulo);
            this.Controls.Add(_lblUser);
            this.Controls.Add(_txtUser);
            this.Controls.Add(_lblPass);
            this.Controls.Add(_txtPass);
            this.Controls.Add(_btnLogin);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // ÚNICO evento de clic para el botón
        private void btnLogin_Click(object? sender, EventArgs e)
        {
            try
            {
                UsuarioService service = new UsuarioService();
                UsuarioDTO? usuarioLogueado = service.IniciarSesion(_txtUser.Text, _txtPass.Text);

                if (usuarioLogueado != null)
                {
                    // Usamos la ruta completa para evitar el error de referencia CS0246
                    Sistema_Completo_De_Ventas.UI.Forms.FrmPrincipal frm = new Sistema_Completo_De_Ventas.UI.Forms.FrmPrincipal(usuarioLogueado);
                    this.Hide();
                    frm.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de Acceso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}