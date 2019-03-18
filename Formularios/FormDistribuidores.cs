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
    public partial class FormDistribuidores : Form
    {
        public Distribuidor dist;

        public FormDistribuidores()
        {
            InitializeComponent();
        }

        public FormDistribuidores(int id):this()
        {
            try
            {
                DataTable data = DB.GetData("SELECT * FROM Distribuidores WHERE id = " + id);
                if (data != null)
                {
                    foreach (DataRow row in data.Rows)
                    {
                        this.txtNombre.Text = row["nombre"].ToString();
                        this.txtDescripcion.Text = row["descripcion"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Archivos.LogError(e);
            }
        }

        private void Distribuidores_Load(object sender, EventArgs e)
        {

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string nom = txtNombre.Text;
            string desc = txtDescripcion.Text;

            try
            {
                if (nom != "" && desc != "")
                {
                    Distribuidor nuevoDist = new Distribuidor(nom, desc);
                    this.dist = nuevoDist;
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormDistribuidores_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
