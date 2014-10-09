using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    [DataContract]
    public class LayoutDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int? GraficoId { get; set; }
        [DataMember]
        public int? TemplateId { get; set; }
        [DataMember]
        public  string CorFundo { get; set; }
        [DataMember]
        public string CorVolume { get; set; }
        [DataMember]
        public string CorEscala { get; set; }
        [DataMember]
        public string CorGrid { get; set; }
        [DataMember]
        public int VolumeStrokeThickness { get; set; }
        [DataMember]
        public  string CorBordaCandleAlta { get; set; }
        [DataMember]
        public  string CorBordaCandleBaixa { get; set; }
        [DataMember]
        public  string CorCandleAlta { get; set; }
        [DataMember]
        public  string CorCandleBaixa { get; set; }
        [DataMember]
        public  int? TipoEscala { get; set; }
        [DataMember]
        public  int? PrecisaoEscala { get; set; }
        [DataMember]
        public  bool? GradeHorizontal { get; set; }
        [DataMember]
        public  bool? GradeVertical { get; set; }
        [DataMember]
        public  bool? PainelInfo { get; set; }
        [DataMember]
        public  int? PosicaoEscala { get; set; }
        [DataMember]
        public  double? EspacoADireitaDoGrafico { get; set; }
        [DataMember]
        public double? EspacoAEsquerdaDoGrafico { get; set; }
        [DataMember]
        public  int? EstiloPreco { get; set; }
        [DataMember]
        public  int? EstiloBarra { get; set; }
        [DataMember]
        public  double? EstiloPrecoParam1 { get; set; }
        [DataMember]
        public  double? EstiloPrecoParam2 { get; set; }
        [DataMember]
        public  bool? UsarCoresAltaBaixaVolume { get; set; }
        [DataMember]
        public  bool? DarvaBox { get; set; }
        [DataMember]
        public  string TipoVolume { get; set; }
        [DataMember]
        public int Periodicidade { get; set; }
        [DataMember]
        public List<IndicadorDTO> Indicadores { get; set; }
        [DataMember]
        public List<PainelDTO>  Paineis { get; set; }
        [DataMember]
        public List<ObjetoEstudoDTO> Objetos { get; set; }
        [DataMember]
        public bool ZoomRealtime { get; set; }
        [DataMember]
        public int VisibleRecords { get; set; }
        [DataMember]
        public int FirstVisibleRecord { get; set; }
        [DataMember]
        public int Index { get; set; }
        
        

    }
}
