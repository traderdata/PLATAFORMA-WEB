using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Traderdata.Server.App.TerminalWeb.DTO
{
    [DataContract]
    public class CondicaoParcelaDTO
    {
        /// <summary>Enumerador que indica qual o tipo físico da parcela.</summary>
        public enum TipoFisicoEnum { Double, Int, String, Desconhecido }

        /// <summary>Enumerador que indica se é uma condição de entrada ou saída.</summary>
        public enum TipoCondicaoEnum { Entrada = 0, Saida };

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CondicaoId { get; set; }

        /// <summary>
        /// Variavel somente usada no Backtesting
        /// </summary>
        [DataMember]
        public int IdBackTest { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public string TipoFisico { get; set; }

        [DataMember]
        public string TipoApresentacao { get; set; }

        [DataMember]
        public double ValorDouble { get; set; }

        [DataMember]
        public string ValorString { get; set; }

        [DataMember]
        public int ValorInteiro { get; set; }

        /// <summary>
        /// Obtém o tipo físico do resultado de forma enumerada.
        /// </summary>
        [DataMember]
        public virtual TipoFisicoEnum TipoFisicoEnumerado
        {
            get
            {
                switch (TipoFisico)
                {
                    case "I":
                        return TipoFisicoEnum.Int;

                    case "S":
                        return TipoFisicoEnum.String;

                    case "D":
                        return TipoFisicoEnum.Double;

                    default:
                        return TipoFisicoEnum.Desconhecido;
                }
            }
            set
            {
                switch (value)
                {
                    case TipoFisicoEnum.Double:
                        this.TipoFisico = "D";
                        break;
                    case TipoFisicoEnum.Int:
                        this.TipoFisico = "I";
                        break;
                    case TipoFisicoEnum.String:
                        this.TipoFisico = "S";
                        break;
                    case TipoFisicoEnum.Desconhecido:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
