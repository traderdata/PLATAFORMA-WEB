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
using Traderdata.Client.TerminalWEB.DAO;
using Traderdata.Client.TerminalWEB.DTO;
using System.Windows.Threading;
using System.Windows.Browser;
using System.Threading;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using Traderdata.Client.TerminalWEB.Util;



namespace Traderdata.Client.TerminalWEB
{
    [ScriptableType]
    public partial class ChartOnlyMainPage : UserControl
    {
        #region Private

        /// <summary>
        /// Variavel que guarda o ativo selecionado
        /// </summary>
        private string ativoSelecionado = "";

        /// <summary>
        /// Timer que sera usado para fazer o carregamento incial
        /// </summary>
        private DispatcherTimer timerCarregamentoInicial = new DispatcherTimer();

        /// <summary>
        /// variavel de controle de bovespa RT/delay
        /// </summary>
        private bool bovespaRTDelayProcessed = false;

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
            new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.ClientDataEndpoint());

        /// <summary>
        /// Login passado
        /// </summary>
        private string login = "";

        #endregion

        #region Construtor

        /// <summary>
        /// Contrutor padrão
        /// </summary>
        public ChartOnlyMainPage(string login, string ativo)
        {
            InitializeComponent();

            //setando variaveis privadas
            this.ativoSelecionado = ativo;
            this.login = login;

            //assinando eventos de template            
            //terminalWebClient.GetTemplatesPorUserIdCompleted += terminalWebClient_GetTemplatesPorUserIdCompleted;
            //terminalWebClient.SalvaTemplateCompleted += terminalWebClient_SalvaTemplateCompleted;
                        
            //assinando eventos de grafico
            //terminalWebClient.RetornaGraficoPorAtivoPeriodicidadeCompleted += terminalWebClient_RetornaGraficoPorAtivoPeriodicidadeCompleted;
            
            //assinando evento de timer para chamar login
            timerLogin.Interval = new TimeSpan(0,0,StaticData.TempoDemo);
            
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
                        
            #region Realtime

            //eventos de Realtime BMF&BVSP
            RealTimeDAO.OnConnectErrorTick += RealTimeDAO_OnConnectErrorBVSP;
            RealTimeDAO.OnConnectSuccessTick += RealTimeDAO_OnConnectSuccessBVSP;

            //Reecbimento de tick
            RealTimeDAO.TickReceived += new RealTimeDAO.TickHandler(RealTimeDAO_TickReceived);

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

            //Executando LoginOrInsert no login
            terminalWebClient.LoginOrInsertUserCompleted += new EventHandler<TerminalWebSVC.LoginOrInsertUserCompletedEventArgs>(terminalWebClient_LoginOrInsertUserCompleted);
            terminalWebClient.LoginOrInsertUserAsync(login);
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

            busyIndicator.BusyContent = "Efetuando Login";
            busyIndicator.IsBusy = true;

            //chamando o metodo de login
            //terminalWebClient.LoginUserDistribuidorIntegradoAsync(StaticData.User.Login, StaticData.DistribuidorId);
            
        }

        /// <summary>
        /// Resposta ao evento de login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_LoginOrInsertUserCompleted(object sender, TerminalWebSVC.LoginOrInsertUserCompletedEventArgs e)
        {
            busyIndicator.IsBusy = false;
            ConnectRTServers();
            NovoGraficoAtalho(ativoSelecionado);
        }
        
        /// <summary>
        /// Metodo que conecta nos servidores de dados continuos
        /// </summary>
        private void ConnectRTServers()
        {
            //Conectando canal de tick
            RealTimeDAO.ConnectBMFBVSP();
        }

        #endregion

        #region Realtime

        /// <summary>
        /// Evento disparado ao obter sucesso na conexao RT/Delay BVSP
        /// </summary>
        void RealTimeDAO_OnConnectSuccessBVSP()
        {
            StaticData.AddLog("Conectado com sucesso no servidor BVSP RT");
            bovespaRTDelayProcessed = true;            
        }

        /// <summary>
        /// Evento disparado ao se obter erro na conexao RT/Delay BVSP
        /// </summary>
        void RealTimeDAO_OnConnectErrorBVSP()
        {
            StaticData.AddLog("Erro ao conectar no servidor BVSP RT");
            bovespaRTDelayProcessed = true;            
        }

        /// <summary>
        /// Evento disparado ao se obter erro na conexao RT/Delay BMF
        /// </summary>
        void RealTimeDAO_OnConnectSuccessBMF()
        {
            bmfRTDelayProcessed = true;
        }

