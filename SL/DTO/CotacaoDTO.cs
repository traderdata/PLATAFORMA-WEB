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
    public class CotacaoDTO
    {        
        public  double Abertura { get; set; }
        public  double Maximo { get; set; }
        public  double Minimo { get; set; }
        public  double Ultimo { get; set; }
        public  double Quantidade { get; set; }
        public  double Volume { get; set; }
        public  DateTime Data { get; set; }
        public  bool AfterMarket { get; set; }
        public  string Hora { get; set; }

        public CotacaoDTO()
        { }

        public CotacaoDTO(double abertura, double maximo, double minimo, double ultimo, double quantidade,
            double volume, DateTime data, bool afterMarket, string hora)
        {
            this.Abertura = abertura;
            this.Maximo = maximo;
            this.Minimo = minimo;
            this.Quantidade = quantidade;
            this.Ultimo = ultimo;
            this.Volume = volume;
            this.Data = data;
            this.AfterMarket = afterMarket;
            this.Hora = hora;
        }
    }
}
