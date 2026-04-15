using System;
using System.Drawing;
using System.Windows.Forms;
using SistemaVentas.DTO;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    public partial class FrmPrincipal : Form
    {
        private UsuarioDTO _usuario;

        public FrmPrincipal(UsuarioDTO usuario)
        {
            InitializeComponent();
            _usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));

            // Garantiza que el proceso se cierre al salir
            this.FormClosed += (s, args) => Application.Exit();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            lblUsuarioLogueado.Text = $"{_usuario.Usuario} ({_usuario.NombreRol})";

            // Seguridad: Ocultar todo primero
            btnMenuVentas.Visible = false;
            btnMenuProductos.Visible = false;
            btnMenuClientes.Visible = false;
            btnMenuReportes.Visible = false;
            btnMenuUsuarios.Visible = false;

            string rol = _usuario.NombreRol.Trim().ToUpper();

            try
            {
                if (rol == "ADMINISTRADOR" || rol == "ADMIN")
                {
                    btnMenuVentas.Visible = true;
                    btnMenuProductos.Visible = true;
                    btnMenuClientes.Visible = true;
                    btnMenuReportes.Visible = true;
                    btnMenuUsuarios.Visible = true;
                    AbrirFormulario(new FrmVentas());
                }
                else if (rol == "CAJERO")
                {
                    btnMenuVentas.Visible = true;
                    btnMenuReportes.Visible = true;
                    AbrirFormulario(new FrmVentas());
                }
                else if (rol == "AUDITOR")
                {
                    btnMenuReportes.Visible = true;
                    AbrirFormulario(new FrmReportes());
                }

                ReorganizarMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void ReorganizarMenu()
        {
            int y = 150; // Posición inicial
            Button[] botones = { btnMenuVentas, btnMenuProductos, btnMenuClientes, btnMenuReportes, btnMenuUsuarios };

            foreach (Button btn in botones)
            {
                if (btn.Visible)
                {
                    btn.Location = new Point(0, y);
                    y += 55;
                }
            }
        }

        private void AbrirFormulario(Form fh)
        {
            if (this.pnlContenedor.Controls.Count > 0)
                this.pnlContenedor.Controls.RemoveAt(0);

            fh.TopLevel = false;
            fh.FormBorderStyle = FormBorderStyle.None;
            fh.Dock = DockStyle.Fill;
            this.pnlContenedor.Controls.Add(fh);
            this.pnlContenedor.Tag = fh;
            fh.Show();
        }

        private void btnMenuVentas_Click(object sender, EventArgs e) => AbrirFormulario(new FrmVentas());
        private void btnMenuProductos_Click(object sender, EventArgs e) => AbrirFormulario(new FrmProductos());
        private void btnMenuClientes_Click(object sender, EventArgs e) => AbrirFormulario(new FrmClientes());
        private void btnMenuReportes_Click(object sender, EventArgs e) => AbrirFormulario(new FrmReportes());
        private void btnMenuUsuarios_Click(object sender, EventArgs e) => AbrirFormulario(new FrmUsuarios());

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar sesión?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Restart();
            }
        }
    }
}