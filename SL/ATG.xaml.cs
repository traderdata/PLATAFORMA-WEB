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

namespace Traderdata.Client.TerminalWEB
{
    public partial class ATG
    {
        #region Variaveis Privadas

        /// <summary>
        /// Variavel que armazena o ultimo registro onde o mouse passou sorbe o grafico
        /// </summary>
        private int registroX = -1;

        /// <summary>
        /// variavel que controla se este é o grafico do primeiro layout, poois este tem configurações especiais
        /// </summary>
        private bool LayoutUm = false;

        /// <summary>
        /// Lista de objetos colocados no gráfico
        /// </summary>
        private List<LineStudy> ListaLineStudy = new List<LineStudy>();

        /// <summary>
        /// Variavel de controla o estado atual do gráfico em relação a colocação de novos objetos
        /// </summary>
        private EstadoGrafio CurrentState = EstadoGrafio.Nenhum;

        /// <summary>
        /// Variavel que armazena o ativo que vai ser aberto
        /// </summary>
        private string ativo = "";

        /// <summary>
        /// Variavel de controle da periodicidade
        /// </summary>
        private Periodicidade Periodicidade = Periodicidade.Diario;

        /// <summary>
        /// Classe que trata os dados de marketdata
        /// </summary>
        private MarketDataDAO marketDataDAO = new MarketDataDAO();
        
        /// <summary>
        /// Variavel de controle de layout
        /// </summary>
        private TerminalWebSVC.LayoutDTO layout = new TerminalWebSVC.LayoutDTO();

        #endregion

        #region Construtor

        /// <summary>
        /// Construtor padrão do gráfico
        /// </summary>
        /// <param name="ativo"></param>
        public ATG(string ativo)
        {
            InitializeComponent();

            //Iniciar a conexao em Realtime
            RealTimeDAO.Connect();

            //Assinando os eventos de comunicazação WCF
            marketDataDAO.GetCotacaoDiariaCompleted += new MarketDataDAO.CotacaoDiarioHandler(marketDataDAO_GetCotacaoDiariaCompleted);
            marketDataDAO.GetCotacaoIntradayCompleted += new MarketDataDAO.CotacaoIntradayHandler(marketDataDAO_GetCotacaoIntradayCompleted);
            marketDataDAO.SetCotacaoDiariaCacheCompleted += new MarketDataDAO.CotacaoDiarioCacheHandler(marketDataDAO_SetCotacaoDiariaCacheCompleted);

            //Rodando as configurações inciais do gráfico
            this.ativo = ativo;
            
            //setando estado do gráfico para carregando
            this.CurrentState = EstadoGrafio.Carregando;

            //Setando o layout
            this.layout.CorBordaCandleAlta = "#FFBBCC88";
            this.layout.CorBordaCandleBaixa = "#FFBBCC88";
            this.layout.CorCandleAlta = "#FFBBCC88";
            this.layout.CorCandleBaixa = "#FFBBCC88";
            this.layout.CorFundo = "#FFFFFFFF";
            this.layout.CorMaximaMinima = "#FFBBCC88";
            this.layout.CorVolume = "#FFFFFFFF";
            this.layout.EspacoADireitaDoGrafico = 20;
            this.layout.PosicaoEscala = 1;
            this.layout.PainelInfo = false;
            this.layout.ZoomRealtime = true;
            this.layout.VisibleRecords = 50;
            this.layout.VolumeStrokeThickness = 7;


        }


        /// <summary>
        /// Evento carregado ao terminar de carregar os componentres gráficos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            //setando o busy para on
            busyIndicator.IsBusy = true;

            //resgatando o historico
            marketDataDAO.GetCotacaoDiariaAsync(ativo);

            //assinando o realtime
            RealTimeDAO.TickReceived += new RealTimeDAO.TickHandler(RealTimeDAO_TickReceived);

        }

