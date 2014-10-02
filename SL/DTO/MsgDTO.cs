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
    public class MsgDTO
    {
        public string Autor { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
    }
}
