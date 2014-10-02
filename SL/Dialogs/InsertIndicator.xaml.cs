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
using Traderdata.Client.TerminalWEB.DTO;
using Traderdata.Client.TerminalWEB.DAO;
using ModulusFE.SL;
using ModulusFE;

namespace Traderdata.Client.TerminalWEB.Dialogs
{
    public partial class InsertIndicator : ChildWindow
    {
        #region Variáveis

        /// <summary>
        /// variavel que armazena o indicador editado
        /// </summary>
        private IndicatorType indicadorEditado = IndicatorType.Unknown;

        /// <summary>
        /// Variavel de identificaçao de tela inteira ou nao
        /// </summary>
        private bool TelaInsert = false;

        /// <summary>
        /// Painel onde deve ser inserido o indicador
        /// </summary>
        public ChartPanel ChartPanel = new ChartPanel();

        /// <summary>
        /// Lista de propriedades do´indicador
        /// </summary>
        public List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

        /// <summary>
        /// Lista de series
        /// </summary>
        private List<Series> listaSeries = new List<Series>();

        /// <summary>
        /// Lista de paineis
        /// </summary>
        private List<ChartPanel> listaPaineis = new List<ChartPanel>();

        /// <summary>
        /// Lista  de ativos
        /// </summary>
        private List<string> listaAtivos = new List<string>();

        /// <summary>
        /// Ativo default
        /// </summary>
        private string defaultAtivo = "";

        /// <summary>
        /// Variavel que armazena a serie 
        /// </summary>
        private string defaultSerie = "";

        /// <summary>
        /// Variavel que recebe o numero de barras presentes no grafico
        /// </summary>
        private int numeroBarras = 0;

        #endregion

        #region Construtor

        /// <summary>
        /// Construtor para inserção
        /// </summary>
        /// <param name="insertScreen"></param>
        /// <param name="listaSeries"></param>
        /// <param name="listaPaineis"></param>
        /// <param name="defaultSerie"></param>
        /// <param name="defaultAtivo"></param>
        /// <param name="listaAtivos"></param>
        public InsertIndicator(bool insertScreen, List<Series> listaSeries, List<ChartPanel> listaPaineis, 
            string defaultSerie, string defaultAtivo, List<string> listaAtivos, int numeroBarras)
        {
            InitializeComponent();
            this.TelaInsert = insertScreen;
            this.listaSeries = listaSeries;
            this.defaultSerie = defaultSerie;
            this.listaPaineis = listaPaineis;
            this.listaAtivos = listaAtivos;
            this.defaultAtivo = defaultAtivo;
            this.numeroBarras = numeroBarras;

            //inserindo paineis na combo de paineis
            foreach (ChartPanel obj in listaPaineis)
            {
                cmbPaineis.Items.Add(obj.Name);
            }
            cmbPaineis.SelectedIndex = 0;

            
        }

        /// <summary>
        /// Construtor para caso de edição
        /// </summary>
        /// <param name="insertScreen"></param>
        /// <param name="listaSeries"></param>
        /// <param name="listaPaineis"></param>
        /// <param name="defaultSerie"></param>
        /// <param name="defaultAtivo"></param>
        /// <param name="listaAtivos"></param>
        public InsertIndicator(bool insertScreen, List<Series> listaSeries, List<ChartPanel> listaPaineis,
            string defaultSerie, string defaultAtivo, List<string> listaAtivos, IndicatorType indicadorEditado,
            Color corIndicador, int grossura, LinePattern tipoLinha, int numeroBarras)
        {
            InitializeComponent();
            this.TelaInsert = insertScreen;
            this.listaSeries = listaSeries;
            this.defaultSerie = defaultSerie;
            this.listaPaineis = listaPaineis;
            this.listaAtivos = listaAtivos;
            this.defaultAtivo = defaultAtivo;
            this.indicadorEditado = indicadorEditado;
            this.numeroBarras = numeroBarras;

            //inserindo paineis na combo de paineis
            foreach (ChartPanel obj in listaPaineis)
            {
                cmbPaineis.Items.Add(obj.Name);
            }
            cmbPaineis.SelectedIndex = 0;
                        
        }

        #endregion Construtor

        #region Eventos

        /// <summary>
        /// Evento disparao ao se clicar sobre o checkBox de novo painel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            cmbPaineis.IsEnabled = !(bool)chkNewPanel.IsChecked;
        }

