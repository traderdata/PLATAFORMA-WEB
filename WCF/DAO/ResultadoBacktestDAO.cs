using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    /// <summary>
    /// Classe que controla acesso a dados e realiza mapeamento de operações.
    /// </summary>
    public class ResultadoBacktestDAO : BaseDAO
    {
        #region Construtor

        public ResultadoBacktestDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Metodos Read

        /// <summary>
        /// Retorna operações realizadas por um backtesting.
        /// </summary>
        /// <param name="idBackTesting"></param>
        public List<ResultadoBacktestDTO> RetornaTodosPorBackTest(int idBackTesting)
        {
            MySql.Data.MySqlClient.MySqlDataReader reader = null;
            List<TemplateBacktestDTO> listaTemplate = new List<TemplateBacktestDTO>();

            try
            {
                string query =
                    @"
                    SELECT * FROM RESULTADO_BACKTESTING O WHERE 
                        BATE_CD_BACKTESTING = ?p0
                        ORDER BY REBA_DT_DATA_HORA";

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(query, readConnection))
                {
                    //setando os parametros
                    command.Parameters.AddWithValue("?p0", idBackTesting);

                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<ResultadoBacktestDTO> listaOperacao = new List<ResultadoBacktestDTO>();
                        while (reader.Read())
                        {
                            ResultadoBacktestDTO operacao = new ResultadoBacktestDTO();
                            operacao.CustodiaParcial = reader.GetDouble("REBA_QN_CUSTODIA_PARCIAL");
                            operacao.DataHora = reader.GetDateTime("REBA_DT_DATA_HORA");
                            operacao.Id = reader.GetInt32("REBA_CD_RESULTADO_BACKTESTING");
                            operacao.IdBackTest = reader.GetInt32("BATE_CD_BACKTESTING");
                            operacao.Operacao = reader.GetInt32("REBA_IN_OPERACAO");
                            //operacao.OperacaoEnumerado
                            operacao.Preco = reader.GetDouble("REBA_VL_PRECO");
                            operacao.Quantidade = reader.GetDouble("REBA_QN_QUANTIDADE");
                            operacao.Rentabilidade = reader.GetDouble("REBA_VL_RENTABILIDADE");
                            operacao.RentabilidadeAcumulada = reader.GetDouble("REBA_VL_RENTABILIDADE_ACUMULADA");
                            operacao.SaldoParcial = reader.GetDouble("REBA_VL_SALDO_PARCIAL");
                            operacao.SaldoTotal = reader.GetDouble("REBA_VL_SALDO_TOTAL");
                            operacao.StopGainAtingido = reader.GetBoolean("REBA_IN_STOP_GAIN_ATINGIDO");
                            operacao.StopLossAtingido = reader.GetBoolean("REBA_IN_STOP_LOSS_ATINGIDO");

                            listaOperacao.Add(operacao);
                        }

                        return listaOperacao;
                    }
                    else
                        return new List<ResultadoBacktestDTO>();
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

        /// <summary>
        /// Exclui resultados.
        /// </summary>
        /// <param name="idBackTesting"></param>
        public void ExcluirPorBacktesting(int idBackTesting)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //salvando o template
                hql = new StringBuilder();
                hql.Append(" DELETE FROM RESULTADO_BACKTESTING WHERE BATE_CD_BACKTESTING = " + idBackTesting);


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
        /// Metodo que faz a inserção de operação
        /// </summary>
        /// <param name="operacao"></param>
        public void InserirOperacao(ResultadoBacktestDTO operacao)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //salvando o template
                hql = new StringBuilder();
                hql.Append("INSERT INTO RESULTADO_BACKTESTING (BATE_CD_BACKTESTING, REBA_DT_DATA_HORA, REBA_VL_PRECO, REBA_IN_OPERACAO,");
                hql.Append("REBA_QN_QUANTIDADE, REBA_IN_STOP_GAIN_ATINGIDO, REBA_IN_STOP_LOSS_ATINGIDO, REBA_VL_SALDO_PARCIAL,");
                hql.Append("REBA_QN_CUSTODIA_PARCIAL, REBA_VL_SALDO_TOTAL, REBA_VL_RENTABILIDADE_ACUMULADA, REBA_VL_RENTABILIDADE) VALUES (");
                hql.Append("?IdBackTest, ?DataHora, ?Preco, ?Operacao, ?Quantidade, ?StopGainAtingido, ?StopLossAtingido,");
                hql.Append("?SaldoParcial, ?CustodiaParcial, ?SaldoTotal, ?RentabilidadeAcumulada, ?Rentabilidade)");


                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //adicionando os parametros
                    command.Parameters.AddWithValue("?IdBackTest", operacao.IdBackTest);
                    command.Parameters.AddWithValue("?DataHora", operacao.DataHora);

                    if (Double.IsNaN(operacao.Preco))
                        operacao.Preco = 0;
                    command.Parameters.AddWithValue("?Preco", operacao.Preco);
                    command.Parameters.AddWithValue("?Operacao", operacao.Operacao);

                    if (Double.IsNaN(operacao.Quantidade))
                        operacao.Quantidade = 0;
                    command.Parameters.AddWithValue("?Quantidade", operacao.Quantidade);

                    command.Parameters.AddWithValue("?StopGainAtingido", operacao.StopGainAtingido);
                    command.Parameters.AddWithValue("?StopLossAtingido", operacao.StopLossAtingido);

                    if (Double.IsNaN(operacao.SaldoParcial))
                        operacao.SaldoParcial = 0;
                    command.Parameters.AddWithValue("?SaldoParcial", operacao.SaldoParcial);

                    if (Double.IsNaN(operacao.CustodiaParcial))
                        operacao.CustodiaParcial = 0;
                    command.Parameters.AddWithValue("?CustodiaParcial", operacao.CustodiaParcial);

                    if (Double.IsNaN(operacao.SaldoTotal))
                        operacao.SaldoTotal = 0;
                    command.Parameters.AddWithValue("?SaldoTotal", operacao.SaldoTotal);

                    if (Double.IsNaN(operacao.RentabilidadeAcumulada))
                        operacao.RentabilidadeAcumulada = 0;
                    command.Parameters.AddWithValue("?RentabilidadeAcumulada", operacao.RentabilidadeAcumulada);

                    if (Double.IsNaN(operacao.Rentabilidade))
                        operacao.Rentabilidade = 0;
                    command.Parameters.AddWithValue("?Rentabilidade", operacao.Rentabilidade);

                    //inserindo operacao
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
