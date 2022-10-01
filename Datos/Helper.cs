using CRUDbanco.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDbanco.Datos
{
    class Helper
    {
        private static Helper instancia;
        private SqlConnection cnn;

        public Helper()
        {
            cnn = new SqlConnection(Properties.Resources.cnnString);
        }

        public static Helper ObtenerInstancia()
        {
            if(instancia == null)
            {
                instancia = new Helper();
            }
            return instancia;
        }

        public int ObtenerProximo(string nombreSp, string nombrePOut)
        {
            SqlCommand cmdProx = new SqlCommand();
            cnn.Open();
            cmdProx.Connection = cnn;
            cmdProx.CommandType = CommandType.StoredProcedure;
            cmdProx.CommandText = nombreSp;
            SqlParameter pOut = new SqlParameter();
            pOut.ParameterName = nombrePOut;
            pOut.DbType = DbType.Int32;
            pOut.Direction = ParameterDirection.Output;
            cmdProx.Parameters.Add(pOut);
            cmdProx.ExecuteNonQuery();
            cnn.Close();
            return (int)pOut.Value;
        }

        public DataTable CargarCombo(string sp)
        {
            DataTable table = new DataTable();
            SqlCommand cmdC = new SqlCommand();
            cnn.Open();
            cmdC.Connection = cnn;
            cmdC.CommandType = CommandType.StoredProcedure;
            cmdC.CommandText = sp;
            table.Load(cmdC.ExecuteReader());
            cnn.Close();
            return table;
        }

        public bool ConfirmarCliente(Cliente c)
        {
            bool ok = true;
            SqlTransaction t = null;
            try
            {
                SqlCommand cmdM = new SqlCommand();
                cnn.Open();
                t = cnn.BeginTransaction();
                cmdM.Connection = cnn;
                cmdM.Transaction = t;
                cmdM.CommandType = CommandType.StoredProcedure;
                cmdM.CommandText = "SP_INSERTAR_CLIENTE";
                cmdM.Parameters.AddWithValue("@apellido", c.Apellido);
                cmdM.Parameters.AddWithValue("@nombre", c.Nombre);
                cmdM.Parameters.AddWithValue("@dni", c.Dni);
                SqlParameter pOut = new SqlParameter();
                pOut.ParameterName = "@cliente_nro";
                pOut.DbType = DbType.Int32;
                pOut.Direction = ParameterDirection.Output;
                cmdM.Parameters.Add(pOut);
                cmdM.ExecuteNonQuery();

                int cliente_nro = (int)pOut.Value;

                foreach(Cuenta ct in c.Cuentas)
                {
                    SqlCommand cmdD = new SqlCommand();
                    cmdD.Connection = cnn;
                    cmdD.Transaction = t;
                    cmdD.CommandType = CommandType.StoredProcedure;
                    cmdD.CommandText = "SP_INSERTAR_CUENTAS";
                    cmdD.Parameters.AddWithValue("@cbu", ct.Cbu);
                    cmdD.Parameters.AddWithValue("@saldo", ct.Saldo);
                    cmdD.Parameters.AddWithValue("@ultimo_mov", ct.Ultimo);
                    cmdD.Parameters.AddWithValue("@id_tipo_cuenta", ct.Tipo.id_tipo_cuenta);
                    cmdD.Parameters.AddWithValue("@id_cliente", cliente_nro);
                    cmdD.ExecuteNonQuery();
                }
                t.Commit();
            }
            catch(Exception)
            {
                if(t != null)
                {
                    t.Rollback();
                    ok = false;
                }
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            return ok;
        }
    }
}
