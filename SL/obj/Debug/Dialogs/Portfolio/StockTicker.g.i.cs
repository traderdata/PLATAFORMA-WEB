﻿#pragma checksum "C:\Backup\Projetos2012\CLIENT\TERMINAL-WEB-2012\SL\Dialogs\Portfolio\StockTicker.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F082E0E5C035514CD6F185192EDB9831"
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


namespace Traderdata.Client.TerminalWEB.Dialogs.Portfolio {
    
    
    public partial class StockTicker : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid _root;
        
        internal System.Windows.Shapes.Polygon _arrow;
        
        internal System.Windows.Media.ScaleTransform _stArrow;
        
        internal System.Windows.Controls.TextBlock _txtValue;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TerminalWeb;component/Dialogs/Portfolio/StockTicker.xaml", System.UriKind.Relative));
            this._root = ((System.Windows.Controls.Grid)(this.FindName("_root")));
            this._arrow = ((System.Windows.Shapes.Polygon)(this.FindName("_arrow")));
            this._stArrow = ((System.Windows.Media.ScaleTransform)(this.FindName("_stArrow")));
            this._txtValue = ((System.Windows.Controls.TextBlock)(this.FindName("_txtValue")));
        }
    }
}

