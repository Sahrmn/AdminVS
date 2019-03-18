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
    public partial class FormLogin : Form, IForm
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string user = txtUsuario.Text;
            string pass = txtPass.Text;

            if (DB.VerificarLogin(user, pass))
            {
                FormAdministrador frmAdministrador = new FormAdministrador();
                frmAdministrador.usuario = user;
                //this.Hide();
                //frmAdministrador.Owner = this;
                this.Visible = false;
                frmAdministrador.Show(this);
            }
            else
            {
                MessageBox.Show("El usuario o la contraseña es incorrecto");
                //txtUsuario.Focus();
                txtUsuario.Select();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void MostrarForm()
        {
            this.txtPass.Text = "";
            this.txtUsuario.Text = "";
            this.txtUsuario.Focus();
            this.txtUsuario.Select();
            this.Visible = true;
            this.Show();
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            txtUsuario.SelectAll();
        }

        private void txtPass_Enter(object sender, EventArgs e)
        {
            txtPass.SelectAll();
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Enter)
            {
                btnAceptar_Click(sender, e);
            }
        }
    }
}
