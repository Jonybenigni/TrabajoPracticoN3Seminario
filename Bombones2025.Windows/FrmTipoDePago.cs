using Bombones2025.Entidades.Entidades;
using Bombones2025.Servicios.Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bombones2025.Windows
{
    public partial class FrmTipoDePago : Form
    {
        private readonly FormaDePagoServicio _servicio = null!;
        private List<FormaDePago> lista = null!;
        public FrmTipoDePago(FormaDePagoServicio servicio)
        {
            InitializeComponent();
            _servicio = servicio;
        }

        private void FrmTipoDePago_Load(object sender, EventArgs e)
        {
            try
            {
                lista = _servicio.GetLista();
                MostrarDatosEnGrilla();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void MostrarDatosEnGrilla()
        {
            dgvDatos.Rows.Clear();
            foreach (FormaDePago fs in lista)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(dgvDatos);
                SetearFila(r, fs);
                AgregarFila(r);
            }
        }

        private void AgregarFila(DataGridViewRow r)
        {
            dgvDatos.Rows.Add(r);
        }

        private void SetearFila(DataGridViewRow r, FormaDePago formaPago)
        {
            r.Cells[0].Value = formaPago.FormaDePagoId;
            r.Cells[1].Value = formaPago.Descripcion;

            r.Tag = formaPago;
        }

        private DataGridViewRow ConstuirFila()
        {
            var r = new DataGridViewRow();
            r.CreateCells(dgvDatos);
            return r;
        }

        private void QuitarFila(DataGridViewRow r)
        {
            dgvDatos.Rows.Remove(r);
        }


        private void TsbNuevo_Click(object sender, EventArgs e)
        {
            FrmFormasDePagoAE frm = new FrmFormasDePagoAE() { Text = "Agregar Tipo de Pago" };
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel) return;
            FormaDePago? formaPago = frm.GetFormaDePago();
            if (formaPago is null) return;
            try
            {
                if (!_servicio.Existe(formaPago))
                {
                    _servicio.Guardar(formaPago);
                    DataGridViewRow r = ConstuirFila();
                    SetearFila(r, formaPago);
                    AgregarFila(r);
                    MessageBox.Show("Registro Agregado", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Registro Duplicado", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TsbBorrar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0) return;
            DataGridViewRow r = dgvDatos.SelectedRows[0];
            FormaDePago? fs = r.Tag as FormaDePago;
            if (fs is null) return;
            DialogResult dr = MessageBox.Show($"¿Desea borrar el registro de {fs}?",
                "Confirmar Baja",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.No) return;
            try
            {
                _servicio.Borrar(fs.FormaDePagoId);
                QuitarFila(r);
                MessageBox.Show("Registro Eliminado", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TsbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0) return;
            DataGridViewRow r = dgvDatos.SelectedRows[0];
            FormaDePago? fs = r.Tag as FormaDePago;
            if (fs is null) return;
            FormaDePago? fsEditar = fs.Clonar();
            FrmFormasDePagoAE frm = new FrmFormasDePagoAE() { Text = "Editar Formas de Pago" };
            frm.SetFormaDePago(fsEditar);
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel) return;
            fsEditar = frm.GetFormaDePago();
            if (fsEditar is null) return;
            try
            {
                if (!_servicio.Existe(fsEditar))
                {
                    _servicio.Guardar(fsEditar);
                    SetearFila(r, fsEditar);
                    MessageBox.Show("Registro Editado", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Registro Duplicado", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TsbActualizar_Click(object sender, EventArgs e)
        {

        }
    }
}
