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
            HtmlPage.RegisterScriptableObject("ShutdownManager", shutdownManager);

            StaticData.UrlWebservice = "https://webservice.traderdata.com.br/tw20/service.svc";

            StaticData.BVSPRTTickHost = "https://webfeeder-a.traderdata.com.br/tw20.tick.bmfbovespa.rt/request.ashx";
            StaticData.BVSPDelayHost = "https://webfeeder-a.traderdata.com.br/tw20.tick.bmfbovespa.delay/request.ashx";
            StaticData.BVSPRTBookHost = "https://easytrader-bookserver.traderdata.com.br/book.bvsp.realtime/request.ashx";
            StaticData.BVSPRTTradeHost = "https://easytrader-tradeserver.traderdata.com.br/trade.bvsp.realtime/request.ashx";

            StaticData.BMFRTTickHost = "https://webfeeder-a.traderdata.com.br/tw20.tick.bmfbovespa.rt/request.ashx";
            StaticData.BMFDelayTickHost = "https://webfeeder-a.traderdata.com.br/tw20.tick.bmfbovespa.delay/request.ashx";
            StaticData.BMFRTBookHost = "https://easytrader-bookserver.traderdata.com.br/book.bmf.realtime/request.ashx";
            StaticData.BMFRTTradeHost = "https://easytrader-tradeserver.traderdata.com.br/trade.bmf.realtime/request.ashx";

            StaticData.URLScannerIntraday = "https://twrt.traderdata.inf.br/sc-intraday/request.ashx";
            StaticData.URLChatServer = "http://easytrader.traderdata.com.br/chat/request.ashx";

            //parametros globais
            StaticData.TempoDemo = 0;
            StaticData.CacheHabilitado = false;
            StaticData.User = new TerminalWebSVC.UsuarioDTO();
            StaticData.DistribuidorId = 1;

            //Comentar antes de subir
            //e.InitParams.Add(new KeyValuePair<string,string>("distribuidor","ATG"));

            #region Segurança Single-Signon
            if (e.InitParams.ContainsKey("login-integrado"))
            {
                if (e.InitParams["login-integrado"] != null)
                {
                    //checando pela segurança para single-signon
                    SHA256Managed hashSHA = new SHA256Managed();
                    hashSHA.ComputeHash(ConvertStringToByteArray(e.InitParams["login-integrado"] + "-" + DateTime.Today.ToString("dd-MM-yyyy")));

                    string stringHash = "";

                    foreach (byte b in hashSHA.Hash)
                    {
                        stringHash += Convert.ToString(Convert.ToInt16(b)) + "-";
                    }

                    stringHash = stringHash.Remove(stringHash.Length - 1);

                    if (e.InitParams.ContainsKey("ambiente"))
                    {
                        if (e.InitParams["ambiente"] == "PRD")
                        {
                            if (stringHash != e.InitParams["token-integrado"])
                            {
                                MessageBox.Show("Token Inválido");
                                return;
                            }
                        }
                        //else
                        //    MessageBox.Show(e.InitParams["ambiente"]);
                    }
                    else
                    {
                        MessageBox.Show("nao encontrou ambiente");
                    }
                }
            }
            #endregion

            if (!e.InitParams.ContainsKey("save"))
            {
                if (e.InitParams.ContainsKey("distribuidor"))
                {

                    if (e.InitParams["distribuidor"] == "ATG")
                    {
                        StaticData.WaterMark = "ATG";
                        StaticData.PluginChat = false;
                        StaticData.PluginRastreadorEOD = false;
                        StaticData.PluginRastreadorRT = false;
                        StaticData.PluginVideoAula = false;
                        StaticData.PluginPortfolio = false;
                        StaticData.ContainerPlugins = false;
                        StaticData.Rastreador = false;
                        StaticData.TimesTrades = false;
                        StaticData.DelayedVersion = false;
                        StaticData.SingleSignOn = true;
                        StaticData.Distribuidor = "ATG";
                        StaticData.DistribuidorId = 5;
                        StaticData.LoginIntegradoDistribuidor = e.InitParams["login-integrado"];
                        StaticData.SymbolSolicitadonoDistribuidor = e.InitParams["symbol"];
                        StaticData.Backtest = false;

                    }
                    else if (e.InitParams["distribuidor"] == "WALPIRES")
                    {
                        StaticData.WaterMark = "WALPIRES";
                        StaticData.PluginChat = false;
                        StaticData.PluginRastreadorEOD = false;
                        StaticData.PluginRastreadorRT = false;
                        StaticData.PluginVideoAula = false;
                        StaticData.PluginPortfolio = false;
                        StaticData.ContainerPlugins = false;
                        StaticData.Rastreador = false;
                        StaticData.TimesTrades = false;
                        StaticData.DelayedVersion = false;
                        StaticData.SingleSignOn = true;
                        StaticData.Distribuidor = "WALPIRES";
                        StaticData.DistribuidorId = 8;
                        StaticData.LoginIntegradoDistribuidor = e.InitParams["login-integrado"];
                        StaticData.SymbolSolicitadonoDistribuidor = e.InitParams["symbol"];
                        StaticData.Backtest = false;

                    }
                    else if (e.InitParams["distribuidor"] == "TITULO")
                    {
                        StaticData.WaterMark = "easyInvest";
                        StaticData.PluginChat = false;
                        StaticData.PluginRastreadorEOD = false;
                        StaticData.PluginRastreadorRT = false;
                        StaticData.PluginVideoAula = false;
                        StaticData.PluginPortfolio = false;
                        StaticData.ContainerPlugins = false;
                        StaticData.Rastreador = false;
                        StaticData.TimesTrades = false;
                        StaticData.DelayedVersion = false;
                        StaticData.SingleSignOn = true;
                        StaticData.Distribuidor = "TITULO";
                        StaticData.DistribuidorId = 8;
                        StaticData.LoginIntegradoDistribuidor = e.InitParams["login-integrado"];
                        StaticData.SymbolSolicitadonoDistribuidor = e.InitParams["symbol"];
                        StaticData.Backtest = false;
                    }

                }
                else
                {
                    StaticData.WaterMark = "WALPIRES";
                    StaticData.PluginChat = false;
                    StaticData.PluginRastreadorEOD = false;
                    StaticData.PluginRastreadorRT = false;
                    StaticData.PluginVideoAula = false;
                    StaticData.PluginPortfolio = false;
                    StaticData.ContainerPlugins = false;
                    StaticData.Rastreador = false;
                    StaticData.TimesTrades = false;
                    StaticData.DelayedVersion = false;
                    StaticData.SingleSignOn = true;
                    StaticData.Distribuidor = "WALPIRES";
                    StaticData.DistribuidorId = 8;
                    StaticData.LoginIntegradoDistribuidor = "FELIPE3";
                    StaticData.SymbolSolicitadonoDistribuidor = "";
                    StaticData.Backtest = false;
                }
                
                if (e.InitParams.ContainsKey("refid"))
                {
                    if (e.InitParams["refid"] != "")
                    {
                        StaticData.RefId = Convert.ToInt32(e.InitParams["refid"]);
                    }
                    else

                        StaticData.RefId = 0;
                }
                                

                this.RootVisual = new ChartOnlyMainPage();
                
            }
            else
            {                
                this.RootVisual = new SavePage();
            }
        }

        private void Application_Exit(object sender, EventArgs e)
        {
            if (!StaticData.WorkspaceSalvo)
            {
                terminalWebClient = new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());

                TerminalWebSVC.WorkspaceDTO workspace = new TerminalWebSVC.WorkspaceDTO();
                workspace.Nome = "DEFAULT";
                workspace.UsuarioId = StaticData.User.Id;
                workspace.Graficos = ((MainPage)this.RootVisual).GetGraficos();

                //salvando workspace no isolated storage
                terminalWebClient.SaveWorkspaceAsync(workspace);

                Thread.Sleep(10000);


                //chamando JS
                //HtmlPage.Window.Invoke("salvar", null);
            }

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
            string result = null;
            var page = (MainPage)Application.Current.RootVisual;
            //page.SalvarWorkspaceFromOutside();

            
                //var messageBoxResult = MessageBox.Show(
                //    "Save changes you have made?", "Save changes?", MessageBoxButton.OKCancel);
                //if (messageBoxResult == MessageBoxResult.OK)
                {
                    //var waitingWindow = new WaitingWindow();
                    //waitingWindow.Show();

                    //waitingWindow.NoticeText = "Saving work on server. Please wait...";
                    //page.SetNotice("Saving work on server. Please wait...");

                    TerminalWebSVC.TerminalWebClient terminalClient = new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());
                    terminalClient.SaveWorkspaceCompleted += ((sender, e) =>
						{
							//waitingWindow.NoticeText = "It is now safe to close this window.";
							//waitingWindow.Close();
						});
                    TerminalWebSVC.WorkspaceDTO workspace = new TerminalWebSVC.WorkspaceDTO();
                    workspace.Nome = "DEFAULT";
                    workspace.UsuarioId = StaticData.User.Id;
                    workspace.Graficos = page.GetGraficos();
                    terminalClient.SaveWorkspaceAsync(workspace);
                    result = "Salvando Workspace...";
                }
                    

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

            return result;
        }

    }
}
