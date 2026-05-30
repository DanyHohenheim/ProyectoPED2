using System;
using System.Windows.Forms;
using RegistroEstudiantes.Datos;
using RegistroEstudiantes.Modelos;

namespace RegistroEstudiantes
{
    public class FrmRegistro : Form
    {
        private TextBox txtNombreCompleto;
        private TextBox txtUsuario;
        private TextBox txtClave;
        private TextBox txtConfirmarClave;
        private ComboBox cmbRol;
        private Button btnRegistrar;
        private Button btnCancelar;
        private Label lblNombreCompleto;
        private Label lblUsuario;
        private Label lblClave;
        private Label lblConfirmarClave;
        private Label lblRol;
        private Label lblTitulo;

        public FrmRegistro()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblNombreCompleto = new Label();
            lblUsuario = new Label();
            lblClave = new Label();
            lblConfirmarClave = new Label();
            lblRol = new Label();
            txtNombreCompleto = new TextBox();
            txtUsuario = new TextBox();
            txtClave = new TextBox();
            txtConfirmarClave = new TextBox();
            cmbRol = new ComboBox();
            btnRegistrar = new Button();
            btnCancelar = new Button();
            SuspendLayout();

            // lblTitulo
            lblTitulo.Text = "Registro de Usuario";
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            lblTitulo.Location = new Point(60, 20);
            lblTitulo.Size = new Size(280, 30);
            lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblNombreCompleto
            lblNombreCompleto.Text = "Nombre Completo:";
            lblNombreCompleto.Location = new Point(30, 70);
            lblNombreCompleto.Size = new Size(130, 20);

            // txtNombreCompleto
            txtNombreCompleto.Location = new Point(170, 68);
            txtNombreCompleto.Size = new Size(180, 23);

            // lblUsuario
            lblUsuario.Text = "Usuario:";
            lblUsuario.Location = new Point(30, 105);
            lblUsuario.Size = new Size(130, 20);

            // txtUsuario
            txtUsuario.Location = new Point(170, 103);
            txtUsuario.Size = new Size(180, 23);

            // lblClave
            lblClave.Text = "Contraseña:";
            lblClave.Location = new Point(30, 140);
            lblClave.Size = new Size(130, 20);

            // txtClave
            txtClave.Location = new Point(170, 138);
            txtClave.Size = new Size(180, 23);
            txtClave.PasswordChar = '*';

            // lblConfirmarClave
            lblConfirmarClave.Text = "Confirmar Contraseña:";
            lblConfirmarClave.Location = new Point(30, 175);
            lblConfirmarClave.Size = new Size(130, 20);

            // txtConfirmarClave
            txtConfirmarClave.Location = new Point(170, 173);
            txtConfirmarClave.Size = new Size(180, 23);
            txtConfirmarClave.PasswordChar = '*';

            // lblRol
            lblRol.Text = "Rol:";
            lblRol.Location = new Point(30, 210);
            lblRol.Size = new Size(130, 20);

            // cmbRol
            cmbRol.Location = new Point(170, 208);
            cmbRol.Size = new Size(180, 23);
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.Items.Add("Administrador");
            cmbRol.Items.Add("Operador");
            cmbRol.SelectedIndex = 1;

            // btnRegistrar
            btnRegistrar.Text = "Registrar";
            btnRegistrar.Location = new Point(100, 255);
            btnRegistrar.Size = new Size(100, 35);
            btnRegistrar.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            btnRegistrar.ForeColor = System.Drawing.Color.White;
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.Click += btnRegistrar_Click;

            // btnCancelar
            btnCancelar.Text = "Cancelar";
            btnCancelar.Location = new Point(215, 255);
            btnCancelar.Size = new Size(100, 35);
            btnCancelar.BackColor = System.Drawing.Color.FromArgb(200, 50, 50);
            btnCancelar.ForeColor = System.Drawing.Color.White;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Click += (s, e) => Close();

            // FrmRegistro
            ClientSize = new Size(400, 320);
            Text = "Registro de Usuario";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = System.Drawing.Color.WhiteSmoke;
            Controls.Add(lblTitulo);
            Controls.Add(lblNombreCompleto);
            Controls.Add(txtNombreCompleto);
            Controls.Add(lblUsuario);
            Controls.Add(txtUsuario);
            Controls.Add(lblClave);
            Controls.Add(txtClave);
            Controls.Add(lblConfirmarClave);
            Controls.Add(txtConfirmarClave);
            Controls.Add(lblRol);
            Controls.Add(cmbRol);
            Controls.Add(btnRegistrar);
            Controls.Add(btnCancelar);
            ResumeLayout(false);
            PerformLayout();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreCompleto.Text) ||
                string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtClave.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtClave.Text != txtConfirmarClave.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtClave.Clear();
                txtConfirmarClave.Clear();
                txtClave.Focus();
                return;
            }

            if (UsuarioRepository.ExisteUsuario(txtUsuario.Text.Trim()))
            {
                MessageBox.Show("El nombre de usuario ya existe.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();
                return;
            }

            var nuevo = new Usuario
            {
                NombreCompleto = txtNombreCompleto.Text.Trim(),
                NombreUsuario = txtUsuario.Text.Trim(),
                ClaveHash = txtClave.Text.Trim(),
                Rol = cmbRol.SelectedItem!.ToString()!
            };

            if (UsuarioRepository.RegistrarUsuario(nuevo))
            {
                MessageBox.Show("Usuario registrado exitosamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                MessageBox.Show("Error al registrar el usuario.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}