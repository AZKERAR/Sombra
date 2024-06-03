using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SOMBRA.DAL;
using SOMBRA.BLL;

namespace SOMBRA.PL
{
    public partial class frmEmpleados : Form
    {
        byte[] imagenByte;
        EmpleadosDAL oEmpleadosDAL;

        public frmEmpleados()
        {
            InitializeComponent();
            oEmpleadosDAL = new EmpleadosDAL();
            CargarEmpleados();
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


        private void frmEmpleados_Load(object sender, EventArgs e)
        {
            DepartamentosDAL objDepartamentos = new DepartamentosDAL();

            cbxDepartamento.DataSource = objDepartamentos.MostrarDepartamentos().Tables[0];
            cbxDepartamento.DisplayMember = "departamento";
            cbxDepartamento.ValueMember = "ID";
            btnModificar.Enabled = false;
            btnBorrar.Enabled = false;

            // Configurar DataGridView
            dgvEmpleados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmpleados.AllowUserToAddRows = false; // Ocultar la última fila
            dgvEmpleados.RowHeadersVisible = false; // Deshabilitar la columna de encabezado de fila
        }


        private void btnExaminar_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectorImagen = new OpenFileDialog();
            selectorImagen.Title = "Seleccionar Imagen";

            if (selectorImagen.ShowDialog() == DialogResult.OK)
            {
                picFoto.Image = Image.FromStream(selectorImagen.OpenFile());
                MemoryStream memoria = new MemoryStream();
                picFoto.Image.Save(memoria, System.Drawing.Imaging.ImageFormat.Png);

                imagenByte = memoria.ToArray();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (Program.UsuarioActual.Nivel < 2)
            {
                MessageBox.Show("Tu nivel es muy bajo para realizar esta acción.");
                return;
            }

            RecolectarDatosYAgregar();
            CargarEmpleados();
        }

        private void RecolectarDatosYAgregar()
        {
            try
            {
                EmpleadosBLL objEmpleados = new EmpleadosBLL();

                int codigoEmpleado = 1;
                int.TryParse(txtID.Text, out codigoEmpleado);

                objEmpleados.ID = codigoEmpleado;
                objEmpleados.NombreEmpleado = txtNombre.Text;
                objEmpleados.PrimerApellido = txtPrimerApellido.Text;
                objEmpleados.SegundoApellido = txtSegundoApellido.Text;
                objEmpleados.Correo = txtCorreo.Text;

                int IDDepartamento = 0;
                int.TryParse(cbxDepartamento.SelectedValue.ToString(), out IDDepartamento);

                objEmpleados.Departamento = IDDepartamento;
                objEmpleados.FotoEmpleado = imagenByte;

                if (oEmpleadosDAL.Agregar(objEmpleados))
                {
                    MessageBox.Show("Empleado agregado exitosamente");
                    LimpiarEntradas(); // Limpiar los campos después de agregar un empleado
                }
                else
                {
                    MessageBox.Show("Error al agregar empleado");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en RecolectarDatosYAgregar: " + ex.Message);
            }
        }

        private void CargarEmpleados()
        {
            dgvEmpleados.DataSource = oEmpleadosDAL.MostrarEmpleados().Tables[0];
            dgvEmpleados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmpleados.AllowUserToAddRows = false; // Ocultar la última fila
            dgvEmpleados.RowHeadersVisible = false; // Deshabilitar la columna de encabezado de fila
        }

        private void dgvEmpleados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvEmpleados.Rows[e.RowIndex];
                txtID.Text = row.Cells["id"].Value.ToString();
                txtNombre.Text = row.Cells["nombres"].Value.ToString();
                txtPrimerApellido.Text = row.Cells["primerapellido"].Value.ToString();
                txtSegundoApellido.Text = row.Cells["segundoapellido"].Value.ToString();
                txtCorreo.Text = row.Cells["correo"].Value.ToString();

                if (row.Cells["foto"].Value != DBNull.Value)
                {
                    byte[] imageBytes = (byte[])row.Cells["foto"].Value;
                    MemoryStream ms = new MemoryStream(imageBytes);
                    picFoto.Image = Image.FromStream(ms);
                }
                else
                {
                    picFoto.Image = null;
                }

                cbxDepartamento.SelectedValue = row.Cells["idDepartamento"].Value;
                btnModificar.Enabled = true;
                btnBorrar.Enabled = true;
            }
        }

