using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class TemplateDAO:BaseDAO
    {
        #region Construtor

        public TemplateDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Read

        /// <summary>
        /// Retorna templates por id do cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<TemplateDTO> GetTemplatesByUser(int userId)
        {
            StringBuilder hql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;
            List<TemplateDTO> listaTemplate = new List<TemplateDTO>();

            try
            {
                //Montando a query
                hql.Append(" SELECT * FROM TEMPLATE TE ");
                hql.Append(" LEFT JOIN LAYOUT L ON TE.TEMP_CD_TEMPLATE = L.TEMP_CD_TEMPLATE ");
                hql.Append(" LEFT JOIN INDICADOR I ON L.LAYO_CD_LAYOUT = I.LAYO_CD_LAYOUT ");
                hql.Append(" LEFT JOIN PAINEL P ON L.LAYO_CD_LAYOUT = P.LAYO_CD_LAYOUT ");
                hql.Append(" WHERE TE.USUA_CD_USUARIO = " + userId);

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), readConnection))
                {
                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        return MapeiaTemplates(reader);
                    }
                    else
                        return new List<TemplateDTO>();
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
        /// Metodo que faz o mapeamento da lista de templates
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<TemplateDTO> MapeiaTemplates(MySqlDataReader reader)
        {
            List<TemplateDTO> listaPrincipal = new List<TemplateDTO>();
            Dictionary<int, TemplateDTO> listaTemplates = new Dictionary<int, TemplateDTO>();
            Dictionary<int, LayoutDTO> listalayout = new Dictionary<int, LayoutDTO>();
            Dictionary<int, IndicadorDTO> listaIndicadores = new Dictionary<int, IndicadorDTO>();
            Dictionary<int, PainelDTO> listaPaineis = new Dictionary<int, PainelDTO>();

            try
            {
                #region Gerando os dicionarios

                while (reader.Read())
                {
                    if (!listaTemplates.ContainsKey(reader.GetInt32("TEMP_CD_TEMPLATE")))
                    {
                        TemplateDTO templateDTO = new TemplateDTO();
                        templateDTO.UsuarioId = reader.GetInt32("USUA_CD_USUARIO");
                        templateDTO.Id = reader.GetInt32("TEMP_CD_TEMPLATE");
                        templateDTO.Nome = reader.GetString("TEMP_NM_TEMPLATE");
                        templateDTO.Layout = new LayoutDTO();

                        //adicionando a lista
                        listaTemplates.Add(reader.GetInt32("TEMP_CD_TEMPLATE"), templateDTO);
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("LAYO_CD_LAYOUT")))
                    {
                        if (!listalayout.ContainsKey(reader.GetInt32("LAYO_CD_LAYOUT")))
                        {

                            LayoutDTO layot = new LayoutDTO();
                            layot.Id = reader.GetInt32("LAYO_CD_LAYOUT");
                            layot.CorVolume = reader.GetString("LAYO_NM_COR_VOLUME");
                            layot.CorFundo = reader.GetString("LAYO_NM_COR_FUNDO");
                            layot.CorBordaCandleAlta = reader.GetString("LAYO_NM_COR_BORDA_ALTA");
                            layot.CorBordaCandleBaixa = reader.GetString("LAYO_NM_COR_BORDA_BAIXA");
                            layot.CorCandleAlta = reader.GetString("LAYO_NM_COR_CANDLE_ALTA");
                            layot.CorCandleBaixa = reader.GetString("LAYO_NM_COR_CANDLE_BAIXA");
                            layot.PosicaoEscala = reader.GetInt32("LAYO_IN_POSICAO_ESCALA");
                            layot.TipoEscala = reader.GetInt32("LAYO_IN_TIPO_ESCALA");
                            layot.PrecisaoEscala = reader.GetInt32("LAYO_NR_PRECISAO_ESCALA");
                            layot.GradeHorizontal = reader.GetBoolean("LAYO_IN_GRADE_HORIZONTAL");
                            layot.GradeVertical = reader.GetBoolean("LAYO_IN_GRADE_VERTICAL");
                            layot.PainelInfo = reader.GetBoolean("LAYO_IN_PAINEL");
                            layot.EspacoADireitaDoGrafico = reader.GetDouble("LAYO_VL_ESPACO_DIREITA");
                            layot.EspacoAEsquerdaDoGrafico = reader.GetDouble("LAYO_VL_ESPACO_ESQUERDA");
                            layot.EstiloPreco = reader.GetInt32("LAYO_IN_ESTILO_PRECO");
                            layot.EstiloPrecoParam1 = reader.GetInt32("LAYO_VL_PRECO_PARAM1");
                            layot.EstiloPrecoParam2 = reader.GetInt32("LAYO_VL_PRECO_PARAM2");
                            layot.EstiloBarra = reader.GetInt32("LAYO_IN_ESTILO_BARRA");
                            layot.DarvaBox = reader.GetBoolean("LAYO_IN_DARVA_BOX");
                            layot.TipoVolume = reader.GetString("LAYO_IN_VOLUME");
                            if (!reader.IsDBNull(reader.GetOrdinal("GRAF_CD_GRAFICO")))
                                layot.GraficoId = reader.GetInt32("GRAF_CD_GRAFICO");
                            if (!reader.IsDBNull(reader.GetOrdinal("TEMP_CD_TEMPLATE")))
                                layot.TemplateId = reader.GetInt32("TEMP_CD_TEMPLATE");
                            layot.VolumeStrokeThickness = reader.GetInt32("LAYO_VL_ESPESSURA_VOLUME");
                            layot.CorGrid = reader.GetString("LAYO_NM_COR_GRID");
                            layot.CorEscala = reader.GetString("LAYO_NM_COR_ESCALA");
                            layot.UsarCoresAltaBaixaVolume = reader.GetBoolean("LAYO_IN_USAR_COR_VOLUME");
                            layot.Indicadores = new List<IndicadorDTO>();
                            layot.Objetos = new List<ObjetoEstudoDTO>();

                            //adicionando a lista
                            listalayout.Add(reader.GetInt32("LAYO_CD_LAYOUT"), layot);
                        }
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("INDI_CD_INDICADOR")))
                    {
                        if (!listaIndicadores.ContainsKey(reader.GetInt32("INDI_CD_INDICADOR")))
                        {
                            IndicadorDTO indicador = new IndicadorDTO();
                            indicador.Cor = reader.GetString("INDI_NM_COR");
                            if (!reader.IsDBNull(reader.GetOrdinal("INDI_NM_COR_FILHA1")))
                                indicador.CorFilha1 = reader.GetString("INDI_NM_COR_FILHA1");

                            if (!reader.IsDBNull(reader.GetOrdinal("INDI_NM_COR_FILHA2")))
                                indicador.CorFilha2 = reader.GetString("INDI_NM_COR_FILHA2");

                            indicador.Espessura = reader.GetInt32("INDI_NR_ESPESSURA");

                            if (!reader.IsDBNull(reader.GetOrdinal("INDI_NR_ESPESSURA_FILHA1")))
                                indicador.EspessuraFilha1 = reader.GetInt32("INDI_NR_ESPESSURA_FILHA1");

                            if (!reader.IsDBNull(reader.GetOrdinal("INDI_NR_ESPESSURA_FILHA2")))
                                indicador.EspessuraFilha2 = reader.GetInt32("INDI_NR_ESPESSURA_FILHA2");

                            indicador.Id = reader.GetInt32("INDI_CD_INDICADOR");
                            indicador.IndexPainel = reader.GetInt32("INDI_CD_INDEX_PAINEL");
                            indicador.Parametros = reader.GetString("INDI_NM_PARAMETRO");
                            indicador.TipoIndicador = reader.GetInt32("INDI_IN_TIPO_INDICADOR");
                            indicador.TipoLinha = reader.GetInt32("INDI_IN_TIPO_LINHA");

                            if (!reader.IsDBNull(reader.GetOrdinal("INDI_IN_TIPO_LINHA_FILHA1")))
                                indicador.TipoLinhaFilha1 = reader.GetInt32("INDI_IN_TIPO_LINHA_FILHA1");

                            if (!reader.IsDBNull(reader.GetOrdinal("INDI_IN_TIPO_LINHA_FILHA2")))
                                indicador.TipoLinhaFilha2 = reader.GetInt32("INDI_IN_TIPO_LINHA_FILHA2");

                            indicador.LayoutId = reader.GetInt32("LAYO_CD_LAYOUT");

                            //adicionando a lista
                            listaIndicadores.Add(reader.GetInt32("INDI_CD_INDICADOR"), indicador);
                        }
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("PAIN_CD_PAINEL")))
                    {
                        if (!listaPaineis.ContainsKey(reader.GetInt32("PAIN_CD_PAINEL")))
                        {
                            PainelDTO painel = new PainelDTO();
                            painel.Altura = reader.GetInt32("PAIN_PR_ALTURA");
                            painel.Id = reader.GetInt32("PAIN_CD_PAINEL");
                            painel.LayoutId = reader.GetInt32("LAYO_CD_LAYOUT");
                            painel.TipoPainel = reader.GetString("PAIN_IN_TIPO");

                            //adicionando a lista
                            listaPaineis.Add(reader.GetInt32("PAIN_CD_PAINEL"), painel);
                        }
                    }

                }
                #endregion

                #region Mapeando os objetos
                //percorrendo os layouts
                for (int l = 0; l < listalayout.Count; l++)
                {
                    //percorrendo os indicadores
                    for (int i = 0; i < listaIndicadores.Count; i++)
                    {
                        if (listaIndicadores[listaIndicadores.Keys.ElementAt(i)].LayoutId == listalayout[listalayout.Keys.ElementAt(l)].Id)
                        {
                            if (listalayout[listalayout.Keys.ElementAt(l)].Indicadores == null)
                                listalayout[listalayout.Keys.ElementAt(l)].Indicadores = new List<IndicadorDTO>();
                            listalayout[listalayout.Keys.ElementAt(l)].Indicadores.Add(listaIndicadores[listaIndicadores.Keys.ElementAt(i)]);
                        }
                    }
                }
                for (int l = 0; l < listalayout.Count; l++)
                {
                    //percorrendo os indicadores
                    for (int i = 0; i < listaPaineis.Count; i++)
                    {
                        if (listaPaineis[listaPaineis.Keys.ElementAt(i)].LayoutId == listalayout[listalayout.Keys.ElementAt(l)].Id)
                        {
                            if (listalayout[listalayout.Keys.ElementAt(l)].Paineis == null)
                                listalayout[listalayout.Keys.ElementAt(l)].Paineis = new List<PainelDTO>();
                            listalayout[listalayout.Keys.ElementAt(l)].Paineis.Add(listaPaineis[listaPaineis.Keys.ElementAt(i)]);
                        }
                    }
                }
                for (int t = 0; t < listaTemplates.Count; t++)
                {
                    TemplateDTO templateDTO = new TemplateDTO();
                    templateDTO = listaTemplates[listaTemplates.Keys.ElementAt(t)];
                    templateDTO.Layout = new LayoutDTO();

                    for (int l = 0; l < listalayout.Count; l++)
                    {
                        if (listalayout[listalayout.Keys.ElementAt(l)].TemplateId == templateDTO.Id)
                        {
                            templateDTO.Layout = listalayout[listalayout.Keys.ElementAt(l)];
                        }
                    }


                    listaPrincipal.Add(templateDTO);
                }
                #endregion

                return listaPrincipal;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        #endregion

        #region Write

        /// <summary>
        /// Insert new user
        /// </summary>
        /// <param name="cotacao"></param>
        public TemplateDTO InsertTemplate(TemplateDTO template)
        {
            StringBuilder sql = new StringBuilder();

            try
            {


                sql.Append("INSERT INTO TEMPLATE ( TEMP_NM_TEMPLATE, USUA_CD_USUARIO ) VALUES ");
                sql.Append("( ?nome, ?userId )");


                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(sql.ToString(), writeConnection))
                {
                    //adicionando parametros
                    command.Parameters.AddWithValue("?nome", template.Nome);
                    command.Parameters.AddWithValue("?userId", template.UsuarioId);

                    command.ExecuteNonQuery();
                    template.Id = Convert.ToInt32(command.LastInsertedId);
                    return template;
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Insert new user
        /// </summary>
        /// <param name="cotacao"></param>
        public void DeleteTemplate(TemplateDTO template)
        {
            StringBuilder sql = new StringBuilder();

            try
            {


                sql.Append("DELETE FROM TEMPLATE WHERE TEMP_CD_TEMPLATE = ?template ");
                


                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(sql.ToString(), writeConnection))
                {
                    //adicionando parametros
                    command.Parameters.AddWithValue("?template", template.Id);
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
