using System;
using System.Drawing;
using System.Windows.Forms;
using SistemaVentas.DTO;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    // Usamos 'partial' para no pelear con el diseñador de Visual Studio
    public partial class FrmPrincipal : Form
    {
        // Controles con guion bajo (_) y null! para evitar choques y advertencias CS8618
        private Panel _panelSidebar = null!;
        private Panel _panelTitleBar = null!;
        private Panel _panelDesktop = null!;
        private Button _btnClientes = null!;
        private Button _btnProductos = null!;
        private Button _btnVentas = null!;
        private Button _btnReportes = null!;
        private Button _btnUsuarios = null!; // NUEVO: Botón para el módulo de usuarios
        private Button _btnSalir = null!;
        private Label _lblTitle = null!;
        private Label _lblUserStatus = null!;
        private Form? _activeForm;

        private readonly UsuarioDTO _usuarioLogueado;

        // Paleta de colores base
        private readonly Color colorSideBar = Color.FromArgb(31, 31, 31);
        private readonly Color colorTitleBar = Color.FromArgb(20, 20, 20);
        private readonly Color colorDesktop = Color.FromArgb(45, 45, 48);
        private readonly Color colorText = Color.FromArgb(240, 240, 240);
        private readonly Color colorButtonHover = Color.FromArgb(60, 60, 60);

        public FrmPrincipal(UsuarioDTO usuario)
        {
            _usuarioLogueado = usuario;

            // Dibujamos nuestra interfaz limpia
            ConstruirInterfaz();

            // Aplicamos reglas de seguridad según el rol
            ConfigurarAccesoPorRol();
        }

        private void ConstruirInterfaz()
        {
            // Limpiamos la basura del diseñador para evitar duplicidades
            this.Controls.Clear();

            // Instanciación de todos los controles
            this._panelSidebar = new Panel();
            this._btnSalir = new Button();
            this._btnUsuarios = new Button(); // Inicializamos el botón de usuarios
            this._btnReportes = new Button();
            this._btnVentas = new Button();
            this._btnProductos = new Button();
            this._btnClientes = new Button();
            this._panelTitleBar = new Panel();
            this._lblTitle = new Label();
            this._lblUserStatus = new Label();
            this._panelDesktop = new Panel();

            this.SuspendLayout();

            this.ClientSize = new Size(1200, 750);
            this.Name = "FrmPrincipal";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sistema de Ventas - Control de Acceso";
            this.BackColor = colorDesktop;
            this.ForeColor = colorText;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            // -- PANEL LATERAL (Sidebar) --
            this._panelSidebar.BackColor = colorSideBar;
            this._panelSidebar.Dock = DockStyle.Left;
            this._panelSidebar.Width = 220;

            // Configuración de botones y sus posiciones Y
            ConfigurarBotonSidebar(_btnClientes, "Clientes", 100);
            this._btnClientes.Click += BtnClientes_Click;

            ConfigurarBotonSidebar(_btnProductos, "Productos", 160);
            this._btnProductos.Click += BtnProductos_Click;

            ConfigurarBotonSidebar(_btnVentas, "Ventas", 220);
            this._btnVentas.Click += BtnVentas_Click;

            ConfigurarBotonSidebar(_btnReportes, "Reportes", 280);
            this._btnReportes.Click += BtnReportes_Click;

            // NUEVO: Botón de Usuarios debajo de Reportes (posición Y = 340)
            ConfigurarBotonSidebar(_btnUsuarios, "Usuarios", 340);
            this._btnUsuarios.Click += BtnUsuarios_Click;

            ConfigurarBotonSidebar(_btnSalir, "Cerrar Sesión", this.ClientSize.Height - 60);
            this._btnSalir.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this._btnSalir.Click += BtnSalir_Click;

            // Añadimos TODOS los botones al panel lateral
            this._panelSidebar.Controls.AddRange(new Control[] {
                _btnClientes, _btnProductos, _btnVentas, _btnReportes, _btnUsuarios, _btnSalir
            });

            // -- BARRA SUPERIOR (TitleBar) --
            this._panelTitleBar.BackColor = colorTitleBar;
            this._panelTitleBar.Dock = DockStyle.Top;
            this._panelTitleBar.Height = 60;

            this._lblTitle.AutoSize = true;
            this._lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            this._lblTitle.Location = new Point(20, 15);
            this._lblTitle.Text = "INICIO";

            this._lblUserStatus.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._lblUserStatus.AutoSize = true;
            this._lblUserStatus.ForeColor = Color.DarkGray;
            this._lblUserStatus.Location = new Point(900, 22);
            this._lblUserStatus.Text = "Usuario: -";

            this._panelTitleBar.Controls.Add(this._lblTitle);
            this._panelTitleBar.Controls.Add(this._lblUserStatus);

            // -- ESCRITORIO (Panel Central) --
            this._panelDesktop.BackColor = colorDesktop;
            this._panelDesktop.Dock = DockStyle.Fill;

            // Agregamos los paneles principales a la ventana
            this.Controls.Add(this._panelDesktop);
            this.Controls.Add(this._panelTitleBar);
            this.Controls.Add(this._panelSidebar);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void ConfigurarAccesoPorRol()
        {
            _lblUserStatus.Text = $"Conectado: {_usuarioLogueado.NombreUsuario} ({_usuarioLogueado.Rol})";

            switch (_usuarioLogueado.Rol)
            {
                case "Cajero":
                    _btnProductos.Enabled = false;
                    _btnReportes.Visible = false;
                    _btnUsuarios.Visible = false; // El cajero NO ve el botón Usuarios
                    break;
                case "Contador":
                    _btnVentas.Enabled = false;
                    _btnUsuarios.Visible = false; // El contador NO ve el botón Usuarios
                    break;
                case "Administrador":
                    // El administrador tiene acceso total (No ocultamos ni bloqueamos nada)
                    break;
                default:
                    MessageBox.Show("Rol no válido.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Application.Exit();
                    break;
            }
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
            if (_activeForm != null) _activeForm.Close();
            _activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this._panelDesktop.Controls.Add(childForm);
            childForm.BringToFront();
            childForm.Show();
            _lblTitle.Text = title.ToUpper();
        }

        // Eventos de los botones del menú
        private void BtnClientes_Click(object? sender, EventArgs e) => OpenChildForm(new FrmClientes(), "Gestión de Clientes");
        private void BtnProductos_Click(object? sender, EventArgs e) => OpenChildForm(new FrmProductos(), "Inventario de Productos");
        private void BtnVentas_Click(object? sender, EventArgs e) => OpenChildForm(new FrmVentas(), "Facturación y Ventas");
        private void BtnReportes_Click(object? sender, EventArgs e) => OpenChildForm(new FrmReportes(), "Reportes Estadísticos");

        // Evento para abrir el formulario de gestión de usuarios
        private void BtnUsuarios_Click(object? sender, EventArgs e) => OpenChildForm(new FrmUsuarios(), "Gestión de Usuarios");

        private void BtnSalir_Click(object? sender, EventArgs e) => Application.Restart(); // Cierra sesión
    }
}