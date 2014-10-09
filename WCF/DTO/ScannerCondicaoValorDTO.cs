using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    [DataContract]
    public class ScannerCondicaoValorDTO
    {        
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CondicaoId { get; set; }

        [DataMember]
        public int ParcelaId { get; set; }

        [DataMember]
        public int ScannerId { get; set; }

        [DataMember]
        public int ValorInteiro { get; set; }

        [DataMember]
        public double ValorDouble { get; set; }

        [DataMember]
        public string ValorString { get; set; }

    }
}
