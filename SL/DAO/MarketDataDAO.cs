using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Traderdata.Client.TerminalWEB.DTO;
using System.Threading;
using System.IO.IsolatedStorage;
using System.IO;
using System.Globalization;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using Traderdata.Client.TerminalWEB.Util;

namespace Traderdata.Client.TerminalWEB.DAO
{
    public class MarketDataDAO
    {
        #region Variaveis

        /// <summary>
        /// Variavel de acesso aos webservices
        /// </summary>
        private TerminalWebSVC.TerminalWebClient terminalWebClient =
            new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());

        #endregion

        #region Eventos


        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de cotação.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void CotacaoDiarioHandler(List<CotacaoDTO> Result);

        /// <summary>Evento disparado quando a ação de GetCotacaoDiaria é executada.</summary>
        public event CotacaoDiarioHandler GetCotacaoDiariaCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de set de cache
        /// </summary>
        /// <param name="tick"></param>
        public delegate void CotacaoDiarioCacheHandler(string Result);

        /// <summary>Evento disparado quando a ação de GetCotacaoDiaria é executada.</summary>
        public event CotacaoDiarioCacheHandler SetCotacaoDiariaCacheCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de set de cache
        /// </summary>
        /// <param name="tick"></param>
        public delegate void SetCotacaoIntradayCacheHandler(string Result);

        /// <summary>Evento disparado quando a ação de GetCotacaoDiaria é executada.</summary>
        public event SetCotacaoIntradayCacheHandler SetCotacaoIntradayCacheCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de cotação.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void CotacaoIntradayHandler(List<CotacaoDTO> Result);

        /// <summary>Evento disparado quando a ação de getCotacaoIntraday é executada.</summary>
        public event CotacaoIntradayHandler GetCotacaoIntradayCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de segmentos.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetSegmentosHandler(List<string> Result);

        /// <summary>Evento disparado quando a lista de segmentos é recebida.</summary>
        public event GetSegmentosHandler GetSegmentosCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de segmentos.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void SetCacheSegmentosHandler(List<string> Result);

        /// <summary>Evento disparado quando a lista de segmentos é recebida.</summary>
        public event SetCacheSegmentosHandler SetCacheSegmentosCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos por segmento.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void AtivosPorSegmentoHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a lista de segmentos é recebida.</summary>
        public event AtivosPorSegmentoHandler GetAtivosPorSegmentoCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de indices.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetIndicesHandler(List<string> Result);

        /// <summary>Evento disparado quando a ação de ping é executada.</summary>
        public event GetIndicesHandler GetIndicesCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de indices.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void SetCacheIndicesHandler(List<string> Result);

        /// <summary>Evento disparado quando a ação de ping é executada.</summary>
        public event SetCacheIndicesHandler SetCacheIndicesCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos por indice.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosPorIndiceHandler(List<AtivoDTO> Result, string Indice);

        /// <summary>Evento disparado quando a ação de ping é executada.</summary>
        public event GetAtivosPorIndiceHandler GetAtivosPorIndiceCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos que devem ser cacheados
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBovespaQueDevemSerCacheadosHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de ping é executada.</summary>
        public event GetAtivosBovespaQueDevemSerCacheadosHandler GetAtivosBovespaQueDevemSerCacheadosCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos por indice.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void SetCacheAtivosPorIndiceHandler(List<AtivoDTO> Result, string Indice);

        /// <summary>Evento disparado quando a ação de ping é executada.</summary>
        public event SetCacheAtivosPorIndiceHandler SetCacheAtivosPorIndiceCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos bovespa.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBovespaTodosHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoTodos é executada.</summary>
        public event GetAtivosBovespaTodosHandler GetAtivosBovespaTodosCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos bovespa.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void SetCacheAtivosBovespaHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoTodos é executada.</summary>
        public event SetCacheAtivosBovespaHandler SetCacheAtivosBovespaCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos bovespa.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBovespaVistaHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoVista é executada.</summary>
        public event GetAtivosBovespaVistaHandler GetAtivosBovespaVistaCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos bovespa.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBovespaOpcaoHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoOpcao é executada.</summary>
        public event GetAtivosBovespaOpcaoHandler GetAtivosBovespaOpcaoCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos bovespa.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBovespaTermoHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoTermo é executada.</summary>
        public event GetAtivosBovespaTermoHandler GetAtivosBovespaTermoCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos BMF.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBMFTodosHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoTermo é executada.</summary>
        public event GetAtivosBMFTodosHandler GetAtivosBMFTodosCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos BMF.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void SetCacheAtivosBMFTodosHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoTermo é executada.</summary>
        public event SetCacheAtivosBMFTodosHandler SetCacheAtivosBMFTodosCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos BMF principais.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBMFPrincpalCheioHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoTermo é executada.</summary>
        public event GetAtivosBMFPrincpalCheioHandler GetAtivosBMFPrincipalCheioCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos BMF - mini contratos
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBMFMiniContratosHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoTermo é executada.</summary>
        public event GetAtivosBMFMiniContratosHandler GetAtivosBMFMiniContratosCompleted;
        #endregion

        #region Construtor

        /// <summary>
        /// Metodo que deve ser executado para se iniciar a classe estatica de marketdata
        /// </summary>        
        public MarketDataDAO()
        {
            //assinando eventos WCF
            terminalWebClient.GetAtivosPorIndiceCompleted += new EventHandler<TerminalWebSVC.GetAtivosPorIndiceCompletedEventArgs>(terminalWebClient_GetAtivosPorIndiceCompleted);
            terminalWebClient.GetIndicesCompleted += new EventHandler<TerminalWebSVC.GetIndicesCompletedEventArgs>(terminalWebClient_GetIndicesCompleted);
            terminalWebClient.GetCotacaoDiarioCompleted += new EventHandler<TerminalWebSVC.GetCotacaoDiarioCompletedEventArgs>(terminalWebClient_GetCotacaoDiarioCompleted);
            terminalWebClient.GetCotacaoIntradayCompleted += new EventHandler<TerminalWebSVC.GetCotacaoIntradayCompletedEventArgs>(terminalWebClient_GetCotacaoIntradayCompleted);
            terminalWebClient.GetCotacaoIntradayDelayedCompleted += new EventHandler<TerminalWebSVC.GetCotacaoIntradayDelayedCompletedEventArgs>(terminalWebClient_GetCotacaoIntradayDelayedCompleted);

            terminalWebClient.GetAtivosBovespaTodosCompleted+=new EventHandler<TerminalWebSVC.GetAtivosBovespaTodosCompletedEventArgs>(terminalWebClient_GetAtivosBovespaTodosCompleted);
            terminalWebClient.GetAtivosBovespaOpcaoCompleted += new EventHandler<TerminalWebSVC.GetAtivosBovespaOpcaoCompletedEventArgs>(terminalWebClient_GetAtivosBovespaOpcaoCompleted);
            terminalWebClient.GetAtivosBovespaTermoCompleted += new EventHandler<TerminalWebSVC.GetAtivosBovespaTermoCompletedEventArgs>(terminalWebClient_GetAtivosBovespaTermoCompleted);
            terminalWebClient.GetAtivosBovespaVistaCompleted += new EventHandler<TerminalWebSVC.GetAtivosBovespaVistaCompletedEventArgs>(terminalWebClient_GetAtivosBovespaVistaCompleted);
            terminalWebClient.GetAtivosBMFTodosCompleted += new EventHandler<TerminalWebSVC.GetAtivosBMFTodosCompletedEventArgs>(terminalWebClient_GetAtivosBMFTodosCompleted);
            terminalWebClient.GetAtivosBMFCheioCompleted += new EventHandler<TerminalWebSVC.GetAtivosBMFCheioCompletedEventArgs>(terminalWebClient_GetAtivosBMFCheioCompleted);
            terminalWebClient.GetAtivosBMFMiniCompleted += new EventHandler<TerminalWebSVC.GetAtivosBMFMiniCompletedEventArgs>(terminalWebClient_GetAtivosBMFMiniCompleted);
            terminalWebClient.GetSegmentosCompleted += new EventHandler<TerminalWebSVC.GetSegmentosCompletedEventArgs>(terminalWebClient_GetSegmentosCompleted);
            terminalWebClient.GetAtivosPorSegmentoCompleted += new EventHandler<TerminalWebSVC.GetAtivosPorSegmentoCompletedEventArgs>(terminalWebClient_GetAtivosPorSegmentoCompleted);
            //terminalWebClient.GetCotacaoIntradayDoDiaCompleted += new EventHandler<TerminalWebSVC.GetCotacaoIntradayDoDiaCompletedEventArgs>(terminalWebClient_GetCotacaoIntradayDoDiaCompleted);

            //assinando eventos de realtime
            RealTimeDAO.TickReceived += new RealTimeDAO.TickHandler(RealTimeDAO_TickReceived);
        }

        

