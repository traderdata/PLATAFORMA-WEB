using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    [DataContract]
    public class UsuarioDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Perfil { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string Senha { get; set; }
        [DataMember]
        public DateTime BovespaRT { get; set; }
        [DataMember]
        public DateTime BMFRT { get; set; }
        [DataMember]
        public DateTime BovespaDELAY { get; set; }
        [DataMember]
        public DateTime BMFDELAY { get; set; }
        [DataMember]
        public DateTime BovespaEOD { get; set; }
        [DataMember]
        public DateTime BMFEOD { get; set; }
        [DataMember]
        public int DistribuidorId { get; set; }
        [DataMember]
        public string FBToken { get; set; }
        [DataMember]
        public string Token { get; set; }
        [DataMember]
        public int RefId { get; set; }
        [DataMember]
        public DateTime Cadastro { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public string CPF { get; set; }
        
        [DataMember]
        public bool HasTrial { get; set; }
        [DataMember]
        public bool HasBovespaRT { get; set; }
        [DataMember]
        public bool HasBMFRT { get; set; }
        [DataMember]
        public bool HasBovespaDELAY { get; set; }
        [DataMember]
        public bool HasBMFDELAY { get; set; }
        [DataMember]
        public bool HasSnapshotBovespaDiario { get; set; }
        [DataMember]
        public bool HasSnapshotBMFDiario { get; set; }
        [DataMember]
        public bool HasSnapshotBovespaIntraday { get; set; }
        [DataMember]
        public bool HasSnapshotBMFIntraday { get; set; }
    }
}
