using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class WorkspaceDAO:BaseDAO
    {
        #region Construtor

        public WorkspaceDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)        
        { }

        #endregion

        #region Metodos Read

        /// <summary>
        /// Metodo que retorna o workspace 0 de um cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<WorkspaceDTO> GetWorkspacesByUser(UsuarioDTO User)
        {
            StringBuilder hql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Montando a query
                hql.Append(" SELECT * FROM WORKSPACE W");
                hql.Append(" LEFT JOIN GRAFICO GR ON W.WORK_CD_WORKSPACE = GR.WORK_CD_WORKSPACE");
                hql.Append(" LEFT JOIN LAYOUT L ON GR.GRAF_CD_GRAFICO = L.GRAF_CD_GRAFICO");
                hql.Append(" LEFT JOIN INDICADOR I ON L.LAYO_CD_LAYOUT = I.LAYO_CD_LAYOUT");
                hql.Append(" LEFT JOIN OBJETO_ESTUDO O ON L.LAYO_CD_LAYOUT = O.LAYO_CD_LAYOUT");
                hql.Append(" LEFT JOIN PAINEL P ON L.LAYO_CD_LAYOUT = P.LAYO_CD_LAYOUT");
                hql.Append(" WHERE W.USUA_CD_USUARIO = " + User.Id);

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), readConnection))
                {                    
                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        return MapeiaWorkspace(reader);
                    }
                    else
                    {
                        reader.Close();
                        return this.GetWorkspaceDefaultPorDistribuidor(User.DistribuidorId);
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                if (!reader.IsClosed)
                    reader.Close();
            }

        }

        /// <summary>
        /// Metodo que retorna o workspace 0 de um cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<WorkspaceDTO> GetWorkspacesByUserId(int userId)
        {
            StringBuilder hql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Montando a query
                hql.Append(" SELECT * FROM WORKSPACE W");
                hql.Append(" LEFT JOIN GRAFICO GR ON W.WORK_CD_WORKSPACE = GR.WORK_CD_WORKSPACE");
                hql.Append(" LEFT JOIN LAYOUT L ON GR.GRAF_CD_GRAFICO = L.GRAF_CD_GRAFICO");
                hql.Append(" LEFT JOIN INDICADOR I ON L.LAYO_CD_LAYOUT = I.LAYO_CD_LAYOUT");
                hql.Append(" LEFT JOIN OBJETO_ESTUDO O ON L.LAYO_CD_LAYOUT = O.LAYO_CD_LAYOUT");
                hql.Append(" LEFT JOIN PAINEL P ON L.LAYO_CD_LAYOUT = P.LAYO_CD_LAYOUT");
                hql.Append(" WHERE W.USUA_CD_USUARIO = " + userId);

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), readConnection))
                {
                    command.Parameters.AddWithValue("?userId", userId);

                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        return MapeiaWorkspace(reader);
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
        /// Metodo que retorna o workspace 0 de um cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<WorkspaceDTO> GetWorkspaceDefaultPorDistribuidor(int distribuidorId)
        {
            StringBuilder hql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Montando a query
                //Montando a query
                hql.Append(" SELECT * FROM WORKSPACE W");
                hql.Append(" LEFT JOIN GRAFICO GR ON W.WORK_CD_WORKSPACE = GR.WORK_CD_WORKSPACE");
                hql.Append(" LEFT JOIN LAYOUT L ON GR.GRAF_CD_GRAFICO = L.GRAF_CD_GRAFICO");
                hql.Append(" LEFT JOIN INDICADOR I ON L.LAYO_CD_LAYOUT = I.LAYO_CD_LAYOUT");
                hql.Append(" LEFT JOIN OBJETO_ESTUDO O ON L.LAYO_CD_LAYOUT = O.LAYO_CD_LAYOUT");
                hql.Append(" LEFT JOIN PAINEL P ON L.LAYO_CD_LAYOUT = P.LAYO_CD_LAYOUT");
                hql.Append(" WHERE W.DIST_CD_DISTRIBUIDOR = " + distribuidorId);
                hql.Append(" ORDER BY W.WORK_CD_WORKSPACE, GR.GRAF_CD_GRAFICO, L.LAYO_NR_INDEX ");

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), readConnection))
                {
                    
                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        return MapeiaWorkspace(reader);
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
        /// Metodo quye faz a conversao de array de objetos para 
        /// </summary>
        /// <param name="listaAux"></param>
        /// <returns></returns>
        private List<WorkspaceDTO> MapeiaWorkspace(MySql.Data.MySqlClient.MySqlDataReader reader)
        {
            try
            {
                List<WorkspaceDTO> listaRetorno = new List<WorkspaceDTO>();
                Dictionary<int, GraficoDTO> listaPrincipalGraficos = new Dictionary<int, GraficoDTO>();
                Dictionary<int, WorkspaceDTO> listaWorkspaces = new Dictionary<int, WorkspaceDTO>();
                Dictionary<int, GraficoDTO> listaGrafico = new Dictionary<int, GraficoDTO>();
                Dictionary<int, IndicadorDTO> listaIndicadores = new Dictionary<int, IndicadorDTO>();
                Dictionary<int, LayoutDTO> listalayout = new Dictionary<int, LayoutDTO>();
                Dictionary<int, PainelDTO> listaPaineis = new Dictionary<int, PainelDTO>();
                Dictionary<int, ObjetoEstudoDTO> listaObjeto = new Dictionary<int, ObjetoEstudoDTO>();

                #region Convertendo em dicionarios

                while (reader.Read())
                {
                    if (!listaWorkspaces.ContainsKey(reader.GetInt32("WORK_CD_WORKSPACE")))
                    {
                        //setando as informações de areadetrabalho
                        WorkspaceDTO workspace = new WorkspaceDTO();
                        if (!reader.IsDBNull(reader.GetOrdinal("USUA_CD_USUARIO")))
                            workspace.UsuarioId = reader.GetInt32("USUA_CD_USUARIO");
                        workspace.Id = reader.GetInt32("WORK_CD_WORKSPACE");
                        workspace.Graficos = new List<GraficoDTO>();

                        listaWorkspaces.Add(workspace.Id, workspace);
                    }

                    //colocando os graficos em uma listagem unica
                    if (!reader.IsDBNull(reader.GetOrdinal("GRAF_CD_GRAFICO")))
                    {
                        if (!listaGrafico.ContainsKey(reader.GetInt32("GRAF_CD_GRAFICO")))
                        {
                            //Criando o gráfico
                            GraficoDTO graficoDTO = new GraficoDTO();
                            WorkspaceDTO workspaceAux = new WorkspaceDTO();
                            workspaceAux.Id = reader.GetInt32("WORK_CD_WORKSPACE");
                            graficoDTO.WorkspaceId = workspaceAux.Id;
                            graficoDTO.Ativo = reader.GetString("GRAF_NM_ATIVO");
                            graficoDTO.Id = reader.GetInt32("GRAF_CD_GRAFICO");
                            graficoDTO.Periodicidade = reader.GetInt32("GRAF_VL_PERIODICIDADE");
                            graficoDTO.Left = reader.GetInt32("GRAF_VL_LEFT");
                            graficoDTO.Top = reader.GetInt32("GRAF_VL_TOP");
                            graficoDTO.Width = reader.GetInt32("GRAF_VL_WIDTH");
                            graficoDTO.Height = reader.GetInt32("GRAF_VL_HEIGHT");
                            listaGrafico.Add(graficoDTO.Id, graficoDTO);
                        }
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
                            layot.Index = reader.GetInt32("LAYO_NR_INDEX");

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
                            painel.Status = reader.GetString("PAIN_IN_STATUS");
                            painel.Index = reader.GetInt32("PAIN_CD_INDEX");

                            //adicionando a lista
                            listaPaineis.Add(reader.GetInt32("PAIN_CD_PAINEL"), painel);
                        }
                    }

                    //colocando os objetos
                    if (!reader.IsDBNull(reader.GetOrdinal("OBES_CD_OBJETO_ESTUDO")))
                    {
                        if (!listaObjeto.ContainsKey(reader.GetInt32("OBES_CD_OBJETO_ESTUDO")))
                        {
                            ObjetoEstudoDTO objetoEstudo = new ObjetoEstudoDTO();
                            objetoEstudo.CorObjeto = reader.GetString("OBES_NM_COR");
                            objetoEstudo.Espessura = reader.GetInt32("OBES_NR_ESPESSURA");
                            objetoEstudo.Id = reader.GetInt32("OBES_CD_OBJETO_ESTUDO");
                            objetoEstudo.IndexPainel = reader.GetInt32("OBES_NR_INDEXPAINEL");
                            objetoEstudo.InfinitaADireita = reader.GetBoolean("OBES_IN_INFINITA");
                            objetoEstudo.Magnetica = reader.GetBoolean("OBES_IN_MAGNETICA");
                            objetoEstudo.Parametros = reader.GetString("OBES_NM_PARAMETRO");
                            objetoEstudo.RecordFinal = reader.GetInt32("OBES_NR_RECORDFINAL");
                            objetoEstudo.RecordInicial = reader.GetInt32("OBES_NR_RECORDINICIAL");
                            objetoEstudo.TamanhoTexto = reader.GetInt32("OBES_NR_TAMANHOTEXTO");
                            objetoEstudo.Texto = reader.GetString("OBES_NM_TEXTO");
                            objetoEstudo.TipoLinha = reader.GetInt32("OBES_IN_TIPO_LINHA");
                            objetoEstudo.TipoObjeto = reader.GetInt32("OBES_IN_TIPOOBJETO");
                            objetoEstudo.ValorErrorChannel = reader.GetDecimal("OBES_VL_ERRORCHANNEL");
                            objetoEstudo.ValorFinal = reader.GetDouble("OBES_VL_FINAL");
                            objetoEstudo.ValorInicial = reader.GetDouble("OBES_VL_INICIAL");
                            objetoEstudo.LayoutId = reader.GetInt32("LAYO_CD_LAYOUT");
                            
                            listaObjeto.Add(objetoEstudo.Id, objetoEstudo);
                        }
                    }
                }
                #endregion

                #region Mapeando objetos
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
                for (int l = 0; l < listalayout.Count; l++)
                {
                    //percorrendo os objetos
                    for (int i = 0; i < listaObjeto.Count; i++)
                    {
                        if (listaObjeto[listaObjeto.Keys.ElementAt(i)].LayoutId == listalayout[listalayout.Keys.ElementAt(l)].Id)
                        {
                            if (listalayout[listalayout.Keys.ElementAt(l)].Objetos == null)
                                listalayout[listalayout.Keys.ElementAt(l)].Objetos = new List<ObjetoEstudoDTO>();
                            listalayout[listalayout.Keys.ElementAt(l)].Objetos.Add(listaObjeto[listaObjeto.Keys.ElementAt(i)]);
                        }
                    }
                }

                for (int t = 0; t < listaGrafico.Count; t++)
                {
                    GraficoDTO graficoDTO = new GraficoDTO();
                    graficoDTO = listaGrafico[listaGrafico.Keys.ElementAt(t)];
                    graficoDTO.Layouts = new List<LayoutDTO>();
                    for (int l = 0; l < listalayout.Count; l++)
                    {
                        if (listalayout[listalayout.Keys.ElementAt(l)].GraficoId == graficoDTO.Id)
                        {
                            graficoDTO.Layouts.Add(listalayout[listalayout.Keys.ElementAt(l)]);
                        }
                    }

                    listaPrincipalGraficos.Add(graficoDTO.Id, graficoDTO);
                }

                for (int a = 0; a < listaWorkspaces.Count; a++)
                {
                    WorkspaceDTO workspaceAux = new WorkspaceDTO();
                    workspaceAux = listaWorkspaces[listaWorkspaces.Keys.ElementAt(a)];

                    for (int g = 0; g < listaPrincipalGraficos.Count; g++)
                    {
                        GraficoDTO graficoDTO = new GraficoDTO();
                        graficoDTO = listaPrincipalGraficos[listaPrincipalGraficos.Keys.ElementAt(g)];
                        if (listaWorkspaces[listaWorkspaces.Keys.ElementAt(a)].Id == graficoDTO.WorkspaceId)
                        {
                            workspaceAux.Graficos.Add(listaPrincipalGraficos[listaPrincipalGraficos.Keys.ElementAt(g)]);
                        }
                                                
                    }

                    listaRetorno.Add(workspaceAux);
                }

                #endregion

                return listaRetorno;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        #endregion

        #region Metodos Write

        /// <summary>
        /// Metodo que exclui o workspace passado
        /// </summary>
        /// <param name="workspace"></param>
        public void ExcluirWorkspace(WorkspaceDTO workspace)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                foreach (GraficoDTO grafico in workspace.Graficos)
                {
                    foreach (LayoutDTO layout in grafico.Layouts)
                    {
                        //apagando os LAYOUTS
                        using (LayoutDAO layoutDAO = new LayoutDAO(readConnection, writeConnection))
                        {
                            layoutDAO.ExcluiLayout(layout);
                        }
                    }

                    //apagando os graficos
                    using (GraficoDAO graficoDAO = new GraficoDAO(readConnection, writeConnection))
                    {
                        graficoDAO.ExcluiGrafico(grafico);
                    }
                }

                //apagando o template
                hql = new StringBuilder();
                hql.Append(" DELETE FROM ");
                hql.Append(" WORKSPACE ");
                hql.Append(" WHERE WORK_CD_WORKSPACE = " + workspace.Id);

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
        /// Metodo que faz a exclusao de todos os workspaces de 1 usuario
        /// </summary>
        /// <param name="userId"></param>
        public void ExcluirWorkspacesUsuario(int userId)
        {
            try
            {
                List<WorkspaceDTO> listaWorkspace = this.GetWorkspacesByUserId(userId);
                if (listaWorkspace != null)
                {
                    foreach (WorkspaceDTO workspace in listaWorkspace)
                    {
                        this.ExcluirWorkspace(workspace);
                    }
                }
            }
            catch (Exception exc)
            {                
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que faz a inserção de um workspace
        /// </summary>
        /// <param name="workspace"></param>
        public void InsereWorkspace(WorkspaceDTO workspace)
        {
            try
            {
                //Salvando os graficos
                StringBuilder hql = new StringBuilder();
                hql.Append(" INSERT INTO WORKSPACE (WORK_NM_WORKSPACE, USUA_CD_USUARIO, DIST_CD_DISTRIBUIDOR) ");
                hql.Append(" VALUES (?nome, ?usuario, ?distribuidor)");

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //Setando os parametros
                    command.Parameters.AddWithValue("?nome", workspace.Nome);
                    command.Parameters.AddWithValue("?usuario", workspace.UsuarioId);
                    command.Parameters.AddWithValue("?distribuidor", null);

                    //executando
                    command.ExecuteNonQuery();
                    workspace.Id = Convert.ToInt32(command.LastInsertedId);
                }
                                
                foreach (GraficoDTO graficoObj in workspace.Graficos)
                {                    
                    //Salvando o grafico
                    using (GraficoDAO graficoDAO = new GraficoDAO(readConnection, writeConnection))
                    {
                        graficoObj.WorkspaceId = workspace.Id;
                        graficoDAO.InserirGrafico(graficoObj);
                    }

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
