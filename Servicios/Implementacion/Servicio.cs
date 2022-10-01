using CRUDbanco.Datos.Implementacion;
using CRUDbanco.Datos.Interfaz;
using CRUDbanco.Dominio;
using CRUDbanco.Servicios.Interfaz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDbanco.Servicios.Implementacion
{
    class Servicio : IServicio
    {
        private IDaoClientes dao;

        public Servicio()
        {
            dao = new DaoClientes();
        }
        public int ObtenerProximo()
        {
            return dao.ObtenerProximo();
        }

        public List<TipoCuenta> ObtenerTipos()
        {
            return dao.ObtenerTipos();
        }
    }
}
