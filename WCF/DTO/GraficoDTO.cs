using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    [DataContract]
    public class GraficoDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Ativo { get; set; }
        [DataMember]
        public List<LayoutDTO> Layouts { get; set; }
        [DataMember]
        public int Left { get; set; }
        [DataMember]
        public int Top { get; set; }
        [DataMember]
        public int Width { get; set; }
        [DataMember]
        public int Height { get; set; }
        [DataMember]
        public int WorkspaceId { get; set; }
        [DataMember]
        public int Periodicidade { get; set; }
        [DataMember]
        public int UsuarioId { get; set; }
        [DataMember]
        public DateTime DataCadastro { get; set; }
        
    }
}