        /// <summary>
        /// Evento disparado ao se obter erro na conexao RT/Delay BMF
        /// </summary>
        void RealTimeDAO_OnConnectErrorBMF()
        {
            bmfRTDelayProcessed = true;
        }

        /// <summary>
        /// Evento disparado quando se recebe um novo tick do ativo
        /// </summary>
        /// <param name="Result"></param>
        void RealTimeDAO_TickReceived(object Result)
        {
            if (((TickDTO)Result).Ativo == this.ativoSelecionado)
            {
                if (((TickDTO)Result).Variacao > 0)
                    txtAtivoVariacao.Foreground = Brushes.Green;
                else if (((TickDTO)Result).Variacao < 0)
                    txtAtivoVariacao.Foreground = Brushes.Red;
                else txtAtivoVariacao.Foreground = Brushes.White;

                txtAtivoVariacao.Text = ((TickDTO)Result).Ativo + " (" + ((TickDTO)Result).Variacao.ToString("n2").Replace(",", ".") + "%) " + ((TickDTO)Result).Ultimo.ToString("n2").Replace(",", ".");
            }
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
                case Key.Escape:
                    StaticData.tipoAcao = StaticData.TipoAcao.Seta;
                    StaticData.tipoFerramenta = StaticData.TipoFerramenta.Nenhum;
                    DesmarcaToolbarLateral();
                    //((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content).DesabilitaCross();
                    tbarSeta.IsChecked = true;
                    break;
                case Key.Delete:
                    //((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content).DeleteObjetosSelecionados();
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

            ((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content).ResetZoom();

            

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

        #region Salvar Grafico

        /// <summary>
        /// Evento do clique no botao salvar grafico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSalvarGrafico_Click_1(object sender, RoutedEventArgs e)
        {
            ((PageCollection)canvasPrincipal.Children[0]).SalvarGrafico();
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

        
        
        #region Janelas
        
        /// <summary>
        /// Metodo que faz a criação de uma nova janela
        /// </summary>
        private void NovoGrafico(string ativo, TerminalWebSVC.TemplateDTO template, Periodicidade periodicidade)
        {         
            //setando o header
            txtAtivoVariacao.Text = ativo;

            //setando o ativo selecionado
            this.ativoSelecionado = ativo;
            
            //assinando o recebimento de tick
            RealTimeDAO.StartTickSubscription(ativo);

            //Adicionando o conteudo            
            PageCollection paginas = new PageCollection(ativo, template, periodicidade);
            paginas.Width = canvasPrincipal.ActualWidth;
            paginas.Height = canvasPrincipal.ActualHeight;

            canvasPrincipal.Children.Clear();
            canvasPrincipal.Children.Add(paginas);
            canvasPrincipal.UpdateLayout();
            busyIndicator.IsBusy = false;
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

        #region Skins

        /// <summary>
        /// Evento disparado ao se clicar em Ok na configuração de cor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationOkClick(object sender, EventArgs e)
        {
            TerminalWebSVC.LayoutDTO layoutTemp = new TerminalWebSVC.LayoutDTO();
            C1TabItem tabItem = (C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem;
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
            
        }

        /// <summary>
        /// Evento disparado ao abrir o form de configuracao
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationOpen(object sender, EventArgs e)
        {
            C1TabItem tabItem = (C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem;

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
            foreach (C1TabItem tabItem in ((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.Items)
            {
                if ((string)tabItem.Header != "+")
                    ((Grafico)tabItem.Content).SetSkinPreto();
            }
            
        }

        /// <summary>
        /// Evento disparado ao se escolher o skin branco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSkinBranco_Click(object sender, RoutedEventArgs e)
        {
            foreach (C1TabItem tabItem in ((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.Items)
            {
                if ((string)tabItem.Header != "+")
                    ((Grafico)tabItem.Content).SetSkinBranco();
            }
            
        }

        /// <summary>
        /// Evento que executa a troca do skin para preto e branco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSkinPretoEBranco_Click(object sender, RoutedEventArgs e)
        {
            foreach (C1TabItem tabItem in ((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.Items)
            {
                if ((string)tabItem.Header != "+")
                    ((Grafico)tabItem.Content).SetSkinPretoBranco();
            }
            
        }

        /// <summary>
        /// Evento disparado ao se trocar para skin azul e branco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSkinAzulEBranco_Click(object sender, RoutedEventArgs e)
        {
            foreach (C1TabItem tabItem in ((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.Items)
            {
                if ((string)tabItem.Header != "+")
                    ((Grafico)tabItem.Content).SetSkinAzulBranco();
            }
            
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
            ((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[canvasPrincipal.Children.Count - 1]).c1TabControl1.SelectedItem).Content)
                      .SetTipoBarra(SeriesTypeEnum.stCandleChart);
        }

        /// <summary>
        /// Evento disparado ao se clicar sobre o botao de tipo de barra barra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void tbarBarra_Click(object sender, RoutedEventArgs e)
        {
            ((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[canvasPrincipal.Children.Count - 1]).c1TabControl1.SelectedItem).Content)
                      .SetTipoBarra(SeriesTypeEnum.stStockBarChart);
        }

        /// <summary>
        /// Evento disparado ao se clicar sobre o botao de tipo de barra linha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void tbarLinha_Click(object sender, RoutedEventArgs e)
        {
            if ((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem != null)
            {
                ((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[canvasPrincipal.Children.Count - 1]).c1TabControl1.SelectedItem).Content)
                        .SetTipoBarra(SeriesTypeEnum.stLineChart);

            }


            
        }

        /// <summary>
        /// Metodo que vai pressionar o botao correto do tipo de barra
        /// </summary>
        private void PressTipoBarraButton()
        {
            if ((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem != null)
            {
                //checando qual o tipo de barra do gráfico
                switch (((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content).GetTipoBarra())
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
            if ((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem != null)
            {
                //checando qual o tipo de barra do gráfico
                switch (((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content).GetTipoEscala())
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
            if ((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem != null)
                //checando qual o tipo de barra do gráfico
                ((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content)
                    .SetTipoEscala(ScalingTypeEnum.Linear);
            

        }

        /// <summary>
        /// Eventos disparado ao se pressionar a escala semi-log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarEscalaSemilog_Click(object sender, RoutedEventArgs e)
        {
            if ((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem != null)
                //checando qual o tipo de barra do gráfico
                ((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content)
                    .SetTipoEscala(ScalingTypeEnum.Semilog);

            

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
            foreach (C1TabItem tabItem in ((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.Items)
            {
                if ((string)tabItem.Header != "+")
                    ((Grafico)tabItem.Content).Refresh();
            }
            
        }

        /// <summary>
        /// Metodo que vai pressionar o botao de periodicidade
        /// </summary>
        private void PressBotaoPeriodicidade()
        {
            if ((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem != null)
            {
                //checando qual o tipo de barra do gráfico
                switch (((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content).GetPeriodicidade())
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
            ((PageCollection)canvasPrincipal.Children[0]).ChangePeriodicity(Periodicidade.UmMinuto);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 2 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar2Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)canvasPrincipal.Children[0]).ChangePeriodicity(Periodicidade.DoisMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 3 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar3Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)canvasPrincipal.Children[0]).ChangePeriodicity(Periodicidade.TresMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 5 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar5Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)canvasPrincipal.Children[0]).ChangePeriodicity(Periodicidade.CincoMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 10 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar10Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)canvasPrincipal.Children[0]).ChangePeriodicity(Periodicidade.DezMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 15 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar15Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)canvasPrincipal.Children[0]).ChangePeriodicity(Periodicidade.QuinzeMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 30 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar30Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)canvasPrincipal.Children[0]).ChangePeriodicity(Periodicidade.TrintaMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 60 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar60Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)canvasPrincipal.Children[0]).ChangePeriodicity(Periodicidade.SessentaMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 120 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar120Minutos_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)canvasPrincipal.Children[0]).ChangePeriodicity(Periodicidade.CentoeVinteMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão Diario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarDiario_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)canvasPrincipal.Children[0]).ChangePeriodicity(Periodicidade.Diario);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão Semanal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSemanal_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)canvasPrincipal.Children[0]).ChangePeriodicity(Periodicidade.Semanal);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão Mensal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarMensal_Click(object sender, RoutedEventArgs e)
        {
            ((PageCollection)canvasPrincipal.Children[0]).ChangePeriodicity(Periodicidade.Mensal);
            
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
            ((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content)._stockChartX.SaveToFile(StockChartX.ImageExportType.Png);
            
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
            C1TabItem tabItem = (C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem;

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

            

        }

        /// <summary>
        /// Evento disparado quando se seleciona um indicador da combo para inserção rápida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menuItemIndicadores_Click(object sender, SourcedEventArgs e)
        {
            //pegando qual gráfico está selecionado
            ((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content).InserirIndicador(((IndicatorInfoDTO)((C1MenuItem)e.Source).Tag));
            
        }

        #endregion

        #region Cores

        /// <summary>
        /// Evento disparado ao se escolher a cor no colorPicker
        /// </summary>
        /// <param name="c"></param>
        private void objectColorPicker_SelectedColorChanged(object sender, PropertyChangedEventArgs<Color> e)
        {
            ((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content).SetColorObjetoGeralSelecionado(e.NewValue);
            StaticData.corSelecionada = e.NewValue;
            borderColorPicker.Background = new SolidColorBrush(e.NewValue);
            
        }


        /// <summary>
        /// Ao clicar no botao deve ser suficiente para trocar a cor do opbjeto selecionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnColorPicker_Click_1(object sender, RoutedEventArgs e)
        {
            ((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content).SetColorObjetoGeralSelecionado(StaticData.corSelecionada);
            
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
            ((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content).SetStrokeThicknessObjetoGeralSelecionado(Convert.ToInt32(((Border)sender).Tag));

            StaticData.strokeThickness = Convert.ToInt32(((Border)sender).Tag);
            tbarStrokeSthickness.IsDropDownOpen = false;
            
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
            ((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content).SetStrokeTypeObjetoSelecionado((LinePattern)Convert.ToInt32((((Border)sender).Tag)));
            StaticData.estiloLinhaSelecionado = (LinePattern)Convert.ToInt32((((Border)sender).Tag));
            tbarStrokeType.IsDropDownOpen = false;
            
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

            if ((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem != null)
            {
                ((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content)
                    .SetInfoPanelVisibility(System.Windows.Visibility.Collapsed);

            }

            //foreach (C1TabItem tabItem in ((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.Items)
            //{
            //    if ((string)tabItem.Header != "+")
            //        if (tbarPainelInformacoes.IsChecked.Value)
            //            ((Grafico)tabItem.Content).SetInfoPanelVisibility(System.Windows.Visibility.Visible);
            //        else
            //            ((Grafico)tabItem.Content).SetInfoPanelVisibility(System.Windows.Visibility.Collapsed);

            //}
            //
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
            //foreach (C1TabItem tabItem in ((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.Items)
            //{
            //    if ((string)tabItem.Header != "+")
            //        if (tbarAfterMarket.IsChecked.Value)
            //            ((Grafico)tabItem.Content).SetAfterMarket(true);
            //        else
            //            ((Grafico)tabItem.Content).SetAfterMarket(false);

            //}
            
        }

        /// <summary>
        /// Metodo que seta ou desseta o botao de aftermarket
        /// </summary>
        private void PressAfterMarketButton()
        {
            //if ((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem != null)
            //{
            //    //checando qual o tipo de barra do gráfico
            //    tbarAfterMarket.IsChecked = ((Grafico)((C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem).Content).GetAfterMarket();

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
                TerminalWebSVC.TemplateDTO template = ((PageCollection)canvasPrincipal.Children[0]).GetTemplate();

                template.Nome = message;
                //terminalWebClient.SalvaTemplateAsync(template);
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
            //terminalWebClient.GetTemplatesPorUserIdAsync(StaticData.User.Id);

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
            //terminalWebClient.GetTemplatesPorUserIdAsync(StaticData.User.Id);

            //logando na barra de status
            StaticData.AddLog("Template salvo com sucesso");
        }

        /// <summary>
        /// Evento retornado ao carregar a lista de templates do usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void terminalWebClient_GetTemplatesPorUserIdCompleted(object sender, TerminalWebSVC.GetTemplatesPorUserIdCompletedEventArgs e)
        //{            
        //    //adicionando as entradas de template                
        //    foreach (TerminalWEB.TerminalWebSVC.TemplateDTO obj in e.Result)
        //    {                
        //        C1MenuItem mnuItemExcluir = new C1MenuItem();
        //        mnuItemExcluir.Header = obj.Nome;
        //        mnuItemExcluir.Click += mnuItemExcluir_Click;
        //        mnuItemExcluir.Tag = obj;
        //        mnuItemExcluir.IsTabStop = false;
         
        //        C1MenuItem mnuItemAplicar = new C1MenuItem();
        //        mnuItemAplicar.Header = obj.Nome;
        //        mnuItemAplicar.Click += mnuItemAplicar_Click;
        //        mnuItemAplicar.Tag = obj;
        //        mnuItemAplicar.IsTabStop = false;
        //        //mnuAplicarTemplate.Items.Add(mnuItemAplicar);
        //    }
        //}

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
                ((PageCollection)canvasPrincipal.Children[0]).AplicarTemplate(layouts,
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
                //terminalWebClient.ExcluiTemplateAsync((TerminalWebSVC.TemplateDTO)((C1MenuItem)sender).Tag);
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
            //BuscaAtivoDialog buscaAtivo = new BuscaAtivoDialog();
            
            //buscaAtivo.Closing += (sender1, e1) =>
            //{
            //    if (buscaAtivo.DialogResult.Value == true)
            //    {
            //        foreach (AtivoDTO item in buscaAtivo._flexGridAtivos.SelectedItems)
            //        {
            //            //busyIndicator.BusyContent = "Abrindo gráfico...";
            //            //busyIndicator.IsBusy = true;
                        
            //            //List<object> args = new List<object>();
            //            //args.Add(item.Codigo);
            //            //args.Add(Convert.ToInt32(((ComboBoxItem)buscaAtivo.cmbPeriodicidade.SelectedItem).Tag));
            //            //terminalWebClient.RetornaGraficoPorAtivoPeriodicidadeAsync(item.Codigo,
            //            //    Convert.ToInt32(((ComboBoxItem)buscaAtivo.cmbPeriodicidade.SelectedItem).Tag),
            //            //    StaticData.User.Id, args);                        
            //            NovoGrafico(item.Codigo, null, GeneralUtil.GetPeriodicidadeFromInt(Convert.ToInt32(((ComboBoxItem)buscaAtivo.cmbPeriodicidade.SelectedItem).Tag)));
            //        }

            //    }
            //};
            //buscaAtivo.Show();

        }

        /// <summary>
        /// Metodo que abre um procura por um gráfico salvo
        /// </summary>
        /// <param name="ativo"></param>
        public void NovoGraficoAtalho(string ativo)
        {
            busyIndicator.BusyContent = "Abrindo gráfico...";
            busyIndicator.IsBusy = true;
            NovoGrafico(ativo, null, Periodicidade.Diario);
        }

        /// <summary>
        /// Buscando grafico ja salvo previamente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void terminalWebClient_RetornaGraficoPorAtivoPeriodicidadeCompleted(object sender, TerminalWebSVC.RetornaGraficoPorAtivoPeriodicidadeCompletedEventArgs e)
        //{
        //    busyIndicator.IsBusy = false;
        //    if (e.Result == null)
        //    {
        //        List<object> args = (List<object>)e.UserState;
        //        NovoGrafico((string)args[0], null, GeneralUtil.GetPeriodicidadeFromInt(Convert.ToInt32(args[1])));
        //    }
        //    else
        //    {
        //        NovoGrafico(e.Result[0]);
        //    }
        //}


        #endregion

        #region Configuração Geral

        /// <summary>
        /// Evento disparado ao se abrir a tela d econfiguração geral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationGeralOpen(object sender, EventArgs e)
        {
            //RECUPERANDO as configurações de cor do grafico selecionado
            //TerminalWebSVC.LayoutDTO layoutTemp = new TerminalWebSVC.LayoutDTO();
            //layoutTemp = ((Grafico)tabItem.Content).GetLayoutDTOFromStockchart();

            //configurationGeral.chkGridHorizontal.IsChecked = layoutTemp.GradeHorizontal;
            //configurationGeral.chkGridVertical.IsChecked = layoutTemp.GradeVertical;
            //configurationGeral.cmbTipoVolume.SelectedItem = 0;
            //if (Convert.ToInt32(layoutTemp.PosicaoEscala) == 1)
            //    configurationGeral.rdbDireita.IsChecked = true;
            //else
            //    configurationGeral.rdbEsquerda.IsChecked = true;
            //configurationGeral.txtPrecisao.SetValue(Convert.ToDouble(layoutTemp.PrecisaoEscala));
            //configurationGeral.txtEspessuraVolume.SetValue(Convert.ToDouble(layoutTemp.VolumeStrokeThickness));


        }

        /// <summary>
        /// Evento disparado ao se clicar em Ok na tela de confgiuração
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationGeralOkClick(object sender, EventArgs e)
        {
            TerminalWebSVC.LayoutDTO layoutTemp = new TerminalWebSVC.LayoutDTO();
            C1TabItem tabItem = (C1TabItem)((PageCollection)canvasPrincipal.Children[0]).c1TabControl1.SelectedItem;
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
            
        }


        #endregion
        
        #endregion

        void timerPressButtons_Tick(object sender, EventArgs e)
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


        
        
        

        private void LayoutRoot_KeyUp_1(object sender, KeyEventArgs e)
        {
            ExecutaKeyPress(e.Key);
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

        private void mnuManual_Click_1(object sender, SourcedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri("https://easytrader.traderdata.com.br/manual.pdf", UriKind.RelativeOrAbsolute), "_new");            
        }

        private void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvasPrincipal.UpdateLayout();
        }

        


        

                
        

        

    }
}

