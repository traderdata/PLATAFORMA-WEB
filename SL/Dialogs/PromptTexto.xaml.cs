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
    public partial class PromptTexto : ChildWindow
    {
        public string Texto { get; set; }

        public PromptTexto(string header, string textoInicial, double fontSize, bool acceptsReturn)
        {
            InitializeComponent();
            this.Title = header;
            txtNovoComentario.Text = textoInicial;
            txtNovoComentario.FontSize = fontSize;
            txtNovoComentario.Focus();
            txtNovoComentario.AcceptsReturn = acceptsReturn;
            txtNovoComentario.SelectionStart = txtNovoComentario.Text.Length;
        }
                

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Texto = txtNovoComentario.Text;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void txtNovoComentario_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (!txtNovoComentario.AcceptsReturn)
                if (e.Key == Key.Enter)
                {
                    Texto = txtNovoComentario.Text;
                    this.DialogResult = true;
                }
                else if (e.Key == Key.Escape)
                {
                    this.DialogResult = false;
                }
        }
    }
}

