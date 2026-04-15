namespace Sistema_Completo_De_Ventas.UI.Forms
{
    partial class FrmPrincipal
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Panel pnlContenedor;
        private System.Windows.Forms.Button btnMenuVentas;
        private System.Windows.Forms.Button btnMenuProductos;
        private System.Windows.Forms.Button btnMenuClientes;
        private System.Windows.Forms.Button btnMenuReportes;
        private System.Windows.Forms.Button btnMenuUsuarios;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Label lblUsuarioLogueado;
        private System.Windows.Forms.Label lblTituloSistema;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.pnlContenedor = new System.Windows.Forms.Panel();
            this.btnMenuVentas = new System.Windows.Forms.Button();
            this.btnMenuProductos = new System.Windows.Forms.Button();
            this.btnMenuClientes = new System.Windows.Forms.Button();
            this.btnMenuReportes = new System.Windows.Forms.Button();
            this.btnMenuUsuarios = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.lblUsuarioLogueado = new System.Windows.Forms.Label();
            this.lblTituloSistema = new System.Windows.Forms.Label();

            this.SuspendLayout();
            this.pnlMenu.SuspendLayout();

            // ── Panel Menú Lateral (Azul Profesional) ──
            this.pnlMenu.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(220, 650);
            this.pnlMenu.Controls.Add(this.lblTituloSistema);
            this.pnlMenu.Controls.Add(this.btnMenuVentas);
            this.pnlMenu.Controls.Add(this.btnMenuProductos);
            this.pnlMenu.Controls.Add(this.btnMenuClientes);
            this.pnlMenu.Controls.Add(this.btnMenuReportes);
            this.pnlMenu.Controls.Add(this.btnMenuUsuarios);
            this.pnlMenu.Controls.Add(this.btnSalir);
            this.pnlMenu.Controls.Add(this.lblUsuarioLogueado);

            // ── Título en el Menú ──
            this.lblTituloSistema.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTituloSistema.ForeColor = System.Drawing.Color.White;
            this.lblTituloSistema.Location = new System.Drawing.Point(0, 20);
            this.lblTituloSistema.Size = new System.Drawing.Size(220, 60);
            this.lblTituloSistema.Text = "VENTAS PRO";
            this.lblTituloSistema.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ── Label Usuario (Abajo del Título) ──
            this.lblUsuarioLogueado.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblUsuarioLogueado.ForeColor = System.Drawing.Color.FromArgb(200, 220, 240);
            this.lblUsuarioLogueado.Location = new System.Drawing.Point(0, 70);
            this.lblUsuarioLogueado.Size = new System.Drawing.Size(220, 30);
            this.lblUsuarioLogueado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ── Estilo Común para Botones ──
            System.Action<System.Windows.Forms.Button, string, int> EstiloBoton = (btn, texto, y) => {
                btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 48);
                btn.BackColor = System.Drawing.Color.Transparent;
                btn.ForeColor = System.Drawing.Color.White;
                btn.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
                btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                btn.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
                btn.Size = new System.Drawing.Size(220, 50);
                btn.Location = new System.Drawing.Point(0, y);
                btn.Text = texto;
                btn.Cursor = System.Windows.Forms.Cursors.Hand;
            };

            EstiloBoton(this.btnMenuVentas, "🛒  Ventas", 150);
            this.btnMenuVentas.Click += new System.EventHandler(this.btnMenuVentas_Click);

            EstiloBoton(this.btnMenuProductos, "📦  Productos", 205);
            this.btnMenuProductos.Click += new System.EventHandler(this.btnMenuProductos_Click);

            EstiloBoton(this.btnMenuClientes, "👥  Clientes", 260);
            this.btnMenuClientes.Click += new System.EventHandler(this.btnMenuClientes_Click);

            EstiloBoton(this.btnMenuReportes, "📊  Reportes", 315);
            this.btnMenuReportes.Click += new System.EventHandler(this.btnMenuReportes_Click);

            EstiloBoton(this.btnMenuUsuarios, "👤  Usuarios", 370);
            this.btnMenuUsuarios.Click += new System.EventHandler(this.btnMenuUsuarios_Click);

            // ── Botón Salir (Rojo discreto al fondo) ──
            EstiloBoton(this.btnSalir, "🚪  Cerrar Sesión", 580);
            this.btnSalir.BackColor = System.Drawing.Color.FromArgb(200, 80, 80);
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);

            // ── Panel Contenedor (Fondo Oscuro Moderno) ──
            this.pnlContenedor.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.pnlContenedor.Dock = System.Windows.Forms.DockStyle.Fill;

            // ── Configuración del Formulario Principal ──
            this.BackColor = System.Drawing.Color.FromArgb(15, 15, 15);
            this.ClientSize = new System.Drawing.Size(1200, 650);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.Add(this.pnlContenedor);
            this.Controls.Add(this.pnlMenu);
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);

            this.pnlMenu.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}