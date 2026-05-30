using System;
using System.Windows.Forms;

namespace RegistroEstudiantes
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var login = new FrmLogin())
            {
                if (login.ShowDialog() == DialogResult.OK)
                    Application.Run(new FrmPrincipal(login.UsuarioActual!));
            }
        }
    }
}