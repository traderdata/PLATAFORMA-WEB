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
    public partial class EscolhaNovaJanelaxJanelaAtual : ChildWindow
    {
        public string TipoJanela { get; set; }

        public EscolhaNovaJanelaxJanelaAtual()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            TipoJanela = "N";
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            TipoJanela = "JCMI";
        }

        private void radioButton3_Checked(object sender, RoutedEventArgs e)
        {
            TipoJanela = "JC";
        }
    }
}

