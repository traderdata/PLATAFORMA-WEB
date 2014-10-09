using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    /// <summary>
    /// Classe que controla acesso a dados e realiza mapeamento de templates.
    /// </summary>
    public class TemplateBacktestDAO : BaseDAO
    {

        #region Construtor

        public TemplateBacktestDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Metodos Read

        /// <summary>
        /// Retorna todos os templates de um usuário.
        /// </summary>
        /// <param name="cliente">Cliente desejado.</param>
        /// <param name="macroCliente">Macro cliente que contém este cliente.</param>
        /// <returns></returns>
        public List<TemplateBacktestDTO> RetornaTodosPorCliente(UsuarioDTO user)
        {
            MySql.Data.MySqlClient.MySqlDataReader reader = null;
            List<TemplateBacktestDTO> listaTemplate = new List<TemplateBacktestDTO>();

            try
            {
                string query =
                    @"
                SELECT 
                    TEBA_CD_TEMPLATE_BACKTESTING, TEBA_NM_ATIVO, TEBA_IN_CONSIDERAR_CORRET_EMOLUMENTOS,
                    TEBA_DT_INICIO, TEBA_DT_TERMINO, TEBA_IN_LIQUIDAR_POSICAO_FIM_PERIODO,
                    TEBA_NM_TEMPLATE, TEBA_NM_OBSERVACAO, TEBA_VL_PERC_PRECO_ENTRADA,
                    TEBA_VL_PERC_PRECO_SAIDA, TEBA_VL_PERIODICIDADE, TEBA_IN_PERMITIR_OPERAR_DESCOBERTO,
                    TEBA_IN_SAIR_EM_STOP_GAIN, TEBA_IN_SAIR_EM_STOP_LOSS, TEBA_IN_TIPO_PRECO,
                    TEBA_VL_EMOLUMENTO, TEBA_VL_EXPOSICAO_MAXIMA, TEBA_VL_VOLUME_FINANC_INICIAL,
                    C.COND_CD_CONDICAO, COND_NM_COMANDO, COND_NM_CONDICAO,
                    BTCV_IN_TIPO_CONDICAO, CP.COPA_CD_CONDICAO_PARCELA, COPA_NM_PARCELA,
                    C.COND_CD_CONDICAO, COPA_IN_TIPO_APRESENTACAO, COPA_IN_TIPO_FISICO,
                    BTCV_VL_DOUBLE, BTCV_VL_INTEIRO, BTCV_NM_STRING, USUA_CD_USUARIO, NUM.NUM_PARCELAS
                FROM 
                    TEMPLATE_BACKTESTING B
                    INNER JOIN BACKTESTING_CONDICAO_VALOR BCV ON BCV.TEBA_CD_TEMPLATE = B.TEBA_CD_TEMPLATE_BACKTESTING 
                    INNER JOIN CONDICAO C ON BCV.COND_CD_CONDICAO = C.COND_CD_CONDICAO
                    INNER JOIN CONDICAO_PARCELA CP ON BCV.COPA_CD_CONDICAO_PARCELA = CP.COPA_CD_CONDICAO_PARCELA
                    INNER JOIN
                    (
                        SELECT
                            COND_CD_CONDICAO,
                            COUNT(*) AS NUM_PARCELAS
                        FROM
                            CONDICAO_PARCELA
                        GROUP BY
                            COND_CD_CONDICAO
                    ) NUM ON NUM.COND_CD_CONDICAO = C.COND_CD_CONDICAO
                WHERE
                    B.USUA_CD_USUARIO = ?cliente
                ORDER BY
                    B.TEBA_CD_TEMPLATE_BACKTESTING DESC, BCV.BTCV_CD_CONDICAO_VALOR";



                //Executando a query
                using (MySqlCommand command = new MySqlCommand(query, readConnection))
                {
                    //setando os parametros
                    command.Parameters.AddWithValue("?cliente", user.Id);
                    
                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        return Mapeia(reader);
                    }
                    else
                        return new List<TemplateBacktestDTO>();
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

        /// <summary>
        /// Mapeia um DataTable em uma lista de DTO.
        /// </summary>
        /// <param name="dados">Dados a serem mapeados.</param>
        /// <returns></returns>
        public List<TemplateBacktestDTO> Mapeia(MySqlDataReader reader)
        {
            List<TemplateBacktestDTO> templates = new List<TemplateBacktestDTO>();

            TemplateBacktestDTO TemplateBacktestDTO = null;
            CondicaoDTO condicaoDTO = null;
            CondicaoParcelaDTO condicaoParcelaDTO = null;

            int idBackTestIteracao = -1;
            int idCondicaoIteracao = -1;

            while (reader.Read())
            {
                if (idBackTestIteracao != Convert.ToInt32(reader.GetInt32("TEMP_CD_TEMPLATE_BACKTESTING")))
                {
                    idBackTestIteracao = Convert.ToInt32(reader.GetInt32("TEMP_CD_TEMPLATE_BACKTESTING"));

                    TemplateBacktestDTO = new TemplateBacktestDTO();
                    TemplateBacktestDTO.Id = idBackTestIteracao;
                    TemplateBacktestDTO.Ativo = reader.GetString("TEMP_NM_ATIVO");
                    TemplateBacktestDTO.ConsiderarCorretagemMaisEmolumento = reader.GetBoolean("TEMP_IN_CONSIDERAR_CORRET_EMOLUMENTOS");
                    TemplateBacktestDTO.DataInicio = reader.GetDateTime("TEMP_DT_INICIO");
                    TemplateBacktestDTO.DataTermino = reader.GetDateTime("TEMP_DT_TERMINO");
                    TemplateBacktestDTO.LiquidarPosicaoFinalPeriodo = reader.GetBoolean("TEMP_IN_LIQUIDAR_POSICAO_FIM_PERIODO");
                    TemplateBacktestDTO.Nome = reader.GetString("TEMP_NM_TEMPLATE");
                    TemplateBacktestDTO.Observacao = reader.GetString("TEMP_NM_OBSERVACAO");
                    TemplateBacktestDTO.PercentualStopLoss = reader.GetDouble("TEMP_VL_PERC_PRECO_ENTRADA");
                    TemplateBacktestDTO.PercentualStopGain = reader.GetDouble("TEMP_VL_PERC_PRECO_SAIDA");
                    TemplateBacktestDTO.Periodicidade = reader.GetInt32("TEMP_VL_PERIODICIDADE");
                    TemplateBacktestDTO.PermitirOperacaoDescoberto = reader.GetBoolean("TEMP_IN_PERMITIR_OPERAR_DESCOBERTO");
                    TemplateBacktestDTO.SairEmStopGain = reader.GetBoolean("TEMP_IN_SAIR_EM_STOP_GAIN");
                    TemplateBacktestDTO.SairEmStopLoss = reader.GetBoolean("TEMP_IN_SAIR_EM_STOP_LOSS");
                    TemplateBacktestDTO.TipoPreco = reader.GetInt32("TEMP_IN_TIPO_PRECO");
                    TemplateBacktestDTO.ValorCorretagem = reader.GetInt32("TEMP_VL_EMOLUMENTO");
                    TemplateBacktestDTO.ValorExposicaoMaxima = reader.GetDouble("TEMP_VL_EXPOSICAO_MAXIMA");
                    TemplateBacktestDTO.VolumeFinanceiroInicial = reader.GetInt64("TEMP_VL_VOLUME_FINANC_INICIAL");

                    TemplateBacktestDTO.CondicoesEntrada = new List<CondicaoDTO>();
                    TemplateBacktestDTO.CondicoesSaida = new List<CondicaoDTO>();

                    templates.Add(TemplateBacktestDTO);

                    idCondicaoIteracao = -1;
                }

                //Mapeando condicao
                if (idCondicaoIteracao != reader.GetInt32("COND_CD_CONDICAO"))
                {
                    idCondicaoIteracao = reader.GetInt32("COND_CD_CONDICAO");

                    condicaoDTO = new CondicaoDTO();
                    condicaoDTO.Id = Convert.ToInt32(reader.GetInt32("COND_CD_CONDICAO"));
                    condicaoDTO.Comando = reader.GetString("COND_NM_COMANDO");
                    condicaoDTO.Nome = reader.GetString("COND_NM_CONDICAO");

                    if ((CondicaoParcelaDTO.TipoCondicaoEnum)reader.GetInt32("BTCV_IN_TIPO_CONDICAO") == CondicaoParcelaDTO.TipoCondicaoEnum.Entrada)
                        TemplateBacktestDTO.CondicoesEntrada.Add(condicaoDTO);
                    else
                        TemplateBacktestDTO.CondicoesSaida.Add(condicaoDTO);
                }


                //Mapeando parcelas
                condicaoParcelaDTO = new CondicaoParcelaDTO();
                condicaoParcelaDTO.Id = reader.GetInt32("COPA_CD_CONDICAO_PARCELA");
                condicaoParcelaDTO.Nome = reader.GetString("COPA_NM_PARCELA");
                condicaoParcelaDTO.CondicaoId = reader.GetInt32("COND_CD_CONDICAO");
                condicaoParcelaDTO.TipoApresentacao = reader.GetString("COPA_IN_TIPO_APRESENTACAO");
                condicaoParcelaDTO.TipoFisico = reader.GetString("COPA_IN_TIPO_FISICO");

                switch (condicaoParcelaDTO.TipoFisicoEnumerado)
                {
                    case CondicaoParcelaDTO.TipoFisicoEnum.Double:
                        condicaoParcelaDTO.ValorDouble = reader.GetDouble("BTCV_VL_DOUBLE");
                        break;
                    case CondicaoParcelaDTO.TipoFisicoEnum.Int:
                        condicaoParcelaDTO.ValorInteiro = reader.GetInt32("BTCV_VL_INTEIRO");
                        break;
                    case CondicaoParcelaDTO.TipoFisicoEnum.String:
                        condicaoParcelaDTO.ValorString = reader.GetString("BTCV_NM_STRING");
                        break;
                }

                condicaoDTO.ListaParcelas.Add(condicaoParcelaDTO);

                //Garantindo que testes com condicoes repetidas sejam mapeadas corretamente
                if (Convert.ToInt32(reader.GetInt32("NUM_PARCELAS")) == condicaoDTO.ListaParcelas.Count)
                    idCondicaoIteracao = -1;
            }

            return templates;
        }

        #endregion

        #region Metodos Write

        /// <summary>
        ///Metodo que faz a exclusao de um template
        /// </summary>
        /// <param name="template"></param>
        public void ExcluirTemplate(TemplateBacktestDTO template)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //salvando o template
                hql = new StringBuilder();
                hql.Append(" DELETE FROM TEMPLATE WHERE TEMP_CD_TEMPLATE_BACKTESTING = " + template.Id);

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
        /// Metodo que faz a inserção de um novo template
        /// </summary>
        /// <param name="template"></param>
        public void InserirTemplate(TemplateBacktestDTO template)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //salvando o template
                hql = new StringBuilder();
                hql.Append("INSERT INTO TEMPLATE (TEMP_NM_ATIVO, TEMP_IN_CONSIDERAR_CORRET_EMOLUMENTOS, TEMP_DT_INICIO, TEMP_DT_TERMINO, ");
                hql.Append("TEMP_NM_MACRO_CLIENTE, TEMP_IN_LIQUIDAR_POSICAO_FIM_PERIODO, TEMP_NM_TEMPLATE, TEMP_NM_OBSERVACAO, ");
                hql.Append("TEMP_VL_PERC_PRECO_ENTRADA, TEMP_VL_PERC_PRECO_SAIDA, TEMP_VL_PERIODICIDADE, TEMP_IN_PERMITIR_OPERAR_DESCOBERTO, ");
                hql.Append("TEMP_IN_SAIR_EM_STOP_GAIN, TEMP_IN_SAIR_EM_STOP_LOSS, TEMP_IN_TIPO_PRECO, TEMP_VL_EMOLUMENTO, ");
                hql.Append("TEMP_VL_EXPOSICAO_MAXIMA, TEMP_VL_VOLUME_FINANC_INICIAL, TEMP_NM_CLIENTE) VALUES (");
                hql.Append("?Ativo, ?ConsiderarCorretagemMaisEmolumento, ?DataInicio, ?DataTermino, ?MacroCliente, ?LiquidarPosicaoFinalPeriodo,");
                hql.Append("?Nome, ?Observacao, ?PercentualStopLoss, ?PercentualStopGain, ?Periodicidade, ?PermitirOperacaoDescoberto,");
                hql.Append("?SairEmStopGain, ?SairEmStopLoss, ?TipoPreco, ?ValorCorretagem, ?ValorExposicaoMaxima, ");
                hql.Append("?VolumeFinanceiroInicial, ?Cliente)");


                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //adicionando os parametros
                    command.Parameters.AddWithValue("?Ativo", template.Ativo);
                    command.Parameters.AddWithValue("?ConsiderarCorretagemMaisEmolumento", template.ConsiderarCorretagemMaisEmolumento);
                    command.Parameters.AddWithValue("?DataInicio", template.DataInicio);
                    command.Parameters.AddWithValue("?DataTermino", template.DataTermino);
                    
                    command.Parameters.AddWithValue("?LiquidarPosicaoFinalPeriodo", template.LiquidarPosicaoFinalPeriodo);
                    command.Parameters.AddWithValue("?Nome", template.Nome);
                    command.Parameters.AddWithValue("?Observacao", template.Observacao);
                    command.Parameters.AddWithValue("?PercentualStopLoss", template.PercentualStopLoss);
                    command.Parameters.AddWithValue("?PercentualStopGain", template.PercentualStopGain);
                    command.Parameters.AddWithValue("?Periodicidade", template.Periodicidade);
                    command.Parameters.AddWithValue("?PermitirOperacaoDescoberto", template.PermitirOperacaoDescoberto);
                    command.Parameters.AddWithValue("?SairEmStopGain", template.SairEmStopGain);
                    command.Parameters.AddWithValue("?SairEmStopLoss", template.SairEmStopLoss);
                    command.Parameters.AddWithValue("?TipoPreco", template.TipoPreco);
                    command.Parameters.AddWithValue("?ValorCorretagem", template.ValorCorretagem);
                    command.Parameters.AddWithValue("?ValorExposicaoMaxima", template.ValorExposicaoMaxima);
                    command.Parameters.AddWithValue("?VolumeFinanceiroInicial", template.VolumeFinanceiroInicial);
                    

                    //inserindo operacao
                    command.ExecuteNonQuery();
                    template.Id = Convert.ToInt32(command.LastInsertedId);
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
