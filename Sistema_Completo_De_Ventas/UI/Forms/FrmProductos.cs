using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    public class FrmProductos : Form
    {
        private DataGridView dgvProductos;
        private Label lblTitulo;

        public FrmProductos()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.dgvProductos = new DataGridView();
            this.lblTitulo = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Theme.DarkText;
            this.lblTitulo.Location = new Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new Size(205, 25);
            this.lblTitulo.Text = "Gestión de Productos";

            // dgvProductos
            this.dgvProductos.Location = new Point(25, 70);
            this.dgvProductos.Name = "dgvProductos";
            this.dgvProductos.Size = new Size(750, 400);
            this.dgvProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Theme.ApplyDarkDataGridView(this.dgvProductos);

            // FrmProductos
            this.BackColor = Theme.DarkDesktop;
            this.ClientSize = new Size(800, 500);
            this.Controls.Add(this.dgvProductos);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FrmProductos";
            this.Text = "Productos";
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
