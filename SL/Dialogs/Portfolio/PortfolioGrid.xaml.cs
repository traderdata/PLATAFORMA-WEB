using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Traderdata.Client.TerminalWEB.DAO;
using Traderdata.Client.TerminalWEB.DTO;


namespace Traderdata.Client.TerminalWEB.Dialogs.Portfolio
{
    public partial class PortfolioGrid : UserControl 
    {
        #region Variaveis

        /// <summary>
        /// Variavel que armazena os dados do portfolio
        /// </summary>
        private ObservableCollection<PortfolioDTO> _portfolioDataList = new ObservableCollection<PortfolioDTO>();
        
        /// <summary>
        /// Variavel de acesso aos webservices
        /// </summary>
        private TerminalWebSVC.TerminalWebClient terminalWebClient =
            new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());


        #endregion

        #region Contrutor

        public PortfolioGrid()
        {
            InitializeComponent();

            //assinando eventos
            RealTimeDAO.TickReceived += RealTimeDAO_TickReceived;

            //asisnando eventos de portfolio
            terminalWebClient.RetornaPortfoliosCompleted += terminalWebClient_RetornaPortfoliosCompleted;
            
            //alterando itens na grid
            _flexFinancial.RowHeaders.Columns.Clear();
            //_flexFinancial.CellFactory = new PortfolioCellFactory();
            _flexFinancial.Columns[0].FontWeight = FontWeights.Bold;

        }

        

        #endregion

        #region Eventos

        /// <summary>
        /// Evento de recebimento de tick
        /// </summary>
        /// <param name="Result"></param>
        void RealTimeDAO_TickReceived(object Result)
        {
            if (Result.GetType().ToString().Contains("Tick"))
            {
                if (StaticData.FerramentasAuxiliaresVisiveis)
                {
                    foreach (PortfolioDTO obj in _portfolioDataList)
                    {
                        if (obj.Ativo == ((TickDTO)Result).Ativo)
                        {
                            obj.Abertura = ((TickDTO)Result).Abertura;
                            obj.Ativo = ((TickDTO)Result).Ativo;
                            obj.Hora = ((TickDTO)Result).Hora.Substring(0, 2) + ":" + ((TickDTO)Result).Hora.Substring(2, 2);
                            obj.Maximo = ((TickDTO)Result).Maximo;
                            obj.Minimo = ((TickDTO)Result).Minimo;
                            obj.Variacao = ((TickDTO)Result).Variacao;
                            obj.Ultimo = ((TickDTO)Result).Ultimo;
                            obj.Volume = ((TickDTO)Result).Volume;
                            break;
                        }
                    }

                    _flexFinancial.ItemsSource = _portfolioDataList;
                }
            }
        }

        /// <summary>
        /// Evento disparado ao se terminar  de carregar a tela
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LayoutRoot_Loaded_1(object sender, RoutedEventArgs e)
        {           
            cmbPortfolios.DisplayMemberPath = "Nome";
            cmbPortfolios.SelectedValuePath = "Id";            
            cmbPortfolios.ItemsSource = StaticData.Portfolios;
            cmbPortfolios.SelectionChanged += cmbPortfolios_SelectionChanged;
            cmbPortfolios.SelectedIndex = 0;            
        }

        /// <summary>
        /// Evento que roda na troca da combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbPortfolios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _portfolioDataList.Clear();
            foreach (string obj in ((TerminalWebSVC.PortfolioDTO)e.AddedItems[0]).Ativos.Split(';'))
            {
                PortfolioDTO portfolioData = new PortfolioDTO();
                portfolioData.Ativo = obj;
                _portfolioDataList.Add(portfolioData);

                _flexFinancial.ItemsSource = _portfolioDataList;
                //iniciando assinatura
                RealTimeDAO.StartTickSubscription(obj);
            }

            _flexFinancial.Columns.Frozen = 1;
            _flexFinancial.Columns[0].AllowDragging = false;
        }

        /// <summary>
        /// Evento disparado apos receber os portfolios do servidor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_RetornaPortfoliosCompleted(object sender, TerminalWebSVC.RetornaPortfoliosCompletedEventArgs e)
        {
            ComboBox cmbPortfolios = new ComboBox();
            cmbPortfolios.DisplayMemberPath = "Nome";
            cmbPortfolios.SelectedValuePath = "Id";

            foreach (TerminalWebSVC.PortfolioDTO obj in StaticData.Portfolios)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = obj;
            }

            cmbPortfolios.SelectedItem = 0;
            ((C1.Silverlight.C1Window)this.Parent).Header = cmbPortfolios;
        }

        #endregion

        #region Metodos
        

        /// <summary>
        /// Metodo que popula o grid com os ativos passados
        /// </summary>
        /// <param name="ativos"></param>
        public void PopulateFinancialGrid(List<AtivoDTO> ativos)
        {
            _portfolioDataList.Clear();
            foreach (AtivoDTO obj in ativos)
            {
                PortfolioDTO portfolioData = new PortfolioDTO();
                portfolioData.Ativo = obj.Codigo;
                _portfolioDataList.Add(portfolioData);

                _flexFinancial.ItemsSource = _portfolioDataList;

                //iniciando assinatura
                RealTimeDAO.StartTickSubscription(obj.Codigo);
            }

            _flexFinancial.Columns.Frozen = 1;
            _flexFinancial.Columns[0].AllowDragging = false;
        }

        #endregion


        private void _flexFinancial_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            //((MainPage)((BusyIndicator)((Grid)((Grid)((Grid)((ContainerDireita)((Grid)((C1.Silverlight.C1TabControl)((C1.Silverlight.C1TabItem)this.Parent).Parent).Parent).Parent)
            //    .Parent).Parent).Parent).Parent).Parent).NovoGraficoAtalho(((PortfolioDTO)_flexFinancial.SelectedItem).Ativo);            

            ((MainPage)((BusyIndicator)((Grid)((Grid)((Grid)((ContainerDireita)((Grid)((C1.Silverlight.C1TabControl)((C1.Silverlight.C1TabItem)this.Parent).Parent).Parent).Parent)
                .Parent).Parent).Parent).Parent).Parent).NovoGraficoAtalho(((PortfolioDTO)_flexFinancial.SelectedItem).Ativo);            
        }

        
    }

}
