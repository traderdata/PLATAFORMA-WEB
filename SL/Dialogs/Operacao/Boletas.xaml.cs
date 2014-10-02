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

namespace Traderdata.Client.TerminalWEB.Dialogs.Operacao
{
    public partial class Boletas : UserControl
    {
        #region Variaveis Privadas

        /// <summary>
        /// Variavel que controla o ativo sendo negociado
        /// </summary>
        public string Ativo = "";

        #endregion

        public Boletas()
        {
            InitializeComponent();            
        }

        
        private void imgMinimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Grid)this.Parent).RowDefinitions[1].Height = new GridLength(20);
            imgMinimize.Visibility = System.Windows.Visibility.Collapsed;
            imgMaximize.Visibility = System.Windows.Visibility.Visible;
        }

        private void imgMaximize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Grid)this.Parent).RowDefinitions[1].Height = new GridLength(250);
            imgMinimize.Visibility = System.Windows.Visibility.Visible;
            imgMaximize.Visibility = System.Windows.Visibility.Collapsed;
        }

    }
}
