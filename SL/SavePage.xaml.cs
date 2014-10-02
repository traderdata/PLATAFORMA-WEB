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
using System.Windows.Navigation;
using System.IO.IsolatedStorage;

namespace Traderdata.Client.TerminalWEB
{
    public partial class SavePage : Page
    {
        /// <summary>
        /// Variavel de acesso aos webservices
        /// </summary>
        private TerminalWebSVC.TerminalWebClient terminalWebClient =
            new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());

        public SavePage()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //recuperando de isolated storage
            IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;
            TerminalWebSVC.WorkspaceDTO workspace = (TerminalWebSVC.WorkspaceDTO)userSettings["workspace"];
            terminalWebClient.SaveWorkspaceCompleted += terminalWebClient_SaveWorkspaceCompleted;
            terminalWebClient.SaveWorkspaceAsync(workspace);
        }

        void terminalWebClient_SaveWorkspaceCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            txtMensagem.Text = "Workspace salvo com sucesso.";
            StaticData.WorkspaceSalvo = true;
        }

    }
}
