using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using SOMBRA.BLL;

namespace SOMBRA.DAL
{
    internal class DepartamentosDAL
    {
        private conexionDAL conexion;

        public DepartamentosDAL()
        {
            // Inicializa el campo de clase conexion
            conexion = new conexionDAL();
        }

        public bool Agregar(DepartamentoBLL oDepartamentoBLL)
        {
            SqlCommand SQLComando = new SqlCommand("INSERT INTO Departamentos VALUES (@Departamento)");

            SQLComando.Parameters.Add("@Departamento", SqlDbType.VarChar).Value = oDepartamentoBLL.Departamento;

            return conexion.ejecutarComandoSinRetorno(SQLComando);
        }

        public bool Eliminar(DepartamentoBLL oDepartamentoBLL) 
        {
            SqlCommand SQLComando = new SqlCommand("DELETE FROM Departamentos WHERE ID = @ID");
            SQLComando.Parameters.Add("@ID",SqlDbType.Int).Value = oDepartamentoBLL.ID;
            return conexion.ejecutarComandoSinRetorno(SQLComando);
        }

        public bool Modificar(DepartamentoBLL oDepartamentoBLL)
        {
            SqlCommand SQLComando = new SqlCommand("UPDATE Departamentos SET departamento=@Departamento WHERE ID = @ID");
            SQLComando.Parameters.Add("@Departamento",SqlDbType.VarChar).Value = oDepartamentoBLL.Departamento;
            SQLComando.Parameters.Add("@ID", SqlDbType.Int).Value = oDepartamentoBLL.ID;

            return conexion.ejecutarComandoSinRetorno(SQLComando);
        }

        public DataSet MostrarDepartamentos() 
        {
            SqlCommand sentencia = new SqlCommand("SELECT * FROM Departamentos");
            return conexion.EjecutarSentencia(sentencia);
        }
    }
}
