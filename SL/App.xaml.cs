using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Traderdata.Client.TerminalWEB.Dialogs;
using System.Security.Cryptography;
using System.Text;

namespace Traderdata.Client.TerminalWEB
{
    public partial class App : Application
    {
        ShutdownManager shutdownManager = new ShutdownManager();

        /// <summary>
        /// Variavel de acesso aos webservices
        /// </summary>
        private TerminalWebSVC.TerminalWebClient terminalWebClient; 

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            StaticData.ClientWebservice = "https://webservice.traderdata.com.br/tweb2/service.svc";
            StaticData.MDWebservice = "https://app-ext.traderdata.com.br/md-api/mdapi.svc";
            StaticData.TickServer = "https://app-ext.traderdata.com.br/tw10.tick.bmfbovespa.rt/request.ashx";
            
            //parametros globais
            StaticData.CacheHabilitado = false;
            StaticData.User = new TerminalWebSVC.UserDTO();
            
            //configurações
            if (e.InitParams.ContainsKey("broker"))
            {
                StaticData.WaterMark = e.InitParams["broker"];
                StaticData.Distribuidor = e.InitParams["broker"];
            }
            else
            {
                StaticData.WaterMark = "TRADERDATA";
                StaticData.Distribuidor = "DEMO";
            }

            //pegando o codigo do cliente
            string login = "";
            if (e.InitParams.ContainsKey("USR"))
            {
                login = StaticData.Distribuidor + "#" + e.InitParams["USR"];
            }
            else
            {
                login = StaticData.Distribuidor + "#DEMO";
            }

            StaticData.DelayedVersion = false;            
            StaticData.DistribuidorId = 8;
            
            //abrindo o main page
            this.RootVisual = new ChartOnlyMainPage(login, "PETR4");
            
        }

        private void Application_Exit(object sender, EventArgs e)
        {
            //if (!StaticData.WorkspaceSalvo)
            //{
            //    terminalWebClient = new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());

            //    TerminalWebSVC.WorkspaceDTO workspace = new TerminalWebSVC.WorkspaceDTO();
            //    workspace.Nome = "DEFAULT";
            //    workspace.UsuarioId = StaticData.User.Id;
            //    //workspace.Graficos = ((MainPage)this.RootVisual).GetGraficos();

            //    //salvando workspace no isolated storage
            //    //terminalWebClient.SaveWorkspaceAsync(workspace);

            //    //Thread.Sleep(10000);


            //    //chamando JS
            //    //HtmlPage.Window.Invoke("salvar", null);
            //}

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }

        //function to convert string to byte array 
        private byte[] ConvertStringToByteArray(string stringToConvert)
        {
            return (new UnicodeEncoding()).GetBytes(stringToConvert);
        }
    }

    [ScriptableType]
    public class ShutdownManager
    {
        [ScriptableMember]
        public string Shutdown()
        {
            //string result = null;
            //var page = (MainPage)Application.Current.RootVisual;
            ////page.SalvarWorkspaceFromOutside();

            
            //    //var messageBoxResult = MessageBox.Show(
            //    //    "Save changes you have made?", "Save changes?", MessageBoxButton.OKCancel);
            //    //if (messageBoxResult == MessageBoxResult.OK)
            //    {
            //        //var waitingWindow = new WaitingWindow();
            //        //waitingWindow.Show();

            //        //waitingWindow.NoticeText = "Saving work on server. Please wait...";
            //        //page.SetNotice("Saving work on server. Please wait...");

            //        TerminalWebSVC.TerminalWebClient terminalClient = new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());
            //        terminalClient.SaveWorkspaceCompleted += ((sender, e) =>
            //            {
            //                //waitingWindow.NoticeText = "It is now safe to close this window.";
            //                //waitingWindow.Close();
            //            });
            //        TerminalWebSVC.WorkspaceDTO workspace = new TerminalWebSVC.WorkspaceDTO();
            //        workspace.Nome = "DEFAULT";
            //        workspace.UsuarioId = StaticData.User.Id;
            //        workspace.Graficos = page.GetGraficos();
            //        terminalClient.SaveWorkspaceAsync(workspace);
            //        result = "Salvando Workspace...";
            //    }
                    

            //        var client = new ShutdownServiceClient();
            //        client.SaveWorkCompleted += ((sender, e) =>
            //        {
            //            waitingWindow.NoticeText = "It is now safe to close this window.";
            //            page.SetNotice("It is now safe to close this window.");
            //            page.Dirty = false;
            //            waitingWindow.Close();
            //        });
            //        client.SaveWorkAsync("Test id");
            //        result = "Saving changes.";
            //    }
            //    else
            //    {
            //        result = "If you close this window you will lose any unsaved work.";
            //    }
            //}

            //return result;
            return null;
        }

    }
}
