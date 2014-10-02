using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Traderdata.Client.TerminalWEB.DTO
{
    public class BookDTO
    {
        public string Ativo { get; set; }

        public string CorretoraCompra { get; set; }
        public int QuantidadeCompra { get; set; }
        public double PrecoCompra { get; set; }

        public string CorretoraVenda { get; set; }
        public int QuantidadeVenda { get; set; }
        public double PrecoVenda { get; set; }

        public BookDTO(string ativo, string corretoraCompra, int quantidadeCompra, double precoCompra, string corretoraVenda, int quantidadeVenda, double precoVenda)
        {
            this.Ativo = ativo;

            this.CorretoraCompra = corretoraCompra;
            this.QuantidadeCompra = quantidadeCompra;
            this.PrecoCompra = precoCompra;

            this.CorretoraVenda = corretoraVenda;
            this.QuantidadeVenda= quantidadeVenda;
            this.PrecoVenda= precoVenda;
        }
    }
}