        void RealTimeDAO_TickReceived(object Result)
        {
            //se for de outro ativo devemos ignorar o tick
            if (((TickDTO)Result).Ativo == this.ativo)
            {
                TickDTO tick = (TickDTO)Result;

                //checando se a ultima barra
                DateTime ultimaBarra = _stockChartX.GetTimestampByIndex(_stockChartX.RecordCount - 1).Value;

                //checando qual o tipo de atualização
                switch (this.Periodicidade)
                {
                    case TerminalWEB.Periodicidade.Diario:
                        AtualizaGraficoDiario(ultimaBarra, tick);
                        break;
                    case TerminalWEB.Periodicidade.UmMinuto:
                        AtualizaGraficoIntraday(ultimaBarra, tick);
                        break;
                }
                if (this.Periodicidade == TerminalWEB.Periodicidade.Diario)
                {


                    _stockChartX.Update();
                }

                //atualizando o infpanel
                AtualizaInfoPanel();
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

            CarregaGrafico(this.layout, ativo, marketDataDAO.ConvertPeriodicidadeDiaria(Result, GetIntPeriodicidade()));

        }

        /// <summary>
        /// Evento que é rodado ao terminar de carregas as cotações intraday
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_GetCotacaoIntradayCompleted(List<CotacaoDTO> Result)
        {
            //setando o busy para false
            busyIndicator.IsBusy = false;

            CarregaGrafico(this.layout, ativo, marketDataDAO.ConvertPeriodicidadeIntraday(Result, GetIntPeriodicidade()));

        }

        /// <summary>
        /// Evento disparado apos se carregar o cache
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_SetCotacaoDiariaCacheCompleted()
        {
            marketDataDAO.GetCotacaoDiariaAsync(ativo);
        }



        #endregion

        #region Eventos Stockchart

        /// <summary>
        /// Evento disparado ao se levantar uma tecla que fora pressionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.PlatformKeyCode)
            {
                case (int)Key.Escape:
                    break;
                case (int)Key.Add:
                    ZoomIn();
                    break;
                case (int)Key.Subtract:
                case 189:
                    ZoomOut();
                    break;
                case (int)Key.Delete:
                    break;
                case (int)Key.Left:
                    _stockChartX.FirstVisibleRecord -= 5;
                    _stockChartX.LastVisibleRecord -= 5;
                    scrollbar.Value -= 5;
                    break;
                case (int)Key.Right:
                    scrollbar.Value += 5;
                    break;
            }
        }

        /// <summary>
        /// Evento disparado ao se terminar de dar zoom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_ChartZoom(object sender, EventArgs e)
        {
            scrollbar.Value = _stockChartX.FirstVisibleRecord;
            scrollbar.ViewportSize = _stockChartX.VisibleRecordCount;
            this.layout.VisibleRecords = _stockChartX.VisibleRecordCount;

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
                    borderDataPosicionada.Visibility = System.Windows.Visibility.Visible;
                    borderValorYPosicionado.Visibility = System.Windows.Visibility.Visible;
                    break;
                case StaticData.TipoAcao.Ferramenta:
                    _stockChartX.Cursor = Cursors.None;
                    switch (StaticData.tipoFerramenta)
                    {
                        case StaticData.TipoFerramenta.Compra:
                            customCursor.Source = new BitmapImage(new Uri("/TerminalWeb;component/images/buy.png", UriKind.RelativeOrAbsolute));
                            break;
                        case StaticData.TipoFerramenta.Vende:
                            customCursor.Source = new BitmapImage(new Uri("/TerminalWeb;component/images/sell.png", UriKind.RelativeOrAbsolute));
                            break;
                        case StaticData.TipoFerramenta.Signal:
                            customCursor.Source = new BitmapImage(new Uri("/TerminalWeb;component/images/signalprice.png", UriKind.RelativeOrAbsolute));
                            break;
                        default:
                            customCursor.Source = new BitmapImage(new Uri("/TerminalWeb;component/images/pencil-icon.png", UriKind.RelativeOrAbsolute));
                            break;
                    }

                    customCursor.Visibility = System.Windows.Visibility.Visible;
                    break;
                case StaticData.TipoAcao.Indicador:
                    _stockChartX.Cursor = Cursors.Hand;
                    break;
                case StaticData.TipoAcao.Zoom:
                    _stockChartX.Cursor = Cursors.None;
                    customCursor.Source = new BitmapImage(new Uri("/TerminalWeb;component/images/zoom-in-icon.png", UriKind.RelativeOrAbsolute));
                    customCursor.Visibility = System.Windows.Visibility.Visible;
                    _stockChartX.DisableZoomArea = false;
                    break;
                default:
                    _stockChartX.Cursor = Cursors.Arrow;
                    break;
            }
            #endregion

            #region Tratando o infopanel
            if (layout.PainelInfo.HasValue)
            {
                _stockChartX.InfoPanelPosition = InfoPanelPositionEnum.Hidden;
                if (layout.PainelInfo.Value)
                    infoPanel.Visibility = System.Windows.Visibility.Visible;
                else
                    infoPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            #endregion
        }

        /// <summary>
        /// Evento disparado ao se sair do componente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_MouseLeave(object sender, MouseEventArgs e)
        {
            customCursor.Visibility = System.Windows.Visibility.Collapsed;
            _stockChartX.DisableZoomArea = true;
            _stockChartX.CrossHairs = false;
            _stockChartX.Cursor = Cursors.Arrow;
            borderDataPosicionada.Visibility = System.Windows.Visibility.Collapsed;
            borderValorYPosicionado.Visibility = System.Windows.Visibility.Collapsed;


        }

