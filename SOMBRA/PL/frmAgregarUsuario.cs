using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using SOMBRA.DAL;

namespace SOMBRA.PL
{
    public partial class frmAgregarUsuario : Form
    {
        private conexionDAL conexion;

        public frmAgregarUsuario()
        {
            InitializeComponent();
            conexion = new conexionDAL();
            CargarRoles();
        }

        private void CargarRoles()
        {
            try
            {
                SqlCommand comando = new SqlCommand("SELECT Id, Nombre FROM Roles", conexion.EstablecerConexión());
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                DataTable tabla = new DataTable();
                adaptador.Fill(tabla);

                cbxRoles.DataSource = tabla;
                cbxRoles.DisplayMember = "Nombre";
                cbxRoles.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar roles: " + ex.Message);
            }
            finally
            {
                conexion.CerrarConexión();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombreUsuario = txtNombreUsuario.Text;
            string contrasena = txtContrasena.Text;
            int idRol = Convert.ToInt32(cbxRoles.SelectedValue);

            SqlCommand comando = new SqlCommand("INSERT INTO Usuarios (NombreUsuario, Contrasena, IdRol) VALUES (@NombreUsuario, @Contrasena, @IdRol)", conexion.EstablecerConexión());
            comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
            comando.Parameters.AddWithValue("@Contrasena", contrasena);
            comando.Parameters.AddWithValue("@IdRol", idRol);

            try
            {
                conexion.AbrirConexión();
                comando.ExecuteNonQuery();
                MessageBox.Show("Usuario agregado exitosamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar usuario: " + ex.Message);
            }
            finally
            {
                conexion.CerrarConexión();
            }
        }
    }
}