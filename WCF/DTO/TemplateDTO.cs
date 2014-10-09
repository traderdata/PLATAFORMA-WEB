using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    [DataContract]
    public class TemplateDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public int Periodicidade { get; set; }
        [DataMember]
        public int UsuarioId { get; set; }
        [DataMember]
        public List<LayoutDTO> Layouts { get; set; }
    }
}
