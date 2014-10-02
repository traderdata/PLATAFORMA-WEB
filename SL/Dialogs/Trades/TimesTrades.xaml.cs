using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Traderdata.Client.TerminalWEB.Dialogs.Trades
{
    public partial class TimesTrades : UserControl
    {
        ObservableCollection<TradeDTO> Trades = new ObservableCollection<TradeDTO>();
        string Ativo = "";

        public TimesTrades(string ativo)
        {
            InitializeComponent();

            //setando o ativo local
            this.Ativo = ativo;
                       
            
            //associando ao evento de atualização de book
            RealTimeDAO.TradeReceived += RealTimeDAO_TradeReceived;
            RealTimeDAO.TickReceived += RealTimeDAO_TickReceived;
            RealTimeDAO.StartTradeSubscription(ativo);
            gridTrade.ItemsSource = Trades;
            
        }

        void RealTimeDAO_TickReceived(object Result)
        {
            if (((TickDTO)Result).Ativo == this.Ativo)
            {
                ((TextBlock)((StackPanel)((C1.Silverlight.C1Window)Parent).Header).Children[0]).Text = "Trades | V: " + Math.Round(((TickDTO)Result).Volume / 1000000, 0) + " | N: " + ((TickDTO)Result).NumeroNegocio + " | Q: " + ((TickDTO)Result).Quantidade;
            }
        }

        void RealTimeDAO_TradeReceived(object Result)
        {
            if (((TradeDTO)Result).Ativo == this.Ativo)
                Trades.Insert(0, (TradeDTO)Result);
        }

    }
}
