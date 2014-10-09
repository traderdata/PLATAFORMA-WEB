using System;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    /// <summary>
    /// Classe que representa uma das muitas operações geradas por um teste.
    /// </summary>
    public class ResultadoBacktestDTO
    {
        #region Campos

        /// <summary>Enumerador que representa o tipo da operação: Compra ou Venda.</summary>
        public enum OperacaoEnum { Compra = 0, Venda }

        #endregion Campos

        #region Construtores

        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public ResultadoBacktestDTO()
        {
        }

        #endregion Construtores

        #region Propriedades

        /// <summary>Obtém ou seta o ID do objeto.</summary>
        public virtual int Id { get; set; }

        /// <summary>Obtém ou seta o ID do Teste.</summary>
        public virtual int IdBackTest { get; set; }

        /// <summary>Obtém ou seta a data e hora da operação.</summary>
        public virtual DateTime DataHora { get; set; }

        /// <summary>Obtém ou seta o preço nominal da operação.</summary>
        public virtual double Preco { get; set; }

        /// <summary>Obtém ou seta o tipo da operação: Comrpa ou Venda.</summary>
        public virtual OperacaoEnum OperacaoEnumerado { get { return (OperacaoEnum)Operacao; } }

        /// <summary>Obtém ou seta o tipo da operação: Comrpa ou Venda.</summary>
        public virtual int Operacao { get; set; }

        /// <summary>Obtém ou seta a quantidade envolvida na operação.</summary>
        public virtual double Quantidade { get; set; }

        /// <summary>Obtém ou seta a flag que indica se a operação ocorreu por conta de um StopGain.</summary>
        public virtual bool StopGainAtingido { get; set; }

        /// <summary>Obtém ou seta a flag que indica se a operação ocorreu por conta de um StopLoss.</summary>
        public virtual bool StopLossAtingido { get; set; }

        /// <summary>Obtém ou seta o saldo parcial após a operação.</summary>
        public virtual double SaldoParcial { get; set; }

        /// <summary>Obtém ou seta custodia parcial após a operação.</summary>
        public virtual double CustodiaParcial { get; set; }

        /// <summary>Obtém ou seta o saldo total (saldo liquido + custodia) após a operação.</summary>
        public virtual double SaldoTotal { get; set; }

        /// <summary>Obtém ou seta a rentabilidade acumulada até esta operação, inclusive.</summary>
        public virtual double RentabilidadeAcumulada { get; set; }

        /// <summary>Obtém ou seta a rentabilidade nesta operação.</summary>
        public virtual double Rentabilidade { get; set; }

        #endregion Propriedades
    }
}
