using ModeloParcial.Datos.Interfaz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModeloParcial.Entidades;

namespace ModeloParcial.Datos.DAO
{
    public class DaoOrden : IDaoOrden
    {
        public int InsertarOrden(OrdenRetiro oOrdenRetiro)
        {
            SqlConnection conexion = DBHelper.ObtenerInstancia().ObtenerConexion();
            SqlTransaction transaction = null;
            int resultado = 0;

            try
            {
                conexion.Open();

                transaction = conexion.BeginTransaction();

                SqlCommand cmdMaestro = new SqlCommand();
                cmdMaestro.Connection = conexion;
                cmdMaestro.Transaction = transaction;
                cmdMaestro.CommandType = CommandType.StoredProcedure;
                cmdMaestro.CommandText = "SP_INSERTAR_ORDEN";

                cmdMaestro.Parameters.AddWithValue("@responsable", oOrdenRetiro.Responsable);

                SqlParameter parametroSalida = new SqlParameter();
                parametroSalida.ParameterName = "@nro";
                parametroSalida.SqlDbType = SqlDbType.Int;
                parametroSalida.Direction = ParameterDirection.Output;

                cmdMaestro.Parameters.Add(parametroSalida);

                cmdMaestro.ExecuteNonQuery();

                int nroOrden = (int)parametroSalida.Value;
                int nroDetalle = 1;

                SqlCommand cmdDetalle;
                foreach(DetalleOrden dOrden in oOrdenRetiro.Detalles)
                {
                    cmdDetalle = new SqlCommand();
                    cmdDetalle.Connection = conexion;
                    cmdDetalle.Transaction = transaction;
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.CommandText = "SP_INSERTAR_DETALLES";

                    cmdDetalle.Parameters.AddWithValue("@nro_orden", nroOrden);
                    cmdDetalle.Parameters.AddWithValue("@detalle", nroDetalle);
                    cmdDetalle.Parameters.AddWithValue("@codigo", dOrden.Material.Codigo);
                    cmdDetalle.Parameters.AddWithValue("@cantidad", dOrden.Cantidad);

                    cmdDetalle.ExecuteNonQuery();

                    nroDetalle++;
                }

                transaction.Commit();
                resultado = nroOrden;
            }
            catch
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                // Verificamos que exista una conexión y que esté abierta
                if (conexion != null && conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }

            return resultado;
        }
    }
}
