using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class Venta
    {
        public string descripcion;
        public string fecha;

        public Venta(string descripcion, string fecha)
        {
            this.descripcion = descripcion;
            this.fecha = fecha;
        }
    }
}
