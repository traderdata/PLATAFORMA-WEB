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
    public class TickDTO
    {
        //Propriedades principais
        public string Ativo { get; set; }
        public double Abertura { get; set; }
        public double Maximo { get; set; }
        public double Minimo { get; set; }
        public double Ultimo { get; set; }
        public double Media { get; set; }
        public double Quantidade { get; set; }
        public double Volume { get; set; }
        public DateTime Data { get; set; }
        public string Hora { get; set; }
        public double FechamentoAnterior { get; set; }
        public double Variacao { get; set; }
        public double MelhorOfertaCompra { get; set; }
        public double MelhorOfertaVenda { get; set; }
        public int NumeroNegocio { get; set; }
        public double QuantidadeMelhorOfertaCompra { get; set; }
        public double QuantidadeMelhorOfertaVenda { get; set; }
        public double QuantidadeUltimoNegocio { get; set; }
        public int Bolsa { get; set; }
        public double VolumeUltimoMinuto { get; set; }
        public double VolumeIncremento { get; set; }
        public bool Dirty { get; set; }
    }
}
