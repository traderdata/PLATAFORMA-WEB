using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    [DataContract]
    public class ObjetoEstudoDTO
    {
        [DataMember]
        public virtual int Id { get; set; }
        [DataMember]
        public virtual int LayoutId { get; set; }
        [DataMember]
        public virtual string CorObjeto { get; set; }
        [DataMember]
        public virtual int? Espessura { get; set; }
        [DataMember]
        public virtual int? TipoLinha { get; set; }
        [DataMember]
        public virtual bool? Magnetica { get; set; }
        [DataMember]
        public virtual bool? InfinitaADireita { get; set; }
        [DataMember]
        public virtual decimal? ValorErrorChannel { get; set; }
        [DataMember]
        public virtual string Texto { get; set; }
        [DataMember]
        public virtual int? TamanhoTexto { get; set; }
        [DataMember]
        public virtual int IndexPainel { get; set; }
        [DataMember]
        public virtual int RecordInicial { get; set; }
        [DataMember]
        public virtual int RecordFinal { get; set; }
        [DataMember]
        public virtual double ValorInicial { get; set; }
        [DataMember]
        public virtual double ValorFinal { get; set; }
        [DataMember]
        public virtual int TipoObjeto { get; set; }
        [DataMember]
        public virtual string Parametros { get; set; }
        
    }
}
