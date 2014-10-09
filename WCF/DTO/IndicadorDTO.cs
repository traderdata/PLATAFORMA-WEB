using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    [DataContract(IsReference = true)]
    public class IndicadorDTO
    {
        [DataMember]
        public virtual int Id { get; set; }
        [DataMember]
        public virtual int? LayoutId { get; set; }
        [DataMember]
        public virtual string Cor { get; set; }
        [DataMember]
        public virtual string CorFilha1 { get; set; }
        [DataMember]
        public virtual string CorFilha2 { get; set; }

        [DataMember]
        public virtual int? TipoLinha { get; set; }
        [DataMember]
        public virtual int? TipoLinhaFilha1 { get; set; }
        [DataMember]
        public virtual int? TipoLinhaFilha2 { get; set; }

        [DataMember]
        public virtual int? Espessura { get; set; }
        [DataMember]
        public virtual int? EspessuraFilha1 { get; set; }
        [DataMember]
        public virtual int? EspessuraFilha2 { get; set; }

        [DataMember]
        public virtual string Parametros { get; set; }
        [DataMember]
        public virtual int? TipoIndicador { get; set; }
        [DataMember]
        public virtual int? IndexPainel { get; set; }

        [DataMember]
        public virtual string Name { get; set; }
    }
}
