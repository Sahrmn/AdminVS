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
    public partial class UCMostrar : UserControl
    {
        public int id_usuario;

        public UCMostrar()
        {
            InitializeComponent();
            this.id_usuario = 0;
        }

        public UCMostrar(string mostrar): this()
        {
            CargaDataGrid(mostrar);
        }

        public UCMostrar(string mostrar, string user): this(mostrar)
        {
            dgvDatos.ReadOnly = true;
            btnEliminar.Enabled = false;
            btnAgregar.Enabled = false;
            int id = 0;
            try
            {
                DataTable data = DB.GetData("SELECT id FROM Propietarios WHERE nombre = '" + user + "'");
                foreach (DataRow row in data.Rows)
                {
                    id = int.Parse(row["id"].ToString());
                }
                dgvDatos.Rows[id].Selected = true;
                dgvDatos.CurrentCell = dgvDatos.Rows[id - 1].Cells[0];
                this.id_usuario = id;
                if (this.id_usuario == 1)
                {
                    btnAgregar.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Archivos.LogError(ex);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                switch (this.Text)
                {
                    case "Productos":
                        FormProducto frmProducto = new FormProducto();
                        frmProducto.ShowDialog(this);
                        if (frmProducto.DialogResult == DialogResult.OK)
                        {
                            Producto prod = frmProducto.prod;
                            string consulta = "INSERT INTO Productos (nombre, id_categoria, id_distribuidor, precio) VALUES(";
                            consulta += "'" + prod.Nombre + "'," + prod.IdCategoria.ToString() + "," + prod.IdDistribuidor.ToString() + "," + prod.Precio.ToString() + ")";
                            try
                            {
                                DB.InsertarEnDB(consulta);
                                CargaDataGrid("Productos");
                            }
                            catch
                            {
                                MessageBox.Show("Error al agregar.");
                            }
                        }
                        break;
                    case "Propietarios":
                        if (id_usuario == 1)
                        {
                            FormPropietario frmPropietario = new FormPropietario();
                            frmPropietario.ShowDialog(this);
                            if (frmPropietario.DialogResult == DialogResult.OK)
                            {
                                Propietario prod = frmPropietario.prop;
                                string consulta = "INSERT INTO Propietarios (nombre, contraseña) VALUES(";
                                consulta += "'" + prod.nombre + "','" + prod.pass + "')";
                                try
                                {
                                    DB.InsertarEnDB(consulta);
                                    CargaDataGrid("Propietarios");
                                }
                                catch
                                {
                                    MessageBox.Show("Error al agregar.");
                                }
                            }
                        }
                        break;
                    case "Distribuidores":
                        FormDistribuidores frmDistribuidores = new FormDistribuidores();
                        frmDistribuidores.ShowDialog(this);
                        if (frmDistribuidores.DialogResult == DialogResult.OK)
                        {
                            Distribuidor prod = frmDistribuidores.dist;
                            string consulta = "INSERT INTO Distribuidores (nombre, descripcion) VALUES(";
                            consulta += "'" + prod.nombre + "','" + prod.descripcion + "')";
                            try
                            {
                                DB.InsertarEnDB(consulta);
                                CargaDataGrid("Distribuidores");
                            }
                            catch
                            {
                                MessageBox.Show("Error al agregar.");
                            }
                        }
                        break;
                    case "Ventas":

                        break;
                    case "Gastos":
                        FormGastos frmGastos = new FormGastos();
                        frmGastos.ShowDialog(this);
                        DataTable data = CargaGastos();
                        dgvDatos.DataSource = data;
                        break;
                    case "Categorias":
                        FormCategoria frmCategoria = new FormCategoria();
                        frmCategoria.ShowDialog(this);
                        if (frmCategoria.DialogResult == DialogResult.OK)
                        {
                            Categoria cat = frmCategoria.cat;
                            string consulta = "INSERT INTO Categorias (nombre, id_propietario) VALUES(";
                            consulta += "'" + cat.nombre + "'," + cat.id_propietario + ")";
                            try
                            {
                                DB.InsertarEnDB(consulta);
                                CargaDataGrid("Categorias");
                            }
                            catch
                            {
                                MessageBox.Show("Error al agregar.");
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Archivos.LogError(ex);
                //MessageBox.Show("Ocurrio un error. Reintente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargaDataGrid(string mostrar)
        {
            DataTable data = null;
            DataTable data2 = null;
            string dato = "";
            switch (mostrar)
            {
                case "Productos":
                    this.Text = "Productos";
                    data = CargaProductos();
                    dgvDatos.DataSource = data;
                    dgvDatos.Columns["id_distribuidor"].Visible = false;
                    dgvDatos.Columns["id_categoria"].Visible = false;
                    break;
                case "Propietarios":
                    this.Text = "Propietarios";
                    data = DB.GetData("SELECT id, nombre FROM Propietarios");
                    //dgvDatos.DataSource = data;
                    //dgvDatos.Columns["id"].Visible = false;
                    break;
                case "Distribuidores":
                    this.Text = "Distribuidores";
                    data = DB.GetData("SELECT * FROM Distribuidores");
                    break;
                case "Gastos":
                    this.Text = "Gastos";
                    this.btnModificar.Visible = true;
                    data = CargaGastos();
                    dgvDatos.DataSource = data;
                    dgvDatos.Columns["id"].Visible = false;
                    break;
                case "Ventas":
                    this.Text = "Ventas";
                    this.btnAgregar.Visible = false;
                    this.btnModificar.Visible = false;
                    this.btnBuscar.Visible = false;
                    data = DB.GetData("SELECT * FROM Ventas");
                    break;
                case "Categorias":
                    this.Text = "Categorias";
                    data = DB.GetData("SELECT * FROM Categorias");

                    //en vez de mostrar solo los id, muestro el nombre de los propietarios
                    data.Columns.Add("Propietario");
                    foreach (DataRow row in data.Rows)
                    {
                        data2 = DB.GetData("SELECT p.nombre FROM Propietarios p INNER JOIN Categorias c ON p.id = c.id_propietario WHERE p.id = " + row["id_propietario"].ToString());
                        foreach (DataRow row1 in data2.Rows)
                        {
                            dato = row1["nombre"].ToString();
                        }
                        row["Propietario"] = dato;
                    }
                    dgvDatos.DataSource = data;
                    dgvDatos.Columns["id_propietario"].Visible = false;
                    break;
                default:
                    break;
            }
            dgvDatos.DataSource = data;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            int id = CapturoID();
            switch (this.Text)
            {
                case "Productos":
                    FormProducto frmProducto = new FormProducto(id);
                    frmProducto.ShowDialog(this);
                    if (frmProducto.DialogResult == DialogResult.OK)
                    {
                        Producto prod = frmProducto.prod;
                        string consulta = "UPDATE Productos SET nombre = '" + prod.nombre + "', id_categoria = " + prod.IdCategoria.ToString() + ", id_distribuidor = " + prod.IdDistribuidor.ToString() + ", precio = " + prod.precio.ToString() + " WHERE id = " + id;
                        try
                        {
                            DB.ModificarEnDB(consulta);
                            CargaDataGrid("Productos");
                        }
                        catch
                        {
                            MessageBox.Show("Error al modificar.");
                        }
                    }
                    break;
                case "Propietarios":
                    if (this.id_usuario == id)
                    {
                        FormPropietario frmPropietario = new FormPropietario(id);
                        frmPropietario.ShowDialog(this);
                        if (frmPropietario.DialogResult == DialogResult.OK)
                        {
                            Propietario prop = frmPropietario.prop;
                            string consulta = "UPDATE Propietarios SET nombre = '" + prop.nombre + "', contraseña = '" + prop.pass + "' WHERE id = " + id;
                            try
                            {
                                DB.ModificarEnDB(consulta);
                                CargaDataGrid("Propietarios");
                            }
                            catch
                            {
                                MessageBox.Show("Error al modificar.");
                            }
                        }
                    }
                    break;
                case "Distribuidores":
                    FormDistribuidores frmDistribuidores = new FormDistribuidores(id);
                    frmDistribuidores.ShowDialog(this);
                    if (frmDistribuidores.DialogResult == DialogResult.OK)
                    {
                        Distribuidor dist = frmDistribuidores.dist;
                        string consulta = "UPDATE Distribuidores SET nombre = '" + dist.nombre + "', descripcion = '" + dist.descripcion + "' WHERE id = " + id;
                        try
                        {
                            DB.ModificarEnDB(consulta);
                            CargaDataGrid("Distribuidores");
                        }
                        catch
                        {
                            MessageBox.Show("Error al modificar.");
                        }
                    }
                    break;
                case "Ventas":

                    break;
                case "Gastos":
                    FormGastos frmGastos = new FormGastos(id);
                    frmGastos.ShowDialog(this);
                    if (frmGastos.DialogResult == DialogResult.OK)
                    {
                        //REVISAR --> NO TERMINADO, REVISAR INSERCION CORRECTA DE FECHA!!!



                        Gasto gast = frmGastos.gastoActual;

                        string consulta = "UPDATE Gastos SET descripcion = '" + gast.descripcion + "', fecha = CONVERT(datetime, '" + gast.fecha + "'), monto = CONVERT(decimal, " + gast.monto + ") WHERE id = " + id;
                        try
                        {
                            DB.ModificarEnDB(consulta);

                            CargaDataGrid("Gastos");
                        }
                        catch
                        {
                            MessageBox.Show("Error al modificar.");
                        }
                    }
                    DataTable data = CargaGastos();
                    dgvDatos.DataSource = data;
                    break;
                case "Categorias":
                    FormCategoria frmCategoria = new FormCategoria(id);
                    frmCategoria.ShowDialog(this);
                    if (frmCategoria.DialogResult == DialogResult.OK)
                    {
                        Categoria cat = frmCategoria.cat;
                        string consulta = "UPDATE Categorias SET nombre = '" + cat.nombre + "', id_propietario = " + cat.id_propietario + " WHERE id = " + id;
                        try
                        {
                            DB.ModificarEnDB(consulta);
                            CargaDataGrid("Categorias");
                        }
                        catch
                        {
                            MessageBox.Show("Error al modificar.");
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private int CapturoID()
        {
            int filaSelect = dgvDatos.CurrentCell.RowIndex;//capturo la fila seleccionada
            object data = dgvDatos[0, filaSelect].Value;//a partir de la fila obtengo el valor de la columna 0(id)
            return (int)data;
        }

        private void dgvDatos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dgvDatos.Rows[e.RowIndex].Selected = true;
            }
            catch
            {
                //no pasa nada
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = CapturoID();
            switch (this.Text)
            {
                case "Productos":
                    if (MessageBox.Show("¿Esta seguro de querer eliminar la celda?", "Eliminar celda", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        string consulta = "DELETE FROM Productos WHERE id = " + id;
                        try
                        {
                            DB.EliminarDeDB(consulta);
                            CargaDataGrid("Productos");
                        }
                        catch
                        {
                            MessageBox.Show("Error al eliminar.");
                        }
                    }
                    break;
                case "Propietarios":
                    if (MessageBox.Show("¿Esta seguro de querer eliminar la celda?", "Eliminar celda", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        string consulta = "DELETE FROM Propietarios WHERE id = " + id;
                        try
                        {
                            DB.EliminarDeDB(consulta);
                            CargaDataGrid("Propietarios");
                        }
                        catch
                        {
                            MessageBox.Show("Error al eliminar.");
                        }
                    }
                    break;
                case "Distribuidores":
                    if (MessageBox.Show("¿Esta seguro de querer eliminar la celda?", "Eliminar celda", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        string consulta = "DELETE FROM Distribuidores WHERE id = " + id;
                        try
                        {
                            DB.EliminarDeDB(consulta);
                            CargaDataGrid("Distribuidores");
                        }
                        catch
                        {
                            MessageBox.Show("Error al eliminar.");
                        }
                    }
                    break;
                case "Ventas":
                    if (MessageBox.Show("¿Esta seguro de querer eliminar la celda?", "Eliminar celda", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        string consulta = "DELETE FROM Ventas WHERE id = " + id;
                        try
                        {
                            DB.EliminarDeDB(consulta);
                            CargaDataGrid("Ventas");
                        }
                        catch
                        {
                            MessageBox.Show("Error al eliminar.");
                        }
                    }
                    break;
                case "Categorias":
                    if (MessageBox.Show("¿Esta seguro de querer eliminar la celda?", "Eliminar celda", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        string consulta = "DELETE FROM Categorias WHERE id = " + id;
                        try
                        {
                            DB.EliminarDeDB(consulta);
                            CargaDataGrid("Categorias");
                        }
                        catch
                        {
                            MessageBox.Show("Error al eliminar.");
                        }
                    }
                    break;
                case "Gastos":
                    if (MessageBox.Show("¿Esta seguro de querer eliminar la celda?", "Eliminar celda", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        string consulta = "DELETE FROM Gastos WHERE id = " + id;
                        try
                        {
                            DB.EliminarDeDB(consulta);
                            //eliminar de gastos_propietarios
                            DB.EliminarDeDB("DELETE FROM Gastos_Propietarios WHERE id_gasto = " + id);

                            CargaDataGrid("Gastos");
                        }
                        catch
                        {
                            MessageBox.Show("Error al eliminar.");
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            switch (this.Text)
            {
                case "Productos":
                    UCBuscar frmBuscar = new UCBuscar("Productos");
                    frmBuscar.Show();
                    break;
                case "Propietarios":
                    UCBuscar frmBuscar = new UCBuscar("Propietarios");
                    frmBuscar.Show();
                    break;
                case "Distribuidores":
                    UCBuscar frmBuscar = new UCBuscar("Distribuidores");
                    frmBuscar.Show();
                    break;
                case "Ventas":
                    //FormBuscar frmBuscarV = new FormBuscar("Ventas");
                    //frmBuscarV.ShowDialog(this);
                    break;
                case "Categorias":
                    UCBuscar frmBuscar = new UCBuscar("Categorias");
                    frmBuscar.Show();
                    break;
                default:
                    break;
            }
        }

        public void EstablecerSeleccionDGV(int id)
        {
            dgvDatos.Rows[id].Selected = true;
            dgvDatos.CurrentCell = dgvDatos.Rows[id - 1].Cells[0];
        }

        //private void FormMostrar_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyValue == (char)Keys.Escape)
        //    {
        //        this.Close();
        //    }
        //}

        private void btnStock_Click(object sender, EventArgs e)
        {
            FormCargaStock frmCargaStock = new FormCargaStock(CapturoID());
            frmCargaStock.ShowDialog(this);
        }

        private DataTable CargaGastos()
        {
            DataTable data = DB.GetData("SELECT * FROM Gastos");
            data.Columns.Add("Asignado a");
            string propietarios = "";
            DataTable data3 = null;
            DataTable data2 = null;

            foreach (DataRow row in data.Rows)
            {
                data2 = DB.GetData("SELECT * FROM Gastos_Propietarios WHERE id_gasto = " + row["id"].ToString());
                foreach (DataRow row2 in data2.Rows)
                {
                    data3 = DB.GetData("SELECT nombre FROM Propietarios WHERE id = " + row2["id_propietario"].ToString());
                    foreach (DataRow row3 in data3.Rows)
                    {
                        if (propietarios == "")
                        {
                            propietarios += row3["nombre"].ToString();
                        }
                        else
                        {
                            propietarios += "- " + row3["nombre"].ToString();
                        }

                    }
                }
                row["Asignado a"] = propietarios;
                propietarios = "";
            }
            return data;
        }

        private DataTable CargaProductos()
        {
            DataTable data = DB.GetData("SELECT * FROM Productos");
            DataTable data2 = null;
            string dato = "";
            btnStock.Visible = true;

            //muestro nombres en vez de id's
            data.Columns.Add("Stock");
            data.Columns.Add("Categoria");
            data.Columns.Add("Distribuidor");

            int stock = 0;
            foreach (DataRow row in data.Rows)
            {
                stock = Producto.CalcularStock((int)row["id"]);
                row["Stock"] = stock;
                data2 = DB.GetData("SELECT c.nombre FROM Categorias c INNER JOIN Productos p ON p.id_categoria = c.id WHERE p.id_categoria = " + row["id_categoria"].ToString());
                foreach (DataRow row1 in data2.Rows)
                {
                    dato = row1["nombre"].ToString();
                }
                row["Categoria"] = dato;
                data2 = DB.GetData("SELECT d.nombre FROM Distribuidores d INNER JOIN Productos p ON p.id_distribuidor = d.id WHERE p.id_distribuidor = " + row["id_distribuidor"].ToString());
                foreach (DataRow row1 in data2.Rows)
                {
                    dato = row1["nombre"].ToString();
                }
                row["Distribuidor"] = dato;
            }

            return data;
        }
    }
}

