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
    public partial class Venda : UserControl
    {
        #region Variaveis Privadas

        /// <summary>
        /// Variavel que controla o ativo sendo negociado
        /// </summary>
        public string Ativo = "";

        #endregion

        #region Construtor

        public Venda()
        {
            InitializeComponent();

            //assinando eventos
            RealTimeDAO.TickReceived += new RealTimeDAO.TickHandler(RealTimeDAO_TickReceived);            
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento disparado ao receber um tick
        /// </summary>
        /// <param name="Result"></param>
        void RealTimeDAO_TickReceived(object Result)
        {
            //if (((TickDTO)Result).Ativo.ToUpper() == txtAtivo.Text.ToUpper())
            //{
            //    string sinal = "";
            //    if (((TickDTO)Result).Variacao >= 0)
            //    {
            //        lblPrecoCorrente.Foreground = new SolidColorBrush(Colors.Black);
            //        lblPercentual.Foreground = new SolidColorBrush(Colors.Black);
            //        if (((TickDTO)Result).Variacao > 0)
            //            sinal = "+";
            //    }
            //    else
            //    {
            //        lblPrecoCorrente.Foreground = new SolidColorBrush(Colors.Black);
            //        lblPercentual.Foreground = new SolidColorBrush(Colors.Black);
            //    }

            //    lblPrecoCorrente.Text = ((TickDTO)Result).Ultimo.ToString("n2");
            //    lblPercentual.Text = sinal + ((TickDTO)Result).Variacao.ToString("n2") + "%";
            //}
        }

        /// <summary>
        /// Evento disparado ao sair do campo de ativo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAtivo_LostFocus(object sender, RoutedEventArgs e)
        {
            RealTimeDAO.StartTickSubscription(txtAtivo.Text.ToUpper());
        }

        /// <summary>
        /// Evento usado para identificar o press de enter no campo de ativo e para transformar em upperCase
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAtivo_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = MakeUpperCase((TextBox)sender, e);
        }

        /// <summary>
        /// evento disparado ao se alterar o campo de ativo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAtivo_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblPercentual.Text = "-";
            lblPrecoCorrente.Text = "-";
        }
        #endregion

        #region Auxiliares

        /// <summary>
        /// Metodo que transforma de lowercase para upper case
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        bool MakeUpperCase(TextBox txt, KeyEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.None || (e.Key < Key.A) || (e.Key > Key.Z))  //do not handle ModifierKeys (work for shift key)
            {
                return false;
            }
            else
            {
                string n = new string(new char[] { (char)e.PlatformKeyCode });
                int nSelStart = txt.SelectionStart;

                txt.Text = txt.Text.Remove(nSelStart, txt.SelectionLength); //remove character from the start to end selection
                txt.Text = txt.Text.Insert(nSelStart, n); //insert value n
                txt.Select(nSelStart + 1, 0); //for cursortext

                return true; //stop to write in txt2
            }

        }

        #endregion
    }
}
