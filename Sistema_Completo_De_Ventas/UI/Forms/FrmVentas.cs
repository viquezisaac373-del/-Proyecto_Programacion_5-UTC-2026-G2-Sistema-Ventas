using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SistemaVentas.BLL;
using SistemaVentas.DAL;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    public class FrmVentas : Form
    {
        private DataGridView dgvCarrito;
        private Label lblTitulo;
        private Label lblTotal;
        private Button btnProcesarVenta;
        private ComboBox cmbClientes;
        private Button btnEliminar;
        private Label lblCliente;
        private Label lblIdCliente;
        private TextBox txtIdCliente;

        private Label lblProducto;
        private ComboBox cmbProductos;
        private Label lblCantidad;
        private NumericUpDown numCantidad;
        private Button btnAgregar;

        private BindingList<ItemCarrito> carritoVentas = new BindingList<ItemCarrito>();

        public FrmVentas()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.dgvCarrito = new DataGridView();
            this.lblTitulo = new Label();
            this.lblTotal = new Label();
            this.btnProcesarVenta = new Button();
            this.cmbClientes = new ComboBox();
            this.lblCliente = new Label();
            this.lblIdCliente = new Label();
            this.txtIdCliente = new TextBox();
            this.lblProducto = new Label();
            this.cmbProductos = new ComboBox();
            this.lblCantidad = new Label();
            this.numCantidad = new NumericUpDown();
            this.btnAgregar = new Button();
            this.btnEliminar = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCantidad)).BeginInit();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Theme.DarkText;
            this.lblTitulo.Location = new Point(20, 10);
            this.lblTitulo.Text = "Facturación";

            // lblCliente
            this.lblCliente.AutoSize = true;
            this.lblCliente.Font = new Font("Segoe UI", 10F);
            this.lblCliente.ForeColor = Theme.DarkText;
            this.lblCliente.Location = new Point(21, 55);
            this.lblCliente.Text = "Cliente:";

            // cmbClientes
            this.cmbClientes.FormattingEnabled = true;
            this.cmbClientes.Location = new Point(85, 52);
            this.cmbClientes.Size = new Size(180, 23);
            this.cmbClientes.BackColor = Theme.DarkControl;
            this.cmbClientes.ForeColor = Theme.DarkText;
            this.cmbClientes.FlatStyle = FlatStyle.Flat;
            this.cmbClientes.SelectedIndexChanged += CmbClientes_SelectedIndexChanged;

            // lblIdCliente
            this.lblIdCliente.AutoSize = true;
            this.lblIdCliente.Font = new Font("Segoe UI", 10F);
            this.lblIdCliente.ForeColor = Theme.DarkText;
            this.lblIdCliente.Location = new Point(275, 55);
            this.lblIdCliente.Text = "ID:";

            // txtIdCliente
            this.txtIdCliente.Location = new Point(305, 52);
            this.txtIdCliente.Size = new Size(60, 23);
            this.txtIdCliente.BackColor = Theme.DarkControl;
            this.txtIdCliente.ForeColor = Theme.DarkText;
            this.txtIdCliente.BorderStyle = BorderStyle.FixedSingle;
            this.txtIdCliente.ReadOnly = true;
            this.txtIdCliente.TabStop = false;

            // lblProducto
            this.lblProducto.AutoSize = true;
            this.lblProducto.Font = new Font("Segoe UI", 10F);
            this.lblProducto.ForeColor = Theme.DarkText;
            this.lblProducto.Location = new Point(370, 55);
            this.lblProducto.Text = "Producto:";

            // cmbProductos
            this.cmbProductos.FormattingEnabled = true;
            this.cmbProductos.Location = new Point(450, 52);
            this.cmbProductos.Size = new Size(170, 23);
            this.cmbProductos.BackColor = Theme.DarkControl;
            this.cmbProductos.ForeColor = Theme.DarkText;
            this.cmbProductos.FlatStyle = FlatStyle.Flat;

            // lblCantidad
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Font = new Font("Segoe UI", 10F);
            this.lblCantidad.ForeColor = Theme.DarkText;
            this.lblCantidad.Location = new Point(625, 55);
            this.lblCantidad.Text = "Cant:";

            // numCantidad
            this.numCantidad.Location = new Point(675, 53);
            this.numCantidad.Size = new Size(55, 23);
            this.numCantidad.Minimum = 1;
            this.numCantidad.Value = 1;
            this.numCantidad.BackColor = Theme.DarkControl;
            this.numCantidad.ForeColor = Theme.DarkText;

            // btnAgregar
            this.btnAgregar.BackColor = Color.DarkGreen;
            this.btnAgregar.FlatStyle = FlatStyle.Flat;
            this.btnAgregar.ForeColor = Color.White;
            this.btnAgregar.Location = new Point(630, 5);
            this.btnAgregar.Size = new Size(150, 35);
            this.btnAgregar.Text = "Agregar Venta";
            this.btnAgregar.Click += BtnAgregar_Click;

            // btnEliminar
            this.btnEliminar.BackColor = Color.DarkRed;
            this.btnEliminar.FlatStyle = FlatStyle.Flat;
            this.btnEliminar.ForeColor = Color.White;
            this.btnEliminar.Location = new Point(470, 5);
            this.btnEliminar.Size = new Size(150, 35);
            this.btnEliminar.Text = "Eliminar Venta";
            this.btnEliminar.Click += BtnEliminar_Click;

            // dgvCarrito
            this.dgvCarrito.Location = new Point(25, 90);
            this.dgvCarrito.Size = new Size(750, 310);
            this.dgvCarrito.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Theme.ApplyDarkDataGridView(this.dgvCarrito);

            // lblTotal
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTotal.ForeColor = Color.White;
            this.lblTotal.Location = new Point(20, 415);
            this.lblTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lblTotal.Text = "Total: $0.00";

            // btnProcesarVenta
            this.btnProcesarVenta.BackColor = Theme.AccentColor;
            this.btnProcesarVenta.FlatStyle = FlatStyle.Flat;
            this.btnProcesarVenta.FlatAppearance.BorderSize = 0;
            this.btnProcesarVenta.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnProcesarVenta.ForeColor = Color.White;
            this.btnProcesarVenta.Location = new Point(625, 415);
            this.btnProcesarVenta.Size = new Size(150, 40);
            this.btnProcesarVenta.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnProcesarVenta.Text = "Procesar Venta";
            this.btnProcesarVenta.Cursor = Cursors.Hand;
            this.btnProcesarVenta.Click += BtnProcesarVenta_Click;

            // FrmVentas
            this.BackColor = Theme.DarkDesktop;
            this.ClientSize = new Size(800, 500);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.numCantidad);
            this.Controls.Add(this.lblCantidad);
            this.Controls.Add(this.cmbProductos);
            this.Controls.Add(this.lblProducto);
            this.Controls.Add(this.txtIdCliente);
            this.Controls.Add(this.lblIdCliente);
            this.Controls.Add(this.btnProcesarVenta);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.dgvCarrito);
            this.Controls.Add(this.cmbClientes);
            this.Controls.Add(this.lblCliente);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FrmVentas";
            this.Text = "Ventas";
            this.Load += FrmVentas_Load;

            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCantidad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void FrmVentas_Load(object? sender, EventArgs e)
        {
            try
            {
                var clienteService = new ClienteService();
                var clientes = clienteService.ObtenerClientes();
                cmbClientes.DataSource = clientes;
                cmbClientes.DisplayMember = "Nombre";
                cmbClientes.ValueMember = "Id";

                if (cmbClientes.SelectedValue != null)
                    txtIdCliente.Text = cmbClientes.SelectedValue.ToString();

                var productos = ProductoDAO.ObtenerProductos();
                cmbProductos.DataSource = null;
                cmbProductos.DataSource = productos;
                cmbProductos.DisplayMember = "Nombre";
                cmbProductos.ValueMember = "Codigo";

                dgvCarrito.DataSource = carritoVentas;
                dgvCarrito.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvCarrito.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvCarrito.MultiSelect = false;

                FormatearGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar combos: " + ex.Message);
            }
        }

        private void CmbClientes_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbClientes.SelectedValue != null)
            {
                txtIdCliente.Text = cmbClientes.SelectedValue.ToString();
            }
        }

        private void FormatearGrid()
        {
            if (dgvCarrito.Columns["Fecha"] != null)
            {
                dgvCarrito.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }

            if (dgvCarrito.Columns["Precio"] != null)
                dgvCarrito.Columns["Precio"].DefaultCellStyle.Format = "N2";

            if (dgvCarrito.Columns["Descuento"] != null)
                dgvCarrito.Columns["Descuento"].DefaultCellStyle.Format = "N2";

            if (dgvCarrito.Columns["Subtotal"] != null)
                dgvCarrito.Columns["Subtotal"].DefaultCellStyle.Format = "N2";

            if (dgvCarrito.Columns["IVA"] != null)
                dgvCarrito.Columns["IVA"].DefaultCellStyle.Format = "N2";

            if (dgvCarrito.Columns["Total"] != null)
                dgvCarrito.Columns["Total"].DefaultCellStyle.Format = "N2";
        }

        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            if (cmbProductos.SelectedItem is Sistema_Completo_De_Ventas.Producto p)
            {
                int cant = (int)numCantidad.Value;

                if (cant > p.Stock)
                {
                    MessageBox.Show("No hay suficiente stock disponible.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var itemExistente = carritoVentas.FirstOrDefault(x => x.CodigoP == p.Codigo);

                decimal precioOriginal = p.Precio;
                decimal descuento = p.Descuento;
                decimal precioFinal = precioOriginal - (precioOriginal * descuento / 100);

                if (itemExistente != null)
                {
                    int nuevaCantidad = itemExistente.Cantidad + cant;

                    if (nuevaCantidad > p.Stock)
                    {
                        MessageBox.Show("Stock insuficiente para esa cantidad.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    itemExistente.Cantidad = nuevaCantidad;
                    itemExistente.Fecha = DateTime.Now;
                    itemExistente.Producto = p.Nombre;
                    itemExistente.Descripcion = p.Descripcion;
                    itemExistente.Descuento = descuento;
                    itemExistente.Precio = precioOriginal;
                    itemExistente.Subtotal = nuevaCantidad * precioFinal;
                }
                else
                {
                    carritoVentas.Add(new ItemCarrito
                    {
                        CodigoP = p.Codigo,
                        Fecha = DateTime.Now,
                        Producto = p.Nombre,
                        Descripcion = p.Descripcion,
                        Descuento = descuento,
                        Cantidad = cant,
                        Precio = precioOriginal,
                        Subtotal = cant * precioFinal
                    });
                }

                dgvCarrito.Refresh();
                FormatearGrid();
                CalcularTotal();
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (dgvCarrito.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una fila para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                "¿Estás seguro de eliminar esta venta?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                var item = (ItemCarrito)dgvCarrito.CurrentRow.DataBoundItem;
                carritoVentas.Remove(item);
                CalcularTotal();
            }
        }

        private void CalcularTotal()
        {
            decimal subtotal = carritoVentas.Sum(x => x.Subtotal);
            decimal iva = subtotal * 0.13m;
            decimal total = subtotal + iva;
            lblTotal.Text = $"Total: ${total:F2}";
        }

        private void BtnProcesarVenta_Click(object? sender, EventArgs e)
        {
            if (carritoVentas.Count == 0 || cmbClientes.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un cliente y agregue productos al carrito.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var venta = new SistemaVentas.DTO.VentaDTO
                {
                    ClienteId = (int)cmbClientes.SelectedValue,
                    Fecha = DateTime.Now
                };

                var detalles = carritoVentas.Select(x => new SistemaVentas.DTO.VentaDetalleDTO
                {
                    CodigoP = x.CodigoP,
                    Cantidad = x.Cantidad,
                    PrecioUnitario = x.Precio - (x.Precio * x.Descuento / 100)
                }).ToList();

                var service = new VentaService();
                int nuevaVentaId = service.RegistrarVenta(venta, detalles);

                MessageBox.Show($"Venta registrada con éxito. (ID: {nuevaVentaId})",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                carritoVentas.Clear();
                CalcularTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar la venta: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        class ItemCarrito
        {
            public int CodigoP { get; set; }
            public DateTime Fecha { get; set; }
            public string Producto { get; set; } = "";
            public string Descripcion { get; set; } = "";
            public decimal Descuento { get; set; }
            public int Cantidad { get; set; }
            public decimal Precio { get; set; }
            public decimal Subtotal { get; set; }
            public decimal IVA => Subtotal * 0.13m;
            public decimal Total => Subtotal + IVA;
        }
    }
}