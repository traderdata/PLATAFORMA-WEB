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

namespace Traderdata.Client.TerminalWEB.Dialogs
{
    public partial class ConfiguracaoGeral : ChildWindow
    {

        #region Variables

        /// <summary>
        /// Variavel de acesso aos webservices
        /// </summary>
        private TerminalWebSVC.TerminalWebClient terminalWebClient =
            new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.ClientDataEndpoint());

        #endregion



        public ConfiguracaoGeral(List<TerminalWebSVC.TemplateDTO> listTemplates)
        {
            InitializeComponent();

            //eventos
            terminalWebClient.DeleteTemplateCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(terminalWebClient_DeleteTemplateCompleted);
            terminalWebClient.GetTemplatesByUserCompleted += new EventHandler<TerminalWebSVC.GetTemplatesByUserCompletedEventArgs>(terminalWebClient_GetTemplatesByUserCompleted);
            gridTemplate.ItemsSource = listTemplates;
        }

        void terminalWebClient_GetTemplatesByUserCompleted(object sender, TerminalWebSVC.GetTemplatesByUserCompletedEventArgs e)
        {
            gridTemplate.ItemsSource = null;
            gridTemplate.ItemsSource = e.Result;
        }

        void terminalWebClient_DeleteTemplateCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            terminalWebClient.GetTemplatesByUserAsync(StaticData.User.Id);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnRemoverTemplates_Click(object sender, RoutedEventArgs e)
        {
            terminalWebClient.DeleteTemplateAsync((TerminalWebSVC.TemplateDTO)gridTemplate.SelectedItem);
        }

       
    }
}

