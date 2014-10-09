using System.Collections.Generic;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    /// <summary>
    /// Classe que representa os resultados de um backtest.
    /// </summary>
    public class SumarioDTO
    {
        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public SumarioDTO()
        {
            Operacoes = new List<ResultadoBacktestDTO>();
        }

        /// <summary>
        /// Construtor que já inicia as operações.
        /// </summary>
        /// <param name="operacoes"></param>
        public SumarioDTO(List<ResultadoBacktestDTO> operacoes)
        {
            this.Operacoes = operacoes;
        }

        /// <summary>Obtém ou seta o resultado máximo obtido.</summary>
        public virtual double ResultadoMaximo { get; set; }

        /// <summary>Obtém ou seta o resultado mínimo obtido.</summary>
        public virtual double ResultadoMinimo { get; set; }

        /// <summary>Obtém ou seta o resultado médio obtido.</summary>
        public virtual double ResultadoMedio { get; set; }

        /// <summary>Obtém ou seta o resultado final obtido.</summary>
        public virtual double ResultadoFinal { get; set; }

        /// <summary>Obtém ou seta o resultado total obtido.</summary>
        public virtual double ResultadoTotal { get; set; }

        /// <summary>Obtém ou seta a quantidade de stop gains ocorridas.</summary>
        public virtual int QtdStopGain { get; set; }

        /// <summary>Obtém ou seta a quantidade de stop loss ocorridas.</summary>
        public virtual int QtdStopLoss { get; set; }

        /// <summary>Obtém ou seta a quantidad de operações bem sucedidas.</summary>
        public virtual int OpBemSucedidas { get; set; }

        /// <summary>Obtém ou seta a quantidad de operações mal sucedidas.</summary>
        public virtual int OpMalSucedidas { get; set; }

        /// <summary>Obtém ou seta quantidade de trades feitos.</summary>
        public virtual int QtdTrades { get; set; }

        /// <summary>Obtém ou seta a posição final do usuário.</summary>
        public virtual int PosicaoFinal { get; set; }

        /// <summary>Obtém ou seta a lista de operações realizadas.</summary>
        public virtual List<ResultadoBacktestDTO> Operacoes { get; set; }
    }
}