        private void LimpiarEntradas()
        {
            txtID.Clear();
            txtNombre.Clear();
            txtPrimerApellido.Clear();
            txtSegundoApellido.Clear();
            txtCorreo.Clear();
            picFoto.Image = null;
            cbxDepartamento.SelectedIndex = -1;
            btnModificar.Enabled = false;
            btnBorrar.Enabled = false;
        }


        private void seleccionar(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvEmpleados.Rows[e.RowIndex];
                txtID.Text = row.Cells["id"].Value.ToString();
                txtNombre.Text = row.Cells["nombres"].Value.ToString();
                txtPrimerApellido.Text = row.Cells["primerapellido"].Value.ToString();
                txtSegundoApellido.Text = row.Cells["segundoapellido"].Value.ToString();
                txtCorreo.Text = row.Cells["correo"].Value.ToString();

                if (row.Cells["foto"].Value != DBNull.Value)
                {
                    byte[] imageBytes = (byte[])row.Cells["foto"].Value;
                    MemoryStream ms = new MemoryStream(imageBytes);
                    picFoto.Image = Image.FromStream(ms);
                }
                else
                {
                    picFoto.Image = null;
                }

                cbxDepartamento.SelectedValue = row.Cells["idDepartamento"].Value;
                btnModificar.Enabled = true;
                btnBorrar.Enabled = true;
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (Program.UsuarioActual.Nivel < 2)
            {
                MessageBox.Show("Tu nivel es muy bajo para realizar esta acción.");
                return;
            }

            RecolectarDatosYModificar();
            CargarEmpleados();
        }

        private void RecolectarDatosYModificar()
        {
            try
            {
                EmpleadosBLL objEmpleados = new EmpleadosBLL();

                int codigoEmpleado = 1;
                int.TryParse(txtID.Text, out codigoEmpleado);

                objEmpleados.ID = codigoEmpleado;
                objEmpleados.NombreEmpleado = txtNombre.Text;
                objEmpleados.PrimerApellido = txtPrimerApellido.Text;
                objEmpleados.SegundoApellido = txtSegundoApellido.Text;
                objEmpleados.Correo = txtCorreo.Text;

                int IDDepartamento = 0;
                int.TryParse(cbxDepartamento.SelectedValue.ToString(), out IDDepartamento);

                objEmpleados.Departamento = IDDepartamento;
                objEmpleados.FotoEmpleado = imagenByte;

                if (oEmpleadosDAL.Modificar(objEmpleados))
                {
                    MessageBox.Show("Empleado modificado exitosamente");
                    LimpiarEntradas(); // Limpiar los campos después de modificar un empleado
                }
                else
                {
                    MessageBox.Show("Error al modificar empleado");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en RecolectarDatosYModificar: " + ex.Message);
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (Program.UsuarioActual.Nivel < 3)
            {
                MessageBox.Show("Tu nivel es muy bajo para realizar esta acción.");
                return;
            }

            RecolectarDatosYBorrar();
            CargarEmpleados();
        }

        private void RecolectarDatosYBorrar()
        {
            try
            {
                EmpleadosBLL objEmpleados = new EmpleadosBLL();

                int codigoEmpleado = 1;
                int.TryParse(txtID.Text, out codigoEmpleado);

                objEmpleados.ID = codigoEmpleado;

                if (oEmpleadosDAL.Eliminar(objEmpleados))
                {
                    MessageBox.Show("Empleado eliminado exitosamente");
                    LimpiarEntradas(); // Limpiar los campos después de eliminar un empleado
                }
                else
                {
                    MessageBox.Show("Error al eliminar empleado");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en RecolectarDatosYBorrar: " + ex.Message);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarEntradas();
        }
    }
}
