using CRUDbanco.Datos.Interfaz;
using CRUDbanco.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDbanco.Datos.Implementacion
{
    class DaoClientes : IDaoClientes
    {
        public int ObtenerProximo()
        {
            string sp = "SP_PROXIMO_ID";
            string pOut = "@next";
            return Helper.ObtenerInstancia().ObtenerProximo(sp, pOut);
        }

        public List<TipoCuenta> ObtenerTipos()
        {
            List<TipoCuenta> lst = new List<TipoCuenta>();
            string sp = "SP_LISTAR_TIPOS_CUENTAS";
            DataTable table = Helper.ObtenerInstancia().CargarCombo(sp);
            foreach(DataRow dr in table.Rows)
            {
                int id = int.Parse(dr["id_tipo_cuenta"].ToString());
                string tipo = dr["nombre_cuenta"].ToString();
                TipoCuenta t = new TipoCuenta(id, tipo);
                lst.Add(t);
            }
            return lst;
        }
    }
}
