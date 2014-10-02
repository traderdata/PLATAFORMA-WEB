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
using Traderdata.Client.TerminalWEB.DTO;

namespace Traderdata.Client.TerminalWEB.Dialogs
{
    public partial class Status : ChildWindow
    {
        public Status()
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

        private void LayoutRoot_Loaded_1(object sender, RoutedEventArgs e)
        {
            List<LogDTO> listaTemp = new List<LogDTO>();
            for(int i = 0; i < StaticData.listaLog.Count;i++)
            {
                listaTemp.Add(new LogDTO(StaticData.listaLog[i].DataHora, StaticData.listaLog[i].Texto));
            }
            gridLog.ItemsSource = listaTemp;

            if (StaticData.User.BovespaRT >= DateTime.Today)
                txtBovespaRT.Text = "Sinal Bovespa RT - Contratado até " + StaticData.User.BovespaRT.ToShortDateString();
            else
                txtBovespaRT.Text = "Sinal Bovespa RT - Expirado";

            if (StaticData.User.BovespaDELAY >= DateTime.Today)
                txtBovespaDelay.Text = "Sinal Bovespa Delay - Contratado até " + StaticData.User.BovespaDELAY.ToShortDateString();
            else
                txtBovespaDelay.Text = "Sinal Bovespa Delay - Expirado";

            if (StaticData.User.BovespaEOD >= DateTime.Today)
                txtBovespaEOD.Text = "Sinal Bovespa EOD - Contratado até " + StaticData.User.BovespaEOD.ToShortDateString();
            else
                txtBovespaEOD.Text = "Sinal Bovespa EOD - Expirado";

            if (StaticData.User.BMFRT >= DateTime.Today)
                txtBMFRT.Text = "Sinal BMF RT - Contratado até " + StaticData.User.BMFRT.ToShortDateString();
            else
                txtBMFRT.Text = "Sinal BMF RT - Expirado";

            if (StaticData.User.BMFDELAY >= DateTime.Today)
                txtBMFDelay.Text = "Sinal BMF Delay - Contratado até " + StaticData.User.BMFDELAY.ToShortDateString();
            else
                txtBMFDelay.Text = "Sinal BMF Delay - Expirado";

            if (StaticData.User.BMFEOD >= DateTime.Today)
                txtBMFEOD.Text = "Sinal BMF EOD - Contratado até " + StaticData.User.BMFEOD.ToShortDateString();
            else
                txtBMFEOD.Text = "Sinal BMF EOD - Expirado";

        }

        private void btnRefresh_Click_1(object sender, RoutedEventArgs e)
        {
            List<LogDTO> listaTemp = new List<LogDTO>();
            for (int i = 0; i < StaticData.listaLog.Count; i++)
            {
                listaTemp.Add(new LogDTO(StaticData.listaLog[i].DataHora, StaticData.listaLog[i].Texto));
            }
            gridLog.ItemsSource = listaTemp;
        }
    }
}

