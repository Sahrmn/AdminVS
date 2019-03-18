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
    public partial class FormCategoria : Form
    {
        public Categoria cat;

        public FormCategoria()
        {
            InitializeComponent();
            CargarCombos();
        }

        public FormCategoria(int id): this()
        {
            try
            {
                DataTable data = DB.GetData("SELECT * FROM Categorias WHERE id = " + id);
                if (data != null)
                {
                    foreach (DataRow row in data.Rows)
                    {
                        this.txtNombre.Text = row["nombre"].ToString();
                        this.comboPropietario.SelectedIndex = int.Parse(row["id_propietario"].ToString()) - 1;
                    }
                }
            }
            catch (Exception e)
            {
                Archivos.LogError(e);
            }
        }

        private void FormCategoria_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string nom = txtNombre.Text;
            string prop = comboPropietario.SelectedValue.ToString();

            try
            {
                if (nom != "" && prop != "")
                {
                    Categoria nuevaCat = new Categoria(nom, int.Parse(prop));
                    this.cat = nuevaCat;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Rellene los campos", "Error");
                }
            }
            catch(Exception ex)
            {
                Archivos.LogError(ex);
                MessageBox.Show("Ocurrio un error. Reintente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarCombos()
        {
            comboPropietario.DisplayMember = "nombre";
            comboPropietario.ValueMember = "id";
            comboPropietario.DataSource = DB.GetData("SELECT * FROM Propietarios");
        }

        private void FormCategoria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
