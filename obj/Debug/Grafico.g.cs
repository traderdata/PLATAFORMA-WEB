﻿#pragma checksum "C:\Backup\Projetos2012\CLIENT\TERMINAL-WEB-2012\Grafico.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8B69B55B59851A965E6DCADCF5C15526"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ModulusFE;
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
    
    
    public partial class Grafico : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.BusyIndicator busyIndicator;
        
        internal System.Windows.Controls.Grid gridPrincipal;
        
        internal ModulusFE.StockChartX _stockChartX;
        
        internal System.Windows.Controls.Canvas canvasAbaixoStockchart;
        
        internal System.Windows.Controls.Border borderDataPosicionada;
        
        internal System.Windows.Controls.TextBlock txtDataPosicionada;
        
        internal System.Windows.Controls.Border borderValorYPosicionado;
        
        internal System.Windows.Controls.TextBlock txtValorYPosicionado;
        
        internal System.Windows.Controls.Primitives.ScrollBar scrollbar;
        
        internal C1.Silverlight.C1ContextMenu ctxMenu;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TerminalWeb;component/Grafico.xaml", System.UriKind.Relative));
            this.busyIndicator = ((System.Windows.Controls.BusyIndicator)(this.FindName("busyIndicator")));
            this.gridPrincipal = ((System.Windows.Controls.Grid)(this.FindName("gridPrincipal")));
            this._stockChartX = ((ModulusFE.StockChartX)(this.FindName("_stockChartX")));
            this.canvasAbaixoStockchart = ((System.Windows.Controls.Canvas)(this.FindName("canvasAbaixoStockchart")));
            this.borderDataPosicionada = ((System.Windows.Controls.Border)(this.FindName("borderDataPosicionada")));
            this.txtDataPosicionada = ((System.Windows.Controls.TextBlock)(this.FindName("txtDataPosicionada")));
            this.borderValorYPosicionado = ((System.Windows.Controls.Border)(this.FindName("borderValorYPosicionado")));
            this.txtValorYPosicionado = ((System.Windows.Controls.TextBlock)(this.FindName("txtValorYPosicionado")));
            this.scrollbar = ((System.Windows.Controls.Primitives.ScrollBar)(this.FindName("scrollbar")));
            this.ctxMenu = ((C1.Silverlight.C1ContextMenu)(this.FindName("ctxMenu")));
        }
    }
}

