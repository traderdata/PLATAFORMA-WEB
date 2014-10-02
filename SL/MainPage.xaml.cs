using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.Silverlight;
using C1.Silverlight.Extended;
using ModulusFE;
using ModulusFE.SL;
using System.Windows.Media.Imaging;
using Traderdata.Client.TerminalWEB.Dialogs;
using Traderdata.Client.TerminalWEB.Dialogs.Portfolio;
using Traderdata.Client.TerminalWEB.DAO;
using Traderdata.Client.TerminalWEB.DTO;
using System.Windows.Threading;
using System.Windows.Browser;
using System.Threading;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
//using Traderdata.Client.TerminalWEB.Dialogs.Scanner;
using Traderdata.Client.TerminalWEB.Util;
using Traderdata.Client.TerminalWEB.Dialogs.Backtest;
//using Traderdata.Client.TerminalWEB.Dialogs.Book;


namespace Traderdata.Client.TerminalWEB
{
    [ScriptableType]
    public partial class MainPage : UserControl
    {
        #region Private

        /// <summary>
        /// Timer que sera usado para fazer o carregamento incial
        /// </summary>
        private DispatcherTimer timerCarregamentoInicial = new DispatcherTimer();

        /// <summary>
        /// variavel de controle de bovespa RT/delay
        /// </summary>
        private bool BMFbovespaRTDelayProcessed = false;

        /// <summary>
        /// variavel de controle de bmf
        /// </summary>
        private bool bmfRTDelayProcessed = false;

        /// <summary>
        /// variavel de controle de carregamento de portfolio
        /// </summary>
        private bool portfolioListProcessed = false;

        /// <summary>
        /// Timer que vai chamar a tela de login
        /// </summary>
        private DispatcherTimer timerLogin = new DispatcherTimer();

        /// <summary>
        /// Lista de forms abertos
        /// </summary>
        private List<C1Window> listaForms = new List<C1Window>();

        /// <summary>
        /// Form selecionado
        /// </summary>
        private C1Window formSelecionado = null;

        /// <summary>
        /// Timer que vai pressionar ou desmarcar os botoes e controlar aparição dos forms
        /// </summary>
        private DispatcherTimer timerPressButtons = new DispatcherTimer();

        /// <summary>
        /// Varialve de acesso ao modulo de marketdata
        /// </summary>
        private MarketDataDAO marketDataDAO = new MarketDataDAO();

        /// <summary>
        /// Variavel de acesso aos webservices
        /// </summary>
        private TerminalWebSVC.TerminalWebClient terminalWebClient =
            new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());

        #endregion

        #region Construtor

        /// <summary>
        /// Contrutor padrão
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
                        
            //assinando eventos de marketdata
            marketDataDAO.SetCacheIndicesCompleted += new MarketDataDAO.SetCacheIndicesHandler(marketDataDAO_SetCacheIndicesCompleted);
            marketDataDAO.SetCacheAtivosPorIndiceCompleted += new MarketDataDAO.SetCacheAtivosPorIndiceHandler(marketDataDAO_SetCacheAtivosPorIndiceCompleted);
            marketDataDAO.SetCotacaoDiariaCacheCompleted += new MarketDataDAO.CotacaoDiarioCacheHandler(marketDataDAO_SetCotacaoDiariaCacheCompleted);
            marketDataDAO.SetCacheAtivosBovespaCompleted += new MarketDataDAO.SetCacheAtivosBovespaHandler(marketDataDAO_SetCacheAtivosBovespaCompleted);
            marketDataDAO.GetAtivosBovespaQueDevemSerCacheadosCompleted += new MarketDataDAO.GetAtivosBovespaQueDevemSerCacheadosHandler(marketDataDAO_GetAtivosBovespaQueDevemSerCacheadosCompleted);
            marketDataDAO.SetCacheSegmentosCompleted += new MarketDataDAO.SetCacheSegmentosHandler(marketDataDAO_SetCacheSegmentosCompleted);
            marketDataDAO.SetCacheAtivosBMFTodosCompleted += new MarketDataDAO.SetCacheAtivosBMFTodosHandler(marketDataDAO_SetCacheAtivosBMFTodosCompleted);
            marketDataDAO.SetCotacaoIntradayCacheCompleted += new MarketDataDAO.SetCotacaoIntradayCacheHandler(marketDataDAO_SetCotacaoIntradayCacheCompleted);

            //assinando eventos de template            
            terminalWebClient.GetTemplatesPorUserIdCompleted += terminalWebClient_GetTemplatesPorUserIdCompleted;
            terminalWebClient.SalvaTemplateCompleted += terminalWebClient_SalvaTemplateCompleted;
            terminalWebClient.ExcluiTemplateCompleted += terminalWebClient_ExcluiTemplateCompleted;

            //assinando eventois de login
            terminalWebClient.LoginUserDistribuidorIntegradoCompleted += new EventHandler<TerminalWebSVC.LoginUserDistribuidorIntegradoCompletedEventArgs>(terminalWebClient_LoginUserDistribuidorIntegradoCompleted);

            //assinando eventos de worskapce
            terminalWebClient.GetWorkspaceDefaultCompleted += terminalWebClient_GetWorkspaceDefaultCompleted;
            terminalWebClient.GetWorkspaceDefaultPorDistribuidorCompleted += terminalWebClient_GetWorkspaceDefaultPorDistribuidorCompleted;
            terminalWebClient.SaveWorkspaceCompleted += terminalWebClient_SaveWorkspaceCompleted;

            
            //assinando eventos de backtesting
            terminalWebClient.RetornaBackTestsCompleted += terminalWebClient_RetornaBackTestsCompleted;
            terminalWebClient.ExcluiBackTestCompleted += terminalWebClient_ExcluiBackTestCompleted;
                       
            //assinando eventos de portfolio
            terminalWebClient.RetornaPortfoliosCompleted += terminalWebClient_RetornaPortfoliosCompleted;

            //assinando eventos de grafico
            terminalWebClient.RetornaGraficoPorAtivoPeriodicidadeCompleted += terminalWebClient_RetornaGraficoPorAtivoPeriodicidadeCompleted;
                       
            
            //já logou portanto devo carregar os caches
            SetCacheBasico();

            //assinando evento de timer para chamar login
            if (!StaticData.FacebookIntegrationLogin)
                timerLogin.Interval = new TimeSpan(0,0,StaticData.TempoDemo);
            else
                timerLogin.Interval = new TimeSpan(0, 0, 1);

            timerLogin.Tick += timerLogin_Tick;
            timerLogin.Start();

            //carregando a listagem de indicadores e suas propriedades
            IndicadorDAO.SetTodosIndicadoresInfo();

            //populando o menu de indicadores
            foreach (IndicatorInfoDTO obj in StaticData.GetListaIndicadores())
            {
                IndicatorInfoDTO indicadorLocal = new IndicatorInfoDTO();
                indicadorLocal = obj;
                C1MenuItem menuItem = new C1MenuItem();
                menuItem.Header = indicadorLocal.NomePortugues;
                menuItem.Tag = indicadorLocal;
                menuItem.HeaderBackground = new SolidColorBrush(Colors.White);
                menuItem.Click += menuItemIndicadores_Click;
                mnuIndicadores.Items.Add(menuItem);
            }

            #region Workspace

            if (!StaticData.FacebookIntegrationLogin)
            {
                //abrindo o workspace default do distribuidor 
                //terminalWebClient.GetWorkspaceDefaultPorDistribuidorAsync(StaticData.DistribuidorId);
            }

            timerCarregamentoInicial.Interval = new TimeSpan(0, 0, 1);
            timerCarregamentoInicial.Tick += timerCarregamentoInicial_Tick;
            timerCarregamentoInicial.Start();
            
            #endregion

            #region Menu

            //desabiliatndo o m enu que sera habilitado somente apos o login
            c1Menu1.IsEnabled = false;

            #endregion
                        
            #region Realtime

            //eventos de Realtime BMF&BVSP
            RealTimeDAO.OnConnectErrorTick += RealTimeDAO_OnConnectErrorBVSP;
            RealTimeDAO.OnConnectSuccessTick += RealTimeDAO_OnConnectSuccessBVSP;

            #endregion