        #endregion

        #region Eventos WCF

        ///// <summary>
        ///// Evento que retorna após o sistema devolver as cotacoes intraday do dia corrente
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void terminalWebClient_GetCotacaoIntradayDoDiaCompleted(object sender, TerminalWebSVC.GetCotacaoIntradayDoDiaCompletedEventArgs e)
        //{
        //    List<CotacaoDTO> listaCotacao = new List<CotacaoDTO>();
        //    foreach (string obj in e.Result)
        //    {
        //        string[] colunas = obj.Split(';');
        //        DateTime data = new DateTime(Convert.ToInt32(colunas[6].Substring(6, 4)),
        //            Convert.ToInt32(colunas[6].Substring(3, 2)),
        //            Convert.ToInt32(colunas[6].Substring(0, 2)),
        //            Convert.ToInt32(colunas[7].Substring(0, 2)),
        //            Convert.ToInt32(colunas[7].Substring(2, 2)),
        //            0);
        //        listaCotacao.Add(new CotacaoDTO(Convert.ToDouble(colunas[0], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[1], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[2], GeneralUtil.NumberProvider),
        //            Convert.ToDouble(colunas[3], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[4], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[5], GeneralUtil.NumberProvider), data, Convert.ToBoolean(colunas[8]), colunas[7]));
        //    }

        //    //montando a lista, juntando as cotações do S3 + as que eu baixei agora
        //    List<CotacaoDTO> listaTemp = new List<CotacaoDTO>();
        //    foreach (CotacaoDTO obj in StaticData.cacheCotacaoIntradayS3[(string)e.UserState])
        //    {
        //        listaTemp.Add(obj);
        //    }
        //    foreach (CotacaoDTO obj in listaCotacao)
        //    {
        //        listaTemp.Add(obj);
        //    }

        //    //armazenando no cache
        //    StaticData.cacheCotacaoIntraday[(string)e.UserState] = listaTemp;

        //    //assinando a atualização para o ativo resgatado
        //    RealTimeDAO.StartTickSubscription((string)e.UserState);

        //    //Disparanbdo o evento
        //    if (GetCotacaoIntradayCompleted != null)
        //        GetCotacaoIntradayCompleted(listaTemp);
        //}


        /// <summary>
        /// Evento de complete dos dados intraday
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetCotacaoIntradayCompleted(object sender, TerminalWebSVC.GetCotacaoIntradayCompletedEventArgs e)
        {
            List<CotacaoDTO> listaCotacao = new List<CotacaoDTO>();
            foreach (string obj in e.Result)
            {
                string[] colunas = obj.Split(';');
                if (colunas[7].Length == 3)
                    colunas[7] = "0" + colunas[7];
                DateTime data = new DateTime(Convert.ToInt32(colunas[6].Substring(6, 4)), 
                    Convert.ToInt32(colunas[6].Substring(3, 2)), 
                    Convert.ToInt32(colunas[6].Substring(0, 2)),
                    Convert.ToInt32(colunas[7].Substring(0,2)),
                    Convert.ToInt32(colunas[7].Substring(2,2)),
                    0);
                listaCotacao.Add(new CotacaoDTO(Convert.ToDouble(colunas[0], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[1], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[2], GeneralUtil.NumberProvider),
                    Convert.ToDouble(colunas[3], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[4], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[5], GeneralUtil.NumberProvider), data, Convert.ToBoolean(colunas[8]), colunas[7]));
            }

            List<object> args = (List<object>)e.UserState;

            //armazenando no cache
            StaticData.cacheCotacaoIntraday[(string)args[0]] = listaCotacao;

            //Disparanbdo o evento
            if ((bool)args[1])
            {
                if (GetCotacaoIntradayCompleted != null)
                    GetCotacaoIntradayCompleted(listaCotacao);
            }
        }

        /// <summary>
        /// Evento disparado ao receber as cotações com atraso intraday
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetCotacaoIntradayDelayedCompleted(object sender, TerminalWebSVC.GetCotacaoIntradayDelayedCompletedEventArgs e)
        {
            List<CotacaoDTO> listaCotacao = new List<CotacaoDTO>();
            foreach (string obj in e.Result)
            {
                string[] colunas = obj.Split(';');
                if (colunas[7].Length == 3)
                    colunas[7] = "0" + colunas[7];
                DateTime data = new DateTime(Convert.ToInt32(colunas[6].Substring(6, 4)),
                    Convert.ToInt32(colunas[6].Substring(3, 2)),
                    Convert.ToInt32(colunas[6].Substring(0, 2)),
                    Convert.ToInt32(colunas[7].Substring(0, 2)),
                    Convert.ToInt32(colunas[7].Substring(2, 2)),
                    0);
                listaCotacao.Add(new CotacaoDTO(Convert.ToDouble(colunas[0], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[1], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[2], GeneralUtil.NumberProvider),
                    Convert.ToDouble(colunas[3], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[4], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[5], GeneralUtil.NumberProvider), data, Convert.ToBoolean(colunas[8]), colunas[7]));
            }

            List<object> args = (List<object>)e.UserState;

            //armazenando no cache
            StaticData.cacheCotacaoIntraday[(string)args[0]] = listaCotacao;

            //Disparanbdo o evento
            if ((bool)args[1])
            {
                if (GetCotacaoIntradayCompleted != null)
                    GetCotacaoIntradayCompleted(listaCotacao);
            }
        }


        /// <summary>
        /// Evento de complete dos dados diarios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetCotacaoDiarioCompleted(object sender, TerminalWebSVC.GetCotacaoDiarioCompletedEventArgs e)
        {
            if (GetCotacaoDiariaCompleted != null)
            {
                List<CotacaoDTO> listaCotacao = new List<CotacaoDTO>();
                foreach (string obj in e.Result)
                {
                    string[] colunas = obj.Split(';');
                    listaCotacao.Add(new CotacaoDTO(Convert.ToDouble(colunas[0], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[1], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[2], GeneralUtil.NumberProvider),
                        Convert.ToDouble(colunas[3], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[4], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[5], GeneralUtil.NumberProvider), Convert.ToDateTime(colunas[6]), false, ""));
                }
                
                //armazenando no cache
                StaticData.cacheCotacaoDiario[(string)e.UserState] = listaCotacao;
                
                //assinando a atualização para o ativo resgatado
                RealTimeDAO.StartTickSubscription((string)e.UserState);

                //Disparanbdo o evento
                GetCotacaoDiariaCompleted(listaCotacao);
            }
                
        }

