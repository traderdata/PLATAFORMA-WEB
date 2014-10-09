using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;
using MySql.Data.MySqlClient;


namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class UsuarioLogadoDAO : BaseDAO
    {
        #region Construtor

        public UsuarioLogadoDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)        
        { }

        #endregion

        #region Metodos Write

        /// <summary>
        /// Metodo que loga que o usuario esta logado
        /// </summary>
        /// <param name="usuario"></param>
        public void Ping(UsuarioLogadoDTO usuario)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //salvando o template
                hql = new StringBuilder();
                hql.Append(" INSERT INTO USUARIO_LOGADO (USLO_CD_USUARIO, USLO_DT_USUARIO_LOGADO)");
                hql.Append(" VALUES ( ?usuario, ?data )");


                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //Setando os parametros
                    command.Parameters.AddWithValue("?usuario", usuario.Usuario);
                    command.Parameters.AddWithValue("?data", DateTime.Now);                    
                    command.ExecuteNonQuery();
                }
                                
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        #endregion
    }
}
