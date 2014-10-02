using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Traderdata.Client.TerminalWEB.Dialogs
{
    public partial class RecomendarCompraVenda : ChildWindow
    {
        #region Variaveis

        /// <summary>
        /// Variavel que armanzena se é compra ou venda
        /// </summary>
        private bool IsCompra = false;

        /// <summary>
        /// Variavel que armazena o ultimo preço
        /// </summary>
        private double LastPrice = 0;

        /// <summary>
        /// Variavel que armazena o ativo
        /// </summary>
        private string Ativo = "";

        #endregion

        #region Construtor

        /// <summary>
        /// Construtor PAdrão
        /// </summary>
        /// <param name="isCompra"></param>
        /// <param name="lastPrice"></param>
        public RecomendarCompraVenda(bool isCompra, double lastPrice, string ativo)
        {
            InitializeComponent();

            this.IsCompra = isCompra;
            this.LastPrice = lastPrice;
            this.Ativo = ativo;

            //setando titulo
            if (isCompra)
                this.Title = "Recomendação de Compra";
            else
                this.Title = "Recomendação de Venda";


            //setando o label
            txtLastPrice.Text = lastPrice.ToString();
        }

        #endregion

        #region Eventos

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //fazendo a recomendação
            string recomendacao = "Recomendação de ";
            if (IsCompra)
                recomendacao += "compra";
            else
                recomendacao += "venda";
            recomendacao += " em " + this.Ativo + " com objetivo em " + txtTarget.Text.ToString() + " stop em " +
                txtStop.Text.ToString() + ". Último preço do ativo " + txtLastPrice.Text.ToString();

            List<object> listaParametros = new List<object>();
            listaParametros.Add(recomendacao);
            HtmlPage.Window.Invoke("postBuySell", listaParametros.ToArray());            

            this.DialogResult = true;
        }

        #endregion
    }
}

