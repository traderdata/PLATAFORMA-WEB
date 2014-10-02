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
using System.Windows.Media.Imaging;

namespace Traderdata.Client.TerminalWEB.Dialogs
{
    public partial class CustomCursor : UserControl
    {
        public CustomCursor()
        {
            InitializeComponent();
        }
        
        public void SetCursor(string resource)
        {
            Uri uri = new Uri(resource, UriKind.RelativeOrAbsolute);
            MyCursor.Source = new BitmapImage(uri);
            MyCursor.Width = 16;
            MyCursor.Height = 16;
        }

        public void MoveTo(Point pt)
        {
            this.SetValue(Canvas.LeftProperty, pt.X);
            this.SetValue(Canvas.TopProperty, pt.Y);
        }
    }
}
