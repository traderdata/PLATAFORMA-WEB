using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Traderdata.Client.TerminalWEB.Util
{
    public class NumericConverter : IValueConverter
    {
        public NumericConverter()
        {
            this.FormatDouble = "N2";
        }

        public string FormatDouble { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.GetType().Name)
            {
                case "Int16":
                    return ((Int16)value).ToString("N0");
                case "Int32":
                    return ((Int32)value).ToString("N0");
                case "Int64":
                    return ((Int64)value).ToString("N0");
                case "Double":
                    return ((Double)value).ToString(this.FormatDouble);
                case "Single":
                    return ((Single)value).ToString("N2");
                default:
                    return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ChangeType(value, targetType, culture.NumberFormat);
        }
    }
}

