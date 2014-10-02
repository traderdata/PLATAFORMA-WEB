using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Traderdata.Client.TerminalWEB.Dialogs
{
    public partial class NumericUpDownUC : UserControl
    {
        NumericUpDown number = new NumericUpDown();
        public NumericUpDownUC()
        {
            InitializeComponent();
            LayoutRoot.Children.Add(number);
        }

        private void LayoutRoot_Loaded_1(object sender, RoutedEventArgs e)
        {
            
            
        }

        public void SetValue(double value)
        {
            number.Value = value;
        }

        public double Value()
        {
            return number.Value;
        }
    }
}
