using System;
using System.Drawing;
using System.Windows.Forms;
using SistemaVentas.DTO;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    // Formulario principal que actúa como contenedor del menú de navegación.
    // Recibe el usuario autenticado y ajusta los módulos visibles según su rol.
    public partial class FrmPrincipal : Form
    {
        // Usuario autenticado que determina los permisos de navegación
        private UsuarioDTO _usuario;

        // Constructor: recibe el DTO del usuario logueado desde FrmLogin
        public FrmPrincipal(UsuarioDTO usuario)
        {
            InitializeComponent();

            // Si no se recibe un usuario válido, se lanza una excepción para evitar
            // que el sistema cargue sin saber quién está operando
            _usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));

            // Al cerrar el formulario principal se termina toda la aplicación
            this.FormClosed += (s, args) => Application.Exit();
        }

        // Evento que se ejecuta al cargar el formulario.
        // Configura el menú lateral según el rol del usuario autenticado.
        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            // Se muestra el nombre y rol del usuario en la barra superior
            lblUsuarioLogueado.Text = $"{_usuario.Usuario} ({_usuario.NombreRol})";

            // Por seguridad, se ocultan todos los botones del menú antes de asignar permisos.
            // Así se evita que un rol no contemplado acceda a módulos restringidos.
            btnMenuVentas.Visible = false;
            btnMenuProductos.Visible = false;
            btnMenuClientes.Visible = false;
            btnMenuReportes.Visible = false;
            btnMenuUsuarios.Visible = false;

            // Se normaliza el nombre del rol para evitar errores por espacios o mayúsculas
            string rol = _usuario.NombreRol.Trim().ToUpper();

            try
            {
                if (rol == "ADMINISTRADOR" || rol == "ADMIN")
                {
                    // El administrador tiene acceso completo a todos los módulos
                    btnMenuVentas.Visible = true;
                    btnMenuProductos.Visible = true;
                    btnMenuClientes.Visible = true;
                    btnMenuReportes.Visible = true;
                    btnMenuUsuarios.Visible = true;
                    AbrirFormulario(new FrmVentas());
                }
                else if (rol == "CAJERO")
                {
                    // El cajero solo puede realizar ventas y ver reportes
                    btnMenuVentas.Visible = true;
                    btnMenuReportes.Visible = true;
                    AbrirFormulario(new FrmVentas());
                }
                else if (rol == "AUDITOR")
                {
                    // El auditor solo tiene acceso al módulo de reportes
                    btnMenuReportes.Visible = true;
                    AbrirFormulario(new FrmReportes());
                }

                // Se reposicionan los botones visibles para que no queden huecos en el menú
                ReorganizarMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // Reposiciona verticalmente solo los botones visibles del menú lateral,
        // evitando espacios vacíos donde estarían los botones ocultos
        private void ReorganizarMenu()
        {
            int y = 150; // Posición Y inicial del primer botón visible
            Button[] botones = { btnMenuVentas, btnMenuProductos, btnMenuClientes, btnMenuReportes, btnMenuUsuarios };

            foreach (Button btn in botones)
            {
                if (btn.Visible)
                {
                    btn.Location = new Point(0, y);
                    y += 55; // Separación entre cada botón del menú
                }
            }
        }

        // Abre un formulario hijo dentro del panel contenedor central.
        // Reemplaza el formulario anterior si ya había uno abierto.
        private void AbrirFormulario(Form fh)
        {
            // Se elimina el formulario anterior del contenedor si existe
            if (this.pnlContenedor.Controls.Count > 0)
                this.pnlContenedor.Controls.RemoveAt(0);

            // Se configura el formulario para que se comporte como panel incrustado
            fh.TopLevel = false;
            fh.FormBorderStyle = FormBorderStyle.None;
            fh.Dock = DockStyle.Fill;

            this.pnlContenedor.Controls.Add(fh);
            this.pnlContenedor.Tag = fh;
            fh.Show();
        }

        // Botones del menú: cada uno abre su formulario correspondiente en el contenedor
        private void btnMenuVentas_Click(object sender, EventArgs e) => AbrirFormulario(new FrmVentas());
        private void btnMenuProductos_Click(object sender, EventArgs e) => AbrirFormulario(new FrmProductos());
        private void btnMenuClientes_Click(object sender, EventArgs e) => AbrirFormulario(new FrmClientes());
        private void btnMenuReportes_Click(object sender, EventArgs e) => AbrirFormulario(new FrmReportes());
        private void btnMenuUsuarios_Click(object sender, EventArgs e) => AbrirFormulario(new FrmUsuarios());

        // Botón de cerrar sesión: reinicia la aplicación para volver al login
        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar sesión?", "Salir",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Application.Restart() cierra y vuelve a ejecutar el programa,
                // lo que regresa al formulario de login limpiamente
                Application.Restart();
            }
        }
    }
}