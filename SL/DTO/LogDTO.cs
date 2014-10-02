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
    public class LogDTO
    {
        public DateTime DataHora { get; set; }
        public string Texto { get; set; }

        public LogDTO(DateTime timestamp, string texto)
        {
            DataHora = timestamp;
            Texto = texto;
        }
    }
}
