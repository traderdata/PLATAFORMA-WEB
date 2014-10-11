using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ModulusFE;
using ModulusFE.Indicators;
using ModulusFE.LineStudies;
using ModulusFE.SL;
using System.Windows.Input;
using C1.Silverlight;
using Traderdata.Client.TerminalWEB.Dialogs;
using Traderdata.Client.TerminalWEB.DAO;
using Traderdata.Client.TerminalWEB.DTO;
using System.Windows.Browser;
using System.Windows.Media.Imaging;
using Traderdata.Client.TerminalWEB.Util;
using Traderdata.Server.Core.DTO;

namespace Traderdata.Client.TerminalWEB
{
    public partial class Grafico
    {
        #region Variaveis Privadas

        private Thickness thicknessRegua = new Thickness(0,0,0,0);
        private double yInicialRegua = 0;
        private int xInicialRegua = 0;

        /// <summary>
        /// Timer em que o chart sera atualizado
        /// </summary>
        private System.Windows.Threading.DispatcherTimer timerUpdate = new System.Windows.Threading.DispatcherTimer();

        /// <summary>
        /// Variavel que sera usada para controlar o inicio das atualizações somente apos o termino do carregamento
        /// </summary>
        private bool lockStartRT = false;

        /// <summary>
        /// Volume auxiliar para multiplas periodicidades
        /// </summary>
        private double volumeIntradayMultiplaPeriodicidade = 0;

        /// <summary>
        /// Variavel local que armazena o volume de todo a semana, menos o volume do dia de hoje
        /// </summary>
        private double volumeSemanaMenosDia = 0;

        /// <summary>
        /// Variavel local que armazena o volume de todo o mes, menos o volume do dia de hoje
        /// </summary>
        private double volumeMesMenosDia = 0;

        /// <summary>
        /// Variavel que controla se o gráfico já foi visualizado
        /// </summary>
        private bool Dirty = false;

        #region Lista de objetos por periodicidades
        private List<TerminalWebSVC.ObjetoEstudoDTO> objetos1Minuto = new List<TerminalWebSVC.ObjetoEstudoDTO>();
        private List<TerminalWebSVC.ObjetoEstudoDTO> objetos2Minuto = new List<TerminalWebSVC.ObjetoEstudoDTO>();
        private List<TerminalWebSVC.ObjetoEstudoDTO> objetos3Minuto = new List<TerminalWebSVC.ObjetoEstudoDTO>();
        private List<TerminalWebSVC.ObjetoEstudoDTO> objetos5Minuto = new List<TerminalWebSVC.ObjetoEstudoDTO>();
        private List<TerminalWebSVC.ObjetoEstudoDTO> objetos10Minuto = new List<TerminalWebSVC.ObjetoEstudoDTO>();
        private List<TerminalWebSVC.ObjetoEstudoDTO> objetos15Minuto = new List<TerminalWebSVC.ObjetoEstudoDTO>();
        private List<TerminalWebSVC.ObjetoEstudoDTO> objetos30Minuto = new List<TerminalWebSVC.ObjetoEstudoDTO>();
        private List<TerminalWebSVC.ObjetoEstudoDTO> objetos60Minuto = new List<TerminalWebSVC.ObjetoEstudoDTO>();
        private List<TerminalWebSVC.ObjetoEstudoDTO> objetos120Minuto = new List<TerminalWebSVC.ObjetoEstudoDTO>();
        private List<TerminalWebSVC.ObjetoEstudoDTO> objetosDiario = new List<TerminalWebSVC.ObjetoEstudoDTO>();
        private List<TerminalWebSVC.ObjetoEstudoDTO> objetosSemanal = new List<TerminalWebSVC.ObjetoEstudoDTO>();
        private List<TerminalWebSVC.ObjetoEstudoDTO> objetosMensal = new List<TerminalWebSVC.ObjetoEstudoDTO>();
        #endregion

        /// <summary>
        /// Lista de indicadores que nao conseguiram ser inseridos devido a alguma falha de validação
        /// </summary>
        List<TerminalWebSVC.IndicadorDTO> listaIndicadoresComErro = new List<TerminalWebSVC.IndicadorDTO>();

        /// <summary>
        /// Lista dos indicadores que devem ser ignorados nas migrações de periodicidades
        /// </summary>
        List<string> listaIndicadoresDevemSerIgnorados = new List<string>();

        /// <summary>
        /// Variavel de controle para indicar se o darvabox está ativo ou nao
        /// </summary>
        private bool DarvaBoxes = false;

        /// <summary>
        /// Posição X em relação ao componenten gráfico
        /// </summary>
        private double X = 0;

        /// <summary>
        /// Posição Y em relação ao componente gráfico
        /// </summary>
        private double Y = 0;

        /// <summary>
        /// Preço Y
        /// </summary>
        private double priceY = 0;

        /// <summary>
        /// Variavel que guarda se deve ou nao carregar o after market
        /// </summary>
        private bool afterMarket = false;

        /// <summary>
        /// Variavel que armazena o ultimo registro onde o mouse passou sorbe o grafico
        /// </summary>
        private int registroX = -1;
        
        /// <summary>
        /// Variavel de controle para evitar rodar o Loaded mais de uma vez
        /// </summary>
        private bool CarregandoPrimeiraVez = true;

        /// <summary>
        /// Lista de objetos colocados no gráfico
        /// </summary>
        private List<LineStudy> ListaLineStudy = new List<LineStudy>();

        /// <summary>
        /// Lista de objetos do tipo valor Y colocados no gráfico
        /// </summary>
        private List<LineStudy> ListaValorY = new List<LineStudy>();

        /// <summary>
        /// Variavel de controla o estado atual do gráfico em relação a colocação de novos objetos
        /// </summary>
        private EstadoGrafio CurrentState = EstadoGrafio.Nenhum;

        /// <summary>
        /// Variavel que armazena o ativo que vai ser aberto
        /// </summary>
        private string ativo = "";
        private string ativoAntigo = "";

        /// <summary>
        /// Variavel de controle da periodicidade
        /// </summary>
        private Periodicidade Periodicidade = Periodicidade.Diario;

        /// <summary>
        /// Variavel de controle da periodicidade no caso de troca de periodicidade
        /// </summary>
        private Periodicidade PeriodicidadeAnterior = Periodicidade.Diario;

        /// <summary>
        /// Classe que trata os dados de marketdata
        /// </summary>
        private MarketDataDAO marketDataDAO = new MarketDataDAO();

        /// <summary>
        /// Variavel de controle de layout
        /// </summary>
        public TerminalWebSVC.LayoutDTO Layout = new TerminalWebSVC.LayoutDTO();

        #endregion

        #region Construtor


        /// <summary>
        /// Construtor padrão do gráfico
        /// </summary>
        /// <param name="ativo"></param>
        public Grafico(string ativo, TerminalWebSVC.LayoutDTO layout, Periodicidade periodicidade)
        {
            InitializeComponent();
            
            //setando a periodicidade
            this.Periodicidade = periodicidade;
            this.PeriodicidadeAnterior = periodicidade;

            //Assinando os eventos de comunicazação WCF
            marketDataDAO.GetCotacaoDiariaCompleted += new MarketDataDAO.CotacaoDiarioHandler(marketDataDAO_GetCotacaoDiariaCompleted);
            marketDataDAO.GetCotacaoIntradayCompleted += new MarketDataDAO.CotacaoIntradayHandler(marketDataDAO_GetCotacaoIntradayCompleted);
            marketDataDAO.SetCotacaoDiariaCacheCompleted += new MarketDataDAO.CotacaoDiarioCacheHandler(marketDataDAO_SetCotacaoDiariaCacheCompleted);
         
            //Rodando as configurações inciais do gráfico
            this.ativo = ativo;
            
            //setando o layout do gráfico
            this.Layout = CloneLayout(layout, true, true);
            
            //setando estado do gráfico para carregando
            this.CurrentState = EstadoGrafio.Carregando;

            //acionando timer
            timerUpdate.Interval = new TimeSpan(0, 0, 1);
            timerUpdate.Tick += new EventHandler(timerUpdate_Tick);
            timerUpdate.Start();

            if (CarregandoPrimeiraVez)
            {
                //setando o busy para on
                busyIndicator.IsBusy = true;
                lockStartRT = true;

                if (!this.Intraday())
                {
                    marketDataDAO.GetCotacaoDiariaAsync(ativo);             
                }
                else
                {
                    marketDataDAO.GetCotacaoIntradayAsync(ativo, true, false);
                }


                //setando o carregamento de primeira vez para false
                CarregandoPrimeiraVez = false;

                //flagando que o gráfico ja fora visto
                Dirty = true;
            }
            else
            {
                //Atualizando cores
                AtualizaCores();
            }

            //assinando o realtime
            RealTimeDAO.TickReceived += new RealTimeDAO.TickHandler(RealTimeDAO_TickReceived);

        }

        void timerUpdate_Tick(object sender, EventArgs e)
        {            
            _stockChartX.RecalculateIndicators();
            foreach (Series obj in _stockChartX.SeriesCollection)
            {
                if (obj.TickBox == TickBoxPosition.Right)
                {
                    obj.TickBox = TickBoxPosition.None;
                    obj.TickBox = TickBoxPosition.Right;
                }
            }
            _stockChartX.Update();
        }

        void RealTimeDAO_TickReceived(object Result)
        {
            try
            {
                //se for de outro ativo devemos ignorar o tick
                if (((TickDTO)Result).Ativo == this.ativo)
                {
                    TickDTO tick = (TickDTO)Result;


                    #region atualização do grafico
                    //checando se a ultima barra
                    DateTime ultimaBarra = _stockChartX.GetTimestampByIndex(_stockChartX.RecordCount - 1).Value;

                    //checando qual o tipo de atualização
                    switch (this.Periodicidade)
                    {
                        case TerminalWEB.Periodicidade.Diario:
                            AtualizaGraficoDiario(ultimaBarra, tick);
                            break;
                        case TerminalWEB.Periodicidade.Semanal:
                            AtualizaGraficoSemanal(ultimaBarra, tick);
                            break;
                        case TerminalWEB.Periodicidade.Mensal:
                            AtualizaGraficoMensal(ultimaBarra, tick);
                            break;
                        case TerminalWEB.Periodicidade.UmMinuto:
                        case TerminalWEB.Periodicidade.DoisMinutos:
                        case TerminalWEB.Periodicidade.TresMinutos:
                        case TerminalWEB.Periodicidade.CincoMinutos:
                        case TerminalWEB.Periodicidade.DezMinutos:
                        case TerminalWEB.Periodicidade.QuinzeMinutos:
                        case TerminalWEB.Periodicidade.TrintaMinutos:
                        case TerminalWEB.Periodicidade.SessentaMinutos:
                        case TerminalWEB.Periodicidade.CentoeVinteMinutos:
                            AtualizaGraficoIntraday(ultimaBarra, tick, GeneralUtil.GetIntPeriodicidade(this.Periodicidade));
                            break;
                    }

        
                    //atualizando o infpanel
                    AtualizaInfoPanel();
                    #endregion

                }
            }
            catch(Exception exc)
            {
            }

        }

        
        

        #endregion
        
        #region Eventos MarketData

        /// <summary>
        /// Eventos disparado apos o carregamento do historico diario
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_GetCotacaoDiariaCompleted(List<CotacaoDTO> Result)
        {
            //setando o busy para false
            busyIndicator.IsBusy = false;
            
            //checando se a periodicidade foi trocada
            if (this.Periodicidade != this.PeriodicidadeAnterior)
            {
                CarregaGrafico(this.Layout, ativo, marketDataDAO.ConvertPeriodicidadeDiaria(Result, GeneralUtil.GetIntPeriodicidade(this.Periodicidade)), true);
                this.PeriodicidadeAnterior = this.Periodicidade;
            }
            else
                CarregaGrafico(this.Layout, ativo, marketDataDAO.ConvertPeriodicidadeDiaria(Result, GeneralUtil.GetIntPeriodicidade(this.Periodicidade)), false);

        }

        /// <summary>
        /// Evento que é rodado ao terminar de carregas as cotações intraday
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_GetCotacaoIntradayCompleted(List<CotacaoDTO> Result)
        {
            //setando o busy para false
            busyIndicator.IsBusy = false;

            //checando se a periodicidade foi trocada
            if (this.Periodicidade != this.PeriodicidadeAnterior)
            {
                CarregaGrafico(this.Layout, ativo, marketDataDAO.ConvertPeriodicidadeIntraday(Result, GeneralUtil.GetIntPeriodicidade(this.Periodicidade)), true);
                this.PeriodicidadeAnterior = this.Periodicidade;
            }
            else
                CarregaGrafico(this.Layout, ativo, marketDataDAO.ConvertPeriodicidadeIntraday(Result, GeneralUtil.GetIntPeriodicidade(this.Periodicidade)), false);
        }

        /// <summary>
        /// Evento disparado apos se carregar o cache
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_SetCotacaoDiariaCacheCompleted(string Result)
        {
            //marketDataDAO.GetCotacaoDiariaAsync(ativo);
        }



        #endregion

        #region Eventos Stockchart
        
        /// <summary>
        /// Evento disparado ao se clicar 2 vezes sobre um objeto do stockchart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_LineStudyDoubleClick_1(object sender, StockChartX.LineStudyMouseEventArgs e)
        {
            
        }

        /// <summary>
        /// Evento disparado ao se clicar sobre o menu 
        /// </summary>
        /// <param name="serie"></param>
        void seriesVolume_EditSerieInChartPanel(Series serie)
        {
            //carregando a listagem de indicadores no contextmenu
            mnuAdicionarIndicadorSemConfiguracao.Items.Clear();
            foreach (IndicatorInfoDTO obj in StaticData.GetListaIndicadores())
            {
                IndicatorInfoDTO objClonado = obj;
                C1MenuItem menuItemAux = new C1MenuItem();
                menuItemAux.Header = objClonado.NomePortugues;
                menuItemAux.Tag = serie;
                menuItemAux.Click += menuItemAddIndicatorOverSeries_Click;
                mnuAdicionarIndicadorSemConfiguracao.Items.Add(menuItemAux);
            }
            
            mnuContextIndicadorSemConfiguracao.Show(_stockChartX, new Point(this.X, this.Y));
        }

        /// <summary>
        /// Evento disparado ao se tentar editar um indicador atraves do proprio grafico
        /// </summary>
        /// <param name="indicator"></param>
        void indicadorTemp_EditIndicatorInChartPanel(Indicator indicator)
        {
            //carregando a listagem de indicadores no contextmenu
            mnuAdicionarIndicador.Items.Clear();
            foreach (IndicatorInfoDTO obj in StaticData.GetListaIndicadores())
            {
                IndicatorInfoDTO objClonado = obj;
                C1MenuItem menuItemAux = new C1MenuItem();
                menuItemAux.Header = objClonado.NomePortugues;
                menuItemAux.Tag = indicator;
                menuItemAux.Click += menuItemAddIndicatorOverIndicator_Click;
                mnuAdicionarIndicador.Items.Add(menuItemAux);
            }

            mnuItemConfigurar.Header = "Configurar " + indicator.FullName;
            mnuItemConfigurar.Tag = indicator;
            mnuItemExcluir.Header = "Excluir " + indicator.FullName;
            mnuItemExcluir.Tag = indicator;

            mnuContextIndicador.Show(_stockChartX, new Point(this.X, this.Y));
        }

        /// <summary>
        /// Evento disparado ao se dar um duplo clique sobre o indicador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_IndicatorDoubleClick_1(object sender, StockChartX.IndicatorDoubleClickEventArgs e)
        {
            EditIndicator(e.Indicator);
        }

        /// <summary>
        /// Evento disparado ao se levantar uma tecla que fora pressionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_KeyUp(object sender, KeyEventArgs e)
        {
            //mainPage.ExecutaKeyPress(e.Key);
        }

        /// <summary>
        /// Evento disparado ao se terminar de dar zoom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_ChartZoom(object sender, EventArgs e)
        {
            scrollbar.Value = _stockChartX.LastVisibleRecord;
            scrollbar.ViewportSize = _stockChartX.VisibleRecordCount;
            scrollbar.Minimum = _stockChartX.VisibleRecordCount;
            scrollbar.Maximum = _stockChartX.RecordCount;
            this.Layout.VisibleRecords = _stockChartX.VisibleRecordCount;

            CurrentState = EstadoGrafio.Nenhum;
        }

        /// <summary>
        /// Evento que é disparado ao se entrar com o mouse sobre o componente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_MouseEnter(object sender, MouseEventArgs e)
        {
            #region Tratamento de troca do cursor do mouse
            switch (StaticData.tipoAcao)
            {
                case StaticData.TipoAcao.CROSS:                    
                    _stockChartX.CrossHairs = true;
                    _stockChartX.Cursor = Cursors.None;
                    borderValorYPosicionado.Visibility = System.Windows.Visibility.Visible;
                    break;
                case StaticData.TipoAcao.Ferramenta:
                    _stockChartX.Cursor = Cursors.Hand;
                    
                    break;
                case StaticData.TipoAcao.Indicador:
                    _stockChartX.Cursor = Cursors.Hand;
                    break;
                case StaticData.TipoAcao.Zoom:
                    _stockChartX.Cursor = Cursors.Hand;
                    _stockChartX.DisableZoomArea = false;
                    break;
                default:
                    _stockChartX.Cursor = Cursors.Arrow;
                    break;
            }
            #endregion

            #region Tratando o infopanel
            //if (Layout.PainelInfo.HasValue)
            //{
            _stockChartX.InfoPanelPosition = InfoPanelPositionEnum.Hidden;
            //    if (Layout.PainelInfo.Value)
            //        infoPanel.Visibility = System.Windows.Visibility.Visible;
            //    else
            //        infoPanel.Visibility = System.Windows.Visibility.Collapsed;
            //}
            #endregion
        }