        /// <summary>
        /// Evento de complete de retorno de ativos por indice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetAtivosPorIndiceCompleted(object sender, TerminalWebSVC.GetAtivosPorIndiceCompletedEventArgs e)
        {
            if (GetAtivosPorIndiceCompleted != null)
            {
                List<AtivoDTO> listaAtivo = new List<AtivoDTO>();

                foreach (string obj in e.Result)
                {
                    string[] colunas = obj.Split(';');
                    AtivoDTO ativo = new AtivoDTO();
                    ativo.Codigo = colunas[0];
                    ativo.Empresa = colunas[1];

                    listaAtivo.Add(ativo);
                }

                //armazenando no cache
                StaticData.cacheAtivosPorIndice[(string)e.UserState] = listaAtivo;

                //Disparanbdo o evento
                GetAtivosPorIndiceCompleted(listaAtivo, (string)e.UserState);
            }
        }

        /// <summary>
        /// Evento de complete de indices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetIndicesCompleted(object sender, TerminalWebSVC.GetIndicesCompletedEventArgs e)
        {
            if (GetIndicesCompleted != null)
            {
                //armazenando no cache
                StaticData.cacheIndices = e.Result;


                //Disparanbdo o evento
                GetIndicesCompleted(e.Result);
            }
        }

        /// <summary>
        /// Evento disparado ao se terminar de carregar os ativos bovespa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetAtivosBovespaTodosCompleted(object sender, TerminalWebSVC.GetAtivosBovespaTodosCompletedEventArgs e)
        {
            if (GetAtivosBovespaTodosCompleted != null)
            {
                List<AtivoDTO> listaAtivo = new List<AtivoDTO>();

                foreach (string obj in e.Result)
                {
                    string[] colunas = obj.Split(';');
                    AtivoDTO ativo = new AtivoDTO();
                    ativo.Codigo = colunas[0];
                    ativo.Empresa = colunas[1];

                    listaAtivo.Add(ativo);
                }

                //armazenando no cache
                StaticData.cacheAtivosBovespaTodos = listaAtivo;

                //Disparanbdo o evento
                GetAtivosBovespaTodosCompleted(listaAtivo);
                
            }
        }

        /// <summary>
        /// Evento disparado ao se terminar de carregar os ativos bovespa (somente a vista)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetAtivosBovespaVistaCompleted(object sender, TerminalWebSVC.GetAtivosBovespaVistaCompletedEventArgs e)
        {
            if (GetAtivosBovespaVistaCompleted != null)
            {
                List<AtivoDTO> listaAtivo = new List<AtivoDTO>();

                foreach (string obj in e.Result)
                {
                    string[] colunas = obj.Split(';');
                    AtivoDTO ativo = new AtivoDTO();
                    ativo.Codigo = colunas[0];
                    ativo.Empresa = colunas[1];

                    listaAtivo.Add(ativo);
                }

                //armazenando no cache
                StaticData.cacheAtivosBovespaVista = listaAtivo;

                //Disparanbdo o evento
                GetAtivosBovespaVistaCompleted(listaAtivo);

            }

        }

        /// <summary>
        /// Evento disparado ao se terminar de carregar os ativos bovespa (somente a termo)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetAtivosBovespaTermoCompleted(object sender, TerminalWebSVC.GetAtivosBovespaTermoCompletedEventArgs e)
        {
            if (GetAtivosBovespaTermoCompleted != null)
            {
                List<AtivoDTO> listaAtivo = new List<AtivoDTO>();

                foreach (string obj in e.Result)
                {
                    string[] colunas = obj.Split(';');
                    AtivoDTO ativo = new AtivoDTO();
                    ativo.Codigo = colunas[0];
                    ativo.Empresa = colunas[1];

                    listaAtivo.Add(ativo);
                }

                //armazenando no cache
                StaticData.cacheAtivosBovespaTermo = listaAtivo;

                //Disparanbdo o evento
                GetAtivosBovespaTermoCompleted(listaAtivo);

            }


        }

        /// <summary>
        /// Evento disparado ao se terminar de carregar os ativos bovespa (somente opção)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetAtivosBovespaOpcaoCompleted(object sender, TerminalWebSVC.GetAtivosBovespaOpcaoCompletedEventArgs e)
        {
            if (GetAtivosBovespaOpcaoCompleted != null)
            {
                List<AtivoDTO> listaAtivo = new List<AtivoDTO>();

                foreach (string obj in e.Result)
                {
                    string[] colunas = obj.Split(';');
                    AtivoDTO ativo = new AtivoDTO();
                    ativo.Codigo = colunas[0];
                    ativo.Empresa = colunas[1];

                    listaAtivo.Add(ativo);
                }

                //armazenando no cache
                StaticData.cacheAtivosBovespaOpcao = listaAtivo;

                //Disparanbdo o evento
                GetAtivosBovespaOpcaoCompleted(listaAtivo);

            }

        }

        /// <summary>
        /// Evento disparado ao se terminar de carregar os ativos BMF (Todos)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetAtivosBMFTodosCompleted(object sender, TerminalWebSVC.GetAtivosBMFTodosCompletedEventArgs e)
        {
            if (GetAtivosBMFTodosCompleted != null)
            {
                List<AtivoDTO> listaAtivo = new List<AtivoDTO>();

                foreach (string obj in e.Result)
                {
                    string[] colunas = obj.Split(';');
                    AtivoDTO ativo = new AtivoDTO();
                    ativo.Codigo = colunas[0];
                    ativo.Empresa = colunas[1];

                    listaAtivo.Add(ativo);
                }

                //armazenando no cache
                StaticData.cacheAtivosBMFTodos = listaAtivo;

                //Disparanbdo o evento
                GetAtivosBMFTodosCompleted(listaAtivo);

            }
  
        }

        /// <summary>
        /// Evento disparado ao se terminar de carregar os ativos BMF (Mini)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetAtivosBMFMiniCompleted(object sender, TerminalWebSVC.GetAtivosBMFMiniCompletedEventArgs e)
        {
            if (GetAtivosBMFMiniContratosCompleted != null)
            {
                List<AtivoDTO> listaAtivo = new List<AtivoDTO>();

                foreach (string obj in e.Result)
                {
                    string[] colunas = obj.Split(';');
                    AtivoDTO ativo = new AtivoDTO();
                    ativo.Codigo = colunas[0];
                    ativo.Empresa = colunas[1];

                    listaAtivo.Add(ativo);
                }

                //armazenando no cache
                StaticData.cacheAtivosBMFMiniContrato = listaAtivo;

                //Disparanbdo o evento
                GetAtivosBMFMiniContratosCompleted(listaAtivo);

            }

        }

        /// <summary>
        /// Evento disparado ao se terminar de carregar os ativos BMF (Cheio)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetAtivosBMFCheioCompleted(object sender, TerminalWebSVC.GetAtivosBMFCheioCompletedEventArgs e)
        {
            if (GetAtivosBMFPrincipalCheioCompleted != null)
            {
                List<AtivoDTO> listaAtivo = new List<AtivoDTO>();

                foreach (string obj in e.Result)
                {
                    string[] colunas = obj.Split(';');
                    AtivoDTO ativo = new AtivoDTO();
                    ativo.Codigo = colunas[0];
                    ativo.Empresa = colunas[1];

                    listaAtivo.Add(ativo);
                }

                //armazenando no cache
                StaticData.cacheAtivosBMFPrincpalCheio = listaAtivo;

                //Disparanbdo o evento
                GetAtivosBMFPrincipalCheioCompleted(listaAtivo);

            }

        }

        /// <summary>
        /// Evento disparado ao se terminar de carregar os segmentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetSegmentosCompleted(object sender, TerminalWebSVC.GetSegmentosCompletedEventArgs e)
        {
            //armazenando no cache
            StaticData.cacheSegmentos = e.Result;

            if (GetSegmentosCompleted != null)
                GetSegmentosCompleted(e.Result);
        }

