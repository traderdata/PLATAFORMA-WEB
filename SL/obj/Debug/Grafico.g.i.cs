﻿#pragma checksum "C:\Backup\Projetos2012\CLIENT\TERMINAL-WEB-2012\SL\Grafico.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0FC6F6BDE048062D19314B4CF43F2081"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
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
        
        internal C1.Silverlight.C1ContextMenu mnuContextIndicador;
        
        internal C1.Silverlight.C1MenuItem mnuItemConfigurar;
        
        internal C1.Silverlight.C1MenuItem mnuItemExcluir;
        
        internal C1.Silverlight.C1MenuItem mnuAdicionarIndicador;
        
        internal C1.Silverlight.C1ContextMenu mnuContextIndicadorSemConfiguracao;
        
        internal C1.Silverlight.C1MenuItem mnuAdicionarIndicadorSemConfiguracao;
        
        internal ModulusFE.StockChartX _stockChartX;
        
        internal System.Windows.Controls.Border borderRegua;
        
        internal System.Windows.Controls.TextBlock txtDifPeriodoRegua;
        
        internal System.Windows.Controls.TextBlock txtDifPercentualRegua;
        
        internal System.Windows.Controls.TextBlock txtDifValorRegua;
        
        internal System.Windows.Controls.Border borderCommand;
        
        internal System.Windows.Controls.TextBox txtComando;
        
        internal System.Windows.Controls.MediaElement mediaAlert;
        
        internal System.Windows.Controls.Canvas canvasAbaixoStockchart;
        
        internal System.Windows.Controls.TextBlock lblDataHora;
        
        internal System.Windows.Controls.TextBlock lblAbertura;
        
        internal System.Windows.Controls.TextBlock lblMinimo;
        
        internal System.Windows.Controls.TextBlock lblMaximo;
        
        internal System.Windows.Controls.TextBlock lblUltimo;
        
        internal System.Windows.Controls.Border borderValorYPosicionado;
        
        internal System.Windows.Controls.TextBlock txtValorYPosicionado;
        
        internal System.Windows.Controls.Primitives.ScrollBar scrollbar;
        
        internal System.Windows.Controls.Button btnMaisLeft;
        
        internal System.Windows.Controls.Button btnMenosLeft;
        
        internal System.Windows.Controls.Button btnMenosRight;
        
        internal System.Windows.Controls.Button btnMaisRight;
        
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
            this.mnuContextIndicador = ((C1.Silverlight.C1ContextMenu)(this.FindName("mnuContextIndicador")));
            this.mnuItemConfigurar = ((C1.Silverlight.C1MenuItem)(this.FindName("mnuItemConfigurar")));
            this.mnuItemExcluir = ((C1.Silverlight.C1MenuItem)(this.FindName("mnuItemExcluir")));
            this.mnuAdicionarIndicador = ((C1.Silverlight.C1MenuItem)(this.FindName("mnuAdicionarIndicador")));
            this.mnuContextIndicadorSemConfiguracao = ((C1.Silverlight.C1ContextMenu)(this.FindName("mnuContextIndicadorSemConfiguracao")));
            this.mnuAdicionarIndicadorSemConfiguracao = ((C1.Silverlight.C1MenuItem)(this.FindName("mnuAdicionarIndicadorSemConfiguracao")));
            this._stockChartX = ((ModulusFE.StockChartX)(this.FindName("_stockChartX")));
            this.borderRegua = ((System.Windows.Controls.Border)(this.FindName("borderRegua")));
            this.txtDifPeriodoRegua = ((System.Windows.Controls.TextBlock)(this.FindName("txtDifPeriodoRegua")));
            this.txtDifPercentualRegua = ((System.Windows.Controls.TextBlock)(this.FindName("txtDifPercentualRegua")));
            this.txtDifValorRegua = ((System.Windows.Controls.TextBlock)(this.FindName("txtDifValorRegua")));
            this.borderCommand = ((System.Windows.Controls.Border)(this.FindName("borderCommand")));
            this.txtComando = ((System.Windows.Controls.TextBox)(this.FindName("txtComando")));
            this.mediaAlert = ((System.Windows.Controls.MediaElement)(this.FindName("mediaAlert")));
            this.canvasAbaixoStockchart = ((System.Windows.Controls.Canvas)(this.FindName("canvasAbaixoStockchart")));
            this.lblDataHora = ((System.Windows.Controls.TextBlock)(this.FindName("lblDataHora")));
            this.lblAbertura = ((System.Windows.Controls.TextBlock)(this.FindName("lblAbertura")));
            this.lblMinimo = ((System.Windows.Controls.TextBlock)(this.FindName("lblMinimo")));
            this.lblMaximo = ((System.Windows.Controls.TextBlock)(this.FindName("lblMaximo")));
            this.lblUltimo = ((System.Windows.Controls.TextBlock)(this.FindName("lblUltimo")));
            this.borderValorYPosicionado = ((System.Windows.Controls.Border)(this.FindName("borderValorYPosicionado")));
            this.txtValorYPosicionado = ((System.Windows.Controls.TextBlock)(this.FindName("txtValorYPosicionado")));
            this.scrollbar = ((System.Windows.Controls.Primitives.ScrollBar)(this.FindName("scrollbar")));
            this.btnMaisLeft = ((System.Windows.Controls.Button)(this.FindName("btnMaisLeft")));
            this.btnMenosLeft = ((System.Windows.Controls.Button)(this.FindName("btnMenosLeft")));
            this.btnMenosRight = ((System.Windows.Controls.Button)(this.FindName("btnMenosRight")));
            this.btnMaisRight = ((System.Windows.Controls.Button)(this.FindName("btnMaisRight")));
            this.ctxMenu = ((C1.Silverlight.C1ContextMenu)(this.FindName("ctxMenu")));
        }
    }
}

