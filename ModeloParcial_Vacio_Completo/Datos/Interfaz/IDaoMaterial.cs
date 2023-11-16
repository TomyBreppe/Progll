using ModeloParcial.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloParcial.Datos.Interfaz
{
    public interface IDaoMaterial
    {
        List<Material> ObtenerMateriales();
    }
}

// GET - PUT - POST - DELETE