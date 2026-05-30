using System;
using System.Drawing;
using System.Windows.Forms;
using RegistroEstudiantes.Datos;
using RegistroEstudiantes.Modelos;

namespace RegistroEstudiantes
{
    public class FrmMovimientos : Form
    {
        private DataGridView dgvMovimientos;
        private Label lblTitulo;
        private ComboBox cmbFiltro;
        private Label lblFiltro;
        private Button btnRefrescar;

        public FrmMovimientos()
        {
            InitializeComponent();
            CargarMovimientos();
        }

        private void CargarMovimientos()
        {
            string filtro = cmbFiltro.SelectedItem?.ToString() ?? "Todos";
            var lista = MovimientoRepository.ObtenerTodos();

            if (filtro != "Todos")
                lista = lista.FindAll(m => m.TipoMovimiento == filtro);

            dgvMovimientos.DataSource = null;
            dgvMovimientos.DataSource = lista;

            // Colorear filas según tipo de movimiento
            foreach (DataGridViewRow row in dgvMovimientos.Rows)
            {
                if (row.Cells["TipoMovimiento"].Value == null) continue;
                string tipo = row.Cells["TipoMovimiento"].Value.ToString()!;
                row.DefaultCellStyle.BackColor = tipo switch
                {
                    "Alta" => Color.FromArgb(220, 255, 220),
                    "Baja" => Color.FromArgb(255, 220, 220),
                    "Actualizacion" => Color.FromArgb(255, 255, 200),
                    _ => Color.White
                };
            }
        }

        private void cmbFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarMovimientos();
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarMovimientos();
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblFiltro = new Label();
            cmbFiltro = new ComboBox();
            btnRefrescar = new Button();
            dgvMovimientos = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvMovimientos).BeginInit();
            SuspendLayout();

            // lblTitulo
            lblTitulo.Text = "Historial de Movimientos";
            lblTitulo.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitulo.Location = new Point(15, 15);
            lblTitulo.Size = new Size(350, 28);
            lblTitulo.ForeColor = Color.FromArgb(0, 80, 160);

            // lblFiltro
            lblFiltro.Text = "Filtrar por:";
            lblFiltro.Location = new Point(15, 55);
            lblFiltro.Size = new Size(80, 20);

            // cmbFiltro
            cmbFiltro.Location = new Point(100, 52);
            cmbFiltro.Size = new Size(150, 23);
            cmbFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltro.Items.Add("Todos");
            cmbFiltro.Items.Add("Alta");
            cmbFiltro.Items.Add("Actualizacion");
            cmbFiltro.Items.Add("Baja");
            cmbFiltro.SelectedIndex = 0;
            cmbFiltro.SelectedIndexChanged += cmbFiltro_SelectedIndexChanged;

            // btnRefrescar
            btnRefrescar.Text = "🔄 Refrescar";
            btnRefrescar.Location = new Point(265, 48);
            btnRefrescar.Size = new Size(110, 30);
            btnRefrescar.BackColor = Color.FromArgb(0, 120, 215);
            btnRefrescar.ForeColor = Color.White;
            btnRefrescar.FlatStyle = FlatStyle.Flat;
            btnRefrescar.Click += btnRefrescar_Click;

            // dgvMovimientos
            dgvMovimientos.Location = new Point(15, 95);
            dgvMovimientos.Size = new Size(770, 400);
            dgvMovimientos.ReadOnly = true;
            dgvMovimientos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMovimientos.MultiSelect = false;
            dgvMovimientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMovimientos.BackgroundColor = Color.White;
            dgvMovimientos.BorderStyle = BorderStyle.Fixed3D;
            dgvMovimientos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 80, 160);
            dgvMovimientos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMovimientos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvMovimientos.EnableHeadersVisualStyles = false;

            // FrmMovimientos
            ClientSize = new Size(800, 520);
            Text = "Historial de Movimientos";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.WhiteSmoke;
            Controls.Add(lblTitulo);
            Controls.Add(lblFiltro);
            Controls.Add(cmbFiltro);
            Controls.Add(btnRefrescar);
            Controls.Add(dgvMovimientos);
            Name = "FrmMovimientos";
            ((System.ComponentModel.ISupportInitialize)dgvMovimientos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}