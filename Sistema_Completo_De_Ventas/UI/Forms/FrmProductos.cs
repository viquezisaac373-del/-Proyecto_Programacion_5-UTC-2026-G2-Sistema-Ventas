using System;
using System.Drawing;
using System.Windows.Forms;
using SistemaVentas.BLL;
using SistemaVentas.DTO;
using System.Linq;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    public class FrmProductos : Form
    {
        private DataGridView dgvProductos;
        private Label lblTitulo;
        private Panel pnlAcciones;
        private Label lblCodigo;
        private TextBox txtCodigo;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblPrecio;
        private TextBox txtPrecio;
        private Label lblStock;
        private NumericUpDown numStock;
        private Label lblDescuento;
        private NumericUpDown numDescuento;
        private Button btnGuardar;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private Button btnExportar;

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

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Theme.DarkText;
            this.lblTitulo.Location = new Point(20, 20);
            this.lblTitulo.Text = "Gestión de Productos";

            // dgvProductos
            this.dgvProductos.Location = new Point(25, 70);
            this.dgvProductos.Size = new Size(500, 400);
            this.dgvProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Theme.ApplyDarkDataGridView(this.dgvProductos);
            this.dgvProductos.CellDoubleClick += DgvProductos_CellDoubleClick;

            // pnlAcciones
            this.pnlAcciones.BackColor = Theme.DarkControl;
            this.pnlAcciones.Location = new Point(540, 70);
            this.pnlAcciones.Size = new Size(240, 400);
            this.pnlAcciones.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            this.pnlAcciones.Padding = new Padding(15);

            ConfigurarInputCRUD(lblCodigo, txtCodigo, "Código Producto:", 10);
            ConfigurarInputCRUD(lblNombre, txtNombre, "Nombre:", 65);
            ConfigurarInputCRUD(lblPrecio, txtPrecio, "Precio:", 120);

            // lblStock y numStock
            lblStock.AutoSize = true;
            lblStock.ForeColor = Theme.DarkText;
            lblStock.Location = new Point(15, 175);
            lblStock.Text = "Stock:";

            numStock.Location = new Point(15, 195);
            numStock.Size = new Size(210, 23);
            numStock.BackColor = Theme.DarkDesktop;
            numStock.ForeColor = Color.White;

            // lblDescuento y numDescuento
            lblDescuento.AutoSize = true;
            lblDescuento.ForeColor = Theme.DarkText;
            lblDescuento.Location = new Point(15, 230);
            lblDescuento.Text = "Descuento % (opcional):";

            numDescuento.Location = new Point(15, 250);
            numDescuento.Size = new Size(210, 23);
            numDescuento.BackColor = Theme.DarkDesktop;
            numDescuento.ForeColor = Color.White;
            numDescuento.DecimalPlaces = 2;

            // Botones
            ConfigurarBotonCRUD(btnGuardar, "Guardar", 285, Theme.AccentColor);
            this.btnGuardar.Click += BtnGuardar_Click;

            ConfigurarBotonCRUD(btnEditar, "Editar", 325, Color.FromArgb(200, 150, 0));
            this.btnEditar.Click += BtnEditar_Click;

            ConfigurarBotonCRUD(btnEliminar, "Eliminar", 365, Color.FromArgb(200, 50, 50));
            this.btnEliminar.Click += BtnEliminar_Click;

           
            this.pnlAcciones.Size = new Size(240, 440);
            
            // btnLimpiar
            this.btnLimpiar.Location = new Point(15, 405);
            this.btnLimpiar.Size = new Size(210, 30);
            this.btnLimpiar.Click += BtnLimpiar_Click;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.BackColor = Theme.DarkDesktop;
            this.btnLimpiar.ForeColor = Color.White;
            this.btnLimpiar.FlatStyle = FlatStyle.Flat;
            this.btnLimpiar.Click += BtnLimpiar_Click;

            // btnExportar
            this.btnExportar.Location = new Point(406, 12);
            this.btnExportar.Size = new Size(200, 35);
            this.btnExportar.Text = "Exportar a JSON";
            this.btnExportar.BackColor = Theme.AccentColor;
            this.btnExportar.ForeColor = Color.White;
            this.btnExportar.FlatStyle = FlatStyle.Flat;
            this.btnExportar.Click += BtnExportar_Click;

            this.pnlAcciones.Controls.Add(lblCodigo);
            this.pnlAcciones.Controls.Add(txtCodigo);
            this.pnlAcciones.Controls.Add(lblNombre);
            this.pnlAcciones.Controls.Add(txtNombre);
            this.pnlAcciones.Controls.Add(lblPrecio);
            this.pnlAcciones.Controls.Add(txtPrecio);
            this.pnlAcciones.Controls.Add(lblStock);
            this.pnlAcciones.Controls.Add(numStock);
            this.pnlAcciones.Controls.Add(lblDescuento);
            this.pnlAcciones.Controls.Add(numDescuento);
            this.pnlAcciones.Controls.Add(btnGuardar);
            this.pnlAcciones.Controls.Add(btnEditar);
            this.pnlAcciones.Controls.Add(btnEliminar);
            this.pnlAcciones.Controls.Add(btnLimpiar);

            // FrmProductos
            this.BackColor = Theme.DarkDesktop;
            this.ClientSize = new Size(800, 500);
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
        }

        private void CargarGrilla()
        {
            try
            {
                this.dgvProductos.DataSource = null;
                var service = new ProductoService();
                var productos = service.ObtenerProductos();
                this.dgvProductos.DataSource = productos;
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
            txtPrecio.Clear();
            numStock.Value = 0;
            numDescuento.Value = 0;
            txtCodigo.Enabled = true;
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
                decimal precio = decimal.Parse(txtPrecio.Text);
                int stock = (int)numStock.Value;
                decimal descuento = numDescuento.Value;

                Producto p;
                if (descuento > 0)
                    p = new ProductoPromocion(txtCodigo.Text, txtNombre.Text, precio, stock, descuento);
                else
                    p = new Producto(txtCodigo.Text, txtNombre.Text, precio, stock);

                var service = new ProductoService();
                service.InsertarProducto(p);
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
                decimal precio = decimal.Parse(txtPrecio.Text);
                int stock = (int)numStock.Value;
                decimal descuento = numDescuento.Value;

                Producto p;
                if (descuento > 0)
                    p = new ProductoPromocion(txtCodigo.Text, txtNombre.Text, precio, stock, descuento);
                else
                    p = new Producto(txtCodigo.Text, txtNombre.Text, precio, stock);

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
                        service.EliminarProducto(txtCodigo.Text);
                        CargarGrilla();
                        LimpiarCampos();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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