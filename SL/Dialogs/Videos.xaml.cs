using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Traderdata.Client.TerminalWEB.Dialogs
{
    public partial class Videos : UserControl
    {
        public Videos()
        {
            InitializeComponent();
        }

        private void LayoutRoot_Loaded_1(object sender, RoutedEventArgs e)
        {
                    
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {            
            HtmlPage.Window.Navigate(new Uri("http://www.youtube.com/watch?v=pEPAep6oJTQ&feature=plcp", UriKind.RelativeOrAbsolute), "_new");
        }

        private void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri("http://www.youtube.com/watch?v=l22y1WXjr-8&feature=plcp", UriKind.RelativeOrAbsolute), "_new");            
        }

        private void HyperlinkButton_Click_3(object sender, RoutedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri("http://www.youtube.com/watch?v=xBbTuJDFXYw&feature=plcp", UriKind.RelativeOrAbsolute), "_new");            
        }
    }
}