        /// <summary>
        /// Evento disparado ao se mover o mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_ChartPanelMouseMove(object sender, StockChartX.ChartPanelMouseMoveArgs e)
        {
            registroX = _stockChartX.FirstVisibleRecord + e.Record;

            //Recuperando os valores de acorod com o registroX para alterar no infopanel
            AtualizaInfoPanel();


            if (StaticData.tipoAcao == StaticData.TipoAcao.CROSS)
            {

                if ((registroX > 0) && (registroX < _stockChartX.RecordCount))
                {
                    txtDataPosicionada.Text = _stockChartX.GetTimestampByIndex(registroX).Value.ToString("dd/MM/yyyy");
                    txtValorYPosicionado.Text = Math.Round(e.Y, 2).ToString();
                    borderDataPosicionada.Margin = new Thickness(e.MouseX, borderDataPosicionada.Margin.Top, borderDataPosicionada.Margin.Right, borderDataPosicionada.Margin.Bottom);
                }
            }
            else
            {
                if (CurrentState == EstadoGrafio.Editando)
                {
                    ListaLineStudy[ListaLineStudy.Count - 1].SetXYValues(ListaLineStudy[ListaLineStudy.Count - 1].X1Value,
                        ListaLineStudy[ListaLineStudy.Count - 1].Y1Value,
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

            switch (StaticData.tipoAcao)
            {
                case StaticData.TipoAcao.CROSS:
                    borderValorYPosicionado.Margin = new Thickness(0, e.GetPosition(_stockChartX).Y, 0, 0);
                    break;
                case StaticData.TipoAcao.Zoom:
                case StaticData.TipoAcao.Ferramenta:
                    customCursor.Margin = new Thickness(e.GetPosition(_stockChartX).X, e.GetPosition(_stockChartX).Y, 0, 0);
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
                            PromptTexto promptTexto = new PromptTexto();
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
                            line = _stockChartX.AddStaticText(e.Price.ToString(), Guid.NewGuid().ToString(),
                                new SolidColorBrush(StaticData.corSelecionada), 10, e.Panel.Index);
                        }

                    }
                    else
                    {
                        //TODOS OS OUTROS ESTUDOS
                        line = _stockChartX.CreateLineStudy(StaticData.LineStudySelecionado(),
                            Guid.NewGuid().ToString(),
                            new SolidColorBrush(StaticData.corSelecionada), e.Panel.Index);
                    }


                    line.IsContextMenuDisabled = true;
                    line.Selectable = false;
                    line.StrokeType = StaticData.estiloLinhaSelecionado;
                    line.StrokeThickness = StaticData.strokeTickeness;
                    int record = _stockChartX.GetReverseX(e.Timestamp.Value, true);
                    line.SetXYValues(record, e.Price, record, e.Price);
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
                    InserirIndicador(e.Panel);
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
            ctxMenu.Show(_stockChartX, e.GetPosition(_stockChartX));
        }

        /// <summary>
        /// Evento disparado ao se clicar o botao esquerdo sobre um objeto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _stockChartX_LineStudyLeftClick(object sender, StockChartX.LineStudyMouseEventArgs e)
        {

        }

        #endregion

        #region Eventos Form

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
                    _stockChartX.FirstVisibleRecord -= 5;
                    _stockChartX.LastVisibleRecord -= 5;
                    break;
                case System.Windows.Controls.Primitives.ScrollEventType.SmallIncrement:
                    _stockChartX.FirstVisibleRecord += 5;
                    if (_stockChartX.LastVisibleRecord + 5 < _stockChartX.RecordCount)
                        _stockChartX.LastVisibleRecord += 5;
                    else
                        _stockChartX.LastVisibleRecord = _stockChartX.RecordCount - 1;
                    break;
                case System.Windows.Controls.Primitives.ScrollEventType.LargeDecrement:
                    _stockChartX.FirstVisibleRecord -= 25;
                    _stockChartX.LastVisibleRecord -= 25;
                    break;
                case System.Windows.Controls.Primitives.ScrollEventType.LargeIncrement:
                    _stockChartX.LastVisibleRecord += 25;
                    _stockChartX.FirstVisibleRecord += 25;
                    break;
                case System.Windows.Controls.Primitives.ScrollEventType.ThumbTrack:
                    if (Convert.ToInt32(scrollbar.Value) > _stockChartX.LastVisibleRecord)
                    {
                        //sentenças invertidas
                        _stockChartX.LastVisibleRecord = Convert.ToInt32(scrollbar.Value);
                        _stockChartX.FirstVisibleRecord = Convert.ToInt32(scrollbar.Value) - this.layout.VisibleRecords;
                    }
                    else
                    {
                        if (Convert.ToInt32(scrollbar.Value) - this.layout.VisibleRecords > 0)
                        {
                            _stockChartX.FirstVisibleRecord = Convert.ToInt32(scrollbar.Value) - this.layout.VisibleRecords;
                            _stockChartX.LastVisibleRecord = Convert.ToInt32(scrollbar.Value);
                        }
                        else
                        {
                            _stockChartX.FirstVisibleRecord = 0;
                            _stockChartX.LastVisibleRecord = this.layout.VisibleRecords;
                        }
                    }
                    break;
                case System.Windows.Controls.Primitives.ScrollEventType.Last:
                    _stockChartX.FirstVisibleRecord = _stockChartX.RecordCount - 1 - this.layout.VisibleRecords;
                    _stockChartX.LastVisibleRecord = _stockChartX.RecordCount - 1;
                    break;
            }
            _stockChartX.Update();

        }

