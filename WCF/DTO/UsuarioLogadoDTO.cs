using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    public class UsuarioLogadoDTO
    {
        public virtual int Id { get; set; }
        public virtual string Usuario { get; set; }
        public virtual DateTime Data { get; set; }        
    }
}
