using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    public class FrmReportes : Form
    {
        private DataGridView dgvReportes;
        private Label lblTitulo;
        private ComboBox cmbTipoReporte;
        private Label lblFiltro;

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
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportes)).BeginInit();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Theme.DarkText;
            this.lblTitulo.Location = new Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new Size(182, 25);
            this.lblTitulo.Text = "Reportes del Sistema";

            // lblFiltro
            this.lblFiltro.AutoSize = true;
            this.lblFiltro.Font = new Font("Segoe UI", 10F);
            this.lblFiltro.ForeColor = Theme.DarkText;
            this.lblFiltro.Location = new Point(21, 60);
            this.lblFiltro.Name = "lblFiltro";
            this.lblFiltro.Size = new Size(111, 19);
            this.lblFiltro.Text = "Tipo de Reporte:";

            // cmbTipoReporte
            this.cmbTipoReporte.FormattingEnabled = true;
            this.cmbTipoReporte.Items.AddRange(new object[] {
                "Ventas Totales por Cliente",
                "Productos Más Vendidos",
                "Inventario Bajo"
            });
            this.cmbTipoReporte.Location = new Point(138, 59);
            this.cmbTipoReporte.Name = "cmbTipoReporte";
            this.cmbTipoReporte.Size = new Size(300, 23);
            this.cmbTipoReporte.BackColor = Theme.DarkControl;
            this.cmbTipoReporte.ForeColor = Theme.DarkText;
            this.cmbTipoReporte.FlatStyle = FlatStyle.Flat;
            this.cmbTipoReporte.SelectedIndex = 0;

            // dgvReportes
            this.dgvReportes.Location = new Point(25, 100);
            this.dgvReportes.Name = "dgvReportes";
            this.dgvReportes.Size = new Size(750, 300);
            this.dgvReportes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Theme.ApplyDarkDataGridView(this.dgvReportes);

            // FrmReportes
            this.BackColor = Theme.DarkDesktop;
            this.ClientSize = new Size(800, 500);
            this.Controls.Add(this.dgvReportes);
            this.Controls.Add(this.cmbTipoReporte);
            this.Controls.Add(this.lblFiltro);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FrmReportes";
            this.Text = "Reportes";
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
