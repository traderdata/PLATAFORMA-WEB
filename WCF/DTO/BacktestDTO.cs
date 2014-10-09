using System;
using System.Collections.Generic;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    /// <summary>
    /// Classe que representa um BackTest.
    /// </summary>
    public class BacktestDTO : SumarioDTO
    {
        #region Campos

        /// <summary>
        /// Enumerado de status do teste.
        /// </summary>
        public enum StatusEnum { Executado = 0, EmExecucao }

        /// <summary>
        /// Enumerado de tipo de preço usado pelo teste.
        /// </summary>
        public enum TipoPrecoEnum { Abertura = 0, Minimo, Medio, Maximo, Ultimo }

        /// <summary>
        /// Enumerado de tipo de periodicidade.
        /// </summary>
        public enum TipoPeriodicidadeEnum { UmMinuto = 0, DoisMinutos, TresMinutos, CincoMinutos, DezMinutos, QuinzeMinutos, TrintaMinutos, SessentaMinutos, Diario, Semanal, Mensal }

        #endregion Campos

        #region Construtores

        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public BacktestDTO()
        {
            this.Ativo = "";
            this.CondicoesEntrada = new List<CondicaoDTO>();
            this.CondicoesSaida = new List<CondicaoDTO>();
            this.TipoPreco = (int)TipoPrecoEnum.Ultimo;
        }

        #endregion Construtores

        #region Propriedades

        /// <summary>
        /// Obtém ou seta o ID do teste.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Obtém ou seta o ativo que será analisado.
        /// </summary>
        public virtual string Ativo { get; set; }

        /// <summary>
        /// Obtém o status do teste.
        /// </summary>
        public StatusEnum StatusEnumerado
        {
            get { return (StatusEnum)Status; }
            set { Status = (int)value; }
        }

        /// <summary>
        /// Obtém ou seta o status do teste.
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// Obtém o status do teste em formato string.
        /// </summary>
        public string StatusStr
        {
            get
            {
                switch (StatusEnumerado)
                {
                    case StatusEnum.Executado:
                        return "Executado";
                    case StatusEnum.EmExecucao:
                        return "Em execução";
                    default:
                        return "";
                }
            }
            //HACK: Obrigatório para que a propriedade aparece no contrato do wcf...
            set { }
        }

        /// <summary>
        /// Obtém ou seta a observação para o teste.
        /// </summary>
        public virtual string Observacao { get; set; }

        /// <summary>
        /// Obtém ou seta o nome do teste.
        /// </summary>
        public virtual string Nome { get; set; }

        /// <summary>
        /// Obtém ou seta o volume financeiro inciado na execução do teste.
        /// </summary>
        public virtual double VolumeFinanceiroInicial { get; set; }

        /// <summary>
        /// Obtém ou seta a data de início do teste.
        /// </summary>
        public virtual DateTime DataInicio { get; set; }

        /// <summary>
        /// Data de início no formato string, de acordo com a periodicidade.
        /// </summary>
        public virtual string DataInicioStr
        {
            get
            {
                switch (PeriodicidadeEnumerado)
                {
                    case TipoPeriodicidadeEnum.Diario:
                    case TipoPeriodicidadeEnum.Semanal:
                    case TipoPeriodicidadeEnum.Mensal:
                        return DataInicio.ToString("dd/MM/yyyy");
                    default:
                        return DataInicio.ToString("dd/MM/yyyy HH:mm");
                }
            }
            set { }
        }

        /// <summary>
        /// Obtém ou seta a data de término do teste.
        /// </summary>
        public virtual DateTime DataTermino { get; set; }


        /// <summary>
        /// Data de termino no formato string, de acordo com a periodicidade.
        /// </summary>
        public virtual string DataTerminoStr
        {
            get
            {
                switch (PeriodicidadeEnumerado)
                {
                    case TipoPeriodicidadeEnum.Diario:
                    case TipoPeriodicidadeEnum.Semanal:
                    case TipoPeriodicidadeEnum.Mensal:
                        return DataTermino.ToString("dd/MM/yyyy");
                    default:
                        return DataTermino.ToString("dd/MM/yyyy HH:mm");
                }
            }
            set { }
        }


        /// <summary>
        /// Obtém a periodicidade do teste.
        /// </summary>
        public TipoPeriodicidadeEnum PeriodicidadeEnumerado
        {
            get { return (TipoPeriodicidadeEnum)Periodicidade; }
            set { Periodicidade = (int)value; }
        }

        /// <summary>
        /// Obtém ou seta a periodicidade do teste.
        /// </summary>
        public virtual int Periodicidade { get; set; }

        /// <summary>
        /// Obtém o tipo de preço no qual o teste irá se basear.
        /// </summary>
        public TipoPrecoEnum TipoPrecoEnumerado
        {
            get { return (TipoPrecoEnum)TipoPreco; }
            set { TipoPreco = (int)value; }
        }

        /// <summary>
        /// Obtém ou seta o tipo de preço no qual o teste irá se basear.
        /// </summary>
        public virtual int TipoPreco { get; set; }

        /// <summary>
        /// Obtém ou seta o flag que indica se ao final de um período a posição deve ser liquidada, 
        /// independente de não atingir uma condição.
        /// </summary>
        public virtual bool LiquidarPosicaoFinalPeriodo { get; set; }

        /// <summary>
        /// Obtém ou seta o flag que indica se deve sair em stop loss.
        /// </summary>
        public virtual bool SairEmStopLoss { get; set; }

        /// <summary>
        /// Obtém ou seta o flag que indica se deve sair em stop gain.
        /// </summary>
        public virtual bool SairEmStopGain { get; set; }

        /// <summary>
        /// Obtém ou seta o percentual sobre o preço de entrada.
        /// </summary>
        public virtual double PercentualStopLoss { get; set; }

        /// <summary>
        /// Obtém ou seta o percentual sobre o preço de saida.
        /// </summary>
        public virtual double PercentualStopGain { get; set; }

        /// <summary>
        /// Obtém ou seta as condições usadas para realizar compras ao longo do teste.
        /// </summary>
        public List<CondicaoDTO> CondicoesEntrada { get; set; }

        /// <summary>
        /// Obtém ou seta as condições usadas para realizar vendas, uma vez que esteja comprado, ao longo do teste.
        /// </summary>
        public List<CondicaoDTO> CondicoesSaida { get; set; }

        /// <summary>
        /// Obtém ou seta a flag que indica se deve ser considerada corretagem mais emolumento nos cálculos. Caso seja verdadeira, utilizar o campo Emulumento.
        /// </summary>
        public virtual bool ConsiderarCorretagemMaisEmolumento { get; set; }

        /// <summary>
        /// Obtém ou seta a flag que indica se será permitido operar descoberto. Caso seja verdadeira, utilizar o campo ValorExposicaoMaxima.
        /// </summary>
        public virtual bool PermitirOperacaoDescoberto { get; set; }

        /// <summary>
        /// Obtém ou seta o valor do emulumento quando for considerada a corretagem mais emolumento. Usar somente quando a flag ConsiderarCorretagemEmolumento estiver ativa.
        /// </summary>
        public virtual double ValorCorretagem { get; set; }

        /// <summary>
        /// Obtém ou seta o valor de exposição máxima para operações descoberto. Usar somente quando a flag PermitirOperacaoDescoberto estiver ativa.
        /// </summary>
        public virtual double ValorExposicaoMaxima { get; set; }

        /// <summary>
        /// Obtém ou seta o cliente.
        /// </summary>
        public virtual UsuarioDTO Usuario { get; set; }

        #endregion Propriedades
    }
}
