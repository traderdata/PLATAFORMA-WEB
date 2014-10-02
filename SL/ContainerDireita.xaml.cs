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
using Traderdata.Client.TerminalWEB.Dialogs;
using Traderdata.Client.TerminalWEB.Dialogs.Portfolio;
//using Traderdata.Client.TerminalWEB.Dialogs.Scanner;
//using Traderdata.Client.TerminalWEB.Dialogs.Social;

namespace Traderdata.Client.TerminalWEB
{
    public partial class ContainerDireita : UserControl
    {
        public ContainerDireita()
        {
            InitializeComponent();

            //Criando componentens par poder associar aos tabitens

            if (StaticData.PluginPortfolio)
            {
                PortfolioGrid portfolioGrid = new PortfolioGrid();
                tabPortfolio.Content = portfolioGrid;
            }
            else
                tabPortfolio.Visibility = System.Windows.Visibility.Collapsed;

            if (StaticData.PluginRastreadorEOD)
            {
                //VisualizaScannerDiario scanner = new VisualizaScannerDiario();
                //tabRastreadorDiario.Content = scanner;
            }
            else
                tabRastreadorDiario.Visibility = System.Windows.Visibility.Collapsed;

            if (StaticData.PluginRastreadorRT)
            {
                //VisualizaScannerIntraday scannerIntraday = new VisualizaScannerIntraday();
                //tabRastreadorIntraday.Content = scannerIntraday;
            }
            else
                tabRastreadorIntraday.Visibility = System.Windows.Visibility.Collapsed;

            if (StaticData.PluginVideoAula)
            {
                Videos videoAula = new Videos();
                tabVideoAula.Content = videoAula;
            }
            else
                tabVideoAula.Visibility = System.Windows.Visibility.Collapsed;

            if (StaticData.PluginChat)
            {
                //Chat chat = new Chat();
                //tabChat.Content = chat;
            }
            else
                tabChat.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
