using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class MensagemDAO:BaseDAO
    {
        #region Construtor

        public MensagemDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Metodos Write

        /// <summary>
        /// Metodo que insere mensagem de chat
        /// </summary>
        /// <param name="id"></param>
        public void InsereMensagem(MensagemDTO mensagemDTO)
        {
            StringBuilder hql = new StringBuilder();

            try
            {

                hql = new StringBuilder();
                hql.Append(" INSERT INTO MENSAGEM (USUA_CD_USUARIO, MENS_DT_MENSAGEM, MENS_TX_MENSAGEM) VALUES (");
                hql.Append(" ?usuario, ?data, ?texto)");
                
                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), writeConnection))
                {
                    command.Parameters.AddWithValue("?usuario", mensagemDTO.UsuarioId);
                    command.Parameters.AddWithValue("?data", DateTime.Now);
                    command.Parameters.AddWithValue("?texto", mensagemDTO.Mensagem);
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
