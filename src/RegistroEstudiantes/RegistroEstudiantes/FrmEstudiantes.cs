using System;
using System.Drawing;
using System.Windows.Forms;
using RegistroEstudiantes.Datos;
using RegistroEstudiantes.Modelos;

namespace RegistroEstudiantes
{
    public partial class FrmEstudiantes : Form
    {
        private Usuario _usuarioActual;
        private ArbolBST _arbol = new ArbolBST();
        private Estudiante? _estudianteSeleccionado = null;

        public FrmEstudiantes(Usuario usuario)
        {
            _usuarioActual = usuario;
            InitializeComponent();
            CargarCarreras();
            CargarEstudiantes();
        }

        private void CargarCarreras()
        {
            cmbCarrera.DataSource = CarreraRepository.ObtenerTodas();
            cmbCarrera.DisplayMember = "NombreCarrera";
            cmbCarrera.ValueMember = "IdCarrera";
        }

        private void CargarEstudiantes()
        {
            var lista = EstudianteRepository.ObtenerTodos();
            _arbol.CargarDesdelista(lista);
            dgvEstudiantes.DataSource = null;
            dgvEstudiantes.DataSource = _arbol.ObtenerInorden();
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txtCarnet.Text = "";
            txtNombres.Text = "";
            txtApellidos.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            txtBuscar.Text = "";
            _estudianteSeleccionado = null;
            btnBaja.Enabled = false;
            btnModificar.Enabled = false;
            if (cmbCarrera.Items.Count > 0) cmbCarrera.SelectedIndex = 0;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCarnet.Text) ||
                string.IsNullOrWhiteSpace(txtNombres.Text) ||
                string.IsNullOrWhiteSpace(txtApellidos.Text))
            {
                MessageBox.Show("Carnet, Nombres y Apellidos son obligatorios.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var est = new Estudiante
            {
                Carnet = txtCarnet.Text.Trim(),
                Nombres = txtNombres.Text.Trim(),
                Apellidos = txtApellidos.Text.Trim(),
                IdCarrera = (int)cmbCarrera.SelectedValue!,
                Correo = txtCorreo.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Activo = true
            };

            bool ok = EstudianteRepository.Agregar(est, _usuarioActual.IdUsuario);
            if (ok)
            {
                MessageBox.Show("Estudiante guardado exitosamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarEstudiantes();
            }
            else
                MessageBox.Show("Error al guardar. ¿Carnet repetido?", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (_estudianteSeleccionado == null) return;

            if (string.IsNullOrWhiteSpace(txtCarnet.Text) ||
                string.IsNullOrWhiteSpace(txtNombres.Text) ||
                string.IsNullOrWhiteSpace(txtApellidos.Text))
            {
                MessageBox.Show("Carnet, Nombres y Apellidos son obligatorios.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _estudianteSeleccionado.Carnet = txtCarnet.Text.Trim();
            _estudianteSeleccionado.Nombres = txtNombres.Text.Trim();
            _estudianteSeleccionado.Apellidos = txtApellidos.Text.Trim();
            _estudianteSeleccionado.IdCarrera = (int)cmbCarrera.SelectedValue!;
            _estudianteSeleccionado.Correo = txtCorreo.Text.Trim();
            _estudianteSeleccionado.Telefono = txtTelefono.Text.Trim();

            bool ok = EstudianteRepository.Modificar(_estudianteSeleccionado, _usuarioActual.IdUsuario);
            if (ok)
            {
                MessageBox.Show("Estudiante modificado exitosamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarEstudiantes();
            }
            else
                MessageBox.Show("Error al modificar estudiante.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            if (_estudianteSeleccionado == null) return;

            if (MessageBox.Show($"¿Dar de baja a {_estudianteSeleccionado.Nombres} {_estudianteSeleccionado.Apellidos}?",
                "Confirmar baja", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                bool ok = EstudianteRepository.Baja(_estudianteSeleccionado.IdEstudiante, _usuarioActual.IdUsuario);
                if (ok)
                {
                    MessageBox.Show("Estudiante dado de baja exitosamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarEstudiantes();
                }
                else
                    MessageBox.Show("Error al dar de baja.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string termino = txtBuscar.Text.Trim();
            if (string.IsNullOrWhiteSpace(termino))
            {
                CargarEstudiantes();
                return;
            }

            var resultado = _arbol.BuscarPorCarnet(termino);
            if (resultado != null)
            {
                dgvEstudiantes.DataSource = null;
                dgvEstudiantes.DataSource = _arbol.ObtenerInorden();

                // Resaltar fila encontrada en amarillo
                foreach (DataGridViewRow row in dgvEstudiantes.Rows)
                {
                    if (row.Cells["Carnet"].Value?.ToString() == resultado.Carnet)
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                        dgvEstudiantes.FirstDisplayedScrollingRowIndex = row.Index;
                        dgvEstudiantes.ClearSelection();
                        row.Selected = true;
                        break;
                    }
                }
            }
            else
                MessageBox.Show("No se encontró ningún estudiante con ese carnet.", "Búsqueda",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            CargarEstudiantes();
        }

        private void btnVerBajas_Click(object sender, EventArgs e)
        {
            var bajas = EstudianteRepository.ObtenerBajas();
            dgvEstudiantes.DataSource = null;
            dgvEstudiantes.DataSource = bajas;

            // Colorear filas de bajas en rojo claro
            foreach (DataGridViewRow row in dgvEstudiantes.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220);
                row.DefaultCellStyle.ForeColor = Color.Black;
            }

            LimpiarFormulario();
            MessageBox.Show($"Se encontraron {bajas.Count} estudiante(s) de baja.", "Alumnos de Baja",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnMovimientos_Click(object sender, EventArgs e)
        {
            var frm = new FrmMovimientos();
            frm.ShowDialog(this);
        }

        private void dgvEstudiantes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvEstudiantes.Rows[e.RowIndex];

            if (row.Cells["IdEstudiante"].Value == null) return;

            _estudianteSeleccionado = new Estudiante
            {
                IdEstudiante = (int)row.Cells["IdEstudiante"].Value,
                Carnet = row.Cells["Carnet"].Value.ToString()!,
                Nombres = row.Cells["Nombres"].Value.ToString()!,
                Apellidos = row.Cells["Apellidos"].Value.ToString()!,
                IdCarrera = (int)row.Cells["IdCarrera"].Value,
                Correo = row.Cells["Correo"].Value.ToString()!,
                Telefono = row.Cells["Telefono"].Value.ToString()!,
            };

            txtCarnet.Text = _estudianteSeleccionado.Carnet;
            txtNombres.Text = _estudianteSeleccionado.Nombres;
            txtApellidos.Text = _estudianteSeleccionado.Apellidos;
            txtCorreo.Text = _estudianteSeleccionado.Correo;
            txtTelefono.Text = _estudianteSeleccionado.Telefono;
            cmbCarrera.SelectedValue = _estudianteSeleccionado.IdCarrera;
            btnBaja.Enabled = true;
            btnModificar.Enabled = true;
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblCarnet = new Label();
            lblNombres = new Label();
            lblApellidos = new Label();
            lblCorreo = new Label();
            lblTelefono = new Label();
            lblCarrera = new Label();
            lblBuscar = new Label();
            txtCarnet = new TextBox();
            txtNombres = new TextBox();
            txtApellidos = new TextBox();
            txtCorreo = new TextBox();
            txtTelefono = new TextBox();
            txtBuscar = new TextBox();
            cmbCarrera = new ComboBox();
            dgvEstudiantes = new DataGridView();
            btnGuardar = new Button();
            btnModificar = new Button();
            btnBaja = new Button();
            btnBuscar = new Button();
            btnLimpiar = new Button();
            btnVerBajas = new Button();
            btnMovimientos = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvEstudiantes).BeginInit();
            SuspendLayout();

            // lblTitulo
            lblTitulo.Text = "Gestión de Estudiantes";
            lblTitulo.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitulo.Location = new Point(10, 10);
            lblTitulo.Size = new Size(280, 28);
            lblTitulo.ForeColor = Color.FromArgb(0, 80, 160);

            // Labels
            lblCarnet.Text = "Carnet:";
            lblCarnet.Location = new Point(15, 55);
            lblCarnet.Size = new Size(80, 20);

            lblNombres.Text = "Nombres:";
            lblNombres.Location = new Point(15, 90);
            lblNombres.Size = new Size(80, 20);

            lblApellidos.Text = "Apellidos:";
            lblApellidos.Location = new Point(15, 125);
            lblApellidos.Size = new Size(80, 20);

            lblCorreo.Text = "Correo:";
            lblCorreo.Location = new Point(15, 160);
            lblCorreo.Size = new Size(80, 20);

            lblTelefono.Text = "Teléfono:";
            lblTelefono.Location = new Point(15, 195);
            lblTelefono.Size = new Size(80, 20);

            lblCarrera.Text = "Carrera:";
            lblCarrera.Location = new Point(15, 230);
            lblCarrera.Size = new Size(80, 20);

            // TextBoxes
            txtCarnet.Location = new Point(100, 52);
            txtCarnet.Size = new Size(160, 23);

            txtNombres.Location = new Point(100, 87);
            txtNombres.Size = new Size(160, 23);

            txtApellidos.Location = new Point(100, 122);
            txtApellidos.Size = new Size(160, 23);

            txtCorreo.Location = new Point(100, 157);
            txtCorreo.Size = new Size(160, 23);

            txtTelefono.Location = new Point(100, 192);
            txtTelefono.Size = new Size(160, 23);

            // cmbCarrera
            cmbCarrera.Location = new Point(100, 227);
            cmbCarrera.Size = new Size(160, 23);
            cmbCarrera.DropDownStyle = ComboBoxStyle.DropDownList;

            // Botones acción
            btnGuardar.Text = "  Guardar";
            btnGuardar.Location = new Point(15, 270);
            btnGuardar.Size = new Size(115, 33);
            btnGuardar.BackColor = Color.FromArgb(0, 120, 215);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Click += btnGuardar_Click;

            btnModificar.Text = "  Modificar";
            btnModificar.Location = new Point(145, 270);
            btnModificar.Size = new Size(115, 33);
            btnModificar.BackColor = Color.FromArgb(255, 140, 0);
            btnModificar.ForeColor = Color.White;
            btnModificar.FlatStyle = FlatStyle.Flat;
            btnModificar.Enabled = false;
            btnModificar.Click += btnModificar_Click;

            btnBaja.Text = "  Dar de Baja";
            btnBaja.Location = new Point(15, 313);
            btnBaja.Size = new Size(115, 33);
            btnBaja.BackColor = Color.FromArgb(200, 50, 50);
            btnBaja.ForeColor = Color.White;
            btnBaja.FlatStyle = FlatStyle.Flat;
            btnBaja.Enabled = false;
            btnBaja.Click += btnBaja_Click;

            // Buscar
            lblBuscar.Text = "Buscar carnet:";
            lblBuscar.Location = new Point(15, 365);
            lblBuscar.Size = new Size(90, 20);

            txtBuscar.Location = new Point(110, 362);
            txtBuscar.Size = new Size(150, 23);

            btnBuscar.Text = " Buscar";
            btnBuscar.Location = new Point(15, 395);
            btnBuscar.Size = new Size(115, 32);
            btnBuscar.BackColor = Color.FromArgb(50, 160, 50);
            btnBuscar.ForeColor = Color.White;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Click += btnBuscar_Click;

            btnLimpiar.Text = "Mostrar todos";
            btnLimpiar.Location = new Point(145, 395);
            btnLimpiar.Size = new Size(115, 32);
            btnLimpiar.BackColor = Color.FromArgb(100, 100, 100);
            btnLimpiar.ForeColor = Color.White;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Click += btnLimpiar_Click;

            btnVerBajas.Text = " Ver Bajas";
            btnVerBajas.Location = new Point(15, 437);
            btnVerBajas.Size = new Size(115, 32);
            btnVerBajas.BackColor = Color.FromArgb(180, 50, 50);
            btnVerBajas.ForeColor = Color.White;
            btnVerBajas.FlatStyle = FlatStyle.Flat;
            btnVerBajas.Click += btnVerBajas_Click;

            btnMovimientos.Text = " Movimientos";
            btnMovimientos.Location = new Point(145, 437);
            btnMovimientos.Size = new Size(115, 32);
            btnMovimientos.BackColor = Color.FromArgb(0, 80, 160);
            btnMovimientos.ForeColor = Color.White;
            btnMovimientos.FlatStyle = FlatStyle.Flat;
            btnMovimientos.Click += btnMovimientos_Click;

            // dgvEstudiantes
            dgvEstudiantes.Location = new Point(280, 50);
            dgvEstudiantes.Size = new Size(580, 420);
            dgvEstudiantes.ReadOnly = true;
            dgvEstudiantes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEstudiantes.MultiSelect = false;
            dgvEstudiantes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEstudiantes.BackgroundColor = Color.White;
            dgvEstudiantes.BorderStyle = BorderStyle.Fixed3D;
            dgvEstudiantes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 80, 160);
            dgvEstudiantes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEstudiantes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvEstudiantes.EnableHeadersVisualStyles = false;
            dgvEstudiantes.CellClick += dgvEstudiantes_CellClick;

            // FrmEstudiantes
            ClientSize = new Size(880, 500);
            Text = "Gestión de Estudiantes";
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.WhiteSmoke;
            Controls.Add(lblTitulo);
            Controls.Add(lblCarnet);
            Controls.Add(lblNombres);
            Controls.Add(lblApellidos);
            Controls.Add(lblCorreo);
            Controls.Add(lblTelefono);
            Controls.Add(lblCarrera);
            Controls.Add(lblBuscar);
            Controls.Add(txtCarnet);
            Controls.Add(txtNombres);
            Controls.Add(txtApellidos);
            Controls.Add(txtCorreo);
            Controls.Add(txtTelefono);
            Controls.Add(txtBuscar);
            Controls.Add(cmbCarrera);
            Controls.Add(btnGuardar);
            Controls.Add(btnModificar);
            Controls.Add(btnBaja);
            Controls.Add(btnBuscar);
            Controls.Add(btnLimpiar);
            Controls.Add(btnVerBajas);
            Controls.Add(btnMovimientos);
            Controls.Add(dgvEstudiantes);
            Name = "FrmEstudiantes";
            ((System.ComponentModel.ISupportInitialize)dgvEstudiantes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitulo;
        private Label lblCarnet;
        private Label lblNombres;
        private Label lblApellidos;
        private Label lblCorreo;
        private Label lblTelefono;
        private Label lblCarrera;
        private Label lblBuscar;
        private TextBox txtCarnet;
        private TextBox txtNombres;
        private TextBox txtApellidos;
        private TextBox txtCorreo;
        private TextBox txtTelefono;
        private TextBox txtBuscar;
        private ComboBox cmbCarrera;
        private DataGridView dgvEstudiantes;
        private Button btnGuardar;
        private Button btnModificar;
        private Button btnBaja;
        private Button btnBuscar;
        private Button btnLimpiar;
        private Button btnVerBajas;
        private Button btnMovimientos;
    }
}