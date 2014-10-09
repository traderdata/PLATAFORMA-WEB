using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class ScannerCondicaoValorDAO:BaseDAO
    {
        
        #region Construtor

        public ScannerCondicaoValorDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Metodos Read

        #endregion

        #region Metodos Write

        /// <summary>
        /// Metodo que exclui as condições de determinado scaner
        /// </summary>
        /// <param name="scannerId"></param>
        public void DeleteCondicaoValorByScannerId(int scannerId)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //Salvando os graficos
                hql = new StringBuilder();
                hql.Append(" DELETE FROM SCANNER_CONDICAO_VALOR WHERE ");
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
        /// Metodo que insere uma nova condição
        /// </summary>
        /// <param name="scannerCondicaoValor"></param>
        public void InsereCondicaoValor(ScannerCondicaoValorDTO scannerCondicaoValor)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //Salvando os graficos
                hql = new StringBuilder();
                hql.Append(" INSERT INTO SCANNER_CONDICAO_VALOR (COND_CD_CONDICAO, SCAN_CD_SCANNER, COPA_CD_CONDICAO_PARCELA, SCCV_VL_INTEIRO, SCCV_VL_DOUBLE, SCCV_NM_STRING) VALUES ");
                hql.Append(" (?condicao, ?scanner, ?parcela, ?inteiro, ?double, ?string) ");

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //colocando os parametros
                    command.Parameters.AddWithValue("?condicao", scannerCondicaoValor.CondicaoId);
                    command.Parameters.AddWithValue("?scanner", scannerCondicaoValor.ScannerId);
                    command.Parameters.AddWithValue("?parcela", scannerCondicaoValor.ParcelaId);
                    command.Parameters.AddWithValue("?inteiro", scannerCondicaoValor.ValorInteiro);
                    command.Parameters.AddWithValue("?double", scannerCondicaoValor.ValorDouble);
                    command.Parameters.AddWithValue("?string", scannerCondicaoValor.ValorString);

                    //executando                    
                    command.ExecuteNonQuery();
                    scannerCondicaoValor.Id = Convert.ToInt32(command.LastInsertedId);
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
