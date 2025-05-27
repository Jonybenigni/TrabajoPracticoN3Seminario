using Bombones2025.Entidades.Entidades;
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
    public partial class FrmFormasDePago : Form
    {
        private FormaDePago? formadepago;
        public FrmFormasDePago()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (formadepago is not null)
            {
                txtFormaDePago.Text = formadepago.Descripcion;
            }
        }

        public FormaDePago? GetFormaDePago()
        {
            return formadepago;
        }
        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (formadepago is null)
                {
                    formadepago = new FormaDePago();

                }
                formadepago.Descripcion = txtFormaDePago.Text;

                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtFormaDePago.Text))
            {
                valido = false;
                errorProvider1.SetError(txtFormaDePago, "El nombre es requerido");

            }
            return valido;
        }

        public void SetFormaDepago(FormaDePago formadepago)
        {
            this.formadepago = formadepago;

        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
