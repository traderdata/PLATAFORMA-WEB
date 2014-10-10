using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    [DataContract]
    public class PainelDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int LayoutId { get; set; }
        [DataMember]
        public int Altura { get; set; }
        [DataMember]
        public string TipoPainel { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int Index { get; set; }
    }
}
