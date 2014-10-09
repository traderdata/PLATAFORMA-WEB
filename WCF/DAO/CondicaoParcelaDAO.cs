using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class CondicaoParcelaDAO:BaseDAO
    {
        #region Construtor

        public CondicaoParcelaDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Metodos Read

        /// <summary>
        /// Metodo que retorna as parcelas de determinada condição
        /// </summary>
        /// <param name="condicaoId"></param>
        /// <returns></returns>
        public List<CondicaoParcelaDTO> GetParcelaByCondicao(int condicaoId)
        {
            StringBuilder sSql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Query
                sSql.Append(" SELECT * FROM CONDICAO_PARCELA C ");
                sSql.Append(" WHERE C.COND_CD_CONDICAO = ?condicao ");
                sSql.Append(" ORDER BY C.COPA_CD_CONDICAO_PARCELA ");

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(sSql.ToString(), readConnection))
                {
                    //Setando objetis
                    command.Parameters.AddWithValue("?condicao", condicaoId);

                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<CondicaoParcelaDTO> listaAux = new List<CondicaoParcelaDTO>();

                        while (reader.Read())
                        {
                            CondicaoParcelaDTO condicaoParcela = new CondicaoParcelaDTO();
                            condicaoParcela.Id = reader.GetInt32("COPA_CD_CONDICAO_PARCELA");
                            condicaoParcela.CondicaoId = reader.GetInt32("COND_CD_CONDICAO");
                            condicaoParcela.Nome = reader.GetString("COPA_NM_PARCELA");
                            condicaoParcela.TipoApresentacao = reader.GetString("COPA_IN_TIPO_APRESENTACAO");
                            condicaoParcela.TipoFisico = reader.GetString("COPA_IN_TIPO_FISICO");

                            listaAux.Add(condicaoParcela);
                        }

                        return listaAux;
                    }
                    else
                        return new List<CondicaoParcelaDTO>();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                reader.Close();
            }

        }

        #endregion

        #region Metodos Write
        #endregion
    }
}
