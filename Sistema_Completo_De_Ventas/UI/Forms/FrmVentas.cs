using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SistemaVentas.BLL;
using SistemaVentas.DAL;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    // Clase del formulario de ventas que hereda de Form
    public class FrmVentas : Form
    {
        // Agregamos = null!; para quitar los errores de tu lista
        private DataGridView dgvCarrito = null!;
        private Label lblTitulo = null!;
        private Label lblTotal = null!;
        private Button btnProcesarVenta = null!;
        private ComboBox cmbClientes = null!;
        private Button btnEliminar = null!;
        private Label lblCliente = null!;
        private Label lblIdCliente = null!;
        private TextBox txtIdCliente = null!;
        private Label lblProducto = null!;
        private ComboBox cmbProductos = null!;
        private Label lblCantidad = null!;
        private NumericUpDown numCantidad = null!;
        private Button btnAgregar = null!;

        // Lista enlazada (BindingList) para almacenar los productos del carrito
        private BindingList<ItemCarrito> carritoVentas = new BindingList<ItemCarrito>();


        // Constructor del formulario
        public FrmVentas()
        {
            InitializeComponent(); // Inicializa los componentes
        }

        // Método donde se configuran todos los controles del formulario
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

            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Theme.DarkText;
            this.lblTitulo.Location = new Point(20, 10);
            this.lblTitulo.Text = "Facturación";

            this.lblCliente.AutoSize = true;
            this.lblCliente.Font = new Font("Segoe UI", 10F);
            this.lblCliente.ForeColor = Theme.DarkText;
            this.lblCliente.Location = new Point(21, 55);
            this.lblCliente.Text = "Cliente:";

            this.cmbClientes.FormattingEnabled = true;
            this.cmbClientes.Location = new Point(85, 52);
            this.cmbClientes.Size = new Size(180, 23);
            this.cmbClientes.BackColor = Theme.DarkControl;
            this.cmbClientes.ForeColor = Theme.DarkText;
            this.cmbClientes.FlatStyle = FlatStyle.Flat;
            this.cmbClientes.SelectedIndexChanged += CmbClientes_SelectedIndexChanged;

            this.lblIdCliente.AutoSize = true;
            this.lblIdCliente.Font = new Font("Segoe UI", 10F);
            this.lblIdCliente.ForeColor = Theme.DarkText;
            this.lblIdCliente.Location = new Point(275, 55);
            this.lblIdCliente.Text = "ID:";

            this.txtIdCliente.Location = new Point(305, 52);
            this.txtIdCliente.Size = new Size(60, 23);
            this.txtIdCliente.BackColor = Theme.DarkControl;
            this.txtIdCliente.ForeColor = Theme.DarkText;
            this.txtIdCliente.BorderStyle = BorderStyle.FixedSingle;
            this.txtIdCliente.ReadOnly = true;
            this.txtIdCliente.TabStop = false;

            this.lblProducto.AutoSize = true;
            this.lblProducto.Font = new Font("Segoe UI", 10F);
            this.lblProducto.ForeColor = Theme.DarkText;
            this.lblProducto.Location = new Point(370, 55);
            this.lblProducto.Text = "Producto:";

            this.cmbProductos.FormattingEnabled = true;
            this.cmbProductos.Location = new Point(450, 52);
            this.cmbProductos.Size = new Size(170, 23);
            this.cmbProductos.BackColor = Theme.DarkControl;
            this.cmbProductos.ForeColor = Theme.DarkText;
            this.cmbProductos.FlatStyle = FlatStyle.Flat;

            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Font = new Font("Segoe UI", 10F);
            this.lblCantidad.ForeColor = Theme.DarkText;
            this.lblCantidad.Location = new Point(625, 55);
            this.lblCantidad.Text = "Cant:";

            this.numCantidad.Location = new Point(675, 53);
            this.numCantidad.Size = new Size(55, 23);
            this.numCantidad.Minimum = 1;
            this.numCantidad.Value = 1;
            this.numCantidad.BackColor = Theme.DarkControl;
            this.numCantidad.ForeColor = Theme.DarkText;

            this.btnAgregar.BackColor = Color.DarkGreen;
            this.btnAgregar.FlatStyle = FlatStyle.Flat;
            this.btnAgregar.ForeColor = Color.White;
            this.btnAgregar.Location = new Point(630, 5);
            this.btnAgregar.Size = new Size(150, 35);
            this.btnAgregar.Text = "Agregar Venta";
            this.btnAgregar.Click += BtnAgregar_Click;

            this.btnEliminar.BackColor = Color.DarkRed;
            this.btnEliminar.FlatStyle = FlatStyle.Flat;
            this.btnEliminar.ForeColor = Color.White;
            this.btnEliminar.Location = new Point(470, 5);
            this.btnEliminar.Size = new Size(150, 35);
            this.btnEliminar.Text = "Eliminar Venta";
            this.btnEliminar.Click += BtnEliminar_Click;

            this.dgvCarrito.Location = new Point(25, 90);
            this.dgvCarrito.Size = new Size(750, 310);
            this.dgvCarrito.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Theme.ApplyDarkDataGridView(this.dgvCarrito);

            // CORRECCIÓN MONEDA
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTotal.ForeColor = Color.White;
            this.lblTotal.Location = new Point(20, 415);
            this.lblTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lblTotal.Text = "Total: ₡0,00";

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

        // Evento al cargar el formulario
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

        // Evento al cambiar cliente
        private void CmbClientes_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbClientes.SelectedValue != null)
            {
                txtIdCliente.Text = cmbClientes.SelectedValue.ToString();
            }
        }

        // CORRECCIÓN FORMATO MONEDA EN GRILLA
        private void FormatearGrid()
        {
            if (dgvCarrito.Columns["Fecha"] != null)
            {
                dgvCarrito.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }

            if (dgvCarrito.Columns["Precio"] != null) dgvCarrito.Columns["Precio"].DefaultCellStyle.Format = "C";
            if (dgvCarrito.Columns["Descuento"] != null) dgvCarrito.Columns["Descuento"].DefaultCellStyle.Format = "N2";
            if (dgvCarrito.Columns["Subtotal"] != null) dgvCarrito.Columns["Subtotal"].DefaultCellStyle.Format = "C";
            if (dgvCarrito.Columns["IVA"] != null) dgvCarrito.Columns["IVA"].DefaultCellStyle.Format = "C";
            if (dgvCarrito.Columns["Total"] != null) dgvCarrito.Columns["Total"].DefaultCellStyle.Format = "C";
        }

        // Agregar producto al carrito
        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            if (cmbProductos.SelectedItem is Sistema_Completo_De_Ventas.Producto p)
            {
                int cant = (int)numCantidad.Value;

                if (cant > p.Stock)
                {
                    MessageBox.Show("No hay suficiente stock disponible.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var itemExistente = carritoVentas.FirstOrDefault(x => x.CodigoP == p.Codigo);

                if (itemExistente != null)
                {
                    int nuevaCantidad = itemExistente.Cantidad + cant;

                    if (nuevaCantidad > p.Stock)
                    {
                        MessageBox.Show("Stock insuficiente para esa cantidad.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    itemExistente.Cantidad = nuevaCantidad;
                    itemExistente.Fecha = DateTime.Now;
                    itemExistente.Producto = p.Nombre;
                    itemExistente.Descripcion = p.Descripcion;
                    itemExistente.Descuento = p.Descuento;
                    itemExistente.Precio = p.Precio;
                }
                else
                {
                    carritoVentas.Add(new ItemCarrito
                    {
                        CodigoP = p.Codigo,
                        Fecha = DateTime.Now,
                        Producto = p.Nombre,
                        Descripcion = p.Descripcion,
                        Descuento = p.Descuento,
                        Cantidad = cant,
                        Precio = p.Precio
                    });
                }

                dgvCarrito.Refresh();
                FormatearGrid();
                CalcularTotal();
            }
        }

        // Eliminar producto del carrito
        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (dgvCarrito.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una fila para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("¿Estás seguro de eliminar esta venta?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var item = (ItemCarrito)dgvCarrito.CurrentRow.DataBoundItem;
                carritoVentas.Remove(item);
                CalcularTotal();
            }
        }

        // CORRECCIÓN TOTAL MONEDA
        private void CalcularTotal()
        {
            decimal subtotal = carritoVentas.Sum(x => x.Subtotal);
            decimal iva = carritoVentas.Sum(x => x.IVA);
            decimal total = carritoVentas.Sum(x => x.Total);

            lblTotal.Text = $"Total: {total:C}";
        }


        // Procesar la venta final
        private void BtnProcesarVenta_Click(object? sender, EventArgs e)
        {
            if (carritoVentas.Count == 0 || cmbClientes.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un cliente y agregue productos al carrito.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                MessageBox.Show($"Venta registrada con éxito. (ID: {nuevaVentaId})", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                carritoVentas.Clear();
                CalcularTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar la venta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Clase interna que representa un producto en el carrito
        class ItemCarrito
        {
            public int CodigoP { get; set; }
            public DateTime Fecha { get; set; }
            public string Producto { get; set; } = "";
            public string Descripcion { get; set; } = "";
            public decimal Descuento { get; set; }
            public int Cantidad { get; set; }
            public decimal Precio { get; set; }

            // Precio con descuento aplicado
            public decimal PrecioFinal => Precio - (Precio * Descuento / 100);

            // Subtotal por línea
            public decimal Subtotal => Cantidad * PrecioFinal;

            // IVA por línea
            public decimal IVA => Subtotal * 0.13m;

            // Total por línea
            public decimal Total => Subtotal + IVA;
        }
    }
}