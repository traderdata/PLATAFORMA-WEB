﻿#pragma checksum "C:\Backup\Projetos2012\CLIENT\TERMINAL-WEB-2012\SL\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "49731B437203BB1ECC5B2205D625797C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
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
using Traderdata.Client.TerminalWEB.Dialogs;


namespace Traderdata.Client.TerminalWEB {
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.BusyIndicator busyIndicator;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal C1.Silverlight.C1Menu c1Menu1;
        
        internal C1.Silverlight.C1MenuItem mnuGraficos;
        
        internal C1.Silverlight.C1MenuItem mnuNovoGrafico;
        
        internal C1.Silverlight.C1MenuItem mnuSalvarGrafico;
        
        internal C1.Silverlight.C1MenuItem mnuSalvarTemplate;
        
        internal C1.Silverlight.C1MenuItem mnuExcluirTemplate;
        
        internal C1.Silverlight.C1MenuItem mnuAplicarTemplate;
        
        internal C1.Silverlight.C1MenuItem mnuFerramentasAuxiliares;
        
        internal C1.Silverlight.C1MenuItem mnuTelaCheia;
        
        internal C1.Silverlight.C1MenuItem mnuManual;
        
        internal C1.Silverlight.C1Menu c1Menu2;
        
        internal System.Windows.Controls.Grid gridPrincipal;
        
        internal C1.Silverlight.C1TabControl tabPrincipal;
        
        internal System.Windows.Controls.Canvas canvasPrincipal;
        
        internal System.Windows.Controls.GridSplitter grsplSplitter;
        
        internal System.Windows.Controls.Grid gridDireita;
        
        internal System.Windows.Controls.RowDefinition gridBoleta;
        
        internal C1.Silverlight.Toolbar.C1ToolbarStrip c1ToolbarSuperior;
        
        internal C1.Silverlight.Toolbar.C1ToolbarButton tbarSalvarWorkspace;
        
        internal C1.Silverlight.Toolbar.C1ToolbarButton tbarNovoGrafico;
        
        internal System.Windows.Controls.TextBox txtAtivo;
        
        internal C1.Silverlight.Toolbar.C1ToolbarButton tbarRefreshCotacoes;
        
        internal C1.Silverlight.Toolbar.C1ToolbarButton tbarInsertIndicador;
        
        internal C1.Silverlight.C1Menu mnuIndicadores;
        
        internal C1.Silverlight.Toolbar.C1ToolbarDropDown tbarSkin;
        
        internal Traderdata.Client.TerminalWEB.Dialogs.ColorConfiguration configuration;
        
        internal C1.Silverlight.Toolbar.C1ToolbarDropDown tbarConfiguration;
        
        internal Traderdata.Client.TerminalWEB.Dialogs.Configuration configurationGeral;
        
        internal System.Windows.Controls.Button btnColorPicker;
        
        internal System.Windows.Controls.Border borderColorPicker;
        
        internal C1.Silverlight.Extended.C1ColorPicker objectColorPicker;
        
        internal C1.Silverlight.Toolbar.C1ToolbarDropDown tbarStrokeSthickness;
        
        internal Traderdata.Client.TerminalWEB.Dialogs.StrokeThicknessPicker strokeThicknessPicker;
        
        internal C1.Silverlight.Toolbar.C1ToolbarDropDown tbarStrokeType;
        
        internal Traderdata.Client.TerminalWEB.Dialogs.StrokeTypePicker strokeTypePicker;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbar1Minuto;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbar2Minutos;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbar3Minutos;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbar5Minutos;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbar10Minutos;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbar15Minutos;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbar30Minutos;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbar60Minutos;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbar120Minutos;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarDiario;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarSemanal;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarMensal;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarCandle;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarBarra;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarLinha;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarEscalaNormal;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarEscalaSemilog;
        
        internal C1.Silverlight.Toolbar.C1ToolbarButton tbarArrumarJanelaHorizontalmente;
        
        internal C1.Silverlight.Toolbar.C1ToolbarButton tbarArrumarJanelasVeticalmente;
        
        internal C1.Silverlight.Toolbar.C1ToolbarButton tbarTile;
        
        internal C1.Silverlight.Toolbar.C1ToolbarButton tbarManual;
        
        internal System.Windows.Controls.ScrollViewer scrollToolBar;
        
        internal C1.Silverlight.Toolbar.C1ToolbarStrip c1Toolbar;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarSeta;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarCross;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarZoomIn;
        
