using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SOMBRA.BLL;

namespace SOMBRA.DAL
{
    internal class EmpleadosDAL
    {
        private conexionDAL conexion;

        public EmpleadosDAL()
        {
            conexion = new conexionDAL();
        }

        public bool Agregar(EmpleadosBLL oEmpleadoBLL)
        {
            try
            {
                // Crear y configurar el comando SQL para insertar un empleado
                SqlCommand SQLComando = new SqlCommand("INSERT INTO Empleados (nombres, primerapellido, segundoapellido, correo, foto) OUTPUT INSERTED.id VALUES (@NombreEmpleado, @PrimerApellido, @SegundoApellido, @Correo, @FotoEmpleado)");

                SQLComando.Parameters.Add("@NombreEmpleado", SqlDbType.VarChar).Value = oEmpleadoBLL.NombreEmpleado;
                SQLComando.Parameters.Add("@PrimerApellido", SqlDbType.VarChar).Value = oEmpleadoBLL.PrimerApellido;
                SQLComando.Parameters.Add("@SegundoApellido", SqlDbType.VarChar).Value = oEmpleadoBLL.SegundoApellido;
                SQLComando.Parameters.Add("@Correo", SqlDbType.VarChar).Value = oEmpleadoBLL.Correo;
                SQLComando.Parameters.Add("@FotoEmpleado", SqlDbType.Image).Value = oEmpleadoBLL.FotoEmpleado;

                SQLComando.Connection = conexion.EstablecerConexión(); // Asignar la conexión al comando

                conexion.AbrirConexión();
                int empleadoID = (int)SQLComando.ExecuteScalar();
                conexion.CerrarConexión();

                // Crear y configurar el comando SQL para insertar en EmpleadoDepartamento
                SqlCommand SQLComandoDept = new SqlCommand("INSERT INTO EmpleadoDepartamento (idEmpleado, idDepartamento) VALUES (@idEmpleado, @idDepartamento)");

                SQLComandoDept.Parameters.Add("@idEmpleado", SqlDbType.Int).Value = empleadoID;
                SQLComandoDept.Parameters.Add("@idDepartamento", SqlDbType.Int).Value = oEmpleadoBLL.Departamento;

                SQLComandoDept.Connection = conexion.EstablecerConexión(); // Asignar la conexión al comando

                return conexion.ejecutarComandoSinRetorno(SQLComandoDept);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en Agregar: " + ex.Message);
                return false;
            }
            finally
            {
                // Asegurarse de que la conexión se cierre aunque ocurra una excepción
                conexion.CerrarConexión();
            }
        }

        public DataSet MostrarEmpleados()
        {
            SqlCommand sentencia = new SqlCommand(
                "SELECT E.id, E.nombres, E.primerapellido, E.segundoapellido, E.correo, E.foto, ED.idDepartamento " +
                "FROM Empleados E " +
                "JOIN EmpleadoDepartamento ED ON E.id = ED.idEmpleado");

            return conexion.EjecutarSentencia(sentencia);
        }

        public bool Modificar(EmpleadosBLL oEmpleadoBLL)
        {
            try
            {
                // Crear y configurar el comando SQL para actualizar un empleado
                SqlCommand SQLComando = new SqlCommand(
                    "UPDATE Empleados SET nombres = @NombreEmpleado, primerapellido = @PrimerApellido, segundoapellido = @SegundoApellido, correo = @Correo, foto = @FotoEmpleado WHERE id = @ID"
                );

                SQLComando.Parameters.Add("@ID", SqlDbType.Int).Value = oEmpleadoBLL.ID;
                SQLComando.Parameters.Add("@NombreEmpleado", SqlDbType.VarChar).Value = oEmpleadoBLL.NombreEmpleado;
                SQLComando.Parameters.Add("@PrimerApellido", SqlDbType.VarChar).Value = oEmpleadoBLL.PrimerApellido;
                SQLComando.Parameters.Add("@SegundoApellido", SqlDbType.VarChar).Value = oEmpleadoBLL.SegundoApellido;
                SQLComando.Parameters.Add("@Correo", SqlDbType.VarChar).Value = oEmpleadoBLL.Correo;
                SQLComando.Parameters.Add("@FotoEmpleado", SqlDbType.Image).Value = oEmpleadoBLL.FotoEmpleado;

                SQLComando.Connection = conexion.EstablecerConexión(); // Asignar la conexión al comando

                conexion.AbrirConexión();
                int rowsAffected = SQLComando.ExecuteNonQuery();
                conexion.CerrarConexión();

                if (rowsAffected > 0)
                {
                    // Crear y configurar el comando SQL para actualizar en EmpleadoDepartamento
                    SqlCommand SQLComandoDept = new SqlCommand(
                        "UPDATE EmpleadoDepartamento SET idDepartamento = @idDepartamento WHERE idEmpleado = @idEmpleado"
                    );

                    SQLComandoDept.Parameters.Add("@idEmpleado", SqlDbType.Int).Value = oEmpleadoBLL.ID;
                    SQLComandoDept.Parameters.Add("@idDepartamento", SqlDbType.Int).Value = oEmpleadoBLL.Departamento;

                    SQLComandoDept.Connection = conexion.EstablecerConexión(); // Asignar la conexión al comando

                    return conexion.ejecutarComandoSinRetorno(SQLComandoDept);
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en Modificar: " + ex.Message);
                return false;
            }
            finally
            {
                // Asegurarse de que la conexión se cierre aunque ocurra una excepción
                conexion.CerrarConexión();
            }
        }

        public bool Eliminar(EmpleadosBLL oEmpleadoBLL)
        {
            try
            {
                // Crear y configurar el comando SQL para eliminar un empleado
                SqlCommand SQLComando = new SqlCommand("DELETE FROM Empleados WHERE id = @ID");

                SQLComando.Parameters.Add("@ID", SqlDbType.Int).Value = oEmpleadoBLL.ID;

                SQLComando.Connection = conexion.EstablecerConexión(); // Asignar la conexión al comando

                conexion.AbrirConexión();
                int rowsAffected = SQLComando.ExecuteNonQuery();
                conexion.CerrarConexión();

                if (rowsAffected > 0)
                {
                    // Crear y configurar el comando SQL para eliminar en EmpleadoDepartamento
                    SqlCommand SQLComandoDept = new SqlCommand("DELETE FROM EmpleadoDepartamento WHERE idEmpleado = @idEmpleado");

                    SQLComandoDept.Parameters.Add("@idEmpleado", SqlDbType.Int).Value = oEmpleadoBLL.ID;

                    SQLComandoDept.Connection = conexion.EstablecerConexión(); // Asignar la conexión al comando

                    return conexion.ejecutarComandoSinRetorno(SQLComandoDept);
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en Eliminar: " + ex.Message);
                return false;
            }
            finally
            {
                // Asegurarse de que la conexión se cierre aunque ocurra una excepción
                conexion.CerrarConexión();
            }
        }


    }
}