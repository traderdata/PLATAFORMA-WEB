using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    public class ResultadoScannerDTO
    {
        public int Id { get; set; }
        public int ScannerId { get; set; }
        public string Ativo { get; set; }
        public DateTime Data { get; set; }
        public double Variacao { get; set; }
        public double Fechamento { get; set; }
        public double Abertura { get; set; }
        public double Maximo { get; set; }
        public double Minimo { get; set; }
        public double Volume { get; set; }
    }
}