            #region Setando as TAGs laterais
            //setando as tags dos comandos laterais
            tbarSeta.Tag = StaticData.TipoFerramenta.Nenhum;
            tbarZoomIn.Tag = StaticData.TipoFerramenta.Nenhum;
            tbarZoomOut.Tag = StaticData.TipoFerramenta.Nenhum;
            tbarCross.Tag = StaticData.TipoFerramenta.Nenhum;
            tbarRetaTendencia.Tag = StaticData.TipoFerramenta.RetaTendencia;
            tbarLinhaHorizontal.Tag = StaticData.TipoFerramenta.RetaHorizontal;
            tbarLinhaVertical.Tag = StaticData.TipoFerramenta.RetaVertical;
            tbarElipse.Tag = StaticData.TipoFerramenta.Elipse;
            tbarRetangulo.Tag = StaticData.TipoFerramenta.Retangulo;
            tbarTexto.Tag = StaticData.TipoFerramenta.Texto;
            tbarValorY.Tag = StaticData.TipoFerramenta.ValorY;
            tbarSuporte.Tag = StaticData.TipoFerramenta.RetaSuporte;
            tbarResistencia.Tag = StaticData.TipoFerramenta.RetaResistencia;
            tbarCompra.Tag = StaticData.TipoFerramenta.Compra;
            tbarVenda.Tag = StaticData.TipoFerramenta.Vende;
            tbarSinal.Tag = StaticData.TipoFerramenta.Signal;
            tbarFiboArcs.Tag = StaticData.TipoFerramenta.FiboArcs;
            tbarFiboFan.Tag = StaticData.TipoFerramenta.FiboFan;
            tbarFiboRetracement.Tag = StaticData.TipoFerramenta.FiboRetracement;
            tbarFiboTimezone.Tag = StaticData.TipoFerramenta.FiboTimezone;
            tbarGannFan.Tag = StaticData.TipoFerramenta.GannFan;
            tbarErrorChannel.Tag = StaticData.TipoFerramenta.ErrorChannel;
            tbarTironeLevels.Tag = StaticData.TipoFerramenta.TironeLevels;
            tbarSpeedLine.Tag = StaticData.TipoFerramenta.SpeedLine;
            tbarRaffRegression.Tag = StaticData.TipoFerramenta.RaffRegression;
            #endregion

        }


        #endregion        

        #region Load
        /// <summary>
        /// Metodo executado apos terminar de carregar o form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            //setando as caracteristicas do form
            timerPressButtons.Interval = new TimeSpan(0, 0, 0, 0, 200);
            timerPressButtons.Tick += new EventHandler(timerPressButtons_Tick);
            timerPressButtons.Start();


            //setando o formulario como busy
            busyIndicator.BusyContent = "Autorizando...";
            busyIndicator.IsBusy = true;
        }
        #endregion

        #region Login

        /// <summary>
        /// Evento disparado para se chamar a tela de login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerLogin_Tick(object sender, EventArgs e)
        {
            timerLogin.Stop();

            if (!StaticData.SingleSignOn)
            {
                if (!StaticData.FacebookIntegrationLogin)
                {
                    //abrindo o form de login
                    Login login = new Login();
                    HtmlPage.RegisterScriptableObject("SL2JS", login);

                    login.Closing += (sender1, e1) =>
                    {
                        if (login.DialogResult != null)
                            if (login.DialogResult.Value == true)
                            {
                                busyIndicator.BusyContent = "Carregando Workspace...";
                                busyIndicator.IsBusy = true;

                                //carregando a lista de portfolios
                                terminalWebClient.RetornaPortfoliosAsync(StaticData.User);

                                //carregamento de segurança
                                CarregaSeguranca();

                                //conectando nos servidores de RT
                                ConnectRTServers();

                            }
                    };
                    login.Show();
                }
                else
                {
                    //abrindo o form de login
                    HtmlPage.RegisterScriptableObject("SL2JS", this);
                    HtmlPage.Window.Invoke("login", null);
                }
            }
            else
            {
                busyIndicator.BusyContent = "Efetuando Login";
                busyIndicator.IsBusy = true;

                //chamando o metodo de login
                terminalWebClient.LoginUserDistribuidorIntegradoAsync(StaticData.LoginIntegradoDistribuidor, StaticData.DistribuidorId);
            }


        }

        /// <summary>
        /// Evento disparado ao terminar o metodo de conexao integrada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_LoginUserDistribuidorIntegradoCompleted(object sender, TerminalWebSVC.LoginUserDistribuidorIntegradoCompletedEventArgs e)
        {
            //Esse bloco será rodado no caso de corretoras e parceiros integrados
            StaticData.User = e.Result;
            busyIndicator.BusyContent = "Carregando Workspace...";
            busyIndicator.IsBusy = true;

            //carregando a lista de portfolios
            portfolioListProcessed = true;

            //carregamento de segurança
            CarregaSeguranca();

            //conectando nos servidores de RT
            ConnectRTServers();
        }


        /// <summary>
        /// Metodo que vai bloquear algumas opções que o cliente não tem permissão
        /// </summary>
        private void CarregaSeguranca()
        {
            if ((StaticData.User.HasSnapshotBMFDiario) || (StaticData.User.HasSnapshotBovespaDiario))
            {
                tbarDiario.IsEnabled = true;
                tbarSemanal.IsEnabled = true;
                tbarMensal.IsEnabled = true;
            }
            else
            {
                tbarDiario.IsEnabled = false;
                tbarSemanal.IsEnabled = false;
                tbarMensal.IsEnabled = false;
            }

            if ((StaticData.User.HasSnapshotBMFIntraday) || (StaticData.User.HasSnapshotBovespaIntraday))
            {
                tbar10Minutos.IsEnabled = true;
                tbar120Minutos.IsEnabled = true;
                tbar15Minutos.IsEnabled = true;
                tbar1Minuto.IsEnabled = true;
                tbar2Minutos.IsEnabled = true;
                tbar30Minutos.IsEnabled = true;
                tbar3Minutos.IsEnabled = true;
                tbar5Minutos.IsEnabled = true;
                tbar60Minutos.IsEnabled = true;                
            }
            else
            {
                tbar10Minutos.IsEnabled = false;
                tbar120Minutos.IsEnabled = false;
                tbar15Minutos.IsEnabled = false;
                tbar1Minuto.IsEnabled = false;
                tbar2Minutos.IsEnabled = false;
                tbar30Minutos.IsEnabled = false;
                tbar3Minutos.IsEnabled = false;
                tbar5Minutos.IsEnabled = false;
                tbar60Minutos.IsEnabled = false;                

            }

             
        }

        /// <summary>
        /// Metodo que faz o carregamento basico do cliente
        /// metodo deve ser logado somente após o usuario ter se logado
        /// </summary>
        private void CarregamentoBasicoCliente()
        {
            //abrindo o workspace do cliente
            terminalWebClient.GetWorkspaceDefaultAsync(StaticData.User);
                        
            //carregando os templates do usuario
            terminalWebClient.GetTemplatesPorUserIdAsync(StaticData.User.Id);

            if (StaticData.Backtest)
            {
                //carregando os backtestings
                terminalWebClient.RetornaBackTestsAsync(StaticData.User);
            }
        }

        /// <summary>
        /// Metodo que conecta nos servidores de dados continuos
        /// </summary>
        private void ConnectRTServers()
        {
            //Connect RT
            RealTimeDAO.ConnectBMFBVSP();
        }

        /// <summary>
        /// Timer que carrega as coisas basicas do cliente, ele existre para que possamos controlar o termino do processamento dos servidores RT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerCarregamentoInicial_Tick(object sender, EventArgs e)
        {
            //checando se ja processou a conexao nos servidores de RT
            if (BMFbovespaRTDelayProcessed && portfolioListProcessed)
            {
                //Apresenta o portfolio
                if (StaticData.ContainerPlugins)
                {
                    ShowHidePortfolio();
                }
                else
                {
                    mnuFerramentasAuxiliares.IsEnabled = false;
                    mnuFerramentasAuxiliares.IsChecked = false;
                    ShowHidePortfolio();
                }

                //carregando cache
                SetCacheCotacao();

                //carregameto de area de trabalho
                CarregamentoBasicoCliente();

                //parando o timer
                timerCarregamentoInicial.Stop();

            }
        }

        #endregion

        #region Realtime

        /// <summary>
        /// Evento disparado ao obter sucesso na conexao RT/Delay BVSP
        /// </summary>
        void RealTimeDAO_OnConnectSuccessBVSP()
        {
            StaticData.AddLog("Conectado com sucesso no servidor BVSP RT");
            BMFbovespaRTDelayProcessed = true;            
        }

        /// <summary>
        /// Evento disparado ao se obter erro na conexao RT/Delay BVSP
        /// </summary>
        void RealTimeDAO_OnConnectErrorBVSP()
        {
            StaticData.AddLog("Erro ao conectar no servidor BVSP RT");
            BMFbovespaRTDelayProcessed = false;            
        }

        #endregion

        #region Cache

        /// <summary>
        /// Metodo que inicia o processamento de cache
        /// </summary>
        private void SetCacheBasico()
        {
            //Carregando os itens cacheaveis
            PopulaCorretoras();
            marketDataDAO.SetCacheSegmentosAsync();
            marketDataDAO.SetCacheIndicesAsync();
            marketDataDAO.SetCacheAtivosBovespaAsync();
            marketDataDAO.SetCacheAtivosBMFAsync();
        }

        /// <summary>
        /// Metodo que seta o cache das cotações
        /// </summary>
        private void SetCacheCotacao()
        {
            marketDataDAO.GetAtivosBovespaQueDevemSerCacheadosAsync();

            //cacheando ativos BMF
            marketDataDAO.SetCacheCotacaoDiarioAsync("WINFUT");
            marketDataDAO.SetCacheCotacaoDiarioAsync("INDFUT");
            marketDataDAO.SetCacheCotacaoDiarioAsync("DOLFUT");
            marketDataDAO.SetCacheCotacaoDiarioAsync("WDOFUT");

        }

        /// <summary>
        /// Evento disparado após encerrar o cache de um ativo intraday
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_SetCotacaoIntradayCacheCompleted(string Result)
        {
            StaticData.AddLog("Historico Intraday de " + Result + " carregado com sucesso...");
        }

        /// <summary>
        /// Evento disparado apos carregar os ativos BMF
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_SetCacheAtivosBMFTodosCompleted(List<AtivoDTO> Result)
        {
            StaticData.AddLog("Lista de ativos BMF carregada com sucesso");
        }

        /// <summary>
        /// Evento disparado apos carregar os segmentos
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_SetCacheSegmentosCompleted(List<string> Result)
        {
            StaticData.AddLog("Lista de segmentos carregada com sucesso");
        }

        /// <summary>
        /// Evento disparado apos carregar os ativos que devem ser cacheados
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_GetAtivosBovespaQueDevemSerCacheadosCompleted(List<AtivoDTO> Result)
        {
            foreach (AtivoDTO obj in Result)
            {
                if (StaticData.CacheHabilitado)
                {
                    marketDataDAO.SetCacheCotacaoDiarioAsync(obj.Codigo);
                    //Thread.Sleep(3000);
                }
            }
        }

        /// <summary>
        /// Evento disparado apos carregar os ativos Bovespa
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_SetCacheAtivosBovespaCompleted(List<AtivoDTO> Result)
        {
            StaticData.AddLog("Lista de ativos de Bovespa carregada com sucesso");
        }

        /// <summary>
        /// Evento disparado apos carregar a lista de indices
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_SetCacheIndicesCompleted(List<string> Result)
        {
            foreach (string obj in Result)
            {
                marketDataDAO.SetCacheAtivosPorIndiceAsync(obj);
            }
        }


        /// <summary>
        /// Evento disparado apos carregar cotações diarias de um ativo
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_SetCotacaoDiariaCacheCompleted(string Result)
        {
            StaticData.AddLog("Cotações diárias de " + Result + " carregadas com sucesso");
        }

        /// <summary>
        /// Evento disparado apos carregar a lista de ativos de um indice
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="Indice"></param>
        void marketDataDAO_SetCacheAtivosPorIndiceCompleted(List<AtivoDTO> Result, string Indice)
        {
            StaticData.AddLog("Lista de ativos de " + Indice + " carregadas com sucesso");
        }


        #endregion

        #region Execução de comandos
        
        /// <summary>
        /// Metodo que faz o tratamento de todo o keypress do sistema
        /// </summary>
        /// <param name="e"></param>
        public void ExecutaKeyPress(Key e)
        {
            switch (e)
            {
                case Key.Add:
                    break;
                case Key.F1:
                    break;
                case Key.B:
                    //((PageCollection)((C1Window)formSelecionado).Content).AbrirBook();
                    break;
                case Key.S:
                    //((PageCollection)((C1Window)formSelecionado).Content).AbrirScannerIntraday();
                    break;
                case Key.T:
                    //((PageCollection)((C1Window)formSelecionado).Content).AbrirTrades();
                    break;
                case Key.Escape:
                    StaticData.tipoAcao = StaticData.TipoAcao.Seta;
                    StaticData.tipoFerramenta = StaticData.TipoFerramenta.Nenhum;
                    DesmarcaToolbarLateral();
                    ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).DesabilitaCross();
                    tbarSeta.IsChecked = true;
                    break;
                case Key.Delete:
                    ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).DeleteObjetosSelecionados();
                    break;
                
                
            }
        }

        #endregion

        #region Auxiliares

        /// <summary>
        /// Metodo que transforma de lowercase para upper case
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        bool MakeUpperCase(TextBox txt, KeyEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.None || (e.Key < Key.A) || (e.Key > Key.Z))  //do not handle ModifierKeys (work for shift key)
            {
                return false;
            }
            else
            {
                string n = new string(new char[] { (char)e.PlatformKeyCode });
                int nSelStart = txt.SelectionStart;

                txt.Text = txt.Text.Remove(nSelStart, txt.SelectionLength); //remove character from the start to end selection
                txt.Text = txt.Text.Insert(nSelStart, n); //insert value n
                txt.Select(nSelStart + 1, 0); //for cursortext

                return true; //stop to write in txt2
            }

        }

        #endregion

        #region General windows

        /// <summary>
        /// MEtodo que é executado quando se fechga uma janela
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void win_Closed(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja salvar o gráfico?", "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                ((PageCollection)((C1Window)sender).Content).SalvarGrafico();
            }

            listaForms.Remove((C1Window)sender);
            C1MenuItem menuASerRemovido = new C1MenuItem();
            foreach (Object obj in mnuGraficos.Items)
            {
                if (obj.ToString().Contains("C1MenuItem"))
                    if (((C1MenuItem)obj).Tag == ((C1Window)sender))
                    {
                        menuASerRemovido = (C1MenuItem)obj;
                        break;
                    }
            }

            mnuGraficos.Items.Remove(menuASerRemovido);
        }

        #endregion

        #region Toolbar Lateral Commands
        /// <summary>
        /// Evento disparado ao se clicar no botão scrolltoolbar Down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrollBarDownClick(object sender, RoutedEventArgs e)
        {
            scrollToolBar.ScrollToVerticalOffset(scrollToolBar.VerticalOffset + 15);
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão scrolltoolbar Up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrollBarUpClick(object sender, RoutedEventArgs e)
        {
            scrollToolBar.ScrollToVerticalOffset(scrollToolBar.VerticalOffset - 15);
        }

        /// <summary>
        /// Evento que é acionado a cada click na toolbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarLateral_Click(object sender, RoutedEventArgs e)
        {
            //desmarcando todos
            DesmarcaToolbarLateral();

            ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)sender).IsChecked = true;
            StaticData.tipoFerramenta = (StaticData.TipoFerramenta)((C1.Silverlight.Toolbar.C1ToolbarToggleButton)sender).Tag;
            StaticData.tipoAcao = StaticData.TipoAcao.Ferramenta;
        }

        /// <summary>
        /// Metodo que desmarca a toolbar lateral
        /// </summary>
        private void DesmarcaToolbarLateral()
        {
            //desmarcando todos os botoes
            foreach (Object obj in c1Toolbar.Items)
            {
                if (obj.ToString().Contains("C1ToolbarToggleButton"))
                    ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)obj).IsChecked = false;
            }
        }

        /// <summary>
        /// Evento executado ao se clicar no botão de zoom out/Resset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarResetZoom_Click(object sender, RoutedEventArgs e)
        {
            //desmarcando todos
            DesmarcaToolbarLateral();

            ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).ResetZoom();

            formSelecionado.Focus();

            //marcando a seta
            tbarSeta.IsChecked = true;
            StaticData.tipoAcao = StaticData.TipoAcao.Seta;
        }

        /// <summary>
        /// Evento disparado ao se clicar sobre a seta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSeta_Click(object sender, RoutedEventArgs e)
        {
            DesmarcaToolbarLateral();
            tbarSeta.IsChecked = true;
            StaticData.tipoFerramenta = StaticData.TipoFerramenta.Nenhum;
            StaticData.tipoAcao = StaticData.TipoAcao.Seta;
        }

        /// <summary>
        /// Evento disparado ao se clicar em Cross
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarCross_Click(object sender, RoutedEventArgs e)
        {
            DesmarcaToolbarLateral();
            tbarCross.IsChecked = true;
            StaticData.tipoFerramenta = StaticData.TipoFerramenta.Nenhum;
            StaticData.tipoAcao = StaticData.TipoAcao.CROSS;
        }

        /// <summary>
        /// Evento disparado ao se clicar em Zoom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarZoomIn_Click(object sender, RoutedEventArgs e)
        {
            DesmarcaToolbarLateral();
            tbarZoomIn.IsChecked = true;
            StaticData.tipoFerramenta = StaticData.TipoFerramenta.Nenhum;
            StaticData.tipoAcao = StaticData.TipoAcao.Zoom;
        }

        #endregion

        #region Toolbar Superior & Menu Superior

        #region Trades

        /// <summary>
        /// Evento executado ao se solicitar para abrir Trades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuTimesTrades_Click_1(object sender, SourcedEventArgs e)
        {
            ((PageCollection)((C1Window)formSelecionado).Content).AbrirTrades();
        }


        #endregion

        #region Salvar Grafico

        /// <summary>
        /// Evento do clique no botao salvar grafico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSalvarGrafico_Click_1(object sender, RoutedEventArgs e)
        {
            ((PageCollection)formSelecionado.Content).SalvarGrafico();
        }

        /// <summary>
        /// Evento do clique no botao Salvar TODOS Graficos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSalvarTodosGrafico_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (C1Window obj in canvasPrincipal.Children)
            {
                if (obj.Content.GetType().ToString().Contains("PageCollection"))
                {
                    ((PageCollection)obj.Content).SalvarGrafico();
                }
            }

        }

        #endregion

        #region Abrir Grafico

        /// <summary>
        /// Evenbto disparado ao se selecionar a opção abrir grafico no menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuAbrirGrafico_Click(object sender, SourcedEventArgs e)
        {
            AbrirGraficoExistente();
        }

        /// <summary>
        /// Evento disparado ao clicar sobre o obtao Abrir na toolbar superior
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarAbrirGrafico_Click(object sender, RoutedEventArgs e)
        {
            AbrirGraficoExistente();
        }

        /// <summary>
        /// Metodo que vai apresentar a tela de abertura de gráficos
        /// </summary>
        private void AbrirGraficoExistente()
        {
            BuscaAtivoDialog buscaAtivo = new BuscaAtivoDialog();
            buscaAtivo.Title = "Abrir Grafico Existente";
            buscaAtivo.Closing += (sender1, e1) =>
            {
                if (buscaAtivo.DialogResult.Value == true)
                {
                    foreach (AtivoDTO item in buscaAtivo._flexGridAtivos.SelectedItems)
                    {
                        busyIndicator.BusyContent = "Abrindo gráfico...";
                        busyIndicator.IsBusy = true;

                        List<object> args = new List<object>();
                        args.Add(item.Codigo);
                        args.Add(Convert.ToInt32(((ComboBoxItem)buscaAtivo.cmbPeriodicidade.SelectedItem).Tag));
                        terminalWebClient.RetornaGraficoPorAtivoPeriodicidadeAsync(item.Codigo,
                            Convert.ToInt32(((ComboBoxItem)buscaAtivo.cmbPeriodicidade.SelectedItem).Tag),
                            StaticData.User.Id, args);                        
                        //NovoGrafico(item.Codigo, null, GeneralUtil.GetPeriodicidadeFromInt(Convert.ToInt32(((ComboBoxItem)buscaAtivo.cmbPeriodicidade.SelectedItem).Tag)));
                    }

                }
            };
            buscaAtivo.Show();
        }

        #endregion

        #region Portfolio

        /// <summary>
        /// Evento disparado ao se terminar de carregar a lista de portfolios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_RetornaPortfoliosCompleted(object sender, TerminalWebSVC.RetornaPortfoliosCompletedEventArgs e)
        {
            portfolioListProcessed = true;
            StaticData.Portfolios = e.Result;
        }

        #endregion

        #region Compartilhamento

        /// <summary>
        /// Evento responsavel por fazer a publicação de determinada analise
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarPublicarTraderdata_Click_1(object sender, RoutedEventArgs e)
        {

        }


        /// <summary>
        /// Evento disparado quando se clica no item de menu para abrir um grafico que foi salvo na zona de compartilhamento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuAbrirGrafico_Click_1(object sender, SourcedEventArgs e)
        {

        }

        #endregion

        #region Backtest

        /// <summary>
        /// Evento disparado apos excluir um backtest
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_ExcluiBackTestCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            StaticData.AddLog("Estratégia excluida com sucesso");
            terminalWebClient.RetornaBackTestsAsync(StaticData.User);
            busyIndicator.IsBusy = false;
        }

        /// <summary>
        /// Evento de retorno ao metodo de retrieve dos backtestes existentes...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_RetornaBackTestsCompleted(object sender, TerminalWebSVC.RetornaBackTestsCompletedEventArgs e)
        {
            //mnuExcluirEstrategia.Items.Clear();
            //mnuVisualizarResultadosEstrategia.Items.Clear();

            foreach (TerminalWebSVC.BacktestDTO obj in e.Result)
            {
                C1MenuItem item = new C1MenuItem();
                item.Tag = obj;
                item.Header = obj.Nome;
                item.Click += excluirEstrategiaClick;
                //mnuExcluirEstrategia.Items.Add(item);

                C1MenuItem item2 = new C1MenuItem();
                item2.Tag = obj;
                item2.Header = obj.Nome;
                item2.Click += visualizarResultadoEstrategiaClick;
                //mnuVisualizarResultadosEstrategia.Items.Add(item2);
            }
        }

        /// <summary>
        /// Evento disparado ao se clicar sobre o item paar excluir uma estrategia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void excluirEstrategiaClick(object sender, SourcedEventArgs e)
        {
            busyIndicator.BusyContent = "Excluindo estrategia...";
            busyIndicator.IsBusy = true;
            terminalWebClient.ExcluiBackTestAsync((TerminalWebSVC.BacktestDTO)((C1MenuItem)sender).Tag);
        }

        /// <summary>
        /// Evento disparado ao se clicar sobre o item para visualizar o resultado de uma estrategia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void visualizarResultadoEstrategiaClick(object sender, SourcedEventArgs e)
        {
            C1Window window = new C1Window();
            VisualizaResultadoBacktest visualizar = new VisualizaResultadoBacktest((TerminalWebSVC.BacktestDTO)((C1MenuItem)sender).Tag);
            window.Content = visualizar;
            window.ShowMinimizeButton = false;
            window.ShowMaximizeButton = false;
            window.ShowCloseButton = true;
            window.Width = 900;
            window.Height = 400;
            window.Header = "Resultado de Backtest - " + ((TerminalWebSVC.BacktestDTO)((C1MenuItem)sender).Tag).Nome;
            window.Canvas = canvasPrincipal;
            window.Show();
        }


        /// <summary>
        /// Evento disparado ao se criar uma nova estrategia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuNovaEstrategia_Click_1(object sender, SourcedEventArgs e)
        {
            AdicionarEditarTesteUI configuraBacktest = new AdicionarEditarTesteUI();
            configuraBacktest.Closing += (sender1, e1) =>
            {
                if ((configuraBacktest.DialogResult.HasValue) && (configuraBacktest.DialogResult.Value == true))
                {
                    //alterando mensagem de log
                    StaticData.AddLog("Backtest salvo com sucesso");

                    //populando os nomes dos scanners no menu
                    terminalWebClient.RetornaBackTestsAsync(StaticData.User);
                }
            };
            configuraBacktest.Show();
        }

        private void mnuExcluirEstrategia_Click_1(object sender, SourcedEventArgs e)
        {

        }

        private void mnuVisualizarResultadosEstrategia_Click_1(object sender, SourcedEventArgs e)
        {

        }

        #endregion

        #region Facebook

        /// <summary>
        /// Clicando nesse item o gráfico selecionado será publicado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuPublicarFacebook_Click_1(object sender, SourcedEventArgs e)
        {
            ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).PublishFacebook();
        }

        private void InviteFacebookFriends_Click_1(object sender, SourcedEventArgs e)
        {
            List<object> listaParametros = new List<object>();
            HtmlPage.Window.Invoke("sendRequestToRecipients", listaParametros.ToArray());
        }

        private void LikeFacebook_Click_1(object sender, SourcedEventArgs e)
        {

        }

        private void RecommendBuy_Click_1(object sender, SourcedEventArgs e)
        {
            ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).RecommendBuyingFacebook();
        }

        /// <summary>
        /// Clicando nesse link voce fará uma recomendação de Venda do ativo aberto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecommendSell_Click_1(object sender, SourcedEventArgs e)
        {
            ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).RecommendSellingFacebook();
        }

        #endregion

        #region Workspace

        /// <summary>
        /// Evento disparado apos salvar o workspace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_SaveWorkspaceCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Workspace salvo com sucesso.");
            StaticData.AddLog("Workspace salvo com sucesso");
        }

        /// <summary>
        /// Metodo que retorna o workspace default de um distribuidor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetWorkspaceDefaultPorDistribuidorCompleted(object sender, TerminalWebSVC.GetWorkspaceDefaultPorDistribuidorCompletedEventArgs e)
        {
            StaticData.Workspace = e.Result;

            //limpando a area de travalho
            canvasPrincipal.Children.Clear();
            listaForms.Clear();

            //carregando forms
            foreach (TerminalWebSVC.GraficoDTO grafico in e.Result.Graficos)
            {
                NovoGrafico(grafico);
            }

            //arrumando como tile
            ArrumarJanelasTile();

            //Quando encerrar desfaz o busy
            busyIndicator.IsBusy = false;
        }


        /// <summary>
        /// Evento disparado ao carregar o workspace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetWorkspaceDefaultCompleted(object sender, TerminalWebSVC.GetWorkspaceDefaultCompletedEventArgs e)
        {
            //setando o workspace
            StaticData.Workspace = e.Result;

            //limpando os gráficos
            canvasPrincipal.Children.Clear();
            listaForms.Clear();
            for (int i = 4; i < mnuGraficos.Items.Count;)
            {
                mnuGraficos.Items.RemoveAt(4);                
            }

            //checando se o cliente passou um gráfico para ser aberto
            if (StaticData.SymbolSolicitadonoDistribuidor != "")
            {
                //TerminalWebSVC.GraficoDTO graficoIntegrado = new TerminalWebSVC.GraficoDTO();
                //graficoIntegrado.Ativo = StaticData.SymbolSolicitadonoDistribuidor;
                //graficoIntegrado.Height = 400;
                //graficoIntegrado.Layouts = new List<TerminalWebSVC.LayoutDTO>();
                //graficoIntegrado.Layouts.Add(Util.GeneralUtil.LayoutFake());
                //graficoIntegrado.Periodicidade = 1440;
                //graficoIntegrado.UsuarioId = StaticData.User.Id;
                //e.Result.Graficos.Add(graficoIntegrado);

                NovoGraficoAtalho(StaticData.SymbolSolicitadonoDistribuidor);

            }



            //carregando os gráfico
            foreach (TerminalWebSVC.GraficoDTO grafico in e.Result.Graficos)
            {
                NovoGrafico(grafico);
            }

            //apresentando o menu
            c1Menu1.IsEnabled = true;

            //arrumando como tile
            ArrumarJanelasTile();

            //setando o busy indicator como false
            busyIndicator.IsBusy = false;
        }

        /// <summary>
        /// Evento disparado ao se clicar no menu Salvar Workspace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSalvarWorkspace_Click_1(object sender, SourcedEventArgs e)
        {
            TerminalWebSVC.WorkspaceDTO workspace = new TerminalWebSVC.WorkspaceDTO();
            workspace.Nome = "DEFAULT";
            workspace.UsuarioId = StaticData.User.Id;
            workspace.Graficos = GetGraficos();

            //chamando o metodo que vai salvar
            terminalWebClient.SaveWorkspaceAsync(workspace);
        }

        /// <summary>
        /// Evento disparado ao se clicar no menu Salvar Workspace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalvarWorkspace_Click_1(object sender, RoutedEventArgs e)
        {
            TerminalWebSVC.WorkspaceDTO workspace = new TerminalWebSVC.WorkspaceDTO();
            workspace.Nome = "DEFAULT";
            workspace.UsuarioId = StaticData.User.Id;
            workspace.Graficos = GetGraficos();

            //chamando o metodo que vai salvar
            terminalWebClient.SaveWorkspaceAsync(workspace);
        }

        public void SalvarWorkspaceFromOutside()
        {
            TerminalWebSVC.WorkspaceDTO workspace = new TerminalWebSVC.WorkspaceDTO();
            workspace.Nome = "DEFAULT";
            workspace.UsuarioId = StaticData.User.Id;
            workspace.Graficos = GetGraficos();

            //chamando o metodo que vai salvar
            terminalWebClient.SaveWorkspaceAsync(workspace);
        }

        /// <summary>
        /// Metodo que retorna uma lista de graficos
        /// </summary>
        /// <returns></returns>
        public List<TerminalWebSVC.GraficoDTO> GetGraficos()
        {
            List<TerminalWebSVC.GraficoDTO> listaGraficos = new List<TerminalWebSVC.GraficoDTO>();

            foreach (C1Window obj in canvasPrincipal.Children)
            {
                if (obj.Content.GetType().ToString().Contains("PageCollection"))
                {
                    TerminalWebSVC.GraficoDTO grafico = new TerminalWebSVC.GraficoDTO();
                    grafico.Ativo = ((PageCollection)obj.Content).Ativo;
                    grafico.Height = (int)obj.ActualHeight;
                    grafico.Layouts = ((PageCollection)obj.Content).GetLayouts();
                    grafico.Left = (int)obj.Left;
                    grafico.Periodicidade = GeneralUtil.GetIntPeriodicidade(((PageCollection)obj.Content).Periodicidade);
                    grafico.Top = (int)obj.Top;
                    grafico.Width = (int)obj.ActualWidth;
                    grafico.UsuarioId = StaticData.User.Id;

                    listaGraficos.Add(grafico);
                }
            }

            //retorna a lista de graficos
            return listaGraficos;
        }

        #endregion

        #region Janelas

        /// <summary>
        /// Evento disparado ao se clicar sobre uma janela
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void win_GotFocus(object sender, RoutedEventArgs e)
        {
            formSelecionado = (C1Window)sender;

            //selecionando o item de menu correto no menu Graficos
            DesmarcarMenuItemsGraficos();
            foreach (object obj in mnuGraficos.Items)
            {
                if (obj.GetType().ToString().Contains("C1MenuItem"))
                    if (((C1MenuItem)obj).Tag == (C1Window)sender)
                        ((C1MenuItem)obj).IsChecked = true;
            }

            //((Grafico)((C1TabItem)((C1TabControl)((Grid)((PageCollection)formSelecionado.Content).Content).).SelectedItem).Content).Focus();

            //selecionando o tipo de barra correto
            //PressTipoBarraButton();
        }

        /// <summary>
        /// Metodo que faz a criação de uma nova janela
        /// </summary>
        private void NovoGrafico(string ativo, TerminalWebSVC.TemplateDTO template, Periodicidade periodicidade)
        {
            int count = 0;
            foreach (C1Window obj in listaForms)
            {
                if (obj.Tag != null)
                    if (obj.Tag.ToString() == ativo)
                        count++;
            }

            //Adicionando o conteudo            
            PageCollection paginas = new PageCollection(ativo, template, periodicidade);
            C1Window win = new C1Window();
            win.MinHeight = 200;
            win.Width = 800;
            win.Height = 400;
            win.Content = paginas;
            win.ShowMaximizeButton = true;
            win.ShowMinimizeButton = true;
            win.Canvas = canvasPrincipal;
            win.Left = listaForms.Count * 20;
            win.Top = listaForms.Count * 20;
            if (count > 0)
                win.Tag = ativo + count.ToString();
            else
                win.Tag = ativo;

            win.GotFocus += new RoutedEventHandler(win_GotFocus);
            win.Closed += new EventHandler(win_Closed);


            win.Show();
            win.Focus();
            win.BringToFront();
            listaForms.Add(win);

            //Desmarcando os itens de menu
            DesmarcarMenuItemsGraficos();
            C1MenuItem menuItem = new C1MenuItem();
            if (template != null)
                menuItem.Header = ativo + " -  " + GeneralUtil.GetPeriodicidadeFromIntToString(template.Periodicidade);
            else
                menuItem.Header = ativo + " -  " + GeneralUtil.GetPeriodicidadeFromIntToString(1440);

            menuItem.Click += new EventHandler<SourcedEventArgs>(menuItem_Click);
            menuItem.IsChecked = true;
            menuItem.Tag = win;
            menuItem.IsCheckable = true;
            mnuGraficos.Items.Add(menuItem);

            foreach (C1Window obj in listaForms)
                obj.BringToFront();

            win.BringToFront();

        }

        //private void NovoGrafico(string ativo, TerminalWebSVC.TemplateDTO template, Periodicidade periodicidade)
        //{            
        //    int count = 0;
        //    foreach (C1Window obj in listaForms)
        //    {
        //        if (obj.Tag != null)
        //            if (obj.Tag.ToString() == ativo)
        //                count++;
        //    }


        //    //PageCollection grafico00 = new PageCollection(ativo, template, periodicidade);
        //    //grafico00.BorderThickness = new Thickness(0);
        //    //grafico00.SetValue(Grid.RowProperty, 0);
        //    //grafico00.SetValue(Grid.ColumnProperty, 0);

        //    //PageCollection grafico01 = new PageCollection(ativo, template, Periodicidade.Diario);
        //    //grafico01.BorderThickness = new Thickness(0);
        //    //grafico01.SetValue(Grid.RowProperty, 0);
        //    //grafico01.SetValue(Grid.ColumnProperty, 1);

        //    //PageCollection grafico10 = new PageCollection(ativo, template, Periodicidade.QuinzeMinutos);
        //    //grafico10.BorderThickness = new Thickness(0);
        //    //grafico10.SetValue(Grid.RowProperty, 1);
        //    //grafico10.SetValue(Grid.ColumnProperty, 0);

        //    //PageCollection grafico11 = new PageCollection(ativo, template, Periodicidade.CincoMinutos);
        //    //grafico11.BorderThickness = new Thickness(0);
        //    //grafico11.SetValue(Grid.RowProperty, 1);
        //    //grafico11.SetValue(Grid.ColumnProperty, 1);

        //    //PageCollectionContainer container = new PageCollectionContainer();
            
        //    //container.LayoutRoot.Children.Add(grafico00);
        //    //container.LayoutRoot.Children.Add(grafico01);
        //    //container.LayoutRoot.Children.Add(grafico10);
        //    //container.LayoutRoot.Children.Add(grafico11);
        //    //container.row1.Height = new GridLength(0);


        //    //Criando uma aba no tabcontrol
        //    C1TabItem tabItem = new C1TabItem();
        //    //StackPanel stack = new StackPanel();
        //    //stack.Orientation = Orientation.Horizontal;
        //    TextBlock txtBlock = new TextBlock();
        //    txtBlock.Text = ativo + " ";
        //    Button btnDestacar = new Button();
        //    btnDestacar.Content = "D";
        //    btnDestacar.Width = 10;
        //    btnDestacar.Height = 10;

        //    //stack.Children.Add(txtBlock);
        //    //stack.Children.Add(btnDestacar);
                 
        //    tabItem.Header = ativo;
        //    tabPrincipal.Items.Add(tabItem);
        //    tabItem.Margin = new Thickness(0);
        //    tabItem.Padding = new Thickness(0);
        //    tabItem.BorderBrush = new SolidColorBrush(Colors.Gray);
        //    tabItem.BorderThickness = new Thickness(1);

        //    //Adicionando o conteudo            
        //    PageCollection paginas = new PageCollection(ativo, template, periodicidade);
        //    tabItem.Content = paginas;
            
            
        //    //C1Window win = new C1Window();
        //    //win.MinHeight = 200;
        //    //win.Width = 800;
        //    //win.Height = 400;
        //    //win.Content = paginas;
        //    //win.ShowMaximizeButton = false;
        //    //win.ShowMinimizeButton = false;
        //    //win.Canvas = canvasPrincipal;
        //    //win.Left = listaForms.Count * 20;
        //    //win.Top = listaForms.Count * 20;
        //    //if (count > 0)
        //    //    win.Tag = ativo + count.ToString();
        //    //else
        //    //   win.Tag = ativo;

        //    //win.GotFocus += new RoutedEventHandler(win_GotFocus);
        //    //win.Closed += new EventHandler(win_Closed);


        //    //win.Show();
        //    //win.Focus();
        //    //win.BringToFront();
        //    //listaForms.Add(win);

        //    //Desmarcando os itens de menu
        //    //DesmarcarMenuItemsGraficos();
        //    //C1MenuItem menuItem = new C1MenuItem();
        //    //if (template != null)
        //    //    menuItem.Header = ativo + " -  " + GeneralUtil.GetPeriodicidadeFromIntToString(template.Periodicidade);
        //    //else
        //    //    menuItem.Header = ativo + " -  " + GeneralUtil.GetPeriodicidadeFromIntToString(1440);

        //    //menuItem.Click += new EventHandler<SourcedEventArgs>(menuItem_Click);
        //    //menuItem.IsChecked = true;
        //    //menuItem.Tag = win;
        //    //menuItem.IsCheckable = true;
        //    //mnuGraficos.Items.Add(menuItem);

        //    ////setando o ultimo form como form selecionado
        //    formSelecionado = tabItem;
        //    tabPrincipal.SelectedItem = tabItem;
        //    //foreach (C1Window obj in listaForms)
        //    //    obj.BringToFront();

        //    //win.BringToFront();

        //}

        /// <summary>
        /// Metodo que faz a criação de uma nova janela
        /// </summary>
        private void NovoGrafico(TerminalWebSVC.GraficoDTO grafico)
        {
            int count = 0;
            foreach (C1Window obj in listaForms)
            {
                if (obj.Tag != null)
                    if (obj.Tag.ToString() == grafico.Ativo)
                        count++;
            }

            PageCollection paginas = new PageCollection(grafico);
            C1Window win = new C1Window();

            win.MinHeight = 200;
            win.Width = 600;
            win.Height = 400;
            win.Content = paginas;
            win.ShowMaximizeButton = true;
            win.ShowMinimizeButton = true;
            win.Canvas = canvasPrincipal;
            win.Left = listaForms.Count * 20;
            win.Top = listaForms.Count * 20;
            if (count > 0)
                win.Tag = grafico.Ativo + count.ToString();
            else
                win.Tag = grafico.Ativo;

            win.GotFocus += new RoutedEventHandler(win_GotFocus);
            win.Closed += new EventHandler(win_Closed);

            win.Show();
            win.Focus();
            listaForms.Add(win);

            //Desmarcando os itens de menu
            DesmarcarMenuItemsGraficos();
            C1MenuItem menuItem = new C1MenuItem();            
            menuItem.Header = grafico.Ativo + " -  " + GeneralUtil.GetPeriodicidadeFromIntToString(grafico.Periodicidade);
            menuItem.Click += new EventHandler<SourcedEventArgs>(menuItem_Click);
            menuItem.IsChecked = true;
            menuItem.Tag = win;
            menuItem.IsCheckable = true;
            mnuGraficos.Items.Add(menuItem);

            //setando o ultimo form como form selecionado
            formSelecionado = win;

            foreach (C1Window obj in listaForms)
                obj.BringToFront();

        }

        /// <summary>
        /// Botão que vai abrri um novo gráfico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarNovoGrafico_Click(object sender, RoutedEventArgs e)
        {
            AbrirNovoGraficoDialog();
        }

        #endregion

        #region Window
        private void mnuArrumarHorizontalmente_Click(object sender, SourcedEventArgs e)
        {
            ArrumarJanelasHorizontalmente();
        }

        private void mnhuArrumarVerticalmente_Click(object sender, SourcedEventArgs e)
        {
            ArrumarJanelasVerticalmente();
        }

        private void mnuTile_Click(object sender, SourcedEventArgs e)
        {
            ArrumarJanelasTile();
        }

        /// <summary>
        /// Botão que vai organizar as janelas horizontalmente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarArrumarJanelaHorizontalmente_Click(object sender, RoutedEventArgs e)
        {
            //((PageCollectionContainer)((C1TabItem)tabPrincipal.SelectedItem).Content).row1.Height = new GridLength(0);
            //((PageCollectionContainer)((C1TabItem)tabPrincipal.SelectedItem).Content).row0.Height = new GridLength(50, GridUnitType.Star);
            //((PageCollectionContainer)((C1TabItem)tabPrincipal.SelectedItem).Content).column0.Width = new GridLength(50, GridUnitType.Star);
            //((PageCollectionContainer)((C1TabItem)tabPrincipal.SelectedItem).Content).column1.Width = new GridLength(0);
            ArrumarJanelasHorizontalmente();
        }

        /// <summary>
        /// Metodo que vai organizar as janelas verticalmente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarArrumarJanelasVeticalmente_Click(object sender, RoutedEventArgs e)
        {
            ArrumarJanelasVerticalmente();
            //((PageCollectionContainer)((C1TabItem)tabPrincipal.SelectedItem).Content).row1.Height = new GridLength(0);
            //((PageCollectionContainer)((C1TabItem)tabPrincipal.SelectedItem).Content).row0.Height = new GridLength(1, GridUnitType.Star);
            //((PageCollectionContainer)((C1TabItem)tabPrincipal.SelectedItem).Content).column0.Width = new GridLength(50, GridUnitType.Star);
            //((PageCollectionContainer)((C1TabItem)tabPrincipal.SelectedItem).Content).column1.Width = new GridLength(50, GridUnitType.Star);
        }

        /// <summary>
        /// Botão que vai colocar as janelas em formato Tile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarTile_Click(object sender, RoutedEventArgs e)
        {
            //((PageCollectionContainer)((C1TabItem)tabPrincipal.SelectedItem).Content).row1.Height = new GridLength(50, GridUnitType.Star);
            //((PageCollectionContainer)((C1TabItem)tabPrincipal.SelectedItem).Content).row0.Height = new GridLength(50, GridUnitType.Star);
            //((PageCollectionContainer)((C1TabItem)tabPrincipal.SelectedItem).Content).column0.Width = new GridLength(50, GridUnitType.Star);
            //((PageCollectionContainer)((C1TabItem)tabPrincipal.SelectedItem).Content).column1.Width = new GridLength(50, GridUnitType.Star);
            ArrumarJanelasTile();
        }

        /// <summary>
        /// Metodo que faz a arrumação das janelas horizontalmente
        /// </summary>
        private void ArrumarJanelasHorizontalmente()
        {
            double i = 0;
            double altura = canvasPrincipal.ActualHeight / listaForms.Count;
            foreach (C1Window obj in listaForms)
            {
                obj.Left = 0;
                obj.Height = altura;
                obj.Top = i;
                obj.Width = canvasPrincipal.ActualWidth;
                i += altura;
            }
        }

        /// <summary>
        /// Metodo que faz a arrumação das janelas verticalmente
        /// </summary>
        private void ArrumarJanelasVerticalmente()
        {
            double i = 0;
            double largura = canvasPrincipal.ActualWidth / listaForms.Count;
            foreach (C1Window obj in listaForms)
            {
                obj.Left = i;
                obj.Height = canvasPrincipal.ActualHeight;
                obj.Top = 0;
                obj.Width = largura;
                i += largura;
            }
        }

        /// <summary>
        /// Metodo que faz a arrumação das janelas em formato Tile
        /// </summary>
        private void ArrumarJanelasTile()
        {
            int linhas = 0;
            int colunas = 0;
            if (listaForms.Count == 1)
                colunas = 1;
            else if (listaForms.Count == 2)
                colunas = 2;
            else if (listaForms.Count == 3)
                colunas = 3;
            else colunas = 4;

            if (listaForms.Count % colunas == 0)
                linhas = listaForms.Count / colunas;
            else
                linhas = (listaForms.Count / colunas) + 1;

            double altura = canvasPrincipal.ActualHeight / linhas;
            double largura = canvasPrincipal.ActualWidth / colunas;

            int countX = 0;
            int count = 1;
            int countY = 0;

            foreach (C1Window obj in listaForms)
            {
                obj.Height = altura;
                if (count < listaForms.Count)
                    obj.Width = largura;
                else
                {
                    obj.Width = canvasPrincipal.ActualWidth - (countX * largura);
                }
                obj.Top = altura * countY;
                obj.Left = largura * countX;
                countX++;
                if (countX == colunas)
                {
                    countX = 0;
                    countY++;
                }
                count++;

            }
        }
        #endregion

        #region Skins

        /// <summary>
        /// Evento disparado ao se clicar em Ok na configuração de cor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationOkClick(object sender, EventArgs e)
        {
            TerminalWebSVC.LayoutDTO layoutTemp = new TerminalWebSVC.LayoutDTO();
            C1TabItem tabItem = (C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem;
            layoutTemp = ((Grafico)tabItem.Content).Layout;

            //motando objeto layout
            layoutTemp.CorEscala = configuration.corEscala.SelectedColor.ToString();
            layoutTemp.CorFundo = configuration.corFundo.SelectedColor.ToString();
            layoutTemp.CorGrid = configuration.corGrid.SelectedColor.ToString();
            layoutTemp.CorCandleAlta = configuration.corCandleAlta.SelectedColor.ToString();
            layoutTemp.CorBordaCandleAlta = configuration.corBordaCandleAlta.SelectedColor.ToString();
            layoutTemp.CorBordaCandleBaixa = configuration.corBordaCandleBaixa.SelectedColor.ToString();
            layoutTemp.CorCandleBaixa = configuration.corCandleBaixa.SelectedColor.ToString();
            layoutTemp.CorVolume = configuration.corVolume.SelectedColor.ToString();
            layoutTemp.UsarCoresAltaBaixaVolume = configuration.chkUsarCoresDiferentesVolume.IsChecked;

            //aplicando layout
            ((Grafico)tabItem.Content).AplicaLayout(layoutTemp, false, false, false);

            //mudando o foco
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparado ao abrir o form de configuracao
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationOpen(object sender, EventArgs e)
        {
            C1TabItem tabItem = (C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem;

            //RECUPERANDO as configurações de cor do grafico selecionado
            TerminalWebSVC.LayoutDTO layoutTemp = new TerminalWebSVC.LayoutDTO();
            layoutTemp = ((Grafico)tabItem.Content).GetLayoutDTOFromStockchart();

            configuration.corFundo.SelectedColor = GeneralUtil.GetColorFromHexa(layoutTemp.CorFundo).Color;
            configuration.corBordaCandleAlta.SelectedColor = GeneralUtil.GetColorFromHexa(layoutTemp.CorBordaCandleAlta).Color;
            configuration.corBordaCandleBaixa.SelectedColor = GeneralUtil.GetColorFromHexa(layoutTemp.CorBordaCandleBaixa).Color;
            configuration.corCandleAlta.SelectedColor = GeneralUtil.GetColorFromHexa(layoutTemp.CorCandleAlta).Color;
            configuration.corCandleBaixa.SelectedColor = GeneralUtil.GetColorFromHexa(layoutTemp.CorCandleBaixa).Color;
            configuration.corEscala.SelectedColor = GeneralUtil.GetColorFromHexa(layoutTemp.CorEscala).Color;
            configuration.corGrid.SelectedColor = GeneralUtil.GetColorFromHexa(layoutTemp.CorGrid).Color;
            configuration.corVolume.SelectedColor = GeneralUtil.GetColorFromHexa(layoutTemp.CorVolume).Color;

        }

        /// <summary>
        /// Evento disparado ao se escolher o skin preto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSkinPreto_Click(object sender, RoutedEventArgs e)
        {
            foreach (C1TabItem tabItem in ((PageCollection)formSelecionado.Content).c1TabControl1.Items)
            {
                if ((string)tabItem.Header != "+")
                    ((Grafico)tabItem.Content).SetSkinPreto();
            }
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparado ao se escolher o skin branco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSkinBranco_Click(object sender, RoutedEventArgs e)
        {
            foreach (C1TabItem tabItem in ((PageCollection)formSelecionado.Content).c1TabControl1.Items)
            {
                if ((string)tabItem.Header != "+")
                    ((Grafico)tabItem.Content).SetSkinBranco();
            }
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento que executa a troca do skin para preto e branco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSkinPretoEBranco_Click(object sender, RoutedEventArgs e)
        {
            foreach (C1TabItem tabItem in ((PageCollection)formSelecionado.Content).c1TabControl1.Items)
            {
                if ((string)tabItem.Header != "+")
                    ((Grafico)tabItem.Content).SetSkinPretoBranco();
            }
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparado ao se trocar para skin azul e branco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSkinAzulEBranco_Click(object sender, RoutedEventArgs e)
        {
            foreach (C1TabItem tabItem in ((PageCollection)formSelecionado.Content).c1TabControl1.Items)
            {
                if ((string)tabItem.Header != "+")
                    ((Grafico)tabItem.Content).SetSkinAzulBranco();
            }
            formSelecionado.Focus();
        }

        #endregion

        #region Tipo de Barra

        /// <summary>
        /// Evento disparado ao se clicar sobre o botao de tipo de barra Candle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarCandle_Click(object sender, RoutedEventArgs e)
        {
            if ((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem != null)
            {
                ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content)
                    .SetTipoBarra(SeriesTypeEnum.stCandleChart);

            }
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparado ao se clicar sobre o botao de tipo de barra barra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void tbarBarra_Click(object sender, RoutedEventArgs e)
        {
            if ((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem != null)
            {
                ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content)
                    .SetTipoBarra(SeriesTypeEnum.stStockBarChart);
            }


            formSelecionado.Focus();

        }

        /// <summary>
        /// Evento disparado ao se clicar sobre o botao de tipo de barra linha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void tbarLinha_Click(object sender, RoutedEventArgs e)
        {
            if ((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem != null)
            {
                    ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content)
                        .SetTipoBarra(SeriesTypeEnum.stLineChart);

            }


            formSelecionado.Focus();
        }

        /// <summary>
        /// Metodo que vai pressionar o botao correto do tipo de barra
        /// </summary>
        private void PressTipoBarraButton()
        {
            if ((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem != null)
            {
                //checando qual o tipo de barra do gráfico
                switch (((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).GetTipoBarra())
                {
                    case SeriesTypeEnum.stCandleChart:
                        tbarCandle.IsChecked = true;
                        tbarLinha.IsChecked = false;
                        tbarBarra.IsChecked = false;
                        break;
                    case SeriesTypeEnum.stLineChart:
                        tbarCandle.IsChecked = false;
                        tbarLinha.IsChecked = true;
                        tbarBarra.IsChecked = false;
                        break;
                    case SeriesTypeEnum.stStockBarChart:
                        tbarCandle.IsChecked = false;
                        tbarLinha.IsChecked = false;
                        tbarBarra.IsChecked = true;
                        break;
                }
            }
        }

        #endregion

        #region Escala

        /// <summary>
        /// Metodo que vai pressionar o botao correto do tipo de escala
        /// </summary>
        private void PressTipoEscalaButton()
        {
            if ((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem != null)
            {
                //checando qual o tipo de barra do gráfico
                switch (((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).GetTipoEscala())
                {
                    case ScalingTypeEnum.Linear:
                        tbarEscalaNormal.IsChecked = true;
                        tbarEscalaSemilog.IsChecked = false;
                        break;
                    case ScalingTypeEnum.Semilog:
                        tbarEscalaNormal.IsChecked = false;
                        tbarEscalaSemilog.IsChecked = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Evento disparado ao se pressionar a escala normal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarEscalaNormal_Click(object sender, RoutedEventArgs e)
        {
            if ((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem != null)
                //checando qual o tipo de barra do gráfico
                ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content)
                    .SetTipoEscala(ScalingTypeEnum.Linear);
            formSelecionado.Focus();

        }

        /// <summary>
        /// Eventos disparado ao se pressionar a escala semi-log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarEscalaSemilog_Click(object sender, RoutedEventArgs e)
        {
            if ((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem != null)
                //checando qual o tipo de barra do gráfico
                ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content)
                    .SetTipoEscala(ScalingTypeEnum.Semilog);

            formSelecionado.Focus();

        }


        #endregion

        #region Refresh

        /// <summary>
        /// Evento disparado ao se clicar  no botão Refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarRefreshCotacoes_Click(object sender, RoutedEventArgs e)
        {
            foreach (C1TabItem tabItem in ((PageCollection)formSelecionado.Content).c1TabControl1.Items)
            {
                if ((string)tabItem.Header != "+")
                    ((Grafico)tabItem.Content).Refresh();
            }
            formSelecionado.Focus();
        }

        /// <summary>
        /// Metodo que vai pressionar o botao de periodicidade
        /// </summary>
        private void PressBotaoPeriodicidade()
        {
            if ((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem != null)
            {
                //checando qual o tipo de barra do gráfico
                switch (((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).GetPeriodicidade())
                {
                    case Periodicidade.UmMinuto:
                        tbar1Minuto.IsChecked = true;
                        tbar2Minutos.IsChecked = false;
                        tbar3Minutos.IsChecked = false;
                        tbar5Minutos.IsChecked = false;
                        tbar10Minutos.IsChecked = false;
                        tbar15Minutos.IsChecked = false;
                        tbar30Minutos.IsChecked = false;
                        tbar60Minutos.IsChecked = false;
                        tbar120Minutos.IsChecked = false;
                        tbarDiario.IsChecked = false;
                        tbarSemanal.IsChecked = false;
                        tbarMensal.IsChecked = false;
                        break;
                    case Periodicidade.DoisMinutos:
                        tbar1Minuto.IsChecked = false;
                        tbar2Minutos.IsChecked = true;
                        tbar3Minutos.IsChecked = false;
                        tbar5Minutos.IsChecked = false;
                        tbar10Minutos.IsChecked = false;
                        tbar15Minutos.IsChecked = false;
                        tbar30Minutos.IsChecked = false;
                        tbar60Minutos.IsChecked = false;
                        tbar120Minutos.IsChecked = false;
                        tbarDiario.IsChecked = false;
                        tbarSemanal.IsChecked = false;
                        tbarMensal.IsChecked = false;
                        break;
                    case Periodicidade.TresMinutos:
                        tbar1Minuto.IsChecked = false;
                        tbar2Minutos.IsChecked = false;
                        tbar3Minutos.IsChecked = true;
                        tbar5Minutos.IsChecked = false;
                        tbar10Minutos.IsChecked = false;
                        tbar15Minutos.IsChecked = false;
                        tbar30Minutos.IsChecked = false;
                        tbar60Minutos.IsChecked = false;
                        tbar120Minutos.IsChecked = false;
                        tbarDiario.IsChecked = false;
                        tbarSemanal.IsChecked = false;
                        tbarMensal.IsChecked = false;
                        break;
                    case Periodicidade.CincoMinutos:
                        tbar1Minuto.IsChecked = false;
                        tbar2Minutos.IsChecked = false;
                        tbar3Minutos.IsChecked = false;
                        tbar5Minutos.IsChecked = true;
                        tbar10Minutos.IsChecked = false;
                        tbar15Minutos.IsChecked = false;
                        tbar30Minutos.IsChecked = false;
                        tbar60Minutos.IsChecked = false;
                        tbar120Minutos.IsChecked = false;
                        tbarDiario.IsChecked = false;
                        tbarSemanal.IsChecked = false;
                        tbarMensal.IsChecked = false;
                        break;
                    case Periodicidade.DezMinutos:
                        tbar1Minuto.IsChecked = false;
                        tbar2Minutos.IsChecked = false;
                        tbar3Minutos.IsChecked = false;
                        tbar5Minutos.IsChecked = false;
                        tbar10Minutos.IsChecked = true;
                        tbar15Minutos.IsChecked = false;
                        tbar30Minutos.IsChecked = false;
                        tbar60Minutos.IsChecked = false;
                        tbar120Minutos.IsChecked = false;
                        tbarDiario.IsChecked = false;
                        tbarSemanal.IsChecked = false;
                        tbarMensal.IsChecked = false;
                        break;
                    case Periodicidade.QuinzeMinutos:
                        tbar1Minuto.IsChecked = false;
                        tbar2Minutos.IsChecked = false;
                        tbar3Minutos.IsChecked = false;
                        tbar5Minutos.IsChecked = false;
                        tbar10Minutos.IsChecked = false;
                        tbar15Minutos.IsChecked = true;
                        tbar30Minutos.IsChecked = false;
                        tbar60Minutos.IsChecked = false;
                        tbar120Minutos.IsChecked = false;
                        tbarDiario.IsChecked = false;
                        tbarSemanal.IsChecked = false;
                        tbarMensal.IsChecked = false;
                        break;
                    case Periodicidade.TrintaMinutos:
                        tbar1Minuto.IsChecked = false;
                        tbar2Minutos.IsChecked = false;
                        tbar3Minutos.IsChecked = false;
                        tbar5Minutos.IsChecked = false;
                        tbar10Minutos.IsChecked = false;
                        tbar15Minutos.IsChecked = false;
                        tbar30Minutos.IsChecked = true;
                        tbar60Minutos.IsChecked = false;
                        tbar120Minutos.IsChecked = false;
                        tbarDiario.IsChecked = false;
                        tbarSemanal.IsChecked = false;
                        tbarMensal.IsChecked = false;
                        break;
                    case Periodicidade.SessentaMinutos:
                        tbar1Minuto.IsChecked = false;
                        tbar2Minutos.IsChecked = false;
                        tbar3Minutos.IsChecked = false;
                        tbar5Minutos.IsChecked = false;
                        tbar10Minutos.IsChecked = false;
                        tbar15Minutos.IsChecked = false;
                        tbar30Minutos.IsChecked = false;
                        tbar60Minutos.IsChecked = true;
                        tbar120Minutos.IsChecked = false;
                        tbarDiario.IsChecked = false;
                        tbarSemanal.IsChecked = false;
                        tbarMensal.IsChecked = false;
                        break;
                    case Periodicidade.CentoeVinteMinutos:
                        tbar1Minuto.IsChecked = false;
                        tbar2Minutos.IsChecked = false;
                        tbar3Minutos.IsChecked = false;
                        tbar5Minutos.IsChecked = false;
                        tbar10Minutos.IsChecked = false;
                        tbar15Minutos.IsChecked = false;
                        tbar30Minutos.IsChecked = false;
                        tbar60Minutos.IsChecked = false;
                        tbar120Minutos.IsChecked = true;
                        tbarDiario.IsChecked = false;
                        tbarSemanal.IsChecked = false;
                        tbarMensal.IsChecked = false;
                        break;
                    case Periodicidade.Diario:
                        tbar1Minuto.IsChecked = false;
                        tbar2Minutos.IsChecked = false;
                        tbar3Minutos.IsChecked = false;
                        tbar5Minutos.IsChecked = false;
                        tbar10Minutos.IsChecked = false;
                        tbar15Minutos.IsChecked = false;
                        tbar30Minutos.IsChecked = false;
                        tbar60Minutos.IsChecked = false;
                        tbar120Minutos.IsChecked = false;
                        tbarDiario.IsChecked = true;
                        tbarSemanal.IsChecked = false;
                        tbarMensal.IsChecked = false;
                        break;
                    case Periodicidade.Semanal:
                        tbar1Minuto.IsChecked = false;
                        tbar2Minutos.IsChecked = false;
                        tbar3Minutos.IsChecked = false;
                        tbar5Minutos.IsChecked = false;
                        tbar10Minutos.IsChecked = false;
                        tbar15Minutos.IsChecked = false;
                        tbar30Minutos.IsChecked = false;
                        tbar60Minutos.IsChecked = false;
                        tbar120Minutos.IsChecked = false;
                        tbarDiario.IsChecked = false;
                        tbarSemanal.IsChecked = true;
                        tbarMensal.IsChecked = false;
                        break;
                    case Periodicidade.Mensal:
                        tbar1Minuto.IsChecked = false;
                        tbar2Minutos.IsChecked = false;
                        tbar3Minutos.IsChecked = false;
                        tbar5Minutos.IsChecked = false;
                        tbar10Minutos.IsChecked = false;
                        tbar15Minutos.IsChecked = false;
                        tbar30Minutos.IsChecked = false;
                        tbar60Minutos.IsChecked = false;
                        tbar120Minutos.IsChecked = false;
                        tbarDiario.IsChecked = false;
                        tbarSemanal.IsChecked = false;
                        tbarMensal.IsChecked = true;
                        break;

                }
            }
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 1 minuto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar1Minuto_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)formSelecionado.Content).ChangePeriodicity(Periodicidade.UmMinuto);
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 2 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar2Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)formSelecionado.Content).ChangePeriodicity(Periodicidade.DoisMinutos);
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 3 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar3Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)formSelecionado.Content).ChangePeriodicity(Periodicidade.TresMinutos);
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 5 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar5Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)formSelecionado.Content).ChangePeriodicity(Periodicidade.CincoMinutos);
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 10 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar10Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)formSelecionado.Content).ChangePeriodicity(Periodicidade.DezMinutos);
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 15 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar15Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)formSelecionado.Content).ChangePeriodicity(Periodicidade.QuinzeMinutos);
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 30 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar30Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)formSelecionado.Content).ChangePeriodicity(Periodicidade.TrintaMinutos);
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 60 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar60Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)formSelecionado.Content).ChangePeriodicity(Periodicidade.SessentaMinutos);
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 120 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar120Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)formSelecionado.Content).ChangePeriodicity(Periodicidade.CentoeVinteMinutos);
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão Diario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarDiario_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)formSelecionado.Content).ChangePeriodicity(Periodicidade.Diario);
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão Semanal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSemanal_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)formSelecionado.Content).ChangePeriodicity(Periodicidade.Semanal);
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão Mensal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarMensal_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)formSelecionado.Content).ChangePeriodicity(Periodicidade.Mensal);
            formSelecionado.Focus();
        }
        #endregion

        #region Cache

        /// <summary>
        /// Evento disparado ao se clicar em Limpar Cache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuLimparCache_Click(object sender, SourcedEventArgs e)
        {
            StaticData.cacheCotacaoDiario = new Dictionary<string, List<CotacaoDTO>>();
            StaticData.cacheCotacaoIntraday = new Dictionary<string, List<CotacaoDTO>>();
            StaticData.cacheAtivosPorIndice = new Dictionary<string, List<AtivoDTO>>();
            StaticData.cacheAtivosPorSegmento = new Dictionary<string, List<AtivoDTO>>();
            StaticData.cacheIndices = new List<string>();
            StaticData.cacheAtivosBMFTodos = new List<AtivoDTO>();
            StaticData.cacheAtivosBMFPrincpalCheio = new List<AtivoDTO>();
            StaticData.cacheAtivosBMFMiniContrato = new List<AtivoDTO>();
            StaticData.cacheAtivosBovespaTodos = new List<AtivoDTO>();
            StaticData.cacheAtivosBovespaVista = new List<AtivoDTO>();
            StaticData.cacheAtivosBovespaOpcao = new List<AtivoDTO>();
            StaticData.cacheAtivosBovespaTermo = new List<AtivoDTO>();
            StaticData.cacheSegmentos = new List<string>();
        }

        private void mnuCacheHabilitado_Click(object sender, SourcedEventArgs e)
        {
            //StaticData.CacheHabilitado = mnuCacheHabilitado.IsChecked;
        }

        #endregion

        #region Salvar Imagem Grafico

        /// <summary>
        /// Evento disparado para salvar somente o gráfico selecionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSalvarGrafico_Click(object sender, SourcedEventArgs e)
        {
            ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content)._stockChartX.SaveToFile(StockChartX.ImageExportType.Png);
            formSelecionado.Focus();
        }

        /// <summary>
        /// Evento disparao ao se tentar salvar todos os arquivos de uma so vez
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSalvarTodosGraficos_Click_1(object sender, SourcedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*";
            sfd.FilterIndex = 1;

            if (sfd.ShowDialog() == false)
                return;

            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(3); //0-9, 9 being the highest level of compression

            //percorrendo todos os gráficos para salvar um a um
            foreach (C1Window obj in canvasPrincipal.Children)
            {
                C1TabItem tabItem = (C1TabItem)((PageCollection)obj.Content).c1TabControl1.SelectedItem;

                if ((string)tabItem.Header != "+")
                {
                    //determinando o nome do arquivo
                    string fileName = ((Grafico)tabItem.Content)._stockChartX.Symbol + "-" + obj.Tag.ToString() + ".png";
                    ZipEntry newEntry = new ZipEntry(fileName);
                    newEntry.DateTime = DateTime.Now;
                    zipStream.PutNextEntry(newEntry);

                    byte[] byteFile = ((Grafico)tabItem.Content)._stockChartX.GetBytes(StockChartX.ImageExportType.Png);

                    MemoryStream memStreamIn = new MemoryStream();
                    memStreamIn.Write(byteFile, 0, byteFile.Length);
                    memStreamIn.Position = 0;
                    StreamUtils.Copy(memStreamIn, zipStream, new byte[4096]);

                    zipStream.CloseEntry();
                    zipStream.IsStreamOwner = false;
                }

            }

            //deve fechar o zipStream antes
            zipStream.Close();


            using (Stream stream = sfd.OpenFile())
            {
                stream.Write(outputMemStream.ToArray(), 0, outputMemStream.ToArray().Length);
                stream.Close();
            }

        }

        #endregion

        #region Indicadores

        /// <summary>
        /// Eventos disparado ao se clicar no botao de indicadores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarInsertIndicador_Click(object sender, RoutedEventArgs e)
        {
            //pegando qual gráfico está selecionado
            C1TabItem tabItem = (C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem;

            List<string> listaSymbol = new List<string>();
            listaSymbol.Add(((Grafico)tabItem.Content)._stockChartX.Symbol);
            InsertIndicator insertIndicador = new InsertIndicator(true,
                ((Grafico)tabItem.Content).GetAllSeries(),
                ((Grafico)tabItem.Content)._stockChartX.PanelsCollection.ToList<ChartPanel>(),
                ((Grafico)tabItem.Content)._stockChartX.Symbol + ".CLOSE",
                ((Grafico)tabItem.Content)._stockChartX.Symbol,
                listaSymbol,
                ((Grafico)tabItem.Content)._stockChartX.RecordCount);

            insertIndicador.Closing += (sender1, e1) =>
            {
                try
                {
                    if ((insertIndicador.DialogResult != null) && (insertIndicador.DialogResult.Value == true))
                    {
                        ((Grafico)tabItem.Content).InserirIndicador(insertIndicador.ChartPanel,
                            insertIndicador.listaPropriedades,
                            (IndicatorInfoDTO)insertIndicador.listBoxIndicadores.SelectedItem,
                            null);
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            };
            insertIndicador.Show();

            formSelecionado.Focus();

        }

        /// <summary>
        /// Evento disparado quando se seleciona um indicador da combo para inserção rápida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menuItemIndicadores_Click(object sender, SourcedEventArgs e)
        {
            //pegando qual gráfico está selecionado
            ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).InserirIndicador(((IndicatorInfoDTO)((C1MenuItem)e.Source).Tag));
            formSelecionado.Focus();
        }

        #endregion

        #region Cores

        /// <summary>
        /// Evento disparado ao se escolher a cor no colorPicker
        /// </summary>
        /// <param name="c"></param>
        private void objectColorPicker_SelectedColorChanged(object sender, PropertyChangedEventArgs<Color> e)
        {
            ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).SetColorObjetoGeralSelecionado(e.NewValue);
            StaticData.corSelecionada = e.NewValue;
            borderColorPicker.Background = new SolidColorBrush(e.NewValue);
            formSelecionado.Focus();
        }


        /// <summary>
        /// Ao clicar no botao deve ser suficiente para trocar a cor do opbjeto selecionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnColorPicker_Click_1(object sender, RoutedEventArgs e)
        {
            ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).SetColorObjetoGeralSelecionado(StaticData.corSelecionada);
            formSelecionado.Focus();
        }

        #endregion

        #region Grossura do Objeto

        /// <summary>
        /// Evento disparado ao se trocar a grossura de objeto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void strokeThicknessPicker_ChangeSelection(object sender, EventArgs e)
        {
            ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).SetStrokeThicknessObjetoGeralSelecionado(Convert.ToInt32(((Border)sender).Tag));

            StaticData.strokeThickness = Convert.ToInt32(((Border)sender).Tag);
            tbarStrokeSthickness.IsDropDownOpen = false;
            formSelecionado.Focus();
        }

        #endregion

        #region Tipo do Objeto

        /// <summary>
        /// Evento disparado ao se trocar a grossura de objeto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void strokeTypePicker_ChangeSelection(object sender, EventArgs e)
        {
            ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).SetStrokeTypeObjetoSelecionado((LinePattern)Convert.ToInt32((((Border)sender).Tag)));
            StaticData.estiloLinhaSelecionado = (LinePattern)Convert.ToInt32((((Border)sender).Tag));
            tbarStrokeType.IsDropDownOpen = false;
            formSelecionado.Focus();
        }

        #endregion

        #region InfoPanel

        /// <summary>
        /// Evento executado ao se clicar sobre o botao de infopanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarPainelInformacoes_Click(object sender, RoutedEventArgs e)
        {
            //mnuPainelInformacao.IsChecked = tbarPainelInformacoes.IsChecked.Value;

            if ((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem != null)
            {
                ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content)
                    .SetInfoPanelVisibility(System.Windows.Visibility.Collapsed);

            }

            //foreach (C1TabItem tabItem in ((PageCollection)formSelecionado.Content).c1TabControl1.Items)
            //{
            //    if ((string)tabItem.Header != "+")
            //        if (tbarPainelInformacoes.IsChecked.Value)
            //            ((Grafico)tabItem.Content).SetInfoPanelVisibility(System.Windows.Visibility.Visible);
            //        else
            //            ((Grafico)tabItem.Content).SetInfoPanelVisibility(System.Windows.Visibility.Collapsed);

            //}
            //formSelecionado.Focus();
        }

        #endregion

        #region AfterMarket

        /// <summary>
        /// Evento disparado ao se clicar  no botão Refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarAfterMarket_Click(object sender, RoutedEventArgs e)
        {
            //foreach (C1TabItem tabItem in ((PageCollection)formSelecionado.Content).c1TabControl1.Items)
            //{
            //    if ((string)tabItem.Header != "+")
            //        if (tbarAfterMarket.IsChecked.Value)
            //            ((Grafico)tabItem.Content).SetAfterMarket(true);
            //        else
            //            ((Grafico)tabItem.Content).SetAfterMarket(false);

            //}
            //formSelecionado.Focus();
        }

        /// <summary>
        /// Metodo que seta ou desseta o botao de aftermarket
        /// </summary>
        private void PressAfterMarketButton()
        {
            //if ((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem != null)
            //{
            //    //checando qual o tipo de barra do gráfico
            //    tbarAfterMarket.IsChecked = ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).GetAfterMarket();

            //}
        }

        #endregion

        #region Ativo

        /// <summary>
        /// Evento usado para identificar o press de enter no campo de ativo e para transformar em upperCase
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAtivo_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NovoGraficoAtalho(txtAtivo.Text);
                return;
            }
            e.Handled = MakeUpperCase((TextBox)sender, e);
        }

        /// <summary>
        /// Evento disparado ao se clicar em cima de ativo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAtivo_GotFocus(object sender, RoutedEventArgs e)
        {
            txtAtivo.Text = "";
        }

        #endregion

        #region Templates

        /// <summary>
        /// Metodo invocado quando o cliente fecha o prompt box da C1
        /// </summary>
        /// <param name="message"></param>
        /// <param name="Result"></param>
        private void promptBoxTemplateAction(string message, MessageBoxResult Result)
        {
            if (Result == MessageBoxResult.OK)
            {
                TerminalWebSVC.TemplateDTO template = ((PageCollection)formSelecionado.Content).GetTemplate();

                template.Nome = message;
                terminalWebClient.SalvaTemplateAsync(template);
            }
        }

        /// <summary>
        /// Evento disparado ao se clicar em Salvar Template no menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSalvarTemplate_Click(object sender, SourcedEventArgs e)
        {
            C1PromptBox.Show("Informe o nome do template", "Salvar Template", promptBoxTemplateAction);
        }

        /// <summary>
        /// Evento disparado após se excluir um template 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_ExcluiTemplateCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //resgatando templates
            terminalWebClient.GetTemplatesPorUserIdAsync(StaticData.User.Id);

            //alterando sttausabar
            StaticData.AddLog("Template excluído com sucesso");
        }

        /// <summary>
        /// Evento disparado ao se salvar um template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_SalvaTemplateCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //resgatando templates
            terminalWebClient.GetTemplatesPorUserIdAsync(StaticData.User.Id);

            //logando na barra de status
            StaticData.AddLog("Template salvo com sucesso");
        }

        /// <summary>
        /// Evento retornado ao carregar a lista de templates do usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_GetTemplatesPorUserIdCompleted(object sender, TerminalWebSVC.GetTemplatesPorUserIdCompletedEventArgs e)
        {
            //limpando o menu de novos graficos
            mnuExcluirTemplate.Items.Clear();
            mnuAplicarTemplate.Items.Clear();

            //adicionando as entradas de template                
            foreach (TerminalWEB.TerminalWebSVC.TemplateDTO obj in e.Result)
            {                
                C1MenuItem mnuItemExcluir = new C1MenuItem();
                mnuItemExcluir.Header = obj.Nome;
                mnuItemExcluir.Click += mnuItemExcluir_Click;
                mnuItemExcluir.Tag = obj;
                mnuItemExcluir.IsTabStop = false;
                mnuExcluirTemplate.Items.Add(mnuItemExcluir);

                C1MenuItem mnuItemAplicar = new C1MenuItem();
                mnuItemAplicar.Header = obj.Nome;
                mnuItemAplicar.Click += mnuItemAplicar_Click;
                mnuItemAplicar.Tag = obj;
                mnuItemAplicar.IsTabStop = false;
                mnuAplicarTemplate.Items.Add(mnuItemAplicar);
            }
        }

        /// <summary>
        /// Metodo que aplica um template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mnuItemAplicar_Click(object sender, SourcedEventArgs e)
        {
            if (MessageBox.Show("Deseja aplicar o template " + ((TerminalWebSVC.TemplateDTO)((C1MenuItem)sender).Tag).Nome, "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                //recuperando os layouts
                List<TerminalWebSVC.LayoutDTO> layouts = ((TerminalWebSVC.TemplateDTO)((C1MenuItem)sender).Tag).Layouts;

                //alterando os lyaouts
                foreach (TerminalWebSVC.LayoutDTO obj in layouts)
                {
                    if (obj.Indicadores.Count > 0)
                    {
                        foreach (TerminalWebSVC.IndicadorDTO indicador in obj.Indicadores)
                        {
                            //capturando os parametros
                            string[] parametros = indicador.Parametros.Split(';');

                            //captura o indicador
                            foreach (IndicatorInfoDTO indicadorObj in StaticData.GetListaIndicadores())
                            {
                                if (indicadorObj.TipoStockchart == (IndicatorType)indicador.TipoIndicador.Value)
                                {
                                    for (int j = 0; j < indicadorObj.Propriedades.Count; j++)
                                    {
                                        if (indicadorObj.Propriedades[j].TipoDoCampo == TipoField.Serie)
                                        {
                                            if (parametros[j - 1].Contains("."))
                                                parametros[j-1] = "." + parametros[j-1].Split('.')[1];
                                            


                                        }
                                    }
                                    break;
                                }
                            }

                            indicador.Parametros = "";
                            for(int o = 0; o < parametros.Length; o++)
                            {
                                if (parametros[o].Length > 0)
                                    indicador.Parametros += parametros[o] + ";";
                            }

                        }
                    }
                }

                //aplica o template
                ((PageCollection)formSelecionado.Content).AplicarTemplate(layouts,
                    ((TerminalWebSVC.TemplateDTO)((C1MenuItem)sender).Tag).Periodicidade);
            }
        }

        /// <summary>
        /// Evento disparado ao se clicar no menu para excluir um template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mnuItemExcluir_Click(object sender, SourcedEventArgs e)
        {
            if (MessageBox.Show("Deseja excluir o template " + ((TerminalWebSVC.TemplateDTO)((C1MenuItem)sender).Tag).Nome, "Confirmação", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                terminalWebClient.ExcluiTemplateAsync((TerminalWebSVC.TemplateDTO)((C1MenuItem)sender).Tag);
            }
        }

        /// <summary>
        /// Evento disparao ao se clicar no menu Novo Grafico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuNovoGrafico_Click_1(object sender, SourcedEventArgs e)
        {
            AbrirNovoGraficoDialog();
        }

        /// <summary>
        /// Evento disparado quando o usuario seleciona a opção Padrão
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AbrirNovoGraficoDialog()
        {
            BuscaAtivoDialog buscaAtivo = new BuscaAtivoDialog();
            
            buscaAtivo.Closing += (sender1, e1) =>
            {
                if (buscaAtivo.DialogResult.Value == true)
                {
                    foreach (AtivoDTO item in buscaAtivo._flexGridAtivos.SelectedItems)
                    {
                        //busyIndicator.BusyContent = "Abrindo gráfico...";
                        //busyIndicator.IsBusy = true;
                        
                        //List<object> args = new List<object>();
                        //args.Add(item.Codigo);
                        //args.Add(Convert.ToInt32(((ComboBoxItem)buscaAtivo.cmbPeriodicidade.SelectedItem).Tag));
                        //terminalWebClient.RetornaGraficoPorAtivoPeriodicidadeAsync(item.Codigo,
                        //    Convert.ToInt32(((ComboBoxItem)buscaAtivo.cmbPeriodicidade.SelectedItem).Tag),
                        //    StaticData.User.Id, args);                        
                        NovoGrafico(item.Codigo, null, GeneralUtil.GetPeriodicidadeFromInt(Convert.ToInt32(((ComboBoxItem)buscaAtivo.cmbPeriodicidade.SelectedItem).Tag)));
                    }

                }
            };
            buscaAtivo.Show();

        }

        /// <summary>
        /// Metodo que abre um procura por um gráfico salvo
        /// </summary>
        /// <param name="ativo"></param>
        public void NovoGraficoAtalho(string ativo)
        {
            //busyIndicator.BusyContent = "Abrindo gráfico...";
            //busyIndicator.IsBusy = true;

            //List<object> args = new List<object>();
            //args.Add(ativo);
            //args.Add(GeneralUtil.GetIntPeriodicidade(Periodicidade.Diario));
            //terminalWebClient.RetornaGraficoPorAtivoPeriodicidadeAsync(ativo,
            //    GeneralUtil.GetIntPeriodicidade(Periodicidade.Diario),
            //    StaticData.User.Id, args);  
            NovoGrafico(ativo, null, Periodicidade.Diario);
        }

        /// <summary>
        /// Buscando grafico ja salvo previamente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_RetornaGraficoPorAtivoPeriodicidadeCompleted(object sender, TerminalWebSVC.RetornaGraficoPorAtivoPeriodicidadeCompletedEventArgs e)
        {
            busyIndicator.IsBusy = false;
            if (e.Result == null)
            {
                List<object> args = (List<object>)e.UserState;
                NovoGrafico((string)args[0], null, GeneralUtil.GetPeriodicidadeFromInt(Convert.ToInt32(args[1])));
            }
            else
            {
                NovoGrafico(e.Result[0]);
            }
        }


        #endregion

        #region Configuração Geral

        /// <summary>
        /// Evento disparado ao se abrir a tela d econfiguração geral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationGeralOpen(object sender, EventArgs e)
        {
            C1TabItem tabItem = (C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem;

            //RECUPERANDO as configurações de cor do grafico selecionado
            TerminalWebSVC.LayoutDTO layoutTemp = new TerminalWebSVC.LayoutDTO();
            layoutTemp = ((Grafico)tabItem.Content).GetLayoutDTOFromStockchart();

            configurationGeral.chkGridHorizontal.IsChecked = layoutTemp.GradeHorizontal;
            configurationGeral.chkGridVertical.IsChecked = layoutTemp.GradeVertical;
            configurationGeral.cmbTipoVolume.SelectedItem = 0;
            if (Convert.ToInt32(layoutTemp.PosicaoEscala) == 1)
                configurationGeral.rdbDireita.IsChecked = true;
            else
                configurationGeral.rdbEsquerda.IsChecked = true;
            configurationGeral.txtPrecisao.SetValue(Convert.ToDouble(layoutTemp.PrecisaoEscala));
            configurationGeral.txtEspessuraVolume.SetValue(Convert.ToDouble(layoutTemp.VolumeStrokeThickness));


        }

        /// <summary>
        /// Evento disparado ao se clicar em Ok na tela de confgiuração
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationGeralOkClick(object sender, EventArgs e)
        {
            TerminalWebSVC.LayoutDTO layoutTemp = new TerminalWebSVC.LayoutDTO();
            C1TabItem tabItem = (C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem;
            layoutTemp = ((Grafico)tabItem.Content).Layout;

            //motando objeto layout
            layoutTemp.GradeHorizontal = configurationGeral.chkGridHorizontal.IsChecked;
            layoutTemp.GradeVertical = configurationGeral.chkGridVertical.IsChecked;
            if ((bool)configurationGeral.rdbDireita.IsChecked)
                layoutTemp.PosicaoEscala = 1;
            else
                layoutTemp.PosicaoEscala = 2;
            layoutTemp.PrecisaoEscala = Convert.ToInt32(configurationGeral.txtPrecisao.Value());
            if (configurationGeral.cmbTipoVolume.SelectedValue == "Financeiro")
                layoutTemp.TipoVolume = "F";

            layoutTemp.VolumeStrokeThickness = Convert.ToInt32(configurationGeral.txtEspessuraVolume.Value());

            //aplicando layout
            ((Grafico)tabItem.Content).AplicaLayout(layoutTemp, false, false, false);

            //mudando o foco
            formSelecionado.Focus();
        }


        #endregion

        #region Suporte

        private void mnuSuporteChatClick(object sender, SourcedEventArgs e)
        {
            HtmlPage.PopupWindow(new Uri("http://messenger.providesupport.com/messenger/traderdata.html"), "_blank", new HtmlPopupWindowOptions());
        }

        private void mnuSuporteEmailClick(object sender, SourcedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri("mailto:suporte@traderdata.com.br"));
        }

        #endregion

        #region Historico Cotacao

        /// <summary>
        /// Metodo executado ao se clicar no historico de cotação
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuHistoricoCotacao_Click(object sender, SourcedEventArgs e)
        {
            if (formSelecionado != null)
            {
                string ativo = ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content)._stockChartX.Symbol;

                HistoricoCotacao historicoCotacao = new HistoricoCotacao(ativo, false);
                C1Window win = new C1Window();

                win.Width = 600;
                win.Height = 400;
                win.Content = historicoCotacao;
                win.ShowMaximizeButton = false;
                win.ShowMinimizeButton = false;
                win.Canvas = canvasPrincipal;
                win.Left = listaForms.Count * 20;
                win.Top = listaForms.Count * 20;

                win.BringToFront();
                win.Show();
                listaForms.Add(win);

                foreach (C1Window obj in listaForms)
                {
                    obj.BringToFront();
                }

                //Desmarcando os itens de menu
                DesmarcarMenuItemsGraficos();
                C1MenuItem menuItem = new C1MenuItem();
                menuItem.Header = listaForms.Count.ToString();
                menuItem.Click += new EventHandler<SourcedEventArgs>(menuItem_Click);
                menuItem.IsChecked = true;
                menuItem.Tag = win;
                menuItem.IsCheckable = true;
                mnuGraficos.Items.Add(menuItem);


            }


        }

        #endregion

        #region Loja Virtual

        /// <summary>
        /// Item disparado ao se clicar sobre o item Loja Virtual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuLojaVirtual_Click_1(object sender, SourcedEventArgs e)
        {
            //abrindo o form de loja virtual
            LojaVirtual lojaVirtual = new LojaVirtual();
            lojaVirtual.Show();
        }

        #endregion

        #region Tela Cheia

        /// <summary>
        /// Metodo que coloca a tela em modo de tela cheia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuTelaCheia_Click(object sender, SourcedEventArgs e)
        {
            Application.Current.Host.Content.IsFullScreen = true;
        }

        #endregion

        #endregion

        #region Area Propaganda

        /// <summary>
        /// Clique do botao de link para pagamento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            HtmlPopupWindowOptions options = new HtmlPopupWindowOptions();
            options.Resizeable = true;
            options.Status = true;
            options.Toolbar = true;
            options.Width = 900;
            options.Height = 500;

            HtmlPage.PopupWindow(new Uri("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=LWMRNWP99Z3CQ"), "_blank", options);
        }

        #endregion

        /// <summary>
        /// Metodo que desmarca os itens de menu das janelas
        /// </summary>
        private void DesmarcarMenuItemsGraficos()
        {
            foreach (Object obj in mnuGraficos.Items)
            {
                if (obj.ToString().Contains("C1MenuItem"))
                    ((C1MenuItem)obj).IsChecked = false;
            }
        }

        void menuItem_Click(object sender, SourcedEventArgs e)
        {

            DesmarcarMenuItemsGraficos();

            ((C1MenuItem)(sender)).IsChecked = true;
            ((C1Window)((C1MenuItem)(sender)).Tag).Focus();
            formSelecionado = (C1Window)((C1MenuItem)(sender)).Tag;
        }

        private void mnuPainlAquecimento_Click(object sender, SourcedEventArgs e)
        {
            foreach (ChartPanel obj in ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).
                _stockChartX.PanelsCollection.ToList<ChartPanel>())
            {
                if (obj.IsHeatMap)
                    return;
            }
            ((Grafico)((C1TabItem)((PageCollection)formSelecionado.Content).c1TabControl1.SelectedItem).Content).
                _stockChartX.AddHeatMapPanel();
        }

        

        void timerPressButtons_Tick(object sender, EventArgs e)
        {
            if (formSelecionado != null)
            {
                try
                {
                    //setando o tipo de barra
                    PressTipoBarraButton();

                    //setando o tipo de escala
                    PressTipoEscalaButton();

                    //setando a periodicidade
                    PressBotaoPeriodicidade();
                }
                catch { }
            }
        }


        
        
        

        private void LayoutRoot_KeyUp_1(object sender, KeyEventArgs e)
        {
            ExecutaKeyPress(e.Key);
        }

        private void mnuStatus_Click_1(object sender, SourcedEventArgs e)
        {
            Status status = new Status();
            status.Show();
        }

          

        private void mnuBookOfertas_Click_1(object sender, SourcedEventArgs e)
        {
            ((PageCollection)((C1Window)formSelecionado).Content).AbrirBook();
        }

        /// <summary>
        /// Metodo auxiliar que vai popular as corretoras
        /// </summary>
        private void PopulaCorretoras()
        {
            StaticData.CorretoraBovespa.Add(3, "XP INVESTIMENTOS");
            StaticData.CorretoraBovespa.Add(114, "ITAÚ");
            StaticData.CorretoraBovespa.Add(45, "CREDIT SUISSE BRASIL");
            StaticData.CorretoraBovespa.Add(10, "SPINELLI");
            StaticData.CorretoraBovespa.Add(90, "TITULO");
            StaticData.CorretoraBovespa.Add(735, "ICAP DO BRASIL");
            StaticData.CorretoraBovespa.Add(72, "BRADESCO");
            StaticData.CorretoraBovespa.Add(76, "INTERBOLSA DO BRASIL");
            StaticData.CorretoraBovespa.Add(85, "BTG PACTUAL");
            StaticData.CorretoraBovespa.Add(102, "BANIF");
            StaticData.CorretoraBovespa.Add(82, "TOV");
            StaticData.CorretoraBovespa.Add(150, "PROSPER");
            StaticData.CorretoraBovespa.Add(70, "HSBC");
            StaticData.CorretoraBovespa.Add(39, "AGORA");
            StaticData.CorretoraBovespa.Add(14, "CRUZEIRO DO SUL");
            StaticData.CorretoraBovespa.Add(27, "SATANDER");
            StaticData.CorretoraBovespa.Add(239, "INTERFLOAT HZ");
            StaticData.CorretoraBovespa.Add(174, "ELITE");
            StaticData.CorretoraBovespa.Add(8, "LINK");
            StaticData.CorretoraBovespa.Add(5, "ISOLDI");
            StaticData.CorretoraBovespa.Add(75, "TALARICO");
            StaticData.CorretoraBovespa.Add(63, "NOVINVEST");
            StaticData.CorretoraBovespa.Add(106, "MERC. DO BRASIL COR.");
            StaticData.CorretoraBovespa.Add(115, "HENCORP COMMCOR");
            StaticData.CorretoraBovespa.Add(98, "ALPES");
            StaticData.CorretoraBovespa.Add(37, "UMUARAMA");
            StaticData.CorretoraBovespa.Add(16, "J. P. MORGAN");
            StaticData.CorretoraBovespa.Add(110, "SLW");
            StaticData.CorretoraBovespa.Add(131, "FATOR");
            StaticData.CorretoraBovespa.Add(34, "SCHAHIN");
            StaticData.CorretoraBovespa.Add(35, "PETRA PERSONAL TRADER");
            StaticData.CorretoraBovespa.Add(47, "SOLIDEZ");
            StaticData.CorretoraBovespa.Add(147, "ATIVA");
            StaticData.CorretoraBovespa.Add(33, "ESC. LEROSA");
            StaticData.CorretoraBovespa.Add(59, "SAFRA");
            StaticData.CorretoraBovespa.Add(95, "CS HEDGING-GRIFFO");
            StaticData.CorretoraBovespa.Add(192, "GERALDO CORREA");
            StaticData.CorretoraBovespa.Add(129, "PLANNER");
            StaticData.CorretoraBovespa.Add(234, "CODEPE");
            StaticData.CorretoraBovespa.Add(736, "PAX");
            StaticData.CorretoraBovespa.Add(190, "PILLA");
            StaticData.CorretoraBovespa.Add(172, "BANRISUL");
            StaticData.CorretoraBovespa.Add(58, "SOCOPA SC PAULISTA");
            StaticData.CorretoraBovespa.Add(40, "MORGAN STANLEY");
            StaticData.CorretoraBovespa.Add(2, "SOUZA BARROS");
            StaticData.CorretoraBovespa.Add(227, "GRADUAL");
            StaticData.CorretoraBovespa.Add(13, "MERRILL LYNCH");
            StaticData.CorretoraBovespa.Add(23, "CONCORDIA");
            StaticData.CorretoraBovespa.Add(38, "CORVAL");
            StaticData.CorretoraBovespa.Add(86, "WALPIRES");
            StaticData.CorretoraBovespa.Add(177, "SOLIDUS");
            StaticData.CorretoraBovespa.Add(77, "CITIGROUP GMB");
            StaticData.CorretoraBovespa.Add(74, "COINVALORES");
            StaticData.CorretoraBovespa.Add(21, "VOTORANTIM");
            StaticData.CorretoraBovespa.Add(181, "MUNDINVEST");
            StaticData.CorretoraBovespa.Add(57, "BRASCAN");
            StaticData.CorretoraBovespa.Add(238, "GOLDMAN SACHS DO BRASIL");
            StaticData.CorretoraBovespa.Add(9, "DEUTSCHE BANK");
            StaticData.CorretoraBovespa.Add(15, "INDUSVAL");
            StaticData.CorretoraBovespa.Add(189, "PRIME");
            StaticData.CorretoraBovespa.Add(186, "CORRETORA GERAL DE VC");
            StaticData.CorretoraBovespa.Add(237, "OLIVEIRA FRANCO");
            StaticData.CorretoraBovespa.Add(140, "DIFERENCIAL");
            StaticData.CorretoraBovespa.Add(54, "BES SECURITIES DO BRASIL");
            StaticData.CorretoraBovespa.Add(187, "SITA");
            StaticData.CorretoraBovespa.Add(175, "OMAR CAMARGO");
            StaticData.CorretoraBovespa.Add(173, "GERAÇÃO FUTURO");
            StaticData.CorretoraBovespa.Add(1, "MAGLIANO");
            StaticData.CorretoraBovespa.Add(564, "NOVA FUTURA");
            StaticData.CorretoraBovespa.Add(88, "CM CAPITAL MARKETS");
            StaticData.CorretoraBovespa.Add(134, "BARCLAYS");
            StaticData.CorretoraBovespa.Add(83, "MAXIMA");
            StaticData.CorretoraBovespa.Add(232, "H.H PICCHIONI");
            StaticData.CorretoraBovespa.Add(191, "SENSO");
            StaticData.CorretoraBovespa.Add(29, "UNILETRA");
            StaticData.CorretoraBovespa.Add(226, "AMARIL FRANKLIN");
            StaticData.CorretoraBovespa.Add(120, "FLOW");
            StaticData.CorretoraBovespa.Add(4, "ALFA");
            StaticData.CorretoraBovespa.Add(157, "ESCRITORIO RUY LAGE SCT");
            StaticData.CorretoraBovespa.Add(99, "TENDENCIA");
            StaticData.CorretoraBovespa.Add(18, "BBM");
            StaticData.CorretoraBovespa.Add(51, "CITIGROUP GLOBAL MARKETS BR");
            StaticData.CorretoraBovespa.Add(228, "UNIBANCO INVESTSHOP");
            StaticData.CorretoraBovespa.Add(262, "MIRAE");
            StaticData.CorretoraBovespa.Add(386, "OCTO");

        }

        private void mnuFerramentasAuxiliares_Click_1(object sender, SourcedEventArgs e)
        {
            ShowHidePortfolio();
        }

        private void ShowHidePortfolio()
        {
            if (mnuFerramentasAuxiliares.IsChecked)
            {
                ContainerDireita containerDireita = new ContainerDireita();
                gridPrincipal.ColumnDefinitions[2].Width = new GridLength(450);

                containerDireita.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
                containerDireita.VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch;
                gridDireita.Children.Add(containerDireita);

                StaticData.FerramentasAuxiliaresVisiveis = true;
            }
            else
            {
                StaticData.FerramentasAuxiliaresVisiveis = false;
                gridPrincipal.ColumnDefinitions[2].Width = new GridLength(0);
            }
        }

        /// <summary>
        /// Item disparado ao clicar sobre o link de menu Scanner Intraday
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuRastraeadorIntraday_Click_1(object sender, SourcedEventArgs e)
        {
            ((PageCollection)((C1Window)formSelecionado).Content).AbrirScannerIntraday();
        }

       

        private void mnuManual_Click_1(object sender, SourcedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri("https://easytrader.traderdata.com.br/manual.pdf", UriKind.RelativeOrAbsolute), "_new");            
        }

        private void tabPrincipal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            formSelecionado = (C1Window)e.AddedItems[0];
        }

        private void versaoDesktop_Clik(object sender, SourcedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri("http://lite.traderdata.com.br/setup.exe", UriKind.RelativeOrAbsolute), "_new");            
        }

        


        

                
        

        

    }
}

