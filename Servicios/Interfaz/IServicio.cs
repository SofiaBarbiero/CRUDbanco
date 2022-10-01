using CRUDbanco.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDbanco.Servicios.Interfaz
{
    interface IServicio
    {
        int ObtenerProximo();
        List<TipoCuenta> ObtenerTipos();
    }
}
