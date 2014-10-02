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
using System.Collections.Generic;
using ModulusFE;
using System.Runtime.Serialization;

namespace Traderdata.Client.TerminalWEB.DTO
{
    [DataContract]
    public class IndicatorInfoDTO  
    {        
        [DataMember]
        public IndicatorType TipoStockchart { get; set; }
        [DataMember]
        public string NomePortugues { get; set; }
        [DataMember]
        public string Ajuda { get; set; }
        [DataMember]
        public bool TemSerieFilha1 { get; set; }
        [DataMember]
        public bool TemSerieFilha2 { get; set; }
        [DataMember]
        public bool NovoPainel { get; set; }
        [DataMember]
        public Color StrokeColor { get; set; }
        [DataMember]
        public int StrokeThickness { get; set; }
        [DataMember]
        public LinePattern StrokeType { get; set; }
        [DataMember]
        public List<IndicatorPropertyDTO> Propriedades { get; set; }

        public IndicatorInfoDTO Clone()
        {
            //return MemberwiseClone();
            IndicatorInfoDTO indicadorLocal = new IndicatorInfoDTO();
            indicadorLocal.Ajuda = Ajuda;
            indicadorLocal.NomePortugues = NomePortugues;
            indicadorLocal.NovoPainel = NovoPainel;
            indicadorLocal.Propriedades = Propriedades;
            indicadorLocal.StrokeColor = StrokeColor;
            indicadorLocal.StrokeThickness = StrokeThickness;
            indicadorLocal.StrokeType = StrokeType;
            indicadorLocal.TemSerieFilha1 = TemSerieFilha1;
            indicadorLocal.TemSerieFilha2 = TemSerieFilha2;
            indicadorLocal.TipoStockchart = TipoStockchart;

            //retornando
            return indicadorLocal;
        }
    }
}
