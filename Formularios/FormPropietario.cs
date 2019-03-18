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
    public partial class FormPropietario : Form
    {
        public Propietario prop;

        public FormPropietario()
        {
            InitializeComponent();
        }

        public FormPropietario(int id):this()
        {
            try
            {
                DataTable data = DB.GetData("SELECT * FROM Propietarios WHERE id = " + id);
                if (data != null)
                {
                    foreach (DataRow row in data.Rows)
                    {
                        this.txtNombre.Text = row["nombre"].ToString();
                        this.txtPass.Text = row["contraseña"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Archivos.LogError(e);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string nom = txtNombre.Text;
            string pass = txtPass.Text;
            string repPass = txtRepPass.Text;

            try
            {
                if (nom != "" && pass != "" && repPass != "")
                {
                    if (pass != repPass)
                    {
                        MessageBox.Show("Las contraseñas no coinciden", "Cambio de Contraseña");
                    }
                    else
                    {
                        Propietario nuevoProp = new Propietario(nom, pass);
                        this.prop = nuevoProp;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
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

        private void FormPropietario_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void FormPropietario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
