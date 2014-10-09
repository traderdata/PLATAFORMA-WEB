using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class ScannerDAO:BaseDAO
    {
        #region Construtor

        public ScannerDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Metodos Read

        /* Método: GetScannerById
         * Date: 16/10/2009
         * Description: Retorna um scanner completo, com suas condicoes e valores dos parametros preenchidos pelo usuário
         * cliente
         */
        public ScannerDTO GetScannerById(int scannerId)
        {
            //Declarando variáveis auxiliares
            StringBuilder sSql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Resgatando os valores das condicoes
                sSql = new StringBuilder();
                sSql.Append(" SELECT * ");
                sSql.Append(" FROM SCANNER_CONDICAO_VALOR s, CONDICAO c, SCANNER sc, CONDICAO_PARCELA cp ");
                sSql.Append(" where s.cond_cd_condicao = c.cond_cd_condicao ");
                sSql.Append(" and s.scan_cd_scanner = sc.scan_cd_scanner ");
                sSql.Append(" and c.cond_cd_condicao = cp.cond_cd_condicao ");
                sSql.Append(" and cp.copa_cd_condicao_parcela = s.copa_cd_condicao_parcela ");
                sSql.Append(" and s.scan_cd_scanner = " + scannerId);
                sSql.Append(" order by c.cond_cd_condicao, cp.copa_cd_condicao_parcela");

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(sSql.ToString(), readConnection))
                {
                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        return MapeiaScanners(reader)[0];
                    }
                    else
                    {
                        return null;
                    }
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
        /// Metodo que retorna todos os scanners
        /// </summary>
        /// <returns></returns>
        private List<ScannerDTO> MapeiaScanners(MySqlDataReader reader)
        {
            StringBuilder hql = new StringBuilder();
            Dictionary<int, ScannerDTO> listaScanner = new Dictionary<int, ScannerDTO>();
            Dictionary<int, CondicaoDTO> listaCondicao = new Dictionary<int, CondicaoDTO>();
            Dictionary<int, CondicaoParcelaDTO> listaParcelas = new Dictionary<int, CondicaoParcelaDTO>();
            Dictionary<int, ScannerCondicaoValorDTO> listaValores = new Dictionary<int, ScannerCondicaoValorDTO>();
            List<ScannerDTO> listaScannerAux = new List<ScannerDTO>();

            try
            {
                if (reader.HasRows)
                {
                    #region Coloca valores em listas
                    while (reader.Read())
                    {
                        if (!listaScanner.ContainsKey(reader.GetInt32("SCAN_CD_SCANNER")))
                        {
                            ScannerDTO scannerDTO = new ScannerDTO();
                            scannerDTO.Formula = reader.GetString("SCAN_TX_FORMULA");
                            scannerDTO.Id = reader.GetInt32("SCAN_CD_SCANNER");
                            scannerDTO.Nome = reader.GetString("SCAN_NM_SCANNER");
                            scannerDTO.Descricao = reader.GetString("SCAN_TX_SCANNER");
                            scannerDTO.Periodicidade = reader.GetInt32("SCAN_NR_PERIODICIDADE");
                            scannerDTO.ListaAtivos = reader.GetString("SCAN_NM_ATIVOS");
                            scannerDTO.PublicarFacebook = reader.GetBoolean("SCAN_IN_FACEBOOK");
                            scannerDTO.ListaCondicoes = new List<CondicaoDTO>();
                            listaScanner.Add(scannerDTO.Id, scannerDTO);
                        }
                        if (!listaValores.ContainsKey(reader.GetInt32("SCCV_CD_CONDICAO_VALOR")))
                        {
                            ScannerCondicaoValorDTO valorDTO = new ScannerCondicaoValorDTO();
                            valorDTO.Id = reader.GetInt32("SCCV_CD_CONDICAO_VALOR");
                            valorDTO.CondicaoId = reader.GetInt32("COND_CD_CONDICAO");
                            valorDTO.ParcelaId = reader.GetInt32("COPA_CD_CONDICAO_PARCELA");
                            valorDTO.ScannerId = reader.GetInt32("SCAN_CD_SCANNER");

                            if (!reader.IsDBNull(reader.GetOrdinal("SCCV_VL_DOUBLE")))
                                valorDTO.ValorDouble = reader.GetDouble("SCCV_VL_DOUBLE");

                            if (!reader.IsDBNull(reader.GetOrdinal("SCCV_VL_INTEIRO")))
                                valorDTO.ValorInteiro = reader.GetInt32("SCCV_VL_INTEIRO");

                            if (!reader.IsDBNull(reader.GetOrdinal("SCCV_NM_STRING")))
                                valorDTO.ValorString = reader.GetString("SCCV_NM_STRING");
                            listaValores.Add(valorDTO.Id, valorDTO);
                        }
                        if (!listaCondicao.ContainsKey(reader.GetInt32("COND_CD_CONDICAO")))
                        {
                            CondicaoDTO condicaoDTO = new CondicaoDTO();
                            condicaoDTO.Comando = reader.GetString("COND_NM_COMANDO");
                            condicaoDTO.Id = reader.GetInt32("COND_CD_CONDICAO");
                            condicaoDTO.Nome = reader.GetString("COND_NM_COMANDO");
                            condicaoDTO.ListaParcelas = new List<CondicaoParcelaDTO>();
                            listaCondicao.Add(condicaoDTO.Id, condicaoDTO);
                        }
                        if (!listaParcelas.ContainsKey(reader.GetInt32("COPA_CD_CONDICAO_PARCELA")))
                        {
                            CondicaoParcelaDTO parcelaDTO = new CondicaoParcelaDTO();
                            parcelaDTO.Id = reader.GetInt32("COPA_CD_CONDICAO_PARCELA");
                            parcelaDTO.CondicaoId = reader.GetInt32("COND_CD_CONDICAO");
                            parcelaDTO.Nome = reader.GetString("COPA_NM_PARCELA");
                            parcelaDTO.TipoApresentacao = reader.GetString("COPA_IN_TIPO_APRESENTACAO");
                            parcelaDTO.TipoFisico = reader.GetString("COPA_IN_TIPO_FISICO");
                            listaParcelas.Add(parcelaDTO.Id, parcelaDTO);
                        }
                    }
                    #endregion

                    #region Criando a lista de scanners

                    foreach (ScannerDTO scannerDTO in listaScanner.Values)
                    {
                        foreach (ScannerCondicaoValorDTO scannerCondicaoValor in listaValores.Values)
                        {
                            if (scannerCondicaoValor.ScannerId == scannerDTO.Id)
                            {
                                bool condicaoJaInserida = false;
                                foreach (CondicaoDTO obj in scannerDTO.ListaCondicoes)
                                {
                                    if (obj.Id == scannerCondicaoValor.CondicaoId)
                                        condicaoJaInserida = true;
                                }
                                if (!condicaoJaInserida)
                                {
                                    CondicaoDTO condicaoDTO = new CondicaoDTO();
                                    condicaoDTO.Id = scannerCondicaoValor.CondicaoId;
                                    condicaoDTO.ListaParcelas = new List<CondicaoParcelaDTO>();

                                    foreach (CondicaoDTO condicaoAux in listaCondicao.Values)
                                    {
                                        if (condicaoAux.Id == condicaoDTO.Id)
                                        {
                                            condicaoDTO.Nome = condicaoAux.Nome;
                                            condicaoDTO.Comando = condicaoAux.Comando;
                                        }
                                    }

                                    scannerDTO.ListaCondicoes.Add(condicaoDTO);
                                }
                            }
                        }

                        listaScannerAux.Add(scannerDTO);
                    }

                    //tenho todos os scanners preenchidos com as suas condições
                    foreach (ScannerDTO scannerDTO in listaScanner.Values)
                    {
                        foreach (CondicaoDTO condicaoDTO in scannerDTO.ListaCondicoes)
                        {
                            foreach (CondicaoParcelaDTO parcelaDTO in listaParcelas.Values)
                            {
                                if (parcelaDTO.CondicaoId == condicaoDTO.Id)
                                    condicaoDTO.ListaParcelas.Add(parcelaDTO);
                            }
                        }

                    }

                    //agora tenho as listas condicoes preenchidas
                    foreach (ScannerDTO scannerDTO in listaScanner.Values)
                    {
                        foreach (CondicaoDTO condicaoDTO in scannerDTO.ListaCondicoes)
                        {
                            foreach (CondicaoParcelaDTO parcelaDTO in condicaoDTO.ListaParcelas)
                            {
                                foreach (ScannerCondicaoValorDTO scannerCondicaoValor in listaValores.Values)
                                {
                                    if ((scannerCondicaoValor.CondicaoId == condicaoDTO.Id)
                                        && (scannerCondicaoValor.ParcelaId == parcelaDTO.Id)
                                        && (scannerCondicaoValor.ScannerId == scannerDTO.Id))
                                    {
                                        parcelaDTO.ValorDouble = scannerCondicaoValor.ValorDouble;
                                        parcelaDTO.ValorInteiro = scannerCondicaoValor.ValorInteiro;
                                        parcelaDTO.ValorString = scannerCondicaoValor.ValorString;
                                    }
                                }
                            }
                        }
                    }

                    return listaScannerAux;
                    #endregion


                }
                else
                    return new List<ScannerDTO>();


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

        /* Método: GetScanners
         * Date: 16/10/2009
         * Description: Retorna uma lista com todos os meus scanners validos e todos os scanners publicos do macro
         * cliente
         */
        public List<ScannerDTO> GetScanners(int userId)
        {
            StringBuilder sSql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Resgatando os valores das condicoes
                sSql = new StringBuilder();
                sSql.Append(" SELECT * ");
                sSql.Append(" FROM SCANNER_CONDICAO_VALOR s, CONDICAO c, SCANNER sc, CONDICAO_PARCELA cp ");
                sSql.Append(" where s.cond_cd_condicao = c.cond_cd_condicao ");
                sSql.Append(" and s.scan_cd_scanner = sc.scan_cd_scanner ");
                sSql.Append(" and c.cond_cd_condicao = cp.cond_cd_condicao ");
                sSql.Append(" and cp.copa_cd_condicao_parcela = s.copa_cd_condicao_parcela ");
                sSql.Append(" and sc.usua_cd_usuario = ?usuario");
                //sSql.Append(" order by c.cond_cd_condicao, cp.copa_cd_condicao_parcela");

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(sSql.ToString(), readConnection))
                {
                    command.Parameters.AddWithValue("?usuario", userId);

                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        return MapeiaScanners(reader);
                    }
                    else
                        return new List<ScannerDTO>();
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
        /// Metodo que insere uma nova condição
        /// </summary>
        /// <param name="scannerCondicaoValor"></param>
        public void InserirScanner(ScannerDTO scannerDTO)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //Salvando os graficos
                hql = new StringBuilder();
                hql.Append(" INSERT INTO SCANNER (USUA_CD_USUARIO, SCAN_NM_SCANNER, SCAN_TX_FORMULA, SCAN_NR_PERIODICIDADE,");
                hql.Append(" SCAN_NM_ATIVOS, SCAN_IN_FACEBOOK, SCAN_IN_EMAIL, SCAN_TX_SCANNER) VALUES ");
                hql.Append(" (?usuario, ?nome, ?formula, ?periodicidade, ?ativos, ?facebook, ?email, ?descricao) ");

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //colocando os parametros
                    command.Parameters.AddWithValue("?usuario", scannerDTO.User.Id);
                    command.Parameters.AddWithValue("?nome", scannerDTO.Nome);
                    command.Parameters.AddWithValue("?formula", scannerDTO.Formula);
                    command.Parameters.AddWithValue("?periodicidade", scannerDTO.Periodicidade);
                    command.Parameters.AddWithValue("?ativos", scannerDTO.ListaAtivos);
                    command.Parameters.AddWithValue("?facebook", scannerDTO.PublicarFacebook);
                    command.Parameters.AddWithValue("?email", scannerDTO.EnviarEmail);
                    command.Parameters.AddWithValue("?descricao", scannerDTO.Descricao);
                    
                    //executando                    
                    command.ExecuteNonQuery();
                    scannerDTO.Id = Convert.ToInt32(command.LastInsertedId);
                }


            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que exclui scaner pelo ID
        /// </summary>
        /// <param name="scannerId"></param>
        public void ExcluirScanner(ScannerDTO scannerDTO)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //Salvando os graficos
                hql = new StringBuilder();
                hql.Append(" DELETE FROM SCANNER WHERE ");
                hql.Append(" SCAN_CD_SCANNER = " + scannerDTO.Id);

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

        #endregion
    }
}
