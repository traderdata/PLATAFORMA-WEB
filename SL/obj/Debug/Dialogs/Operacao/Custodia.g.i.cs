﻿#pragma checksum "C:\Backup\Projetos2012\CLIENT\TERMINAL-WEB-2012\SL\Dialogs\Operacao\Custodia.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C5BD6F2BF0715A17A95C74FA76BF5ADD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Traderdata.Client.TerminalWEB.Dialogs.Operacao {
    
    
    public partial class Custodia : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ComboBox cmbConta;
        
        internal C1.Silverlight.FlexGrid.C1FlexGrid _flexFinancial;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/TerminalWeb;component/Dialogs/Operacao/Custodia.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.cmbConta = ((System.Windows.Controls.ComboBox)(this.FindName("cmbConta")));
            this._flexFinancial = ((C1.Silverlight.FlexGrid.C1FlexGrid)(this.FindName("_flexFinancial")));
        }
    }
}

