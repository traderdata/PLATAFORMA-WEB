using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Traderdata.Client.TerminalWEB.Dialogs.Portfolio
{
    /// <summary>
    /// Interaction logic for StockTicker.xaml
    /// </summary>
    public partial class StockTicker : UserControl
    {
        public static readonly DependencyProperty ValueProperty = 
            DependencyProperty.Register(
            "Value", 
            typeof(double), 
            typeof(StockTicker), 
            new PropertyMetadata(0.0, ValueChanged));

        string _format = "n2";
        Storyboard _flash;
        string _bindingSource;
        bool _firstTime = true;

        static Brush _brNegative = new SolidColorBrush(Colors.Red);
        static Brush _brPositive = new SolidColorBrush(Colors.Green);
        static Color _clrNegative = Colors.Red;
        static Color _clrPositive = Colors.Green;

        public StockTicker()
        {
            InitializeComponent();
            _arrow.Fill = null;
            _flash = (Storyboard)Resources["_sbFlash"];
        }
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ticker = d as StockTicker;
            var value = (double)e.NewValue;
            var oldValue = (double)e.OldValue;
            
            // calculate percentage change
            var change = oldValue == 0 || double.IsNaN(oldValue) ? 0 : (value - oldValue) / oldValue;

            // update text
            ticker._txtValue.Text = value.ToString(ticker._format);
            
            // update flash color
            var ca = ticker._flash.Children[0] as ColorAnimation;

            // update symbol
            if (oldValue == value)            
                ticker._arrow.Fill = null;
            else if (oldValue > value)
            {
                ticker._stArrow.ScaleY = -1;
                ticker._arrow.Fill = _brNegative;
                ca.From = _clrNegative;
            }
            else
            {
                ticker._stArrow.ScaleY = +1;
                ticker._arrow.Fill = _brPositive;
                ca.From = _clrPositive;
            }


            // flash new value (but not right after the control was created)
            if (!ticker._firstTime && change != 0)
            {
                ticker._flash.Begin();
            }
            ticker._firstTime = false;
        }
        public string BindingSource
        {
            get { return _bindingSource; }
            set { _bindingSource = value; }
        }
        public string Format
        {
            get { return _format; }
            set
            {
                _format = value;
                _txtValue.Text = Value.ToString(_format);
            }
        }
    }
}
