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
    public partial class FormGastos : Form
    {
        public Gasto gastoActual;

        public FormGastos()
        {
            InitializeComponent();
            //txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

            //cargo checklist con los propietarios
            checklistPropietarios.DataSource = DB.GetData("SELECT id, nombre FROM Propietarios");
            checklistPropietarios.DisplayMember = "nombre";
            checklistPropietarios.ValueMember = "id";
            dtpFecha.Enabled = false;
        }

        public FormGastos(int id): this()
        {
            this.Text = "Modificar Gasto";
            this.checklistPropietarios.Enabled = false;
            this.btnSeleccionarCheck.Enabled = false;
            this.btnDeseleccionarCheck.Enabled = false;
            try
            {
                DataTable data = DB.GetData("SELECT * FROM Gastos WHERE id = " + id);
                if (data != null)
                {
                    foreach (DataRow row in data.Rows)
                    {
                        this.txtMonto.Text = decimal.Round(decimal.Parse(row["monto"].ToString()), 2).ToString();
                        this.txtDescripcion.Text = row["descripcion"].ToString();
                        this.dtpFecha.Value = (DateTime)row["fecha"];
                    }
                }

                //data = DB.GetData("SELECT * FROM Gastos_Propietarios WHERE id = " + id);
                //foreach (DataRow row in data.Rows)
                //{
                //    for (int i = 0; i < checklistPropietarios.Items.Count; i++)
                //    {
                //        if (checklistPropietarios.Items[i] == row["id"])
                //        {
                //            checklistPropietarios.SetItemChecked(i, true);
                //        }
                //    }
                //}
            }
            catch (Exception e)
            {
                Archivos.LogError(e);
            }
        }

        private void btnDeseleccionarCheck_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checklistPropietarios.Items.Count; i++)
            {
                checklistPropietarios.SetItemChecked(i, false);
            }
        }

        private void btnSeleccionarCheck_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < checklistPropietarios.Items.Count; i++)
            {
                checklistPropietarios.SetItemChecked(i, true);
            }
        }

        private void txtMonto_KeyPress(object sender, KeyPressEventArgs e)
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMonto.Text != "" && txtDescripcion.Text != "")
                {
                    // this.Text == "Gastos e Impuestos" && 
                     
                    string monto = txtMonto.Text;
                    string descripcion = txtDescripcion.Text;
                    DateTime fecha = dtpFecha.Value;

                    if (monto[0] == '-')
                    {
                        monto = monto.TrimStart('-');
                    }

                    Gasto unGasto = new Gasto(descripcion, fecha, decimal.Parse(monto));
                    this.gastoActual = unGasto;


                    if (this.Text == "Gastos e Impuestos")
                    {
                        if (checklistPropietarios.CheckedItems.Count >= 1)
                        {
                        
                            DataTable data = null;
                            int idGasto = 0;

                            //DB.InsertarEnGastos(this.gastoActual.descripcion, this.gastoActual.fecha, this.gastoActual.monto);
                            DB.InsertarEnDB("INSERT INTO Gastos (descripcion, fecha, monto) VALUES('" + this.gastoActual.descripcion + "', CONVERT(datetime, '" + this.gastoActual.fecha + "'), CONVERT(decimal, " + this.gastoActual.monto + "))");

                            foreach (object item in checklistPropietarios.CheckedItems)
                            {
                                DataRowView row = item as DataRowView;
                                data = DB.GetData("SELECT TOP 1 id FROM Gastos ORDER BY id DESC");
                                foreach (DataRow row1 in data.Rows)
                                {
                                    idGasto = (int)row1["id"];
                                }
                                DB.InsertarEnDB("INSERT INTO Gastos_Propietarios (id_gasto, id_propietario) VALUES(" + idGasto + ", " + row["id"].ToString() + ")");
                            }
                            MessageBox.Show("Gasto correctamente asignado", "Gasto asignado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtMonto.Text = "";
                            txtDescripcion.Text = "";
                            // deselecciono checkedlist
                            for (int i = 0; i < checklistPropietarios.Items.Count; i++)
                            {
                                checklistPropietarios.SetItemChecked(i, false);
                            }
                        
                        }
                        else
                        {
                            MessageBox.Show("Seleccione a quien se debe asignar el gasto", "Error");
                        }
                    }
                    else
                    {
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

        private void FormGastos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
