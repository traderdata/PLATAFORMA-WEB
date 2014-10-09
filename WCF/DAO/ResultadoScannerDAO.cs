using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class ResultadoScannerDAO:BaseDAO
    {
        #region Construtor

        public ResultadoScannerDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Metodos Read

        /* Método:GetResultadoScanner
         * Descrição: Retorna a lista de ativos que foi disparada como resultado do scanner acionado
         * Data: 01/09/2008
         */
        public List<ResultadoScannerDTO> GetResultadoScanner(int codigoScanner)
        {
            StringBuilder sql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Montando a query
                sql.Append(" SELECT * FROM RESULTADO_SCANNER WHERE ");
                sql.Append(" SCAN_CD_SCANNER = ?scanner ");

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(sql.ToString(), readConnection))
                {
                    //setando os parametros
                    command.Parameters.AddWithValue("?scanner", codigoScanner);

                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<ResultadoScannerDTO> listaAux = new List<ResultadoScannerDTO>();

                        while (reader.Read())
                        {
                            ResultadoScannerDTO resultadoScanner = new ResultadoScannerDTO();

                            resultadoScanner.Abertura = reader.GetDouble("COTA_VL_ABERTURA");
                            resultadoScanner.Ativo = reader.GetString("RESC_NM_ATIVO");
                            resultadoScanner.ScannerId = reader.GetInt32("SCAN_CD_SCANNER");
                            resultadoScanner.Data = reader.GetDateTime("COTA_DT_PREGAO");
                            resultadoScanner.Fechamento = reader.GetDouble("COTA_VL_FECHAMENTO");
                            resultadoScanner.Id = reader.GetInt32("RESC_CD_RESULTADO_SCANNER");
                            resultadoScanner.Maximo = reader.GetDouble("COTA_VL_MAXIMO");
                            resultadoScanner.Minimo = reader.GetDouble("COTA_VL_MINIMO");
                            resultadoScanner.Variacao = reader.GetDouble("COTA_VL_VARIACAO");
                            resultadoScanner.Volume = reader.GetDouble("COTA_VL_VOLUME");



                            listaAux.Add(resultadoScanner);
                        }

                        return listaAux;
                    }
                    else
                        return new List<ResultadoScannerDTO>();
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
        /// Metodo que faz a exclusao dos resultados de determinado scanner
        /// </summary>
        /// <param name="scannerId"></param>
        public void DeleteResultadoScannerByScannerId(int scannerId)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //Salvando os graficos
                hql = new StringBuilder();
                hql.Append(" DELETE FROM RESULTADO_SCANNER WHERE ");
                hql.Append(" SCAN_CD_SCANNER = " + scannerId);

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //executando                    
                    command.ExecuteNonQuery();
                }


            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que salva
        /// </summary>
        /// <param name="lista"></param>
        public void SalvarListaResultadoScannerDiario(ResultadoScannerDTO resultado)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //Montando a query
                hql.Append(" INSERT INTO RESULTADO_SCANNER (SCAN_CD_SCANNER, RESC_NM_ATIVO, COTA_DT_PREGAO, COTA_VL_VARIACAO, ");
                hql.Append(" COTA_VL_FECHAMENTO, COTA_VL_ABERTURA, COTA_VL_MAXIMO, COTA_VL_MINIMO, COTA_VL_VOLUME) VALUES ");
                hql.Append(" (?scanner, ?ativo, ?data, ?variacao, ?fechamento, ?abertura, ?maximo, ?minimo, ?volume)");


                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), writeConnection))
                {
                    //setando os parametros
                    command.Parameters.AddWithValue("?scanner", resultado.ScannerId);
                    command.Parameters.AddWithValue("?ativo", resultado.Ativo);
                    command.Parameters.AddWithValue("?data", resultado.Data);
                    command.Parameters.AddWithValue("?variacao", resultado.Variacao);
                    command.Parameters.AddWithValue("?fechamento", resultado.Fechamento);
                    command.Parameters.AddWithValue("?abertura", resultado.Abertura);
                    command.Parameters.AddWithValue("?maximo", resultado.Maximo);
                    command.Parameters.AddWithValue("?minimo", resultado.Minimo);
                    command.Parameters.AddWithValue("?volume", resultado.Volume);

                    //executando a query
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
