﻿#pragma checksum "C:\Backup\Projetos2012\CLIENT\TERMINAL-WEB-2012\SL\Dialogs\Scanner\ConfiguracaoScanner.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DAAD53839E196EC24C9DDB8C67905FD6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
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
    
    
    public partial class ConfiguracaoScanner : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtNome;
        
        internal System.Windows.Controls.ComboBox cmbPeriodicidade;
        
        internal System.Windows.Controls.CheckBox chkEnviarPorEmail;
        
        internal System.Windows.Controls.CheckBox chkPublicarFacebook;
        
        internal System.Windows.Controls.TextBlock btnAdicionarCondicao;
        
        internal System.Windows.Controls.StackPanel stackPanelCondicoes;
        
        internal System.Windows.Controls.DataGrid gridAtivos;
        
        internal System.Windows.Controls.Button btnIncluirAtivo;
        
        internal System.Windows.Controls.Button btnIncluirTodosAtivos;
        
        internal System.Windows.Controls.Button btnExcluirAtivo;
        
        internal System.Windows.Controls.Button btnExcluirTodosAtivo;
        
        internal System.Windows.Controls.DataGrid gridTodosAtivos;
        
        internal System.Windows.Controls.Button btnSalvar;
        
        internal System.Windows.Controls.Button btnCancelar;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/TerminalWeb;component/Dialogs/Scanner/ConfiguracaoScanner.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtNome = ((System.Windows.Controls.TextBox)(this.FindName("txtNome")));
            this.cmbPeriodicidade = ((System.Windows.Controls.ComboBox)(this.FindName("cmbPeriodicidade")));
            this.chkEnviarPorEmail = ((System.Windows.Controls.CheckBox)(this.FindName("chkEnviarPorEmail")));
            this.chkPublicarFacebook = ((System.Windows.Controls.CheckBox)(this.FindName("chkPublicarFacebook")));
            this.btnAdicionarCondicao = ((System.Windows.Controls.TextBlock)(this.FindName("btnAdicionarCondicao")));
            this.stackPanelCondicoes = ((System.Windows.Controls.StackPanel)(this.FindName("stackPanelCondicoes")));
            this.gridAtivos = ((System.Windows.Controls.DataGrid)(this.FindName("gridAtivos")));
            this.btnIncluirAtivo = ((System.Windows.Controls.Button)(this.FindName("btnIncluirAtivo")));
            this.btnIncluirTodosAtivos = ((System.Windows.Controls.Button)(this.FindName("btnIncluirTodosAtivos")));
            this.btnExcluirAtivo = ((System.Windows.Controls.Button)(this.FindName("btnExcluirAtivo")));
            this.btnExcluirTodosAtivo = ((System.Windows.Controls.Button)(this.FindName("btnExcluirTodosAtivo")));
            this.gridTodosAtivos = ((System.Windows.Controls.DataGrid)(this.FindName("gridTodosAtivos")));
            this.btnSalvar = ((System.Windows.Controls.Button)(this.FindName("btnSalvar")));
            this.btnCancelar = ((System.Windows.Controls.Button)(this.FindName("btnCancelar")));
        }
    }
}

