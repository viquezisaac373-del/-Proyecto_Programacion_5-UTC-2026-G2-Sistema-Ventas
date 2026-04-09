using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    public class FrmVentas : Form
    {
        private DataGridView dgvCarrito;
        private Label lblTitulo;
        private Label lblTotal;
        private Button btnProcesarVenta;
        private ComboBox cmbClientes;
        private Label lblCliente;

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
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).BeginInit();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitulo.ForeColor = Theme.DarkText;
            this.lblTitulo.Location = new Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new Size(116, 25);
            this.lblTitulo.Text = "Facturación";

            // lblCliente
            this.lblCliente.AutoSize = true;
            this.lblCliente.Font = new Font("Segoe UI", 10F);
            this.lblCliente.ForeColor = Theme.DarkText;
            this.lblCliente.Location = new Point(21, 60);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new Size(128, 19);
            this.lblCliente.Text = "Seleccionar Cliente:";

            // cmbClientes
            this.cmbClientes.FormattingEnabled = true;
            this.cmbClientes.Location = new Point(155, 59);
            this.cmbClientes.Name = "cmbClientes";
            this.cmbClientes.Size = new Size(300, 23);
            this.cmbClientes.BackColor = Theme.DarkControl;
            this.cmbClientes.ForeColor = Theme.DarkText;
            this.cmbClientes.FlatStyle = FlatStyle.Flat;

            // dgvCarrito
            this.dgvCarrito.Location = new Point(25, 100);
            this.dgvCarrito.Name = "dgvCarrito";
            this.dgvCarrito.Size = new Size(750, 300);
            this.dgvCarrito.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Theme.ApplyDarkDataGridView(this.dgvCarrito);

            // lblTotal
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTotal.ForeColor = Theme.AccentColor;
            this.lblTotal.Location = new Point(20, 420);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lblTotal.Size = new Size(125, 30);
            this.lblTotal.Text = "Total: $0.00";

            // btnProcesarVenta
            this.btnProcesarVenta.BackColor = Theme.AccentColor;
            this.btnProcesarVenta.FlatStyle = FlatStyle.Flat;
            this.btnProcesarVenta.FlatAppearance.BorderSize = 0;
            this.btnProcesarVenta.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnProcesarVenta.ForeColor = Color.White;
            this.btnProcesarVenta.Location = new Point(625, 415);
            this.btnProcesarVenta.Name = "btnProcesarVenta";
            this.btnProcesarVenta.Size = new Size(150, 40);
            this.btnProcesarVenta.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnProcesarVenta.Text = "Procesar Venta";
            this.btnProcesarVenta.Cursor = Cursors.Hand;

            // FrmVentas
            this.BackColor = Theme.DarkDesktop;
            this.ClientSize = new Size(800, 500);
            this.Controls.Add(this.btnProcesarVenta);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.dgvCarrito);
            this.Controls.Add(this.cmbClientes);
            this.Controls.Add(this.lblCliente);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FrmVentas";
            this.Text = "Ventas";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarrito)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
