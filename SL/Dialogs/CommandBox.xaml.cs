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
    public partial class CommandBox : UserControl
    {
        #region Variaveis

        /// <summary>
        /// Variavel que armazena o comando a ser executado
        /// </summary>
        public string Comando { get; set; }

        /// <summary>
        /// Variavel que armazena o resultado da caixa de dialogo
        /// </summary>
        public bool DialogResult { get; set; }

        #endregion

        public CommandBox(Key e)
        {
            InitializeComponent();
            txtNovoComentario.Text = e.ToString();
            txtNovoComentario.Focus();
            txtNovoComentario.SelectionStart = txtNovoComentario.Text.Length;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Comando = txtNovoComentario.Text;
            DialogResult = true;
            ((C1.Silverlight.C1Window)this.Parent).Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            ((C1.Silverlight.C1Window)this.Parent).Close();
        }

        private void txtNovoComentario_KeyDown_1(object sender, KeyEventArgs e)
        {
            e.Handled = MakeUpperCase((TextBox)sender, e);
            if (e.Key == Key.Enter)
            {
                Comando = txtNovoComentario.Text;
                DialogResult = true;
                ((C1.Silverlight.C1Window)this.Parent).Close();
            }
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                ((C1.Silverlight.C1Window)this.Parent).Close();
            }
        }

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
    }
}