        /// <summary>
        /// Evento disparado ao se carregar os ativos por segenton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetAtivosPorSegmentoCompleted(object sender, TerminalWebSVC.GetAtivosPorSegmentoCompletedEventArgs e)
        {
            if (GetAtivosPorSegmentoCompleted!= null)
            {
                List<AtivoDTO> listaAtivo = new List<AtivoDTO>();

                foreach (string obj in e.Result)
                {
                    string[] colunas = obj.Split(';');
                    AtivoDTO ativo = new AtivoDTO();
                    ativo.Codigo = colunas[0];
                    ativo.Empresa = colunas[1];

                    listaAtivo.Add(ativo);
                }

                //armazenando no cache
                StaticData.cacheAtivosPorSegmento[(string)e.UserState] = listaAtivo;

                //Disparanbdo o evento
                GetAtivosPorSegmentoCompleted(listaAtivo);
            }
        }

        #endregion

        #region Eventos Realtime

        /// <summary>
        /// Evento disparado ao se receber um novo tick de qualquer ativo assinado
        /// </summary>
        /// <param name="Result"></param>
        void RealTimeDAO_TickReceived(object Result)
        {
            //atualizando cache diario
            //if (StaticData.cacheCotacaoDiario.ContainsKey(((TickDTO)Result).Ativo))
            //{                
            //    CotacaoDTO cotacaoAux = new CotacaoDTO(((TickDTO)Result).Abertura, ((TickDTO)Result).Maximo,
            //        ((TickDTO)Result).Minimo, ((TickDTO)Result).Ultimo, ((TickDTO)Result).Quantidade, ((TickDTO)Result).Volume,
            //        ((TickDTO)Result).Data.Date, false, ((TickDTO)Result).Hora);

            //    if (StaticData.cacheCotacaoDiario[((TickDTO)Result).Ativo][StaticData.cacheCotacaoDiario[((TickDTO)Result).Ativo].Count - 1].Data.Date == DateTime.Today)
            //        StaticData.cacheCotacaoDiario[((TickDTO)Result).Ativo][StaticData.cacheCotacaoDiario[((TickDTO)Result).Ativo].Count - 1] = cotacaoAux;
            //    else
            //        StaticData.cacheCotacaoDiario[((TickDTO)Result).Ativo].Add(cotacaoAux);
            //}

        }

        #endregion

        #region Eventos Cache

        /// <summary>
        /// Evento disparado ao se terminar de carregar os ativos BMF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webClientDownloadAtivosBMF_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            //descompactando 
            string text = Uncompress(e.Result);

            List<AtivoDTO> listaAtivoTodos = new List<AtivoDTO>();
            List<AtivoDTO> listaAtivoMini = new List<AtivoDTO>();
            List<AtivoDTO> listaAtivoCheio = new List<AtivoDTO>();
            
            foreach (string obj in text.Split('|'))
            {
                if (obj.Trim().Length > 0)
                {
                    string[] colunas = obj.Split(';');
                    AtivoDTO ativo = new AtivoDTO();
                    ativo.Codigo = colunas[0].Trim();
                    ativo.Empresa = colunas[1].Trim();

                    listaAtivoTodos.Add(ativo);

                    if ((ativo.Codigo.Substring(0, 3) == "WIN") ||
                         (ativo.Codigo.Substring(0, 3) == "WDO") ||
                         (ativo.Codigo.Substring(0, 3) == "WCF") ||
                         (ativo.Codigo.Substring(0, 3) == "WGI"))
                        listaAtivoMini.Add(ativo);

                    if ((ativo.Codigo.Substring(0,3) == "IND") ||
                         (ativo.Codigo.Substring(0, 3) =="DOL") ||
                         (ativo.Codigo.Substring(0, 3)=="ICF") ||
                         (ativo.Codigo.Substring(0, 3)=="BGI"))
                        listaAtivoCheio.Add(ativo);


                }
            }

            //armazenando no cache
            StaticData.cacheAtivosBMFTodos = listaAtivoTodos;
            StaticData.cacheAtivosBMFMiniContrato= listaAtivoMini;
            StaticData.cacheAtivosBMFPrincpalCheio = listaAtivoCheio;
            
