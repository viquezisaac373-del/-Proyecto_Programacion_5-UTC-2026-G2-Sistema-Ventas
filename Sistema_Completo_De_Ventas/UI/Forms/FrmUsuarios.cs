using System;
using System.Drawing;
using System.Windows.Forms;
using SistemaVentas.BLL;
using SistemaVentas.DTO;
using System.Security.Cryptography;
using System.Text;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    public class FrmUsuarios : Form
    {
        private DataGridView dgvUsuarios;
        private Label lblTitulo;

        private Panel pnlAcciones;

        private Label lblId;
        private TextBox txtId;

        private Label lblUsuario;
        private TextBox txtUsuario;

        private Label lblPassword;
        private TextBox txtPassword;

        private Label lblRol;
        private ComboBox cmbRol;

        private Button btnGuardar;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnLimpiar;

        public FrmUsuarios()
        {
            InitializeComponent();
            txtId.Enabled = false;
        }

        private void InitializeComponent()
        {
            this.dgvUsuarios = new DataGridView();
            this.lblTitulo = new Label();
            this.pnlAcciones = new Panel();

            this.lblId = new Label();
            this.txtId = new TextBox();

            this.lblUsuario = new Label();
            this.txtUsuario = new TextBox();

            this.lblPassword = new Label();
            this.txtPassword = new TextBox();

            this.lblRol = new Label();
            this.cmbRol = new ComboBox();

            this.btnGuardar = new Button();
            this.btnEditar = new Button();
            this.btnEliminar = new Button();
            this.btnLimpiar = new Button();

            // Título
            lblTitulo.Text = "Gestión de Usuarios";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 20);

            // DataGrid
            dgvUsuarios.Location = new Point(25, 70);
            dgvUsuarios.Size = new Size(500, 400);
            dgvUsuarios.CellDoubleClick += DgvUsuarios_CellDoubleClick;

            // Panel
            pnlAcciones.Location = new Point(540, 70);
            pnlAcciones.Size = new Size(240, 400);

            // Inputs
            ConfigurarInput(lblId, txtId, "ID:", 20);
            ConfigurarInput(lblUsuario, txtUsuario, "Usuario:", 80);
            ConfigurarInput(lblPassword, txtPassword, "Contraseña:", 140);

            txtPassword.UseSystemPasswordChar = true;

            // ComboBox Rol
            lblRol.Text = "Rol:";
            lblRol.Location = new Point(15, 200);
            lblRol.ForeColor = Color.White;

            cmbRol.Location = new Point(15, 225);
            cmbRol.Size = new Size(210, 23);

            // Botones
            ConfigurarBoton(btnGuardar, "Guardar", 260);
            btnGuardar.Click += BtnGuardar_Click;

            ConfigurarBoton(btnEditar, "Editar", 305);
            btnEditar.Click += BtnEditar_Click;

            ConfigurarBoton(btnEliminar, "Eliminar", 350);
            btnEliminar.Click += BtnEliminar_Click;

            btnLimpiar.Text = "Limpiar";
            btnLimpiar.Location = new Point(15, 395);
            btnLimpiar.Size = new Size(210, 30);
            btnLimpiar.Click += BtnLimpiar_Click;

            // Agregar al panel
            pnlAcciones.Controls.AddRange(new Control[] {
                lblId, txtId,
                lblUsuario, txtUsuario,
                lblPassword, txtPassword,
                lblRol, cmbRol,
                btnGuardar, btnEditar, btnEliminar, btnLimpiar
            });

            // Form
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ClientSize = new Size(800, 500);

            this.Controls.Add(lblTitulo);
            this.Controls.Add(dgvUsuarios);
            this.Controls.Add(pnlAcciones);

            this.Load += FrmUsuarios_Load;
        }

        private void ConfigurarInput(Label lbl, TextBox txt, string texto, int y)
        {
            lbl.Text = texto;
            lbl.Location = new Point(15, y);
            lbl.ForeColor = Color.White;

            txt.Location = new Point(15, y + 25);
            txt.Size = new Size(210, 23);
        }

        private void ConfigurarBoton(Button btn, string texto, int y)
        {
            btn.Text = texto;
            btn.Location = new Point(15, y);
            btn.Size = new Size(210, 35);
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
            CargarRoles();
        }

        private void CargarUsuarios()
        {
            var service = new UsuarioService();
            dgvUsuarios.DataSource = service.ObtenerUsuarios();
        }

        private void CargarRoles()
        {
            var service = new RolService();
            var roles = service.ObtenerRoles();

            cmbRol.DataSource = roles;
            cmbRol.DisplayMember = "Nombre"; // lo que ve el usuario
            cmbRol.ValueMember = "Id";       // lo que se guarda
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Use EDITAR para modificar un usuario");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("La contraseña no puede estar vacía");
                return;
            }

            var usuario = new UsuarioDTO
            {
                Usuario = txtUsuario.Text,
                Password = EncriptarPassword(txtPassword.Text),
                IdRol = Convert.ToInt32(cmbRol.SelectedValue)
            };

            new UsuarioService().InsertarUsuario(usuario);

            CargarUsuarios();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["Id"].Value);

            var usuario = new UsuarioDTO
            {
                Id = id,
                Usuario = txtUsuario.Text,
                Password = EncriptarPassword(txtPassword.Text),
                IdRol = Convert.ToInt32(cmbRol.SelectedValue)
            };

            new UsuarioService().ActualizarUsuario(usuario);
            CargarUsuarios();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["Id"].Value);

            new UsuarioService().EliminarUsuario(id);
            CargarUsuarios();
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            txtId.Clear();
            txtUsuario.Clear();
            txtPassword.Clear();
        }

        private void DgvUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvUsuarios.Rows[e.RowIndex];

                txtId.Text = row.Cells["Id"].Value.ToString();
                txtUsuario.Text = row.Cells["Usuario"].Value.ToString();

                int idRol = Convert.ToInt32(row.Cells["IdRol"].Value);

                cmbRol.SelectedValue = idRol; // clave
            }
        }

        private string EncriptarPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();

                foreach (var b in bytes)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }
    }
}