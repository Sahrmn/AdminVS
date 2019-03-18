using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Entidades
{
    public static class DB
    {
        public static bool InsertarEnDB(string comando)
        {
            bool retValue = false;
            try
            {
                SqlConnection sql = new SqlConnection(Properties.Settings.Default.Conexion);
                sql.Open();
                SqlCommand consulta = new SqlCommand();
                consulta.CommandText = comando;
                consulta.Connection = sql;
                int data = consulta.ExecuteNonQuery();
                if (data == 1)
                {
                    retValue = true;
                }
                sql.Close();
            }
            catch(Exception e)
            {
                retValue = false;
                Archivos.LogError(e);
            }
            return retValue;
        }

        public static bool InsertarEnVentas(DateTime fecha)
        {
            return InsertarEnVentas("NULL", fecha);
        }

        public static bool InsertarEnVentas(string desc, DateTime fecha)
        {
            bool retValue = false;
            try
            {
                //DateTime fecha_actual = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                SqlConnection sql = new SqlConnection(Properties.Settings.Default.Conexion);
                sql.Open();
                string comando = "INSERT INTO Ventas (fecha, descripcion) VALUES(@fecha, '" + desc + "')";
                SqlCommand consulta = new SqlCommand(comando, sql);
                consulta.Parameters.Add(new SqlParameter("@fecha", SqlDbType.DateTime)).Value = fecha;
                int data = consulta.ExecuteNonQuery();
                if (data == 1)
                {
                    retValue = true;
                }
                sql.Close();
            }
            catch (Exception e)
            {
                retValue = false;
                Archivos.LogError(e);
            }
            return retValue;
        }

        /*public static bool InsertarEnGastos(string desc, DateTime fecha, decimal monto)
        {
            bool retValue = false;
            try
            {
                SqlConnection sql = new SqlConnection(Properties.Settings.Default.Conexion);
                sql.Open();
                string comando = "INSERT INTO Gastos (descripcion, fecha, monto) VALUES('" + desc + "', @fecha, " + monto.ToString() + ")";
                SqlCommand consulta = new SqlCommand(comando, sql);
                consulta.Parameters.Add(new SqlParameter("@fecha", SqlDbType.DateTime)).Value = fecha;
                int data = consulta.ExecuteNonQuery();
                if (data == 1)
                {
                    retValue = true;
                }
                sql.Close();
            }
            catch (Exception e)
            {
                retValue = false;
                Archivos.LogError(e);
            }
            return retValue;
        }*/

        /*public static bool InsertarEnStock(string idProd, string cant)
        {
            bool retValue = false;
            try
            {
                DateTime fecha_actual = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                SqlConnection sql = new SqlConnection(Properties.Settings.Default.Conexion);
                sql.Open();
                string comando = "INSERT INTO Stock (id_producto, cantidad, fecha) VALUES(" + idProd + ", " + cant + ", @fecha)";
                SqlCommand consulta = new SqlCommand(comando, sql);
                consulta.Parameters.Add(new SqlParameter("@fecha", SqlDbType.DateTime)).Value = fecha_actual;
                int data = consulta.ExecuteNonQuery();
                if (data == 1)
                {
                    retValue = true;
                }
                sql.Close();
            }
            catch (Exception e)
            {
                retValue = false;
                Archivos.LogError(e);
            }
            return retValue;
        }*/

        public static bool ModificarEnDB(string comando)
        {
            return InsertarEnDB(comando);
        }

        public static bool EliminarDeDB(string comando)
        {
            bool retValue = false;
            try
            {
                //"DELETE FROM Personas WHERE id = 3";
                SqlConnection sql = new SqlConnection(Properties.Settings.Default.Conexion);
                sql.Open();
                SqlCommand consulta = new SqlCommand();
                consulta.CommandText = comando;
                consulta.Connection = sql;
                int data = consulta.ExecuteNonQuery();
                if (data == 1)
                {
                    retValue = true;
                }
                sql.Close();
            }
            catch(Exception e)
            {
                retValue = false;
                Archivos.LogError(e);
            }
            return retValue;
        }

        /// <summary>
        /// Verifica contraseña correcta
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="salida"></param>
        /// <returns></returns>
        public static bool VerificarLogin(string usuario, string pass)
        {
            bool retValue = false;
            try
            {
                SqlConnection sql = new SqlConnection(Properties.Settings.Default.Conexion);
                sql.Open();
                SqlCommand consulta = new SqlCommand("SELECT * FROM Propietarios WHERE nombre = '" + usuario + "'");
                consulta.Connection = sql;
                SqlDataReader data = consulta.ExecuteReader();
                if (data != null)
                {
                    while (data.Read())
                    {
                        string contraseña = data["contraseña"].ToString();
                        contraseña.Trim();
                        pass.Trim();
                        if (contraseña == pass)
                        {
                            retValue = true;
                        }
                    }
                }
                data.Close();
                sql.Close();
            }
            catch (SqlException e)
            {
                retValue = false;
                MessageBox.Show("El usuario o la contraseña es incorrecto");
                Archivos.LogError(e);
            }
            catch (Exception e)
            {
                retValue = false;
                Archivos.LogError(e);
            }
            return retValue;
        }


        public static DataTable GetData(string comando)
        {
            DataTable tabla = null;
            try
            {
                SqlConnection sql = new SqlConnection(Properties.Settings.Default.Conexion);
                sql.Open();
                SqlCommand consulta = new SqlCommand(comando);
                consulta.Connection = sql;
                SqlDataAdapter data = new SqlDataAdapter();
                data.SelectCommand = consulta;
                tabla = new DataTable();
                //tabla.Locale = System.Globalization.CultureInfo.CurrentCulture;
                data.Fill(tabla);
                sql.Close();
            }
            catch (Exception e)
            {
                Archivos.LogError(e);
                //Console.WriteLine(e.Message);
            }
            return tabla;
        }

        public static DataTable Buscar(string texto)
        {
            DataTable tabla = null;
            try
            {
                SqlConnection sql = new SqlConnection(Properties.Settings.Default.Conexion);
                sql.Open();
                //string comando = "SELECT * FROM Productos WHERE nombre LIKE '%@text%'";
                string comando = "SELECT * FROM Productos WHERE nombre LIKE '%" + texto + "%'";
                SqlCommand consulta = new SqlCommand(comando, sql);
                //consulta.Parameters.Add(new SqlParameter("@text", SqlDbType.Text)).Value = texto;
                //consulta.Parameters.AddWithValue("@text", texto);
                SqlDataAdapter data = new SqlDataAdapter(consulta);
                //data.SelectCommand = consulta;
                tabla = new DataTable();
                data.Fill(tabla);
                sql.Close();
            }
            catch (Exception e)
            {
                Archivos.LogError(e);
                //Console.WriteLine(e.Message);
            }
            return tabla;
        }
    }
}
