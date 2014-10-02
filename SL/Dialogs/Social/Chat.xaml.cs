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
using Traderdata.Client.TerminalWEB.DAO;
using Traderdata.Client.TerminalWEB.DTO;

namespace Traderdata.Client.TerminalWEB.Dialogs.Social
{
    public partial class Chat : UserControl
    {
        #region variaveis privadas

        /// <summary>
        /// Variavel de acesso a camada WCF
        /// </summary>
        private TerminalWebSVC.TerminalWebClient terminalWebClient;

        #endregion

        #region Construtor

        public Chat()
        {
            InitializeComponent();

            //inciaindo comunicaçção de chat em tempo real
            RealTimeDAO.ChatMessageReceived += new RealTimeDAO.ChatHandler(RealTimeDAO_ChatMessageReceived);
            RealTimeDAO.ConnectChat();

            //inciiando a camda servidora
            terminalWebClient = new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());
            
        }

        

        #endregion

        void RealTimeDAO_ChatMessageReceived(object Result)
        {
            MsgDTO msg = (MsgDTO)Result;
            TextBlock txtHeader = new TextBlock();
            txtHeader.Text = msg.Autor + " disse em " + msg.DateTime.ToString("dd-MM-yyyy HH:mm");
            txtHeader.Foreground = new SolidColorBrush(Colors.Purple);
            txtHeader.FontStyle = FontStyles.Italic;            
            panelTextos.Children.Add(txtHeader);

            TextBlock txtMessage = new TextBlock();
            txtMessage.Text = msg.Message;
            txtMessage.TextWrapping = TextWrapping.Wrap;
            txtMessage.Margin = new Thickness(10, 10, 10, 10);
            panelTextos.Children.Add(txtMessage);

            scrollPrincipal.ScrollToBottom();
        }

        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void SendMessage()
        {
            MsgDTO msg = new MsgDTO();
            msg.Autor = StaticData.User.Login.Split('@')[0];
            msg.DateTime = DateTime.Now;
            msg.Message = txtMsg.Text;

            RealTimeDAO.PublishMessageToChat(msg);
            TerminalWebSVC.MensagemDTO msgServer = new TerminalWebSVC.MensagemDTO();
            msgServer.Mensagem = txtMsg.Text;
            msgServer.UsuarioId = StaticData.User.Id;

            terminalWebClient.InsereMensagemAsync(msgServer);

            txtMsg.Text = "";
        }

        private void txtMsg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage();
        }
    }
}
