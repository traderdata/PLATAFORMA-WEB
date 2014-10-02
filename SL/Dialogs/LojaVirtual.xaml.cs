using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Traderdata.Client.TerminalWEB.Dialogs
{
    public partial class LojaVirtual : ChildWindow
    {
        #region Variáveis

        /// <summary>
        /// Variavel de acesso aos webservices
        /// </summary>
        private TerminalWebSVC.TerminalWebClient terminalWebClient =
            new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());

        /// <summary>
        /// Valor a ser pago no segmento BMF
        /// </summary>
        double valBmf = 0;

        /// <summary>
        /// Valor a ser pago pelo segmento Bovespa
        /// </summary>
        double valBovespa = 0;

        /// <summary>
        /// String para pagamento
        /// </summary>
        string url = "";

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public LojaVirtual()
        {
            InitializeComponent();
            terminalWebClient.RetornaLinkPagamentoBMFDELAYCompleted += terminalWebClient_RetornaLinkPagamentoBMFDELAYCompleted;
            terminalWebClient.RetornaLinkPagamentoBMFEODCompleted += terminalWebClient_RetornaLinkPagamentoBMFEODCompleted;
            terminalWebClient.RetornaLinkPagamentoBMFRTCompleted += terminalWebClient_RetornaLinkPagamentoBMFRTCompleted;
            terminalWebClient.RetornaLinkPagamentoBVSPDELAYBMFDELAYCompleted += terminalWebClient_RetornaLinkPagamentoBVSPDELAYBMFDELAYCompleted;
            terminalWebClient.RetornaLinkPagamentoBVSPDELAYBMFEODCompleted += terminalWebClient_RetornaLinkPagamentoBVSPDELAYBMFEODCompleted;
            terminalWebClient.RetornaLinkPagamentoBVSPDELAYBMFRTCompleted += terminalWebClient_RetornaLinkPagamentoBVSPDELAYBMFRTCompleted;
            terminalWebClient.RetornaLinkPagamentoBVSPDELAYCompleted += terminalWebClient_RetornaLinkPagamentoBVSPDELAYCompleted;
            terminalWebClient.RetornaLinkPagamentoBVSPEODBMFDELAYCompleted += terminalWebClient_RetornaLinkPagamentoBVSPEODBMFDELAYCompleted;
            terminalWebClient.RetornaLinkPagamentoBVSPEODBMFEODCompleted += terminalWebClient_RetornaLinkPagamentoBVSPEODBMFEODCompleted;
            terminalWebClient.RetornaLinkPagamentoBVSPEODBMFRTCompleted += terminalWebClient_RetornaLinkPagamentoBVSPEODBMFRTCompleted;
            terminalWebClient.RetornaLinkPagamentoBVSPEODCompleted += terminalWebClient_RetornaLinkPagamentoBVSPEODCompleted;
            terminalWebClient.RetornaLinkPagamentoBVSPRTBMFDELAYCompleted += terminalWebClient_RetornaLinkPagamentoBVSPRTBMFDELAYCompleted;
            terminalWebClient.RetornaLinkPagamentoBVSPRTBMFEODCompleted += terminalWebClient_RetornaLinkPagamentoBVSPRTBMFEODCompleted;
            terminalWebClient.RetornaLinkPagamentoBVSPRTBMFRTCompleted += terminalWebClient_RetornaLinkPagamentoBVSPRTBMFRTCompleted;
            terminalWebClient.RetornaLinkPagamentoBVSPRTCompleted += terminalWebClient_RetornaLinkPagamentoBVSPRTCompleted;
            terminalWebClient.RetornaPrecoBMFDELAYCompleted += terminalWebClient_RetornaPrecoBMFDELAYCompleted;
            terminalWebClient.RetornaPrecoBMFEODCompleted += terminalWebClient_RetornaPrecoBMFEODCompleted;
            terminalWebClient.RetornaPrecoBMFRTCompleted += terminalWebClient_RetornaPrecoBMFRTCompleted;
            terminalWebClient.RetornaPrecoBVSPDELAYCompleted += terminalWebClient_RetornaPrecoBVSPDELAYCompleted;
            terminalWebClient.RetornaPrecoBVSPEODCompleted += terminalWebClient_RetornaPrecoBVSPEODCompleted;
            terminalWebClient.RetornaPrecoBVSPRTCompleted += terminalWebClient_RetornaPrecoBVSPRTCompleted;            
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Evento carregado depois dos objetos serem criados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_Loaded_1(object sender, RoutedEventArgs e)
        {
            rdbBMFRT.IsChecked = true;
            rdbBovespaRT.IsChecked = true;
        }

        /// <summary>
        /// Evento disparado ao se clicar em Ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {            

            if (rdbBovespaRT.IsChecked.Value)
            {
                if (rdbBMFNenhum.IsChecked.Value)
                    terminalWebClient.RetornaLinkPagamentoBVSPRTAsync();
                if (rdbBMFRT.IsChecked.Value)
                    terminalWebClient.RetornaLinkPagamentoBVSPRTBMFRTAsync();
                if (rdbBMFDelay.IsChecked.Value)
                    terminalWebClient.RetornaLinkPagamentoBVSPRTBMFDELAYAsync();
                if (rdbBMFEOD.IsChecked.Value)
                    terminalWebClient.RetornaLinkPagamentoBVSPRTBMFEODAsync();
            }
            if (rdbBovespaDelay.IsChecked.Value)
            {
                if (rdbBMFNenhum.IsChecked.Value)
                    terminalWebClient.RetornaLinkPagamentoBVSPDELAYAsync();
                if (rdbBMFRT.IsChecked.Value)
                    terminalWebClient.RetornaLinkPagamentoBVSPDELAYBMFRTAsync();
                if (rdbBMFDelay.IsChecked.Value)
                    terminalWebClient.RetornaLinkPagamentoBVSPDELAYBMFDELAYAsync();
                if (rdbBMFEOD.IsChecked.Value)
                    terminalWebClient.RetornaLinkPagamentoBVSPDELAYBMFEODAsync();
            }
            if (rdbBovespaEOD.IsChecked.Value)
            {
                if (rdbBMFNenhum.IsChecked.Value)
                    terminalWebClient.RetornaLinkPagamentoBVSPEODAsync();
                if (rdbBMFRT.IsChecked.Value)
                    terminalWebClient.RetornaLinkPagamentoBVSPEODBMFRTAsync();
                if (rdbBMFDelay.IsChecked.Value)
                    terminalWebClient.RetornaLinkPagamentoBVSPEODBMFDELAYAsync();
                if (rdbBMFEOD.IsChecked.Value)
                    terminalWebClient.RetornaLinkPagamentoBVSPEODBMFEODAsync();
            }
            if (rdbBovespaNenhum.IsChecked.Value)
            {
                if (rdbBMFNenhum.IsChecked.Value)
                    MessageBox.Show("É necessário selecionar pelo menos 1 produto para prosseguir.");
                if (rdbBMFRT.IsChecked.Value)
                    terminalWebClient.RetornaLinkPagamentoBMFRTAsync();
                if (rdbBMFDelay.IsChecked.Value)
                    terminalWebClient.RetornaLinkPagamentoBMFDELAYAsync();
                if (rdbBMFEOD.IsChecked.Value)
                    terminalWebClient.RetornaLinkPagamentoBMFEODAsync();
            }

        }

        /// <summary>
        /// Envento disparado ao se clicar no botão cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Chamando o suporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            //C1.Silverlight.C1Window window = new C1.Silverlight.C1Window();
            //C1.Silverlight.Extended.C1HtmlHost htmlHost = new C1.Silverlight.Extended.C1HtmlHost();
            //htmlHost.SourceUri = new Uri("http://messenger.providesupport.com/messenger/traderdata.html");
            //window.ShowMinimizeButton = false;
            //window.Content = htmlHost;
            //window.Width = 500;
            //window.Height = 600;
            //window.Show();
            HtmlPage.PopupWindow(new Uri("http://messenger.providesupport.com/messenger/traderdata.html"), "_blank", new HtmlPopupWindowOptions());
        }

        /// <summary>
        /// Evento disparado ao clicar em Bovespa EOD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbBovespaEOD_Checked_1(object sender, RoutedEventArgs e)
        {
            AtualizaPreco();
        }

        /// <summary>
        /// Evento disparado ao clicar em Bovesdpa Delay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbBovespaDelay_Checked_1(object sender, RoutedEventArgs e)
        {
            AtualizaPreco();
        }

        /// <summary>
        /// Evento disparado ao clicar em Bovespa RT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbBovespaRT_Checked_1(object sender, RoutedEventArgs e)
        {
            AtualizaPreco();
        }

        /// <summary>
        /// Evento disparado ao clicar em Bovespa Nenhum
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbBovespaNenhum_Checked_1(object sender, RoutedEventArgs e)
        {
            valBovespa = 0;
            AtualizaPreco();
            AtualizaLabelTotal();
        }

        /// <summary>
        /// Evento disparado ao clicar em BMF Nenhum
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbBMFNenhum_Checked_1(object sender, RoutedEventArgs e)
        {
            valBmf = 0;
            AtualizaPreco();
            AtualizaLabelTotal();
        }

        /// <summary>
        /// Evento disparado ao clicar em BMF RT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbBMFRT_Checked_1(object sender, RoutedEventArgs e)
        {
            AtualizaPreco();
        }

        /// <summary>
        /// Evento disparado ao clicar em BMF Delay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbBMFDelay_Checked_1(object sender, RoutedEventArgs e)
        {
            AtualizaPreco();
        }

        /// <summary>
        /// Evento disparado ao clicar em BMF EOD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbBMFEOD_Checked_1(object sender, RoutedEventArgs e)
        {
            AtualizaPreco();
        }

        #endregion

        

        #region Eventos Async

        void terminalWebClient_RetornaPrecoBVSPRTCompleted(object sender, TerminalWebSVC.RetornaPrecoBVSPRTCompletedEventArgs e)
        {
            valBovespa = e.Result;
            AtualizaLabelTotal();
        }

        void terminalWebClient_RetornaPrecoBVSPEODCompleted(object sender, TerminalWebSVC.RetornaPrecoBVSPEODCompletedEventArgs e)
        {
            valBovespa = e.Result;
            AtualizaLabelTotal();
        }

        void terminalWebClient_RetornaPrecoBVSPDELAYCompleted(object sender, TerminalWebSVC.RetornaPrecoBVSPDELAYCompletedEventArgs e)
        {
            valBovespa = e.Result;
            AtualizaLabelTotal();
        }

        void terminalWebClient_RetornaPrecoBMFRTCompleted(object sender, TerminalWebSVC.RetornaPrecoBMFRTCompletedEventArgs e)
        {
            valBmf = e.Result;
            AtualizaLabelTotal();
        }

        void terminalWebClient_RetornaPrecoBMFEODCompleted(object sender, TerminalWebSVC.RetornaPrecoBMFEODCompletedEventArgs e)
        {
            valBmf = e.Result;
            AtualizaLabelTotal();
        }

        void terminalWebClient_RetornaPrecoBMFDELAYCompleted(object sender, TerminalWebSVC.RetornaPrecoBMFDELAYCompletedEventArgs e)
        {
            valBmf = e.Result;
            AtualizaLabelTotal();
        }

        void terminalWebClient_RetornaLinkPagamentoBVSPRTCompleted(object sender, TerminalWebSVC.RetornaLinkPagamentoBVSPRTCompletedEventArgs e)
        {
            url = e.Result.Replace(";", "&");
            ChamaBrowserExterno();
        }

        void terminalWebClient_RetornaLinkPagamentoBVSPRTBMFRTCompleted(object sender, TerminalWebSVC.RetornaLinkPagamentoBVSPRTBMFRTCompletedEventArgs e)
        {
            url = e.Result.Replace(";", "&");
            ChamaBrowserExterno();
        }

        void terminalWebClient_RetornaLinkPagamentoBVSPRTBMFEODCompleted(object sender, TerminalWebSVC.RetornaLinkPagamentoBVSPRTBMFEODCompletedEventArgs e)
        {
            url = e.Result.Replace(";", "&");
            ChamaBrowserExterno();
        }

        void terminalWebClient_RetornaLinkPagamentoBVSPRTBMFDELAYCompleted(object sender, TerminalWebSVC.RetornaLinkPagamentoBVSPRTBMFDELAYCompletedEventArgs e)
        {
            url = e.Result.Replace(";", "&");
            ChamaBrowserExterno();
        }

        void terminalWebClient_RetornaLinkPagamentoBVSPEODCompleted(object sender, TerminalWebSVC.RetornaLinkPagamentoBVSPEODCompletedEventArgs e)
        {
            url = e.Result.Replace(";", "&");
            ChamaBrowserExterno();
        }

        void terminalWebClient_RetornaLinkPagamentoBVSPEODBMFRTCompleted(object sender, TerminalWebSVC.RetornaLinkPagamentoBVSPEODBMFRTCompletedEventArgs e)
        {
            url = e.Result.Replace(";", "&");
            ChamaBrowserExterno();
        }

        void terminalWebClient_RetornaLinkPagamentoBVSPEODBMFEODCompleted(object sender, TerminalWebSVC.RetornaLinkPagamentoBVSPEODBMFEODCompletedEventArgs e)
        {
            url = e.Result.Replace(";", "&");
            ChamaBrowserExterno();
        }

        void terminalWebClient_RetornaLinkPagamentoBVSPEODBMFDELAYCompleted(object sender, TerminalWebSVC.RetornaLinkPagamentoBVSPEODBMFDELAYCompletedEventArgs e)
        {
            url = e.Result.Replace(";", "&");
            ChamaBrowserExterno();
        }

        void terminalWebClient_RetornaLinkPagamentoBVSPDELAYCompleted(object sender, TerminalWebSVC.RetornaLinkPagamentoBVSPDELAYCompletedEventArgs e)
        {
            url = e.Result.Replace(";", "&");
            ChamaBrowserExterno();
        }

        void terminalWebClient_RetornaLinkPagamentoBVSPDELAYBMFRTCompleted(object sender, TerminalWebSVC.RetornaLinkPagamentoBVSPDELAYBMFRTCompletedEventArgs e)
        {
            url = e.Result.Replace(";", "&");
            ChamaBrowserExterno();
        }

        void terminalWebClient_RetornaLinkPagamentoBVSPDELAYBMFEODCompleted(object sender, TerminalWebSVC.RetornaLinkPagamentoBVSPDELAYBMFEODCompletedEventArgs e)
        {
            url = e.Result.Replace(";", "&");
            ChamaBrowserExterno();
        }

        void terminalWebClient_RetornaLinkPagamentoBVSPDELAYBMFDELAYCompleted(object sender, TerminalWebSVC.RetornaLinkPagamentoBVSPDELAYBMFDELAYCompletedEventArgs e)
        {
            url = e.Result.Replace(";", "&");
            ChamaBrowserExterno();
        }

        void terminalWebClient_RetornaLinkPagamentoBMFRTCompleted(object sender, TerminalWebSVC.RetornaLinkPagamentoBMFRTCompletedEventArgs e)
        {
            url = e.Result.Replace(";", "&");
            ChamaBrowserExterno();
        }

        void terminalWebClient_RetornaLinkPagamentoBMFEODCompleted(object sender, TerminalWebSVC.RetornaLinkPagamentoBMFEODCompletedEventArgs e)
        {
            url = e.Result.Replace(";", "&");
            ChamaBrowserExterno();
        }

        void terminalWebClient_RetornaLinkPagamentoBMFDELAYCompleted(object sender, TerminalWebSVC.RetornaLinkPagamentoBMFDELAYCompletedEventArgs e)
        {
            url = e.Result.Replace(";", "&");
            ChamaBrowserExterno();
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Metodo responsavel por atualizar os preços
        /// </summary>
        private void AtualizaPreco()
        {            
            if (rdbBovespaRT.IsChecked.Value)
                terminalWebClient.RetornaPrecoBVSPRTAsync();
            if (rdbBovespaDelay.IsChecked.Value)
                terminalWebClient.RetornaPrecoBVSPDELAYAsync();
            if (rdbBovespaEOD.IsChecked.Value)
                terminalWebClient.RetornaPrecoBVSPEODAsync();

            if (rdbBMFRT.IsChecked.Value)
                terminalWebClient.RetornaPrecoBMFRTAsync();
            if (rdbBMFDelay.IsChecked.Value)
                terminalWebClient.RetornaPrecoBMFDELAYAsync();
            if (rdbBMFEOD.IsChecked.Value)
                terminalWebClient.RetornaPrecoBMFEODAsync();
        }

        /// <summary>
        /// Metodo que atualiza o total
        /// </summary>
        private void AtualizaLabelTotal()
        {
            lblTotal.Content = "Total R$ " + (valBovespa + valBmf).ToString();
        }

        /// <summary>
        /// Metodo que chama o browser externo
        /// </summary>
        private void ChamaBrowserExterno()
        {
            if (url.Length > 0)
            {
                //chamando JS
                HtmlPage.Window.Invoke("abrirLoja", url);
            }

            this.DialogResult = true;
        }

        #endregion

        

        

    }
}

