﻿using ModeloParcial.Datos.DAO;
using ModeloParcial.Datos.Interfaz;
using ModeloParcial.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloParcial.Servicios
{
    public class GestorMateriales
    {
        private IDaoMaterial daoMaterial;

        public GestorMateriales()
        {
            daoMaterial = new DaoMaterial();
        }

        public List<Material> ObtenerMateriales()
        {
            return daoMaterial.ObtenerMateriales();
        }
    }
}
