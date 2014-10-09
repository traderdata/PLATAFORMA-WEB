using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class IndicadorDAO : BaseDAO
    {
        #region Construtor

        public IndicadorDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Metodos Read

        /// <summary>
        /// Metodo que retorna a lista de indicadores de um grafico
        /// </summary>
        /// <param name="graficoId"></param>
        /// <returns></returns>
        public List<IndicadorDTO> RetornaIndicadorPorGraficoId(int graficoId)
        {
            StringBuilder hql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Montando a query
                hql.Append(" SELECT * FROM INDICADOR I");
                hql.Append(" WHERE I.GRAF_CD_GRAFICO = " + graficoId);

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), readConnection))
                {
                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<IndicadorDTO> listaAux = new List<IndicadorDTO>();
                        while (reader.Read())
                        {
                            IndicadorDTO indicador = new IndicadorDTO();
                            indicador.Cor = reader.GetString("INDI_NM_COR");
                            indicador.CorFilha1 = reader.GetString("INDI_NM_COR_FILHA1");
                            indicador.CorFilha2 = reader.GetString("INDI_NM_COR_FILHA2");
                            indicador.Espessura = reader.GetInt32("INDI_NR_ESPESSURA");
                            indicador.EspessuraFilha1 = reader.GetInt32("INDI_NR_ESPESSURA_FILHA1");
                            indicador.EspessuraFilha2 = reader.GetInt32("INDI_NR_ESPESSURA_FILHA2");
                            indicador.Id = reader.GetInt32("INDI_CD_INDICADOR");
                            indicador.IndexPainel = reader.GetInt32("INDI_CD_INDEX_PAINEL");
                            indicador.Parametros = reader.GetString("INDI_NM_PARAMETRO");
                            indicador.TipoIndicador = reader.GetInt32("INDI_IN_TIPO_INDICADOR");
                            indicador.TipoLinha = reader.GetInt32("INDI_IN_TIPO_LINHA");
                            indicador.TipoLinhaFilha1 = reader.GetInt32("INDI_IN_TIPO_LINHA_FILHA1");
                            indicador.TipoLinhaFilha2 = reader.GetInt32("INDI_IN_TIPO_LINHA_FILHA2");

                            //adicionando a lista
                            listaAux.Add(indicador);
                        }

                        return listaAux;
                    }
                    else
                        return new List<IndicadorDTO>();
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
        /// Metodo que retorna a lista de indicadores de um TEMPLATE
        /// </summary>
        /// <param name="graficoId"></param>
        /// <returns></returns>
        public List<IndicadorDTO> RetornaIndicadorPorTemplateId(int templateId)
        {
            StringBuilder hql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Montando a query
                hql.Append(" SELECT * FROM INDICADOR I");
                hql.Append(" WHERE I.TEMP_CD_TEMPLATE = " + templateId);

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), readConnection))
                {
                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<IndicadorDTO> listaAux = new List<IndicadorDTO>();
                        while (reader.Read())
                        {
                            IndicadorDTO indicador = new IndicadorDTO();
                            indicador.Cor = reader.GetString("INDI_NM_COR");
                            indicador.CorFilha1 = reader.GetString("INDI_NM_COR_FILHA1");
                            indicador.CorFilha2 = reader.GetString("INDI_NM_COR_FILHA2");
                            indicador.Espessura = reader.GetInt32("INDI_NR_ESPESSURA");
                            indicador.EspessuraFilha1 = reader.GetInt32("INDI_NR_ESPESSURA_FILHA1");
                            indicador.EspessuraFilha2 = reader.GetInt32("INDI_NR_ESPESSURA_FILHA2");
                            indicador.Id = reader.GetInt32("INDI_CD_INDICADOR");
                            indicador.IndexPainel = reader.GetInt32("INDI_CD_INDEX_PAINEL");
                            indicador.Parametros = reader.GetString("INDI_NM_PARAMETRO");
                            indicador.TipoIndicador = reader.GetInt32("INDI_IN_TIPO_INDICADOR");
                            indicador.TipoLinha = reader.GetInt32("INDI_IN_TIPO_LINHA");
                            indicador.TipoLinhaFilha1 = reader.GetInt32("INDI_IN_TIPO_LINHA_FILHA1");
                            indicador.TipoLinhaFilha2 = reader.GetInt32("INDI_IN_TIPO_LINHA_FILHA2");
                            
                            //adicionando a lista
                            listaAux.Add(indicador);
                        }

                        return listaAux;
                    }
                    else
                        return new List<IndicadorDTO>();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        
        #endregion

        #region Metodos Write

        /// <summary>
        /// Metodo que exclui indicador de acordo com o id do template
        /// </summary>
        /// <param name="id"></param>
        public void ExcluiIndicadorDeAcordoComLayoutId(int layoutId)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                hql = new StringBuilder();
                hql.Append(" DELETE FROM ");
                hql.Append(" INDICADOR ");
                hql.Append(" WHERE LAYO_CD_LAYOUT = " + layoutId );

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
        /// Metodo que exclui indicador de acordo com o id
        /// </summary>
        /// <param name="id"></param>
        public void ExcluiIndicadorPorId(int id)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                hql = new StringBuilder();
                hql.Append(" DELETE FROM ");
                hql.Append(" INDICADOR ");
                hql.Append(" WHERE INDI_CD_INDICADOR = " + id);

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
        /// Metodo que salva o indicador
        /// </summary>
        /// <param name="indicadorObj"></param>
        public void SalvarIndicador(IndicadorDTO indicadorObj)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //Salvando os graficos
                hql = new StringBuilder();
                hql.Append(" INSERT INTO INDICADOR (INDI_NM_COR, INDI_NM_COR_FILHA1, INDI_NM_COR_FILHA2, INDI_IN_TIPO_LINHA,");
                hql.Append(" INDI_IN_TIPO_LINHA_FILHA1, INDI_IN_TIPO_LINHA_FILHA2, INDI_NR_ESPESSURA, INDI_NR_ESPESSURA_FILHA1, ");
                hql.Append(" INDI_NR_ESPESSURA_FILHA2, INDI_NM_PARAMETRO, INDI_IN_TIPO_INDICADOR, INDI_CD_INDEX_PAINEL, ");
                hql.Append(" LAYO_CD_LAYOUT) VALUES (");
                hql.Append(" ?cor, ?corFilha1, ?corFilha2, ?tipoLinha, ?tipoLinhaFilha1, ");
                hql.Append(" ?tipoLinhaFilha2, ?espessura, ?espessuraFilha1, ?espessuraFilha2, ");
                hql.Append(" ?parametro, ?tipo, ?indexPainel, ?layout)");

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //Setando os parametros
                    command.Parameters.AddWithValue("?cor", indicadorObj.Cor);
                    command.Parameters.AddWithValue("?corFilha1",indicadorObj.CorFilha1);
                    command.Parameters.AddWithValue("?corFilha2",indicadorObj.CorFilha2);

                    command.Parameters.AddWithValue("?tipoLinha", indicadorObj.TipoLinha);
                    command.Parameters.AddWithValue("?tipoLinhaFilha1", indicadorObj.TipoLinhaFilha1);
                    command.Parameters.AddWithValue("?tipoLinhaFilha2", indicadorObj.TipoLinhaFilha2);

                    command.Parameters.AddWithValue("?espessura", indicadorObj.Espessura);
                    command.Parameters.AddWithValue("?espessuraFilha1", indicadorObj.EspessuraFilha1);
                    command.Parameters.AddWithValue("?espessuraFilha2", indicadorObj.EspessuraFilha2);

                    command.Parameters.AddWithValue("?parametro", indicadorObj.Parametros);
                    command.Parameters.AddWithValue("?tipo", indicadorObj.TipoIndicador);
                    command.Parameters.AddWithValue("?indexPainel", indicadorObj.IndexPainel);

                    //command.Parameters.AddWithValue("?valorAltura", indicadorObj.AlturaPainel);
                    //command.Parameters.AddWithValue("?statusPainel", indicadorObj.StatusPainel);
                    //command.Parameters.AddWithValue("?painelIndicador", indicadorObj.PainelIndicador);

                    //command.Parameters.AddWithValue("?painelPreco", indicadorObj.PainelPreco);
                    //command.Parameters.AddWithValue("?painelVolume", indicadorObj.PainelVolume);
                    //command.Parameters.AddWithValue("?painelAbaixoPreco", indicadorObj.PainelAbaixoPreco);

                    if (indicadorObj.LayoutId.HasValue)
                        command.Parameters.AddWithValue("?layout", indicadorObj.LayoutId.Value);                        
                    else
                        command.Parameters.AddWithValue("?layout", null);
                        

                    //executando
                    command.ExecuteNonQuery();
                    indicadorObj.Id = Convert.ToInt32(command.LastInsertedId);
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
