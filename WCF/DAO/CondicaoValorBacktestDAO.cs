using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    /// <summary>
    /// Classe que controla acesso a dados e realiza mapeamento de valores de condição-parcela.
    /// </summary>
    public class CondicaoValorBackTestDAO : BaseDAO
    {
        #region Construtor

        public CondicaoValorBackTestDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Metodos Write

        /// <summary>
        /// Metodo que inclui as condições
        /// </summary>
        /// <param name="condicoes"></param>
        /// <param name="idTemplateBackTest"></param>
        /// <param name="tipoCondicao"></param>
        /// <param name="template"></param>
        public void IncluirAPartirCondicoes(List<CondicaoDTO> condicoes, int idTemplateBackTest, CondicaoParcelaDTO.TipoCondicaoEnum tipoCondicao, bool template)
        {
            List<CondicaoValorBacktestDTO> condicoesValor = ConverteCondicoesEmCondicaoValorDTO(condicoes, idTemplateBackTest, tipoCondicao, template);

            foreach (CondicaoValorBacktestDTO obj in condicoesValor)
            {
                if (!template)
                    InserirCondicaoValorBackTest(obj);
                else
                    InserirCondicaoValorTemplate(obj);
            }
        }

        /// <summary>
        /// Exclui todas os valores-condicao de um template.
        /// </summary>
        /// <param name="idTemplate">Id do template proprietário.</param>
        public void ExcluirCondicoesValorTemplate(int idTemplate)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //salvando o template
                hql = new StringBuilder();
                hql.Append(" DELETE FROM backtesting_condicao_valor WHERE ");
                hql.Append(" TEMP_CD_TEMPLATE = " + idTemplate);


                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /// <summary>
        /// Exclui todas os valores-condicao de um backTest.
        /// </summary>
        /// <param name="idbackTest">Id do backtest proprietário.</param>
        public void ExcluirCondicoesValorBackTest(int idbackTest)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //salvando o template
                hql = new StringBuilder();
                hql.Append(" DELETE FROM BACKTESTING_CONDICAO_VALOR WHERE ");
                hql.Append(" BATE_CD_BACKTESTING = " + idbackTest);

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /// <summary>
        /// Metodo que faz a inserção de uma tupla-condição-backtest
        /// </summary>
        /// <param name="condicao"></param>
        private void InserirCondicaoValorBackTest(CondicaoValorBacktestDTO condicao)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //salvando o template
                hql = new StringBuilder();
                hql.Append(" INSERT INTO BACKTESTING_CONDICAO_VALOR (");
                hql.Append(" COND_CD_CONDICAO, BATE_CD_BACKTESTING, BTCV_VL_INTEIRO, BTCV_VL_DOUBLE, BTCV_NM_STRING,");
                hql.Append(" COPA_CD_CONDICAO_PARCELA, BTCV_IN_TIPO_CONDICAO) VALUES (");
                hql.Append(" ?IdCondicao, ?IdBackTest, ?ValorInteiro, ?ValorDouble, ?ValorString, ?IdCondicaoParcela, ?TipoCondicao)");

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //setando os parametros
                    command.Parameters.AddWithValue("?IdCondicao", condicao.IdCondicao);
                    command.Parameters.AddWithValue("?IdBackTest", condicao.IdBackTest);
                    command.Parameters.AddWithValue("?ValorInteiro", condicao.ValorInteiro);
                    command.Parameters.AddWithValue("?ValorDouble", condicao.ValorDouble);
                    command.Parameters.AddWithValue("?ValorString", condicao.ValorString);
                    command.Parameters.AddWithValue("?IdCondicaoParcela", condicao.IdCondicaoParcela);
                    command.Parameters.AddWithValue("?TipoCondicao", condicao.TipoCondicao);

                    command.ExecuteNonQuery();
                    condicao.Id = Convert.ToInt32(command.LastInsertedId);

                }

            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /// <summary>
        /// Metodo que faz a inserção de uma tupla-condição-backtest
        /// </summary>
        /// <param name="condicao"></param>
        private void InserirCondicaoValorTemplate(CondicaoValorBacktestDTO condicao)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //salvando o template
                hql = new StringBuilder();
                hql.Append(" INSERT INTO backtesting_condicao_valor (");
                hql.Append(" COND_CD_CONDICAO, TEMP_CD_TEMPLATE, BTCV_VL_INTEIRO, BTCV_VL_DOUBLE, BTCV_NM_STRING,");
                hql.Append(" COPA_CD_CONDICAO_PARCELA, BTCV_IN_TIPO_CONDICAO) VALUES (");
                hql.Append(" ?IdCondicao, ?IdTemplate, ?ValorInteiro, ?ValorDouble, ?ValorString, ?IdCondicaoParcela, ?TipoCondicao)");

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //setando os parametros
                    command.Parameters.AddWithValue("?IdCondicao", condicao.IdCondicao);
                    command.Parameters.AddWithValue("?IdTemplate", condicao.IdTemplate);
                    command.Parameters.AddWithValue("?ValorInteiro", condicao.ValorInteiro);
                    command.Parameters.AddWithValue("?ValorDouble", condicao.ValorDouble);
                    command.Parameters.AddWithValue("?ValorString", condicao.ValorString);
                    command.Parameters.AddWithValue("?IdCondicaoParcela", condicao.IdCondicaoParcela);
                    command.Parameters.AddWithValue("?TipoCondicao", condicao.TipoCondicao);

                    command.ExecuteNonQuery();
                    condicao.Id = Convert.ToInt32(command.LastInsertedId);

                }

            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        #endregion

        #region Outros

        /// <summary>
        /// Converte uma lista de condições em condições valores.
        /// </summary>
        /// <param name="condicoes"></param>
        /// <param name="idBackTestTemplate"></param>
        /// <param name="tipoCondicao"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public List<CondicaoValorBacktestDTO> ConverteCondicoesEmCondicaoValorDTO(List<CondicaoDTO> condicoes, int idBackTestTemplate, CondicaoParcelaDTO.TipoCondicaoEnum tipoCondicao, bool template)
        {
            List<CondicaoValorBacktestDTO> condicoesValor = new List<CondicaoValorBacktestDTO>();
            CondicaoValorBacktestDTO condValor = null;

            foreach (CondicaoDTO cond in condicoes)
            {
                foreach (CondicaoParcelaDTO parcela in cond.ListaParcelas)
                {
                    condValor = new CondicaoValorBacktestDTO();

                    if (template)
                        condValor.IdTemplate = idBackTestTemplate;
                    else
                        condValor.IdBackTest = idBackTestTemplate;

                    condValor.IdCondicao = cond.Id;
                    condValor.IdCondicaoParcela = parcela.Id;

                    condValor.TipoCondicao = (int)tipoCondicao;

                    condValor.ValorDouble = parcela.ValorDouble;
                    condValor.ValorInteiro = parcela.ValorInteiro;
                    condValor.ValorString = parcela.ValorString;

                    condicoesValor.Add(condValor);
                }
            }

            return condicoesValor;
        }

        #endregion
    }
}
