using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    public class MensagemDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Mensagem { get; set; }
        public DateTime Data { get; set; }
    }
}
