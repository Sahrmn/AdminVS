using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Entidades
{
    public class Producto
    {
        public string nombre;
        private int _idCategoria;
        private int _idDistribuidor;
        public decimal precio;

        public string Nombre
        {
            get { return this.nombre; }
        }

        public int IdCategoria
        {
            get { return this._idCategoria; }
        }

        public int IdDistribuidor
        {
            get { return this._idDistribuidor; }
        }

        public decimal Precio
        {
            get { return this.precio; }
        }

        public Producto(string nombre, int categoria, int distribuidor, decimal precio)
        {
            this.nombre = nombre;
            this._idCategoria = categoria;
            this._idDistribuidor = distribuidor;
            this.precio = precio;
        }

        public static int CalcularStock(int idProducto)
        {
            //sumo el total del stock
            DataTable data = DB.GetData("SELECT ISNULL(SUM(cantidad),0) AS Total FROM Stock WHERE id_producto = " + idProducto);
            int cantidad = 0;
            foreach (DataRow row in data.Rows)
            {
                cantidad = (int)row["Total"];
            }
            //resto la cantidad de ventas de ese producto
            data = DB.GetData("SELECT ISNULL(SUM(cantidad),0) AS Total FROM Ventas_Productos WHERE id_producto = " + idProducto);
            foreach (DataRow row in data.Rows)
            {
                cantidad -= (int)row["Total"];
            }
            return cantidad;
        }
    }
}
