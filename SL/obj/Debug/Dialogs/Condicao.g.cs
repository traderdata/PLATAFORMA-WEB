﻿#pragma checksum "C:\Backup\Projetos2012\CLIENT\TERMINAL-WEB-2012\SL\Dialogs\Condicao.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "ECB9382B02BDA610B8EA7BF6F7DBDEDD"
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


namespace Traderdata.Client.TerminalWEB.Dialogs {
    
    
    public partial class Condicao : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.MediaElement somMonitor;
        
        internal System.Windows.Controls.StackPanel stackPanelCondicoes;
        
        internal System.Windows.Controls.DataGrid gridCondicoes;
        
        internal System.Windows.Controls.DataGridTemplateColumn lblCondicao;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TerminalWeb;component/Dialogs/Condicao.xaml", System.UriKind.Relative));
            this.somMonitor = ((System.Windows.Controls.MediaElement)(this.FindName("somMonitor")));
            this.stackPanelCondicoes = ((System.Windows.Controls.StackPanel)(this.FindName("stackPanelCondicoes")));
            this.gridCondicoes = ((System.Windows.Controls.DataGrid)(this.FindName("gridCondicoes")));
            this.lblCondicao = ((System.Windows.Controls.DataGridTemplateColumn)(this.FindName("lblCondicao")));
        }
    }
}
