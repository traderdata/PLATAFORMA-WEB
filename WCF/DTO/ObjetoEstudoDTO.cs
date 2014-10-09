using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    public class ObjetoEstudoDTO
    {
        public virtual int Id { get; set; }
        public virtual int LayoutId { get; set; }
        public virtual string CorObjeto { get; set; }
        public virtual int? Espessura { get; set; }
        public virtual int? TipoLinha { get; set; } 
        public virtual bool? Magnetica { get; set; }
        public virtual bool? InfinitaADireita { get; set; }
        public virtual decimal? ValorErrorChannel { get; set; }
        public virtual string Texto { get; set; }
        public virtual int? TamanhoTexto { get; set; }
        public virtual int IndexPainel { get; set; }
        public virtual int RecordInicial { get; set; }
        public virtual int RecordFinal { get; set; }
        public virtual double ValorInicial { get; set; }
        public virtual double ValorFinal { get; set; }
        public virtual int TipoObjeto { get; set; }
        public virtual string Parametros { get; set; }
        
    }
}
