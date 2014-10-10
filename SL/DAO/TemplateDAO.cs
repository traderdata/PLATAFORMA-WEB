using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Traderdata.Client.TerminalWEB.DAO
{
    public class TemplateDAO
    {
        #region Variaveis

        /// <summary>
        /// Variavel de acesso aos webservices
        /// </summary>
        private TerminalWebSVC.TerminalWebClient terminalWebClient =
            new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());

        #endregion

        #region Construtor

        /// <summary>
        /// Metodo que deve ser executado para se iniciar a classe estatica de marketdata
        /// </summary>        
        public TemplateDAO()
        {
            //assinando eventos WCF
            
        }

        #endregion

        #region Eventos WCF

               

        #endregion
    }
}
