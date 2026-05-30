using System;
using System.Windows.Forms;
using RegistroEstudiantes.Datos;
using RegistroEstudiantes.Modelos;

namespace RegistroEstudiantes
{
    public partial class FrmLogin : Form
    {
        public Usuario? UsuarioActual { get; private set; }

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtClave.Text))
            {
                MessageBox.Show("Ingrese usuario y contraseña.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = UsuarioRepository.ValidarUsuario(txtUsuario.Text.Trim(), txtClave.Text.Trim());
            if (user != null)
            {
                UsuarioActual = user;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtClave.Clear();
                txtClave.Focus();
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            var frm = new FrmRegistro();
            frm.ShowDialog(this);
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblUsuario = new Label();
            lblClave = new Label();
            txtUsuario = new TextBox();
            txtClave = new TextBox();
            btnIngresar = new Button();
            btnRegistrar = new Button();
            SuspendLayout();

            // lblTitulo
            lblTitulo.Text = "Sistema de Registro de Estudiantes";
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            lblTitulo.Location = new Point(30, 20);
            lblTitulo.Size = new Size(340, 30);
            lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblUsuario
            lblUsuario.Text = "Usuario:";
            lblUsuario.Location = new Point(60, 75);
            lblUsuario.Size = new Size(80, 20);

            // txtUsuario
            txtUsuario.Location = new Point(150, 72);
            txtUsuario.Size = new Size(180, 23);

            // lblClave
            lblClave.Text = "Contraseña:";
            lblClave.Location = new Point(60, 115);
            lblClave.Size = new Size(80, 20);

            // txtClave
            txtClave.Location = new Point(150, 112);
            txtClave.Size = new Size(180, 23);
            txtClave.PasswordChar = '*';

            // btnIngresar
            btnIngresar.Text = "Ingresar";
            btnIngresar.Location = new Point(100, 160);
            btnIngresar.Size = new Size(110, 35);
            btnIngresar.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            btnIngresar.ForeColor = System.Drawing.Color.White;
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.Click += btnIngresar_Click;

            // btnRegistrar
            btnRegistrar.Text = "Crear cuenta";
            btnRegistrar.Location = new Point(220, 160);
            btnRegistrar.Size = new Size(110, 35);
            btnRegistrar.BackColor = System.Drawing.Color.FromArgb(50, 160, 50);
            btnRegistrar.ForeColor = System.Drawing.Color.White;
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.Click += btnRegistrar_Click;

            // FrmLogin
            ClientSize = new Size(400, 230);
            Text = "Iniciar Sesión";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = System.Drawing.Color.WhiteSmoke;
            Controls.Add(lblTitulo);
            Controls.Add(lblUsuario);
            Controls.Add(txtUsuario);
            Controls.Add(lblClave);
            Controls.Add(txtClave);
            Controls.Add(btnIngresar);
            Controls.Add(btnRegistrar);
            Name = "FrmLogin";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitulo;
        private Label lblUsuario;
        private Label lblClave;
        private TextBox txtUsuario;
        private TextBox txtClave;
        private Button btnIngresar;
        private Button btnRegistrar;
    }
}