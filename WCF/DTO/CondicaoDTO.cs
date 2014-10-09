using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    [DataContract]
    public class CondicaoDTO
    {
        public CondicaoDTO()
        {
            this.ListaParcelas = new List<CondicaoParcelaDTO>();
        }


        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public string Comando { get; set; }

        [DataMember]
        public List<CondicaoParcelaDTO> ListaParcelas { get; set; }
        
    }
}
