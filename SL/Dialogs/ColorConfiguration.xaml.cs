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
    public partial class ColorConfiguration : UserControl
    {
        /// <summary>
        /// Representa o método que irá manipular o evento de seleção de ok
        /// </summary>
        /// <param name="tick"></param>
        public delegate void ClickOkHandler(object sender, EventArgs e);

        /// <summary>Evento disparado quando a ação de click no botao ok é executada.</summary>
        public event ClickOkHandler ClickOk;

        /// <summary>
        /// Representa o método que irá manipular o evento de quando abrir a tela
        /// </summary>
        /// <param name="tick"></param>
        public delegate void OpenHandler(object sender, EventArgs e);

        /// <summary>Evento disparado quando o form é aberto.</summary>
        public event OpenHandler Opened;

        public ColorConfiguration()
        {
            InitializeComponent();
        }

       

        private void btnOk_Click_1(object sender, RoutedEventArgs e)
        {            
            if (ClickOk != null)
                ClickOk(sender, e);
        }

        private void LayoutRoot_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (Opened != null)
                Opened(sender, e);
        }
    }
}