        internal C1.Silverlight.Toolbar.C1ToolbarButton tbarZoomOut;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarRetaTendencia;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarLinhaHorizontal;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarLinhaVertical;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarElipse;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarRetangulo;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarTexto;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarValorY;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarSuporte;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarResistencia;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarCompra;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarVenda;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarSinal;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarFiboRetracement;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarFiboArcs;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarFiboFan;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarFiboTimezone;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarGannFan;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarErrorChannel;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarSpeedLine;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarTironeLevels;
        
        internal C1.Silverlight.Toolbar.C1ToolbarToggleButton tbarRaffRegression;
        
        internal System.Windows.Controls.Button scrollToolbarUp;
        
        internal System.Windows.Controls.Button scrollToolbarDown;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TerminalWeb;component/MainPage.xaml", System.UriKind.Relative));
            this.busyIndicator = ((System.Windows.Controls.BusyIndicator)(this.FindName("busyIndicator")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.c1Menu1 = ((C1.Silverlight.C1Menu)(this.FindName("c1Menu1")));
            this.mnuGraficos = ((C1.Silverlight.C1MenuItem)(this.FindName("mnuGraficos")));
            this.mnuNovoGrafico = ((C1.Silverlight.C1MenuItem)(this.FindName("mnuNovoGrafico")));
            this.mnuSalvarGrafico = ((C1.Silverlight.C1MenuItem)(this.FindName("mnuSalvarGrafico")));
            this.mnuSalvarTemplate = ((C1.Silverlight.C1MenuItem)(this.FindName("mnuSalvarTemplate")));
            this.mnuExcluirTemplate = ((C1.Silverlight.C1MenuItem)(this.FindName("mnuExcluirTemplate")));
            this.mnuAplicarTemplate = ((C1.Silverlight.C1MenuItem)(this.FindName("mnuAplicarTemplate")));
            this.mnuFerramentasAuxiliares = ((C1.Silverlight.C1MenuItem)(this.FindName("mnuFerramentasAuxiliares")));
            this.mnuTelaCheia = ((C1.Silverlight.C1MenuItem)(this.FindName("mnuTelaCheia")));
            this.mnuManual = ((C1.Silverlight.C1MenuItem)(this.FindName("mnuManual")));
            this.c1Menu2 = ((C1.Silverlight.C1Menu)(this.FindName("c1Menu2")));
            this.gridPrincipal = ((System.Windows.Controls.Grid)(this.FindName("gridPrincipal")));
            this.tabPrincipal = ((C1.Silverlight.C1TabControl)(this.FindName("tabPrincipal")));
            this.canvasPrincipal = ((System.Windows.Controls.Canvas)(this.FindName("canvasPrincipal")));
            this.grsplSplitter = ((System.Windows.Controls.GridSplitter)(this.FindName("grsplSplitter")));
            this.gridDireita = ((System.Windows.Controls.Grid)(this.FindName("gridDireita")));
            this.gridBoleta = ((System.Windows.Controls.RowDefinition)(this.FindName("gridBoleta")));
            this.c1ToolbarSuperior = ((C1.Silverlight.Toolbar.C1ToolbarStrip)(this.FindName("c1ToolbarSuperior")));
            this.tbarSalvarWorkspace = ((C1.Silverlight.Toolbar.C1ToolbarButton)(this.FindName("tbarSalvarWorkspace")));
            this.tbarNovoGrafico = ((C1.Silverlight.Toolbar.C1ToolbarButton)(this.FindName("tbarNovoGrafico")));
            this.txtAtivo = ((System.Windows.Controls.TextBox)(this.FindName("txtAtivo")));
            this.tbarRefreshCotacoes = ((C1.Silverlight.Toolbar.C1ToolbarButton)(this.FindName("tbarRefreshCotacoes")));
            this.tbarInsertIndicador = ((C1.Silverlight.Toolbar.C1ToolbarButton)(this.FindName("tbarInsertIndicador")));
            this.mnuIndicadores = ((C1.Silverlight.C1Menu)(this.FindName("mnuIndicadores")));
            this.tbarSkin = ((C1.Silverlight.Toolbar.C1ToolbarDropDown)(this.FindName("tbarSkin")));
            this.configuration = ((Traderdata.Client.TerminalWEB.Dialogs.ColorConfiguration)(this.FindName("configuration")));
            this.tbarConfiguration = ((C1.Silverlight.Toolbar.C1ToolbarDropDown)(this.FindName("tbarConfiguration")));
            this.configurationGeral = ((Traderdata.Client.TerminalWEB.Dialogs.Configuration)(this.FindName("configurationGeral")));
            this.btnColorPicker = ((System.Windows.Controls.Button)(this.FindName("btnColorPicker")));
            this.borderColorPicker = ((System.Windows.Controls.Border)(this.FindName("borderColorPicker")));
            this.objectColorPicker = ((C1.Silverlight.Extended.C1ColorPicker)(this.FindName("objectColorPicker")));
            this.tbarStrokeSthickness = ((C1.Silverlight.Toolbar.C1ToolbarDropDown)(this.FindName("tbarStrokeSthickness")));
            this.strokeThicknessPicker = ((Traderdata.Client.TerminalWEB.Dialogs.StrokeThicknessPicker)(this.FindName("strokeThicknessPicker")));
            this.tbarStrokeType = ((C1.Silverlight.Toolbar.C1ToolbarDropDown)(this.FindName("tbarStrokeType")));
            this.strokeTypePicker = ((Traderdata.Client.TerminalWEB.Dialogs.StrokeTypePicker)(this.FindName("strokeTypePicker")));
            this.tbar1Minuto = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbar1Minuto")));
            this.tbar2Minutos = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbar2Minutos")));
            this.tbar3Minutos = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbar3Minutos")));
            this.tbar5Minutos = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbar5Minutos")));
            this.tbar10Minutos = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbar10Minutos")));
            this.tbar15Minutos = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbar15Minutos")));
            this.tbar30Minutos = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbar30Minutos")));
            this.tbar60Minutos = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbar60Minutos")));
            this.tbar120Minutos = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbar120Minutos")));
            this.tbarDiario = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarDiario")));
            this.tbarSemanal = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarSemanal")));
            this.tbarMensal = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarMensal")));
            this.tbarCandle = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarCandle")));
            this.tbarBarra = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarBarra")));
            this.tbarLinha = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarLinha")));
            this.tbarEscalaNormal = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarEscalaNormal")));
            this.tbarEscalaSemilog = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarEscalaSemilog")));
            this.tbarArrumarJanelaHorizontalmente = ((C1.Silverlight.Toolbar.C1ToolbarButton)(this.FindName("tbarArrumarJanelaHorizontalmente")));
            this.tbarArrumarJanelasVeticalmente = ((C1.Silverlight.Toolbar.C1ToolbarButton)(this.FindName("tbarArrumarJanelasVeticalmente")));
            this.tbarTile = ((C1.Silverlight.Toolbar.C1ToolbarButton)(this.FindName("tbarTile")));
            this.tbarManual = ((C1.Silverlight.Toolbar.C1ToolbarButton)(this.FindName("tbarManual")));
            this.scrollToolBar = ((System.Windows.Controls.ScrollViewer)(this.FindName("scrollToolBar")));
            this.c1Toolbar = ((C1.Silverlight.Toolbar.C1ToolbarStrip)(this.FindName("c1Toolbar")));
            this.tbarSeta = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarSeta")));
            this.tbarCross = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarCross")));
            this.tbarZoomIn = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarZoomIn")));
            this.tbarZoomOut = ((C1.Silverlight.Toolbar.C1ToolbarButton)(this.FindName("tbarZoomOut")));
            this.tbarRetaTendencia = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarRetaTendencia")));
            this.tbarLinhaHorizontal = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarLinhaHorizontal")));
            this.tbarLinhaVertical = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarLinhaVertical")));
            this.tbarElipse = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarElipse")));
            this.tbarRetangulo = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarRetangulo")));
            this.tbarTexto = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarTexto")));
            this.tbarValorY = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarValorY")));
            this.tbarSuporte = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarSuporte")));
            this.tbarResistencia = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarResistencia")));
            this.tbarCompra = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarCompra")));
            this.tbarVenda = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarVenda")));
            this.tbarSinal = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarSinal")));
            this.tbarFiboRetracement = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarFiboRetracement")));
            this.tbarFiboArcs = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarFiboArcs")));
            this.tbarFiboFan = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarFiboFan")));
            this.tbarFiboTimezone = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarFiboTimezone")));
            this.tbarGannFan = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarGannFan")));
            this.tbarErrorChannel = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarErrorChannel")));
            this.tbarSpeedLine = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarSpeedLine")));
            this.tbarTironeLevels = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarTironeLevels")));
            this.tbarRaffRegression = ((C1.Silverlight.Toolbar.C1ToolbarToggleButton)(this.FindName("tbarRaffRegression")));
            this.scrollToolbarUp = ((System.Windows.Controls.Button)(this.FindName("scrollToolbarUp")));
            this.scrollToolbarDown = ((System.Windows.Controls.Button)(this.FindName("scrollToolbarDown")));
        }
    }
}

