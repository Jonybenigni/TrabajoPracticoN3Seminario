using Bombones2025.DatosSql.Repositorios;
using Bombones2025.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombones2025.Servicios.Servicios
{
    public class FormaDePagoServicio
    {
        private readonly FormaDePagoRepositorio _formadepagoRepositorio = null!;
        public FormaDePagoServicio()
        {
            _formadepagoRepositorio = new FormaDePagoRepositorio(true);
        }

        public bool Existe(FormaDePago formaPago)
        {
            return _formadepagoRepositorio.Existe(formaPago);
        }

        public List<FormaDePago> GetLista()
        {
            return _formadepagoRepositorio.GetLista();
        }

        public void Guardar(FormaDePago formaPago)
        {
            if (formaPago.FormaDePagoId == 0)
            {
                _formadepagoRepositorio.Agregar(formaPago);
            }
            else
            {
                _formadepagoRepositorio.Editar(formaPago);
            }
        }

        public void Borrar(int formaPagoId)
        {
            _formadepagoRepositorio.Borrar(formaPagoId);
        }
    }
}
