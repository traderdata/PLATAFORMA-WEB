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
    public class ScannerDTO
    {
        public string Estrategia { get; set; }
        public string Periodicidade { get; set; }
        public string Ativo { get; set; }
        public double Ultimo { get; set; }
        public string Hora { get; set; }
    }
}
