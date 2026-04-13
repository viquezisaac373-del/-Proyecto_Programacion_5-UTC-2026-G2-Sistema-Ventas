using System;
using System.Drawing;
using System.Linq;
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
        private Label lblUmbral;
        private NumericUpDown numUmbral;

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
            this.lblUmbral = new Label();
            this.numUmbral = new NumericUpDown();

            ((System.ComponentModel.ISupportInitialize)(this.dgvReportes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUmbral)).BeginInit();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Theme.DarkText;
            this.lblTitulo.Location = new Point(20, 20);
            this.lblTitulo.Text = "Reportes del Sistema";

            // lblFiltro
            this.lblFiltro.AutoSize = true;
            this.lblFiltro.Font = new Font("Segoe UI", 10F);
            this.lblFiltro.ForeColor = Theme.DarkText;
            this.lblFiltro.Location = new Point(21, 60);
            this.lblFiltro.Text = "Tipo de Reporte:";

            // cmbTipoReporte
            this.cmbTipoReporte.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTipoReporte.Items.AddRange(new object[] {
                "Ventas Totales por Cliente",
                "Productos Más Vendidos",
                "Inventario Bajo"
            });
            this.cmbTipoReporte.Location = new Point(155, 59);
            this.cmbTipoReporte.Size = new Size(250, 23);
            this.cmbTipoReporte.BackColor = Theme.DarkControl;
            this.cmbTipoReporte.ForeColor = Theme.DarkText;
            this.cmbTipoReporte.FlatStyle = FlatStyle.Flat;
            this.cmbTipoReporte.SelectedIndex = 0;
            this.cmbTipoReporte.SelectedIndexChanged += CmbTipoReporte_SelectedIndexChanged;

            // lblUmbral
            this.lblUmbral.AutoSize = true;
            this.lblUmbral.Font = new Font("Segoe UI", 10F);
            this.lblUmbral.ForeColor = Theme.DarkText;
            this.lblUmbral.Location = new Point(410, 60);
            this.lblUmbral.Text = "Umbral:";
            this.lblUmbral.Visible = false;

            // numUmbral
            this.numUmbral.Location = new Point(478, 59);
            this.numUmbral.Size = new Size(60, 23);
            this.numUmbral.BackColor = Theme.DarkControl;
            this.numUmbral.ForeColor = Theme.DarkText;
            this.numUmbral.Value = 10;
            this.numUmbral.Visible = false;

            // btnGenerar
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

            // dgvReportes
            this.dgvReportes.Location = new Point(25, 100);
            this.dgvReportes.Size = new Size(750, 300);
            this.dgvReportes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Theme.ApplyDarkDataGridView(this.dgvReportes);

            // FrmReportes
            this.BackColor = Theme.DarkDesktop;
            this.ClientSize = new Size(800, 500);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.numUmbral);
            this.Controls.Add(this.lblUmbral);
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
            lblUmbral.Visible = esBajoStock;
            numUmbral.Visible = esBajoStock;
        }

        private void BtnGenerar_Click(object? sender, EventArgs e)
        {
            try
            {
                var ventaService = new VentaService();

                if (cmbTipoReporte.SelectedIndex == 0) // Ventas por cliente
                {
                    var ventas = ventaService.ObtenerVentas();
                    var reporte = ventas.GroupBy(v => v.ClienteId)
                                        .Select(g => new {
                                            ClienteID = g.Key,
                                            TotalVentasRegistradas = g.Count()
                                        })
                                        .OrderByDescending(x => x.TotalVentasRegistradas)
                                        .ToList();
                    dgvReportes.DataSource = reporte;
                }
                else if (cmbTipoReporte.SelectedIndex == 1) // Top Productos
                {
                    var top = ventaService.ObtenerTopProductos();
                    var reporte = top.Select(p => new {
                        CodigoP = p.Codigo,
                        CantidadVendida = p.Cantidad
                    }).ToList();
                    dgvReportes.DataSource = reporte;
                }
                else if (cmbTipoReporte.SelectedIndex == 2) // Inventario Bajo
                {
                    int umbral = (int)numUmbral.Value;
                    var lista = ProductoDAO.ObtenerBajoStock(umbral);
                    var reporte = lista.Select(p => new {
                        Codigo = p.Codigo,
                        Nombre = p.Nombre,
                        StockActual = p.Stock
                    }).ToList();
                    dgvReportes.DataSource = reporte;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar reporte: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
