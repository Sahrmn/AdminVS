using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class Gasto
    {
        public string descripcion;
        public DateTime fecha;
        public decimal monto;

        public Gasto(string descripcion, DateTime fecha, decimal monto)
        {
            this.descripcion = descripcion;
            this.fecha = fecha;
            this.monto = monto;
        }
    }
}
