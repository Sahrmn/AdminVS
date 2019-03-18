using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Entidades;

namespace Formularios
{
    public delegate void DelegadoCambiarColor();
    public partial class FormPrincipal : Form
    {
        public string usuario;
        public DelegadoCambiarColor delColor;

        public FormPrincipal()
        {
            InitializeComponent();
            btnRestaurar.Visible = false;
            delColor = new DelegadoCambiarColor(this.CambiarColoresBotones);
            this.usuario = "SAMI";
        }

        //public event DelegadoCambiarColor EventoCambioColorBotones;

        #region Formulario

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("¿Esta seguro de querer salir?", "Salir", MessageBoxButtons.OKCancel) == DialogResult.OK)
                Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //METODO PARA ARRASTRAR EL FORMULARIO---------------------------------------------------------------------
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelSuperior_MouseMove_1(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }


        //METODO PARA ABRIR FORMULARIOS DENTRO DEL PANEL CONTENEDOR
        //private void AbrirFormulario<MiUC>() where MiUC : UserControl, new()
        //{
        //    // Busca en la coleccion el formulario
        //    UserControl usercontrol = panelFormularios.Controls.OfType<MiUC>().FirstOrDefault();

        //    //si la instancia del formulario no existe
        //    if (usercontrol == null)
        //    {
        //        usercontrol = new MiUC();
        //        //usercontrol.TopLevel = false;
        //        panelFormularios.Controls.Add(usercontrol);
        //        panelFormularios.Tag = usercontrol;
        //        //usercontrol.FormBorderStyle = FormBorderStyle.None;
        //        usercontrol.Dock = DockStyle.Fill;
        //        usercontrol.Show();
        //        usercontrol.BringToFront();
        //    }
        //    // si la instancia del formulario existe
        //    else
        //    {
        //        usercontrol.BringToFront();//llevar al frente
        //    }
        //}

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            this.btnMaximizar.Visible = false;
            this.btnRestaurar.Visible = true;
            this.WindowState = FormWindowState.Maximized;
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.btnRestaurar.Visible = false;
            this.btnMaximizar.Visible = true;
        }

        #endregion

        private void btnGastos_Click(object sender, EventArgs e)
        {
            UserControl userControl = panelFormularios.Controls.OfType<UCMostrar>().FirstOrDefault();

            //si la instancia del formulario no existe
            if (userControl == null)
            {
                userControl = new UCMostrar("Gastos");
                panelFormularios.Controls.Add(userControl);
                panelFormularios.Tag = userControl;
                userControl.Dock = DockStyle.Fill;
                userControl.Show();
                userControl.BringToFront();
            }
            // si la instancia del formulario existe
            else
            {
                this.delColor.Invoke();
                userControl.BringToFront();//llevar al frente
            }

            btnGastos.BackColor = Color.FromArgb(22, 34, 56);

        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            UserControl userControl = panelFormularios.Controls.OfType<UCMostrar>().FirstOrDefault();

            //si la instancia del formulario no existe
            if (userControl == null)
            {
                userControl = new UCMostrar("Ventas");
                panelFormularios.Controls.Add(userControl);
                panelFormularios.Tag = userControl;
                userControl.Dock = DockStyle.Fill;
                userControl.Show();
                userControl.BringToFront();
            }
            // si la instancia del formulario existe
            else
            {
                this.delColor.Invoke();
                userControl.BringToFront();//llevar al frente
            }

            btnVentas.BackColor = Color.FromArgb(22, 34, 56);
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            UserControl userControl = panelFormularios.Controls.OfType<UCMostrar>().FirstOrDefault();

            //si la instancia del formulario no existe
            if (userControl == null)
            {
                userControl = new UCMostrar("Productos");
                panelFormularios.Controls.Add(userControl);
                panelFormularios.Tag = userControl;
                userControl.Dock = DockStyle.Fill;
                userControl.Show();
                userControl.BringToFront();
            }
            // si la instancia del formulario existe
            else
            {
                this.delColor.Invoke();
                userControl.BringToFront();//llevar al frente
            }

            btnProductos.BackColor = Color.FromArgb(22, 34, 56);
        }

        private void btnPropietarios_Click(object sender, EventArgs e)
        {
            UserControl userControl = panelFormularios.Controls.OfType<UCMostrar>().FirstOrDefault();

            //si la instancia del formulario no existe
            if (userControl == null)
            {
                userControl = new UCMostrar("Propietarios");
                panelFormularios.Controls.Add(userControl);
                panelFormularios.Tag = userControl;
                userControl.Dock = DockStyle.Fill;
                userControl.Show();
                userControl.BringToFront();
            }
            // si la instancia del formulario existe
            else
            {
                this.delColor.Invoke();
                userControl.BringToFront();//llevar al frente
            }

            btnPropietarios.BackColor = Color.FromArgb(22, 34, 56);
        }

        private void btnDistribuidores_Click(object sender, EventArgs e)
        {
            UserControl userControl = panelFormularios.Controls.OfType<UCMostrar>().FirstOrDefault();

            //si la instancia del formulario no existe
            if (userControl == null)
            {
                userControl = new UCMostrar("Distribuidores");
                panelFormularios.Controls.Add(userControl);
                panelFormularios.Tag = userControl;
                userControl.Dock = DockStyle.Fill;
                userControl.Show();
                userControl.BringToFront();
            }
            // si la instancia del formulario existe
            else
            {
                this.delColor.Invoke();
                userControl.BringToFront();//llevar al frente
            }

            btnDistribuidores.BackColor = Color.FromArgb(22, 34, 56);
        }

        private void btnCategorias_Click(object sender, EventArgs e)
        {
            UserControl userControl = panelFormularios.Controls.OfType<UCMostrar>().FirstOrDefault();

            //si la instancia del formulario no existe
            if (userControl == null)
            {
                userControl = new UCMostrar("Categorias");
                panelFormularios.Controls.Add(userControl);
                panelFormularios.Tag = userControl;
                userControl.Dock = DockStyle.Fill;
                userControl.Show();
                userControl.BringToFront();
            }
            // si la instancia del formulario existe
            else
            {
                this.delColor.Invoke();
                userControl.BringToFront();//llevar al frente
            }

            btnCategorias.BackColor = Color.FromArgb(22, 34, 56);
        }

        private void btnFacturacion_Click(object sender, EventArgs e)
        {
            UserControl userControl = panelFormularios.Controls.OfType<UCVentas>().FirstOrDefault();

            //si la instancia del formulario no existe
            if (userControl == null)
            {
                userControl = new UCVentas(this.usuario);
                panelFormularios.Controls.Add(userControl);
                panelFormularios.Tag = userControl;
                userControl.Dock = DockStyle.Fill;
                userControl.Show();
                userControl.BringToFront();
            }
            // si la instancia del formulario existe
            else
            {
                this.delColor.Invoke();
                userControl.BringToFront();//llevar al frente
            }

            btnFacturacion.BackColor = Color.FromArgb(22, 34, 56);
        }

        private void CambiarColoresBotones()
        {
            btnVentas.BackColor = Color.FromArgb(50, 67, 89);
            btnCategorias.BackColor = Color.FromArgb(50, 67, 89);
            btnDistribuidores.BackColor = Color.FromArgb(50, 67, 89);
            btnPropietarios.BackColor = Color.FromArgb(50, 67, 89);
            btnGastos.BackColor = Color.FromArgb(50, 67, 89);
            btnProductos.BackColor = Color.FromArgb(50, 67, 89);
            btnFacturacion.BackColor = Color.FromArgb(50, 67, 89);
        }

        private void panelFormularios_ControlRemoved(object sender, ControlEventArgs e)
        {
            CambiarColoresBotones();
        }

        private void btnCaja_Click(object sender, EventArgs e)
        {
            UserControl userControl = panelFormularios.Controls.OfType<UCCaja>().FirstOrDefault();

            //si la instancia del formulario no existe
            if (userControl == null)
            {
                userControl = new UCCaja();
                panelFormularios.Controls.Add(userControl);
                panelFormularios.Tag = userControl;
                userControl.Dock = DockStyle.Fill;
                userControl.Show();
                userControl.BringToFront();
            }
            // si la instancia del formulario existe
            else
            {
                this.delColor.Invoke();
                userControl.BringToFront();//llevar al frente
            }

            btnCaja.BackColor = Color.FromArgb(22, 34, 56);
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Esta seguro de querer cerrar sesion?", "Cerrar Sesion", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Hide();//cierra al formulario anterior -- revisar
                            //((FormLogin)this.Owner).Visible = true;
                IForm formInterface = this.Owner as IForm;
                if (formInterface != null)
                {
                    formInterface.MostrarForm();
                }
            }
        }
    }
}