        /// <summary>
        /// Evento disparado ao se sair do componente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_MouseLeave(object sender, MouseEventArgs e)
        {
            _stockChartX.DisableZoomArea = true;
            _stockChartX.CrossHairs = false;
            _stockChartX.Cursor = Cursors.Arrow;
            borderValorYPosicionado.Visibility = System.Windows.Visibility.Collapsed;

            
        }

        /// <summary>
        /// Evento disparado ao se mover o mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_ChartPanelMouseMove(object sender, StockChartX.ChartPanelMouseMoveArgs e)
        {
            priceY = e.Y;
            registroX = _stockChartX.FirstVisibleRecord + e.Record;

            //Recuperando os valores de acorod com o registroX para alterar no infopanel
            AtualizaInfoPanel();

            //#region Trocando cursor
            //if ((StaticData.tipoAcao == StaticData.TipoAcao.Ferramenta) ||
            //    (StaticData.tipoAcao == StaticData.TipoAcao.Indicador) ||
            //    (StaticData.tipoAcao == StaticData.TipoAcao.Zoom))
            //        _stockChartX.Cursor = Cursors.Hand;
            //else
            //        _stockChartX.Cursor = Cursors.Arrow;
            
            //#endregion

            if (StaticData.tipoAcao == StaticData.TipoAcao.CROSS)
            {
            
                if ((registroX > 0) && (registroX < _stockChartX.RecordCount))
                {                    
                    txtValorYPosicionado.Text = Math.Round(e.Y, 2).ToString();

                    //calculando os valores da caixa de regua
                    txtDifPeriodoRegua.Text = Math.Abs(registroX - xInicialRegua).ToString() + " Períodos";
                    txtDifValorRegua.Text = "R$ " + (priceY - yInicialRegua).ToString("n2");
                    txtDifPercentualRegua.Text = Math.Round(((Convert.ToDouble(this.txtValorYPosicionado.Text) - yInicialRegua) / yInicialRegua)*100, 2).ToString() + "%";
                    
                    //movendo a reta                 
                    if (borderRegua.Visibility == System.Windows.Visibility.Visible)
                    {
                        _stockChartX.SetObjectPosition("REGUA", Convert.ToInt32(_stockChartX.GetObjectStartRecord("REGUA").Value), _stockChartX.GetObjectStartValue("REGUA").Value,
                            registroX, priceY);
                    }
                }
            }
            else
            {
                if (CurrentState == EstadoGrafio.Editando)
                {
                    ListaLineStudy[ListaLineStudy.Count-1].SetXYValues(ListaLineStudy[ListaLineStudy.Count-1].X1Value,
                        ListaLineStudy[ListaLineStudy.Count-1].Y1Value,
                        registroX, e.Y);
                }
            }
        }

        /// <summary>
        /// Evento disparado ao se passar o mouse sobre o componente stockchart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_MouseMove(object sender, MouseEventArgs e)
        {            
            //setando variaveis globais de posicionamento do mouse em relação ao _stockchart
            this.X = e.GetPosition(_stockChartX).X;
            this.Y = e.GetPosition(_stockChartX).Y;

            switch (StaticData.tipoAcao)
            {
                case StaticData.TipoAcao.CROSS:
                    borderValorYPosicionado.Margin = new Thickness(0, e.GetPosition(_stockChartX).Y, 0, 0);

                    //posicionando o border junto do cross
                    thicknessRegua.Left = X + 10;
                    thicknessRegua.Top = Y + 10;
                    borderRegua.Margin = thicknessRegua;

                    break;
                
            }
                
        }

        /// <summary>
        /// Metodo que é executado quando se clica com o botão esquerdo do mouse sobre um painel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_ChartPanelMouseLeftClick(object sender, StockChartX.ChartPanelMouseLeftClickEventArgs e)
        {
            //variaveis auxiliares
            object[] args = new object[0];

            //inicia uma inserção de objeto
            if (CurrentState == EstadoGrafio.Nenhum)
            {
                //checando se é um tipo de objeto válido
                if (StaticData.LineStudySelecionado() != LineStudy.StudyTypeEnum.Unknown)
                {
                    LineStudy line = null;

                    if (StaticData.LineStudySelecionado() == LineStudy.StudyTypeEnum.VerticalLine)
                    {
                        args = new object[]
                         {
                           false, //true - show record number, false - show datetime
                           true, //true - show text with line, false - show only line
                           "d", //custom datetime format, when args[0] == false. See MSDN:DateTime.ToString(string) for legal values
                         };
                        line = _stockChartX.CreateLineStudy(StaticData.LineStudySelecionado(), Guid.NewGuid().ToString(), new SolidColorBrush(StaticData.corSelecionada), e.Panel.Index, args);
                    }
                    else if (StaticData.LineStudySelecionado() == LineStudy.StudyTypeEnum.SymbolObject)
                    {
                        line = _stockChartX.CreateSymbolObject(StaticData.SymbolTypeSelecionado(), Guid.NewGuid().ToString(), e.Panel.Index, new Size(16, 16));
                    }
                    else if (StaticData.LineStudySelecionado() == LineStudy.StudyTypeEnum.StaticText)
                    {
                        if (StaticData.tipoFerramenta != StaticData.TipoFerramenta.ValorY)
                        {
                            PromptTexto promptTexto = new PromptTexto("Comentario", "", 10, true);
                            promptTexto.Width = 400;
                            promptTexto.Height = 300;

                            promptTexto.Closing += (sender1, e1) =>
                            {
                                if (promptTexto.DialogResult.Value == true)
                                {
                                    if (promptTexto.Texto != "")
                                    {
                                        line = _stockChartX.AddStaticText(promptTexto.Texto, Guid.NewGuid().ToString(), 
                                            new SolidColorBrush(StaticData.corSelecionada), 10, e.Panel.Index);
                                        line.IsContextMenuDisabled = true;
                                        line.Selectable = true;
                                        int recordLocal = _stockChartX.GetReverseX(e.Timestamp.Value, true);
                                        line.SetXYValues(recordLocal, e.Price, recordLocal, e.Price);
                                        CurrentState = EstadoGrafio.Nenhum;
                                        ListaLineStudy.Add(line);
                                        _stockChartX.Update();
                                    }
                                }
                            };
                            promptTexto.Show();
                            return;
                        }
                        else
                        {
                            line = _stockChartX.AddStaticText(Math.Round(e.Price,2).ToString(), Guid.NewGuid().ToString(), 
                                new SolidColorBrush(StaticData.corSelecionada), 10, e.Panel.Index);
                            
                            line.IsContextMenuDisabled = true;
                            line.Selectable = true;
                            int recordLocal = _stockChartX.GetReverseX(e.Timestamp.Value, true);
                            line.SetXYValues(recordLocal, e.Price, recordLocal, e.Price);
                            CurrentState = EstadoGrafio.Nenhum;
                            ListaLineStudy.Add(line);
                            ListaValorY.Add(line);
                            _stockChartX.Update();
                            return;
                            
                        }

                    }
                    else if (StaticData.LineStudySelecionado() == LineStudy.StudyTypeEnum.FibonacciRetracements)
                    {

                        List<object> arParams = new List<object>();
                        arParams.Add(0);
                        arParams.Add(0.382);
                        arParams.Add(0.50);
                        arParams.Add(0.618);
                        arParams.Add(1);

                        //arParams.Add(0.60);

                        //TODOS OS OUTROS ESTUDOS
                        line = _stockChartX.CreateLineStudy(StaticData.LineStudySelecionado(), Guid.NewGuid().ToString(),
                                new SolidColorBrush(StaticData.corSelecionada), e.Panel.Index, arParams.ToArray<object>());


                    }
                    else
                    {
                        //TODOS OS OUTROS ESTUDOS
                        line = _stockChartX.CreateLineStudy(StaticData.LineStudySelecionado(),
                            Guid.NewGuid().ToString(),
                            new SolidColorBrush(StaticData.corSelecionada), e.Panel.Index);



                        if (StaticData.tipoFerramenta == StaticData.TipoFerramenta.RetaSuporte)
                        {
                            double pontoMinimo = _stockChartX.GetValue(_stockChartX.Symbol + ".Low", _stockChartX.FirstVisibleRecord + _stockChartX.GetReverseX(e.X)).Value;
                            line.SetXYValues(line.X1Value, pontoMinimo, line.X2Value, pontoMinimo);
                        }
                        if (StaticData.tipoFerramenta == StaticData.TipoFerramenta.RetaResistencia)
                        {
                            double pontoMaximo = _stockChartX.GetValue(_stockChartX.Symbol + ".High", _stockChartX.FirstVisibleRecord + _stockChartX.GetReverseX(e.X)).Value;
                            line.SetXYValues(line.X1Value, pontoMaximo, line.X2Value, pontoMaximo);
                        }

                    }


                    line.IsContextMenuDisabled = true;
                    line.Selectable = false;
                    line.StrokeType = StaticData.estiloLinhaSelecionado;
                    line.StrokeThickness = StaticData.strokeThickness;

                    if ( (StaticData.tipoFerramenta != StaticData.TipoFerramenta.RetaSuporte) &&
                         (StaticData.tipoFerramenta != StaticData.TipoFerramenta.RetaResistencia) )
                    {
                        int record = _stockChartX.GetReverseX(e.Timestamp.Value, true);
                        line.SetXYValues(record, e.Price, record, e.Price);
                    }

                    CurrentState = EstadoGrafio.Editando;
                    ListaLineStudy.Add(line);

                    //checando se é objeto de 1 clique somente
                    if ((StaticData.LineStudySelecionado() == LineStudy.StudyTypeEnum.HorizontalLine) ||
                         (StaticData.LineStudySelecionado() == LineStudy.StudyTypeEnum.VerticalLine) ||
                         (StaticData.LineStudySelecionado() == LineStudy.StudyTypeEnum.ImageObject) ||
                         (StaticData.LineStudySelecionado() == LineStudy.StudyTypeEnum.StaticText) ||
                         (StaticData.LineStudySelecionado() == LineStudy.StudyTypeEnum.SymbolObject))
                    {
                        CurrentState = EstadoGrafio.Nenhum;
                        _stockChartX.Cursor = Cursors.Arrow;
                        ListaLineStudy[ListaLineStudy.Count - 1].Selectable = true;
                        _stockChartX.Update();
                    }

                }
                else if (StaticData.tipoAcao == StaticData.TipoAcao.Indicador)
                {
                    CurrentState = EstadoGrafio.InserindoIndicador;
                    //InserirIndicador(e.Panel);                    
                }
                else if (StaticData.tipoAcao == StaticData.TipoAcao.CROSS)
                {
                    if (borderRegua.Visibility == System.Windows.Visibility.Collapsed)
                    {
                        xInicialRegua = _stockChartX.GetReverseX(e.Timestamp.Value, true);
                        yInicialRegua = e.Price;

                        LineStudy line = _stockChartX.CreateLineStudy(LineStudy.StudyTypeEnum.TrendLine, "REGUA", new SolidColorBrush(Colors.Black), e.Panel.Index);

                        int record = _stockChartX.GetReverseX(e.Timestamp.Value, true);
                        line.SetXYValues(record, e.Price, record, e.Price);
                        //borderRegua.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        //borderRegua.Visibility = System.Windows.Visibility.Collapsed;
                        //_stockChartX.RemoveObject("REGUA");
                    }
                }
                
            }
            else
            {
                CurrentState = EstadoGrafio.Nenhum;
                ListaLineStudy[ListaLineStudy.Count - 1].Selectable = true;
                _stockChartX.Update();
            }

            

        }

        /// <summary>
        /// Evento disparado ao se clicar o botao direito sobre um layout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {            
            //ctxMenu.Show(_stockChartX, e.GetPosition(_stockChartX));
        }
        
        /// <summary>
        /// Evento disparado ao se clicar o botao esquerdo sobre um objeto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_LineStudyLeftClick(object sender, StockChartX.LineStudyMouseEventArgs e)
        {            
            //tratamento especial para ferramenta Valor Y
            if (ListaValorY.Contains(e.LineStudy))
            {
                double y1 = e.LineStudy.Y1Value;
                double y2 = e.LineStudy.Y2Value;
                double x1 = e.LineStudy.X1Value;
                double x2 = e.LineStudy.X2Value;
                
                _stockChartX.RemoveObject(e.LineStudy.Key);

                LineStudy line = _stockChartX.AddStaticText(Math.Round(y1, 2).ToString(), Guid.NewGuid().ToString(),
                                    new SolidColorBrush(StaticData.corSelecionada), 10, e.LineStudy.Panel.Index);
                line.IsContextMenuDisabled = true;
                line.Selectable = true;
                
                line.SetXYValues(x1, y1, x2, y2);
                CurrentState = EstadoGrafio.Nenhum;
                ListaLineStudy.Add(line);
                ListaValorY.Add(line);
                _stockChartX.Update();
            }
        }


        #endregion

        #region Eventos Form
        

        #region Grid Principal

        /// <summary>
        /// Evento carregado ao terminar de carregar os componentres gráficos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            //if (CarregandoPrimeiraVez)
            //{
            //    //associando ao form pai
            //    //mainPage = ((ChartOnlyMainPage)((BusyIndicator)((Grid)((Grid)((Canvas)((PageCollection)((Grid)((Canvas)((C1TabControl)((C1TabItem)this.Parent).Parent).Parent).Parent).Parent).Parent)
            //    //        .Parent).Parent).Parent).Parent);
                
            //    //setando o busy para on
            //    busyIndicator.IsBusy = true;
            //    lockStartRT = true;
                
            //    if (!this.Intraday())
            //    {                    
            //        marketDataDAO.GetCotacaoDiariaAsync(ativo);
            //        //marketDataDAO.SetCacheCotacaoIntradayAsync(ativo);
                    
            //    }
            //    else
            //    {
            //        //marketDataDAO.SetCacheCotacaoDiarioAsync(ativo);
            //        marketDataDAO.GetCotacaoIntradayAsync(ativo, true, false);
            //    }
                

            //    //setando o carregamento de primeira vez para false
            //    CarregandoPrimeiraVez = false;

            //    //flagando que o gráfico ja fora visto
            //    Dirty = true;
            //}
            //else
            //{
            //    //Atualizando cores
            //    AtualizaCores();
            //}

