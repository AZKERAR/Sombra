using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SOMBRA.BLL;
using SOMBRA.DAL;

namespace SOMBRA.PL
{
    public partial class frmDepartamentos : Form
    {
        DepartamentosDAL oDepartamentosDAL;

        public frmDepartamentos()
        {
            oDepartamentosDAL = new DepartamentosDAL();
            InitializeComponent();
            LLegarGrid();
            LimpiarEntradas();
            ConfigurarPermisos();
        }

        private void ConfigurarPermisos()
        {
            int nivelUsuario = Program.UsuarioActual.Nivel;

            if (nivelUsuario == 1) // Usuario (solo ver)
            {
                btnAgregar.Enabled = false;
                btnModificar.Enabled = false;
                btnBorrar.Enabled = false;
            }
            else if (nivelUsuario == 2) // Gerente (agregar y modificar)
            {
                btnAgregar.Enabled = true;
                btnModificar.Enabled = true;
                btnBorrar.Enabled = false;
            }
            else if (nivelUsuario == 3) // Administrador (todos los permisos)
            {
                btnAgregar.Enabled = true;
                btnModificar.Enabled = true;
                btnBorrar.Enabled = true;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (Program.UsuarioActual.Nivel < 2)
            {
                MessageBox.Show("Tu nivel es muy bajo para realizar esta acción.");
                return;
            }

            oDepartamentosDAL.Agregar(RecuperarInformación());
            LLegarGrid();
            LimpiarEntradas();
        }

        private DepartamentoBLL RecuperarInformación() 
        {
            DepartamentoBLL oDepartamentoBLL = new DepartamentoBLL();

            int ID = 0; int.TryParse(txtID.Text, out ID);
           
            oDepartamentoBLL.ID = ID;

            oDepartamentoBLL.Departamento=txtNombre.Text;

           return oDepartamentoBLL;
        }

        private void Seleccionar(object sender, DataGridViewCellMouseEventArgs e)
        {
            int indice = e.RowIndex;

            dgvDepartamentos.ClearSelection();

            if (indice>=0)
            {
                
            txtID.Text = dgvDepartamentos.Rows[indice].Cells[0].Value.ToString();
            txtNombre.Text = dgvDepartamentos.Rows[indice].Cells[1].Value.ToString();

            btnAgregar.Enabled = false;
            btnBorrar.Enabled = true;
            btnModificar.Enabled = true;
            btnCancelar.Enabled = true;
            
            }


        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (Program.UsuarioActual.Nivel < 3)
            {
                MessageBox.Show("Tu nivel es muy bajo para realizar esta acción.");
                return;
            }

            oDepartamentosDAL.Eliminar(RecuperarInformación());
            LLegarGrid();
            LimpiarEntradas();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (Program.UsuarioActual.Nivel < 2)
            {
                MessageBox.Show("Tu nivel es muy bajo para realizar esta acción.");
                return;
            }

            oDepartamentosDAL.Modificar(RecuperarInformación());
            LLegarGrid();
            LimpiarEntradas();
        }


        public void LLegarGrid() 
        {
            dgvDepartamentos.DataSource = oDepartamentosDAL.MostrarDepartamentos().Tables[0];
            dgvDepartamentos.Columns[0].HeaderText = "ID";
            dgvDepartamentos.Columns[1].HeaderText = "Nombre Departamento";

        }

        public void LimpiarEntradas() 
        {
            txtID.Text = "";
            txtNombre.Text = "";

            btnAgregar.Enabled = true;
            btnBorrar.Enabled = false;
            btnModificar.Enabled = false;
            btnCancelar.Enabled = false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarEntradas();
        }
    }
}
