using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    /// <summary>
    /// Classe que controla acesso a dados e realiza mapeamento de backtest.
    /// </summary>
    public class BacktestDAO : BaseDAO
    {
        #region Construtor

        public BacktestDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Metodos Read

        /// <summary>
        /// Retorna os testes de um usuário.
        /// </summary>
        /// <param name="cliente">Cliente desejado.</param>
        /// <param name="macroCliente">Macro cliente ao qual o cliente pertence.</param>
        /// <returns></returns>
        public List<BacktestDTO> RetornaTodosPorCliente(UsuarioDTO user)
        {
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                string query =
                    @"
                 SELECT 
                    B.BATE_CD_BACKTESTING, BATE_NM_ATIVO, BATE_IN_CONSIDERAR_CORRET_EMOLUMENTOS,
                    BATE_DT_INICIO, BATE_DT_TERMINO, BATE_IN_LIQUIDAR_POSICAO_FIM_PERIODO,
                    BATE_NM_BACKTESTING, BATE_NM_OBSERVACAO, BATE_VL_PERC_PRECO_ENTRADA,
                    BATE_VL_PERC_PRECO_SAIDA, BATE_VL_PERIODICIDADE, BATE_IN_PERMITIR_OPERAR_DESCOBERTO,
                    BATE_IN_SAIR_EM_STOP_GAIN, BATE_IN_SAIR_EM_STOP_LOSS, BATE_IN_TIPO_PRECO,
                    BATE_VL_EMOLUMENTO, BATE_VL_EXPOSICAO_MAXIMA, BATE_VL_VOLUME_FINANC_INICIAL,
                    BATE_IN_STATUS, BATE_VL_RES_MAX, BATE_VL_RES_MIN, BATE_VL_RES_MED, BATE_VL_RES_FINAL,
                    BATE_QT_STOP_GAIN, BATE_QT_STOP_LOSS, BATE_QT_OP_BEM_SUC, BATE_QT_OP_MAL_SUC, 
                    BATE_QT_TRADES, BATE_VL_POSICAO_FINAL, C.COND_CD_CONDICAO, COND_NM_COMANDO,
                    COND_NM_CONDICAO, BTCV_IN_TIPO_CONDICAO, CP.COPA_CD_CONDICAO_PARCELA,
                    COPA_NM_PARCELA, C.COND_CD_CONDICAO, COPA_IN_TIPO_APRESENTACAO,
                    COPA_IN_TIPO_FISICO, BTCV_VL_DOUBLE, BTCV_VL_INTEIRO, BTCV_NM_STRING, NUM.NUM_PARCELAS, BATE_VL_RES_TOTAL
                FROM 
                    BACKTESTING B
                    INNER JOIN BACKTESTING_CONDICAO_VALOR BCV ON BCV.BATE_CD_BACKTESTING = B.BATE_CD_BACKTESTING 
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
                    B.USUA_CD_USUARIO = ?user
                ORDER BY
                    B.BATE_CD_BACKTESTING DESC, BCV.BTCV_CD_CONDICAO_VALOR";

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(query, readConnection))
                {
                    command.Parameters.AddWithValue("?user", user.Id);

                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        return Mapeia(reader);
                    }
                    else
                        return new List<BacktestDTO>();
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
        /// Retorna o teste por id
        /// </summary>
        /// <param name="cliente">Cliente desejado.</param>
        /// <param name="macroCliente">Macro cliente ao qual o cliente pertence.</param>
        /// <returns></returns>
        public List<BacktestDTO> RetornaBackTestPorId(int id)
        {
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                string query =
                    @"
                SELECT 
                    B.BATE_CD_BACKTESTING, BATE_NM_ATIVO, BATE_IN_CONSIDERAR_CORRET_EMOLUMENTOS,
                    BATE_DT_INICIO, BATE_DT_TERMINO, BATE_IN_LIQUIDAR_POSICAO_FIM_PERIODO,
                    BATE_NM_BACKTESTING, BATE_NM_OBSERVACAO, BATE_VL_PERC_PRECO_ENTRADA,
                    BATE_VL_PERC_PRECO_SAIDA, BATE_VL_PERIODICIDADE, BATE_IN_PERMITIR_OPERAR_DESCOBERTO,
                    BATE_IN_SAIR_EM_STOP_GAIN, BATE_IN_SAIR_EM_STOP_LOSS, BATE_IN_TIPO_PRECO,
                    BATE_VL_EMOLUMENTO, BATE_VL_EXPOSICAO_MAXIMA, BATE_VL_VOLUME_FINANC_INICIAL,
                    BATE_IN_STATUS, BATE_VL_RES_MAX, BATE_VL_RES_MIN, BATE_VL_RES_MED, BATE_VL_RES_FINAL,
                    BATE_QT_STOP_GAIN, BATE_QT_STOP_LOSS, BATE_QT_OP_BEM_SUC, BATE_QT_OP_MAL_SUC, 
                    BATE_QT_TRADES, BATE_VL_POSICAO_FINAL, C.COND_CD_CONDICAO, COND_NM_COMANDO,
                    COND_NM_CONDICAO, BTCV_IN_TIPO_CONDICAO, CP.COPA_CD_CONDICAO_PARCELA,
                    COPA_NM_PARCELA, C.COND_CD_CONDICAO, COPA_IN_TIPO_APRESENTACAO,
                    COPA_IN_TIPO_FISICO, BTCV_VL_DOUBLE, BTCV_VL_INTEIRO, BTCV_NM_STRING, BATE_VL_RES_TOTAL
                FROM 
                    BACKTESTING B
                    INNER JOIN backtesting_condicao_valor BCV ON BCV.BTCV_CD_BACKTEST = B.BATE_CD_BACKTESTING 
                    INNER JOIN condicao C ON BCV.COND_CD_CONDICAO = C.COND_CD_CONDICAO
                    INNER JOIN condicao_parcela CP ON BCV.COPA_CD_CONDICAO_PARCELA = CP.COPA_CD_CONDICAO_PARCELA
                WHERE
                    B.BATE_CD_BACKTESTING = " + id.ToString();


                //Executando a query
                using (MySqlCommand command = new MySqlCommand(query, readConnection))
                {
                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        return Mapeia(reader);
                    }
                    else
                        return new List<BacktestDTO>();
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
        /// Retorna o sumario de um teste.
        /// </summary>
        /// <param name="id">Id do teste.</param>
        /// <returns></returns>
        public SumarioDTO RetornaSumarioPorBacktesting(int id)
        {
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                string query =
                    @"
                SELECT 
                    BATE_VL_RES_MAX, BATE_VL_RES_MIN, BATE_VL_RES_MED, 
                    BATE_VL_RES_FINAL, BATE_QT_STOP_GAIN, BATE_QT_STOP_LOSS, 
                    BATE_QT_OP_BEM_SUC, BATE_QT_OP_MAL_SUC, BATE_QT_TRADES, 
                    BATE_VL_POSICAO_FINAL, BATE_VL_RES_TOTAL
                FROM 
                    BACKTESTING B
                WHERE
                    B.BATE_CD_BACKTESTING = " + id.ToString();


                //Executando a query
                using (MySqlCommand command = new MySqlCommand(query, readConnection))
                {
                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        return MapeiaSumario(reader);
                    }
                    else
                        return null;
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
        public List<BacktestDTO> Mapeia(MySqlDataReader reader)
        {
            List<BacktestDTO> backTests = new List<BacktestDTO>();

            BacktestDTO backTest = null;
            CondicaoDTO condicaoDTO = null;
            CondicaoParcelaDTO condicaoParcelaDTO = null;

            int idBackTestIteracao = -1;
            int idCondicaoIteracao = -1;

            while (reader.Read())
            {
                if (idBackTestIteracao != reader.GetInt32("BATE_CD_BACKTESTING"))
                {
                    idBackTestIteracao = reader.GetInt32("BATE_CD_BACKTESTING");

                    backTest = new BacktestDTO();
                    backTest.Id = idBackTestIteracao;
                    backTest.Ativo = reader.GetString("BATE_NM_ATIVO");
                    backTest.ConsiderarCorretagemMaisEmolumento = reader.GetBoolean("BATE_IN_CONSIDERAR_CORRET_EMOLUMENTOS");
                    backTest.DataInicio = reader.GetDateTime("BATE_DT_INICIO");
                    backTest.DataTermino = reader.GetDateTime("BATE_DT_TERMINO");
                    backTest.LiquidarPosicaoFinalPeriodo = reader.GetBoolean("BATE_IN_LIQUIDAR_POSICAO_FIM_PERIODO");
                    backTest.Nome = reader.GetString("BATE_NM_BACKTESTING");
                    backTest.Observacao = reader.GetString("BATE_NM_OBSERVACAO");
                    backTest.PercentualStopLoss = reader.GetDouble("BATE_VL_PERC_PRECO_ENTRADA");
                    backTest.PercentualStopGain = reader.GetDouble("BATE_VL_PERC_PRECO_SAIDA");
                    backTest.Periodicidade = reader.GetInt32("BATE_VL_PERIODICIDADE");
                    backTest.PermitirOperacaoDescoberto = reader.GetBoolean("BATE_IN_PERMITIR_OPERAR_DESCOBERTO");
                    backTest.SairEmStopGain = reader.GetBoolean("BATE_IN_SAIR_EM_STOP_GAIN");
                    backTest.SairEmStopLoss = reader.GetBoolean("BATE_IN_SAIR_EM_STOP_LOSS");
                    backTest.TipoPreco = reader.GetInt32("BATE_IN_TIPO_PRECO");
                    backTest.ValorCorretagem = reader.GetDouble("BATE_VL_EMOLUMENTO");
                    backTest.ValorExposicaoMaxima = reader.GetDouble("BATE_VL_EXPOSICAO_MAXIMA");
                    backTest.VolumeFinanceiroInicial = reader.GetDouble("BATE_VL_VOLUME_FINANC_INICIAL");
                    backTest.Status = reader.GetInt32("BATE_IN_STATUS");

                    backTest.ResultadoMaximo = reader.GetDouble("BATE_VL_RES_MAX");
                    backTest.ResultadoMinimo = reader.GetDouble("BATE_VL_RES_MIN");
                    backTest.ResultadoMedio = reader.GetDouble("BATE_VL_RES_MED");
                    backTest.ResultadoFinal = reader.GetDouble("BATE_VL_RES_FINAL");
                    backTest.QtdStopGain = reader.GetInt32("BATE_QT_STOP_GAIN");
                    backTest.QtdStopLoss = reader.GetInt32("BATE_QT_STOP_LOSS");
                    backTest.OpBemSucedidas = reader.GetInt32("BATE_QT_OP_BEM_SUC");
                    backTest.OpMalSucedidas = reader.GetInt32("BATE_QT_OP_MAL_SUC");
                    backTest.QtdTrades = reader.GetInt32("BATE_QT_TRADES");
                    backTest.PosicaoFinal = reader.GetInt32("BATE_VL_POSICAO_FINAL");

                    backTest.ResultadoTotal = reader.GetDouble("BATE_VL_RES_TOTAL");

                    backTest.CondicoesEntrada = new List<CondicaoDTO>();
                    backTest.CondicoesSaida = new List<CondicaoDTO>();

                    backTests.Add(backTest);

                    idCondicaoIteracao = -1;
                }

                //Mapeando condicao
                if (idCondicaoIteracao != reader.GetInt32("COND_CD_CONDICAO"))
                {
                    idCondicaoIteracao = reader.GetInt32("COND_CD_CONDICAO");

                    condicaoDTO = new CondicaoDTO();
                    condicaoDTO.Id = reader.GetInt32("COND_CD_CONDICAO");
                    condicaoDTO.Comando = reader.GetString("COND_NM_COMANDO");
                    condicaoDTO.Nome = reader.GetString("COND_NM_CONDICAO");

                    if ((CondicaoParcelaDTO.TipoCondicaoEnum)reader.GetInt32("BTCV_IN_TIPO_CONDICAO") == CondicaoParcelaDTO.TipoCondicaoEnum.Entrada)
                        backTest.CondicoesEntrada.Add(condicaoDTO);
                    else
                        backTest.CondicoesSaida.Add(condicaoDTO);
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
                        if (reader.IsDBNull(reader.GetOrdinal("BTCV_NM_STRING")))
                            condicaoParcelaDTO.ValorString = null;
                        else
                            condicaoParcelaDTO.ValorString = reader.GetString("BTCV_NM_STRING");
                        break;
                }

                condicaoDTO.ListaParcelas.Add(condicaoParcelaDTO);

                ////Garantindo que testes com condicoes repetidas sejam mapeadas corretamente
                //if (Convert.ToInt32(linha[41]) == condicaoDTO.ListaParcelas.Count)
                //    idCondicaoIteracao = -1;
            }

            return backTests;
        }

        /// <summary>
        /// Mapeia um DataRow em uma DTO de sumário.
        /// </summary>
        /// <param name="linha">Tupla a ser mapeada.</param>
        /// <returns></returns>
        private SumarioDTO MapeiaSumario(MySqlDataReader reader)
        {
            SumarioDTO sumario = new SumarioDTO();

            sumario.ResultadoMaximo = reader.GetDouble("BATE_VL_RES_MAX");
            sumario.ResultadoMinimo = reader.GetDouble("BATE_VL_RES_MIN");
            sumario.ResultadoMedio = reader.GetDouble("BATE_VL_RES_MED");
            sumario.ResultadoFinal = reader.GetDouble("BATE_VL_RES_FINAL");
            sumario.QtdStopGain = reader.GetInt32("BATE_QT_STOP_GAIN");
            sumario.QtdStopLoss = reader.GetInt32("BATE_QT_STOP_LOSS");
            sumario.OpBemSucedidas = reader.GetInt32("BATE_QT_OP_BEM_SUC");
            sumario.OpMalSucedidas = reader.GetInt32("BATE_QT_OP_MAL_SUC");
            sumario.QtdTrades = reader.GetInt32("BATE_QT_TRADES");
            sumario.PosicaoFinal = reader.GetInt32("BATE_VL_POSICAO_FINAL");
            sumario.ResultadoTotal = reader.GetDouble("BATE_VL_RES_TOTAL");

            return sumario;
        }

        #endregion

        #region Metodos Write

        /// <summary>
        /// Metodo que faz a exclusao de um backtest
        /// </summary>
        /// <param name="backTest"></param>
        public void ExcluirBackTest(BacktestDTO backTest)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //excluindo operações
                using (ResultadoBacktestDAO operacaoDAO = new ResultadoBacktestDAO(readConnection, writeConnection))
                {
                    operacaoDAO.ExcluirPorBacktesting(backTest.Id);
                }

                //excluindo as condições valor
                using (CondicaoValorBackTestDAO condicaoValorDAO = new CondicaoValorBackTestDAO(readConnection, writeConnection))
                {
                    condicaoValorDAO.ExcluirCondicoesValorBackTest(backTest.Id);
                }

                //excluindo o backtest
                hql = new StringBuilder();
                hql.Append(" DELETE FROM BACKTESTING WHERE BATE_CD_BACKTESTING = " + backTest.Id);

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
        /// Metodo que faz a inserção do backtes
        /// </summary>
        /// <param name="backTest"></param>
        public void InserirBackTest(BacktestDTO backTest)
        {
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append("INSERT INTO BACKTESTING ( BATE_NM_ATIVO, BATE_IN_CONSIDERAR_CORRET_EMOLUMENTOS, BATE_DT_INICIO,");
                sql.Append(" BATE_DT_TERMINO, BATE_IN_LIQUIDAR_POSICAO_FIM_PERIODO, BATE_NM_BACKTESTING, ");
                sql.Append(" BATE_NM_OBSERVACAO, BATE_VL_PERC_PRECO_ENTRADA, BATE_VL_PERC_PRECO_SAIDA, BATE_VL_PERIODICIDADE, ");
                sql.Append(" BATE_IN_PERMITIR_OPERAR_DESCOBERTO, BATE_IN_SAIR_EM_STOP_GAIN, BATE_IN_SAIR_EM_STOP_LOSS, ");
                sql.Append(" BATE_IN_TIPO_PRECO, BATE_VL_EMOLUMENTO, BATE_VL_EXPOSICAO_MAXIMA, BATE_VL_VOLUME_FINANC_INICIAL,");
                sql.Append(" BATE_IN_STATUS, BATE_VL_RES_MAX, BATE_VL_RES_MIN, BATE_VL_RES_MED, BATE_VL_RES_FINAL,");
                sql.Append(" BATE_VL_RES_TOTAL, BATE_QT_STOP_GAIN, BATE_QT_STOP_LOSS, BATE_QT_OP_BEM_SUC, BATE_QT_OP_MAL_SUC,");
                sql.Append(" BATE_QT_TRADES, BATE_VL_POSICAO_FINAL, USUA_CD_USUARIO) VALUES (");
                sql.Append(" ?Ativo, ?ConsiderarCorretagemMaisEmolumento, ?DataInicio, ?DataTermino, ?LiquidarPosicaoFinalPeriodo, ");
                sql.Append(" ?Nome, ?Observacao, ?PercentualStopLoss, ?PercentualStopGain, ?Periodicidade, ?PermitirOperacaoDescoberto, ");
                sql.Append(" ?SairEmStopGain, ?SairEmStopLoss, ?TipoPreco, ?ValorCorretagem, ?ValorExposicaoMaxima, ?VolumeFinanceiroInicial,");
                sql.Append(" ?Status, ?ResultadoMaximo, ?ResultadoMinimo, ?ResultadoMedio, ?ResultadoFinal, ?ResultadoTotal, ?QtdStopGain, ");
                sql.Append(" ?QtdStopLoss, ?OpBemSucedidas, ?OpMalSucedidas, ?QtdTrades, ?PosicaoFinal,?usuario )");

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(sql.ToString(), writeConnection))
                {
                    //adicionando parametros
                    command.Parameters.AddWithValue("?Ativo", backTest.Ativo);
                    command.Parameters.AddWithValue("?ConsiderarCorretagemMaisEmolumento", backTest.ConsiderarCorretagemMaisEmolumento);
                    command.Parameters.AddWithValue("?DataInicio", backTest.DataInicio);
                    command.Parameters.AddWithValue("?DataTermino", backTest.DataTermino);
                                        
                    command.Parameters.AddWithValue("?LiquidarPosicaoFinalPeriodo", backTest.LiquidarPosicaoFinalPeriodo);
                    command.Parameters.AddWithValue("?Nome", backTest.Nome);
                    command.Parameters.AddWithValue("?Observacao", backTest.Observacao);

                    command.Parameters.AddWithValue("?PercentualStopLoss", backTest.PercentualStopLoss);
                    command.Parameters.AddWithValue("?PercentualStopGain", backTest.PercentualStopGain);
                    command.Parameters.AddWithValue("?Periodicidade", backTest.Periodicidade);
                    command.Parameters.AddWithValue("?PermitirOperacaoDescoberto", backTest.PermitirOperacaoDescoberto);

                    command.Parameters.AddWithValue("?SairEmStopGain", backTest.SairEmStopGain);
                    command.Parameters.AddWithValue("?SairEmStopLoss", backTest.SairEmStopLoss);
                    command.Parameters.AddWithValue("?TipoPreco", backTest.TipoPreco);
                    command.Parameters.AddWithValue("?ValorCorretagem", backTest.ValorCorretagem);

                    command.Parameters.AddWithValue("?ValorExposicaoMaxima", backTest.ValorExposicaoMaxima);
                    command.Parameters.AddWithValue("?VolumeFinanceiroInicial", backTest.VolumeFinanceiroInicial);
                    command.Parameters.AddWithValue("?usuario", backTest.Usuario.Id);
                    command.Parameters.AddWithValue("?Status", backTest.Status);

                    if (Double.IsNaN(backTest.ResultadoTotal))
                        backTest.ResultadoTotal = 0;
                    command.Parameters.AddWithValue("?ResultadoMaximo", backTest.ResultadoMaximo);

                    if (Double.IsNaN(backTest.ResultadoMinimo))
                        backTest.ResultadoMinimo = 0;
                    command.Parameters.AddWithValue("?ResultadoMinimo", backTest.ResultadoMinimo);

                    if (Double.IsNaN(backTest.ResultadoMedio))
                        backTest.ResultadoMedio = 0;
                    command.Parameters.AddWithValue("?ResultadoMedio", backTest.ResultadoMedio);

                    if (Double.IsNaN(backTest.ResultadoFinal))
                        backTest.ResultadoFinal = 0;
                    command.Parameters.AddWithValue("?ResultadoFinal", backTest.ResultadoFinal);

                    if (Double.IsNaN(backTest.ResultadoTotal))
                        backTest.ResultadoTotal = 0;
                    command.Parameters.AddWithValue("?ResultadoTotal", backTest.ResultadoTotal);

                    command.Parameters.AddWithValue("?QtdStopGain", backTest.QtdStopGain);
                    command.Parameters.AddWithValue("?QtdStopLoss", backTest.QtdStopLoss);
                    command.Parameters.AddWithValue("?OpBemSucedidas", backTest.OpBemSucedidas);

                    command.Parameters.AddWithValue("?OpMalSucedidas", backTest.OpMalSucedidas);
                    command.Parameters.AddWithValue("?QtdTrades", backTest.QtdTrades);
                    command.Parameters.AddWithValue("?PosicaoFinal", backTest.PosicaoFinal);

                    command.ExecuteNonQuery();
                    backTest.Id = Convert.ToInt32(command.LastInsertedId);
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que atualiza o sumario
        /// </summary>
        /// <param name="backtest"></param>
        public void AtualizarSumario(BacktestDTO backtest)
        {
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append("UPDATE BACKTESTING SET ");
                sql.Append(" BATE_VL_RES_MAX = ?maximo, ");
                sql.Append(" BATE_VL_RES_MIN = ?minimo, ");
                sql.Append(" BATE_VL_RES_MED = ?media, ");
                sql.Append(" BATE_VL_RES_FINAL = ?final, ");
                sql.Append(" BATE_VL_RES_TOTAL = ?total, ");
                sql.Append(" BATE_QT_STOP_GAIN = ?stopgain, ");
                sql.Append(" BATE_QT_STOP_LOSS = ?stoploss, ");
                sql.Append(" BATE_QT_OP_BEM_SUC = ?opbemsucedida, ");
                sql.Append(" BATE_QT_OP_MAL_SUC = ?opmalsucedida, ");
                sql.Append(" BATE_QT_TRADES = ?quttrades, ");
                sql.Append(" BATE_VL_POSICAO_FINAL = ?posicaofinal ");
                sql.Append(" WHERE BATE_CD_BACKTESTING = ?id");

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(sql.ToString(), writeConnection))
                {
                    //adicionando parametros
                    if (Double.IsNaN(backtest.ResultadoMaximo))
                        backtest.ResultadoMaximo = 0;
                    command.Parameters.AddWithValue("?maximo", backtest.ResultadoMaximo);

                    if (Double.IsNaN(backtest.ResultadoMinimo))
                        backtest.ResultadoMinimo = 0;
                    command.Parameters.AddWithValue("?minimo", backtest.ResultadoMinimo);

                    if (Double.IsNaN(backtest.ResultadoMedio))
                        backtest.ResultadoMedio = 0;
                    command.Parameters.AddWithValue("?media", backtest.ResultadoMedio);

                    if (Double.IsNaN(backtest.ResultadoFinal))
                        backtest.ResultadoFinal = 0;
                    command.Parameters.AddWithValue("?final", backtest.ResultadoFinal);

                    if (Double.IsNaN(backtest.ResultadoTotal))
                        backtest.ResultadoTotal = 0;
                    command.Parameters.AddWithValue("?total", backtest.ResultadoTotal);


                    command.Parameters.AddWithValue("?stopgain", backtest.QtdStopGain);
                    command.Parameters.AddWithValue("?stoploss", backtest.QtdStopLoss);
                    command.Parameters.AddWithValue("?opbemsucedida", backtest.OpBemSucedidas);
                    command.Parameters.AddWithValue("?opmalsucedida", backtest.OpMalSucedidas);
                    command.Parameters.AddWithValue("?quttrades", backtest.QtdTrades);
                    command.Parameters.AddWithValue("?posicaofinal", backtest.PosicaoFinal);
                    command.Parameters.AddWithValue("?id", backtest.Id);

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
