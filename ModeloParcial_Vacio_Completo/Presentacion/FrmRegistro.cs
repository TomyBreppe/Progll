using ModeloParcial.Entidades;
using ModeloParcial.Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModeloParcial
{
    public partial class FrmRegistro : Form
    {
        private OrdenRetiro oOrdenRetiro;
        private GestorMateriales gestorMateriales;
        private GestorOrdenes gestorOrdenes;

        public FrmRegistro()
        {
            InitializeComponent();
            gestorMateriales = new GestorMateriales();
            gestorOrdenes = new GestorOrdenes();
            oOrdenRetiro = new OrdenRetiro();
        }

        private void FrmRegistro_Load(object sender, EventArgs e)
        {
            dtpFecha.Value = DateTime.Now;
            CargarCombo();
        }

        private void CargarCombo()
        {
            cboMateriales.DataSource = gestorMateriales.ObtenerMateriales();
            cboMateriales.DisplayMember = "nombre";
            cboMateriales.ValueMember = "codigo";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (ValidarDetalle())
            {

                Material material = (Material)cboMateriales.SelectedItem;

                int cantidad = (int)numCantidad.Value;
                int stock = material.Stock;

                // False -> True | True -> False
                if (!ValidarStock(cantidad, stock))
                {
                    // Esto se ejecuta solo si el if es True
                    return;
                }

                DetalleOrden oDetalle = new DetalleOrden(material, cantidad);

                oOrdenRetiro.AgregarDetalle(oDetalle);

                object[] row = new object[]
                {
                    material.Codigo,
                    material.Nombre,
                    material.Stock,
                    cantidad,
                    "Quitar"
                };

                dgvDetallesOrdenes.Rows.Add(row);
            }
        }

        private bool ValidarStock(int cantidad, int stock)
        {
            if (cantidad > stock)
            {
                MessageBox.Show("Stock insuficiente", "Control", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool ValidarDetalle()
        {
            if (numCantidad.Value == 0)
            {
                MessageBox.Show("Debe ingresar una cantidad", "Control", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboMateriales.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un material", "Control", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            foreach (DataGridViewRow row in dgvDetallesOrdenes.Rows)
            {
                if (row.Cells["colMaterial"].Value.ToString() == cboMateriales.Text)
                {
                    MessageBox.Show("Ese material ya fue ingresado", "Control", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (ValidarMaestro())
            {
                oOrdenRetiro.Responsable = txtResponsable.Text;
                oOrdenRetiro.Fecha = dtpFecha.Value;

                int nroOrden = gestorOrdenes.InsertarOrden(oOrdenRetiro);
                if (nroOrden != 0)
                {
                    MessageBox.Show($"Se insertó la orden {nroOrden} con exito", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("Algo ha salido mal", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void LimpiarCampos()
        {
            txtResponsable.Text = "";
            cboMateriales.SelectedIndex = -1;
            numCantidad.Value = 0;

            dgvDetallesOrdenes.Rows.Clear();
            oOrdenRetiro = new OrdenRetiro();
        }

        private bool ValidarMaestro()
        {
            if (txtResponsable.Text == String.Empty)
            {
                MessageBox.Show("Debe ingresar un responsable", "Control", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dgvDetallesOrdenes.Rows.Count == 0)
            {
                MessageBox.Show("Debe ingresar al menos un material", "Control", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void dgvDetallesOrdenes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetallesOrdenes.CurrentCell.ColumnIndex == (4))
            {
                int posicion = dgvDetallesOrdenes.CurrentRow.Index;
                oOrdenRetiro.QuitarDetalle(posicion);
                dgvDetallesOrdenes.Rows.RemoveAt(posicion);
            }
            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Seguro que quiere salir?", "Saliendo...", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            Close();
        }
    }    
}