            ////assinando o realtime
            //RealTimeDAO.TickReceived += new RealTimeDAO.TickHandler(RealTimeDAO_TickReceived);

        }

        /// <summary>
        /// Evento disparado ao se fazer o resize do grafico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridPrincipal_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            try
            {
                _stockChartX.GetPanelByIndex(0).ChartPanelLabel.Margin = new Thickness((_stockChartX.ActualWidth / 2) - (_stockChartX.GetPanelByIndex(0).ChartPanelLabel.ActualWidth / 2)
                    , 0, 0, 0);
            }
            catch { }
        }

        #endregion

        #region Scrollbar

        /// <summary>
        /// MEtodo responsavel por scrollar os registros para a esquerda
        /// </summary>
        /// <param name="records"></param>
        private void ScrollChartLeft(int records)
        {
            if (_stockChartX.FirstVisibleRecord - records > 0)
            {
                _stockChartX.FirstVisibleRecord -= records;
                _stockChartX.LastVisibleRecord -= records;
            }
            else
            {
                _stockChartX.FirstVisibleRecord = 0;
                _stockChartX.LastVisibleRecord = (int)scrollbar.ViewportSize;

            }
        }

        /// <summary>
        /// Metodo que serve para scrollar os registros para a direita
        /// </summary>
        /// <param name="records"></param>
        private void ScrollChartRight(int records)
        {
            if (_stockChartX.LastVisibleRecord + records < _stockChartX.RecordCount)
            {
                _stockChartX.FirstVisibleRecord += records;
                _stockChartX.LastVisibleRecord += records;
            }
            else
            {
                _stockChartX.LastVisibleRecord = _stockChartX.RecordCount;
                _stockChartX.FirstVisibleRecord = _stockChartX.LastVisibleRecord - (int)scrollbar.ViewportSize;
            }
        }

        /// <summary>
        /// Evento disparado ao se scrolar a scrollbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void scrollbar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {            
            switch (e.ScrollEventType)
            {
                case System.Windows.Controls.Primitives.ScrollEventType.SmallDecrement:
                    ScrollChartLeft(5);                    
                    break;
                case System.Windows.Controls.Primitives.ScrollEventType.SmallIncrement:
                    ScrollChartRight(5);
                    break;
                case System.Windows.Controls.Primitives.ScrollEventType.LargeDecrement:

                    if (_stockChartX.FirstVisibleRecord - 25 > 0)
                    {
                        _stockChartX.FirstVisibleRecord = _stockChartX.FirstVisibleRecord - 25;
                        _stockChartX.LastVisibleRecord = _stockChartX.LastVisibleRecord - 25;
                    }
                    else
                    {
                        int visibleRecordsTemp = _stockChartX.VisibleRecordCount;
                        _stockChartX.FirstVisibleRecord = 1;
                        _stockChartX.LastVisibleRecord = visibleRecordsTemp;
                    }
                    break;
                case System.Windows.Controls.Primitives.ScrollEventType.LargeIncrement:
                    if (_stockChartX.LastVisibleRecord + 25 <= _stockChartX.RecordCount)
                    {
                        _stockChartX.LastVisibleRecord = _stockChartX.LastVisibleRecord + 25;
                        _stockChartX.FirstVisibleRecord = _stockChartX.FirstVisibleRecord + 25;                        
                    }
                    else
                    {
                        int visibleRecordsTemp = _stockChartX.VisibleRecordCount;
                        _stockChartX.LastVisibleRecord = this._stockChartX.RecordCount;
                        _stockChartX.FirstVisibleRecord = _stockChartX.LastVisibleRecord - visibleRecordsTemp;
                        
                    }
                   
                    break;
                case System.Windows.Controls.Primitives.ScrollEventType.ThumbTrack:
                    if (Convert.ToInt32(scrollbar.Value) > _stockChartX.LastVisibleRecord)
                    {
                        //sentenças invertidas
                        _stockChartX.LastVisibleRecord = Convert.ToInt32(scrollbar.Value);
                        _stockChartX.FirstVisibleRecord = Convert.ToInt32(scrollbar.Value) - Convert.ToInt32(scrollbar.ViewportSize);
                    }
                    else
                    {
                        if (Convert.ToInt32(scrollbar.Value) - scrollbar.ViewportSize > 0)
                        {
                            _stockChartX.FirstVisibleRecord = Convert.ToInt32(scrollbar.Value) - Convert.ToInt32(scrollbar.ViewportSize);
                            _stockChartX.LastVisibleRecord = Convert.ToInt32(scrollbar.Value);
                        }
                        else
                        {
                            _stockChartX.FirstVisibleRecord = 0;
                            _stockChartX.LastVisibleRecord = this.Layout.VisibleRecords;
                        }
                    }
                    break;
                
            }
            _stockChartX.Update();
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão Mais a esquerda para adicionar espaço a esquerda para desenhos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMaisLeftClick(object sender, RoutedEventArgs e)
        {            

            if (_stockChartX.LeftChartSpace < 300)
            {
                _stockChartX.LeftChartSpace += 20;
                _stockChartX.Update();
            }            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão Mais a esquerda para adicionar espaço a esquerda para desenhos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMenosLeftClick(object sender, RoutedEventArgs e)
        {
            if (_stockChartX.LeftChartSpace > 10)
            {
                _stockChartX.LeftChartSpace -= 20;
                _stockChartX.Update();
            }
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão Mais a direita para adicionar espaço a direita para desenhos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMaisRightClick(object sender, RoutedEventArgs e)
        {
            if (_stockChartX.RightChartSpace < 300)
            {
                _stockChartX.RightChartSpace += 20;
                _stockChartX.Update();
            }
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão Mais a direita para adicionar espaço a direita para desenhos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMenosRightClick(object sender, RoutedEventArgs e)
        {
            if (_stockChartX.RightChartSpace > 0)
            {
                _stockChartX.RightChartSpace -= 20;
                _stockChartX.Update();
            }
        }

        #endregion

        #region InfoPanel

        /// <summary>
        /// Evento disparado ao se passar o mouse sobre o info panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void infoPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            //if (infoPanel.Margin.Left == 0)
            //    infoPanel.Margin = new Thickness(200, 0, 0, 0);
            //else
            //    infoPanel.Margin = new Thickness(0, 0, 0, 0);
        }

        #endregion

        #region Indicator Context Menu

        /// <summary>
        /// Ação executada quando o usuairo clica em Excluir no context menu de indicador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuItemExcluir_Click_1(object sender, SourcedEventArgs e)
        {
            try
            {
                if (!IsSource((Series)((Indicator)((C1MenuItem)sender).Tag)))
                    _stockChartX.RemoveSeries(((Series)((Indicator)((C1MenuItem)sender).Tag)));
                else
                    MessageBox.Show("O indicador não pode ser removido pois é usado como fonte de dados para outra série. Exclua inicialmente a outra série para poder apagar esta segunda série em seguida.");

            }
            catch (Exception exc)
            {
                throw exc;
            }
            
        }

        /// <summary>
        /// Ação disparada quando o usuario clica em Configurar indicador no context menu de indicador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuItemConfigurar_Click_1(object sender, SourcedEventArgs e)
        {
            EditIndicator((Indicator)((C1MenuItem)sender).Tag);
        }

        /// <summary>
        /// Menu que faz a inserção de um indicador sobre outro indicador        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menuItemAddIndicatorOverIndicator_Click(object sender, SourcedEventArgs e)
        {
            //Recuperando o painel onde deve ser inserido o indicador
            ChartPanel chartPanel = ((Indicator)((C1MenuItem)sender).Tag).Panel;

            //pegando o source
            string Source = ((Indicator)((C1MenuItem)sender).Tag).FullName;

            //percorrendo e buscando o indicador na nossa lista o indicador
            IndicatorInfoDTO indicadorLocal = new IndicatorInfoDTO();
            foreach (IndicatorInfoDTO obj in StaticData.GetListaIndicadores())
            {
                if (obj.NomePortugues.ToUpper().Trim() == (((C1MenuItem)sender).Header.ToString().Trim().ToUpper()))
                {
                    indicadorLocal = obj;
                    break;
                }
            }

            //verificando se ele possui tipo de propriedade Serie, se possuir devemos setar como sendo
            //o indicador que fora selecionado
            foreach (IndicatorPropertyDTO obj in indicadorLocal.Propriedades)
            {
                if (obj.TipoDoCampo == TipoField.Serie)
                {
                    obj.Value = Source;
                }
            }

            //fazendo a inserção
            InserirIndicador(chartPanel, indicadorLocal.Propriedades, indicadorLocal, null);
        }

        /// <summary>
        /// Menu que faz a inserção de um indicador sobre outro indicador        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menuItemAddIndicatorOverSeries_Click(object sender, SourcedEventArgs e)
        {
            //Recuperando o painel onde deve ser inserido o indicador
            ChartPanel chartPanel = ((Series)((C1MenuItem)sender).Tag).Panel;

            //pegando o source
            string Source = ((Series)((C1MenuItem)sender).Tag).FullName;

            //percorrendo e buscando o indicador na nossa lista o indicador
            IndicatorInfoDTO indicadorLocal = new IndicatorInfoDTO();
            foreach (IndicatorInfoDTO obj in StaticData.GetListaIndicadores())
            {
                if (obj.NomePortugues.ToUpper().Trim() == (((C1MenuItem)sender).Header.ToString().Trim().ToUpper()))
                {
                    indicadorLocal = obj;
                    break;
                }
            }

            //verificando se ele possui tipo de propriedade Serie, se possuir devemos setar como sendo
            //o indicador que fora selecionado
            foreach (IndicatorPropertyDTO obj in indicadorLocal.Propriedades)
            {
                if (obj.TipoDoCampo == TipoField.Serie)
                {
                    obj.Value = Source;
                }
            }

            //fazendo a inserção
            InserirIndicador(chartPanel, indicadorLocal.Propriedades, indicadorLocal, null);
        }

        #endregion

        #endregion

        #region Metodos Privados

        #region StockChart

        /// <summary>
        /// Metodo que faz os post-processing do graifoc
        /// </summary>
        private void AplicaLayoutPostProcessing()
        {
            Thread.Sleep(200);

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {                
                //atuslizando status
                AtualizaStatus();

                _stockChartX.Update();
            });
        }

        /// <summary>
        /// Metodo que percorre paineis atualizando seus status
        /// </summary>
        private void AtualizaStatus()
        {

            foreach (TerminalWebSVC.PainelDTO obj in this.Layout.Paineis)
            {
                foreach (ChartPanel panel in _stockChartX.PanelsCollection)
                {

                    if (panel.Index == obj.Index)
                    {
                        if (obj.Status == "m")
                            panel.Minimize();
                        else if (obj.Status == "M")
                            panel.Maximize();

                        break;

                    }
                }
            }
        }

        /// <summary>
        /// Metodo que faz a atualização dos tamanhos dos paineis e seus status
        /// </summary>
        private void AtualizaTamanhoPaineis()
        {
            //Thread.Sleep(200);

            //Deployment.Current.Dispatcher.BeginInvoke(() =>
            //{
                List<double> listaNewSizes = new List<double>();

                //configura paineis
                foreach (TerminalWebSVC.PainelDTO obj in this.Layout.Paineis)
                {
                    listaNewSizes.Add(obj.Altura);
                }
            
            //alterando os tamanhos
               _stockChartX.SetPanelHeight(_stockChartX.PanelsCollection.ToList<ChartPanel>(), listaNewSizes);

               

            //});
        }

        /// <summary>
        /// Metodo que verifica se a serie passada esta sendo usada como source de algum chart
        /// </summary>
        /// <param name="objExcluido"></param>
        /// <returns></returns>
        private bool IsSource(Series objExcluido)
        {
            foreach (Indicator indicador in _stockChartX.IndicatorsCollection)
            {
                for (int i = 0; i < indicador.IndicatorParams.Count; i++)
                {
                    if ((indicador.IndicatorParams[i].ParameterType == ParameterType.ptSource) ||
                         (indicador.IndicatorParams[i].ParameterType == ParameterType.ptSource1) ||
                         (indicador.IndicatorParams[i].ParameterType == ParameterType.ptSource2) ||
                         (indicador.IndicatorParams[i].ParameterType == ParameterType.ptSource3))
                    {
            
                        if (indicador.ParamStr(i).ToUpper().Trim() == objExcluido.FullName.ToUpper().Trim())
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Metodo que retorna o ultimo preço plotado
        /// </summary>
        /// <returns></returns>
        private double LastPrice()
        {
            return _stockChartX.GetValue(_stockChartX.Symbol + ".CLOSE",
                        _stockChartX.GetTimestampByIndex(_stockChartX.RecordCount - 1).Value).Value;
        }

        /// <summary>
        /// Metodo responsavel por EDITAR o indicador
        /// </summary>
        /// <param name="e"></param>
        private void EditIndicator(Indicator e)
        {
            Indicator indicator = null;
            if (e.IsTwin)
                indicator = e.TwinsParentIndicator;
            else
                indicator = e;


            //montando a lista de propriedades que deve ser apresentada
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();
            IndicatorInfoDTO indicadorEditado = new IndicatorInfoDTO();

            //recuperando o indicador que deve ser editado
            foreach (IndicatorInfoDTO indicador in StaticData.GetListaIndicadores())
            {
                if (indicator.IndicatorType == indicador.TipoStockchart)
                {
                    indicadorEditado = indicador;
                    break;
                }
            }

            //varrendo os parametros do indicador
            foreach (IndicatorPropertyDTO obj in indicadorEditado.Propriedades)
            {
                if (obj.IndexStockChart >= 0)
                    obj.Value = indicator.GetParameterValue(obj.IndexStockChart);
            }

            List<string> listaSymbol = new List<string>();
            listaSymbol.Add(_stockChartX.Symbol);
            EditIndicator editIndicador = new EditIndicator(indicadorEditado,
                GetAllSeries(), listaSymbol, _stockChartX.RecordCount);

            bool showEditScreen = false;
            for (int i = 0; i < indicadorEditado.Propriedades.Count; i++)
            {
                if (indicadorEditado.Propriedades[i].TipoDoCampo != TipoField.SymbolList)
                {
                    showEditScreen = true;
                    break;
                }
            }
            if (showEditScreen)
            {
                editIndicador.Closing += (sender1, e1) =>
                {
                    if (editIndicador.DialogResult.Value == true)
                    {
                        //editando as propriedades
                        foreach (IndicatorPropertyDTO obj in editIndicador.listaPropriedades)
                        {
                            if (obj.TipoDoCampo != TipoField.SymbolList)
                                indicator.SetParameterValue(obj.IndexStockChart, obj.Value);
                        }
                        _stockChartX.Update();

                    }
                };
                editIndicador.Show();
            }
            else
                MessageBox.Show("Não existem parâmetros a serem editados.");
            
        }

        /// <summary>
        /// Metodo que retorna todas as series, dado que os indicadores nao retornam por default na SeriesCollection
        /// </summary>
        public List<Series> GetAllSeries()
        {
            List<Series> listaTemp = new List<Series>();
            foreach (Series obj in _stockChartX.SeriesCollection.ToList<Series>())
            {
                listaTemp.Add(obj);
            }
            foreach (Indicator obj in _stockChartX.IndicatorsCollection.ToList<Indicator>())
            {
                listaTemp.Add(obj);
            }

            return listaTemp;
        }

        /// <summary>
        /// Metodo que faz o refresh do gráfico
        /// </summary>
        private void RefreshLayout()
        {
            //TODO: Existe um chamado aberto na modulus para verificar este funcionamento que está incorreto
            foreach (Series obj in _stockChartX.SeriesCollection)
            {
                if (obj.TickBox == TickBoxPosition.Right)
                {
                    obj.TickBox = TickBoxPosition.None;
                    obj.TickBox = TickBoxPosition.Right;
                }
            }
        }

        /// <summary>
        /// Metodo que faz as configurações iniciais de layout
        /// </summary>
        /// <param name="layout"></param>
        private void CarregaGrafico(TerminalWebSVC.LayoutDTO layout, string ativo, List<CotacaoDTO> listaCotacao, bool changePeriodicity)
        {
            try
            {
                //Retirando todas as series do gráfico 
                _stockChartX.ApplyTemplate();
                _stockChartX.ClearAll();
                _stockChartX.ClearMinimizeBar();

                //setando o ultimo volume da semana
                if (this.Periodicidade == TerminalWEB.Periodicidade.Semanal)
                {
                    List<CotacaoDTO> listaTemp = StaticData.cacheCotacaoDiario[ativo];
                    volumeSemanaMenosDia = listaCotacao[listaCotacao.Count - 1].Volume;
                }

                //setando o ultimo volume do mes
                if (this.Periodicidade == TerminalWEB.Periodicidade.Mensal)
                {
                    volumeMesMenosDia = listaCotacao[listaCotacao.Count - 1].Volume;
                }

                //setando o volume se for intraday
                if (Intraday())
                {
                    volumeIntradayMultiplaPeriodicidade = listaCotacao[listaCotacao.Count - 1].Volume;
                }

                //setando o ativo
                _stockChartX.Symbol = ativo;

                //desabilitando o zoom area
                _stockChartX.DisableZoomArea = true;

                //apresentando os titles dos paineis
                _stockChartX.DisplayTitles = true;


                //setando o tipo de calendario
                _stockChartX.CalendarVersion = CalendarVersionType.Version1;

                _stockChartX.IndicatorTwinTitleVisibility = System.Windows.Visibility.Collapsed;


                //Criando paineis padrões
                ChartPanel topPanel = _stockChartX.AddChartPanel(ChartPanel.PositionType.AlwaysTop);
                ChartPanel volumePanel = _stockChartX.AddChartPanel(ChartPanel.PositionType.AlwaysBottom);


                //omitindo o botão de fechamento dos paineis
                topPanel.CloseBox = false;
                volumePanel.CloseBox = false;

                
                    

                //criando paineis extras
                foreach (TerminalWebSVC.PainelDTO obj in layout.Paineis)
                {
                    if ((obj.TipoPainel != "P") && (obj.TipoPainel != "V"))
                    {
                        ChartPanel newPanel = _stockChartX.AddChartPanel(ChartPanel.PositionType.AlwaysTop);
                        //newPanel.CloseBox = false;
                    }
                }


                //setando os nomes
                topPanel.Name = "Preços";
                volumePanel.Name = "Volume";

                //setando as cores de fundo
                topPanel.Background = volumePanel.Background = GeneralUtil.GetColorFromHexa(layout.CorFundo);

                //adicionando series
                Series[] ohlcSeries = _stockChartX.AddOHLCSeries(_stockChartX.Symbol, topPanel.Index);
                Series seriesVolume = _stockChartX.AddVolumeSeries(_stockChartX.Symbol, volumePanel.Index);
                seriesVolume.EditSerieInChartPanel += seriesVolume_EditSerieInChartPanel;
                foreach (CotacaoDTO obj in listaCotacao)
                {
                    if (this.Intraday())
                    {
                        if (this.afterMarket)
                        {
                            _stockChartX.AppendOHLCValues(_stockChartX.Symbol, obj.Data, obj.Abertura, obj.Maximo, obj.Minimo, obj.Ultimo);
                            _stockChartX.AppendVolumeValue(_stockChartX.Symbol, obj.Data, obj.Volume);
                        }
                        else
                        {
                            if (!obj.AfterMarket)
                            {
                                _stockChartX.AppendOHLCValues(_stockChartX.Symbol, obj.Data, obj.Abertura, obj.Maximo, obj.Minimo, obj.Ultimo);
                                _stockChartX.AppendVolumeValue(_stockChartX.Symbol, obj.Data, obj.Volume);
                            }
                        }
                    }
                    else
                    {
                        _stockChartX.AppendOHLCValues(_stockChartX.Symbol, obj.Data.Date, obj.Abertura, obj.Maximo, obj.Minimo, obj.Ultimo);
                        _stockChartX.AppendVolumeValue(_stockChartX.Symbol, obj.Data.Date, obj.Volume);
                    }

                }

                //setando o tickbox em cima doa valor do ultimo na direita
                ohlcSeries[3].TickBox = TickBoxPosition.Right;

                //setando a cor
                ohlcSeries[0].StrokeColor = Colors.Black;
                ohlcSeries[1].StrokeColor = Colors.Black;
                ohlcSeries[2].StrokeColor = Colors.Black;
                ohlcSeries[3].StrokeColor = Colors.Black;

                //caracteristicas da escala
                _stockChartX.ScaleAlignment = (ScaleAlignmentTypeEnum)layout.PosicaoEscala.Value;

                //Realtime ou nao            
                _stockChartX.RealTimeXLabels = this.Intraday();

                //Checando a compressao das barras
                //_stockChartX.TickCompressionType = TickCompressionEnum.Time;

                //switch (this.Periodicidade)
                //{
                //    case Periodicidade.UmMinuto:
                //        _stockChartX.TickPeriodicity = 60;
                //        break;
                //    case Periodicidade.DoisMinutos:
                //        _stockChartX.TickPeriodicity = 120;
                //        break;
                //    case Periodicidade.TresMinutos:
                //        _stockChartX.TickPeriodicity = 180;
                //        break;
                //    case Periodicidade.CincoMinutos:
                //        _stockChartX.TickPeriodicity = 300;
                //        break;
                //}


                //setando as caracteristicas da Scrollbar
                int visibleRecords = 0;
                if (GeneralUtil.LayoutFake().VisibleRecords > _stockChartX.RecordCount)
                    visibleRecords = _stockChartX.RecordCount;
                else
                    visibleRecords = GeneralUtil.LayoutFake().VisibleRecords;

                scrollbar.ViewportSize = visibleRecords;
                scrollbar.Maximum = _stockChartX.RecordCount;
                scrollbar.Minimum = visibleRecords;

                if (GeneralUtil.LayoutFake().ZoomRealtime)
                {
                    _stockChartX.FirstVisibleRecord = _stockChartX.RecordCount - visibleRecords;
                    _stockChartX.LastVisibleRecord = _stockChartX.FirstVisibleRecord + visibleRecords;
                    scrollbar.Value = scrollbar.Maximum;
                }
                else
                {
                    _stockChartX.FirstVisibleRecord = visibleRecords;
                    _stockChartX.LastVisibleRecord = _stockChartX.FirstVisibleRecord + visibleRecords;
                    scrollbar.Value = _stockChartX.LastVisibleRecord;
                }


                _stockChartX.OptimizePainting = true;
                _stockChartX.Update();

                //aplicando as configurações finais
                if (changePeriodicity)
                    AplicaLayout(layout, true, false, true);
                else
                    AplicaLayout(layout, true, true, false);

                //setando o estado do gráfico para aguardando alguma operação
                this.CurrentState = EstadoGrafio.Nenhum;

                TextBlock lbl = _stockChartX.GetPanelByIndex(0).ChartPanelLabel;
                lbl.FontSize = 30;
                lbl.Foreground = Brushes.Black;
                lbl.Foreground.Opacity = 0.3;
                lbl.Text = StaticData.WaterMark;
                lbl.Visibility = System.Windows.Visibility.Visible;


                _stockChartX.GetPanelByIndex(0).ChartPanelLabel.Margin = new Thickness((_stockChartX.ActualWidth / 2) - (_stockChartX.GetPanelByIndex(0).ChartPanelLabel.ActualWidth / 2)
                    , 0, 0, 0);


                timerUpdate.Start();
                busyIndicator.IsBusy = false;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        

        /// <summary>
        /// Metodo que faz a atualização do infopanel de acordo com o posicionamento do mouse
        /// </summary>
        private void AtualizaInfoPanel()
        {
            
            if ((registroX > 0) && (registroX < _stockChartX.RecordCount))
            {
                if (this.Intraday())
                    lblDataHora.Text = "Data: " + _stockChartX.GetTimestampByIndex(registroX).Value.ToString("dd/MM/yyyy HH:mm");
                else
                    lblDataHora.Text = "Data: " + _stockChartX.GetTimestampByIndex(registroX).Value.ToString("dd/MM/yyyy");
                lblMinimo.Text = "Min: " + _stockChartX.GetValue(_stockChartX.Symbol + ".low", registroX).Value.ToString("0.00", GeneralUtil.NumberProvider);
                lblAbertura.Text = "Abe: " + _stockChartX.GetValue(_stockChartX.Symbol + ".open", registroX).Value.ToString("0.00", GeneralUtil.NumberProvider);
                lblMaximo.Text = "Max: " + _stockChartX.GetValue(_stockChartX.Symbol + ".high", registroX).Value.ToString("0.00", GeneralUtil.NumberProvider);
                lblUltimo.Text = "Ult: " + _stockChartX.GetValue(_stockChartX.Symbol + ".close", registroX).Value.ToString("0.00", GeneralUtil.NumberProvider);
            }
            
        }

        #endregion

        #region Atualizando dados

        /// <summary>
        /// Metodo que faz a atualização da barra diaria
        /// </summary>
        /// <param name="ultimaBarra"></param>
        /// <param name="tick"></param>
        private void AtualizaGraficoDiario(DateTime ultimaBarra, TickDTO tick)
        {
            if (ultimaBarra.Date == tick.Data.Date)
            {
                int record = _stockChartX.RecordCount - 1;
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".open",
                    record, tick.Abertura);
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".high",
                    record, tick.Maximo);
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".low",
                    record, tick.Minimo);
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".close",
                    record, tick.Ultimo);
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".volume",
                    record, tick.Volume);
            }
            else
            {
                _stockChartX.AppendOHLCVValues(_stockChartX.Symbol, tick.Data.Date, tick.Abertura, tick.Maximo,
                    tick.Minimo, tick.Ultimo, tick.Volume, false);
            }

            this.RefreshLayout();
            
        }

        /// <summary>
        /// Metodo que faz a atualização da barra semanal
        /// </summary>
        /// <param name="ultimaBarra"></param>
        /// <param name="tick"></param>
        private void AtualizaGraficoSemanal(DateTime ultimaBarra, TickDTO tick)
        {
            if (tick.Data.Date.Subtract(ultimaBarra.Date.Date) < new TimeSpan(7,0,0,0))
            {
                //ultimo
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".close",
                       _stockChartX.RecordCount - 1, tick.Ultimo);

                //maximo                
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".high",
                    _stockChartX.RecordCount - 1, Math.Max(tick.Maximo,_stockChartX.GetValue(_stockChartX.Symbol+".high",_stockChartX.RecordCount-1).Value));

                //minimo
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".low",
                    _stockChartX.RecordCount - 1, Math.Min(tick.Minimo, _stockChartX.GetValue(_stockChartX.Symbol + ".low", _stockChartX.RecordCount - 1).Value));
                
                //volume deve ser calculado d seguinte forma:
                //volume do dia (vem do tick em RT ou delay) + volume da semana menos o volume do dia                
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".volume",
                    _stockChartX.RecordCount - 1, volumeSemanaMenosDia + tick.Volume);
            }
            else
            {
                volumeSemanaMenosDia = 0;
                _stockChartX.AppendOHLCVValues(_stockChartX.Symbol, tick.Data.Date, tick.Abertura, tick.Maximo,
                    tick.Minimo, tick.Ultimo, tick.Volume, false);
            }

            this.RefreshLayout();
        }

        /// <summary>
        /// Metodo que faz a atualização da barra mensal
        /// </summary>
        /// <param name="ultimaBarra"></param>
        /// <param name="tick"></param>
        private void AtualizaGraficoMensal(DateTime ultimaBarra, TickDTO tick)
        {
            if (tick.Data.Date.Month <= ultimaBarra.Date.Month)
            {
                //ultimo
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".close",
                       _stockChartX.RecordCount - 1, tick.Ultimo);

                //maximo                
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".high",
                    _stockChartX.RecordCount - 1, Math.Max(tick.Maximo, _stockChartX.GetValue(_stockChartX.Symbol + ".high", _stockChartX.RecordCount - 1).Value));

                //minimo
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".low",
                    _stockChartX.RecordCount - 1, Math.Min(tick.Minimo, _stockChartX.GetValue(_stockChartX.Symbol + ".low", _stockChartX.RecordCount - 1).Value));

                //volume deve ser calculado d seguinte forma:
                //volume do dia (vem do tick em RT ou delay) + volume da semana menos o volume do dia                
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".volume",
                    _stockChartX.RecordCount - 1, volumeMesMenosDia + tick.Volume);
            }
            else
            {
                volumeMesMenosDia = 0;
                _stockChartX.AppendOHLCVValues(_stockChartX.Symbol, tick.Data.Date, tick.Abertura, tick.Maximo,
                    tick.Minimo, tick.Ultimo, tick.Volume, false);
            }

            this.RefreshLayout();
        }

        /// <summary>
        /// Metodo que faz a atualização da barra intraday de 1 minuto
        /// </summary>
        /// <param name="ultimaBarra"></param>
        /// <param name="tick"></param>
        private void AtualizaGraficoIntraday(DateTime ultimaBarra, TickDTO tick, int periodicidade)
        {
            DateTime tickDateCompleta = new DateTime(tick.Data.Year, tick.Data.Month, tick.Data.Day, 
                Convert.ToInt32(tick.Hora.Substring(0,2)), Convert.ToInt32(tick.Hora.Substring(2,2)),0);
            if (tickDateCompleta.Subtract(new TimeSpan(0,periodicidade-1,0)) > ultimaBarra)
            {
                _stockChartX.AppendOHLCVValues(_stockChartX.Symbol, tickDateCompleta, tick.Ultimo, tick.Ultimo,
                tick.Ultimo, tick.Ultimo, tick.VolumeUltimoMinuto, false);

            }
            else

            {
                //int index = _stockChartX.GetReverseX(ultimaBarra, false);
                int record = _stockChartX.RecordCount - 1;

                //maximo                
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".high", record, Math.Max(tick.Ultimo, _stockChartX.GetValue(_stockChartX.Symbol + ".high", record).Value));

                //minimo
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".low", record, Math.Min(tick.Ultimo, _stockChartX.GetValue(_stockChartX.Symbol + ".low", record).Value));

                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".close", record, tick.Ultimo);

                if (periodicidade == 1)
                {
                    _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".volume", record, tick.VolumeUltimoMinuto);
                }
                else
                {
                    double volume = 0;
                    //calculando o volume
                    foreach (CotacaoDTO obj in StaticData.cacheCotacaoIntraday[ativo])
                    {                        
                        if ((obj.Data >= ultimaBarra) && (obj.Data < ultimaBarra.AddMinutes(periodicidade)))
                        {
                            volume += obj.Volume;
                        }

                    }

                    _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".volume", record,  volume);

                    
                }
            }
            
        }

        #endregion

        #region Layouts

        /// <summary>
        /// Metodo que clona o objeto de layout
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        public TerminalWebSVC.LayoutDTO CloneLayout(TerminalWebSVC.LayoutDTO layout, bool indicadores, bool objetos)
        {
            TerminalWebSVC.LayoutDTO obj = new TerminalWebSVC.LayoutDTO();
            obj.CorBordaCandleAlta = layout.CorBordaCandleAlta;
            obj.CorBordaCandleBaixa = layout.CorBordaCandleBaixa;
            obj.CorCandleAlta = layout.CorCandleAlta;
            obj.CorCandleBaixa = layout.CorCandleBaixa;
            obj.CorEscala = layout.CorEscala;
            obj.CorFundo = layout.CorFundo;
            obj.CorGrid = layout.CorGrid;
            obj.CorVolume = layout.CorVolume;
            obj.DarvaBox = layout.DarvaBox;
            obj.EspacoADireitaDoGrafico = layout.EspacoADireitaDoGrafico;
            obj.EspacoAEsquerdaDoGrafico = layout.EspacoAEsquerdaDoGrafico;
            obj.EstiloBarra = layout.EstiloBarra;
            obj.EstiloPreco = layout.EstiloPreco;
            obj.EstiloPrecoParam1 = layout.EstiloPrecoParam1;
            obj.EstiloPrecoParam2 = layout.EstiloPrecoParam2;
            obj.FirstVisibleRecord = layout.FirstVisibleRecord;
            obj.GradeHorizontal = layout.GradeHorizontal;
            obj.GradeVertical = layout.GradeVertical;
            obj.GraficoId = layout.GraficoId;
            obj.Id = layout.Id;
            if (indicadores)
                obj.Indicadores = layout.Indicadores;
            else
                obj.Indicadores =new List<TerminalWebSVC.IndicadorDTO>();
            if (objetos)
                obj.Objetos = layout.Objetos;
            else
                obj.Objetos = new List<TerminalWebSVC.ObjetoEstudoDTO>();
            
            obj.Paineis = new List<TerminalWebSVC.PainelDTO>();
            foreach (TerminalWebSVC.PainelDTO panel in layout.Paineis)
            {
                //panel.Status = "N";
                obj.Paineis.Add(panel);
            }
            obj.PainelInfo = layout.PainelInfo;
            obj.Periodicidade = layout.Periodicidade;
            obj.PosicaoEscala = layout.PosicaoEscala;
            obj.PrecisaoEscala = layout.PrecisaoEscala;
            obj.TemplateId = layout.TemplateId;
            obj.TipoEscala = layout.TipoEscala;
            obj.TipoVolume = layout.TipoVolume;
            obj.UsarCoresAltaBaixaVolume = layout.UsarCoresAltaBaixaVolume;
            obj.VisibleRecords = layout.VisibleRecords;
            obj.VolumeStrokeThickness = layout.VolumeStrokeThickness;
            obj.ZoomRealtime = layout.ZoomRealtime;

            return obj;
        }

        #endregion

        #endregion
        
        #region Metodos Publicos

        #region Cross

        /// <summary>
        /// Metodo que deve desabilitar o cross no gráfico corrente
        /// </summary>
        public void DesabilitaCross()
        {
            _stockChartX.CrossHairs = false;
            _stockChartX.Cursor = Cursors.Arrow;
            borderValorYPosicionado.Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion

        #region Refresh Dados & Periodicidade

        /// <summary>
        /// Metodo que retorna as cotações e recarrega o gráfico
        /// </summary>
        public void Refresh()
        {
            SetPeriodicidade(this.Periodicidade, true, null);
        }

        /// <summary>
        /// Metodo que retorna as cotações e recarrega o gráfico
        /// </summary>
        public void Refresh(TerminalWebSVC.LayoutDTO layout)
        {
            timerUpdate.Stop();
            SetPeriodicidade(this.Periodicidade, true, layout);
        }

        /// <summary>
        /// Metodo que retorna se o gráfico é intradya ou nao
        /// </summary>
        private bool Intraday()
        {
            switch (Periodicidade)
            {
                case Periodicidade.Diario:
                case Periodicidade.Mensal:
                case Periodicidade.Semanal:
                    return false;                    
                default:
                    return true;                    
            }
        }

        /// <summary>
        /// Set periodicidade sem forçar um refresh
        /// </summary>
        /// <param name="periodicidade"></param>
        public void SetPeriodicidade(Periodicidade periodicidade)
        {
            this.SetPeriodicidade(periodicidade, false, null);
        }

        /// <summary>
        /// Metodo que faz o refresh alterando a periodicidade
        /// </summary>
        /// <param name="periodicidade"></param>
        public void SetPeriodicidade(Periodicidade periodicidade, bool forceRefresh, TerminalWebSVC.LayoutDTO layout)
        {
            //setando o busy para on
            busyIndicator.IsBusy = true;

            this.Periodicidade = periodicidade;
            if (Dirty)
            {
                if (layout == null)
                    this.Layout = GetLayoutDTOFromStockchart();
                

                //armazenando objetos
                switch (this.PeriodicidadeAnterior)
                {
                    case TerminalWEB.Periodicidade.UmMinuto:
                        objetos1Minuto = GetObjetos();
                        break;
                    case TerminalWEB.Periodicidade.Diario:
                        objetosDiario = GetObjetos();
                        break;
                    case TerminalWEB.Periodicidade.Semanal:
                        objetosSemanal = GetObjetos();
                        break;
                    case TerminalWEB.Periodicidade.Mensal:
                        objetosMensal = GetObjetos();
                        break;
                }
            }
            else
            {

            }

            if (Intraday())
                marketDataDAO.GetCotacaoIntradayAsync(this.ativo, true, forceRefresh);
            else
                marketDataDAO.GetCotacaoDiariaAsync(this.ativo);
        }

        /// <summary>
        /// Metodo que retorna a periodicidade do gráfico
        /// </summary>
        /// <returns></returns>
        public Periodicidade GetPeriodicidade()
        {
            return this.Periodicidade;
        }

        #endregion

        #region Tipo de Barra

        /// <summary>
        /// Metodo que vai setar o tipo de serie presente no gráfico
        /// </summary>
        /// <param name="tipoSerie"></param>
        public void SetTipoBarra(SeriesTypeEnum tipoSerie)
        {
            try
            {
                foreach (ChartPanel obj in _stockChartX.PanelsCollection.ToList<ChartPanel>())
                {
                    foreach (Series series in obj.SeriesCollection.ToList<Series>())
                    {
                        if ((series.SeriesType == SeriesTypeEnum.stCandleChart) ||
                             (series.SeriesType == SeriesTypeEnum.stLineChart) ||
                            (series.SeriesType == SeriesTypeEnum.stStockBarChart) ||
                            (series.SeriesType == SeriesTypeEnum.stStockBarChartHLC))
                        {
                            series.SeriesType = tipoSerie;                                    
                        }
                    }
                }
                _stockChartX.Update();
            }
            catch (Exception exc)
            {                
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna o tipo de barra
        /// </summary>
        public SeriesTypeEnum GetTipoBarra()
        {
            return _stockChartX.PanelsCollection.ToList<ChartPanel>()[0].SeriesCollection.ToList<Series>()[0].SeriesType;
        }

        #endregion

        #region Escala


        /// <summary>
        /// Metodo que vai setar o tipo de escala no gráfico
        /// </summary>
        /// <param name="tipoSerie"></param>
        public void SetTipoEscala(ScalingTypeEnum tipoEscala)
        {
            try
            {
                _stockChartX.ScalingType = tipoEscala;
                //_stockChartX.SetYScale(0, 24, 0.24);
                _stockChartX.Update();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que vai retornar o tipo de escala no gráfico
        /// </summary>
        /// <param name="tipoSerie"></param>
        public ScalingTypeEnum GetTipoEscala()
        {
            try
            {
                return _stockChartX.ScalingType;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        #endregion

        #region Skins

        /// <summary>
        /// Metodo que seta o skin preto no gráfico
        /// </summary>
        public void SetSkinPreto()
        {
            // Criando um gradiente para a barra divisora
            LinearGradientBrush brushDivisorPreto = new LinearGradientBrush();
            brushDivisorPreto.StartPoint = new Point(0, 0);
            brushDivisorPreto.EndPoint = new Point(0, 10);

            // Create and add Gradient stops
            GradientStop GrayGS = new GradientStop();
            GrayGS.Color = Colors.Gray;
            GrayGS.Offset = 0.0;
            brushDivisorPreto.GradientStops.Add(GrayGS);

            GradientStop balckGS2 = new GradientStop();
            balckGS2.Color = Colors.Black;
            balckGS2.Offset = 1;

            brushDivisorPreto.GradientStops.Add(balckGS2);

            _stockChartX.Background = Brushes.Black;
            foreach (ChartPanel obj in _stockChartX.PanelsCollection.ToList<ChartPanel>())
            {
                obj.Background = Brushes.Black;
                obj.YAxesBackground = Brushes.Black;
                obj.Foreground = Brushes.White;
                obj.TitleBarBackground = brushDivisorPreto;

            }
            _stockChartX.FontForeground = Brushes.White;
            _stockChartX.ThreeDStyle = true;
            _stockChartX.CandleDownOutlineColor = null;
            _stockChartX.CandleUpOutlineColor = null;
            _stockChartX.UpColor = Colors.Green;
            _stockChartX.DownColor = Colors.Red;

            gridPrincipal.Background = Brushes.Black;
            canvasAbaixoStockchart.Background = Brushes.Black;
            _stockChartX.XGrid = true;
            _stockChartX.YGrid = true;
            this.Background = Brushes.Black;            
        }
        
        /// <summary>
        /// Metodo que seta o skin de cores padrão branco
        /// </summary>
        public void SetSkinBranco()
        {
            _stockChartX.Background = Brushes.White;
            // Criando um gradiente para a barra divisora
            LinearGradientBrush brushDivisorBranco = new LinearGradientBrush();
            brushDivisorBranco.StartPoint = new Point(0, 0);
            brushDivisorBranco.EndPoint = new Point(0, 10);

            // Create and add Gradient stops
            GradientStop whiteGS = new GradientStop();
            whiteGS.Color = Colors.White;
            whiteGS.Offset = 0.0;
            brushDivisorBranco.GradientStops.Add(whiteGS);

            GradientStop balckGS = new GradientStop();
            balckGS.Color = Colors.Black;
            balckGS.Offset = 1;
            
            brushDivisorBranco.GradientStops.Add(balckGS);

            foreach (ChartPanel obj in _stockChartX.PanelsCollection.ToList<ChartPanel>())
            {
                obj.Background = Brushes.White;
                obj.YAxesBackground = Brushes.White;
                obj.Foreground = Brushes.Black;
                obj.TitleBarBackground = brushDivisorBranco;
            }
            _stockChartX.FontForeground = Brushes.DarkRed;
            _stockChartX.CandleDownOutlineColor = null;
            _stockChartX.CandleUpOutlineColor = null;
            _stockChartX.UpColor = ColorsEx.Lime;
            _stockChartX.DownColor = Colors.Red;
            _stockChartX.ThreeDStyle = true;

            gridPrincipal.Background = Brushes.White;
            canvasAbaixoStockchart.Background = Brushes.White;
            _stockChartX.XGrid = true;
            _stockChartX.YGrid = true;
            this.Background = Brushes.White;            
        }

        /// <summary>
        /// Seta um skin de cores como o Enfoque
        /// </summary>
        public void SetSkinPretoBranco()
        {
            //setando as configuraçoes do cross
            _stockChartX.CrossHairsStroke = Brushes.DarkRed;
            borderValorYPosicionado.Background = Brushes.Red;
            txtValorYPosicionado.Foreground = Brushes.White;
            
            _stockChartX.Background = Brushes.White;
            // Criando um gradiente para a barra divisora
            LinearGradientBrush brushDivisorBranco = new LinearGradientBrush();
            brushDivisorBranco.StartPoint = new Point(0, 0);
            brushDivisorBranco.EndPoint = new Point(0, 10);

            // Create and add Gradient stops
            GradientStop whiteGS = new GradientStop();
            whiteGS.Color = Colors.White;
            whiteGS.Offset = 0.0;
            brushDivisorBranco.GradientStops.Add(whiteGS);

            GradientStop balckGS = new GradientStop();
            balckGS.Color = Colors.Black;
            balckGS.Offset = 1;

            brushDivisorBranco.GradientStops.Add(balckGS);
            foreach (ChartPanel obj in _stockChartX.PanelsCollection.ToList<ChartPanel>())
            {
                obj.Background = Brushes.White;
                obj.YAxesBackground = Brushes.White;
                obj.Foreground = Brushes.Black;
                obj.TitleBarBackground = brushDivisorBranco;
                
                //percorrendo as series
                foreach (Series serie in obj.SeriesCollection.ToList<Series>())
                {
                    serie.StrokeColor = Colors.Black;
                }
            }
            _stockChartX.FontForeground = Brushes.DarkRed;
            _stockChartX.ThreeDStyle = false;
            _stockChartX.CandleDownOutlineColor = Colors.Black;
            _stockChartX.CandleUpOutlineColor = Colors.Black;
            _stockChartX.UpColor = Colors.White;
            _stockChartX.DownColor = Colors.Black;
            _stockChartX.CalendarBackground = Brushes.White;
            gridPrincipal.Background = Brushes.White;
            canvasAbaixoStockchart.Background = Brushes.White;
            _stockChartX.XGrid = true;
            _stockChartX.YGrid = true;
            this.Background = Brushes.White;            
        }

        /// <summary>
        /// Seta um skin de cores customizado
        /// </summary>
        public void AplicaLayout(TerminalWebSVC.LayoutDTO layout, bool insereIndicadores,bool insereObjetosFromLayout, bool insereObjetosFromPeriodicityList)
        {
            try
            {                

                //setando o layout local
                this.Layout = layout;

                //setando a cor do grid
                _stockChartX.GridStroke = GeneralUtil.GetColorFromHexa(layout.CorGrid);

                //setando a cor de fundo
                _stockChartX.Background = GeneralUtil.GetColorFromHexa(layout.CorFundo);

                //setando a cor da escala (Foregorund)
                _stockChartX.FontForeground = GeneralUtil.GetColorFromHexa(layout.CorEscala);

                //setando as configuraçoes do cross
                _stockChartX.CrossHairsStroke = Brushes.DarkRed;
                borderValorYPosicionado.Background = Brushes.Red;
                txtValorYPosicionado.Foreground = Brushes.White;
                


                // Criando um gradiente para a barra divisora
                LinearGradientBrush brushDivisorBranco = new LinearGradientBrush();
                brushDivisorBranco.StartPoint = new Point(0, 0);
                brushDivisorBranco.EndPoint = new Point(0, 5);

                // Create and add Gradient stops
                GradientStop whiteGS = new GradientStop();
                whiteGS.Color = Colors.White;
                whiteGS.Offset = 0.0;
                brushDivisorBranco.GradientStops.Add(whiteGS);

                GradientStop balckGS = new GradientStop();
                balckGS.Color = Colors.Black;
                balckGS.Offset = 1;

                brushDivisorBranco.GradientStops.Add(balckGS);
                foreach (ChartPanel obj in _stockChartX.PanelsCollection.ToList<ChartPanel>())
                {
                    obj.Background = GeneralUtil.GetColorFromHexa(layout.CorFundo);
                    obj.YAxesBackground = GeneralUtil.GetColorFromHexa(layout.CorFundo);
                    obj.Foreground = Brushes.Black;
                    obj.TitleBarBackground = brushDivisorBranco;

                    //percorrendo as series para encontrar o volume
                    foreach (Series serie in obj.SeriesCollection.ToList<Series>())
                    {
                        if (serie.OHLCType == SeriesTypeOHLC.Volume)
                        {
                            serie.StrokeColor = GeneralUtil.GetColorFromHexa(layout.CorVolume).Color;
                            serie.StrokeThickness = layout.VolumeStrokeThickness;
                        }
                    }
                }


                _stockChartX.ThreeDStyle = false;
                _stockChartX.CandleDownOutlineColor = GeneralUtil.GetColorFromHexa(layout.CorBordaCandleBaixa).Color;
                _stockChartX.CandleDownWickMatchesOutlineColor = true;
                _stockChartX.CandleUpOutlineColor = GeneralUtil.GetColorFromHexa(layout.CorBordaCandleAlta).Color;
                _stockChartX.CandleUpWickMatchesOutlineColor = true;
                _stockChartX.UpColor = GeneralUtil.GetColorFromHexa(layout.CorCandleAlta).Color;
                _stockChartX.DownColor = GeneralUtil.GetColorFromHexa(layout.CorCandleBaixa).Color;
                _stockChartX.CalendarBackground = GeneralUtil.GetColorFromHexa(layout.CorFundo);
                gridPrincipal.Background = GeneralUtil.GetColorFromHexa(layout.CorFundo);
                canvasAbaixoStockchart.Background = GeneralUtil.GetColorFromHexa(layout.CorFundo);
               // _stockChartX.LeftChartSpace = layout.EspacoAEsquerdaDoGrafico.Value;
                _stockChartX.RightChartSpace = layout.EspacoADireitaDoGrafico.Value;

                _stockChartX.XGrid = (bool)layout.GradeHorizontal;
                _stockChartX.YGrid = (bool)layout.GradeVertical;

                if (this.Intraday())                
                    _stockChartX.VolumeDivisor = 1;
                else
                    _stockChartX.VolumeDivisor = 1000000;


                if (layout.PosicaoEscala == 2)
                    _stockChartX.ScaleAlignment = ScaleAlignmentTypeEnum.Left;
                else
                    _stockChartX.ScaleAlignment = ScaleAlignmentTypeEnum.Right;

                _stockChartX.ScalePrecision = Convert.ToInt32(layout.PrecisaoEscala);

                this.Background = GeneralUtil.GetColorFromHexa(layout.CorFundo);

                _stockChartX.UseVolumeUpDownColors = (bool)layout.UsarCoresAltaBaixaVolume;

                //setando info panel
                //if ((bool)layout.PainelInfo)
                //    this.infoPanel.Visibility = System.Windows.Visibility.Visible;
                //else
                //    this.infoPanel.Visibility = System.Windows.Visibility.Collapsed;

                //setando o tipo de escala
                SetTipoEscala((ScalingTypeEnum)Layout.TipoEscala);

                //setando o estilo das barras
                SetTipoBarra((SeriesTypeEnum)Layout.EstiloBarra);

                _stockChartX.Update();

                if (insereIndicadores)
                {
                    //recuperando indicadores que obtiveram falhas
                    foreach (TerminalWebSVC.IndicadorDTO obj in listaIndicadoresComErro)
                    {
                        if (obj != null)
                            layout.Indicadores.Add(obj);
                    }
                    listaIndicadoresComErro.Clear();

                    //inserindo os indicadores
                    InsereIndicadores(layout.Indicadores);

                }

                if (insereObjetosFromLayout)
                {
                    //inserindo os indicadores
                    InsereObjetos(layout.Objetos);
                }

                if (insereObjetosFromPeriodicityList)
                {
                    switch (this.Periodicidade)
                    {
                        case TerminalWEB.Periodicidade.UmMinuto:
                            InsereObjetos(objetos1Minuto);
                            break;
                        case TerminalWEB.Periodicidade.Diario:
                            InsereObjetos(objetosDiario);
                            break;
                        case TerminalWEB.Periodicidade.Semanal:
                            InsereObjetos(objetosSemanal);
                            break;
                        case TerminalWEB.Periodicidade.Mensal:
                            InsereObjetos(objetosMensal);
                            break;
                    }



                }

                //abrindop thread para executar
                AtualizaTamanhoPaineis();

                //fazendo o update
                _stockChartX.Update();

                //Executando thread de post processing para layout
                Thread threadPostProcessingLayout = new Thread(new ThreadStart(AplicaLayoutPostProcessing));
                threadPostProcessingLayout.Start();

            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /// <summary>
        /// Seta um skin de cores customizado
        /// </summary>
        public void AplicaLayout2(TerminalWebSVC.LayoutDTO layout)
        {
            try
            {
                _stockChartX.ClearAll();

                //setando o layout local
                this.Layout = layout;

                //setando a cor do grid
                _stockChartX.GridStroke = GeneralUtil.GetColorFromHexa(layout.CorGrid);

                //setando a cor de fundo
                _stockChartX.Background = GeneralUtil.GetColorFromHexa(layout.CorFundo);

                //setando a cor da escala (Foregorund)
                _stockChartX.FontForeground = GeneralUtil.GetColorFromHexa(layout.CorEscala);

                //setando as configuraçoes do cross
                _stockChartX.CrossHairsStroke = Brushes.DarkRed;
                borderValorYPosicionado.Background = Brushes.Red;
                txtValorYPosicionado.Foreground = Brushes.White;



                // Criando um gradiente para a barra divisora
                LinearGradientBrush brushDivisorBranco = new LinearGradientBrush();
                brushDivisorBranco.StartPoint = new Point(0, 0);
                brushDivisorBranco.EndPoint = new Point(0, 5);

                // Create and add Gradient stops
                GradientStop whiteGS = new GradientStop();
                whiteGS.Color = Colors.White;
                whiteGS.Offset = 0.0;
                brushDivisorBranco.GradientStops.Add(whiteGS);

                GradientStop balckGS = new GradientStop();
                balckGS.Color = Colors.Black;
                balckGS.Offset = 1;

                brushDivisorBranco.GradientStops.Add(balckGS);
                foreach (ChartPanel obj in _stockChartX.PanelsCollection.ToList<ChartPanel>())
                {
                    obj.Background = GeneralUtil.GetColorFromHexa(layout.CorFundo);
                    obj.YAxesBackground = GeneralUtil.GetColorFromHexa(layout.CorFundo);
                    obj.Foreground = Brushes.Black;
                    obj.TitleBarBackground = brushDivisorBranco;

                    //percorrendo as series para encontrar o volume
                    foreach (Series serie in obj.SeriesCollection.ToList<Series>())
                    {
                        if (serie.OHLCType == SeriesTypeOHLC.Volume)
                        {
                            serie.StrokeColor = GeneralUtil.GetColorFromHexa(layout.CorVolume).Color;
                            serie.StrokeThickness = layout.VolumeStrokeThickness;
                        }
                    }
                }


                _stockChartX.ThreeDStyle = false;
                _stockChartX.CandleDownOutlineColor = GeneralUtil.GetColorFromHexa(layout.CorBordaCandleBaixa).Color;
                _stockChartX.CandleDownWickMatchesOutlineColor = true;
                _stockChartX.CandleUpOutlineColor = GeneralUtil.GetColorFromHexa(layout.CorBordaCandleAlta).Color;
                _stockChartX.CandleUpWickMatchesOutlineColor = true;
                _stockChartX.UpColor = GeneralUtil.GetColorFromHexa(layout.CorCandleAlta).Color;
                _stockChartX.DownColor = GeneralUtil.GetColorFromHexa(layout.CorCandleBaixa).Color;
                _stockChartX.CalendarBackground = GeneralUtil.GetColorFromHexa(layout.CorFundo);
                gridPrincipal.Background = GeneralUtil.GetColorFromHexa(layout.CorFundo);
                canvasAbaixoStockchart.Background = GeneralUtil.GetColorFromHexa(layout.CorFundo);
                // _stockChartX.LeftChartSpace = layout.EspacoAEsquerdaDoGrafico.Value;
                _stockChartX.RightChartSpace = layout.EspacoADireitaDoGrafico.Value;

                _stockChartX.XGrid = (bool)layout.GradeHorizontal;
                _stockChartX.YGrid = (bool)layout.GradeVertical;

                if (this.Intraday())
                    _stockChartX.VolumeDivisor = 1;
                else
                    _stockChartX.VolumeDivisor = 1000000;


                if (layout.PosicaoEscala == 2)
                    _stockChartX.ScaleAlignment = ScaleAlignmentTypeEnum.Left;
                else
                    _stockChartX.ScaleAlignment = ScaleAlignmentTypeEnum.Right;

                _stockChartX.ScalePrecision = Convert.ToInt32(layout.PrecisaoEscala);

                this.Background = GeneralUtil.GetColorFromHexa(layout.CorFundo);

                _stockChartX.UseVolumeUpDownColors = (bool)layout.UsarCoresAltaBaixaVolume;

                //setando o tipo de escala
                SetTipoEscala((ScalingTypeEnum)Layout.TipoEscala);

                //setando o estilo das barras
                SetTipoBarra((SeriesTypeEnum)Layout.EstiloBarra);

                _stockChartX.Update();
                
                InsereIndicadores(layout.Indicadores);

                //abrindop thread para executar
                AtualizaTamanhoPaineis();

                //fazendo o update
                _stockChartX.Update();

                //Executando thread de post processing para layout
                Thread threadPostProcessingLayout = new Thread(new ThreadStart(AplicaLayoutPostProcessing));
                threadPostProcessingLayout.Start();

            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /// <summary>
        /// Seta um skin de cores como o Enfoque
        /// </summary>
        public void SetSkinAzulBranco()
        {
            _stockChartX.Background = Brushes.White;
            // Criando um gradiente para a barra divisora
            LinearGradientBrush brushDivisorBranco = new LinearGradientBrush();
            brushDivisorBranco.StartPoint = new Point(0, 0);
            brushDivisorBranco.EndPoint = new Point(0, 10);

            // Create and add Gradient stops
            GradientStop whiteGS = new GradientStop();
            whiteGS.Color = Colors.White;
            whiteGS.Offset = 0.0;
            brushDivisorBranco.GradientStops.Add(whiteGS);

            GradientStop balckGS = new GradientStop();
            balckGS.Color = Colors.Black;
            balckGS.Offset = 1;

            brushDivisorBranco.GradientStops.Add(balckGS);
            foreach (ChartPanel obj in _stockChartX.PanelsCollection.ToList<ChartPanel>())
            {
                obj.Background = Brushes.White;
                obj.YAxesBackground = Brushes.White;
                obj.Foreground = Brushes.Black;
                obj.TitleBarBackground = brushDivisorBranco;

                //percorrendo as series
                foreach (Series serie in obj.SeriesCollection.ToList<Series>())
                {
                    serie.StrokeColor = Colors.Black;
                }
            }
            _stockChartX.FontForeground = Brushes.DarkRed;
            _stockChartX.ThreeDStyle = false;
            _stockChartX.CandleDownOutlineColor = Colors.Black;
            _stockChartX.CandleUpOutlineColor = Colors.Black;
            _stockChartX.UpColor = Colors.White;
            _stockChartX.DownColor = Colors.Black;

            gridPrincipal.Background = Brushes.White;
            canvasAbaixoStockchart.Background = Brushes.White;
            _stockChartX.XGrid = true;
            _stockChartX.YGrid = true;
            this.Background = Brushes.White;
        }

        #endregion              

        #region Zoom
        
        /// <summary>
        /// Metodo publico que faz um resset no gráfico
        /// </summary>
        public void ResetZoom()
        {
            //setando as caracteristicas da Scrollbar
            int visibleRecords = 0;
            if (GeneralUtil.LayoutFake().VisibleRecords > _stockChartX.RecordCount)
                visibleRecords = _stockChartX.RecordCount;
            else
                visibleRecords = GeneralUtil.LayoutFake().VisibleRecords;

            _stockChartX.LastVisibleRecord = _stockChartX.RecordCount;
            if (_stockChartX.RecordCount > visibleRecords)
                _stockChartX.FirstVisibleRecord = _stockChartX.RecordCount - visibleRecords;
            else
                _stockChartX.FirstVisibleRecord = 0;

            scrollbar.Value = _stockChartX.RecordCount;
            scrollbar.ViewportSize = _stockChartX.VisibleRecordCount;
            scrollbar.Maximum = _stockChartX.RecordCount;
            scrollbar.Minimum = visibleRecords;
            this.Layout.VisibleRecords = _stockChartX.VisibleRecordCount;
            _stockChartX.ResetYScale(0);
        }

        #endregion

        #region Objetos

        /// <summary>
        /// Metodo que faz a exclusao dos objetos selecionados
        /// </summary>
        public void DeleteObjetosSelecionados()
        {
            //devo percorrer todos os objetos checando se estão selecionados ou nao
            //caso estejam devo apaga-los
            foreach (object obj in _stockChartX.SelectedObjectsCollection)
            {
                try
                {
                    if (obj.GetType().ToString().Contains("Series"))
                        if (!IsSource((Series)obj))
                            _stockChartX.RemoveSeries(((Series)obj));
                        else
                            MessageBox.Show("A série não pode ser removida pois é usada como fonte de dados para outra série. Exclua inicialmente a outra série para poder apagar esta segunda série em seguida.");
                    if (obj.GetType().ToString().Contains("Line"))
                        _stockChartX.RemoveObject(((LineStudy)obj));
                    if (obj.GetType().ToString().Contains("Indicator"))
                    {
                        if (!IsSource((Series)obj))                        
                            _stockChartX.RemoveSeries(((Series)obj));
                        else
                            MessageBox.Show("O indicador não pode ser removido pois é usado como fonte de dados para outra série. Exclua inicialmente a outra série para poder apagar esta segunda série em seguida.");

                    }
                }
                catch(Exception exc)
                {
                    throw exc;
                }
                
            }
        }

        /// <summary>
        /// Metodo que vai alterar a cor do objeto selecionado
        /// </summary>
        public void SetStrokeThicknessObjetoGeralSelecionado(int strokeThickness)
        {
            foreach (LineStudy obj in _stockChartX.LineStudiesCollection)
            {
                if (obj.Selected)
                {
                    //LineStudy
                    Brush brush = obj.Stroke;
                    obj.Stroke = Brushes.Black;
                    obj.Stroke = brush;
                    obj.StrokeThickness = strokeThickness;                    
                }
            }

            foreach (Series obj in _stockChartX.SeriesCollection)
            {
                if (obj.Selected)
                    obj.StrokeThickness = strokeThickness;
            }

            foreach (Indicator obj in _stockChartX.IndicatorsCollection)
            {
                if (obj.Selected)
                    obj.StrokeThickness = strokeThickness;
            }
                        
            _stockChartX.Update();
        }

        /// <summary>
        /// Metodo que vai alterar a cor do objeto selecionado
        /// </summary>
        public void SetColorObjetoGeralSelecionado(Color cor)
        {
            foreach (LineStudy obj in _stockChartX.LineStudiesCollection)
            {
                if (obj.Selected)
                    obj.Stroke = new SolidColorBrush(cor);
            }

            foreach (Series obj in _stockChartX.SeriesCollection)
            {
                if (obj.Selected)
                    obj.StrokeColor = cor;
            }

            foreach (Indicator obj in _stockChartX.IndicatorsCollection)
            {
                if (obj.Selected)
                    obj.StrokeColor = cor;
            }

        }

        /// <summary>
        /// Metodo que vai alterar a cor do objeto selecionado
        /// </summary>
        public void SetStrokeTypeObjetoSelecionado(LinePattern linePattern)
        {
            foreach (LineStudy obj in _stockChartX.LineStudiesCollection)
            {
                if (obj.Selected)
                    obj.StrokeType = linePattern;
            }

            foreach (Series obj in _stockChartX.SeriesCollection)
            {
                if (obj.Selected)
                    obj.StrokePattern = linePattern;
            }

            foreach (Indicator obj in _stockChartX.IndicatorsCollection)
            {
                if (obj.Selected)
                    obj.StrokePattern = linePattern;
            }

            _stockChartX.Update();
        }

        #endregion

        #region Infopanel

        /// <summary>
        /// Metodo que é executado pela mainpage para setara a visibilidade
        /// </summary>
        /// <param name="visibility"></param>
        public void SetInfoPanelVisibility(Visibility visibility)
        {
            //if (visibility == System.Windows.Visibility.Visible)
            //    this.Layout.PainelInfo = true;
            //else
            //    this.Layout.PainelInfo = false;

            //this.infoPanel.Visibility = visibility;
        }

        /// <summary>
        /// Metodo que retorna se esse infopainel esta visivel ou nao.
        /// </summary>
        /// <returns></returns>
        public bool GetInfoPainelVisibility()
        {
            return this.Layout.PainelInfo.Value;
        }

        #endregion

        #region AfterMarket

        /// <summary>
        /// Metodo que retorna o flag indicando se deve ou nao carregar dados de after market
        /// </summary>
        /// <returns></returns>
        public bool GetAfterMarket()
        {
            return this.afterMarket;
        }

        /// <summary>
        /// Metodo que seta o after market
        /// </summary>
        /// <param name="afterMarket"></param>
        public void SetAfterMarket(bool afterMarket)
        {
            this.afterMarket = afterMarket;
            this.Refresh();
        }

        #endregion

        #region Indicadores

        /// <summary>
        /// Metodo que faz a inserção do indcador no painel selecionado
        /// </summary>
        /// <param name="painelOndeDevoInserir"></param>
        public void InserirIndicador(ChartPanel painelOndeDevoInserir, List<IndicatorPropertyDTO> listaPropriedades, 
            IndicatorInfoDTO indicador, TerminalWebSVC.IndicadorDTO indicadorServer)
        {

            try
            {
                //variavel de indicador
                Indicator indicadorTemp = null;

                //estabelecendp p nome do indicador
                string name = indicador.NomePortugues + "("
                            + (_stockChartX.GetIndicatorCountByType(indicador.TipoStockchart) + 1).ToString() + ")";
                                
                //fazendo a validação dos parametros
                string msg = IndicadorDAO.ValidaIndicator(indicador.TipoStockchart, listaPropriedades, _stockChartX.RecordCount);

                if (msg != "")
                {
                    MessageBox.Show("O indicador " + name + " teve seus paramêtros alterados, pois não era possível plota-lo com os parâmetros atuais");
                    listaIndicadoresComErro.Add(indicadorServer);

                    listaPropriedades = IndicadorDAO.GetPropriedadesDefault(indicador.TipoStockchart);

                    listaIndicadoresDevemSerIgnorados.Add(name);

                    //foreach (IndicatorInfoDTO obj in StaticData.GetListaIndicadores())
                    //{
                    //    if (obj.TipoStockchart == indicador.TipoStockchart)
                    //    {
                    //        InserirIndicador(obj, true);
                    //        break;
                    //    }
                    //}

                    //return;
                }                


                #region Criando o indicador em novo painel
                if (painelOndeDevoInserir == null)
                {
                    ChartPanel newPanel = _stockChartX.AddChartPanel();
                    if (newPanel != null)
                    {
                        //newPanel.CloseBox = false;
                        newPanel.Background = _stockChartX.PanelsCollection.ToList<ChartPanel>()[0].Background;
                        newPanel.YAxesBackground = _stockChartX.PanelsCollection.ToList<ChartPanel>()[0].YAxesBackground;
                        newPanel.Foreground = _stockChartX.PanelsCollection.ToList<ChartPanel>()[0].Foreground;
                        //newPanel.Name = indicador.NomePortugues;
                        newPanel.TitleBarBackground = _stockChartX.PanelsCollection.ToList<ChartPanel>()[0].TitleBarBackground;
                        newPanel.TitleBarButtonForeground = _stockChartX.PanelsCollection.ToList<ChartPanel>()[0].TitleBarButtonForeground;

                        indicadorTemp = _stockChartX.AddIndicator(indicador.TipoStockchart,
                            name.ToString(), newPanel, false);

                    }
                    else
                    {
                        MessageBox.Show("Número limite de paineis criados");
                        return;
                    }

                }
                else
                {
                    indicadorTemp = _stockChartX.AddIndicator(indicador.TipoStockchart,
                     name, painelOndeDevoInserir, false);
                }
                #endregion

                #region Configurando Caracteristicas da serie principal

                if (indicadorServer == null)
                {
                    indicadorTemp.StrokeColor = StaticData.corSelecionada;
                    indicadorTemp.StrokeThickness = StaticData.strokeThickness;
                    indicadorTemp.StrokePattern = StaticData.estiloLinhaSelecionado;
                }
                else
                {
                    indicadorTemp.StrokeColor = GeneralUtil.GetColorFromHexa(indicadorServer.Cor).Color;
                    indicadorTemp.StrokeThickness = Convert.ToDouble(indicadorServer.Espessura);
                    indicadorTemp.StrokePattern = (LinePattern)(int)indicadorServer.TipoLinha;
                }
                #endregion

                #region Capturando os parametros

                foreach (IndicatorPropertyDTO propriedade in listaPropriedades)
                {
                    switch (propriedade.TipoDoCampo)
                    {
                        case TipoField.SymbolList:
                            if (propriedade.Value == null)
                                propriedade.Value = _stockChartX.Symbol;
                            indicadorTemp.SetParameterValue(propriedade.IndexStockChart, propriedade.Value);
                            break;
                        case TipoField.NumericUpDownInteger:
                            indicadorTemp.SetParameterValue(propriedade.IndexStockChart, Convert.ToInt32(propriedade.Value));
                            break;
                        case TipoField.Double:
                            indicadorTemp.SetParameterValue(propriedade.IndexStockChart, Convert.ToDouble(propriedade.Value));
                            break;
                        case TipoField.Serie:
                            string nomeSerie = (string)propriedade.Value;
                            if ((nomeSerie == ".OPEN") ||
                                (nomeSerie == ".LOW") ||
                                (nomeSerie == ".HIGH") ||
                                (nomeSerie == ".CLOSE") ||
                                (nomeSerie == ".VOLUME"))
                                nomeSerie = _stockChartX.Symbol + nomeSerie;
                            

                            indicadorTemp.SetParameterValue(propriedade.IndexStockChart, nomeSerie);
                            break;
                        case TipoField.Media:
                            IndicatorType media = new IndicatorType();
                            switch ((string)propriedade.Value)
                            {
                                case "Ponderada":
                                    media = IndicatorType.WeightedMovingAverage;
                                    break;
                                case "VYDIA":
                                    media = IndicatorType.VIDYA;
                                    break;
                                case "Variável":
                                    media = IndicatorType.VariableMovingAverage;
                                    break;
                                case "Triangular":
                                    media = IndicatorType.TriangularMovingAverage;
                                    break;
                                case "TimeSeries":
                                    media = IndicatorType.TimeSeriesMovingAverage;
                                    break;
                                case "Simples":
                                    media = IndicatorType.SimpleMovingAverage;
                                    break;
                                case "Exponencial":
                                    media = IndicatorType.ExponentialMovingAverage;
                                    break;
                                default:
                                    media = IndicatorType.SimpleMovingAverage;
                                    break;
                            }
                            indicadorTemp.SetParameterValue(propriedade.IndexStockChart, media);
                            break;


                    }

                }

                #endregion

                #region Assinando eventos

                indicadorTemp.EditIndicatorInChartPanel += indicadorTemp_EditIndicatorInChartPanel;

                #endregion



                //atualizando
                _stockChartX.Update();

                //iniciando rotina de postupdate para indicadores
                if ((indicadorServer != null) && (indicadorTemp != null))
                {
                    Thread threadPostIndicatorUpdate = new Thread(new ParameterizedThreadStart(PostUpdateIndicator));
                    IndicadorComposto indicadorComposto = new IndicadorComposto();
                    indicadorComposto.indicadorServer = indicadorServer;
                    indicadorComposto.indicator = indicadorTemp;
                    threadPostIndicatorUpdate.Start(indicadorComposto);
                }

            }
            catch (Exception exc)
            {
                MessageBox.Show("Erro ao inserir um indicador.\nMessage:" + exc.Message);
            }
            
        }

        struct IndicadorComposto
        {
            public Indicator indicator;
            public TerminalWebSVC.IndicadorDTO indicadorServer;
        }

        /// <summary>
        /// Metodo executado apos a inserção do indicador
        /// </summary>
        /// <param name="indicador"></param>
        private void PostUpdateIndicator(object indicador)
        {
            Thread.Sleep(200);            
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (IsParent(((IndicadorComposto)indicador).indicator))
                {
                    for (int i = 0; i < GetIndicatorChildren(((IndicadorComposto)indicador).indicator).Count;i++ )
                    {
                        if (i == 0)
                            GetIndicatorChildren(((IndicadorComposto)indicador).indicator)[0].StrokeColor 
                                = GeneralUtil.GetColorFromHexa(((IndicadorComposto)indicador).indicadorServer.CorFilha1).Color;
                        else
                            GetIndicatorChildren(((IndicadorComposto)indicador).indicator)[1].StrokeColor
                                = GeneralUtil.GetColorFromHexa(((IndicadorComposto)indicador).indicadorServer.CorFilha2).Color;
                    }
                }

            });
        }

        /// <summary>
        /// Metodo que retorna a lista de filhos do indicador
        /// </summary>
        /// <param name="indicador"></param>
        /// <returns></returns>
        private List<Indicator> GetIndicatorChildren(Indicator indicador)
        {
            List<Indicator> listaChildren = new List<Indicator>();
            foreach (Indicator obj in _stockChartX.IndicatorsCollection)
            {
                if (obj.TwinsParentIndicator == indicador)
                    listaChildren.Add(obj);
            }

            return listaChildren;
        }

        /// <summary>
        /// Metodo que informa se o indicador é do tipo parent ou nao
        /// </summary>
        /// <param name="indicator"></param>
        /// <returns></returns>
        private bool IsParent(Indicator indicator)
        {
            foreach (Indicator obj in _stockChartX.IndicatorsCollection)
            {
                if (obj.TwinsParentIndicator == indicator)
                    return true;
            }
            return true;
        }


        /// <summary>
        /// Metodo que faz a inserção do indcador no painel selecionado
        /// </summary>
        /// <param name="panelClicado"></param>
        public void InserirIndicador(IndicatorInfoDTO newIndicator)
        {
            try
            {
                #region Criando o indicador em novo painel
                string name = newIndicator.NomePortugues + "("
                            + (_stockChartX.GetIndicatorCountByType(newIndicator.TipoStockchart) + 1).ToString() + ")";
                if (newIndicator.NovoPainel)
                {
                    ChartPanel newPanel = _stockChartX.AddChartPanel();
                    if (newPanel != null)
                    {
                        //newPanel.CloseBox = false;
                        newPanel.Background = _stockChartX.PanelsCollection.ToList<ChartPanel>()[0].Background;
                        newPanel.YAxesBackground = _stockChartX.PanelsCollection.ToList<ChartPanel>()[0].YAxesBackground;
                        newPanel.Foreground = _stockChartX.PanelsCollection.ToList<ChartPanel>()[0].Foreground;
                        //newPanel.Name = newIndicator.NomePortugues;
                        newPanel.TitleBarBackground = _stockChartX.PanelsCollection.ToList<ChartPanel>()[0].TitleBarBackground;
                        newPanel.TitleBarButtonForeground = _stockChartX.PanelsCollection.ToList<ChartPanel>()[0].TitleBarButtonForeground;

                        InserirIndicador(newPanel, newIndicator.Propriedades, newIndicator, null);
                    }
                    else
                    {
                        MessageBox.Show("Número limite de paineis criados");
                        return;
                    }

                }
                else
                {
                    this.InserirIndicador(_stockChartX.PanelsCollection.ToList<ChartPanel>()[0], newIndicator.Propriedades, newIndicator, null);
                }
                #endregion
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        /// <summary>
        /// Metodo que retorna a lista de indicadores
        /// </summary>
        /// <returns></returns>
        private List<TerminalWebSVC.IndicadorDTO> GetIndicadores()
        {
            try
            {
                List<TerminalWebSVC.IndicadorDTO> listaAux = new List<TerminalWebSVC.IndicadorDTO>();
                foreach (Indicator obj in _stockChartX.IndicatorsCollection)
                {
                    if (!obj.IsTwin)
                    {
                        TerminalWebSVC.IndicadorDTO indicador = new TerminalWebSVC.IndicadorDTO();
                        indicador.Cor = obj.StrokeColor.ToString();
                        indicador.Espessura = Convert.ToInt32(obj.StrokeThickness);
                        indicador.IndexPainel = obj.Panel.Index;
                        indicador.TipoLinha = Convert.ToInt32(obj.StrokePattern);
                        indicador.TipoIndicador = Convert.ToInt32(obj.IndicatorType);
                        int i = 0;
                        foreach (StockChartX_IndicatorsParameters.IndicatorParameter parametro in obj.IndicatorParams)
                        {
                            indicador.Parametros += obj.GetParameterValue(i).ToString() + ";";
                            i++;
                        }

                        indicador.Name = obj.FullName;
                        listaAux.Add(indicador);
                    }
                }

                //pegando os filhos
                foreach (Indicator obj in _stockChartX.IndicatorsCollection)
                {
                    if (obj.IsTwin)
                    {
                        foreach (TerminalWebSVC.IndicadorDTO objServer in listaAux)
                        {
                            if (objServer.Name == obj.TwinsParentIndicator.FullName)
                            {
                                //nesse caso encontrei o pai
                                if (objServer.CorFilha1 == null)
                                {
                                    objServer.CorFilha1 = obj.StrokeColor.ToString();
                                    objServer.EspessuraFilha1 = Convert.ToInt32(obj.StrokeThickness);
                                    objServer.TipoLinhaFilha1 = Convert.ToInt32(obj.StrokePattern);
                                }
                                else
                                {
                                    objServer.CorFilha2 = obj.StrokeColor.ToString();
                                    objServer.EspessuraFilha2 = Convert.ToInt32(obj.StrokeThickness);
                                    objServer.TipoLinhaFilha2 = Convert.ToInt32(obj.StrokePattern);
                                }
                            }
                        }

                    }
                }

                //retornando a lista
                return listaAux;
            }
            catch
            {
                return new List<TerminalWebSVC.IndicadorDTO>();
            }
        }

        /// <summary>
        /// Metodo que faz a inserção de indicadores de acordo com a lista passada
        /// </summary>
        /// <param name="indicadores"></param>
        public void InsereIndicadores(List<TerminalWebSVC.IndicadorDTO> indicadores)
        {
            if (indicadores!= null)
            {
                foreach (TerminalWebSVC.IndicadorDTO indicador in indicadores)
                {
                    if (listaIndicadoresDevemSerIgnorados.Contains(indicador.Name))
                    {
                        listaIndicadoresDevemSerIgnorados.Remove(indicador.Name);
                        continue;
                    }

                    //resgatando qual o tipo de indicador na lista local
                    IndicatorInfoDTO indicadorInfo = new IndicatorInfoDTO();
                    foreach (IndicatorInfoDTO obj in StaticData.GetListaIndicadores())
                    {
                        if ((int)obj.TipoStockchart == (int)indicador.TipoIndicador)
                        {
                            indicadorInfo = obj;
                            break;
                        }
                    }

                    //montando a lista de propriedades
                    List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();
                    string[] arPropriedades = indicador.Parametros.Split(';');
                    int i = 0;
                    foreach (IndicatorPropertyDTO propriedade in indicadorInfo.Propriedades)
                    {
                        if (propriedade.IndexStockChart >= 0)
                        {
                            if (ativoAntigo != "")
                                propriedade.Value = arPropriedades[i].Replace(ativoAntigo, ativo);
                            else
                                propriedade.Value = arPropriedades[i];
                            i++;
                        }
                    }
                    
                    this.InserirIndicador(_stockChartX.PanelsCollection.ToList<ChartPanel>()[(int)indicador.IndexPainel], 
                        indicadorInfo.Propriedades, indicadorInfo, indicador);
                }
            }
        }

        #endregion

        #region Layouts

        /// <summary>
        /// Metodo usado para se retornar um objeto TemplateDTO a partir do grafico
        /// </summary>
        /// <returns></returns>
        public TerminalWebSVC.LayoutDTO GetLayoutDTOFromStockchart()
        {
            TerminalWebSVC.LayoutDTO layout = new TerminalWebSVC.LayoutDTO();

            //recuperando os indicadores
            layout.Indicadores = this.GetIndicadores();

            //recuperando os paineis
            layout.Paineis = this.GetPaineis();

            //recuperando os objetos
            layout.Objetos = this.GetObjetos();

            //recuperando as configurações gerais
            layout.CorGrid = ((SolidColorBrush)_stockChartX.GridStroke).Color.ToString();
            layout.CorEscala = ((SolidColorBrush)_stockChartX.FontForeground).Color.ToString();
            layout.CorFundo = ((SolidColorBrush)_stockChartX.Background).Color.ToString();
            layout.CorCandleAlta = _stockChartX.UpColor.ToString();

            layout.CorBordaCandleAlta = _stockChartX.CandleUpOutlineColor.ToString();
            layout.CorBordaCandleBaixa = _stockChartX.CandleDownOutlineColor.ToString();
            
            layout.CorCandleBaixa = _stockChartX.DownColor.ToString();
            

            foreach(Series obj in _stockChartX.SeriesCollection)
            {
                if (obj.OHLCType == SeriesTypeOHLC.Volume)
                {
                    layout.CorVolume = obj.StrokeColor.ToString();
                    layout.VolumeStrokeThickness = Convert.ToInt32(obj.StrokeThickness);
                }
                else
                {
                    layout.CorVolume = Colors.Black.ToString();
                    layout.VolumeStrokeThickness = 1;
                }
            }
            layout.DarvaBox = this.DarvaBoxes;
            layout.EspacoADireitaDoGrafico = _stockChartX.RightChartSpace;
            layout.EspacoAEsquerdaDoGrafico = _stockChartX.LeftChartSpace;
            layout.EstiloBarra = Convert.ToInt32(_stockChartX.SeriesCollection.ToList<Series>()[0].SeriesType);
            
            //TODO:ALTERAR
            layout.EstiloPreco = 1;
            layout.EstiloPrecoParam1 = 0;
            layout.EstiloPrecoParam2 = 0;
            //--------------------------

            layout.GradeHorizontal = _stockChartX.XGrid;
            layout.GradeVertical = _stockChartX.YGrid;
            layout.PainelInfo = this.GetInfoPainelVisibility();
            layout.Periodicidade = GeneralUtil.GetIntPeriodicidade(this.Periodicidade);
            layout.PosicaoEscala = Convert.ToInt32(_stockChartX.ScaleAlignment);
            layout.PrecisaoEscala = _stockChartX.ScalePrecision;
            layout.TipoEscala = Convert.ToInt32(_stockChartX.ScalingType);
            layout.TipoVolume = "F";
            layout.UsarCoresAltaBaixaVolume = _stockChartX.UseVolumeUpDownColors;

            
            //retornando valores
            return layout;
        }


        #endregion

        #region Paineis

        /// <summary>
        /// Metodo que retorna a lista de paineis
        /// </summary>
        /// <returns></returns>
        public List<TerminalWebSVC.PainelDTO> GetPaineis()
        {
            List<TerminalWebSVC.PainelDTO> lista = new List<TerminalWebSVC.PainelDTO>();
            foreach (ChartPanel obj in _stockChartX.PanelsCollection.ToList<ChartPanel>())
            {
                TerminalWebSVC.PainelDTO painel = new TerminalWebSVC.PainelDTO();
                
                //checando se é painel de preço
                if (IsPricePanel(obj))
                    painel.TipoPainel = "P";
                else if (IsVolumePanel(obj))
                    painel.TipoPainel = "V";
                else
                    painel.TipoPainel = "O";


                //painel.Altura = Convert.ToInt32(((obj.ActualHeight + 10) / _stockChartX.ActualHeight) * 100);
                painel.Altura = Convert.ToInt32(obj.ActualHeight);

                switch (obj.State)
                {
                    case ChartPanel.StateType.Maximized:
                        painel.Status = "M";
                        break;
                    case ChartPanel.StateType.Minimized:
                        painel.Status = "m";
                        break;
                    default:
                        painel.Status = "N";
                        break;
                }

                painel.Index = obj.Index;
                
                lista.Add(painel);
            }

            //retornando lista
            return lista;
        }

        /// <summary>
        /// Metodo que verifica se é um painel de preço
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        private bool IsPricePanel(ChartPanel panel)
        {
            foreach (Series obj in panel.SeriesCollection.ToList<Series>())
            {
                if (obj.OHLCType == SeriesTypeOHLC.Close)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Metodo que verifica se é um painel de volume
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        private bool IsVolumePanel(ChartPanel panel)
        {
            foreach (Series obj in panel.SeriesCollection.ToList<Series>())
            {
                if (obj.OHLCType == SeriesTypeOHLC.Volume)
                    return true;
            }
            return false;
        }

        #endregion

        #region Objetos

        /// <summary>
        /// Metodo que faz a inserção automatica dos objetos passados
        /// </summary>
        /// <param name="lista"></param>
        private void InsereObjetos(List<TerminalWebSVC.ObjetoEstudoDTO> lista)
        {
            if (lista != null)
            {
                foreach (TerminalWebSVC.ObjetoEstudoDTO estudo in lista)
                {   
                    object[] parametros = new List<object>().ToArray();
                    LineStudy line = null;
                 
                    //Inserção de objeto de texto
                    if (estudo.TipoObjeto == 17)
                    {
                        if (estudo.Parametros == "Y")
                        {
                            line = _stockChartX.AddStaticText(estudo.Texto, Guid.NewGuid().ToString(),
                                                GeneralUtil.GetColorFromHexa(estudo.CorObjeto), estudo.Espessura.Value, estudo.IndexPainel);
                            line.IsContextMenuDisabled = true;
                            line.Selectable = true;
                            line.SetXYValues(estudo.RecordInicial, estudo.ValorInicial, estudo.RecordInicial, estudo.ValorFinal);
                            CurrentState = EstadoGrafio.Nenhum;
                            ListaLineStudy.Add(line);
                            ListaValorY.Add(line);
                            _stockChartX.Update();
                        }
                        else
                        {
                            line = _stockChartX.AddStaticText(estudo.Texto, Guid.NewGuid().ToString(),
                                                GeneralUtil.GetColorFromHexa(estudo.CorObjeto), estudo.Espessura.Value, estudo.IndexPainel);
                            line.IsContextMenuDisabled = true;
                            line.Selectable = true;
                            line.SetXYValues(estudo.RecordInicial, estudo.ValorInicial, estudo.RecordInicial, estudo.ValorFinal);
                            CurrentState = EstadoGrafio.Nenhum;
                            ListaLineStudy.Add(line);
                            _stockChartX.Update();
                        }              
                    }
                    else if (estudo.TipoObjeto == 16)
                    {
                        line = _stockChartX.CreateSymbolObject((SymbolType)Convert.ToInt32(estudo.Parametros), 
                            Guid.NewGuid().ToString(), estudo.IndexPainel, new Size(16, 16));
                        line.IsContextMenuDisabled = true;
                        line.Selectable = true;
                        line.SetXYValues(estudo.RecordInicial, estudo.ValorInicial, estudo.RecordInicial, estudo.ValorFinal);
                        CurrentState = EstadoGrafio.Nenhum;
                        ListaLineStudy.Add(line);
                        _stockChartX.Update();   
                    }
                    else if (estudo.TipoObjeto == 7)
                    {

                        List<object> arParams = new List<object>();
                        arParams.Add(0);
                        arParams.Add(0.382);
                        arParams.Add(0.50);
                        arParams.Add(0.618);
                        arParams.Add(1);
                        arParams.Add(1);

                        //Adicionando fibonacii retracement
                        line = _stockChartX.CreateLineStudy((LineStudy.StudyTypeEnum)estudo.TipoObjeto, Guid.NewGuid().ToString(),
                                GeneralUtil.GetColorFromHexa(estudo.CorObjeto), estudo.IndexPainel, arParams.ToArray<object>());
                        //aplicando outras propriedades
                        line.StrokeThickness = (int)estudo.Espessura;
                        line.SetXYValues(estudo.RecordInicial, estudo.ValorInicial, estudo.RecordFinal, estudo.ValorFinal);
                        line.StrokeType = (LinePattern)estudo.TipoLinha;

                    }
                    else
                    {
                        line = _stockChartX.CreateLineStudy((LineStudy.StudyTypeEnum)estudo.TipoObjeto, Guid.NewGuid().ToString(),
                            GeneralUtil.GetColorFromHexa(estudo.CorObjeto), estudo.IndexPainel, parametros);

                        //aplicando outras propriedades
                        line.StrokeThickness = (int)estudo.Espessura;
                        line.SetXYValues(estudo.RecordInicial, estudo.ValorInicial, estudo.RecordFinal, estudo.ValorFinal);
                        line.StrokeType = (LinePattern)estudo.TipoLinha;
                    }
                }
            }
            
        }

        /// <summary>
        /// Metodo que retorna a lista de objetios
        /// </summary>
        /// <returns></returns>
        private List<TerminalWebSVC.ObjetoEstudoDTO> GetObjetos()
        {
            try
            {
                List<TerminalWebSVC.ObjetoEstudoDTO> listaAux = new List<TerminalWebSVC.ObjetoEstudoDTO>();
                foreach (LineStudy obj in _stockChartX.LineStudiesCollection)
                {
                    TerminalWebSVC.ObjetoEstudoDTO objetoEstudo = new TerminalWebSVC.ObjetoEstudoDTO();
                    objetoEstudo.CorObjeto = ((SolidColorBrush)obj.Stroke).Color.ToString();
                    objetoEstudo.Espessura = Convert.ToInt32(obj.StrokeThickness);
                    objetoEstudo.IndexPainel = obj.Panel.Index;
                    objetoEstudo.RecordFinal = Convert.ToInt32(obj.X2Value);
                    objetoEstudo.RecordInicial = Convert.ToInt32(obj.X1Value);
                    objetoEstudo.TipoLinha = (int)obj.StrokeType;
                    objetoEstudo.TipoObjeto = (int)obj.StudyType;
                    objetoEstudo.ValorFinal = obj.Y2Value;
                    objetoEstudo.ValorInicial = obj.Y1Value;
                    objetoEstudo.TamanhoTexto = 10;
                    if (obj.StudyType == LineStudy.StudyTypeEnum.ImageObject)
                        objetoEstudo.Parametros = Convert.ToString((int)((Object[])obj.ExtraArgs)[0]);
                    else if (obj.StudyType == LineStudy.StudyTypeEnum.StaticText)
                    {
                        foreach (LineStudy estudoTemp in ListaValorY)
                        {
                            if (obj.Key == estudoTemp.Key)
                                objetoEstudo.Parametros = "Y";
                        }

                        if (objetoEstudo.Parametros == null)
                            objetoEstudo.Parametros = "";
                    }
                    else
                        objetoEstudo.Parametros = "";

                    if (obj.StudyType == LineStudy.StudyTypeEnum.StaticText)
                        objetoEstudo.Texto = (string)((Object[])obj.ExtraArgs)[0];
                    else
                        objetoEstudo.Texto = "";




                    //TODO: Rever parametros setados de forma fixa abaixo
                    objetoEstudo.InfinitaADireita = false;
                    objetoEstudo.Magnetica = false;



                    objetoEstudo.ValorErrorChannel = 0;


                    //adicionando a lista auxilair
                    listaAux.Add(objetoEstudo);

                }

                //retornando a lista
                return listaAux;
            }
            catch
            {
                return new List<TerminalWebSVC.ObjetoEstudoDTO>();
            }
        }

        #endregion

        #endregion

        public void AtualizaCores()
        {
            //atualiza status do panel
            foreach(ChartPanel obj in _stockChartX.PanelsCollection.ToList<ChartPanel>())
            {
                obj.YAxesBackground = new SolidColorBrush(Colors.White);
            }
            _stockChartX.Update();
        }

        private void _stockChartX_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //fechando o comando
            txtComando.Text = "";
            borderCommand.Visibility = System.Windows.Visibility.Collapsed;

            if (StaticData.tipoAcao == StaticData.TipoAcao.CROSS)
                {
                    if (borderRegua.Visibility == System.Windows.Visibility.Collapsed)
                    {            
                        borderRegua.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        borderRegua.Visibility = System.Windows.Visibility.Collapsed;
                        _stockChartX.RemoveObject("REGUA");
                    }
                }
        }

        private void gridPrincipal_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    borderCommand.Visibility = System.Windows.Visibility.Collapsed;
                    txtComando.Text = "";
                    break;
                case Key.Delete:
                case Key.Shift:
                case Key.Ctrl:
                case Key.Enter:
                case Key.Back:
                case Key.Home:
                case Key.CapsLock:
                case Key.Alt:
                case Key.PageDown:
                case Key.PageUp:
                    break;
                case Key.Right:
                    scrollbar.Value += 5;
                    ScrollChartRight(5);
                    break;
                case Key.Left:
                    scrollbar.Value -= 5;
                    ScrollChartLeft(5);
                    break;
                case Key.Up:
                case Key.Down:
                    break;                    
                default:                    
                    borderCommand.Visibility = System.Windows.Visibility.Visible;
                    txtComando.Focus();
                    e.Handled = MakeUpperCase(txtComando, e);
                    //txtComando.Text += e.Key.ToString();
                    
                    break;
            }
            
        }

        private void txtComando_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtComando.Text == "D")
                    this.SetPeriodicidade(TerminalWEB.Periodicidade.Diario);
                else if (txtComando.Text == "S")
                    this.SetPeriodicidade(TerminalWEB.Periodicidade.Semanal);
                else if (txtComando.Text == "M")
                    this.SetPeriodicidade(TerminalWEB.Periodicidade.Mensal);
                else if (txtComando.Text == "1")
                    this.SetPeriodicidade(TerminalWEB.Periodicidade.UmMinuto);
                else if (txtComando.Text == "2")
                    this.SetPeriodicidade(TerminalWEB.Periodicidade.DoisMinutos);
                else if (txtComando.Text == "3")
                    this.SetPeriodicidade(TerminalWEB.Periodicidade.TresMinutos);
                else if (txtComando.Text == "5")
                    this.SetPeriodicidade(TerminalWEB.Periodicidade.CincoMinutos);
                else if (txtComando.Text == "10")
                    this.SetPeriodicidade(TerminalWEB.Periodicidade.DezMinutos);
                else if (txtComando.Text == "15")
                    this.SetPeriodicidade(TerminalWEB.Periodicidade.QuinzeMinutos);
                else if (txtComando.Text == "30")
                    this.SetPeriodicidade(TerminalWEB.Periodicidade.TrintaMinutos);
                else if (txtComando.Text == "60")
                    this.SetPeriodicidade(TerminalWEB.Periodicidade.SessentaMinutos);
                else
                {
                    //verificando se ativo existe
                    foreach (AtivoDTO obj in StaticData.cacheAtivosBMFTodos)
                    {
                        if (obj.Codigo.ToUpper().Trim() == txtComando.Text.ToUpper().Trim())
                        {
                            this.ativoAntigo = this.ativo;
                            this.ativo = txtComando.Text;
                            Refresh();
                            //((PageCollection)((Grid)((Canvas)((C1TabControl)((C1TabItem)this.Parent).Parent).Parent).Parent).Parent).ChangeAtivo(this.ativo);
                
                            break;
                        }
                    }
                    foreach (AtivoDTO obj in StaticData.cacheAtivosBovespaTodos)
                    {
                        if (obj.Codigo.ToUpper().Trim() == txtComando.Text.ToUpper().Trim())
                        {
                            this.ativoAntigo = this.ativo;
                            this.ativo = txtComando.Text;
                            Refresh();
                            //((PageCollection)((Grid)((Canvas)((C1TabControl)((C1TabItem)this.Parent).Parent).Parent).Parent).Parent).ChangeAtivo(this.ativo);
                
                            break;
                        }
                    }

                    
                }
                
                txtComando.Text = "";
                borderCommand.Visibility = System.Windows.Visibility.Collapsed;
                return;
                
            }
            e.Handled = MakeUpperCase((TextBox)sender, e);
        }

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

        
    }

    #region Classes Auxiliares Retiradas de dentro do Stockchart para nao termos que mexer dentro do codigo da Modulus

    public class EditableImage
    {
        private int _width;
        private int _height;
        private bool _init;
        private byte[] _buffer;
        private int _rowLength;

        public event EventHandler<EditableImageErrorEventArgs> ImageError;

        public EditableImage(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (_init)
                {
                    OnImageError("Error: Cannot change Width after the EditableImage has been initialized");
                }
                else if ((value <= 0) || (value > 2047))
                {
                    OnImageError("Error: Width must be between 0 and 2047");
                }
                else
                {
                    _width = value;
                }
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (_init)
                {
                    OnImageError("Error: Cannot change Height after the EditableImage has been initialized");
                }
                else if ((value <= 0) || (value > 2047))
                {
                    OnImageError("Error: Height must be between 0 and 2047");
                }
                else
                {
                    _height = value;
                }
            }
        }

        public void SetPixel(int col, int row, Color color)
        {
            SetPixel(col, row, color.R, color.G, color.B, color.A);
        }

        public void SetPixel(int col, int row, byte red, byte green, byte blue, byte alpha)
        {
            if (!_init)
            {
                _rowLength = _width * 4 + 1;
                _buffer = new byte[_rowLength * _height];

                // Initialize
                for (int idx = 0; idx < _height; idx++)
                {
                    _buffer[idx * _rowLength] = 0;      // Filter bit
                }

                _init = true;
            }

            if ((col > _width) || (col < 0))
            {
                OnImageError("Error: Column must be greater than 0 and less than the Width");
            }
            else if ((row > _height) || (row < 0))
            {
                OnImageError("Error: Row must be greater than 0 and less than the Height");
            }

            // Set the pixel
            int start = _rowLength * row + col * 4 + 1;
            _buffer[start] = red;
            _buffer[start + 1] = green;
            _buffer[start + 2] = blue;
            _buffer[start + 3] = alpha;
        }

        public Color GetPixel(int col, int row)
        {
            if ((col > _width) || (col < 0))
            {
                OnImageError("Error: Column must be greater than 0 and less than the Width");
            }
            else if ((row > _height) || (row < 0))
            {
                OnImageError("Error: Row must be greater than 0 and less than the Height");
            }

            Color color = new Color();
            int _base = _rowLength * row + col + 1;

            color.R = _buffer[_base];
            color.G = _buffer[_base + 1];
            color.B = _buffer[_base + 2];
            color.A = _buffer[_base + 3];

            return color;
        }

        public Stream GetStream()
        {
            Stream stream;

            if (!_init)
            {
                OnImageError("Error: Image has not been initialized");
                stream = null;
            }
            else
            {
                stream = PngEncoder.Encode(_buffer, _width, _height);
            }

            return stream;
        }

        private void OnImageError(string msg)
        {
            if (null == ImageError) return;
            EditableImageErrorEventArgs args = new EditableImageErrorEventArgs { ErrorMessage = msg };
            ImageError(this, args);
        }

        public class EditableImageErrorEventArgs : EventArgs
        {
            private string _errorMessage = string.Empty;

            public string ErrorMessage
            {
                get { return _errorMessage; }
                set { _errorMessage = value; }
            }
        }
    }

    public class PngEncoder
    {
        private const int _ADLER32_BASE = 65521;
        private const int _MAXBLOCK = 0xFFFF;
        private static readonly byte[] _HEADER = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        private static readonly byte[] _IHDR = { (byte)'I', (byte)'H', (byte)'D', (byte)'R' };
        private static readonly byte[] _GAMA = { (byte)'g', (byte)'A', (byte)'M', (byte)'A' };
        private static readonly byte[] _IDAT = { (byte)'I', (byte)'D', (byte)'A', (byte)'T' };
        private static readonly byte[] _IEND = { (byte)'I', (byte)'E', (byte)'N', (byte)'D' };
        private static readonly byte[] _4BYTEDATA = { 0, 0, 0, 0 };
        private static readonly byte[] _ARGB = { 0, 0, 0, 0, 0, 0, 0, 0, 8, 6, 0, 0, 0 };


        public static Stream Encode(byte[] data, int width, int height)
        {
            MemoryStream ms = new MemoryStream();

            // Write PNG header
            ms.Write(_HEADER, 0, _HEADER.Length);

            // Write IHDR
            //  Width:              4 bytes
            //  Height:             4 bytes
            //  Bit depth:          1 byte
            //  Color type:         1 byte
            //  Compression method: 1 byte
            //  Filter method:      1 byte
            //  Interlace method:   1 byte

            byte[] size = BitConverter.GetBytes(width);
            _ARGB[0] = size[3]; _ARGB[1] = size[2]; _ARGB[2] = size[1]; _ARGB[3] = size[0];

            size = BitConverter.GetBytes(height);
            _ARGB[4] = size[3]; _ARGB[5] = size[2]; _ARGB[6] = size[1]; _ARGB[7] = size[0];

            // Write IHDR chunk
            WriteChunk(ms, _IHDR, _ARGB);

            // Set gamma = 1
            size = BitConverter.GetBytes(1 * 100000);
            _4BYTEDATA[0] = size[3]; _4BYTEDATA[1] = size[2]; _4BYTEDATA[2] = size[1]; _4BYTEDATA[3] = size[0];

            // Write gAMA chunk
            WriteChunk(ms, _GAMA, _4BYTEDATA);

            // Write IDAT chunk
            uint widthLength = (uint)(width * 4) + 1;
            uint dcSize = widthLength * (uint)height;

            // First part of ZLIB header is 78 1101 1010 (DA) 0000 00001 (01)
            // ZLIB info
            //
            // CMF Byte: 78
            //  CINFO = 7 (32K window size)
            //  CM = 8 = (deflate compression)
            // FLG Byte: DA
            //  FLEVEL = 3 (bits 6 and 7 - ignored but signifies max compression)
            //  FDICT = 0 (bit 5, 0 - no preset dictionary)
            //  FCHCK = 26 (bits 0-4 - ensure CMF*256+FLG / 31 has no remainder)
            // Compressed data
            //  FLAGS: 0 or 1
            //    00000 00 (no compression) X (X=1 for last block, 0=not the last block)
            //    LEN = length in bytes (equal to ((width*4)+1)*height
            //    NLEN = one's compliment of LEN
            //    Example: 1111 1011 1111 1111 (FB), 0000 0100 0000 0000 (40)
            //    Data for each line: 0 [RGBA] [RGBA] [RGBA] ...
            //    ADLER32

            uint adler = ComputeAdler32(data);
            MemoryStream comp = new MemoryStream();

            // Calculate number of 64K blocks
            uint rowsPerBlock = _MAXBLOCK / widthLength;
            uint blockSize = rowsPerBlock * widthLength;
            uint blockCount;
            uint remainder = dcSize;

            if ((dcSize % blockSize) == 0)
            {
                blockCount = dcSize / blockSize;
            }
            else
            {
                blockCount = (dcSize / blockSize) + 1;
            }

            // Write headers
            comp.WriteByte(0x78);
            comp.WriteByte(0xDA);

            for (uint blocks = 0; blocks < blockCount; blocks++)
            {
                // Write LEN
                ushort length = (ushort)((remainder < blockSize) ? remainder : blockSize);

                if (length == remainder)
                {
                    comp.WriteByte(0x01);
                }
                else
                {
                    comp.WriteByte(0x00);
                }

                comp.Write(BitConverter.GetBytes(length), 0, 2);

                // Write one's compliment of LEN
                comp.Write(BitConverter.GetBytes((ushort)~length), 0, 2);

                // Write blocks
                comp.Write(data, (int)(blocks * blockSize), length);

                // Next block
                remainder -= blockSize;
            }

            WriteReversedBuffer(comp, BitConverter.GetBytes(adler));
            comp.Seek(0, SeekOrigin.Begin);

            byte[] dat = new byte[comp.Length];
            comp.Read(dat, 0, (int)comp.Length);

            WriteChunk(ms, _IDAT, dat);

            // Write IEND chunk
            WriteChunk(ms, _IEND, new byte[0]);

            // Reset stream
            ms.Seek(0, SeekOrigin.Begin);

            return ms;

            // See http://www.libpng.org/pub/png//spec/1.2/PNG-Chunks.html
            // See http://www.libpng.org/pub/png/book/chapter08.html#png.ch08.div.4
            // See http://www.gzip.org/zlib/rfc-zlib.html (ZLIB format)
            // See ftp://ftp.uu.net/pub/archiving/zip/doc/rfc1951.txt (ZLIB compression format)
        }

        private static void WriteReversedBuffer(Stream stream, byte[] data)
        {
            int size = data.Length;
            byte[] reorder = new byte[size];

            for (int idx = 0; idx < size; idx++)
            {
                reorder[idx] = data[size - idx - 1];
            }
            stream.Write(reorder, 0, size);
        }

        private static void WriteChunk(Stream stream, byte[] type, byte[] data)
        {
            int idx;
            int size = type.Length;
            byte[] buffer = new byte[type.Length + data.Length];

            // Initialize buffer
            for (idx = 0; idx < type.Length; idx++)
            {
                buffer[idx] = type[idx];
            }

            for (idx = 0; idx < data.Length; idx++)
            {
                buffer[idx + size] = data[idx];
            }

            // Write length
            WriteReversedBuffer(stream, BitConverter.GetBytes(data.Length));

            // Write type and data
            stream.Write(buffer, 0, buffer.Length);   // Should always be 4 bytes

            // Compute and write the CRC
            WriteReversedBuffer(stream, BitConverter.GetBytes(GetCRC(buffer)));
        }

        private static readonly uint[] _crcTable = new uint[256];
        private static bool _crcTableComputed;

        private static void MakeCRCTable()
        {
            for (int n = 0; n < 256; n++)
            {
                uint c = (uint)n;
                for (int k = 0; k < 8; k++)
                {
                    if ((c & (0x00000001)) > 0)
                        c = 0xEDB88320 ^ (c >> 1);
                    else
                        c = c >> 1;
                }
                _crcTable[n] = c;
            }

            _crcTableComputed = true;
        }

        private static uint UpdateCRC(uint crc, byte[] buf, int len)
        {
            uint c = crc;

            if (!_crcTableComputed)
            {
                MakeCRCTable();
            }

            for (int n = 0; n < len; n++)
            {
                c = _crcTable[(c ^ buf[n]) & 0xFF] ^ (c >> 8);
            }

            return c;
        }

        /* Return the CRC of the bytes buf[0..len-1]. */
        private static uint GetCRC(byte[] buf)
        {
            return UpdateCRC(0xFFFFFFFF, buf, buf.Length) ^ 0xFFFFFFFF;
        }

        private static uint ComputeAdler32(byte[] buf)
        {
            uint s1 = 1;
            uint s2 = 0;
            int length = buf.Length;

            for (int idx = 0; idx < length; idx++)
            {
                s1 = (s1 + buf[idx]) % _ADLER32_BASE;
                s2 = (s2 + s1) % _ADLER32_BASE;
            }

            return (s2 << 16) + s1;
        }
    }

    #endregion


    public enum EstadoGrafio { Nenhum, Editando, Carregando, InserindoIndicador, Zooming }

    public enum Periodicidade { UmMinuto, DoisMinutos, TresMinutos, CincoMinutos, DezMinutos, QuinzeMinutos, TrintaMinutos, SessentaMinutos, CentoeVinteMinutos, Diario, Semanal, Mensal, Nenhum }


}
