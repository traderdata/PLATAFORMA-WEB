
namespace Traderdata.Server.App.TerminalWeb.DTO
{
    /// <summary>
    /// Classe que guarda os valores das condições de teste.
    /// </summary>
    public class CondicaoValorBacktestDTO
    {
        /// <summary>Obtém ou seta o Id da configuração.</summary>
        public virtual int Id { get; set; }

        /// <summary>Obtém ou seta o Id da condição.</summary>
        public virtual int IdCondicao { get; set; }

        /// <summary>Obtém ou seta o Id do backtest ou template.</summary>
        public virtual int IdBackTest { get; set; }

        /// <summary>Obtém ou seta o Id do backtest ou template.</summary>
        public virtual int IdTemplate { get; set; }

        /// <summary>Obtém ou seta o valor inteiro.</summary>
        public virtual int ValorInteiro { get; set; }

        /// <summary>Obtém ou seta o valor double.</summary>
        public virtual double ValorDouble { get; set; }

        /// <summary>Obtém ou seta o valor string.</summary>
        public virtual string ValorString { get; set; }

        /// <summary>Obtém ou seta o id da condição parcela.</summary>
        public virtual int IdCondicaoParcela { get; set; }

        /// <summary>Obtém ou seta o tipo de condição.</summary>
        public virtual int TipoCondicao { get; set; }

        /// <summary>Obtém ou seta se representa um template ou backtest.</summary>
        //public virtual bool CondicaoTemplate { get; set; }
    }
}
