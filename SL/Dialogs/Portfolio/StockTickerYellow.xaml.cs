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
    public partial class StockTickerYellow : UserControl
    {
        public static readonly DependencyProperty ValueProperty = 
            DependencyProperty.Register(
            "Value", 
            typeof(double), 
            typeof(StockTickerYellow), 
            new PropertyMetadata(0.0, ValueChanged));

        string _format = "n2";
        Storyboard _flash;
        string _bindingSource;
        bool _firstTime = true;

        static Brush _brYellow = new SolidColorBrush(Colors.Yellow);
        
        public StockTickerYellow(string format)
        {
            InitializeComponent();
            this._format = format;
            _flash = (Storyboard)Resources["_sbFlash"];
        }
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ticker = d as StockTickerYellow;
            var value = (double)e.NewValue;
           
            // update text
            ticker._txtValue.Text = value.ToString(ticker._format);
            
            // update flash color
            var ca = ticker._flash.Children[0] as ColorAnimation;
            ca.From = Colors.Orange;

            // flash new value (but not right after the control was created)
            if (!ticker._firstTime )
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
