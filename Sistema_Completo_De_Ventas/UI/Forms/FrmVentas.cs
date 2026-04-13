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
        private Label lblSubtotal;
        private Label lblIVA;
        private Button btnProcesarVenta;
        private ComboBox cmbClientes;
        private Button btnEliminar;
        private Label lblCliente;

        // Controles para Productos
        private Label lblProducto;
        private ComboBox cmbProductos;
        private Label lblCantidad;
        private NumericUpDown numCantidad;
        private Button btnAgregar;

        // Lista de carrito
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
            this.lblSubtotal = new Label();
            this.lblIVA = new Label();
            this.btnProcesarVenta = new Button();
            this.cmbClientes = new ComboBox();
            this.lblCliente = new Label();
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
            this.cmbClientes.Size = new Size(250, 23);
            this.cmbClientes.BackColor = Theme.DarkControl;
            this.cmbClientes.ForeColor = Theme.DarkText;
            this.cmbClientes.FlatStyle = FlatStyle.Flat;

            // lblProducto
            this.lblProducto.AutoSize = true;
            this.lblProducto.Font = new Font("Segoe UI", 10F);
            this.lblProducto.ForeColor = Theme.DarkText;
            this.lblProducto.Location = new Point(340, 55);
            this.lblProducto.Text = "Producto:";

            // cmbProductos
            this.cmbProductos.FormattingEnabled = true;
            this.cmbProductos.Location = new Point(420, 52);
            this.cmbProductos.Size = new Size(200, 23);
            this.cmbProductos.BackColor = Theme.DarkControl;
            this.cmbProductos.ForeColor = Theme.DarkText;
            this.cmbProductos.FlatStyle = FlatStyle.Flat;

            // lblCantidad
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Font = new Font("Segoe UI", 10F);
            this.lblCantidad.ForeColor = Theme.DarkText;
            this.lblCantidad.Location = new Point(620, 55);
            this.lblCantidad.Text = "Cant:";

            // numCantidad
            this.numCantidad.Location = new Point(670, 53);
            this.numCantidad.Size = new Size(60, 23);
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

            // lblSubtotal
            this.lblSubtotal.AutoSize = true;
            this.lblSubtotal.Font = new Font("Segoe UI", 12F);
            this.lblSubtotal.Location = new Point(20, 460);
            this.lblSubtotal.ForeColor = Color.White;
            this.lblSubtotal.Text = "Subtotal: $0.00";

            // lblIVA
            this.lblIVA.AutoSize = true;
            this.lblIVA.Font = new Font("Segoe UI", 12F);
            this.lblIVA.Location = new Point(20, 490);
            this.lblIVA.ForeColor = Color.White;
            this.lblIVA.Text = "IVA (13%): $0.00";

            // lblTotal
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTotal.ForeColor = Theme.AccentColor;
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
            this.Controls.Add(this.btnProcesarVenta);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblSubtotal);
            this.Controls.Add(this.lblIVA);
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
                // Cargar Clientes
                var clienteService = new ClienteService();
                var clientes = clienteService.ObtenerClientes();
                cmbClientes.DataSource = clientes;
                cmbClientes.DisplayMember = "Nombre";
                cmbClientes.ValueMember = "Id";

                // Cargar Productos
                var productos = ProductoDAO.ObtenerProductos();
                cmbProductos.DataSource = null;
                cmbProductos.DataSource = ProductoDAO.ObtenerProductos();
                cmbProductos.DataSource = productos;
                cmbProductos.DisplayMember = "Nombre";
                cmbProductos.ValueMember = "Codigo";

                // Bind DataGrid
                dgvCarrito.DataSource = carritoVentas;

                // 🔥 AJUSTES VISUALES
                dgvCarrito.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvCarrito.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvCarrito.MultiSelect = false;

                // 🔥 FORMATO SEGURO
                FormatearGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar combos: " + ex.Message);
            }
        }

        private void FormatearGrid()
        {
            if (dgvCarrito.Columns["Precio"] != null)
            {
                dgvCarrito.Columns["Precio"].DefaultCellStyle.Format = "N2";
                dgvCarrito.Columns["Subtotal"].DefaultCellStyle.Format = "N2";
                dgvCarrito.Columns["IVA"].DefaultCellStyle.Format = "N2";
                dgvCarrito.Columns["Total"].DefaultCellStyle.Format = "N2";
            }
        }

        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            if (cmbProductos.SelectedItem is Sistema_Completo_De_Ventas.Producto p)
            {
                int cant = (int)numCantidad.Value;

                // 🔥 VALIDAR STOCK
                if (cant > p.Stock)
                {
                    MessageBox.Show("No hay suficiente stock disponible.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var itemExistente = carritoVentas.FirstOrDefault(x => x.CodigoP == p.Codigo);

                if (itemExistente != null)
                {
                    int nuevaCantidad = itemExistente.Cantidad + cant;

                    // 🔥 VALIDAR STOCK TOTAL
                    if (nuevaCantidad > p.Stock)
                    {
                        MessageBox.Show("Stock insuficiente para esa cantidad.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    decimal precioFinal = p.CalcularPrecioVenta();

                    itemExistente.Cantidad = nuevaCantidad;
                    itemExistente.Precio = precioFinal;
                    itemExistente.Subtotal = itemExistente.Cantidad * precioFinal;
                }
                else
                {
                    decimal precioFinal = p.CalcularPrecioVenta();
                    carritoVentas.Add(new ItemCarrito
                    {
                        CodigoP = p.Codigo,
                        Producto = p.Nombre,
                        Cantidad = cant,
                        Precio = precioFinal,
                        Subtotal = precioFinal * cant
                    });
                }

                dgvCarrito.Refresh();
                CalcularTotal();
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            // Validar selección
            if (dgvCarrito.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una fila para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirmación
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

            lblSubtotal.Text = $"Subtotal: ${subtotal:F2}";
            lblIVA.Text = $"IVA (13%): ${iva:F2}";
            lblTotal.Text = $"Total: ${total:F2}";
        }

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
                    PrecioUnitario = x.Precio
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

        class ItemCarrito
        {
            public int CodigoP { get; set; }
            public string Producto { get; set; } = "";
            public int Cantidad { get; set; }
            public decimal Precio { get; set; }
            public decimal Subtotal { get; set; }
            public decimal IVA => Subtotal * 0.13m;
            public decimal Total => Subtotal + IVA;
        }
    }
} 
