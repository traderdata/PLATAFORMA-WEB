using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    [DataContract]
    public class ScannerDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public string Formula { get; set; }

        [DataMember]
        public int Periodicidade { get; set; }

        [DataMember]
        public List<CondicaoDTO> ListaCondicoes { get; set; }

        [DataMember]
        public string ListaAtivos { get; set; }

        [DataMember]
        public UsuarioDTO User { get; set; }

        [DataMember]
        public bool PublicarFacebook { get; set; }

        [DataMember]
        public bool EnviarEmail { get; set; }

        [DataMember]
        public List<ResultadoScannerDTO> Resultados { get; set; }

    }    
}
