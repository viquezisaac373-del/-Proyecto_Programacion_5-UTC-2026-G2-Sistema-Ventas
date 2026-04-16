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
    // Clase principal del formulario de reportes que hereda de Form
    public class FrmReportes : Form
    {
        private Label lblTitulo = null!;
        private ComboBox cmbTipoReporte = null!;
        private Button btnGenerar = null!;
        private Button btnExportarJson = null!;
        private DataGridView dgvReportes = null!;
        private DateTimePicker dtpDesde = null!;
        private DateTimePicker dtpHasta = null!;

        // Etiquetas del Dashboard
        private Label lblTotalIngresos = null!;
        private Label lblTotalVentas = null!;
        private Label lblUtilidadEstimada = null!;

        public FrmReportes()
        {
            InitializeComponent();  // Inicializa todos los componentes
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Configuración del Formulario
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ClientSize = new Size(950, 650);
            this.MinimumSize = new Size(900, 600);
            this.Text = "Dashboard de Negocios y Reportes";

            lblTitulo = new Label
            {
                Text = "REPORTES Y ESTADÍSTICAS",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(25, 15),
                AutoSize = true
            };

            // --- CONTENEDOR DE TARJETAS (Solución al desorden visual) ---
            TableLayoutPanel tlpDashboard = new TableLayoutPanel
            {
                Location = new Point(25, 65),
                Size = new Size(880, 110),
                ColumnCount = 3,
                RowCount = 1,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            tlpDashboard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpDashboard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpDashboard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            tlpDashboard.Controls.Add(CrearTarjeta("INGRESOS BRUTOS", out lblTotalIngresos, Color.FromArgb(0, 122, 204)), 0, 0);
            tlpDashboard.Controls.Add(CrearTarjeta("TRANSACCIONES", out lblTotalVentas, Color.FromArgb(0, 150, 136)), 1, 0);
            tlpDashboard.Controls.Add(CrearTarjeta("UTILIDAD ESTIMADA", out lblUtilidadEstimada, Color.FromArgb(255, 152, 0)), 2, 0);

            // --- FILTROS DE FECHA Y TIPO ---
            int yFiltro = 195;
            Label lDesde = new Label { Text = "Desde:", ForeColor = Color.Gray, Location = new Point(25, yFiltro + 5), AutoSize = true };
            dtpDesde = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(75, yFiltro), Size = new Size(110, 25) };
            dtpDesde.Value = DateTime.Now.AddDays(-30);

            Label lHasta = new Label { Text = "Hasta:", ForeColor = Color.Gray, Location = new Point(200, yFiltro + 5), AutoSize = true };
            dtpHasta = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(250, yFiltro), Size = new Size(110, 25) };

            cmbTipoReporte = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(380, yFiltro),
                Size = new Size(220, 25),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White
            };
            cmbTipoReporte.Items.AddRange(new object[] {
                "Cierre de Ventas (IVA)",
                "Ranking de Productos",
                "Auditoría de Clientes",
                "Análisis de Utilidad"
            });
            cmbTipoReporte.SelectedIndex = 0;

            btnGenerar = new Button
            {
                Text = "GENERAR",
                Location = new Point(620, yFiltro - 2),
                Size = new Size(130, 32),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGenerar.Click += BtnGenerar_Click;

            btnExportarJson = new Button
            {
                Text = "EXPORTAR",
                Location = new Point(760, yFiltro - 2),
                Size = new Size(100, 32),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportarJson.Click += BtnExportarJson_Click;

            // --- TABLA DE RESULTADOS ---
            dgvReportes = new DataGridView
            {
                Location = new Point(25, 245),
                Size = new Size(880, 360),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            Theme.ApplyDarkDataGridView(dgvReportes);

            this.Controls.AddRange(new Control[] { lblTitulo, tlpDashboard, lDesde, dtpDesde, lHasta, dtpHasta, cmbTipoReporte, btnGenerar, btnExportarJson, dgvReportes });
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Panel CrearTarjeta(string titulo, out Label lblValor, Color acento)
        {
            Panel p = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(45, 45, 48), Margin = new Padding(10) };
            Panel borde = new Panel { Dock = DockStyle.Left, Width = 6, BackColor = acento };
            Label t = new Label { Text = titulo, ForeColor = Color.Gray, Location = new Point(20, 15), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            lblValor = new Label { Text = "₡0,00", ForeColor = Color.White, Location = new Point(20, 45), AutoSize = true, Font = new Font("Segoe UI", 16F, FontStyle.Bold) };
            p.Controls.Add(borde); p.Controls.Add(t); p.Controls.Add(lblValor);
            return p;
        }

        // Evento del botón generar reporte
        private void BtnGenerar_Click(object? sender, EventArgs e)
        {
            try
            {
                var vService = new VentaService();
                var cService = new ClienteService();
                var todas = vService.ObtenerVentas();
                var filtradas = todas.Where(v => v.Fecha.Date >= dtpDesde.Value.Date && v.Fecha.Date <= dtpHasta.Value.Date).ToList();

                // Actualizar Dashboard (Colones ₡)
                decimal bruto = filtradas.Sum(x => x.Total);
                lblTotalIngresos.Text = bruto.ToString("C");
                lblTotalVentas.Text = filtradas.Count.ToString();
                lblUtilidadEstimada.Text = (bruto * 0.25m).ToString("C"); // Margen de utilidad ejemplo 25%

                int op = cmbTipoReporte.SelectedIndex;

                if (op == 0) // Cierre de Ventas
                {
                    dgvReportes.DataSource = filtradas.Select(v => new {
                        Factura = v.Id,
                        v.Fecha,
                        v.Subtotal,
                        IVA = v.Impuesto,
                        v.Total
                    }).OrderByDescending(x => x.Factura).ToList();
                }
                else if (op == 1) // Ranking de Productos (CON NOMBRES)
                {
                    var top = vService.ObtenerTopProductos();
                    var prods = ProductoDAO.ObtenerProductos();
                    dgvReportes.DataSource = top.Select(t => new {
                        t.Codigo,
                        Producto = prods.FirstOrDefault(p => p.Codigo == t.Codigo)?.Nombre ?? "Desconocido",
                        Vendidos = t.Cantidad
                    }).OrderByDescending(x => x.Vendidos).ToList();
                }
                else if (op == 2) // Auditoría de Clientes
                {
                    var clientes = cService.ObtenerClientes();
                    dgvReportes.DataSource = filtradas.GroupBy(v => v.ClienteId).Select(g => new {
                        Nombre = clientes.FirstOrDefault(c => c.Id == g.Key)?.Nombre ?? "Consumidor Final",
                        Compras = g.Count(),
                        TotalGastado = g.Sum(x => x.Total)
                    }).OrderByDescending(x => x.TotalGastado).ToList();
                }
                else if (op == 3) // Análisis de Utilidad
                {
                    dgvReportes.DataSource = filtradas.Select(v => new {
                        Factura = v.Id,
                        v.Total,
                        MargenUtilidad = v.Total * 0.25m
                    }).ToList();
                }

                FormatearGrilla();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void FormatearGrilla()
        {
            string[] colsMoneda = { "Subtotal", "IVA", "Total", "TotalGastado", "MargenUtilidad" };
            foreach (var c in colsMoneda)
                if (dgvReportes.Columns[c] != null)
                    dgvReportes.Columns[c].DefaultCellStyle.Format = "C";
        }

        // Evento para exportar el reporte a JSON
        private void BtnExportarJson_Click(object? sender, EventArgs e)
        {
            try
            {
                if (dgvReportes.Rows.Count == 0) return;
                var datos = new List<object>();
                foreach (DataGridViewRow r in dgvReportes.Rows) if (!r.IsNewRow) datos.Add(r.DataBoundItem);

                string json = JsonSerializer.Serialize(datos, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReporteEjecutivo.json"), json);
                MessageBox.Show("Reporte exportado al Escritorio.");
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
    }
}