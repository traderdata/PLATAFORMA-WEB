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
using ModulusFE.SL;

namespace Traderdata.Client.TerminalWEB.Dialogs
{
    public partial class StrokeThicknessPicker : UserControl
    {
        #region Eventos

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de cotação.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void ChangeSelectionHandler(object sender, EventArgs e);

        /// <summary>Evento disparado quando a ação de GetCotacaoDiaria é executada.</summary>
        public event ChangeSelectionHandler ChangeSelection;

        #endregion


        public StrokeThicknessPicker()
        {
            InitializeComponent();
        }

       

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = Brushes.LightBlue;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Colors.White);
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Colors.White);
            if (ChangeSelection != null)
                ChangeSelection(sender, e);
        }
    }
}
