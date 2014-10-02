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
    public partial class StockTickerSymbol : UserControl
    {
        public static readonly DependencyProperty ValueProperty = 
            DependencyProperty.Register(
            "ValueSymbolName", 
            typeof(string), 
            typeof(StockTickerSymbol), 
            new PropertyMetadata("", ValueChanged));

        public static readonly DependencyProperty ValuePropertyVariacao =
            DependencyProperty.Register(
            "ValueVariacao",
            typeof(double),
            typeof(StockTickerSymbol),
            new PropertyMetadata(0.0, ValueChangedVariacao));

        string _bindingSource;
        
        public StockTickerSymbol()
        {
            InitializeComponent();                    
        }
        public string ValueSymbolName
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public double ValueVariacao
        {
            get { return (double)GetValue(ValuePropertyVariacao); }
            set { SetValue(ValuePropertyVariacao, value); }
        }
        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ticker = d as StockTickerSymbol;
            var value = (string)e.NewValue;

            // update text
            ticker._txtValue.Text = ticker.ValueSymbolName;
                       
        }
        public void SetColorRed()
        {
            _txtValue.Foreground = new SolidColorBrush(Colors.Red);
        }
        public void SetColorGreen()
        {
            _txtValue.Foreground = new SolidColorBrush(Colors.Green);
        }
        public void SetColorWhite()
        {
            _txtValue.Foreground = new SolidColorBrush(Colors.White);
        }
        private static void ValueChangedVariacao(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ticker = d as StockTickerSymbol;
            var value = (double)e.NewValue;

            // update text
            if (value > 0)
                ticker._txtValue.Foreground = new SolidColorBrush(Colors.Green);
            else
                ticker._txtValue.Foreground = new SolidColorBrush(Colors.Red);

        }
        public string BindingSource
        {
            get { return _bindingSource; }
            set { _bindingSource = value; }
        }
        
    }
}
