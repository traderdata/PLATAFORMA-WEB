using System;
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
    public class TradeDTO
    {
        public virtual int Id { get; set; }
        public virtual string Ativo { get; set; }
        public virtual DateTime TimeStamp { get; set; }
        public virtual string HoraBolsa { get; set; }
        public virtual double Quantidade { get; set; }
        public virtual double Valor { get; set; }
        public virtual int Numero { get; set; }
        public virtual string CorretoraCompradora { get; set; }
        public virtual string CorretoraVendedora { get; set; }
        public virtual int Bolsa { get; set; }
        public virtual string TipoRegistro { get; set; }
        public virtual DateTime DataHora { get; set; }
    }
}