        /// <summary>
        /// Evento disparado ao carregar o form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            listBoxIndicadores.Items.Clear();
            listBoxIndicadores.DisplayMemberPath = "NomePortugues";
            listBoxIndicadores.ItemsSource = StaticData.GetListaIndicadores();
            listBoxIndicadores.SelectedIndex = 0;

            //checando se for edição devo bloquear alguns itens
            if (!TelaInsert)
            {
                foreach (IndicatorInfoDTO obj in listBoxIndicadores.Items)
                {
                    if (obj.TipoStockchart == indicadorEditado)
                    {
                        listBoxIndicadores.SelectedItem = obj;
                        break;
                    }
                }

                listBoxIndicadores.SelectedItem = indicadorEditado;
                chkNewPanel.IsEnabled = false;
                cmbPaineis.IsEnabled = false;
                listBoxIndicadores.IsEnabled = false;
            }
        }

        /// <summary>
        /// Evento disparado ao se clicar no botão Ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            //setando o painel correto
            if (!(bool)chkNewPanel.IsChecked)
            {
                foreach (ChartPanel obj in listaPaineis)
                {
                    if (obj.Name == (string)cmbPaineis.SelectedItem)
                    {
                        this.ChartPanel = obj;
                        break;
                    }
                }
            }
            else
                this.ChartPanel = null;

            //setando as propriedades
            foreach (StackPanel obj in stackParametros.Children)
            {
                switch ((TipoField)obj.Tag)
                {
                    case TipoField.NumericUpDownInteger:
                        foreach (object objInterno in obj.Children)
                        {
                            if (objInterno.GetType().ToString().Contains("NumericUpDown"))
                            {
                                if (((NumericUpDown)objInterno).Tag != null)
                                {
                                    if (((NumericUpDown)objInterno).Tag.GetType().ToString().Contains("IndicatorPropertyDTO"))
                                    {
                                        ((IndicatorPropertyDTO)((NumericUpDown)objInterno).Tag).Value = ((NumericUpDown)objInterno).Value;
                                        listaPropriedades.Add((IndicatorPropertyDTO)((NumericUpDown)objInterno).Tag);
                                    }
                                }
                            }

                        }
                        break;
                    case TipoField.Double:
                        foreach (object objInterno in obj.Children)
                        {
                            if (objInterno.GetType().ToString().Contains("NumericUpDown"))
                            {
                                if (((NumericUpDown)objInterno).Tag != null)
                                {
                                    if (((NumericUpDown)objInterno).Tag.GetType().ToString().Contains("IndicatorPropertyDTO"))
                                    {
                                        ((IndicatorPropertyDTO)((NumericUpDown)objInterno).Tag).Value = ((NumericUpDown)objInterno).Value;
                                        listaPropriedades.Add((IndicatorPropertyDTO)((NumericUpDown)objInterno).Tag);
                                    }
                                }
                            }

                        }
                        break;
                    case TipoField.Serie:
                        foreach (object objInterno in obj.Children)
                        {
                            if (objInterno.GetType().ToString().Contains("ComboBox"))
                            {
                                if (((ComboBox)objInterno).Tag.GetType().ToString().Contains("IndicatorPropertyDTO"))
                                {
                                    ((IndicatorPropertyDTO)((ComboBox)objInterno).Tag).Value = ((ComboBox)objInterno).SelectedItem;
                                    listaPropriedades.Add((IndicatorPropertyDTO)((ComboBox)objInterno).Tag);
                                }
                            }

                        }
                        break;
                    case TipoField.Media:
                        foreach (object objInterno in obj.Children)
                        {
                            if (objInterno.GetType().ToString().Contains("ComboBox"))
                            {
                                if (((ComboBox)objInterno).Tag.GetType().ToString().Contains("IndicatorPropertyDTO"))
                                {
                                    ((IndicatorPropertyDTO)((ComboBox)objInterno).Tag).Value = ((ComboBox)objInterno).SelectedItem;
                                    listaPropriedades.Add((IndicatorPropertyDTO)((ComboBox)objInterno).Tag);
                                }
                            }

                        }
                        break;
                    case TipoField.SymbolList:
                        foreach (object objInterno in obj.Children)
                        {
                            if (objInterno.GetType().ToString().Contains("ComboBox"))
                            {
                                if (((ComboBox)objInterno).Tag.GetType().ToString().Contains("IndicatorPropertyDTO"))
                                {
                                    ((IndicatorPropertyDTO)((ComboBox)objInterno).Tag).Value = ((ComboBox)objInterno).SelectedItem;
                                    listaPropriedades.Add((IndicatorPropertyDTO)((ComboBox)objInterno).Tag);
                                }
                            }

                        }
                        break;

                }


            }

            if (ValidaPreenchimento())            
                this.DialogResult = true;
        }

        /// <summary>
        /// Evento disparado ao se clicar sobre o botao Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Evento disparado quando o item na listBox é selecionado
        /// </summary>
        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //limpando o canvas de parametros
            stackParametros.Children.Clear();

            //Recupera o nome do indicador selecionado
            IndicatorInfoDTO indicador = (IndicatorInfoDTO)listBoxIndicadores.SelectedItem;
            lblIndicador.Content = indicador.NomePortugues;

            //setando como novo painel ou nao
            chkNewPanel.IsChecked = indicador.NovoPainel;
            cmbPaineis.IsEnabled = !indicador.NovoPainel;

            
            //montar o canvas de acordo com as propriedades
            foreach (IndicatorPropertyDTO obj in indicador.Propriedades)
            {
                StackPanel painelInterno = new StackPanel();
                painelInterno.Orientation = Orientation.Horizontal;
                painelInterno.Margin = new Thickness(0, 5, 0, 0);
                painelInterno.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                painelInterno.Tag = obj.TipoDoCampo;
                TextBlock textBlock = new TextBlock();
                textBlock.Text = obj.Label;
                textBlock.Width = 100;
                textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                painelInterno.Children.Add(textBlock);

                switch (obj.TipoDoCampo)
                {
                    case TipoField.Header:
                        textBlock.Margin = new Thickness(10, 0, 0, 0);
                        textBlock.FontWeight = FontWeights.Bold;
                        textBlock.FontSize = 11;
                        break;
                    case TipoField.NumericUpDownInteger:
                        textBlock.Margin = new Thickness(30, 0, 0, 0);
                        NumericUpDown numberField = new NumericUpDown();
                        numberField.Width = 60;
                        numberField.DecimalPlaces = 0;
                        numberField.Tag = obj;
                        numberField.Value = Convert.ToDouble(obj.Value);
                        painelInterno.Children.Add(numberField);
                        break;
                    case TipoField.Double:
                        textBlock.Margin = new Thickness(30, 0, 0, 0);
                        NumericUpDown numberDouble = new NumericUpDown();
                        numberDouble.DecimalPlaces = 2;
                        numberDouble.Width = 60;
                        numberDouble.Tag = obj;
                        numberDouble.Value = Convert.ToDouble(obj.Value);
                        painelInterno.Children.Add(numberDouble);
                        break;
                    case TipoField.Media:
                        textBlock.Margin = new Thickness(30, 0, 0, 0);
                        ComboBox cmbTipoMedia = new ComboBox();
                        cmbTipoMedia.Margin = new Thickness(00, 0, 0, 10);
                        cmbTipoMedia.Items.Add("Simples");
                        cmbTipoMedia.Items.Add("Exponencial");
                        cmbTipoMedia.Items.Add("TimeSeries");
                        cmbTipoMedia.Items.Add("Triangular");
                        cmbTipoMedia.Items.Add("Variável");
                        cmbTipoMedia.Items.Add("VYDIA");
                        cmbTipoMedia.Items.Add("Ponderada");
                        cmbTipoMedia.Width = 100;
                        cmbTipoMedia.Height = 20;
                        cmbTipoMedia.Tag = obj;
                        cmbTipoMedia.SelectedItem = (string)obj.Value;
                        painelInterno.Children.Add(cmbTipoMedia);
                        break;
                    case TipoField.Serie:
                        textBlock.Margin = new Thickness(30, 0, 0, 0);
                        ComboBox cmbSerie = new ComboBox();
                        foreach (Series serie in this.listaSeries)
                        {
                            cmbSerie.Items.Add(serie.FullName.ToUpper());
                        }

                        cmbSerie.Margin = new Thickness(00, 0, 0,0);
                        cmbSerie.Width = 130;
                        cmbSerie.Height = 20;
                        cmbSerie.Tag = obj;
                        cmbSerie.SelectedItem = defaultAtivo + obj.Value;
                        painelInterno.Children.Add(cmbSerie);
                        break;
                    case TipoField.SymbolList:
                        textBlock.Margin = new Thickness(30, 0, 0, 0);
                        ComboBox cmbSymbolList = new ComboBox();
                        foreach (string ativo in this.listaAtivos)
                        {
                            cmbSymbolList.Items.Add(ativo);
                        }

                        cmbSymbolList.Margin = new Thickness(00, 0, 0, 0);
                        cmbSymbolList.Width = 130;
                        cmbSymbolList.Height = 20;
                        cmbSymbolList.Tag = obj;
                        cmbSymbolList.SelectedItem = defaultAtivo;
                        painelInterno.Children.Add(cmbSymbolList);
                        break;

                }

                if ((obj.TipoDoCampo == TipoField.SymbolList))
                {
                    painelInterno.Visibility = System.Windows.Visibility.Collapsed;
                }

                stackParametros.Children.Add(painelInterno);


            }
            if (indicador.Ajuda != null)
                ajuda.Text = indicador.Ajuda;

        }


        /// <summary>
        /// Evendo disparado ao teclar a teclar Enter
        /// </summary>
        private void ChildWindow_KeyDown(object sender, KeyEventArgs e)
        {
            //Dispara o ok se a tecla enter for pressionada
            if (e.Key == Key.Enter)
                OKButton_Click(this, e);

            //Dispara o cancel se a tecla ESC for pressionada
            else if (e.Key == Key.Escape)
                CancelButton_Click(this, e);
        }

        #endregion Eventos

        #region Metodos Privados

        /// <summary>
        /// Metodo que faz as validações para cada indicador
        /// </summary>
        /// <returns></returns>
        private bool ValidaPreenchimento()
        {
            IndicatorInfoDTO indicador = (IndicatorInfoDTO)listBoxIndicadores.SelectedItem;
            string msg = "";
            switch (indicador.TipoStockchart)
            {

                case IndicatorType.WilliamsAccumulationDistribution:
                    msg = IndicadorDAO.ValidaWilliamsAccumulationDistribution(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.Aroon:
                    msg = IndicadorDAO.ValidaAroon(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.BollingerBands:
                    msg = IndicadorDAO.ValidaBollingerBands(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.FractalChaosBands:
                    msg = IndicadorDAO.ValidaFractalChaosBands(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.PrimeNumberBands:
                    msg = IndicadorDAO.ValidaPrimeNumberBands(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.HighLowBands:
                    msg = IndicadorDAO.ValidaHighLowBands(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.ChaikinMoneyFlow:
                    msg = IndicadorDAO.ValidaChaikinMoneyFlow(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.ChaikinVolatility:
                    msg = IndicadorDAO.ValidaChaikinVolatility(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.CommodityChannelIndex:
                    msg = IndicadorDAO.ValidaCommodityChannelIndex(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.EaseOfMovement:
                    msg = IndicadorDAO.ValidaEaseOfMovement(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.MovingAverageEnvelope:
                    msg = IndicadorDAO.ValidaMovingAverageEnvelope(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.WeightedClose:
                    msg = IndicadorDAO.ValidaWeightedClose(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.VerticalHorizontalFilter:
                    msg = IndicadorDAO.ValidaVerticalHorizontalFilter(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.HighMinusLow:
                    msg = IndicadorDAO.ValidaHighMinusLow(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.MoneyFlowIndex:
                    msg = IndicadorDAO.ValidaMoneyFlowIndex(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.RelativeStrengthIndex:
                    msg = IndicadorDAO.ValidaRelativeStrengthIndex(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.ComparativeRelativeStrength:
                    msg = IndicadorDAO.ValidaComparativeRelativeStrength(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.PerformanceIndex:
                    msg = IndicadorDAO.ValidaPerformanceIndex(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.NegativeVolumeIndex:
                    msg = IndicadorDAO.ValidaNegativeVolumeIndex(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.PositiveVolumeIndex:
                    msg = IndicadorDAO.ValidaPositiveVolumeIndex(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.StochasticMomentumIndex:
                    msg = IndicadorDAO.ValidaStochasticMomentumIndex(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.SwingIndex:
                    msg = IndicadorDAO.ValidaSwingIndex(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.TradeVolumeIndex:
                    msg = IndicadorDAO.ValidaTradeVolumeIndex(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.KeltnerChannel:
                    msg = IndicadorDAO.ValidaKeltnerChannel(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.MACD:
                    msg = IndicadorDAO.ValidaMACD(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.MACDHistogram:
                    msg = IndicadorDAO.ValidaMACDHistogram(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.MassIndex:
                    msg = IndicadorDAO.ValidaMassIndex(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.ExponentialMovingAverage:
                    msg = IndicadorDAO.ValidaExponentialMovingAverage(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.WeightedMovingAverage:
                    msg = IndicadorDAO.ValidaWeightedMovingAverage(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.SimpleMovingAverage:
                    msg = IndicadorDAO.ValidaSimpleMovingAverage(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.TimeSeriesMovingAverage:
                    msg = IndicadorDAO.ValidaTimeSeriesMovingAverage(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.TriangularMovingAverage:
                    msg = IndicadorDAO.ValidaTriangularMovingAverage(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.VariableMovingAverage:
                    msg = IndicadorDAO.ValidaVariableMovingAverage(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.VIDYA:
                    msg = IndicadorDAO.ValidaVIDYA(listaPropriedades,numeroBarras);
                    break;
                case IndicatorType.DirectionalMovementSystem:
                    msg = IndicadorDAO.ValidaDirectionalMovementSystem(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.OnBalanceVolume:
                    msg = IndicadorDAO.ValidaOnBalanceVolume(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.AroonOscillator:
                    msg = IndicadorDAO.ValidaAroonOscillator(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.ChandeMomentumOscillator:
                    msg = IndicadorDAO.ValidaChandeMomentumOscillator(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.FractalChaosOscillator:
                    msg = IndicadorDAO.ValidaFractalChaosOscillator(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.PriceOscillator:
                    msg = IndicadorDAO.ValidaPriceOscillator(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.VolumeOscillator:
                    msg = IndicadorDAO.ValidaVolumeOscillator(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.DetrendedPriceOscillator:
                    msg = IndicadorDAO.ValidaDetrendedPriceOscillator(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.StochasticOscillator:
                    msg = IndicadorDAO.ValidaStochasticOscillator(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.MomentumOscillator:
                    msg = IndicadorDAO.ValidaMomentumOscillator(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.PrimeNumberOscillator:
                    msg = IndicadorDAO.ValidaPrimeNumberOscillator(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.RainbowOscillator:
                    msg = IndicadorDAO.ValidaRainbowOscillator(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.UltimateOscillator:
                    msg = IndicadorDAO.ValidaUltimateOscillator(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.PriceROC:
                    msg = IndicadorDAO.ValidaPriceROC(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.Median:
                    msg = IndicadorDAO.ValidaMedian(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.LinearRegressionForecast:
                    msg = IndicadorDAO.ValidaLinearRegressionForecast(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.LinearRegressionIntercept:
                    msg = IndicadorDAO.ValidaLinearRegressionIntercept(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.LinearRegressionRSquared:
                    msg = IndicadorDAO.ValidaLinearRegressionRSquared(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.LinearRegressionSlope:
                    msg = IndicadorDAO.ValidaLinearRegressionSlope(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.ParabolicSAR:
                    msg = IndicadorDAO.ValidaParabolicSAR(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.PriceVolumeTrend:
                    msg = IndicadorDAO.ValidaPriceVolumeTrend(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.TRIX:
                    msg = IndicadorDAO.ValidaTRIX(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.TrueRange:
                    msg = IndicadorDAO.ValidaTrueRange(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.TypicalPrice:
                    msg = IndicadorDAO.ValidaTypicalPrice(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.HistoricalVolatility:
                    msg = IndicadorDAO.ValidaHistoricalVolatility(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.VolumeROC:
                    msg = IndicadorDAO.ValidaVolumeROC(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.WellesWilderSmoothing:
                    msg = IndicadorDAO.ValidaWellesWilderSmoothing(listaPropriedades, numeroBarras);
                    break;
                case IndicatorType.WilliamsPctR:
                    msg = IndicadorDAO.ValidaWilliamsPctR(listaPropriedades, numeroBarras);
                    break;

                default:
                    return true;
            }

            if (msg.Length > 0)
            {
                MessageBox.Show(msg);
                return false;
            }
            else
                return true;
        }

        #endregion
    }

}

