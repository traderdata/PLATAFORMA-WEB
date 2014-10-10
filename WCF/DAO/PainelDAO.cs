using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class PainelDAO:BaseDAO
    {
        #region Construtor

        public PainelDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Read

        #endregion

        #region Write

        /// <summary>
        /// Metodo que faz a inserção de paineis
        /// </summary>
        /// <param name="painel"></param>
        public void InsertPainel(PainelDTO painel)
        {
            try
            {
                //salvando o template
                StringBuilder hql = new StringBuilder();

                hql.Append(" INSERT INTO PAINEL (PAIN_CD_PAINEL, PAIN_PR_ALTURA, LAYO_CD_LAYOUT, PAIN_IN_TIPO, PAIN_IN_STATUS, PAIN_CD_INDEX)");
                hql.Append(" VALUES (?cd, ?altura, ?layout, ?tipopainel, ?status, ?painel)");

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //Setando os parametros
                    command.Parameters.AddWithValue("?cd", painel.Id);
                    command.Parameters.AddWithValue("?altura", painel.Altura);
                    command.Parameters.AddWithValue("?layout", painel.LayoutId);
                    command.Parameters.AddWithValue("?tipopainel", painel.TipoPainel);
                    command.Parameters.AddWithValue("?status", painel.Status);
                    command.Parameters.AddWithValue("?painel", painel.Index);

                    command.ExecuteNonQuery();
                    painel.Id = Convert.ToInt32(command.LastInsertedId);
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
