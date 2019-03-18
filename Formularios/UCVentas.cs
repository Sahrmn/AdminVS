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
    public partial class UCVentas : UserControl
    {
        public UCVentas()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        }

        public UCVentas(string usuario): this()
        {
            this.lblVendedor.Text = usuario;
            //txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            this.txtProducto.Enabled = false;
            //dgvVentas.AutoGenerateColumns = false;

            DataTable dt = new DataTable();
            dt.Columns.Add("ID_PRODUCTO");
            dt.Columns.Add("CANTIDAD");
            dt.Columns.Add("DESCRIPCION");
            dt.Columns.Add("PRECIO");
            dt.Columns.Add("IMPORTE");
            this._dt = dt;
            dgvVentas.DataSource = this._dt;
            dgvVentas.Columns["DESCRIPCION"].ReadOnly = true;
            dgvVentas.Columns["IMPORTE"].ReadOnly = true;
            dgvVentas.Columns["ID_PRODUCTO"].Visible = false;
            this.txtProducto.Text = "F3";
            this.txtProducto.ForeColor = Color.Gray;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

        private DataTable _dt;
        private int _idProducto;

        public string NombreProducto
        {
            get { return this.txtProducto.Text; }
            set { this.txtProducto.Text = value; }
        }

        public int IdProducto
        {
            get { return this._idProducto; }
            set { this._idProducto = value; }
        }

        public string PrecioProducto
        {
            get { return this.txtPrecio.Text; }
            set { this.txtPrecio.Text = value; }
        }
        
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            int id = this.IdProducto; //tomar id del formulario de buscar producto
            if (txtCantidad.Text != "" && txtProducto.Text != "" && txtPrecio.Text != "")
            {
                if (this._dt.Rows.Count == 0)
                {
                    AgregarFila(id, int.Parse(txtCantidad.Text), txtProducto.Text, decimal.Parse(txtPrecio.Text));
                    dgvVentas.DataSource = this._dt;
                    txtProducto.Text = "";
                    txtPrecio.Text = "";
                    txtCantidad.Text = "";
                    txtProducto.Focus();
                    txtProducto.Select();
                    CalcularTotal();
                }
                else
                {
                    //verificar que no se repita el producto
                    string texto = txtProducto.Text;
                    bool flag = false;
                    foreach (DataRow row in this._dt.Rows)
                    {
                        if (texto == row["descripcion"].ToString())
                        {
                            flag = true;
                        }
                    }

                    if (flag == false)
                    {
                        AgregarFila(id, int.Parse(txtCantidad.Text), txtProducto.Text, decimal.Parse(txtPrecio.Text));
                        dgvVentas.DataSource = this._dt;
                        txtProducto.Text = "";
                        txtPrecio.Text = "";
                        txtCantidad.Text = "";
                        txtProducto.Focus();
                        txtProducto.Select();
                        CalcularTotal();
                    }
                    else
                    {
                        MessageBox.Show("Ya hay un producto del mismo tipo agregado. Modifique la cantidad si desea agregar más productos.");
                    }
                }
            }
        }

        private void AgregarFila(int id, int cantidad, string descripcion, decimal precio)
        {
            int cantStock = Producto.CalcularStock(id);
            if ((cantStock - cantidad) <= 0)
            {
                MessageBox.Show("No hay stock suficiente para realizar la venta!", "Error de Stock", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                btnBuscar.Focus();
                btnBuscar.Select();
            }
            else
            {
                DataRow row = this._dt.NewRow();
                row["id_producto"] = id;
                row["cantidad"] = cantidad;
                row["descripcion"] = descripcion;
                row["precio"] = precio;
                row["importe"] = (precio * cantidad);
                this._dt.Rows.Add(row);
            }
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))//Si es número 
            {
                e.Handled = false;
            }
            else if (e.KeyChar == (char)Keys.Back)//si es tecla borrar
            {
                e.Handled = false;
            }
            else //Si es otra tecla cancelamos
            {
                e.Handled = true;
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || Char.IsPunctuation(e.KeyChar))//Si es número o punto o coma
            {
                e.Handled = false;
            }
            else if (e.KeyChar == (char)Keys.Back)//si es tecla borrar
            {
                e.Handled = false;
            }
            else //Si es otra tecla cancelamos
            {
                e.Handled = true;
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Esta seguro de querer eliminar la celda?", "Eliminar celda", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                int fila = CapturoFilaSeleccionada();
                this._dt.Rows[fila].Delete();
                CalcularTotal();
            }
        }

        private int CapturoFilaSeleccionada()
        {
            int filaSelect = dgvVentas.CurrentCell.RowIndex;//capturo la fila seleccionada
            //object data = dgvVentas[0, filaSelect].Value;//a partir de la fila obtengo el valor de la columna 0(id)
            //return int.Parse(data.ToString());
            return filaSelect;
        }

        private void dgvVentas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dgvVentas.Rows[e.RowIndex].Selected = true;
            }
            catch
            {
                //no pasa nada
            }
        }

        private void CalcularTotal()
        {
            decimal num = 0;
            foreach (DataRow row in this._dt.Rows)
            {
                num += decimal.Parse(row["importe"].ToString());
            }
            lblTotal.Text = num.ToString();
        }

        private void btnFacturar_Click(object sender, EventArgs e)
        {
            string descripcion = txtDescripcion.Text;
            DateTime fecha = dtpFecha.Value;
            try
            {
                string consulta;
                if (descripcion == "")
                {
                    DB.InsertarEnVentas(fecha);
                }
                else
                {
                    DB.InsertarEnVentas(descripcion, fecha);
                }

                //recupero el ultimo id de venta generada 
                DataTable data = DB.GetData("SELECT TOP 1 id FROM Ventas ORDER BY id DESC");
                string idVenta = "";
                foreach (DataRow row in data.Rows)
                {
                    idVenta = row["id"].ToString();
                }

                //recupero los productos 
                foreach (DataRow row in this._dt.Rows)
                {
                    consulta = "INSERT INTO Ventas_Productos (id_venta, id_producto, precio, cantidad) ";
                    consulta += "VALUES(" + idVenta + ", " + row["id_producto"].ToString() + ", " + row["precio"] + ", " + row["cantidad"] + ")";
                    DB.InsertarEnDB(consulta);
                }
                MessageBox.Show("Facturacion realizada correctamente!", "Facturacion");
            }
            catch (Exception ex)
            {
                Archivos.LogError(ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtProducto_TextChanged(object sender, EventArgs e)
        {
            txtProducto.ForeColor = Color.Black;
        }

        private void txtProducto_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.F3)
            {
                UCBuscar frmBuscar = new UCBuscar("Productos");
                frmBuscar.Text = "Productos";
                frmBuscar.Show();
            }
        }

        private void txtProducto_Enter(object sender, EventArgs e)
        {
            if (txtProducto.Text == "F3")
            {
                txtProducto.Text = "";
            }
        }

        private void txtProducto_Leave(object sender, EventArgs e)
        {
            if (txtProducto.Text == "")
            {
                txtProducto.Text = "F3";
                txtProducto.ForeColor = Color.Gray;
            }
        }

        private void FormVentas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.F3)
            {
                UCBuscar frmBuscar = new UCBuscar("Productos");
                frmBuscar.Text = "Productos";
                frmBuscar.Show();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            UCBuscar frmBuscar = new UCBuscar("Productos");
            frmBuscar.Text = "Productos";
            frmBuscar.Show();
        }

        private void FormVentas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Escape)
            {
                //this.Close();
            }
        }
    }
}
