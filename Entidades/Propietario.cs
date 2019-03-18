using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class Propietario
    {
        public string nombre;
        public string pass;

        public Propietario(string nombre, string contraseña)
        {
            this.nombre = nombre;
            this.pass = contraseña;
        }
    }
}
