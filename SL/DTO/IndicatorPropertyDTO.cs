using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;

namespace Traderdata.Client.TerminalWEB.DTO
{
    [DataContract]
    public class IndicatorPropertyDTO
    {
        #region Propriedads

        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public TipoField TipoDoCampo { get; set; }
        [DataMember]
        public object Value { get; set; }
        [DataMember]
        public int IndexStockChart { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Construtor padrao
        /// </summary>
        /// <param name="label"></param>
        /// <param name="tipoCampo"></param>
        /// <param name="value"></param>
        public IndicatorPropertyDTO(string label, TipoField tipoCampo, object value, int indexStockChart)
        {
            this.Label = label;
            this.TipoDoCampo = tipoCampo;
            this.Value = value;
            this.IndexStockChart = indexStockChart;
        }

        #endregion
                
    }

 
    public enum TipoField {Data, Opcoes, NumericUpDownInteger, Serie, SymbolList, Media, Header, Double}
}
