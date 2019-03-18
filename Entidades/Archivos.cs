using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Entidades
{
    public static class Archivos
    {
        public static bool Escribir(string path, string contenido, bool sobreescribir)
        {
            bool retValue = true;
            try
            {
                StreamWriter escribe = new StreamWriter(path, sobreescribir);
                //escribe.Write(contenido); escribe una cadena sin provocar salto de linea
                escribe.WriteLine(contenido);
                escribe.Close();
            }
            catch
            {
                retValue = false;
            }
            return retValue;
        }

        public static bool LogError(Exception e)
        {
            bool retValue = true;
            string contenido = "";
            try
            {
                StreamWriter escribe = new StreamWriter("logErrores.txt", true);
                //escribe.Write(contenido); escribe una cadena sin provocar salto de linea

                StackTrace st = new StackTrace(e, true);
                StackFrame frame = st.GetFrame(st.FrameCount - 1);
                // Los datos separados de la excepcion
                // se encuentran en la variable frame

                contenido += DateTime.Now.ToString();
                contenido += " ";
                contenido += e.Message;
                escribe.WriteLine(contenido);

                // Obtengo el nombre de archivo
                contenido = frame.GetFileName();
                escribe.WriteLine(contenido);

                // Obtengo nombre del metodo
                //contenido = frame.GetMethod().DeclaringType.Name;
                //escribe.WriteLine(contenido);

                // Obtengo el numero de linea
                contenido = "Numero de linea: ";
                contenido += frame.GetFileLineNumber().ToString();
                escribe.WriteLine(contenido);

                // Obtengo codigo de error
                //contenido += frame.GetHashCode();

                
                //escribe.WriteLine(contenido);
                escribe.Close();
            }
            catch
            {
                retValue = false;
            }
            return retValue;
        }
    }
}
