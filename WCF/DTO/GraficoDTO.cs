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
        public LayoutDTO Layout { get; set; }
        [DataMember]
        public int Periodicidade { get; set; }
        [DataMember]
        public string PeriodicidadeStr { get; set; }
        [DataMember]
        public int UsuarioId { get; set; }
        [DataMember]
        public DateTime DataCadastro { get; set; }
        
    }
}
