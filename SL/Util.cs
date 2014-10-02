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
using System.Globalization;
using ModulusFE;

namespace Traderdata.Client.TerminalWEB
{
    public static class Util
    {

        public static TerminalWebSVC.LayoutDTO LayoutFake()
        {
            TerminalWebSVC.LayoutDTO layout = new TerminalWebSVC.LayoutDTO();
            layout.CorBordaCandleAlta = "#FFBBCC88";
            layout.CorBordaCandleBaixa = "#FFBBCC88";
            layout.CorCandleAlta = "#FFBBCC88";
            layout.CorCandleBaixa = "#FFBBCC88";
            layout.CorFundo = "#FFFFFFFF";
            layout.CorGrid = "#FFFFFFFF";
            layout.CorEscala = "#FFFFFFFF";
            layout.CorVolume = "#FFFFFFFF";
            layout.EspacoADireitaDoGrafico = 20;
            layout.PosicaoEscala = 1;
            layout.PainelInfo = true;
            layout.ZoomRealtime = true;
            layout.VisibleRecords = 50;
            layout.VolumeStrokeThickness = 7;
            layout.UsarCoresAltaBaixaVolume = false;
            layout.GradeVertical = true;
            layout.GradeHorizontal = true;
            layout.PrecisaoEscala = 2;
            layout.Paineis = new System.Collections.Generic.List<TerminalWebSVC.PainelDTO>();
            layout.PosicaoEscala = 1;
            layout.TipoEscala = (int)ScalingTypeEnum.Linear;
            layout.EstiloBarra = (int)SeriesTypeEnum.stCandleChart;

            return layout;
        }

        /// <summary>
        /// Provider padrão para número. Utiliza 2 casas decimais e separador decimal ".".
        /// </summary>
        public static NumberFormatInfo NumberProvider
        {
            get
            {
                NumberFormatInfo numberProvider = new NumberFormatInfo();
                numberProvider.NumberDecimalDigits = 2;
                numberProvider.NumberDecimalSeparator = ".";
                numberProvider.NumberGroupSeparator = "";

                return numberProvider.Clone() as NumberFormatInfo;
            }
        }

        public static SolidColorBrush GetColorFromHexa(string hexaColor)
        {
            if (hexaColor.Length > 0)
            {
                return new SolidColorBrush(
                    Color.FromArgb(
                        Convert.ToByte(hexaColor.Substring(1, 2), 16),
                        Convert.ToByte(hexaColor.Substring(3, 2), 16),
                        Convert.ToByte(hexaColor.Substring(5, 2), 16),
                        Convert.ToByte(hexaColor.Substring(7, 2), 16)
                    )
                );
            }
            else
                return new SolidColorBrush(Colors.Black);
        }
    }
}
