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
using System.Windows.Media.Imaging;
using C1.Silverlight;
using Traderdata.Client.TerminalWEB.DTO;

namespace Traderdata.Client.TerminalWEB
{
    public partial class HistoricoCotacao : UserControl
    {
        #region Variaveis Privadas

        private string ativo = "";

        private bool intraday = false;

        private MarketDataDAO marketdataDAO = new MarketDataDAO();

        #endregion

        #region Construtor

        /// <summary>
        /// Contrutor padrao
        /// </summary>
        /// <param name="ativo"></param>
        /// <param name="intraday"></param>
        public HistoricoCotacao(string ativo, bool intraday)
        {
            InitializeComponent();

            this.ativo = ativo;
            this.intraday = intraday;

            //assinando os eventos de Realtime Data
            RealTimeDAO.TickReceived += new RealTimeDAO.TickHandler(RealTimeDAO_TickReceived);
            RealTimeDAO.StartTickSubscription(ativo);    

            //assinando eventos
            marketdataDAO.GetCotacaoDiariaCompleted += new MarketDataDAO.CotacaoDiarioHandler(marketdataDAO_GetCotacaoDiariaCompleted);
            marketdataDAO.GetCotacaoIntradayCompleted += new MarketDataDAO.CotacaoIntradayHandler(marketdataDAO_GetCotacaoIntradayCompleted);
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento disparado ao se receber um tick do ativo selecionado
        /// </summary>
        /// <param name="Result"></param>
        void RealTimeDAO_TickReceived(object Result)
        {
            if (((TickDTO)Result).Ativo == this.ativo)
            {
                if (((TickDTO)Result).Variacao > 0)
                    SetHeaderTitle(((TickDTO)Result).Ativo + " [" + ((TickDTO)Result).Ultimo.ToString("0.00") + "/" +
                        ((TickDTO)Result).Variacao.ToString("0.00") + "%]", new Uri("/TerminalWeb;component/images/buy.png", UriKind.RelativeOrAbsolute));
                else
                    SetHeaderTitle(((TickDTO)Result).Ativo + " [" + ((TickDTO)Result).Ultimo.ToString("0.00") + "/" +
                        ((TickDTO)Result).Variacao.ToString("0.00") + "%]", new Uri("/TerminalWeb;component/images/sell.png", UriKind.RelativeOrAbsolute));
            }
        }

        /// <summary>
        /// Evento de resposta de cotacao intraday
        /// </summary>
        /// <param name="Result"></param>
        void marketdataDAO_GetCotacaoIntradayCompleted(List<DTO.CotacaoDTO> Result)
        {
            Result.Reverse();
            _flexGridCotacao.ItemsSource = Result;
        }

        /// <summary>
        /// Evento de resposta da cotacao diaria
        /// </summary>
        /// <param name="Result"></param>
        void marketdataDAO_GetCotacaoDiariaCompleted(List<DTO.CotacaoDTO> Result)
        {
            Result.Reverse();
            _flexGridCotacao.ItemsSource = Result;            
        }

        /// <summary>
        /// metodo que carrega na abertura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            if (!intraday)
                marketdataDAO.GetCotacaoDiariaAsync(ativo);
            else
                marketdataDAO.GetCotacaoIntradayAsync(ativo, true, true);
        }

        #endregion

        #region Metodos


        /// <summary>
        /// Metodo que altera o header do form
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sourceImagem"></param>
        private void SetHeaderTitle(string message, Uri sourceImagem)
        {
            StackPanel stackHeader = new StackPanel();
            stackHeader.Orientation = Orientation.Horizontal;
            stackHeader.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            stackHeader.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            if (sourceImagem != null)
            {
                Image imagem = new Image();
                imagem.Source = new BitmapImage(sourceImagem);
                imagem.Margin = new Thickness(3, 0, 10, 0);
                stackHeader.Children.Add(imagem);
            }

            TextBlock textTitle = new TextBlock();
            textTitle.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            textTitle.Text = message;
            textTitle.Margin = new Thickness(0, 0, 10, 0);
            stackHeader.Children.Add(textTitle);


            //alterando o header
            ((C1Window)this.Parent).Header = stackHeader;
        }

        #endregion
    }
}
