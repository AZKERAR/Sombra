using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SOMBRA.PL;

namespace SOMBRA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ConfigurarPermisos();
        }

        private void ConfigurarPermisos()
        {
            int nivelUsuario = Program.UsuarioActual.Nivel;

            if (nivelUsuario == 1) // Usuario (solo ver)
            {
                btnDepartamentos.Enabled = false;
                btnEmpleados.Enabled = false;
                btnAgregarUsuario.Enabled = false;
            }
            else if (nivelUsuario == 2) // Gerente (agregar y modificar)
            {
                btnDepartamentos.Enabled = true;
                btnEmpleados.Enabled = true;
                btnAgregarUsuario.Enabled = false; // Los gerentes no pueden agregar usuarios
            }
            else if (nivelUsuario == 3) // Administrador (todos los permisos)
            {
                btnDepartamentos.Enabled = true;
                btnEmpleados.Enabled = true;
                btnAgregarUsuario.Enabled = true;
            }
        }

        private void btnDepartamentos_Click(object sender, EventArgs e)
        {
            frmDepartamentos formularioDepartamentos = new frmDepartamentos();
            formularioDepartamentos.Show();
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            frmEmpleados formularioEmpleados = new frmEmpleados();
            formularioEmpleados.Show();
        }

        private void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            if (Program.UsuarioActual.Nivel < 3)
            {
                MessageBox.Show("Tu nivel es muy bajo para realizar esta acción.");
                return;
            }

            frmAgregarUsuario formularioAgregarUsuario = new frmAgregarUsuario();
            formularioAgregarUsuario.Show();
        }

        // El botón cerrar sesión se puede eliminar si ya no es necesario
        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
