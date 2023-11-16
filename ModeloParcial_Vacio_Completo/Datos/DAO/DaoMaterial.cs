using ModeloParcial.Datos.Interfaz;
using ModeloParcial.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloParcial.Datos.DAO
{
    public class DaoMaterial : IDaoMaterial
    {
        public List<Material> ObtenerMateriales()
        {
            List<Material> listaMateriales = new List<Material>();
            DataTable dataTable = DBHelper.ObtenerInstancia().Consultar("SP_CONSULTAR_MATERIALES");

            foreach (DataRow row in dataTable.Rows)
            {
                Material oMaterial = new Material();
                oMaterial.Codigo = Convert.ToInt32(row["codigo"]);
                oMaterial.Nombre = Convert.ToString(row["nombre"]);
                oMaterial.Stock = Convert.ToInt32(row["stock"]);

                listaMateriales.Add(oMaterial);
            }

            return listaMateriales;
        }
    }
}
