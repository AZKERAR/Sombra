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
    public partial class frmLogin : Form
    {
        private conexionDAL conexion;

        public frmLogin()
        {
            InitializeComponent();
            conexion = new conexionDAL();
            txtContrasena.PasswordChar = '*'; // Establecer el carácter de contraseña
            CargarGif();
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            string nombreUsuario = txtNombreUsuario.Text;
            string contrasena = txtContrasena.Text;

            if (AutenticarUsuario(nombreUsuario, contrasena))
            {
                // Usuario autenticado correctamente
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Nombre de usuario o contraseña incorrectos.");
            }
        }

        private bool AutenticarUsuario(string nombreUsuario, string contrasena)
        {
            try
            {
                SqlCommand comando = new SqlCommand("SELECT U.Id, U.NombreUsuario, R.Nombre AS Rol, R.Nivel FROM Usuarios U JOIN Roles R ON U.IdRol = R.Id WHERE U.NombreUsuario = @NombreUsuario AND U.Contrasena = @Contrasena", conexion.EstablecerConexión());
                comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                comando.Parameters.AddWithValue("@Contrasena", contrasena);

                conexion.AbrirConexión();
                SqlDataReader reader = comando.ExecuteReader();
                if (reader.Read())
                {
                    Program.UsuarioActual = new Usuario()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        NombreUsuario = reader["NombreUsuario"].ToString(),
                        Rol = reader["Rol"].ToString(),
                        Nivel = Convert.ToInt32(reader["Nivel"])
                    };
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al autenticar usuario: " + ex.Message);
                return false;
            }
            finally
            {
                conexion.CerrarConexión();
            }
        }

        private void CargarGif()
        {
            // Ruta completa al archivo GIF
            string rutaGif = @"C:\Users\AZKER\Pictures\SOMBRA_OW\craneo.gif";
            pbGif.Image = Image.FromFile(rutaGif);
            pbGif.SizeMode = PictureBoxSizeMode.StretchImage; // Ajustar el tamaño del PictureBox si es necesario
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}