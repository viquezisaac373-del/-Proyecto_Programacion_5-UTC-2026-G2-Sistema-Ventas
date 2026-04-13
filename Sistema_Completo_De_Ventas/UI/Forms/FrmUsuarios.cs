using System;
using System.Drawing;
using System.Windows.Forms;
using SistemaVentas.BLL;
using SistemaVentas.DTO;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    public partial class FrmUsuarios : Form
    {
        // Controles de la UI
        private DataGridView _dgvUsuarios = null!;
        private TextBox _txtId = null!;
        private TextBox _txtNombre = null!;
        private TextBox _txtClave = null!;
        private ComboBox _cmbRol = null!;
        private Button _btnGuardar = null!;
        private Button _btnEditar = null!;
        private Button _btnEliminar = null!;
        private Button _btnLimpiar = null!;

        private UsuarioService _usuarioService;

        public FrmUsuarios()
        {
            _usuarioService = new UsuarioService();
            ConstruirInterfaz();
            CargarUsuarios();
        }

        private void ConstruirInterfaz()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;
            this.Text = "Gestión de Usuarios";
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            // Título
            Label lblTitulo = new Label
            {
                Text = "Gestión de Usuarios",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitulo);

            // --- PANEL DERECHO (Formulario) ---
            int formX = 750; // Posición X para la columna de inputs

            CrearLabel("ID:", formX, 60);
            _txtId = CrearTextBox(formX, 85, true); // true = ReadOnly (Solo lectura)

            CrearLabel("Nombre de Usuario:", formX, 120);
            _txtNombre = CrearTextBox(formX, 145, false);

            CrearLabel("Contraseña:", formX, 180);
            _txtClave = CrearTextBox(formX, 205, false);

            CrearLabel("Rol:", formX, 240);
            _cmbRol = new ComboBox
            {
                Location = new Point(formX, 265),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _cmbRol.Items.AddRange(new string[] { "Administrador", "Cajero", "Contador" });
            this.Controls.Add(_cmbRol);

            // Botones
            _btnGuardar = CrearBoton("Guardar", Color.FromArgb(0, 122, 204), formX, 320);
            _btnGuardar.Click += BtnGuardar_Click;

            _btnEditar = CrearBoton("Editar", Color.FromArgb(204, 153, 0), formX, 370);
            _btnEditar.Click += BtnEditar_Click;

            _btnEliminar = CrearBoton("Eliminar", Color.FromArgb(204, 51, 51), formX, 420);
            _btnEliminar.Click += BtnEliminar_Click;

            _btnLimpiar = CrearBoton("Limpiar", Color.Transparent, formX, 470);
            _btnLimpiar.FlatAppearance.BorderSize = 1;
            _btnLimpiar.FlatAppearance.BorderColor = Color.White;
            _btnLimpiar.Click += BtnLimpiar_Click;

            // --- PANEL IZQUIERDO (Tabla DataGridView) ---
            _dgvUsuarios = new DataGridView
            {
                Location = new Point(20, 70),
                Size = new Size(700, 450),
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.FromArgb(30, 30, 30),
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Estilos oscuros/azules para la tabla
            _dgvUsuarios.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            _dgvUsuarios.DefaultCellStyle.ForeColor = Color.White;
            _dgvUsuarios.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            _dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 122, 204);
            _dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            this.Controls.Add(_dgvUsuarios);
        }

        private void CargarUsuarios()
        {
            try
            {
                _dgvUsuarios.DataSource = _usuarioService.ListarUsuarios();

                // Ocultar la columna de contraseña en la tabla por seguridad
                if (_dgvUsuarios.Columns.Contains("Clave"))
                    _dgvUsuarios.Columns["Clave"].Visible = false;
            }
            catch (Exception ex)
            {
                // Mostramos error solo si no es el primer arranque sin métodos
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                UsuarioDTO usuario = new UsuarioDTO
                {
                    IdUsuario = string.IsNullOrEmpty(_txtId.Text) ? 0 : int.Parse(_txtId.Text),
                    NombreUsuario = _txtNombre.Text,
                    Clave = _txtClave.Text,
                    Rol = _cmbRol.Text
                };

                _usuarioService.GuardarUsuario(usuario);
                MessageBox.Show("Usuario guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarUsuarios();
                LimpiarCampos();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void BtnEditar_Click(object? sender, EventArgs e)
        {
            if (_dgvUsuarios.SelectedRows.Count > 0)
            {
                _txtId.Text = _dgvUsuarios.SelectedRows[0].Cells["IdUsuario"].Value.ToString();
                _txtNombre.Text = _dgvUsuarios.SelectedRows[0].Cells["NombreUsuario"].Value.ToString();
                _cmbRol.Text = _dgvUsuarios.SelectedRows[0].Cells["Rol"].Value.ToString();
                _txtClave.Text = ""; // Dejar vacío para escribir una nueva
                _txtClave.Focus();
                MessageBox.Show("Escriba la nueva contraseña y presione Guardar.", "Modo Edición", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Seleccione un usuario de la lista para editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (_dgvUsuarios.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("¿Está seguro de eliminar este usuario?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        int id = Convert.ToInt32(_dgvUsuarios.SelectedRows[0].Cells["IdUsuario"].Value);
                        _usuarioService.EliminarUsuario(id);
                        CargarUsuarios();
                        LimpiarCampos();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un usuario de la lista para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnLimpiar_Click(object? sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            _txtId.Clear();
            _txtNombre.Clear();
            _txtClave.Clear();
            _cmbRol.SelectedIndex = -1;
            _txtNombre.Focus();
        }

        // Métodos para crear controles visuales
        private void CrearLabel(string texto, int x, int y)
        {
            Label lbl = new Label { Text = texto, Location = new Point(x, y), AutoSize = true, ForeColor = Color.DarkGray };
            this.Controls.Add(lbl);
        }

        private TextBox CrearTextBox(int x, int y, bool isReadOnly)
        {
            TextBox txt = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(200, 25),
                BackColor = isReadOnly ? Color.FromArgb(45, 45, 48) : Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = isReadOnly
            };
            this.Controls.Add(txt);
            return txt;
        }

        private Button CrearBoton(string texto, Color backColor, int x, int y)
        {
            Button btn = new Button
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(200, 35),
                BackColor = backColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btn);
            return btn;
        }
    }
}