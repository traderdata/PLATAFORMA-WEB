﻿#pragma checksum "C:\Backup\Projetos2012\CLIENT\TERMINAL-WEB-2012\SL\ContainerDireita.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "26FA74AE86ED431FB1DFAA05D9CBDB72"
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


namespace Traderdata.Client.TerminalWEB {
    
    
    public partial class ContainerDireita : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal C1.Silverlight.C1TabControl tabControl;
        
        internal C1.Silverlight.C1TabItem tabPortfolio;
        
        internal C1.Silverlight.C1TabItem tabChat;
        
        internal C1.Silverlight.C1TabItem tabRastreadorDiario;
        
        internal C1.Silverlight.C1TabItem tabRastreadorIntraday;
        
        internal C1.Silverlight.C1TabItem tabVideoAula;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TerminalWeb;component/ContainerDireita.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.tabControl = ((C1.Silverlight.C1TabControl)(this.FindName("tabControl")));
            this.tabPortfolio = ((C1.Silverlight.C1TabItem)(this.FindName("tabPortfolio")));
            this.tabChat = ((C1.Silverlight.C1TabItem)(this.FindName("tabChat")));
            this.tabRastreadorDiario = ((C1.Silverlight.C1TabItem)(this.FindName("tabRastreadorDiario")));
            this.tabRastreadorIntraday = ((C1.Silverlight.C1TabItem)(this.FindName("tabRastreadorIntraday")));
            this.tabVideoAula = ((C1.Silverlight.C1TabItem)(this.FindName("tabVideoAula")));
        }
    }
}
