using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class Categoria
    {
        public string nombre;
        public int id_propietario;

        public Categoria(string nombre, int idPropietario)
        {
            this.nombre = nombre;
            this.id_propietario = idPropietario;
        }
    }
}
