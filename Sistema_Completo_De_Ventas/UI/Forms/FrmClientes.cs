using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    public class FrmClientes : Form
    {
        private DataGridView dgvClientes;
        private Label lblTitulo;

        public FrmClientes()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.dgvClientes = new DataGridView();
            this.lblTitulo = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).BeginInit();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Theme.DarkText;
            this.lblTitulo.Location = new Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new Size(182, 25);
            this.lblTitulo.Text = "Gestión de Clientes";

            // dgvClientes
            this.dgvClientes.Location = new Point(25, 70);
            this.dgvClientes.Name = "dgvClientes";
            this.dgvClientes.Size = new Size(750, 400);
            this.dgvClientes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Theme.ApplyDarkDataGridView(this.dgvClientes);

            // FrmClientes
            this.BackColor = Theme.DarkDesktop;
            this.ClientSize = new Size(800, 500);
            this.Controls.Add(this.dgvClientes);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FrmClientes";
            this.Text = "Clientes";
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
