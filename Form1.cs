using CRUDbanco.Datos;
using CRUDbanco.Dominio;
using CRUDbanco.Servicios.Implementacion;
using CRUDbanco.Servicios.Interfaz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDbanco
{
    public partial class FrmAlta : Form
    {
        private IServicio gestor;
        private Cliente nuevo;
        public FrmAlta()
        {
            InitializeComponent();
            gestor = new Servicio();
            nuevo = new Cliente();
        }

        private void FrmAlta_Load(object sender, EventArgs e)
        {
            ObtenerProximo();
            ObtenerTipos();

        }

        private void ObtenerTipos()
        {
            cboTipo.ValueMember = "id_tipo_cuenta";
            cboTipo.DisplayMember = "tipoCuenta";
            cboTipo.DataSource = gestor.ObtenerTipos();
            cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void ObtenerProximo()
        {
            int next = gestor.ObtenerProximo();
            if(next > 0)
            {
                lblCliente.Text = "Cliente N°: " + next.ToString();
            }
            else
            {
                MessageBox.Show("No se puede obtener el numero de cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if(cboTipo.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un tipo de cuenta", "Control", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            if(txtCBU.Text == "")
            {
                MessageBox.Show("Ingrese un cbu", "Control", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            foreach(DataGridViewRow dr in dgvCuentas.Rows)
            {
                if (dr.Cells["ColTipo"].Value.ToString().Equals(cboTipo.Text))
                {
                    MessageBox.Show("El tipo de cuenta: " + cboTipo.Text + " ya es parte de la lista", "Control", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            int cbu = int.Parse(txtCBU.Text);
            double saldo = double.Parse(txtSaldo.Text);
            TipoCuenta t = (TipoCuenta)cboTipo.SelectedItem;
            DateTime fecha = dtpUltimo.Value;

            Cuenta c = new Cuenta(cbu, saldo, t, fecha);
            nuevo.AgregarCuenta(c);
            dgvCuentas.Rows.Add(c.Tipo.id_tipo_cuenta, c.Cbu, c.Saldo, c.Tipo.tipoCuenta, c.Ultimo);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if(txtApellido.Text == "")
            {
                MessageBox.Show("Ingrese un apellido", "Control", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            GuardarCliente();
        }

        private void GuardarCliente()
        {
            nuevo.Apellido = txtApellido.Text;
            nuevo.Nombre = txtNombre.Text;
            nuevo.Dni = int.Parse(txtDNI.Text);

            if(Helper.ObtenerInstancia().ConfirmarCliente(nuevo))
            {
                MessageBox.Show("Se inserto el cliente con exito", "Confirmacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else
            {
                MessageBox.Show("No se puede insertar el cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void dgvCuentas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvCuentas.CurrentCell.ColumnIndex == 5)
            {
                nuevo.EliminarCuenta(dgvCuentas.CurrentRow.Index);
                dgvCuentas.Rows.Remove(dgvCuentas.CurrentRow);
            }
        }
    }
}
