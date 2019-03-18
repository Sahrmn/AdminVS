using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entidades;

namespace Formularios
{
    public partial class UCCaja : UserControl
    {
        public UCCaja()
        {
            InitializeComponent();
            dgvCaja.AutoGenerateColumns = false;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            CalculoTotalCaja();
        }

        private void CalculoTotalCaja()
        {
            try
            {
                int comparaFecha = DateTime.Compare(dtp1.Value, dtp2.Value);
                if (comparaFecha < 0)
                {
                    string fechaInicial = dtp1.Value.ToString("yyyyMMdd");
                    string fechaFinal = dtp2.Value.ToString("yyyyMMdd");

                    //MessageBox.Show(fechaInicial + " " + fechaFinal);

                    //calculo ganancias totales
                    DataTable data = DB.GetData("SELECT id FROM ventas WHERE fecha BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + " 23:59:59'");

                    //cargo datagrid
                    CargaDataGrid(data);

                    DataTable dataVentasProductos = null;
                    decimal totalGanancias = 0;

                    foreach (DataRow row in data.Rows)
                    {
                        dataVentasProductos = DB.GetData("SELECT * FROM Ventas_Productos WHERE id_venta = " + row["id"].ToString());
                        foreach (DataRow row2 in dataVentasProductos.Rows)
                        {
                            decimal num = (decimal.Parse(row2["precio"].ToString()) * decimal.Parse(row2["cantidad"].ToString()));
                            totalGanancias += decimal.Round(num, 2);
                        }
                    }

                    //MessageBox.Show(totalGanancias.ToString());

                    lblEntradas.Text = decimal.Round(totalGanancias, 2).ToString();

                    //calculo gastos totales

                    decimal totalGastos = 0;
                    data = DB.GetData("SELECT ISNULL(SUM(monto),0) AS Monto FROM Gastos WHERE Fecha >= CONVERT(datetime, '" + fechaInicial + "') AND Fecha < CONVERT(datetime, '" + fechaFinal + "')");
                    foreach (DataRow row in data.Rows)
                    {
                        totalGastos = decimal.Parse(row["Monto"].ToString());
                        //totalGastos = (double)row["Monto"];
                        //MessageBox.Show(row["Monto"].ToString());
                    }

                    lblSalidas.Text = decimal.Round(totalGastos, 2).ToString();

                    lblTotal.Text = decimal.Round((totalGanancias - totalGastos), 2).ToString();
                }
                else if (comparaFecha > 0)
                {
                    MessageBox.Show("La segunda fecha debe ser anterior a la primera. Reintente", "Ocurrio un problema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Archivos.LogError(ex);
                MessageBox.Show("Ocurrio un error. Reintente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargaDataGrid(DataTable dtVentas)
        {
            //creo tabla source para el datagrid
            DataTable tabla = new DataTable();
            tabla.Columns.Add("id");
            tabla.Columns.Add("nombre");
            tabla.Columns.Add("total");

            DataTable tabla2 = DB.GetData("SELECT id, nombre FROM Propietarios");
            foreach (DataRow row in tabla2.Rows)
            {
                DataRow newRow = tabla.NewRow();
                newRow["nombre"] = row["nombre"].ToString();
                newRow["id"] = row["id"].ToString();
                newRow["total"] = 0;
                tabla.Rows.Add(newRow);
            }
            //dgvCaja.DataSource = tabla;

            //datagrid con los id de ventas en las fechas especificadas -> dtVentas
            foreach (DataRow row in dtVentas.Rows)
            {
                DataTable tablita = DB.GetData("SELECT * FROM Ventas_Productos WHERE id_venta = " + row["id"].ToString());
                decimal monto = 0;
                int idProp = 0;

                if (tablita != null)
                {
                    foreach (DataRow row2 in tablita.Rows)
                    {
                        monto = (decimal)row2["precio"] * (int)row2["cantidad"];
                        DataTable tablaPropietarios = DB.GetData("SELECT TOP 1 c.id_propietario FROM Categorias c INNER JOIN Productos p ON c.id = p.id_categoria INNER JOIN Ventas_Productos vp ON vp.id_producto = p.id WHERE p.id = " + row2["id_producto"]);
                        foreach (DataRow row3 in tablaPropietarios.Rows)
                        {
                            idProp = (int)row3["id_propietario"];
                        }
                    }

                    //agrego en la tabla la info obtenida

                    for (int i = 0; i < tabla.Rows.Count; i++)
                    {
                        if (int.Parse(tabla.Rows[i]["id"].ToString()) == idProp)
                        {
                            decimal num = decimal.Parse(tabla.Rows[i]["total"].ToString()) + monto;
                            tabla.Rows[i]["total"] = decimal.Round(num, 2);
                            break;
                        }
                    }
                }

            }
            dgvCaja.DataSource = tabla;
        }

        private void UCCaja_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Escape)
            {
                this.Parent.Controls.Remove(this);
            }
        }
    }
}
