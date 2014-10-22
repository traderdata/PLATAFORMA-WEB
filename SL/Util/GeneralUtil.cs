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
using System.Collections.Generic;
using ModulusFE;
using System.Text.RegularExpressions;

namespace Traderdata.Client.TerminalWEB.Util
{
    public static class GeneralUtil
    {
        public static int VisibleRecords { get; set; }

        public static TerminalWebSVC.LayoutDTO LayoutFake()
        {
            TerminalWebSVC.LayoutDTO layout = new TerminalWebSVC.LayoutDTO();
            layout.CorBordaCandleAlta = "#FF000000";
            layout.CorBordaCandleBaixa = "#FF000000";
            layout.CorCandleAlta = "#FFFFFFFF";
            layout.CorCandleBaixa = "#FF000000";
            layout.CorFundo = "#FFFFFFFF";
            layout.CorGrid = "#FFF2F2F2";
            layout.CorEscala = "#FF000000";
            layout.CorVolume = "#FFDDD9C3";
            layout.EspacoADireitaDoGrafico = 20;
            layout.EspacoAEsquerdaDoGrafico = 10;
            layout.PosicaoEscala = 1;
            layout.PainelInfo = true;
            layout.ZoomRealtime = true;
            layout.VisibleRecords = 200;
            layout.VolumeStrokeThickness = 7;
            layout.UsarCoresAltaBaixaVolume = false;
            layout.GradeVertical = true;
            layout.GradeHorizontal = true;
            layout.PrecisaoEscala = 2;

            TerminalWebSVC.PainelDTO painelPreco = new TerminalWebSVC.PainelDTO();
            painelPreco.Index = 0;
            painelPreco.TipoPainel = "P";
            painelPreco.Status = "N";
            painelPreco.Altura = 75;

            TerminalWebSVC.PainelDTO painelVolume = new TerminalWebSVC.PainelDTO();
            painelVolume.Index = 1;
            painelVolume.TipoPainel = "V";
            painelVolume.Status = "N";
            painelVolume.Altura = 25;

            List<TerminalWebSVC.PainelDTO> ListaPainel = new List<TerminalWebSVC.PainelDTO>();
            ListaPainel.Add(painelPreco);
            ListaPainel.Add(painelVolume);


            layout.Paineis = ListaPainel;
            layout.PosicaoEscala = 1;
            layout.TipoEscala = (int)ScalingTypeEnum.Linear;
            layout.EstiloBarra = (int)SeriesTypeEnum.stCandleChart;

            return layout;
        }

        public static void ChangeVisibleRecordsFakeLayout(int record)
        {

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

        /// <summary>
        /// Permite ao textbox somente número decimal.
        /// </summary>
        public static void ValidaNumeroDecimal(TextBox txt, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
                return;

            var thisKeyStr = "";

            string separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            if ((separator == "." && (e.PlatformKeyCode == 190 || e.PlatformKeyCode == 110)) || (separator == "," && e.PlatformKeyCode == 188))
                thisKeyStr = separator;
            else
                thisKeyStr = e.Key.ToString().Replace("D", "").Replace("NumPad", "");

            string s = txt.Text + thisKeyStr;
            string rStr = "^[0-9]+[" + separator + "]?[0-9]*$";
            var r = new Regex(rStr, RegexOptions.IgnoreCase);

            e.Handled = !r.IsMatch(s);
        }

        /// <summary>
        /// Metodo que converte de Hexa para SolidColorBrush
        /// </summary>
        /// <param name="hexaColor"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Metodo que retorna a periodicidade do gráfico
        /// </summary>
        /// <returns></returns>
        public static int GetIntPeriodicidade(Periodicidade periodicidade)
        {
            switch (periodicidade)
            {
                case TerminalWEB.Periodicidade.UmMinuto:
                    return 1;
                case TerminalWEB.Periodicidade.DoisMinutos:
                    return 2;
                case TerminalWEB.Periodicidade.TresMinutos:
                    return 3;
                case TerminalWEB.Periodicidade.CincoMinutos:
                    return 5;
                case TerminalWEB.Periodicidade.DezMinutos:
                    return 10;
                case TerminalWEB.Periodicidade.QuinzeMinutos:
                    return 15;
                case TerminalWEB.Periodicidade.TrintaMinutos:
                    return 30;
                case TerminalWEB.Periodicidade.SessentaMinutos:
                    return 60;
                case TerminalWEB.Periodicidade.CentoeVinteMinutos:
                    return 120;
                case TerminalWEB.Periodicidade.Diario:
                    return 1440;
                case TerminalWEB.Periodicidade.Semanal:
                    return 10080;
                case TerminalWEB.Periodicidade.Mensal:
                    return 43200;
                default:
                    return 1;
            }
        }

        /// <summary>
        /// Metodo retorna o nome da periodiciade ao inves do inteiro
        /// </summary>
        /// <param name="periodicidade"></param>
        /// <returns></returns>
        public static string GetPeriodicidadeFromIntToString(int periodicidade)
        {
            switch (periodicidade)
            {
                case 1:
                    return "1 Minuto";
                case 2:
                    return "2 Minutos";
                case 3:
                    return "3 Minutos";
                case 5:
                    return "5 Minutos";
                case 10:
                    return "10 Minutos";
                case 15:
                    return "15 Minutos";
                case 30:
                    return "30 Minutos";
                case 60:
                    return "1 Hora";
                case 120:
                    return "2 Horas";
                case 1440:
                    return "Diário";
                case 10080:
                    return "Semanal";
                case 43200:
                    return "Mensal";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Metodo que retorna a periodicidade do gráfico
        /// </summary>
        /// <returns></returns>
        public static Periodicidade GetPeriodicidadeFromInt(int periodicidade)
        {
            switch (periodicidade)
            {
                case 1:
                    return TerminalWEB.Periodicidade.UmMinuto;
                case 2:
                    return TerminalWEB.Periodicidade.DoisMinutos;
                case 3:
                    return TerminalWEB.Periodicidade.TresMinutos;
                case 5:
                    return TerminalWEB.Periodicidade.CincoMinutos;
                case 10:
                    return TerminalWEB.Periodicidade.DezMinutos;
                case 15:
                    return TerminalWEB.Periodicidade.QuinzeMinutos;
                case 30:
                    return TerminalWEB.Periodicidade.TrintaMinutos;
                case 60:
                    return TerminalWEB.Periodicidade.SessentaMinutos;
                case 120:
                    return TerminalWEB.Periodicidade.CentoeVinteMinutos;
                case 1440:
                    return TerminalWEB.Periodicidade.Diario;
                case 10080:
                    return TerminalWEB.Periodicidade.Semanal;
                case 43200:
                    return TerminalWEB.Periodicidade.Mensal;
                default:
                    return TerminalWEB.Periodicidade.Nenhum;
            }
        }

        /// <summary>
        /// Valida sestring tem formato de email
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string strIn)
        {
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            return r.IsMatch(strIn);
        }
    }
}
