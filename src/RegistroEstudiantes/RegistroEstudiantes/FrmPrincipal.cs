using System;
using System.Windows.Forms;
using RegistroEstudiantes.Modelos;

namespace RegistroEstudiantes
{
    public partial class FrmPrincipal : Form
    {
        private Usuario _usuarioActual;

        public FrmPrincipal(Usuario usuario)
        {
            _usuarioActual = usuario;
            InitializeComponent();
            lblBienvenido.Text = $"Bienvenido, {_usuarioActual.NombreCompleto} ({_usuarioActual.Rol})";
        }

        private void btnEstudiantes_Click(object sender, EventArgs e)
        {
            var frm = new FrmEstudiantes(_usuarioActual);
            frm.ShowDialog(this);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar sesión?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblBienvenido = new Label();
            btnEstudiantes = new Button();
            btnSalir = new Button();
            SuspendLayout();

            // lblTitulo
            lblTitulo.Text = "Sistema de Registro de Estudiantes";
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            lblTitulo.Location = new Point(30, 20);
            lblTitulo.Size = new Size(440, 30);
            lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblTitulo.ForeColor = System.Drawing.Color.FromArgb(0, 80, 160);

            // lblBienvenido
            lblBienvenido.Text = "";
            lblBienvenido.Font = new System.Drawing.Font("Segoe UI", 10);
            lblBienvenido.Location = new Point(30, 60);
            lblBienvenido.Size = new Size(440, 20);
            lblBienvenido.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblBienvenido.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);

            // btnEstudiantes
            btnEstudiantes.Text = " Gestión de Estudiantes";
            btnEstudiantes.Location = new Point(130, 110);
            btnEstudiantes.Size = new Size(240, 50);
            btnEstudiantes.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            btnEstudiantes.ForeColor = System.Drawing.Color.White;
            btnEstudiantes.FlatStyle = FlatStyle.Flat;
            btnEstudiantes.Font = new System.Drawing.Font("Segoe UI", 11);
            btnEstudiantes.Click += btnEstudiantes_Click;

            // btnSalir
            btnSalir.Text = " Cerrar Sesión";
            btnSalir.Location = new Point(130, 180);
            btnSalir.Size = new Size(240, 50);
            btnSalir.BackColor = System.Drawing.Color.FromArgb(200, 50, 50);
            btnSalir.ForeColor = System.Drawing.Color.White;
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.Font = new System.Drawing.Font("Segoe UI", 11);
            btnSalir.Click += btnSalir_Click;

            // FrmPrincipal
            ClientSize = new Size(500, 280);
            Text = "Menú Principal";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = System.Drawing.Color.WhiteSmoke;
            Controls.Add(lblTitulo);
            Controls.Add(lblBienvenido);
            Controls.Add(btnEstudiantes);
            Controls.Add(btnSalir);
            Name = "FrmPrincipal";
            ResumeLayout(false);
        }

        private Label lblTitulo;
        private Label lblBienvenido;
        private Button btnEstudiantes;
        private Button btnSalir;
    }
}
