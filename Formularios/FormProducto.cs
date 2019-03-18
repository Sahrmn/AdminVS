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
    public partial class FormProducto : Form
    {
        public Producto prod;

        public FormProducto()
        {
            InitializeComponent();
            //para que tome los centecimos de los precios ingresados!!!!
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            CargarCombos();
        }

        public FormProducto(int id):this()
        {
            try
            {
                DataTable data = DB.GetData("SELECT * FROM Productos WHERE id = " + id);
                
                foreach (DataRow row in data.Rows)
                {
                    this.txtNombre.Text = row["nombre"].ToString();
                    int index = int.Parse(row["id_categoria"].ToString());
                    this.comboCategoria.SelectedIndex = index -1 ;
                    index = int.Parse(row["id_distribuidor"].ToString());
                    this.comboDistribuidor.SelectedIndex = index -1 ;
                    this.txtPrecio.Text = decimal.Round((decimal)row["precio"], 2).ToString();
                    
                }
            }
            catch(Exception e)
            {
                Archivos.LogError(e);
            }
        }

        private void Producto_Load(object sender, EventArgs e)
        {
            
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboCategoria.SelectedValue.ToString() != "Seleccione una categoria..." && comboDistribuidor.SelectedValue.ToString() != "Seleccione un distribuidor...")
                {
                    string nombre = txtNombre.Text;
                    string categoria = comboCategoria.SelectedValue.ToString();
                    string distribuidor = comboDistribuidor.SelectedValue.ToString();
                    decimal precio = decimal.Parse(txtPrecio.Text);

                    Producto nuevoProd = new Producto(nombre, int.Parse(categoria), int.Parse(distribuidor), precio);
                    this.prod = nuevoProd;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Seleccione bien los datos", "Error en el ingreso de datos", MessageBoxButtons.OK);
                }
            }
            catch(Exception ex)
            {
                Archivos.LogError(ex);
                //this.DialogResult = DialogResult.No;
                MessageBox.Show("Ocurrio un error. Reintente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CargarCombos()
        {
            comboCategoria.DisplayMember = "nombre";
            comboCategoria.ValueMember = "id";
            comboCategoria.DataSource = DB.GetData("SELECT * FROM Categorias");

            comboDistribuidor.DisplayMember = "nombre";
            comboDistribuidor.ValueMember = "id";
            comboDistribuidor.DataSource = DB.GetData("SELECT * FROM Distribuidores");
        }

        private void FormProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
