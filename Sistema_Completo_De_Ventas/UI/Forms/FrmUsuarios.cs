using System;
using System.Drawing;
using System.Windows.Forms;
using SistemaVentas.BLL;
using SistemaVentas.DTO;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    // Formulario para la gestión de usuarios del sistema (crear y eliminar)
    public class FrmUsuarios : Form
    {
        // Tabla para mostrar los usuarios registrados
        private DataGridView _dgv = null!;

        // Campos de texto para ingresar el nombre de usuario y contraseña
        private TextBox _txtUser = null!, _txtPass = null!;

        // Lista desplegable para seleccionar el rol del nuevo usuario
        private ComboBox _cmbRol = null!;

        // Botones para guardar un nuevo usuario o eliminar el seleccionado
        private Button _btnGuardar = null!, _btnEliminar = null!;

        // Servicio de la capa de negocio que procesa las operaciones de usuario
        private UsuarioService _service = new UsuarioService();

        // Constructor: construye la interfaz y carga los usuarios al abrir el formulario
        public FrmUsuarios()
        {
            ConstruirInterfaz();
            RefrescarTabla();
        }

        // Método que define y posiciona todos los controles visuales del formulario
        private void ConstruirInterfaz()
        {
            // Colores oscuros para la interfaz del panel de administración
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;

            // Tabla de usuarios: solo lectura, selección por fila completa
            _dgv = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(600, 400),
                BackgroundColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Black,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,  // Solo se puede seleccionar un usuario a la vez
                ReadOnly = true       // El usuario no puede editar los datos directamente en la tabla
            };

            // Aplicar el tema oscuro
            Theme.ApplyDarkDataGridView(_dgv);

            // Etiqueta y campo para el nombre de usuario
            Label l1 = new Label { Text = "Usuario:", Location = new Point(640, 20) };
            _txtUser = new TextBox { Location = new Point(640, 45), Size = new Size(200, 25) };

            // Etiqueta y campo para la contraseña (oculta los caracteres por seguridad)
            Label l2 = new Label { Text = "Contraseña:", Location = new Point(640, 80) };
            _txtPass = new TextBox
            {
                Location = new Point(640, 105),
                Size = new Size(200, 25),
                UseSystemPasswordChar = true  // Muestra asteriscos en lugar del texto real
            };

            // Etiqueta y lista desplegable para seleccionar el rol
            Label l3 = new Label { Text = "Rol:", Location = new Point(640, 140) };
            _cmbRol = new ComboBox
            {
                Location = new Point(640, 165),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList  // Impide escribir opciones no válidas
            };
            _cmbRol.Items.AddRange(new string[] { "Administrador", "Auditor", "Cajero" });

            // Botón para crear un nuevo usuario
            _btnGuardar = new Button
            {
                Text = "Crear Usuario",
                Location = new Point(640, 210),
                Size = new Size(200, 40),
                BackColor = Color.DarkCyan
            };
            _btnGuardar.Click += BtnGuardar_Click;

            // Botón para eliminar el usuario seleccionado en la tabla
            _btnEliminar = new Button
            {
                Text = "Eliminar Seleccionado",
                Location = new Point(640, 260),
                Size = new Size(200, 40),
                BackColor = Color.Brown
            };
            _btnEliminar.Click += BtnEliminar_Click;

            // Se agregan todos los controles al formulario en una sola operación
            this.Controls.AddRange(new Control[] { _dgv, l1, _txtUser, l2, _txtPass, l3, _cmbRol, _btnGuardar, _btnEliminar });
        }

        // Recarga la tabla con los datos actualizados desde la base de datos
        private void RefrescarTabla() => _dgv.DataSource = _service.ListarUsuarios();

        // Evento del botón "Crear Usuario": valida los datos y registra el nuevo usuario en la BD
        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                // Se traduce la selección del ComboBox al ID de rol correspondiente en la BD
                int idRolSeleccionado = 0;
                if (_cmbRol.Text == "Administrador") idRolSeleccionado = 1;
                else if (_cmbRol.Text == "Auditor") idRolSeleccionado = 2;
                else if (_cmbRol.Text == "Cajero") idRolSeleccionado = 3;

                // Se construye el DTO con los datos ingresados en el formulario
                var u = new UsuarioDTO { Usuario = _txtUser.Text, IdRol = idRolSeleccionado };

                // Se delega la lógica de guardado a la capa de negocio
                _service.GuardarUsuario(u, _txtPass.Text);

                MessageBox.Show("Usuario Creado Correctamente");

                // Se limpian los campos para permitir registrar otro usuario
                _txtUser.Clear(); _txtPass.Clear(); _cmbRol.SelectedIndex = -1;

                // Se refresca la tabla para mostrar el nuevo usuario
                RefrescarTabla();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // Evento del botón "Eliminar Seleccionado": elimina el usuario marcado en la tabla
        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (_dgv.SelectedRows.Count > 0)
            {
                try
                {
                    // Se obtiene el ID del usuario desde la celda "Id" de la fila seleccionada
                    int id = Convert.ToInt32(_dgv.SelectedRows[0].Cells["Id"].Value);

                    // Se elimina el usuario a través del servicio y se refresca la tabla
                    _service.EliminarUsuario(id);
                    RefrescarTabla();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
    }
}