        #region InfoPanel

        /// <summary>
        /// Evento disparado ao se passar o mouse sobre o info panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void infoPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            if (infoPanel.Margin.Left == 0)
                infoPanel.Margin = new Thickness(200, 0, 0, 0);
            else
                infoPanel.Margin = new Thickness(0, 0, 0, 0);
        }

        /// <summary>
        /// Evento executado ao se clicar sobre o botao de infopanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarPainelInformacoes_Click(object sender, RoutedEventArgs e)
        {
            if (tbarPainelInformacoes.IsChecked.Value)
                SetInfoPanelVisibility(System.Windows.Visibility.Visible);
            else
                SetInfoPanelVisibility(System.Windows.Visibility.Collapsed);
            
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
            Refresh();
        }
               

        /// <summary>
        /// Evento disparado ao se clicar no botão 1 minuto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar1Minuto_Click(object sender, RoutedEventArgs e)
        {
            SetPeriodicidade(Periodicidade.UmMinuto);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 2 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar2Minutos_Click(object sender, RoutedEventArgs e)
        {
            SetPeriodicidade(Periodicidade.DoisMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 3 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar3Minutos_Click(object sender, RoutedEventArgs e)
        {
            SetPeriodicidade(Periodicidade.TresMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 5 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar5Minutos_Click(object sender, RoutedEventArgs e)
        {
            SetPeriodicidade(Periodicidade.CincoMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 10 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar10Minutos_Click(object sender, RoutedEventArgs e)
        {
            SetPeriodicidade(Periodicidade.DezMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 15 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar15Minutos_Click(object sender, RoutedEventArgs e)
        {
            SetPeriodicidade(Periodicidade.QuinzeMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 30 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar30Minutos_Click(object sender, RoutedEventArgs e)
        {
            SetPeriodicidade(Periodicidade.TrintaMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 60 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar60Minutos_Click(object sender, RoutedEventArgs e)
        {
            SetPeriodicidade(Periodicidade.SessentaMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão 120 minutos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar120Minutos_Click(object sender, RoutedEventArgs e)
        {
            SetPeriodicidade(Periodicidade.CentoeVinteMinutos);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão Diario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarDiario_Click(object sender, RoutedEventArgs e)
        {
            SetPeriodicidade(Periodicidade.Diario);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão Semanal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarSemanal_Click(object sender, RoutedEventArgs e)
        {
            SetPeriodicidade(Periodicidade.Semanal);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão Mensal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbarMensal_Click(object sender, RoutedEventArgs e)
        {
            SetPeriodicidade(Periodicidade.Mensal);            
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
            SetTipoBarra(SeriesTypeEnum.stCandleChart);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar sobre o botao de tipo de barra barra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void tbarBarra_Click(object sender, RoutedEventArgs e)
        {
            SetTipoBarra(SeriesTypeEnum.stStockBarChart);
            
        }

        /// <summary>
        /// Evento disparado ao se clicar sobre o botao de tipo de barra linha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void tbarLinha_Click(object sender, RoutedEventArgs e)
        {
            SetTipoBarra(SeriesTypeEnum.stLineChart);
        }


        #endregion

        #endregion

        #region Metodos Privados

        #region StockChart

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

            _stockChartX.Update();
        }

        /// <summary>
        /// Metodo que faz as configurações iniciais de layout
        /// </summary>
        /// <param name="layout"></param>
        private void CarregaGrafico(TerminalWebSVC.LayoutDTO layout, string ativo, List<CotacaoDTO> listaCotacao)
        {

            //Retirando todas as series do gráfico
            _stockChartX.ApplyTemplate();
            _stockChartX.ClearAll();

            //setando o ativo
            _stockChartX.Symbol = ativo;

            //desabilitando o zoom area
            _stockChartX.DisableZoomArea = true;

            //checando se é o grafico do layuout 1
            if (this.LayoutUm)
            {
                _stockChartX.DisplayTitles = false;
            }

            //setando o tipo de calendario
            _stockChartX.CalendarVersion = CalendarVersionType.Version1;

            _stockChartX.IndicatorDialogBackground = Brushes.Black;
            _stockChartX.IndicatorTwinTitleVisibility = System.Windows.Visibility.Collapsed;


            //Criando paineis padrões
            ChartPanel topPanel = _stockChartX.AddChartPanel(ChartPanel.PositionType.AlwaysTop);
            ChartPanel volumePanel = _stockChartX.AddChartPanel(ChartPanel.PositionType.AlwaysBottom);


            //setando as cores de fundo
            topPanel.Background = volumePanel.Background = Util.GetColorFromHexa(layout.CorFundo);

            //adicionando series
            Series[] ohlcSeries = _stockChartX.AddOHLCSeries(_stockChartX.Symbol, topPanel.Index);
            Series seriesVolume = _stockChartX.AddVolumeSeries(_stockChartX.Symbol, volumePanel.Index);

            foreach (CotacaoDTO obj in listaCotacao)
            {
                _stockChartX.AppendOHLCValues(_stockChartX.Symbol, obj.Data, obj.Abertura, obj.Maximo, obj.Minimo, obj.Ultimo);
                _stockChartX.AppendVolumeValue(_stockChartX.Symbol, obj.Data, obj.Volume);
            }

            //setando as cores das series padrao
            ohlcSeries[0].StrokeColor = Util.GetColorFromHexa(layout.CorMaximaMinima).Color;
            ohlcSeries[1].StrokeColor = Util.GetColorFromHexa(layout.CorMaximaMinima).Color;
            ohlcSeries[2].StrokeColor = Util.GetColorFromHexa(layout.CorMaximaMinima).Color;
            ohlcSeries[3].StrokeColor = Util.GetColorFromHexa(layout.CorMaximaMinima).Color;

            //setando o tickbox em cima doa valor do ultimo na direita
            ohlcSeries[3].TickBox = TickBoxPosition.Right;

            //carcateristicas do painel inicial
            if (this.LayoutUm)
            {

            }

            //setando as configurações do volume
            seriesVolume.StrokeColor = Util.GetColorFromHexa(layout.CorVolume).Color;
            seriesVolume.StrokeThickness = layout.VolumeStrokeThickness;

            //caracteristicas da escala
            _stockChartX.ScaleAlignment = (ScaleAlignmentTypeEnum)layout.PosicaoEscala.Value;

            //espaço ao lado direito do gráfico
            _stockChartX.RightChartSpace = layout.EspacoADireitaDoGrafico.Value;

            //Realtime ou nao            
            _stockChartX.RealTimeXLabels = this.Intraday();

            //Checando a compressao das barras
            _stockChartX.TickCompressionType = TickCompressionEnum.Time;

            switch (this.Periodicidade)
            {
                case Periodicidade.UmMinuto:
                    _stockChartX.TickPeriodicity = 60;
                    break;
                case Periodicidade.DoisMinutos:
                    _stockChartX.TickPeriodicity = 120;
                    break;
                case Periodicidade.TresMinutos:
                    _stockChartX.TickPeriodicity = 180;
                    break;
                case Periodicidade.CincoMinutos:
                    _stockChartX.TickPeriodicity = 300;
                    break;
            }


            _stockChartX.SetPanelHeight(0, _stockChartX.ActualHeight * 0.75);

            //setando as caracteristicas da Scrollbar
            scrollbar.ViewportSize = Util.LayoutFake().VisibleRecords;
            scrollbar.Maximum = _stockChartX.RecordCount;
            scrollbar.Minimum = 0;

            if (Util.LayoutFake().ZoomRealtime)
            {
                _stockChartX.FirstVisibleRecord = _stockChartX.RecordCount - Util.LayoutFake().VisibleRecords;
                _stockChartX.LastVisibleRecord = _stockChartX.FirstVisibleRecord + Util.LayoutFake().VisibleRecords;
                scrollbar.Value = scrollbar.Maximum;
            }
            else
            {
                _stockChartX.FirstVisibleRecord = Util.LayoutFake().FirstVisibleRecord;
                _stockChartX.LastVisibleRecord = _stockChartX.FirstVisibleRecord + Util.LayoutFake().VisibleRecords;
                scrollbar.Value = _stockChartX.LastVisibleRecord;
            }


            _stockChartX.GridStroke = new SolidColorBrush(Color.FromArgb(0x33, 0xCC, 0xCC, 0xCC));
            _stockChartX.ThreeDStyle = true;

            _stockChartX.OptimizePainting = true;
            _stockChartX.Update();

            //setando o estado do gráfico para aguardando alguma operação
            this.CurrentState = EstadoGrafio.Nenhum;

            var lbl = _stockChartX.GetPanelByIndex(0).ChartPanelLabel;
            lbl.FontSize = 30;
            lbl.Margin = new Thickness(20, 20, 0, 0);
            lbl.Foreground = Brushes.Black;
            lbl.Foreground.Opacity = 0.2;
            lbl.Text = "ATG\nPowered by Traderdata";

            //setando o layout
            SetSkinClear();
        }

        /// <summary>
        /// Metodo que faz a inserção do indcador no painel selecionado
        /// </summary>
        /// <param name="panel"></param>
        private void InserirIndicador(ChartPanel panel)
        {

            Indicator indicadorTemp = _stockChartX.AddIndicator(StaticData.tipoIndicador,
                Guid.NewGuid().ToString(), panel, false);
            IList<StockChartX_IndicatorsParameters.IndicatorParameter> a = indicadorTemp.IndicatorParams;
            //indicadorTemp.ShowParametersDialog();

            //_stockChartX.Cursor = Cursors.Arrow;
            //StaticData.ferramentaSelecionada = "SETA";
            _stockChartX.Update();
            CurrentState = EstadoGrafio.Nenhum;

        }

        /// <summary>
        /// Metodo que faz a atualização do infopanel de acordo com o posicionamento do mouse
        /// </summary>
        private void AtualizaInfoPanel()
        {
            if ((registroX > 0) && (registroX < _stockChartX.RecordCount))
            {
                List<InfoPanelItemDTO> itens = new List<InfoPanelItemDTO>();
                itens.Add(new InfoPanelItemDTO("Abertura", _stockChartX.GetValue(_stockChartX.Symbol + ".open", registroX).Value.ToString("0.00", Util.NumberProvider), false));
                itens.Add(new InfoPanelItemDTO("Minimo", _stockChartX.GetValue(_stockChartX.Symbol + ".low", registroX).Value.ToString("0.00", Util.NumberProvider), false));
                itens.Add(new InfoPanelItemDTO("Maximo", _stockChartX.GetValue(_stockChartX.Symbol + ".high", registroX).Value.ToString("0.00", Util.NumberProvider), false));
                itens.Add(new InfoPanelItemDTO("Ultimo", _stockChartX.GetValue(_stockChartX.Symbol + ".close", registroX).Value.ToString("0.00", Util.NumberProvider), false));
                itens.Add(new InfoPanelItemDTO("", "", true));
                itens.Add(new InfoPanelItemDTO("Volume", (_stockChartX.GetValue(_stockChartX.Symbol + ".volume", registroX).Value / 1000000).ToString("0.00", Util.NumberProvider) + "M", false));
                infoPanel.SetInfo(itens, _stockChartX.GetTimestampByIndex(registroX).Value);
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
            if (ultimaBarra == tick.Data.Date)
            {
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".open",
                    _stockChartX.RecordCount - 1, tick.Abertura);
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".high",
                    _stockChartX.RecordCount - 1, tick.Maximo);
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".low",
                    _stockChartX.RecordCount - 1, tick.Minimo);
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".close",
                    _stockChartX.RecordCount - 1, tick.Ultimo);
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".volume",
                    _stockChartX.RecordCount - 1, tick.Volume);
            }
            else
            {
                _stockChartX.AppendOHLCVValues(_stockChartX.Symbol, tick.Data, tick.Abertura, tick.Maximo,
                    tick.Minimo, tick.Ultimo, tick.Volume, false);
            }

            this.RefreshLayout();

        }

        /// <summary>
        /// Metodo que faz a atualização da barra intraday de 1 minuto
        /// </summary>
        /// <param name="ultimaBarra"></param>
        /// <param name="tick"></param>
        private void AtualizaGraficoIntraday(DateTime ultimaBarra, TickDTO tick)
        {
            DateTime tickDateCompleta = new DateTime(tick.Data.Year, tick.Data.Month, tick.Data.Day,
                Convert.ToInt32(tick.Hora.Substring(0, 2)), Convert.ToInt32(tick.Hora.Substring(4, 2)), 0);
            if (ultimaBarra == tickDateCompleta)
            {
                int index = _stockChartX.GetReverseX(ultimaBarra, false);

                if (tick.Ultimo > _stockChartX.GetValue(_stockChartX.Symbol + ".high", ultimaBarra))
                    _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".high", index, tick.Maximo);

                if (tick.Ultimo < _stockChartX.GetValue(_stockChartX.Symbol + ".low", ultimaBarra))
                    _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".low", index, tick.Minimo);

                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".close",
                    index, tick.Ultimo);
                _stockChartX.EditValueByRecord(_stockChartX.Symbol + ".volume",
                    index, tick.VolumeUltimoMinuto);
            }
            else
            {
                _stockChartX.AppendOHLCVValues(_stockChartX.Symbol, tickDateCompleta, tick.Ultimo, tick.Ultimo,
                    tick.Ultimo, tick.Ultimo, tick.VolumeUltimoMinuto, false);
            }
        }

        #endregion

        #endregion

        #region Metodos Publicos

        #region Refresh Dados & Periodicidade

        /// <summary>
        /// Metodo que retorna as cotações e recarrega o gráfico
        /// </summary>
        public void Refresh()
        {
            SetPeriodicidade(this.Periodicidade);
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
        /// Metodo que faz o refresh alterando a periodicidade
        /// </summary>
        /// <param name="periodicidade"></param>
        public void SetPeriodicidade(Periodicidade periodicidade)
        {
            //setando o busy para on
            busyIndicator.IsBusy = true;

            this.Periodicidade = periodicidade;

            if (Intraday())
                marketDataDAO.GetCotacaoIntradayAsync(this.ativo);
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

        /// <summary>
        /// Metodo que retorna a periodicidade do gráfico
        /// </summary>
        /// <returns></returns>
        private int GetIntPeriodicidade()
        {
            switch (this.Periodicidade)
            {
                case TerminalWEB.Periodicidade.UmMinuto:
                    return 1;
                case TerminalWEB.Periodicidade.DoisMinutos:
                    return 2;
                case TerminalWEB.Periodicidade.TresMinutos:
                    return 3;
                case TerminalWEB.Periodicidade.CincoMinutos:
                    return 5;
                case TerminalWEB.Periodicidade.DezMinutos:
                    return 10;
                case TerminalWEB.Periodicidade.QuinzeMinutos:
                    return 15;
                case TerminalWEB.Periodicidade.TrintaMinutos:
                    return 30;
                case TerminalWEB.Periodicidade.SessentaMinutos:
                    return 60;
                case TerminalWEB.Periodicidade.CentoeVinteMinutos:
                    return 120;
                case TerminalWEB.Periodicidade.Diario:
                    return 1;
                case TerminalWEB.Periodicidade.Semanal:
                    return 7;
                case TerminalWEB.Periodicidade.Mensal:
                    return 30;
                default:
                    return 1;
            }
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
            borderDataPosicionada.Background = Brushes.Red;
            borderValorYPosicionado.Background = Brushes.Red;
            txtValorYPosicionado.Foreground = Brushes.White;
            txtDataPosicionada.Foreground = Brushes.White;

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
        /// Seta um skin de cores como o Enfoque
        /// </summary>
        public void SetSkinClear()
        {
            //selecionando o cross
            StaticData.tipoAcao = StaticData.TipoAcao.CROSS;

            //setando as configuraçoes do cross
            _stockChartX.CrossHairsStroke = Brushes.DarkRed;
            borderDataPosicionada.Background = Brushes.Red;
            borderValorYPosicionado.Background = Brushes.Red;
            txtValorYPosicionado.Foreground = Brushes.White;
            txtDataPosicionada.Foreground = Brushes.White;

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
            _stockChartX.CandleDownOutlineColor = Colors.Red;
            _stockChartX.CandleUpOutlineColor = Colors.Green;
            _stockChartX.UpColor = Colors.Green;
            _stockChartX.DownColor = Colors.Red;
            _stockChartX.CalendarBackground = Brushes.White;
            gridPrincipal.Background = Brushes.White;
            canvasAbaixoStockchart.Background = Brushes.White;
            _stockChartX.XGrid = true;
            _stockChartX.YGrid = true;
            this.Background = Brushes.White;
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

        #region Publicação

        /// <summary>
        /// Metodo que gera os bytes no formato Png a partir do componente
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private byte[] GetBytesInternalPng(UIElement element)
        {
            WriteableBitmap w = new WriteableBitmap(element, new TranslateTransform());
            EditableImage imageData = new EditableImage(w.PixelWidth, w.PixelHeight);

            try
            {
                for (int y = 0; y < w.PixelHeight; ++y)
                {
                    for (int x = 0; x < w.PixelWidth; ++x)
                    {
                        int pixel = w.Pixels[w.PixelWidth * y + x];
                        imageData.SetPixel(x, y,

                                           (byte)((pixel >> 16) & 0xFF),
                                           (byte)((pixel >> 8) & 0xFF),
                                           (byte)(pixel & 0xFF), (byte)((pixel >> 24) & 0xFF)
                          );
                    }
                }
            }
            catch (System.Security.SecurityException)
            {
                MessageBox.Show("Cannot print images from other domains");
                return null;
            }

            Stream pngStream = imageData.GetStream();
            StreamReader sr = new StreamReader(pngStream);
            byte[] binaryData = new Byte[pngStream.Length];
            pngStream.Read(binaryData, 0, (int)pngStream.Length);

            return binaryData;
        }

        /// <summary>
        /// Metodo quer vai publicar gráfico no mural do usuairo que se conectar
        /// </summary>
        public void PublishFacebook()
        {
            PromptTexto promptComentario = new PromptTexto();

            promptComentario.Closing += (sender1, e1) =>
            {
                if (promptComentario.DialogResult.Value == true)
                {

                    busyIndicator.BusyContent = "Publicando no Facebook";
                    busyIndicator.IsBusy = true;

                    //fazendo upload da imagem para um compartilhamento temporario na S3
                    string bucketName = "traderdata-temp";
                    byte[] imagem = GetBytesInternalPng(_stockChartX);
                    string fileName = Guid.NewGuid().ToString() + ".png";
                    TerminalWebSVC.TerminalWebClient client = new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());
                    client.UploadFileS3Completed += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_UploadFileS3Completed);
                    client.UploadFileS3Async(bucketName, fileName, imagem, StaticData.S3Endpoint + bucketName + "/" + fileName + "@" + promptComentario.Texto);

                }
            };
            promptComentario.Show();

        }

        /// <summary>
        /// Evento executado ao terminal de fazer o upload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_UploadFileS3Completed(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            busyIndicator.IsBusy = false;
            //publicando no facebook
            List<object> listaParametros = new List<object>();

            string mensagem = ((string)e.UserState).Split('@')[1];
            string arquivo = ((string)e.UserState).Split('@')[0];


            listaParametros.Add(mensagem + " - Powered by Traderdata");
            listaParametros.Add(arquivo);

            HtmlPage.Window.Invoke("postPhoto", listaParametros.ToArray());

        }

        #endregion

        #region Zoom

        /// <summary>
        /// Metodo que faz o zoomIn
        /// </summary>
        public void ZoomIn()
        {
            _stockChartX.DisableZoomArea = true;
            _stockChartX.FirstVisibleRecord += 10;
            this.layout.VisibleRecords = _stockChartX.VisibleRecordCount;
            scrollbar.ViewportSize = this.layout.VisibleRecords;

        }

        /// <summary>
        /// Metodo que faz o ZoomOut
        /// </summary>
        public void ZoomOut()
        {
            if (_stockChartX.FirstVisibleRecord - 10 > 0)
                _stockChartX.FirstVisibleRecord -= 10;
            else
                _stockChartX.FirstVisibleRecord = 0;

            this.layout.VisibleRecords = _stockChartX.VisibleRecordCount;
            scrollbar.ViewportSize = this.layout.VisibleRecords;

        }

        /// <summary>
        /// Metodo publico que faz um resset no gráfico
        /// </summary>
        public void ResetZoom()
        {
            _stockChartX.LastVisibleRecord = _stockChartX.RecordCount;
            if (_stockChartX.RecordCount > 50)
                _stockChartX.FirstVisibleRecord = _stockChartX.RecordCount - 50;
            else
                _stockChartX.FirstVisibleRecord = 0;

            scrollbar.Value = _stockChartX.RecordCount - 1;
            scrollbar.ViewportSize = _stockChartX.VisibleRecordCount;
            this.layout.VisibleRecords = _stockChartX.VisibleRecordCount;

        }

        #endregion

        #region Propriedades dos Objetos

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
            if (visibility == System.Windows.Visibility.Visible)
                this.layout.PainelInfo = true;
            else
                this.layout.PainelInfo = false;

            this.infoPanel.Visibility = visibility;
        }

        /// <summary>
        /// Metodo que retorna se esse infopainel esta visivel ou nao.
        /// </summary>
        /// <returns></returns>
        public bool GetInfoPainelVisibility()
        {
            return this.layout.PainelInfo.Value;
        }

        #endregion




        #endregion




    }
}
