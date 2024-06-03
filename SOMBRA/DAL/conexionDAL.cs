using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SOMBRA.DAL
{
    internal class conexionDAL
    {
        private string CadenaConexion = "Server=DESKTOP-59975D0;Database=dbSistema;Trusted_Connection=True;";
        private SqlConnection Conexion;

        public SqlConnection EstablecerConexión()
        {
            if (this.Conexion == null)
            {
                this.Conexion = new SqlConnection(this.CadenaConexion);
            }
            return this.Conexion;
        }

        public void AbrirConexión()
        {
            if (Conexion == null)
            {
                EstablecerConexión();
            }

            if (Conexion.State != ConnectionState.Open)
            {
                Conexion.Open();
            }
        }

        public void CerrarConexión()
        {
            if (Conexion != null && Conexion.State == ConnectionState.Open)
            {
                Conexion.Close();
            }
        }

        public bool ejecutarComandoSinRetorno(SqlCommand SQLComando)
        {
            try
            {
                SQLComando.Connection = this.EstablecerConexión(); // Asignar la conexión al comando
                AbrirConexión();
                SQLComando.ExecuteNonQuery();
                CerrarConexión();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en ejecutarComandoSinRetorno: " + ex.Message);
                return false;
            }
            finally
            {
                CerrarConexión();
            }
        }

        public DataSet EjecutarSentencia(SqlCommand sqlComando)
        {
            DataSet DS = new DataSet();
            SqlDataAdapter Adaptador = new SqlDataAdapter();

            try
            {
                sqlComando.Connection = EstablecerConexión(); // Asignar la conexión al comando
                Adaptador.SelectCommand = sqlComando;
                AbrirConexión();
                Adaptador.Fill(DS);
                CerrarConexión();
                return DS;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en EjecutarSentencia: " + ex.Message);
                return DS;
            }
            finally
            {
                CerrarConexión();
            }
        }
    }
}

