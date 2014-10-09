using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    public class AnaliseCompartilhadaDTO
    {
        public virtual int Id { get; set; }
        public virtual UsuarioDTO Usuario { get; set; }
        public virtual string Ativo { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual string Comentario { get; set; }
        public virtual string CaminhoImagem { get; set; }
        public virtual GraficoDTO Grafico { get; set; }
    }
}
