﻿#pragma checksum "C:\Backup\Projetos2012\CLIENT\TERMINAL-WEB-2012\SL\Dialogs\Backtest\VisualizaResultadoBacktest.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0EB5697B4BA7EB6CFC25974D5EB1E206"
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


namespace Traderdata.Client.TerminalWEB.Dialogs.Backtest {
    
    
    public partial class VisualizaResultadoBacktest : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TabControl tabResultado;
        
        internal System.Windows.Controls.TabItem tabItem1;
        
        internal System.Windows.Controls.TextBlock lblNomeTeste;
        
        internal System.Windows.Controls.TextBlock lblResultadoMax;
        
        internal System.Windows.Controls.DescriptionViewer ttpResultMax;
        
        internal System.Windows.Controls.TextBlock lblResultadoMin;
        
        internal System.Windows.Controls.DescriptionViewer ttpResultMin;
        
        internal System.Windows.Controls.TextBlock lblResultadoMedio;
        
        internal System.Windows.Controls.DescriptionViewer ttpResultMed;
        
        internal System.Windows.Controls.TextBlock lblResultadoFinal;
        
        internal System.Windows.Controls.DescriptionViewer ttpResultFinal;
        
        internal System.Windows.Controls.TextBlock lblSaldoTotal;
        
        internal System.Windows.Controls.DescriptionViewer ttpSaldoTotal;
        
        internal System.Windows.Controls.TextBlock lblQteGain;
        
        internal System.Windows.Controls.DescriptionViewer ttpQtdStopGain;
        
        internal System.Windows.Controls.TextBlock lblQteLoss;
        
        internal System.Windows.Controls.DescriptionViewer ttpQtdStopLoss;
        
        internal System.Windows.Controls.TextBlock lblOpBemSucedidas;
        
        internal System.Windows.Controls.DescriptionViewer ttpOpBemSucess;
        
        internal System.Windows.Controls.TextBlock lblOpMalSucedidas;
        
        internal System.Windows.Controls.DescriptionViewer ttpOpMalSucess;
        
        internal System.Windows.Controls.TextBlock lblQteTrades;
        
        internal System.Windows.Controls.DescriptionViewer ttpQtdTrades;
        
        internal System.Windows.Controls.TextBlock lblPosicaoFinal;
        
        internal System.Windows.Controls.DescriptionViewer ttpPosFinal;
        
        internal System.Windows.Controls.TextBlock lblSaldoInicial;
        
        internal System.Windows.Controls.DescriptionViewer ttpSaldoInicial;
        
        internal System.Windows.Controls.TextBlock lblVolumeMaxExposicao;
        
        internal System.Windows.Controls.DescriptionViewer ttpVolumeMaxExposicao;
        
        internal System.Windows.Controls.CheckBox chkOperacaoDescobertaPermitida;
        
        internal System.Windows.Controls.TabItem tabItem2;
        
        internal System.Windows.Controls.DataGrid gridOperacoes;
        
        internal System.Windows.Controls.Button btnExportar;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TerminalWeb;component/Dialogs/Backtest/VisualizaResultadoBacktest.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.tabResultado = ((System.Windows.Controls.TabControl)(this.FindName("tabResultado")));
            this.tabItem1 = ((System.Windows.Controls.TabItem)(this.FindName("tabItem1")));
            this.lblNomeTeste = ((System.Windows.Controls.TextBlock)(this.FindName("lblNomeTeste")));
            this.lblResultadoMax = ((System.Windows.Controls.TextBlock)(this.FindName("lblResultadoMax")));
            this.ttpResultMax = ((System.Windows.Controls.DescriptionViewer)(this.FindName("ttpResultMax")));
            this.lblResultadoMin = ((System.Windows.Controls.TextBlock)(this.FindName("lblResultadoMin")));
            this.ttpResultMin = ((System.Windows.Controls.DescriptionViewer)(this.FindName("ttpResultMin")));
            this.lblResultadoMedio = ((System.Windows.Controls.TextBlock)(this.FindName("lblResultadoMedio")));
            this.ttpResultMed = ((System.Windows.Controls.DescriptionViewer)(this.FindName("ttpResultMed")));
            this.lblResultadoFinal = ((System.Windows.Controls.TextBlock)(this.FindName("lblResultadoFinal")));
            this.ttpResultFinal = ((System.Windows.Controls.DescriptionViewer)(this.FindName("ttpResultFinal")));
            this.lblSaldoTotal = ((System.Windows.Controls.TextBlock)(this.FindName("lblSaldoTotal")));
            this.ttpSaldoTotal = ((System.Windows.Controls.DescriptionViewer)(this.FindName("ttpSaldoTotal")));
            this.lblQteGain = ((System.Windows.Controls.TextBlock)(this.FindName("lblQteGain")));
            this.ttpQtdStopGain = ((System.Windows.Controls.DescriptionViewer)(this.FindName("ttpQtdStopGain")));
            this.lblQteLoss = ((System.Windows.Controls.TextBlock)(this.FindName("lblQteLoss")));
            this.ttpQtdStopLoss = ((System.Windows.Controls.DescriptionViewer)(this.FindName("ttpQtdStopLoss")));
            this.lblOpBemSucedidas = ((System.Windows.Controls.TextBlock)(this.FindName("lblOpBemSucedidas")));
            this.ttpOpBemSucess = ((System.Windows.Controls.DescriptionViewer)(this.FindName("ttpOpBemSucess")));
            this.lblOpMalSucedidas = ((System.Windows.Controls.TextBlock)(this.FindName("lblOpMalSucedidas")));
            this.ttpOpMalSucess = ((System.Windows.Controls.DescriptionViewer)(this.FindName("ttpOpMalSucess")));
            this.lblQteTrades = ((System.Windows.Controls.TextBlock)(this.FindName("lblQteTrades")));
            this.ttpQtdTrades = ((System.Windows.Controls.DescriptionViewer)(this.FindName("ttpQtdTrades")));
            this.lblPosicaoFinal = ((System.Windows.Controls.TextBlock)(this.FindName("lblPosicaoFinal")));
            this.ttpPosFinal = ((System.Windows.Controls.DescriptionViewer)(this.FindName("ttpPosFinal")));
            this.lblSaldoInicial = ((System.Windows.Controls.TextBlock)(this.FindName("lblSaldoInicial")));
            this.ttpSaldoInicial = ((System.Windows.Controls.DescriptionViewer)(this.FindName("ttpSaldoInicial")));
            this.lblVolumeMaxExposicao = ((System.Windows.Controls.TextBlock)(this.FindName("lblVolumeMaxExposicao")));
            this.ttpVolumeMaxExposicao = ((System.Windows.Controls.DescriptionViewer)(this.FindName("ttpVolumeMaxExposicao")));
            this.chkOperacaoDescobertaPermitida = ((System.Windows.Controls.CheckBox)(this.FindName("chkOperacaoDescobertaPermitida")));
            this.tabItem2 = ((System.Windows.Controls.TabItem)(this.FindName("tabItem2")));
            this.gridOperacoes = ((System.Windows.Controls.DataGrid)(this.FindName("gridOperacoes")));
            this.btnExportar = ((System.Windows.Controls.Button)(this.FindName("btnExportar")));
        }
    }
}

