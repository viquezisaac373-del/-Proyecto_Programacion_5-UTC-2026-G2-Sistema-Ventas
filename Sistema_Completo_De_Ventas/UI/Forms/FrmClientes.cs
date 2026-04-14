using System;
using System.Drawing;
using System.Windows.Forms;
using SistemaVentas.DAL;
using SistemaVentas.BLL;
using SistemaVentas.DTO;
using System.Text.RegularExpressions;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    public class FrmClientes : Form
    {
        private DataGridView dgvClientes;
        private Label lblTitulo;

        // Controles CRUD
        private Panel pnlAcciones;
        private Label lblId;
        private TextBox txtId;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblCorreo;
        private TextBox txtCorreo;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Button btnGuardar;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private Button btnExportar;

        public FrmClientes()
        {
            InitializeComponent();

            txtId.Enabled = false; //bloqueado siempre
        }

        private void InitializeComponent()
        {
            this.dgvClientes = new DataGridView();
            this.lblTitulo = new Label();
            this.pnlAcciones = new Panel();
            this.lblId = new Label();
            this.txtId = new TextBox();
            this.lblNombre = new Label();
            this.txtNombre = new TextBox();
            this.lblCorreo = new Label();
            this.txtCorreo = new TextBox();
            this.lblTelefono = new Label();
            this.txtTelefono = new TextBox();

            this.btnGuardar = new Button();
            this.btnEditar = new Button();
            this.btnEliminar = new Button();
            this.btnLimpiar = new Button();
            this.btnExportar = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).BeginInit();
            this.pnlAcciones.SuspendLayout();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Theme.DarkText;
            this.lblTitulo.Location = new Point(20, 20);
            this.lblTitulo.Text = "Gestión de Clientes";

            // dgvClientes
            this.dgvClientes.Location = new Point(25, 70);
            this.dgvClientes.Size = new Size(500, 400); // Se reduce el ancho para hacer espacio al panel
            this.dgvClientes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Theme.ApplyDarkDataGridView(this.dgvClientes);
            this.dgvClientes.CellDoubleClick += DgvClientes_CellDoubleClick;

            // pnlAcciones
            this.pnlAcciones.BackColor = Theme.DarkControl;
            this.pnlAcciones.Location = new Point(540, 70);
            this.pnlAcciones.Size = new Size(240, 400);
            this.pnlAcciones.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            this.pnlAcciones.Padding = new Padding(15);

            // Labels y TextBoxes en panel
            ConfigurarInputCRUD(lblId, txtId, "ID Cliente:", 20);
            ConfigurarInputCRUD(lblNombre, txtNombre, "Nombre Completo:", 80);
            ConfigurarInputCRUD(lblCorreo, txtCorreo, "Correo Electrónico:", 140);
            ConfigurarInputCRUD(lblTelefono, txtTelefono, "Teléfono:", 200);
            this.txtTelefono.KeyPress += TxtTelefono_KeyPress;

            // Botones
            ConfigurarBotonCRUD(btnGuardar, "Guardar", 260, Theme.AccentColor);
            this.btnGuardar.Click += BtnGuardar_Click;

            ConfigurarBotonCRUD(btnEditar, "Editar", 305, Color.FromArgb(200, 150, 0));
            this.btnEditar.Click += BtnEditar_Click;

            ConfigurarBotonCRUD(btnEliminar, "Eliminar", 350, Color.FromArgb(200, 50, 50));
            this.btnEliminar.Click += BtnEliminar_Click;

            this.pnlAcciones.Size = new Size(240, 440);

            this.btnLimpiar.Location = new Point(15, 395);
            this.btnLimpiar.Size = new Size(210, 30);
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.BackColor = Theme.DarkDesktop;
            this.btnLimpiar.ForeColor = Color.White;
            this.btnLimpiar.FlatStyle = FlatStyle.Flat;
            this.btnLimpiar.Click += BtnLimpiar_Click;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.BackColor = Theme.DarkDesktop;
            this.btnLimpiar.ForeColor = Color.White;
            this.btnLimpiar.FlatStyle = FlatStyle.Flat;
            this.btnLimpiar.Click += BtnLimpiar_Click;

            this.btnExportar.Location = new Point(406, 12);
            this.btnExportar.Size = new Size(200, 35);
            this.btnExportar.Text = "Exportar a JSON";
            this.btnExportar.BackColor = Theme.AccentColor;
            this.btnExportar.ForeColor = Color.White;
            this.btnExportar.FlatStyle = FlatStyle.Flat;
            this.btnExportar.Click += BtnExportar_Click;

            this.pnlAcciones.Controls.Add(lblId);
            this.pnlAcciones.Controls.Add(txtId);
            this.pnlAcciones.Controls.Add(lblNombre);
            this.pnlAcciones.Controls.Add(txtNombre);
            this.pnlAcciones.Controls.Add(lblCorreo);
            this.pnlAcciones.Controls.Add(txtCorreo);
            this.pnlAcciones.Controls.Add(lblTelefono);
            this.pnlAcciones.Controls.Add(txtTelefono);
            this.pnlAcciones.Controls.Add(btnGuardar);
            this.pnlAcciones.Controls.Add(btnEditar);
            this.pnlAcciones.Controls.Add(btnEliminar);
            this.pnlAcciones.Controls.Add(btnLimpiar);

            // FrmClientes
            this.BackColor = Theme.DarkDesktop;
            this.ClientSize = new Size(800, 500);
            this.Controls.Add(this.pnlAcciones);
            this.Controls.Add(this.dgvClientes);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.btnExportar);
            this.Name = "FrmClientes";
            this.Text = "Clientes";
            this.Load += FrmClientes_Load;

            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).EndInit();
            this.pnlAcciones.ResumeLayout(false);
            this.pnlAcciones.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void ConfigurarInputCRUD(Label lbl, TextBox txt, string texto, int y)
        {
            lbl.AutoSize = true;
            lbl.ForeColor = Theme.DarkText;
            lbl.Location = new Point(15, y);
            lbl.Text = texto;

            txt.Location = new Point(15, y + 25);
            txt.Size = new Size(210, 23);
            txt.BackColor = Theme.DarkDesktop;
            txt.ForeColor = Color.White;
            txt.BorderStyle = BorderStyle.FixedSingle;
        }

        private void ConfigurarBotonCRUD(Button btn, string texto, int y, Color color)
        {
            btn.Location = new Point(15, y);
            btn.Size = new Size(210, 35);
            btn.Text = texto;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        private void FrmClientes_Load(object? sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void TxtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números y tecla borrar
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Bloquea letras
            }
        }
        private void CargarGrilla()
        {
            try
            {
                this.dgvClientes.DataSource = null;
                var service = new ClienteService();
                var clientes = service.ObtenerClientes();
                this.dgvClientes.DataSource = clientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}");
            }
        }

        private void LimpiarCampos()
        {
            txtId.Clear();
            txtNombre.Clear();
            txtCorreo.Clear();
            txtTelefono.Clear();
            txtId.ReadOnly = true;
        }

        private bool ValidarCampos()
        {

            // Nombre
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || txtNombre.Text.Trim().Length < 3)
            {
                MessageBox.Show("El nombre debe tener al menos 3 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            // Correo
            if (string.IsNullOrWhiteSpace(txtCorreo.Text) || !EsCorreoValido(txtCorreo.Text))
            {
                MessageBox.Show("Ingrese un correo electrónico válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCorreo.Focus();
                return false;
            }

            // Teléfono
            if (string.IsNullOrWhiteSpace(txtTelefono.Text) || txtTelefono.Text.Trim().Length < 4)
            {
                MessageBox.Show("El teléfono debe tener al menos 4 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTelefono.Focus();
                return false;
            }

            return true;
        }

        private bool EsCorreoValido(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            // Validación más estricta
            string patron = @"^[a-zA-Z0-9._%+-]{3,}@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (!Regex.IsMatch(correo, patron))
                return false;

            // Validar que el dominio no sea muy corto
            string dominio = correo.Split('@')[1];
            string nombreDominio = dominio.Split('.')[0];

            if (nombreDominio.Length < 3)
                return false;

            return true;
        }

        private void BtnLimpiar_Click(object? sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void DgvClientes_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvClientes.Rows[e.RowIndex];
                txtId.Text = row.Cells["Id"].Value?.ToString();
                txtId.ReadOnly = true;
                txtNombre.Text = row.Cells["Nombre"].Value?.ToString();
                txtCorreo.Text = row.Cells["Correo"].Value?.ToString();
                txtTelefono.Text = row.Cells["Telefono"].Value?.ToString();
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos())
                    return;

                if (!string.IsNullOrWhiteSpace(txtId.Text))
                {
                    MessageBox.Show(
                        "Este cliente ya existe. Use el botón EDITAR.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                var service = new ClienteService();

                var cliente = new ClienteDTO
                {
                    Nombre = txtNombre.Text,
                    Correo = txtCorreo.Text,
                    Telefono = txtTelefono.Text
                };

                service.InsertarCliente(cliente);

                CargarGrilla();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al guardar: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnEditar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (dgvClientes.CurrentRow == null)
                {
                    MessageBox.Show("Seleccione un cliente.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidarCampos())
                    return;

                int id = Convert.ToInt32(dgvClientes.CurrentRow.Cells["Id"].Value);

                var cliente = new ClienteDTO
                {
                    Id = id,
                    Nombre = txtNombre.Text,
                    Correo = txtCorreo.Text,
                    Telefono = txtTelefono.Text
                };

                var service = new ClienteService();
                service.ActualizarCliente(cliente);

                CargarGrilla();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (dgvClientes.CurrentRow == null)
                {
                    MessageBox.Show("Seleccione un cliente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var res = MessageBox.Show("¿Seguro que desea eliminar?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvClientes.CurrentRow.Cells["Id"].Value);

                    var service = new ClienteService();
                    service.EliminarCliente(id);

                    CargarGrilla();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportar_Click(object? sender, EventArgs e)
        {
            try
            {
                var service = new ClienteService();
                var clientes = service.ObtenerClientes();
                JsonHelper.ExportarClientes(clientes);
                MessageBox.Show("Clientes exportados correctamente a su Escritorio.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error exportando los archivos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}