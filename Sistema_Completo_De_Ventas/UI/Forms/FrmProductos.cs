using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SistemaVentas.BLL;
using SistemaVentas.DTO;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    public class FrmProductos : Form
    {
        // = null!; agregado a todos para quitar los errores
        private DataGridView dgvProductos = null!;
        private Label lblTitulo = null!;
        private Panel pnlAcciones = null!;
        private Label lblCodigo = null!;
        private TextBox txtCodigo = null!;
        private Label lblNombre = null!;
        private TextBox txtNombre = null!;
        private Label lblDescripcion = null!;
        private TextBox txtDescripcion = null!;
        private Label lblPrecio = null!;
        private TextBox txtPrecio = null!;
        private Label lblStock = null!;
        private NumericUpDown numStock = null!;
        private Label lblDescuento = null!;
        private NumericUpDown numDescuento = null!;
        private Button btnGuardar = null!;
        private Button btnEditar = null!;
        private Button btnEliminar = null!;
        private Button btnLimpiar = null!;
        private Button btnExportar = null!;

        public FrmProductos()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.dgvProductos = new DataGridView();
            this.lblTitulo = new Label();
            this.pnlAcciones = new Panel();
            this.lblCodigo = new Label();
            this.txtCodigo = new TextBox();
            this.lblNombre = new Label();
            this.txtNombre = new TextBox();
            this.lblDescripcion = new Label();
            this.txtDescripcion = new TextBox();
            this.lblPrecio = new Label();
            this.txtPrecio = new TextBox();
            this.lblStock = new Label();
            this.numStock = new NumericUpDown();
            this.lblDescuento = new Label();
            this.numDescuento = new NumericUpDown();
            this.btnGuardar = new Button();
            this.btnEditar = new Button();
            this.btnEliminar = new Button();
            this.btnLimpiar = new Button();
            this.btnExportar = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDescuento)).BeginInit();
            this.pnlAcciones.SuspendLayout();
            this.SuspendLayout();

            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Theme.DarkText;
            this.lblTitulo.Location = new Point(20, 20);
            this.lblTitulo.Text = "Gestión de Productos";

            this.dgvProductos.Location = new Point(25, 70);
            this.dgvProductos.Size = new Size(500, 430);
            this.dgvProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Theme.ApplyDarkDataGridView(this.dgvProductos);
            this.dgvProductos.CellDoubleClick += DgvProductos_CellDoubleClick;

            this.pnlAcciones.BackColor = Theme.DarkControl;
            this.pnlAcciones.Location = new Point(540, 70);
            this.pnlAcciones.Size = new Size(240, 500);
            this.pnlAcciones.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            this.pnlAcciones.Padding = new Padding(15);

            ConfigurarInputCRUD(lblCodigo, txtCodigo, "ID Producto:", 10);
            ConfigurarInputCRUD(lblNombre, txtNombre, "Nombre:", 65);
            ConfigurarInputCRUD(lblDescripcion, txtDescripcion, "Descripción:", 120);
            ConfigurarInputCRUD(lblPrecio, txtPrecio, "Precio:", 175);

            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Height = 50;

            this.lblPrecio.Location = new Point(15, 185);
            this.txtPrecio.Location = new Point(15, 205);
            this.txtPrecio.Size = new Size(210, 23);

            this.lblStock.AutoSize = true;
            this.lblStock.ForeColor = Theme.DarkText;
            this.lblStock.Location = new Point(15, 240);
            this.lblStock.Text = "Stock:";

            this.numStock.Location = new Point(15, 260);
            this.numStock.Size = new Size(210, 23);
            this.numStock.BackColor = Theme.DarkDesktop;
            this.numStock.ForeColor = Color.White;
            this.numStock.Minimum = 0;
            this.numStock.Maximum = 100000;
            this.numStock.KeyPress += NumStock_KeyPress;

            this.lblDescuento.AutoSize = true;
            this.lblDescuento.ForeColor = Theme.DarkText;
            this.lblDescuento.Location = new Point(15, 295);
            this.lblDescuento.Text = "Descuento % (opcional):";

            this.numDescuento.Location = new Point(15, 315);
            this.numDescuento.Size = new Size(210, 23);
            this.numDescuento.BackColor = Theme.DarkDesktop;
            this.numDescuento.ForeColor = Color.White;
            this.numDescuento.DecimalPlaces = 2;
            this.numDescuento.Minimum = 0;
            this.numDescuento.Maximum = 100;

            ConfigurarBotonCRUD(btnGuardar, "Guardar", 350, Theme.AccentColor);
            this.btnGuardar.Click += BtnGuardar_Click;

            ConfigurarBotonCRUD(btnEditar, "Editar", 390, Color.FromArgb(200, 150, 0));
            this.btnEditar.Click += BtnEditar_Click;

            ConfigurarBotonCRUD(btnEliminar, "Eliminar", 430, Color.FromArgb(200, 50, 50));
            this.btnEliminar.Click += BtnEliminar_Click;

            this.btnLimpiar.Location = new Point(15, 470);
            this.btnLimpiar.Size = new Size(210, 30);
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.BackColor = Theme.DarkDesktop;
            this.btnLimpiar.ForeColor = Color.White;
            this.btnLimpiar.FlatStyle = FlatStyle.Flat;
            this.btnLimpiar.FlatAppearance.BorderSize = 0;
            this.btnLimpiar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnLimpiar.Cursor = Cursors.Hand;
            this.btnLimpiar.Click += BtnLimpiar_Click;

            this.btnExportar.Location = new Point(580, 20);
            this.btnExportar.Size = new Size(200, 35);
            this.btnExportar.Text = "Exportar a JSON";
            this.btnExportar.BackColor = Theme.AccentColor;
            this.btnExportar.ForeColor = Color.White;
            this.btnExportar.FlatStyle = FlatStyle.Flat;
            this.btnExportar.FlatAppearance.BorderSize = 0;
            this.btnExportar.Click += BtnExportar_Click;

            this.pnlAcciones.Controls.Add(this.lblCodigo);
            this.pnlAcciones.Controls.Add(this.txtCodigo);
            this.pnlAcciones.Controls.Add(this.lblNombre);
            this.pnlAcciones.Controls.Add(this.txtNombre);
            this.pnlAcciones.Controls.Add(this.lblDescripcion);
            this.pnlAcciones.Controls.Add(this.txtDescripcion);
            this.pnlAcciones.Controls.Add(this.lblPrecio);
            this.pnlAcciones.Controls.Add(this.txtPrecio);
            this.pnlAcciones.Controls.Add(this.lblStock);
            this.pnlAcciones.Controls.Add(this.numStock);
            this.pnlAcciones.Controls.Add(this.lblDescuento);
            this.pnlAcciones.Controls.Add(this.numDescuento);
            this.pnlAcciones.Controls.Add(this.btnGuardar);
            this.pnlAcciones.Controls.Add(this.btnEditar);
            this.pnlAcciones.Controls.Add(this.btnEliminar);
            this.pnlAcciones.Controls.Add(this.btnLimpiar);

            this.BackColor = Theme.DarkDesktop;
            this.ClientSize = new Size(800, 590);
            this.Controls.Add(this.pnlAcciones);
            this.Controls.Add(this.dgvProductos);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.btnExportar);
            this.Name = "FrmProductos";
            this.Text = "Productos";
            this.Load += FrmProductos_Load;

            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDescuento)).EndInit();
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

            txt.Location = new Point(15, y + 20);
            txt.Size = new Size(210, 23);
            txt.BackColor = Theme.DarkDesktop;
            txt.ForeColor = Color.White;
            txt.BorderStyle = BorderStyle.FixedSingle;
        }

        private void ConfigurarBotonCRUD(Button btn, string texto, int y, Color color)
        {
            btn.Location = new Point(15, y);
            btn.Size = new Size(210, 30);
            btn.Text = texto;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        private void FrmProductos_Load(object? sender, EventArgs e)
        {
            CargarGrilla();
            txtCodigo.ReadOnly = true;
            txtCodigo.Enabled = false;
        }

        private void CargarGrilla()
        {
            try
            {
                this.dgvProductos.DataSource = null;
                var service = new ProductoService();
                var productos = service.ObtenerProductos();
                this.dgvProductos.DataSource = productos;

                if (this.dgvProductos.Columns["Precio"] != null)
                {
                    this.dgvProductos.Columns["Precio"].DefaultCellStyle.Format = "C";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}");
            }
        }

        private void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            numStock.Value = 0;
            numDescuento.Value = 0;
            txtCodigo.Enabled = false;
        }

        private void BtnLimpiar_Click(object? sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void DgvProductos_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvProductos.Rows[e.RowIndex];
                txtCodigo.Text = row.Cells["Codigo"].Value?.ToString();
                txtCodigo.Enabled = false;
                txtNombre.Text = row.Cells["Nombre"].Value?.ToString();
                txtDescripcion.Text = row.Cells["Descripcion"]?.Value?.ToString();
                txtPrecio.Text = row.Cells["Precio"].Value?.ToString();

                if (int.TryParse(row.Cells["Stock"].Value?.ToString(), out int s))
                    numStock.Value = s;

                if (row.DataGridView.Columns.Contains("Descuento") && row.Cells["Descuento"].Value != null)
                {
                    if (decimal.TryParse(row.Cells["Descuento"].Value?.ToString(), out decimal d))
                        numDescuento.Value = d;
                }
                else
                {
                    numDescuento.Value = 0;
                }
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!ValidarCamposProducto()) return;

                if (!string.IsNullOrWhiteSpace(txtCodigo.Text))
                {
                    MessageBox.Show("Este producto ya existe. Use el botón EDITAR.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var service = new ProductoService();
                decimal precio = decimal.Parse(txtPrecio.Text);
                int stock = (int)numStock.Value;
                decimal descuento = numDescuento.Value;

                Producto p;
                if (descuento > 0)
                    p = new ProductoPromocion(0, txtNombre.Text, precio, stock, descuento, txtDescripcion.Text);
                else
                    p = new Producto(0, txtNombre.Text, precio, stock, txtDescripcion.Text);

                service.InsertarProducto(p);
                txtCodigo.Text = p.Codigo.ToString();
                CargarGrilla();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnEditar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!ValidarCamposProducto()) return;

                if (string.IsNullOrWhiteSpace(txtCodigo.Text))
                {
                    MessageBox.Show("Seleccione un producto para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal precio = decimal.Parse(txtPrecio.Text);
                int stock = (int)numStock.Value;
                decimal descuento = numDescuento.Value;
                int codigo = int.Parse(txtCodigo.Text);

                Producto p;
                if (descuento > 0)
                    p = new ProductoPromocion(codigo, txtNombre.Text, precio, stock, descuento, txtDescripcion.Text);
                else
                    p = new Producto(codigo, txtNombre.Text, precio, stock, txtDescripcion.Text);

                var service = new ProductoService();
                service.ActualizarProducto(p);
                CargarGrilla();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al editar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCodigo.Text))
                {
                    var res = MessageBox.Show("¿Seguro que desea eliminar este producto?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res == DialogResult.Yes)
                    {
                        var service = new ProductoService();
                        service.EliminarProducto(int.Parse(txtCodigo.Text));
                        CargarGrilla();
                        LimpiarCampos();
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un producto para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool ValidarCamposProducto()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus(); return false;
            }
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("La descripción no puede estar vacía.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescripcion.Focus(); return false;
            }
            if (txtNombre.Text.Trim().Length < 3)
            {
                MessageBox.Show("El nombre debe tener al menos 3 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus(); return false;
            }
            if (txtNombre.Text.Any(char.IsDigit))
            {
                MessageBox.Show("El nombre no debe contener números.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus(); return false;
            }
            if (numStock.Value < 0)
            {
                MessageBox.Show("El stock no puede ser negativo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numStock.Focus(); return false;
            }
            if (string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("El precio es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus(); return false;
            }
            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("El precio debe ser un número válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus(); return false;
            }
            if (precio <= 0)
            {
                MessageBox.Show("El precio debe ser mayor que 0.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus(); return false;
            }
            return true;
        }

        private void NumStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '-')
            {
                MessageBox.Show("No se permite stock negativo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Handled = true;
            }
        }

        private void BtnExportar_Click(object? sender, EventArgs e)
        {
            try
            {
                var service = new ProductoService();
                var productos = service.ObtenerProductos();
                var dtos = productos.Select(p => new ProductoDTO
                {
                    Codigo = p.Codigo,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Stock = p.Stock
                }).ToList();

                JsonHelper.ExportarProductos(dtos);
                MessageBox.Show("Productos exportados correctamente a su Escritorio.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error exportando los archivos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}