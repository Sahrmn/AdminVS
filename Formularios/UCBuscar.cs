using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Formularios
{
    public partial class UCBuscar : UserControl
    {
        public UCBuscar()
        {
            InitializeComponent();
        }

        public UCBuscar(string mostrar): this()
        {
            DataTable data = null;
            switch (mostrar)
            {
                case "Stock":
                    data = DB.GetData("SELECT * FROM Productos");
                    this.Text = "Stock Productos";
                    //this.listBuscar.DataSource = data;

                    break;
                case "Productos":
                    data = DB.GetData("SELECT * FROM Productos");
                    this.Text = "Buscar Producto";
                    //this.listBuscar.DataSource = data;
                    //this.listBuscar.DisplayMember = "nombre";
                    //this.listBuscar.ValueMember = "id";
                    break;
                case "Propietarios":
                    data = DB.GetData("SELECT nombre, id FROM Propietarios");
                    this.Text = "Buscar Propietarios";
                    //this.listBuscar.DataSource = data;
                    //this.listBuscar.DisplayMember = "nombre";
                    //this.listBuscar.ValueMember = "id";
                    break;
                case "Distribuidores":
                    data = DB.GetData("SELECT * FROM Distribuidores");
                    this.Text = "Buscar Distribuidores";
                    //this.listBuscar.DataSource = data;
                    //this.listBuscar.DisplayMember = "nombre";
                    //this.listBuscar.ValueMember = "id";
                    break;
                /*case "Ventas":
                    DataTable data3 = DB.GetData("SELECT * FROM Ventas");
                    this.listProductos.DataSource = data3;
                    this.listProductos.DisplayMember = "nombre";
                    this.listProductos.ValueMember = "id";
                    break;*/
                case "Categorias":
                    data = DB.GetData("SELECT * FROM Categorias");
                    this.Text = "Buscar Categorias";
                    //this.listBuscar.DataSource = data;
                    //this.listBuscar.DisplayMember = "nombre";
                    //this.listBuscar.ValueMember = "id";
                    break;
                default:
                    break;
            }
            this.listBuscar.DataSource = data;
            this.listBuscar.DisplayMember = "nombre";
            this.listBuscar.ValueMember = "id";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            DataRow row;
            int id = 0;
            switch (this.Text)
            {
                case "Productos": //hace esto en Buscar del modulo Ventas
                    row = ((DataTable)listBuscar.DataSource).Rows[listBuscar.SelectedIndex];
                    id = (int)row["id"];
                    string name = (string)row["nombre"];
                    decimal precio = (decimal)row["precio"];
                    if (this.Owner is FormVentas)
                    {
                        ((FormVentas)this.Owner).NombreProducto = name;
                        ((FormVentas)this.Owner).IdProducto = id;
                        ((FormVentas)this.Owner).PrecioProducto = decimal.Round(precio, 2).ToString();
                    }

                    break;
                case "Stock Productos":
                    row = ((DataTable)listBuscar.DataSource).Rows[listBuscar.SelectedIndex];
                    id = (int)row["id"];
                    string nombre = (string)row["nombre"];
                    //busco el stock del producto
                    DataTable data = DB.GetData("SELECT cantidad FROM Stock WHERE id = " + id);
                    int cantidad = 0;
                    foreach (DataRow row1 in data.Rows)
                    {
                        cantidad = (int)row1["cantidad"];
                    }
                    ((FormCargaStock)this.Owner).NombreProducto = nombre;
                    //((FormCargaStock)this.Owner).Cantidad = cantidad.ToString();
                    ((FormCargaStock)this.Owner).CalcularStock(id);
                    ((FormCargaStock)this.Owner).Id = id.ToString();
                    this.Close();
                    break;
                case "Buscar Producto"://hace esto en buscar del modulo Ver(Productos)

                //break;
                case "Buscar Propietarios":

                //break;
                case "Buscar Distribuidores":

                //break;
                /*case "Ventas":
                    
                    break;*/
                case "Buscar Categorias":
                    row = ((DataTable)listBuscar.DataSource).Rows[listBuscar.SelectedIndex];
                    id = (int)row["id"];
                    ((FormMostrar)this.Owner).EstablecerSeleccionDGV(id);
                    break;
                default:
                    break;
            }
            this.Parent.Controls.Remove(this);
        }

        private void txtBuscar_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != (char)Keys.Enter)
            {
                DataTable data = DB.Buscar(txtBuscar.Text);
                this.listBuscar.DataSource = data;
                this.listBuscar.DisplayMember = "nombre";
                this.listBuscar.ValueMember = "id";
            }
            else if (e.KeyValue == (char)Keys.Escape)
            {
                this.Parent.Controls.Remove(this);
            }
        }

        private void UCBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Escape)
            {
                this.Parent.Controls.Remove(this);
            }
        }
    }
}
