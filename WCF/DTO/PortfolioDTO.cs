using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    public class PortfolioDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Ativos { get; set; }
        public string Colunas { get; set; }
        public string TamanhoColunas { get; set; }
        public int UserId { get; set; }
        public bool Publico { get; set; }
    }
}
