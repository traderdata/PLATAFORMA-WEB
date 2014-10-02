using System;
using System.Windows;
using System.Windows.Controls;
using System.ServiceModel;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Media;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Globalization;
using System.Windows.Browser;
using Traderdata.Client.TerminalWEB.TerminalWebSVC;

namespace Traderdata.Client.TerminalWEB.Dialogs.Scanner
{
	public partial class VisualizaScannerDiario : UserControl
    {
        #region Campos e Construtores
        
        //Variaveis do scanner
        private ScannerDTO Scanner = new ScannerDTO();

        /// <summary>
        /// Variavel de acesso aos webservices
        /// </summary>
        private TerminalWebSVC.TerminalWebClient terminalWebClient =
            new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public VisualizaScannerDiario(ScannerDTO scanner)
        {
            InitializeComponent();

            //setando as variaveis globais do aplicativo
            this.Scanner = scanner;

            //Inicializando o webservice e os eventos assincronos
            //baseService = new TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());

            //assinando os eventos
            //baseService.ReProcessaScannerCompleted += baseService_ReProcessaScannerCompleted;
            //baseService.GetResultadoScannerCompleted += new EventHandler<GetResultadoScannerCompletedEventArgs>(baseService_GetResultadoScannerCompleted);
            
            //solicitando o resultado do scanner
            busyIndicator.BusyContent = "Carregando resultados...";
            busyIndicator.IsBusy = true;            
            //baseService.GetResultadoScannerAsync(this.Scanner.Id);
        }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public VisualizaScannerDiario()
        {
            InitializeComponent();

            //Assinando eventos
            terminalWebClient.GetScannersCompleted += terminalWebClient_GetScannersCompleted;
            terminalWebClient.GetResultadoScannerCompleted += terminalWebClient_GetResultadoScannerCompleted;
            terminalWebClient.ReProcessaScannerCompleted += terminalWebClient_ReProcessaScannerCompleted;
            terminalWebClient.DeleteScannerCompleted += terminalWebClient_DeleteScannerCompleted;
            
            //solicitando a lista de scanners
            busyIndicator.BusyContent = "Carregando scanners...";
            busyIndicator.IsBusy = true;
            terminalWebClient.GetScannersAsync(StaticData.User.Id);
        }

        void terminalWebClient_DeleteScannerCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //populando os nomes dos scanners no menu
            busyIndicator.IsBusy = false;
            cmbScanners.SelectionChanged -= cmbScanners_SelectionChanged;
            terminalWebClient.GetScannersAsync(StaticData.User.Id);
        }

        void terminalWebClient_ReProcessaScannerCompleted(object sender, AsyncCompletedEventArgs e)
        {
            busyIndicator.IsBusy = false;
            busyIndicator.IsBusy = true;
            busyIndicator.BusyContent = "Carregando resultados salvos...";
            terminalWebClient.GetResultadoScannerAsync((int)cmbScanners.SelectedValue);
        }

        void terminalWebClient_GetResultadoScannerCompleted(object sender, GetResultadoScannerCompletedEventArgs e)
        {
            busyIndicator.IsBusy = false;
            _gridAtivos.ItemsSource = e.Result;  
            
        }

        void terminalWebClient_GetScannersCompleted(object sender, GetScannersCompletedEventArgs e)
        {            
            busyIndicator.IsBusy = false;
            cmbScanners.DisplayMemberPath = "Nome";
            cmbScanners.SelectedValuePath = "Id";
            cmbScanners.ItemsSource = e.Result;
            cmbScanners.SelectionChanged += cmbScanners_SelectionChanged;

            if (e.Result.Count > 0)
            {
                if (e.UserState == null)
                    cmbScanners.SelectedIndex = 0;
                else
                {
                    for (int i = 0; i < e.Result.Count; i++)
                    {
                        if (((TerminalWebSVC.ScannerDTO)e.UserState).Id == e.Result[i].Id)
                        {
                            cmbScanners.SelectedIndex = i;
                            break;
                        }
                    }
                }
                btnExcluirScanner.IsEnabled = true;
                btnRefreshScanner.IsEnabled = true;
            }
            else
            {
                btnExcluirScanner.IsEnabled = false;
                btnRefreshScanner.IsEnabled = false;
            }

        }

        void cmbScanners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            busyIndicator.IsBusy = true;
            busyIndicator.BusyContent = "Carregando resultados salvos...";
            terminalWebClient.GetResultadoScannerAsync((int)cmbScanners.SelectedValue);
        }

        

       
        #endregion Campos e Construtores

        #region Eventos

        #region Respostas de Solicitações WCF
        

        #endregion Respostas de Solicitações WCF

        #region Eventos do Aplicativo Silverlight

        
        #endregion Eventos do Aplicativo Silverlight

        /// <summary>
        /// Evento que vai abrir o gráfico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseLeftButtonDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((MainPage)((BusyIndicator)((Grid)((Grid)((Grid)this.Parent).Parent).Parent)
                .Parent).Parent).NovoGraficoAtalho(((TextBlock)sender).Text);            
        }

        #endregion Eventos

        private void btnNovoScanner_Click_1(object sender, RoutedEventArgs e)
        {
            CriaEditaScannerDiario configuraScanner = new CriaEditaScannerDiario();
            configuraScanner.Closing += (sender1, e1) =>
            {
                if ((configuraScanner.DialogResult.HasValue) && (configuraScanner.DialogResult.Value == true))
                {
                    //alterando mensagem de log
                    StaticData.AddLog("Scanner salvo com sucesso");
                    
                    //populando os nomes dos scanners no menu
                    cmbScanners.SelectionChanged -= cmbScanners_SelectionChanged;
                    terminalWebClient.GetScannersAsync(StaticData.User.Id, configuraScanner.Scanner);
                }
            };
            configuraScanner.Show();
        }

        private void btnExcluirScanner_Click_1(object sender, RoutedEventArgs e)
        {
            terminalWebClient.DeleteScannerAsync((TerminalWebSVC.ScannerDTO)cmbScanners.SelectedItem);
        }

        private void btnRefreshScanner_Click_1(object sender, RoutedEventArgs e)
        {
            busyIndicator.BusyContent = "Reprocessando Rastreador...";
            busyIndicator.IsBusy = true;
            terminalWebClient.ReProcessaScannerAsync((TerminalWebSVC.ScannerDTO)cmbScanners.SelectedItem);
        }

        /// <summary>
        /// Evento de double click sobre a grid de resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _gridAtivos_DoubleClick_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((MainPage)((BusyIndicator)((Grid)((Grid)((Grid)((ContainerDireita)((Grid)((C1.Silverlight.C1TabControl)((C1.Silverlight.C1TabItem)this.Parent).Parent).Parent).Parent)
                .Parent).Parent).Parent).Parent).Parent).NovoGraficoAtalho(((TerminalWebSVC.ResultadoScannerDTO)_gridAtivos.SelectedItem).Ativo);            
        }

    }

    
}