using System;
using System.Drawing;
using System.Windows.Forms;
using SistemaVentas.BLL;
using SistemaVentas.DTO;

namespace Sistema_Completo_De_Ventas.UI.Forms
{
    public class FrmUsuarios : Form
    {
        private DataGridView _dgv = null!;
        private TextBox _txtUser = null!, _txtPass = null!;
        private ComboBox _cmbRol = null!;
        private Button _btnGuardar = null!, _btnEliminar = null!;
        private UsuarioService _service = new UsuarioService();

        public FrmUsuarios()
        {
            ConstruirInterfaz();
            RefrescarTabla();
        }

        private void ConstruirInterfaz()
        {
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;

            _dgv = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(600, 400),
                BackgroundColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Black,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true
            };

            Label l1 = new Label { Text = "Usuario:", Location = new Point(640, 20) };
            _txtUser = new TextBox { Location = new Point(640, 45), Size = new Size(200, 25) };

            Label l2 = new Label { Text = "Contraseña:", Location = new Point(640, 80) };
            _txtPass = new TextBox { Location = new Point(640, 105), Size = new Size(200, 25), UseSystemPasswordChar = true };

            Label l3 = new Label { Text = "Rol:", Location = new Point(640, 140) };
            _cmbRol = new ComboBox { Location = new Point(640, 165), Size = new Size(200, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbRol.Items.AddRange(new string[] { "Administrador", "Auditor", "Cajero" });

            _btnGuardar = new Button { Text = "Crear Usuario", Location = new Point(640, 210), Size = new Size(200, 40), BackColor = Color.DarkCyan };
            _btnGuardar.Click += BtnGuardar_Click;

            _btnEliminar = new Button { Text = "Eliminar Seleccionado", Location = new Point(640, 260), Size = new Size(200, 40), BackColor = Color.Brown };
            _btnEliminar.Click += BtnEliminar_Click;

            this.Controls.AddRange(new Control[] { _dgv, l1, _txtUser, l2, _txtPass, l3, _cmbRol, _btnGuardar, _btnEliminar });
        }

        private void RefrescarTabla() => _dgv.DataSource = _service.ListarUsuarios();

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                // Asignamos el ID de rol basado en la selección (1=Admin, 2=Auditor, 3=Cajero según tu BD)
                int idRolSeleccionado = 0;
                if (_cmbRol.Text == "Administrador") idRolSeleccionado = 1;
                else if (_cmbRol.Text == "Auditor") idRolSeleccionado = 2;
                else if (_cmbRol.Text == "Cajero") idRolSeleccionado = 3;

                var u = new UsuarioDTO { Usuario = _txtUser.Text, IdRol = idRolSeleccionado };

                _service.GuardarUsuario(u, _txtPass.Text);

                MessageBox.Show("Usuario Creado Correctamente");
                _txtUser.Clear(); _txtPass.Clear(); _cmbRol.SelectedIndex = -1;
                RefrescarTabla();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (_dgv.SelectedRows.Count > 0)
            {
                try
                {
                    int id = Convert.ToInt32(_dgv.SelectedRows[0].Cells["Id"].Value);
                    _service.EliminarUsuario(id);
                    RefrescarTabla();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
    }
}