            if (SetCacheAtivosBMFTodosCompleted != null)
            {
                //Disparanbdo o evento
                SetCacheAtivosBMFTodosCompleted(listaAtivoTodos);
            }
        }

        /// <summary>
        /// Evento disparado ao terminar de fazer download do arquivo de indices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webClientDownloadIndices_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            //descompactando 
            string text = Uncompress(e.Result);

            foreach (string obj in text.Split('|'))
            {
                if (obj.Trim().Length > 0)
                {
                    StaticData.cacheIndices.Add(obj.Trim());
                }
            }
                        
            if (SetCacheIndicesCompleted != null)
            {
                //Disparanbdo o evento
                SetCacheIndicesCompleted(StaticData.cacheIndices);
            }
        }

        /// <summary>
        /// Evento disparado ao receber um arquivo de cotações intraday
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webclientDowloadIntraday_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                //descompactando
                string text = Uncompress(e.Result);

                List<CotacaoDTO> listaCotacao = new List<CotacaoDTO>();

                string[] linhas = text.Split('|');
                foreach (string obj in linhas)
                {
                    if (obj.Length > 0)
                    {
                        string[] colunas = obj.Split(';');
                        DateTime dataTemp = Convert.ToDateTime(colunas[6]).AddHours(Convert.ToDouble(colunas[7].Substring(0,2))).AddMinutes(Convert.ToDouble(colunas[7].Substring(2,2)));
                        

                        listaCotacao.Add(new CotacaoDTO(Convert.ToDouble(colunas[0], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[1], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[2], GeneralUtil.NumberProvider),
                            Convert.ToDouble(colunas[3], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[4], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[5], GeneralUtil.NumberProvider), dataTemp, Convert.ToBoolean(colunas[8]), colunas[7]));
                    }
                }

                //armazenando no cache
                if (StaticData.cacheCotacaoIntradayS3.ContainsKey((string)e.UserState))
                    StaticData.cacheCotacaoIntradayS3[(string)e.UserState] = listaCotacao;
                else
                    StaticData.cacheCotacaoIntradayS3.Add((string)e.UserState, listaCotacao);

                //disparando evento de setCache
                if (SetCotacaoIntradayCacheCompleted != null)
                    SetCotacaoIntradayCacheCompleted((string)e.UserState);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Evento disparado ao terminar de fazer download do arquivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webclientDowloadDiarioCache_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                //descompactando
                string text = Uncompress(e.Result);

                List<CotacaoDTO> listaCotacao = new List<CotacaoDTO>();

                string[] linhas = text.Split('|');
                foreach (string obj in linhas)
                {
                    if (obj.Length > 0)
                    {
                        string[] colunas = obj.Split(';');
                        listaCotacao.Add(new CotacaoDTO(Convert.ToDouble(colunas[0], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[1], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[2], GeneralUtil.NumberProvider),
                            Convert.ToDouble(colunas[3], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[4], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[5], GeneralUtil.NumberProvider), Convert.ToDateTime(colunas[6]), false, ""));
                    }
                }

                //armazenando no cache
                StaticData.cacheCotacaoDiario[(string)e.UserState] = listaCotacao;

                //disparando evento de setCache
                if (SetCotacaoDiariaCacheCompleted != null)
                    SetCotacaoDiariaCacheCompleted((string)e.UserState);
            }
            catch 
            {

            }

        }

        /// <summary>
        /// Evento disparado ao terminar de fazer download do arquivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webclientDowloadDiario_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                //descompactando
                string text = Uncompress(e.Result);

                List<CotacaoDTO> listaCotacao = new List<CotacaoDTO>();

                string[] linhas = text.Split('|');
                foreach (string obj in linhas)
                {
                    if (obj.Length > 0)
                    {
                        string[] colunas = obj.Split(';');
                        listaCotacao.Add(new CotacaoDTO(Convert.ToDouble(colunas[0], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[1], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[2], GeneralUtil.NumberProvider),
                            Convert.ToDouble(colunas[3], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[4], GeneralUtil.NumberProvider), Convert.ToDouble(colunas[5], GeneralUtil.NumberProvider), Convert.ToDateTime(colunas[6]), false, ""));
                    }
                }

                //armazenando no cache
                StaticData.cacheCotacaoDiario[(string)e.UserState] = listaCotacao;

                //disparando evento de setCache
                if (GetCotacaoDiariaCompleted != null)
                    GetCotacaoDiariaCompleted(StaticData.cacheCotacaoDiario[(string)e.UserState]);
            }
            catch(Exception exc)
            {
                throw exc;
            }

        }

        /// <summary>
        /// Evento disparado ao se terminar de baixar a lista de ativos que compoem um indice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webClientDownloadAtivoPorIndice_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            //descompactando 
            string text = Uncompress(e.Result);

            List<AtivoDTO> listaAtivo = new List<AtivoDTO>();

            foreach (string obj in text.Split('|'))
            {
                if (obj.Trim().Length > 0)
                {
                    string[] colunas = obj.Split(';');
                    AtivoDTO ativo = new AtivoDTO();
                    ativo.Codigo = colunas[0].Trim();
                    ativo.Empresa = colunas[1].Trim();

                    listaAtivo.Add(ativo);
                }
            }

            //armazenando no cache
            StaticData.cacheAtivosPorIndice[(string)e.UserState] = listaAtivo;

            if (SetCacheAtivosPorIndiceCompleted != null)
            {
                //Disparanbdo o evento
                SetCacheAtivosPorIndiceCompleted(listaAtivo, (string)e.UserState);
            }
        }

        /// <summary>
        /// Setando o cache de ativos Bovespa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webClientDownloadAtivosBovespa_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            //descompactando 
            string text = Uncompress(e.Result);

            List<AtivoDTO> listaAtivoTodos = new List<AtivoDTO>();
            List<AtivoDTO> listaAtivoOpcao = new List<AtivoDTO>();
            List<AtivoDTO> listaAtivoVista = new List<AtivoDTO>();
            List<AtivoDTO> listaAtivoTermo = new List<AtivoDTO>();

            foreach (string obj in text.Split('|'))
            {
                if (obj.Trim().Length > 0)
                {
                    string[] colunas = obj.Split(';');
                    AtivoDTO ativo = new AtivoDTO();
                    ativo.Codigo = colunas[0].Trim();
                    ativo.Empresa = colunas[1].Trim();
                    
                    listaAtivoTodos.Add(ativo);

                    switch (colunas[2].Trim())
                    {
                        case "O":
                            listaAtivoOpcao.Add(ativo);
                            break;
                        case "V":
                            listaAtivoVista.Add(ativo);
                            break;
                        case "T":
                            listaAtivoTermo.Add(ativo);
                            break;
                    }
                }
            }

            //armazenando no cache
            StaticData.cacheAtivosBovespaTodos = listaAtivoTodos;
            StaticData.cacheAtivosBovespaVista = listaAtivoVista;
            StaticData.cacheAtivosBovespaTermo = listaAtivoTermo;
            StaticData.cacheAtivosBovespaOpcao = listaAtivoOpcao;

            if (SetCacheAtivosBovespaCompleted != null)
            {
                //Disparanbdo o evento
                SetCacheAtivosBovespaCompleted(listaAtivoTodos);
            }
        }

        /// <summary>
        /// Lista de ativos que devem ser cachados de Bovespa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webClientDownloadAtivosQuedevemSerCachadosBovespa_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            //descompactando 
            string text = Uncompress(e.Result);

            List<AtivoDTO> listaAtivo = new List<AtivoDTO>();
            
            foreach (string obj in text.Split('|'))
            {
                if (obj.Trim().Length > 0)
                {
                    string[] colunas = obj.Split(';');
                    AtivoDTO ativo = new AtivoDTO();
                    ativo.Codigo = colunas[0].Trim();
                    ativo.Empresa = colunas[1].Trim();

                    listaAtivo.Add(ativo);

                }
            }

            //setando o cache
            StaticData.cachePortfolioPadraoBovespa = listaAtivo;

            if (GetAtivosBovespaQueDevemSerCacheadosCompleted != null)
            {
                //Disparanbdo o evento
                GetAtivosBovespaQueDevemSerCacheadosCompleted(listaAtivo);
            }
        }

        /// <summary>
        /// Evento disparado ao se terminar de carregar os segmentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webClientDownloadSegmentos_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            //descompactando 
            string text = Uncompress(e.Result);

            foreach (string obj in text.Split('|'))
            {
                if (obj.Trim().Length > 0)
                {
                    string[] colunas = obj.Split(';');

                    if (!StaticData.cacheSegmentos.Contains(colunas[0].Trim()))
                        StaticData.cacheSegmentos.Add(colunas[0].Trim());                    
                        
                    if (StaticData.cacheAtivosPorSegmento.ContainsKey(colunas[0].Trim()))
                    {
                        AtivoDTO ativo = new AtivoDTO();
                        ativo.Codigo = colunas[1];
                        ativo.Empresa = colunas[2];
                        StaticData.cacheAtivosPorSegmento[colunas[0].Trim()].Add(ativo);
                    }
                    else
                    {
                        AtivoDTO ativo = new AtivoDTO();
                        ativo.Codigo = colunas[1];
                        ativo.Empresa = colunas[2];
                        List<AtivoDTO> listaTemp = new List<AtivoDTO>();
                        listaTemp.Add(ativo);
                        StaticData.cacheAtivosPorSegmento.Add(colunas[0].Trim(),listaTemp);
                    }
                }
            }

            if (SetCacheSegmentosCompleted != null)
            {
                //Disparanbdo o evento
                SetCacheSegmentosCompleted(StaticData.cacheSegmentos);
            }
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Metodo que retorna se o ativo é de Bovespa
        /// </summary>
        /// <param name="ativo"></param>
        public static bool IsBovespa(string ativo)
        {
            foreach (AtivoDTO obj in StaticData.cacheAtivosBovespaTodos)
            {
                if (obj.Codigo == ativo)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Metodo que retorna as cotações diarias
        /// </summary>
        /// <param name="ativo"></param>
        public void GetCotacaoDiariaAsync(string ativo)
        {
            try
            {
                if ((ativo != "PETR4") && (ativo != "IBOV"))
                {
                    if (IsBovespa(ativo))
                    {
                        if (!StaticData.User.HasSnapshotBovespaDiario)
                            return;
                    }
                    else
                    {
                        if (!StaticData.User.HasSnapshotBMFDiario)
                            return;
                    }
                }

                if (!StaticData.cacheCotacaoDiario.ContainsKey(ativo))
                {
                    terminalWebClient.GetCotacaoDiarioAsync(ativo, ativo);
                }
                else
                {
                    if (GetCotacaoDiariaCompleted != null)
                        GetCotacaoDiariaCompleted(StaticData.cacheCotacaoDiario[ativo]);
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
                        
        }

        /// <summary>
        /// Metodo que retorna as cotações intraday
        /// </summary>
        /// <param name="ativo"></param>
        public void GetCotacaoIntradayAsync(string ativo, bool disparaEvento, bool forceRefresh)
        {
            try
            {
                if (!forceRefresh)
                {
                    if (StaticData.cacheCotacaoIntraday.ContainsKey(ativo))
                    {
                        if (GetCotacaoIntradayCompleted != null)
                            GetCotacaoIntradayCompleted(StaticData.cacheCotacaoIntraday[ativo]);
                    }
                    else
                    {
                        List<object> args = new List<object>();
                        args.Add(ativo);
                        args.Add(disparaEvento);

                        if (!StaticData.DelayedVersion)
                            terminalWebClient.GetCotacaoIntradayAsync(ativo, 15, args);
                        else
                            terminalWebClient.GetCotacaoIntradayDelayedAsync(ativo, 45, args);
                    }
                }
                else
                {
                    List<object> args = new List<object>();
                    args.Add(ativo);
                    args.Add(disparaEvento);

                    if (!StaticData.DelayedVersion)
                        terminalWebClient.GetCotacaoIntradayAsync(ativo, 15, args);
                    else
                        terminalWebClient.GetCotacaoIntradayDelayedAsync(ativo, 45, args);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
              
        /// <summary>
        /// Metodo que seta o cache das cotações intraday
        /// </summary>
        /// <param name="ativo"></param>
        public void SetCacheCotacaoIntradayAsync(string ativo)
        {
            try
            {
                //if (!StaticData.cacheCotacaoDiario.ContainsKey(ativo))
                //{
                //    WebClient webclientDowloadIntraday = new WebClient();
                //    webclientDowloadIntraday.OpenReadCompleted += new OpenReadCompletedEventHandler(webclientDowloadIntraday_OpenReadCompleted);
                //    webclientDowloadIntraday.OpenReadAsync(new Uri("https://s3-sa-east-1.amazonaws.com/td-marketdata-intraday/" + ativo + ".zip"), ativo);
                //}
                List<object> args = new List<object>();
                args.Add(ativo);
                args.Add(false);
                if (!StaticData.DelayedVersion)
                    terminalWebClient.GetCotacaoIntradayAsync(ativo, 15, args);
                else
                    terminalWebClient.GetCotacaoIntradayDelayedAsync(ativo, 45, args);

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
               

        /// <summary>
        /// Metodo que seta o cache das cotações intraday
        /// </summary>
        /// <param name="ativo"></param>
        public void SetCacheCotacaoDiarioAsync(string ativo)
        {
            try
            {
                if (!StaticData.cacheCotacaoDiario.ContainsKey(ativo))
                {
                    WebClient webclientDowloadDiario = new WebClient();
                                        
                    webclientDowloadDiario.OpenReadCompleted += new OpenReadCompletedEventHandler(webclientDowloadDiarioCache_OpenReadCompleted);
                    //webclientDowloadDiario.OpenReadAsync(new Uri("https://s3-sa-east-1.amazonaws.com/md-cache/" + ativo + ".zip"), ativo);
                }
                
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna os segmentos
        /// </summary>
        /// <param name="ativo"></param>
        public void GetSegmentosAsync()
        {
            try
            {
                if (StaticData.cacheSegmentos.Count > 0)
                {
                    if (GetSegmentosCompleted != null)
                        GetSegmentosCompleted(StaticData.cacheSegmentos);

                }
                else
                    terminalWebClient.GetSegmentosAsync();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna os segmentos
        /// </summary>
        /// <param name="ativo"></param>
        public void SetCacheSegmentosAsync()
        {
            try
            {
                WebClient webClientDownloadSegmentos = new WebClient();
                webClientDownloadSegmentos.OpenReadCompleted += new OpenReadCompletedEventHandler(webClientDownloadSegmentos_OpenReadCompleted);
                //webClientDownloadSegmentos.OpenReadAsync(new Uri("https://s3-sa-east-1.amazonaws.com/td-marketdata/segmentos.zip"));

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        /// <summary>
        /// Metodo que retorna os ativos por segmento
        /// </summary>
        /// <param name="ativo"></param>
        public void GetAtivosPorSegmentoAsync(string segmento)
        {
            try
            {
                if (StaticData.cacheAtivosPorSegmento.ContainsKey(segmento))
                {
                    if (GetAtivosPorSegmentoCompleted != null)
                        GetAtivosPorSegmentoCompleted(StaticData.cacheAtivosPorSegmento[segmento]);

                }
                else
                    terminalWebClient.GetAtivosPorSegmentoAsync(segmento, segmento);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna os ativos por segmento
        /// </summary>
        /// <param name="ativo"></param>
        public void GetAtivosBovespaQueDevemSerCacheadosAsync()
        {
            try
            {
                if (StaticData.cachePortfolioPadraoBovespa.Count > 0)
                {
                    if (GetAtivosBovespaQueDevemSerCacheadosCompleted != null)
                    {
                        GetAtivosBovespaQueDevemSerCacheadosCompleted(StaticData.cachePortfolioPadraoBovespa);
                    }
                }
                else
                {
                    WebClient webClientDownloadAtivosQuedevemSerCachadosBovespa = new WebClient();
                    webClientDownloadAtivosQuedevemSerCachadosBovespa.OpenReadCompleted += new OpenReadCompletedEventHandler(webClientDownloadAtivosQuedevemSerCachadosBovespa_OpenReadCompleted);
                    //webClientDownloadAtivosQuedevemSerCachadosBovespa.OpenReadAsync(new Uri("https://s3-sa-east-1.amazonaws.com/td-marketdata/ativosCacheadosBovespa.zip"));
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna os indices
        /// </summary>
        /// <param name="ativo"></param>
        public void GetIndicesAsync()
        {
            try
            {
                if (StaticData.cacheIndices.Count > 0)
                {
                    if (GetIndicesCompleted != null)
                        GetIndicesCompleted(StaticData.cacheIndices);

                }
                else
                    terminalWebClient.GetIndicesAsync();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna os indices
        /// </summary>
        /// <param name="ativo"></param>
        public void SetCacheIndicesAsync()
        {
            try
            {
                WebClient webClientDownloadIndices = new WebClient();
                webClientDownloadIndices.OpenReadCompleted += new OpenReadCompletedEventHandler(webClientDownloadIndices_OpenReadCompleted);
                //webClientDownloadIndices.OpenReadAsync(new Uri("https://s3-sa-east-1.amazonaws.com/td-marketdata/indices.zip"));
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna os ativos por indices
        /// </summary>
        /// <param name="ativo"></param>
        public void GetAtivosPorIndiceAsync(string indice)
        {
            try
            {
                if (StaticData.cacheAtivosPorIndice.ContainsKey(indice))
                {
                    if (GetAtivosPorIndiceCompleted != null)
                        GetAtivosPorIndiceCompleted(StaticData.cacheAtivosPorIndice[indice], indice);
                }
                else
                {
                    terminalWebClient.GetAtivosPorIndiceAsync(indice, indice);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna os ativos por indices
        /// </summary>
        /// <param name="ativo"></param>
        public void SetCacheAtivosPorIndiceAsync(string indice)
        {
            try
            {
                WebClient webClientDownloadAtivosIndices = new WebClient();
                webClientDownloadAtivosIndices.OpenReadCompleted += new OpenReadCompletedEventHandler(webClientDownloadAtivoPorIndice_OpenReadCompleted);
                //webClientDownloadAtivosIndices.OpenReadAsync(new Uri("https://s3-sa-east-1.amazonaws.com/td-marketdata/" + indice + ".zip"), indice);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna os ativos bmf
        /// </summary>
        /// <param name="ativo"></param>
        public void GetAtivosBMFTodosAsync()
        {
            try
            {
                if (StaticData.cacheAtivosBMFTodos.Count > 0)
                {
                    if (GetAtivosBMFTodosCompleted != null)
                        GetAtivosBMFTodosCompleted(StaticData.cacheAtivosBMFTodos);
                }
                else
                    terminalWebClient.GetAtivosBMFTodosAsync();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna os ativos bmf
        /// </summary>
        /// <param name="ativo"></param>
        public void GetAtivosBMFMiniContratosAsync()
        {
            try
            {
                if (StaticData.cacheAtivosBMFMiniContrato.Count > 0)
                {
                    if (GetAtivosBMFMiniContratosCompleted != null)
                        GetAtivosBMFMiniContratosCompleted(StaticData.cacheAtivosBMFMiniContrato);
                }
                else
                    terminalWebClient.GetAtivosBMFMiniAsync();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna os ativos bmf principais cheios
        /// </summary>
        /// <param name="ativo"></param>
        public void GetAtivosBMFPrincipaisCheiosAsync()
        {
            try
            {
                if (StaticData.cacheAtivosBMFPrincpalCheio.Count > 0)
                {
                    if (GetAtivosBMFPrincipalCheioCompleted!= null)
                        GetAtivosBMFPrincipalCheioCompleted(StaticData.cacheAtivosBMFPrincpalCheio);
                }
                else
                    terminalWebClient.GetAtivosBMFCheioAsync();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna os ativos bovepa
        /// </summary>
        /// <param name="ativo"></param>
        public void GetAtivosBovespaTodosAsync()
        {
            try
            {
                if (StaticData.cacheAtivosBovespaTodos.Count > 0)
                {
                    if (GetAtivosBovespaTodosCompleted != null)
                        GetAtivosBovespaTodosCompleted(StaticData.cacheAtivosBovespaTodos);
                }
                else
                    terminalWebClient.GetAtivosBovespaTodosAsync();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna os ativos bovepa
        /// </summary>
        /// <param name="ativo"></param>
        public void SetCacheAtivosBovespaAsync()
        {
            try
            {
                WebClient webClientDownloadAtivosBovespa = new WebClient();
                webClientDownloadAtivosBovespa.OpenReadCompleted += new OpenReadCompletedEventHandler(webClientDownloadAtivosBovespa_OpenReadCompleted);
                //webClientDownloadAtivosBovespa.OpenReadAsync(new Uri("https://s3-sa-east-1.amazonaws.com/td-marketdata/ativosBovespa.zip"));
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna os ativos BMF
        /// </summary>
        /// <param name="ativo"></param>
        public void SetCacheAtivosBMFAsync()
        {
            try
            {
                WebClient webClientDownloadAtivosBMF = new WebClient();
                webClientDownloadAtivosBMF.OpenReadCompleted += new OpenReadCompletedEventHandler(webClientDownloadAtivosBMF_OpenReadCompleted);
                //webClientDownloadAtivosBMF.OpenReadAsync(new Uri("https://s3-sa-east-1.amazonaws.com/td-marketdata/ativosBMF.zip"));
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        /// <summary>
        /// Metodo que retorna os ativos bovepa (somente a vista)
        /// </summary>
        /// <param name="ativo"></param>
        public void GetAtivosBovespaVistaAsync()
        {
            try
            {
                if (StaticData.cacheAtivosBovespaVista.Count > 0)
                {
                    if (GetAtivosBovespaVistaCompleted != null)
                        GetAtivosBovespaVistaCompleted(StaticData.cacheAtivosBovespaVista);
                }
                else
                    terminalWebClient.GetAtivosBovespaVistaAsync();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna os ativos bovepa (somente opcao)
        /// </summary>
        /// <param name="ativo"></param>
        public void GetAtivosBovespaOpcaoAsync()
        {
            try
            {
                if (StaticData.cacheAtivosBovespaOpcao.Count > 0)
                {
                    if (GetAtivosBovespaOpcaoCompleted != null)
                        GetAtivosBovespaOpcaoCompleted(StaticData.cacheAtivosBovespaOpcao);
                }
                else
                    terminalWebClient.GetAtivosBovespaOpcaoAsync();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna os ativos bovepa (somente  atermo)
        /// </summary>
        /// <param name="ativo"></param>
        public void GetAtivosBovespaTermoAsync()
        {
            try
            {
                if (StaticData.cacheAtivosBovespaTermo.Count > 0)
                {
                    if (GetAtivosBovespaTermoCompleted != null)
                        GetAtivosBovespaTermoCompleted(StaticData.cacheAtivosBovespaTermo);
                }
                else
                    terminalWebClient.GetAtivosBovespaTermoAsync();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que espera receber a lista de cotação intraday em barras de minuto e devolve uma serie na periodicidade solicitada
        /// </summary>
        /// <returns></returns>
        public List<CotacaoDTO> ConvertPeriodicidadeIntraday(List<CotacaoDTO> listaCotacao1Minuto, int periodicidade)
        {
            try
            {
                //se a periodicidade for igual a um nao tem que converter
                if (periodicidade == 1)
                    return listaCotacao1Minuto;

                return AplicaPeriodicidadeGenericaPorMinuto(listaCotacao1Minuto, periodicidade);
            }
            catch (Exception exc)
            {                
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que espera receber a lista de cotação intraday em barras diarias e devolve uma serie na periodicidade solicitada
        /// </summary>
        /// <returns></returns>
        public List<CotacaoDTO> ConvertPeriodicidadeDiaria(List<CotacaoDTO> listaCotacaoDiaria, int periodicidade)
        {
            try
            {
                //se a periodicidade for igual a um nao tem que converter
                switch (periodicidade)
                {
                    case 1440:
                        return listaCotacaoDiaria;
                    case 10080:
                        return AplicaPeriodicidadeSemanal(listaCotacaoDiaria);
                    case 43200:
                        return AplicaPeriodicidadeMensal(listaCotacaoDiaria);
                    default:
                        return listaCotacaoDiaria;
                }
                    
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
                
        #region Ajuste de Periodicidade

        /// <summary>
        /// Metodo retornas as cotações organizadas em periodicidade semanal, de acordo com a semana brasileira
        /// </summary>
        /// <param name="listaTemp"></param>
        /// <returns></returns>
        private List<CotacaoDTO> AplicaPeriodicidadeSemanal(List<CotacaoDTO> listaCotacao)
        {
            List<CotacaoDTO> listaResultado = new List<CotacaoDTO>();
            List<CotacaoDTO> listaTemp = new List<CotacaoDTO>();
            int numeroSemana = -1;
            int numeroSemanaAnterior = -1;
            CultureInfo myCI = new CultureInfo("pt-br");
            System.Globalization.Calendar myCal = myCI.Calendar;

            try
            {
                //Percorrer a lista de cotações, verificando pelo quebra do número da semana
                foreach (CotacaoDTO objTemp in listaCotacao)
                {
                    numeroSemana = myCal.GetWeekOfYear(objTemp.Data, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
                    if (numeroSemana != numeroSemanaAnterior)
                    {
                        if (listaTemp.Count > 0)
                        {
                            //nesse caso deve gerar a barra e inserir no resultado
                            listaResultado.Add(GeraBarraDiaria(listaTemp));
                            listaTemp.Clear();
                        }
                        listaTemp.Add(objTemp);
                        numeroSemanaAnterior = numeroSemana;
                    }
                    else
                        listaTemp.Add(objTemp);
                }
                //Verifico se ficou dado no hub de cotações e caso tenha ficado submeto
                if (listaTemp.Count > 0)
                    listaResultado.Add(GeraBarraDiaria(listaTemp));

                //retornando o resultado
                return listaResultado;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo retornas as cotações organizadas em periodicidade mensal, de acordo com a semana brasileira
        /// </summary>
        /// <param name="listaTemp"></param>
        /// <returns></returns>
        private List<CotacaoDTO> AplicaPeriodicidadeMensal(List<CotacaoDTO> listaCotacao)
        {
            List<CotacaoDTO> listaResultado = new List<CotacaoDTO>();
            List<CotacaoDTO> listaTemp = new List<CotacaoDTO>();
            int numeroMes = -1;
            int numeroMesAnterior = -1;
            CultureInfo myCI = new CultureInfo("pt-br");
            System.Globalization.Calendar myCal = myCI.Calendar;

            try
            {
                //Percorrer a lista de cotações, verificando pelo quebra do número da semana
                foreach (CotacaoDTO objTemp in listaCotacao)
                {
                    numeroMes = myCal.GetMonth(objTemp.Data);
                    if (numeroMes != numeroMesAnterior)
                    {
                        if (listaTemp.Count > 0)
                        {
                            //nesse caso deve gerar a barra e inserir no resultado
                            listaResultado.Add(GeraBarraDiaria(listaTemp));
                            listaTemp.Clear();
                        }
                        listaTemp.Add(objTemp);
                        numeroMesAnterior = numeroMes;
                    }
                    else
                        listaTemp.Add(objTemp);
                }

                //Verifico se ficou dado no hub de cotações e caso tenha ficado submeto
                if (listaTemp.Count > 0)
                    listaResultado.Add(GeraBarraDiaria(listaTemp));

                //retornando o resultado
                return listaResultado;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que recebe a lista de cotações em barras de minuto e converte para a periodicidade
        /// </summary>
        /// <param name="listaCotacao"></param>
        /// <param name="periodicidade"></param>
        /// <returns></returns>
        private List<CotacaoDTO> AplicaPeriodicidadeGenericaPorMinuto(List<CotacaoDTO> listaCotacao, int periodicidade)
        {
            List<BarraComprimida> listaBarrasComprimidas = new List<BarraComprimida>();
            List<CotacaoDTO> listaFinal = new List<CotacaoDTO>();
            string horaNormalizada = "";
            try
            {
                //Percorro a lista com barras de 1 minuto
                foreach (CotacaoDTO obj in listaCotacao)
                {
                    //Efetuo a normalização da barra
                    horaNormalizada = NormalizaPeriodo(obj.Hora, periodicidade);
                    DateTime dataHoraBarraNormalizada = new DateTime(obj.Data.Year,
                        obj.Data.Month, obj.Data.Day,
                        Convert.ToInt16(horaNormalizada.Substring(0, 2)),
                        Convert.ToInt16(horaNormalizada.Substring(2, 2)), 0);

                    //Verifico a data da ultima normalização                    
                    if ((listaBarrasComprimidas.Count > 0) && (dataHoraBarraNormalizada == listaBarrasComprimidas[listaBarrasComprimidas.Count - 1].data))
                        listaBarrasComprimidas[listaBarrasComprimidas.Count - 1].listaCotacoes.Add(obj);
                    else
                    {
                        BarraComprimida novaBarra = new BarraComprimida();
                        novaBarra.data = dataHoraBarraNormalizada;
                        novaBarra.listaCotacoes = new List<CotacaoDTO>();
                        novaBarra.listaCotacoes.Add(obj);

                        //incluindo no hub de datas
                        listaBarrasComprimidas.Add(novaBarra);
                    }
                }

                //Devo percorrer os ubconuntos de barras, gerando as barras finais
                foreach (BarraComprimida obj in listaBarrasComprimidas)
                {
                    listaFinal.Add(GeraBarraIntraday(obj.listaCotacoes, periodicidade));
                }

                return listaFinal;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo deve retornar a barra consolidada de acordo com a lista de cotações
        /// </summary>
        /// <param name="listaCotacao"></param>
        /// <returns></returns>
        private CotacaoDTO GeraBarraIntraday(List<CotacaoDTO> listaCotacao, int periodicidade)
        {
            CotacaoDTO barraResultado = new CotacaoDTO();

            try
            {
                //Setando dados iniciais da barra
                barraResultado.Abertura = listaCotacao[0].Abertura;
                barraResultado.AfterMarket = listaCotacao[0].AfterMarket;                
                barraResultado.Data = listaCotacao[0].Data;
                barraResultado.Maximo = Double.MinValue;
                barraResultado.Minimo = Double.MaxValue;
                barraResultado.Hora = NormalizaPeriodo(listaCotacao[0].Hora, periodicidade);
                barraResultado.Quantidade = 0;
                barraResultado.Ultimo = 0;
                barraResultado.Volume = 0;


                foreach (CotacaoDTO obj in listaCotacao)
                {
                    if (obj.Maximo > barraResultado.Maximo)
                        barraResultado.Maximo = obj.Maximo;
                    if (obj.Minimo < barraResultado.Minimo)
                        barraResultado.Minimo = obj.Minimo;
                    barraResultado.Volume += obj.Volume;
                    barraResultado.Quantidade += obj.Quantidade;
                }

                //setando o fechamento
                barraResultado.Ultimo = listaCotacao[listaCotacao.Count - 1].Ultimo;

                //Retornando a barra gerada
                return barraResultado;
            }
            catch (Exception exc)
            {
                throw exc;
            }


        }

        /// <summary>
        /// Metodo deve retornar a barra consolidada de acordo com a lista de cotações
        /// </summary>
        /// <param name="listaCotacao"></param>
        /// <returns></returns>
        private CotacaoDTO GeraBarraDiaria(List<CotacaoDTO> listaCotacao)
        {
            CotacaoDTO barraResultado = new CotacaoDTO();

            try
            {
                //Setando dados iniciais da barra
                barraResultado.Abertura = listaCotacao[0].Abertura;                
                barraResultado.Data = listaCotacao[0].Data;
                barraResultado.Maximo = Double.MinValue;
                barraResultado.Minimo = Double.MaxValue;
                barraResultado.Quantidade = 0;
                barraResultado.Ultimo = 0;
                barraResultado.Volume = 0;


                foreach (CotacaoDTO obj in listaCotacao)
                {
                    if (obj.Maximo > barraResultado.Maximo)
                        barraResultado.Maximo = obj.Maximo;
                    if (obj.Minimo < barraResultado.Minimo)
                        barraResultado.Minimo = obj.Minimo;
                    barraResultado.Volume += obj.Volume;
                    barraResultado.Quantidade += obj.Quantidade;
                }

                //setando o fechamento
                barraResultado.Ultimo = listaCotacao[listaCotacao.Count - 1].Ultimo;

                //Retornando a barra gerada
                return barraResultado;
            }
            catch (Exception exc)
            {
                throw exc;
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Hora"></param>
        /// <param name="periodicidade"></param>
        private string NormalizaPeriodo(string Hora, int periodicidade)
        {
            try
            {
                //ajustando a hora de acordo com a periodicidade
                if (Convert.ToInt16(Hora.Substring(2, 2)) % periodicidade == 0)
                    return Hora;
                else
                {
                    //nesse caso tenho que percorrer a hora para baixo até encontrar algum divisivel
                    for (int i = Convert.ToInt16(Hora); i >= 0; i--)
                    {
                        string aux = Convert.ToString(i);
                        if (aux.Length < 4)
                            aux = "0" + aux;
                        if (Convert.ToUInt16(aux.Substring(2, 2)) % periodicidade == 0)
                        {
                            return Convert.ToString(aux);
                        }
                    }

                    throw new Exception("Hora passada não é normalizavel");
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

     

        #endregion

        /// <summary>
        /// Metodo que recebe um stream compactado e retorna uma string
        /// </summary>
        /// <param name="Result"></param>
        /// <returns></returns>
        private string Uncompress(Stream Result)
        {
            ZipInputStream zipInputStream = new ZipInputStream(Result);
            ZipEntry zipEntry = zipInputStream.GetNextEntry();
            MemoryStream streamWriter = new MemoryStream();
            while (zipEntry != null)
            {
                String entryFileName = zipEntry.Name;
                // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                // Optionally match entrynames against a selection list here to skip as desired.
                // The unpacked length is available in the zipEntry.Size property.

                byte[] buffer = new byte[4096];		// 4K is optimum

                // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                // of the file, but does not waste memory.
                // The "using" will close the stream even if an exception occurs.
                StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                zipEntry = zipInputStream.GetNextEntry();
            }



            //convertendo MemoryStream to string
            string text = "";
            streamWriter.Position = 0;
            using (StreamReader sr = new StreamReader(streamWriter))
            {
                text = sr.ReadToEnd();
            }

            return text.Replace('\r', ' ').Replace('\n', '|');
        }


        #endregion

        private struct BarraComprimida
        {
            public DateTime data;
            public List<CotacaoDTO> listaCotacoes;
        }
    }

    
}


