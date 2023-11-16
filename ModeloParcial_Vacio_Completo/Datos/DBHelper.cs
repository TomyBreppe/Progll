using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloParcial.Datos
{
    public class DBHelper
    {
        private static DBHelper instancia = null;
        private SqlConnection conexion;
        private string cadenaConexion = @"Data Source=LAPTOP-DQ5FILPL\SQLEXPRESS;Initial Catalog=ORDENES;Integrated Security=True";

        public DBHelper()
        {
            conexion = new SqlConnection(cadenaConexion);
        }

        public static DBHelper ObtenerInstancia()
        {
            if (instancia == null)
            {
                instancia = new DBHelper();
            }

            return instancia;
        }

        public SqlConnection ObtenerConexion()
        {
            return this.conexion;
        }

        public DataTable Consultar(string nombreSP)
        {
            conexion.Open();

            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = nombreSP;

            DataTable dataTable = new DataTable();
            dataTable.Load(comando.ExecuteReader());

            conexion.Close();

            return dataTable;
        }
    }
}
