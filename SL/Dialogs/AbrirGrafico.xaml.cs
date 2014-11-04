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
using System.Windows.Browser;

namespace Traderdata.Client.TerminalWEB.Dialogs
{
    public partial class AbrirGrafico : ChildWindow
    {
        #region Variaveis

        /// <summary>
        /// Variavel de acesso aos webservices
        /// </summary>
        private TerminalWebSVC.TerminalWebClient terminalWebClient =
            new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.ClientDataEndpoint());

        #endregion

        public AbrirGrafico()
        {
            InitializeComponent();

            //carregando graficos
            terminalWebClient.GetGraficosByUserIdCompleted += new EventHandler<TerminalWebSVC.GetGraficosByUserIdCompletedEventArgs>(terminalWebClient_GetGraficosByUserIdCompleted);
            terminalWebClient.GetGraficosByUserIdAsync(StaticData.User.Id);
        }

        #region Eventos

        void terminalWebClient_GetGraficosByUserIdCompleted(object sender, TerminalWebSVC.GetGraficosByUserIdCompletedEventArgs e)
        {
            gridGraficos.ItemsSource = e.Result;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (TerminalWebSVC.GraficoDTO chart in gridGraficos.SelectedItems)
            {
                HtmlPage.Window.Navigate(new Uri("./Grafico2.aspx?ativo=" + chart.Ativo + "&token=" + StaticData.Token + "&usr=" + StaticData.User.Login, UriKind.RelativeOrAbsolute), "_new");            
            }


            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #endregion
    }
}

