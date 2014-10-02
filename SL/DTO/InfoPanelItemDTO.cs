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
    public class InfoPanelItemDTO
    {
        public string Titulo { get; set; }
        public string Value { get; set; }
        public bool Separator { get; set; }

        public InfoPanelItemDTO(string titulo, string value, bool separator)
        {
            this.Titulo = titulo;
            this.Value = value;
            this.Separator = separator;
        }
    }
}
