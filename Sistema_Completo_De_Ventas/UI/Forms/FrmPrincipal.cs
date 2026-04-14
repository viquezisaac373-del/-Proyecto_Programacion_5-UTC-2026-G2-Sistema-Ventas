using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    public class FrmPrincipal : Form
    {
        private Panel panelSidebar;
        private Panel panelTitleBar;
        private Panel panelDesktop;
        private Button btnClientes;
        private Button btnProductos;
        private Button btnVentas;
        private Button btnReportes;
        private Button btnSalir;
        private Label lblTitle;
        private Form? activeForm;

        private Color colorSideBar = Color.FromArgb(31, 31, 31);
        private Color colorTitleBar = Color.FromArgb(20, 20, 20);
        private Color colorDesktop = Color.FromArgb(45, 45, 48);
        private Color colorText = Color.FromArgb(240, 240, 240);
        private Color colorButtonHover = Color.FromArgb(60, 60, 60);

        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.panelSidebar = new Panel();
            this.btnSalir = new Button();
            this.btnReportes = new Button();
            this.btnVentas = new Button();
            this.btnProductos = new Button();
            this.btnClientes = new Button();
            this.panelTitleBar = new Panel();
            this.lblTitle = new Label();
            this.panelDesktop = new Panel();

            this.ClientSize = new Size(1350, 800);
            this.Name = "FrmPrincipal";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sistema de Ventas";
            this.BackColor = colorDesktop;
            this.ForeColor = colorText;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            // panelSidebar
            this.panelSidebar.BackColor = colorSideBar;
            this.panelSidebar.Dock = DockStyle.Left;
            this.panelSidebar.Width = 140;

            // btnClientes
            ConfigurarBotonSidebar(btnClientes, "Clientes", 100);
            this.btnClientes.Click += BtnClientes_Click;

            // btnProductos
            ConfigurarBotonSidebar(btnProductos, "Productos", 160);
            this.btnProductos.Click += BtnProductos_Click;

            // btnVentas
            ConfigurarBotonSidebar(btnVentas, "Ventas", 220);
            this.btnVentas.Click += BtnVentas_Click;

            // btnReportes
            ConfigurarBotonSidebar(btnReportes, "Reportes", 280);
            this.btnReportes.Click += BtnReportes_Click;

            // btnSalir
            ConfigurarBotonSidebar(btnSalir, "Salir", this.ClientSize.Height - 60);
            this.btnSalir.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnSalir.Click += BtnSalir_Click;

            this.panelSidebar.Controls.Add(this.btnClientes);
            this.panelSidebar.Controls.Add(this.btnProductos);
            this.panelSidebar.Controls.Add(this.btnVentas);
            this.panelSidebar.Controls.Add(this.btnReportes);
            this.panelSidebar.Controls.Add(this.btnSalir);

            // panelTitleBar
            this.panelTitleBar.BackColor = colorTitleBar;
            this.panelTitleBar.Dock = DockStyle.Top;
            this.panelTitleBar.Height = 60;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblTitle.ForeColor = colorText;
            this.lblTitle.Location = new Point(20, 15);
            this.lblTitle.Text = "INICIO";
            this.panelTitleBar.Controls.Add(this.lblTitle);

            // panelDesktop
            this.panelDesktop.BackColor = colorDesktop;
            this.panelDesktop.Dock = DockStyle.Fill;
            this.panelDesktop.Padding = new Padding(20);

            this.Controls.Add(this.panelDesktop);
            this.Controls.Add(this.panelTitleBar);
            this.Controls.Add(this.panelSidebar);
        }

        private void ConfigurarBotonSidebar(Button btn, string texto, int y)
        {
            btn.Text = texto;
            btn.Location = new Point(0, y);
            btn.Size = new Size(220, 60);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = colorButtonHover;
            btn.ForeColor = colorText;
            btn.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(20, 0, 0, 0);
            btn.Cursor = Cursors.Hand;
        }

        private void OpenChildForm(Form childForm, string title)
        {
            if (activeForm != null)
                activeForm.Close();

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            this.panelDesktop.Controls.Add(childForm);
            this.panelDesktop.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();

            lblTitle.Text = title.ToUpper();
        }

        private void BtnClientes_Click(object? sender, EventArgs e)
        {
            OpenChildForm(new FrmClientes(), "Clientes");
        }

        private void BtnProductos_Click(object? sender, EventArgs e)
        {
            OpenChildForm(new FrmProductos(), "Productos");
        }

        private void BtnVentas_Click(object? sender, EventArgs e)
        {
            OpenChildForm(new FrmVentas(), "Ventas (Facturación)");
        }

        private void BtnReportes_Click(object? sender, EventArgs e)
        {
            OpenChildForm(new FrmReportes(), "Reportes");
        }

        private void BtnSalir_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
