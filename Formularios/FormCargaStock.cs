using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entidades;

namespace Formularios
{
    public partial class FormCargaStock : Form
    {
        public FormCargaStock()
        {
            InitializeComponent();
        }

        public FormCargaStock(int id):this()
        {
            try
            {
                DataTable data = DB.GetData("SELECT * FROM Productos WHERE id = " + id);

                foreach (DataRow row in data.Rows)
                {
                    this.txtProducto.Text = row["nombre"].ToString();
                    lblId.Text = row["id"].ToString();
                }
                txtCantidad.Focus();
                txtCantidad.Select();
            }
            catch (Exception e)
            {
                Archivos.LogError(e);
            }
        }

        public string NombreProducto
        {
            set { this.txtProducto.Text = value; }
        }

        public string Cantidad
        {
            set { this.lblCantidad.Text = value; }
        }

        public string Id
        {
            set { this.lblId.Text = value; }
        }

        private void CargaStock_Load(object sender, EventArgs e)
        {
            this.txtFecha.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            this.txtProducto.Enabled = false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtProducto.Text != "" && txtCantidad.Text != "")
                {
                    //bool verif = DB.InsertarEnStock(lblId.Text, txtCantidad.Text);
                    bool verif = DB.InsertarEnDB("INSERT INTO Stock (id_producto, cantidad, fecha) VALUES(" + lblId.Text + ", " + txtCantidad.Text + ", CONVERT(datetime, '" + DateTime.Now.ToString() + "'))");
                    if (verif)
                    {
                        MessageBox.Show("Carga realizada con exito!");
                        txtProducto.Text = "";
                        txtCantidad.Text = "";
                        lblId.Text = "";
                        btnBuscar.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Rellene los campos", "Error en el ingreso de datos", MessageBoxButtons.OK);
                }
            }
            catch(Exception ex)
            {
                Archivos.LogError(ex);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            FormBuscar frmBuscar = new FormBuscar("Stock");
            frmBuscar.ShowDialog(this);
        }

        public void CalcularStock(int idProducto)
        {
            int cantidad = Producto.CalcularStock(idProducto);
            this.lblCantidad.Text = cantidad.ToString();
        }

        private void FormCargaStock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
