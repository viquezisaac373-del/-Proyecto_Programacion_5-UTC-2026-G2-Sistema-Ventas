using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using SistemaVentas.BLL;
using SistemaVentas.DAL;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    public class FrmReportes : Form
    {
        private DataGridView dgvReportes;
        private Label lblTitulo;
        private ComboBox cmbTipoReporte;
        private Label lblFiltro;
        private Button btnGenerar;
        private Button btnExportarJson;
        private Label lblUmbral;
        private NumericUpDown numUmbral;
        private Label lblClientesFactura;
        private ComboBox cmbClientesFactura;
        private Label lblIdClienteSeleccionado;
        private TextBox txtIdClienteSeleccionado;
        private Label lblFechaCompra;
        private DateTimePicker dtpFechaCompra;

        public FrmReportes()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.dgvReportes = new DataGridView();
            this.lblTitulo = new Label();
            this.cmbTipoReporte = new ComboBox();
            this.lblFiltro = new Label();
            this.btnGenerar = new Button();
            this.btnExportarJson = new Button();
            this.lblUmbral = new Label();
            this.numUmbral = new NumericUpDown();
            this.lblClientesFactura = new Label();
            this.cmbClientesFactura = new ComboBox();
            this.lblIdClienteSeleccionado = new Label();
            this.txtIdClienteSeleccionado = new TextBox();
            this.lblFechaCompra = new Label();
            this.dtpFechaCompra = new DateTimePicker();

            ((System.ComponentModel.ISupportInitialize)(this.dgvReportes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUmbral)).BeginInit();
            this.SuspendLayout();

            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Theme.DarkText;
            this.lblTitulo.Location = new Point(20, 20);
            this.lblTitulo.Text = "Reportes del Sistema";

            this.lblFiltro.AutoSize = true;
            this.lblFiltro.Font = new Font("Segoe UI", 10F);
            this.lblFiltro.ForeColor = Theme.DarkText;
            this.lblFiltro.Location = new Point(21, 60);
            this.lblFiltro.Text = "Tipo de Reporte:";

            this.cmbTipoReporte.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTipoReporte.Items.AddRange(new object[] {
                "Ventas Totales por Cliente",
                "Productos Más Vendidos",
                "Inventario Bajo",
                "Facturas"
            });
            this.cmbTipoReporte.Location = new Point(155, 59);
            this.cmbTipoReporte.Size = new Size(250, 23);
            this.cmbTipoReporte.BackColor = Theme.DarkControl;
            this.cmbTipoReporte.ForeColor = Theme.DarkText;
            this.cmbTipoReporte.FlatStyle = FlatStyle.Flat;
            this.cmbTipoReporte.SelectedIndex = 0;
            this.cmbTipoReporte.SelectedIndexChanged += CmbTipoReporte_SelectedIndexChanged;

            this.lblUmbral.AutoSize = true;
            this.lblUmbral.Font = new Font("Segoe UI", 10F);
            this.lblUmbral.ForeColor = Theme.DarkText;
            this.lblUmbral.Location = new Point(410, 60);
            this.lblUmbral.Text = "Umbral:";
            this.lblUmbral.Visible = false;

            this.numUmbral.Location = new Point(478, 59);
            this.numUmbral.Size = new Size(60, 23);
            this.numUmbral.BackColor = Theme.DarkControl;
            this.numUmbral.ForeColor = Theme.DarkText;
            this.numUmbral.Value = 10;
            this.numUmbral.Visible = false;

            this.lblClientesFactura.AutoSize = true;
            this.lblClientesFactura.Font = new Font("Segoe UI", 10F);
            this.lblClientesFactura.ForeColor = Theme.DarkText;
            this.lblClientesFactura.Location = new Point(25, 95);
            this.lblClientesFactura.Text = "Clientes:";
            this.lblClientesFactura.Visible = false;

            this.cmbClientesFactura.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbClientesFactura.Location = new Point(100, 94);
            this.cmbClientesFactura.Size = new Size(220, 23);
            this.cmbClientesFactura.BackColor = Theme.DarkControl;
            this.cmbClientesFactura.ForeColor = Theme.DarkText;
            this.cmbClientesFactura.FlatStyle = FlatStyle.Flat;
            this.cmbClientesFactura.Visible = false;
            this.cmbClientesFactura.SelectedIndexChanged += CmbClientesFactura_SelectedIndexChanged;

            this.lblIdClienteSeleccionado.AutoSize = true;
            this.lblIdClienteSeleccionado.Font = new Font("Segoe UI", 10F);
            this.lblIdClienteSeleccionado.ForeColor = Theme.DarkText;
            this.lblIdClienteSeleccionado.Location = new Point(330, 95);
            this.lblIdClienteSeleccionado.Text = "ID:";
            this.lblIdClienteSeleccionado.Visible = false;

            this.txtIdClienteSeleccionado.Location = new Point(360, 94);
            this.txtIdClienteSeleccionado.Size = new Size(80, 23);
            this.txtIdClienteSeleccionado.BackColor = Theme.DarkControl;
            this.txtIdClienteSeleccionado.ForeColor = Theme.DarkText;
            this.txtIdClienteSeleccionado.BorderStyle = BorderStyle.FixedSingle;
            this.txtIdClienteSeleccionado.ReadOnly = true;
            this.txtIdClienteSeleccionado.Visible = false;

            this.lblFechaCompra.AutoSize = true;
            this.lblFechaCompra.Font = new Font("Segoe UI", 10F);
            this.lblFechaCompra.ForeColor = Theme.DarkText;
            this.lblFechaCompra.Location = new Point(460, 95);
            this.lblFechaCompra.Text = "Fecha de compra:";
            this.lblFechaCompra.Visible = false;

            this.dtpFechaCompra.Format = DateTimePickerFormat.Short;
            this.dtpFechaCompra.Location = new Point(610, 94);
            this.dtpFechaCompra.Size = new Size(120, 23);
            this.dtpFechaCompra.Visible = false;

            this.btnGenerar.BackColor = Theme.AccentColor;
            this.btnGenerar.FlatStyle = FlatStyle.Flat;
            this.btnGenerar.FlatAppearance.BorderSize = 0;
            this.btnGenerar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnGenerar.ForeColor = Color.White;
            this.btnGenerar.Location = new Point(555, 55);
            this.btnGenerar.Size = new Size(100, 35);
            this.btnGenerar.Text = "Generar";
            this.btnGenerar.Cursor = Cursors.Hand;
            this.btnGenerar.Click += BtnGenerar_Click;

            this.btnExportarJson.BackColor = Theme.AccentColor;
            this.btnExportarJson.FlatStyle = FlatStyle.Flat;
            this.btnExportarJson.FlatAppearance.BorderSize = 0;
            this.btnExportarJson.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnExportarJson.ForeColor = Color.White;
            this.btnExportarJson.Location = new Point(555, 10);
            this.btnExportarJson.Size = new Size(140, 35);
            this.btnExportarJson.Text = "Exportar JSON";
            this.btnExportarJson.Cursor = Cursors.Hand;
            this.btnExportarJson.Click += BtnExportarJson_Click;

            this.dgvReportes.Location = new Point(25, 130);
            this.dgvReportes.Size = new Size(750, 300);
            this.dgvReportes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Theme.ApplyDarkDataGridView(this.dgvReportes);

            this.BackColor = Theme.DarkDesktop;
            this.ClientSize = new Size(800, 500);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.btnExportarJson);
            this.Controls.Add(this.numUmbral);
            this.Controls.Add(this.lblUmbral);
            this.Controls.Add(this.lblClientesFactura);
            this.Controls.Add(this.cmbClientesFactura);
            this.Controls.Add(this.lblIdClienteSeleccionado);
            this.Controls.Add(this.txtIdClienteSeleccionado);
            this.Controls.Add(this.lblFechaCompra);
            this.Controls.Add(this.dtpFechaCompra);
            this.Controls.Add(this.dgvReportes);
            this.Controls.Add(this.cmbTipoReporte);
            this.Controls.Add(this.lblFiltro);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FrmReportes";
            this.Text = "Reportes";

            ((System.ComponentModel.ISupportInitialize)(this.dgvReportes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUmbral)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void CmbTipoReporte_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool esBajoStock = cmbTipoReporte.SelectedIndex == 2;
            bool esFacturas = cmbTipoReporte.SelectedIndex == 3;

            lblUmbral.Visible = esBajoStock;
            numUmbral.Visible = esBajoStock;

            lblClientesFactura.Visible = esFacturas;
            cmbClientesFactura.Visible = esFacturas;
            lblIdClienteSeleccionado.Visible = esFacturas;
            txtIdClienteSeleccionado.Visible = esFacturas;
            lblFechaCompra.Visible = esFacturas;
            dtpFechaCompra.Visible = esFacturas;

            if (esFacturas)
            {
                CargarClientesConFacturas();
            }
            else
            {
                cmbClientesFactura.DataSource = null;
                txtIdClienteSeleccionado.Clear();
            }
        }

        private void CargarClientesConFacturas()
        {
            var ventaService = new VentaService();
            var ventas = ventaService.ObtenerVentas();
            var clientes = ClienteDAO.ObtenerClientes();

            var clientesConVentas = clientes
                .Where(c => ventas.Any(v => v.ClienteId == c.Id))
                .ToList();

            cmbClientesFactura.DataSource = null;
            cmbClientesFactura.DataSource = clientesConVentas;
            cmbClientesFactura.DisplayMember = "Nombre";
            cmbClientesFactura.ValueMember = "Id";

            if (clientesConVentas.Count > 0)
            {
                CargarFechasPorCliente();
            }
        }

        private void CmbClientesFactura_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbClientesFactura.SelectedValue == null)
                return;

            txtIdClienteSeleccionado.Text = cmbClientesFactura.SelectedValue.ToString();
            CargarFechasPorCliente();
        }

        private void CargarFechasPorCliente()
        {
            if (cmbClientesFactura.SelectedValue == null)
                return;

            int clienteId;
            if (!int.TryParse(cmbClientesFactura.SelectedValue.ToString(), out clienteId))
                return;

            var ventaService = new VentaService();
            var ventas = ventaService.ObtenerVentas()
                .Where(v => v.ClienteId == clienteId)
                .ToList();

            if (ventas.Count > 0)
            {
                dtpFechaCompra.MinDate = ventas.Min(v => v.Fecha).Date;
                dtpFechaCompra.MaxDate = ventas.Max(v => v.Fecha).Date;
                dtpFechaCompra.Value = ventas.Max(v => v.Fecha).Date;
            }
        }

        private void BtnGenerar_Click(object? sender, EventArgs e)
        {
            try
            {
                var ventaService = new VentaService();

                if (cmbTipoReporte.SelectedIndex == 0)
                {
                    var ventas = ventaService.ObtenerVentas();
                    var clientes = ClienteDAO.ObtenerClientes();

                    var reporte = ventas
                        .GroupBy(v => v.ClienteId)
                        .Select(g =>
                        {
                            var cliente = clientes.FirstOrDefault(c => c.Id == g.Key);
                            return new
                            {
                                ClienteID = g.Key,
                                NombreCliente = cliente != null ? cliente.Nombre : "Desconocido",
                                TotalVentasRegistradas = g.Count()
                            };
                        })
                        .OrderByDescending(x => x.TotalVentasRegistradas)
                        .ToList();

                    dgvReportes.DataSource = reporte;
                }
                else if (cmbTipoReporte.SelectedIndex == 1)
                {
                    var top = ventaService.ObtenerTopProductos();
                    var productos = ProductoDAO.ObtenerProductos();

                    var reporte = top
                        .Select(p =>
                        {
                            var producto = productos.FirstOrDefault(x => x.Codigo == p.Codigo);
                            return new
                            {
                                CodigoP = p.Codigo,
                                NombreProducto = producto != null ? producto.Nombre : "Desconocido",
                                CantidadVendida = p.Cantidad
                            };
                        })
                        .ToList();

                    dgvReportes.DataSource = reporte;
                }
                else if (cmbTipoReporte.SelectedIndex == 2)
                {
                    int umbral = (int)numUmbral.Value;
                    var lista = ProductoDAO.ObtenerBajoStock(umbral);

                    var reporte = lista
                        .Select(p => new
                        {
                            Codigo = p.Codigo,
                            Nombre = p.Nombre,
                            StockActual = p.Stock
                        })
                        .ToList();

                    dgvReportes.DataSource = reporte;
                }
                else if (cmbTipoReporte.SelectedIndex == 3)
                {
                    if (cmbClientesFactura.SelectedValue == null)
                    {
                        MessageBox.Show("Seleccione un cliente.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int clienteId = Convert.ToInt32(cmbClientesFactura.SelectedValue);
                    DateTime fecha = dtpFechaCompra.Value.Date;

                    var ventas = ventaService.ObtenerVentas();
                    var productos = ProductoDAO.ObtenerProductos();
                    var clientes = ClienteDAO.ObtenerClientes();

                    var ventasFiltradas = ventas
                        .Where(v => v.ClienteId == clienteId && v.Fecha.Date == fecha)
                        .ToList();

                    var listaFinal = new List<object>();

                    foreach (var venta in ventasFiltradas)
                    {
                        var detalles = ventaService.ObtenerDetalles(venta.Id);
                        var cliente = clientes.FirstOrDefault(c => c.Id == venta.ClienteId);

                        foreach (var d in detalles)
                        {
                            var producto = productos.FirstOrDefault(p => p.Codigo == d.CodigoP);

                            listaFinal.Add(new
                            {
                                FacturaID = venta.Id,
                                Fecha = venta.Fecha.ToString("dd/MM/yyyy"),
                                IdCliente = venta.ClienteId,
                                Cliente = cliente != null ? cliente.Nombre : "Desconocido",
                                CodigoP = d.CodigoP,
                                Producto = producto != null ? producto.Nombre : "Desconocido",
                                Descripcion = producto != null ? producto.Descripcion : "",
                                Cantidad = d.Cantidad,
                                Precio = producto != null ? producto.Precio : 0,
                                TotalPagado = venta.Total
                            });
                        }
                    }

                    dgvReportes.DataSource = listaFinal;

                    if (dgvReportes.Columns["Precio"] != null)
                        dgvReportes.Columns["Precio"].DefaultCellStyle.Format = "N2";

                    if (dgvReportes.Columns["TotalPagado"] != null)
                        dgvReportes.Columns["TotalPagado"].DefaultCellStyle.Format = "N2";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar reporte: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportarJson_Click(object? sender, EventArgs e)
        {
            try
            {
                if (dgvReportes.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos para exportar.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var lista = new List<Dictionary<string, object>>();

                foreach (DataGridViewRow row in dgvReportes.Rows)
                {
                    if (row.IsNewRow) continue;

                    var dic = new Dictionary<string, object>();

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string columna = dgvReportes.Columns[cell.ColumnIndex].HeaderText;
                        dic[columna] = cell.Value ?? "";
                    }

                    lista.Add(dic);
                }

                string json = JsonSerializer.Serialize(
                    lista,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string nombreArchivo = "Reporte.json";
                string rutaCompleta = Path.Combine(escritorio, nombreArchivo);

                File.WriteAllText(rutaCompleta, json);

                MessageBox.Show(
                    "Reporte exportado correctamente a su Escritorio",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}