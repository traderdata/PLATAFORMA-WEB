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
using Traderdata.Client.TerminalWEB.DAO;
using System.Threading;
using Traderdata.Client.TerminalWEB.DTO;
using System.Windows.Data;

namespace Traderdata.Client.TerminalWEB.Dialogs
{
    public partial class BuscaAtivoDialog : ChildWindow
    {
        #region Variaveis Privadas

        private bool IndicesCarregados = false;
        private MarketDataDAO marketDataDAO = new MarketDataDAO();

        #endregion

        #region Construtor

        public BuscaAtivoDialog()
        {
            InitializeComponent();
            
            //assinando eventos
            marketDataDAO.GetAtivosPorIndiceCompleted +=new MarketDataDAO.GetAtivosPorIndiceHandler(marketDataDAO_GetAtivosPorIndiceCompleted);
            marketDataDAO.GetIndicesCompleted+=new MarketDataDAO.GetIndicesHandler(marketDataDAO_GetIndicesCompleted);
            marketDataDAO.GetAtivosBovespaTodosCompleted += new MarketDataDAO.GetAtivosBovespaTodosHandler(marketDataDAO_GetAtivosBovespaTodosCompleted);
            marketDataDAO.GetAtivosBovespaOpcaoCompleted += new MarketDataDAO.GetAtivosBovespaOpcaoHandler(marketDataDAO_GetAtivosBovespaOpcaoCompleted);
            marketDataDAO.GetAtivosBovespaTermoCompleted += new MarketDataDAO.GetAtivosBovespaTermoHandler(marketDataDAO_GetAtivosBovespaTermoCompleted);
            marketDataDAO.GetAtivosBovespaVistaCompleted += new MarketDataDAO.GetAtivosBovespaVistaHandler(marketDataDAO_GetAtivosBovespaVistaCompleted);
            marketDataDAO.GetAtivosBMFTodosCompleted += new MarketDataDAO.GetAtivosBMFTodosHandler(marketDataDAO_GetAtivosBMFTodosCompleted);
            marketDataDAO.GetAtivosBMFMiniContratosCompleted += new MarketDataDAO.GetAtivosBMFMiniContratosHandler(marketDataDAO_GetAtivosBMFMiniContratosCompleted);
            marketDataDAO.GetAtivosBMFPrincipalCheioCompleted += new MarketDataDAO.GetAtivosBMFPrincpalCheioHandler(marketDataDAO_GetAtivosBMFPrincipalCheioCompleted);
            marketDataDAO.GetSegmentosCompleted += new MarketDataDAO.GetSegmentosHandler(marketDataDAO_GetSegmentosCompleted);
            marketDataDAO.GetAtivosPorSegmentoCompleted += new MarketDataDAO.AtivosPorSegmentoHandler(marketDataDAO_GetAtivosPorSegmentoCompleted);

        }

        

        #endregion

        #region Eventos Completed MarketData

        /// <summary>
        /// Evento disparado ao se retornar a lista de segmentos
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_GetSegmentosCompleted(List<string> Result)
        {
            //setando os indices como carregados
           // IndicesCarregados = true;

            //populando a arvore com opções de indices
            treeSegmentos.Items.Clear();
            foreach (string obj in Result)
            {
                C1.Silverlight.C1TreeViewItem itemSegmento = new C1.Silverlight.C1TreeViewItem();
                itemSegmento.Header = obj;
                itemSegmento.Click += new EventHandler<C1.Silverlight.SourcedEventArgs>(itemSegmento_Click);
                treeSegmentos.Items.Add(itemSegmento);
            }
        }

        /// <summary>
        /// Evento disparado ao se receber a lista de ativos para um segmento
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_GetAtivosPorSegmentoCompleted(List<AtivoDTO> Result)
        {
            CarregaDataGrid(Result);
        }


        /// <summary>
        /// Evento disparado ao se retornar a lista de indices
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_GetIndicesCompleted(List<string> Result)
        {
            //setando os indices como carregados
            IndicesCarregados = true;

            //populando a arvore com opções de indices
            treeAtivoPorIndice.Items.Clear();
            foreach (string obj in Result)
            {
                C1.Silverlight.C1TreeViewItem item = new C1.Silverlight.C1TreeViewItem();
                item.Header = obj;
                item.Click += new EventHandler<C1.Silverlight.SourcedEventArgs>(item_Click);
                treeAtivoPorIndice.Items.Add(item);
            }

            List<AtivoDTO> ativos = new List<AtivoDTO>();
            foreach (string obj in Result)
            {
                AtivoDTO ativo = new AtivoDTO();
                ativo.Codigo = obj;
                ativo.Empresa = obj;
                ativos.Add(ativo);
            }
            CarregaDataGrid(ativos);

        }

        /// <summary>
        /// Evento disparado ao se retornar a lista de ativos por indice
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_GetAtivosPorIndiceCompleted(List<AtivoDTO> Result, string Indice)
        {
            CarregaDataGrid(Result);
        }

        /// <summary>
        /// Evento disparado ao se retornar todos os ativos de BMF
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_GetAtivosBMFTodosCompleted(List<AtivoDTO> Result)
        {
            CarregaDataGrid(Result);
        }

        /// <summary>
        /// Evento disparado ao se retornar todos os ativos de bovespa
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_GetAtivosBovespaTodosCompleted(List<AtivoDTO> Result)
        {
            CarregaDataGrid(Result);
        }

        /// <summary>
        /// Evento disparado ao se retornar todos os ativos de bovespa (Vista)
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_GetAtivosBovespaVistaCompleted(List<AtivoDTO> Result)
        {
            CarregaDataGrid(Result);
        }

        /// <summary>
        /// Evento disparado ao se retornar todos os ativos de bovespa (Termo)
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_GetAtivosBovespaTermoCompleted(List<AtivoDTO> Result)
        {
            CarregaDataGrid(Result);
        }

        /// <summary>
        /// Evento disparado ao se retornar todos os ativos de bovespa (Opcao)
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_GetAtivosBovespaOpcaoCompleted(List<AtivoDTO> Result)
        {
            CarregaDataGrid(Result);
        }

        /// <summary>
        /// Evento disparado ao se retornar todos os ativos de BMF Principais cheios
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_GetAtivosBMFPrincipalCheioCompleted(List<AtivoDTO> Result)
        {
            CarregaDataGrid(Result);
        }

        /// <summary>
        /// Evento disparado ao se retornar todos os ativos de BMF Mini contratos
        /// </summary>
        /// <param name="Result"></param>
        void marketDataDAO_GetAtivosBMFMiniContratosCompleted(List<AtivoDTO> Result)
        {
            CarregaDataGrid(Result);
        }

        #endregion
        
        #region Eventos de forms
        /// <summary>
        /// Evento carregado ao abrir o formulario (apos de carregar componentes)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            //carregando as periodicidades
            List<Periodicidade> listaPeridicidade = new List<Periodicidade>();
            ComboBoxItem irem = new ComboBoxItem();
            irem.Content = "Diário";
            irem.Tag = 1440;


            //carregando lista de indices
            marketDataDAO.GetIndicesAsync();

            //carregando a lista de segmentos
            marketDataDAO.GetSegmentosAsync();

        }

        /// <summary>
        /// Evento disparado ao se clicar sobre um segmento especifico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void itemSegmento_Click(object sender, C1.Silverlight.SourcedEventArgs e)
        {
            marketDataDAO.GetAtivosPorSegmentoAsync((string)((C1.Silverlight.C1TreeViewItem)sender).Header);
        }

        /// <summary>
        /// Evento disparado ao se clicar sobre um indice em especifico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_Click(object sender, C1.Silverlight.SourcedEventArgs e)
        {
            marketDataDAO.GetAtivosPorIndiceAsync((string)((C1.Silverlight.C1TreeViewItem)sender).Header);
        }

        /// <summary>
        /// evento disparado ao se clicar no botao Ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {            
            if (_flexGridAtivos.SelectedItems != null)
            if (_flexGridAtivos.SelectedItems.Count > 0)
                this.DialogResult = true;
            else
                MessageBox.Show("Por favor selecione pelo menos 1 ativo");
        }

        /// <summary>
        /// Evento disparado ao se clicar no botao cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Evento disparado ao se clicar sobre os indices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeIndices_Click(object sender, C1.Silverlight.SourcedEventArgs e)
        {
            while (!IndicesCarregados)
            {
                Thread.Sleep(100);
            }

            List<AtivoDTO> ativos = new List<AtivoDTO>();
            foreach (string obj in StaticData.cacheIndices)
            {
                AtivoDTO ativo = new AtivoDTO();
                ativo.Codigo = obj;
                ativo.Empresa = obj;
                ativos.Add(ativo);
            }
            CarregaDataGrid(ativos);
        }

        /// <summary>
        /// Evento que será disparado ao se solicitar todos os ativos bovespa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeTodosBovespa_Click(object sender, C1.Silverlight.SourcedEventArgs e)
        {
            marketDataDAO.GetAtivosBovespaTodosAsync();
        }

        /// <summary>
        /// Evento que será disparado ao se solicitar todos os ativos bovespa (TERMO)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeTermoBovespa_Click(object sender, C1.Silverlight.SourcedEventArgs e)
        {
            marketDataDAO.GetAtivosBovespaTermoAsync();
        }

        /// <summary>
        /// Evento que será disparado ao se solicitar todos os ativos bovespa (OPCAO)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeOpcaoBovespa_Click(object sender, C1.Silverlight.SourcedEventArgs e)
        {
            marketDataDAO.GetAtivosBovespaOpcaoAsync();
        }

        /// <summary>
        /// Evento que será disparado ao se solicitar todos os ativos bovespa (VISTA)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeVistaBovespa_Click(object sender, C1.Silverlight.SourcedEventArgs e)
        {
            marketDataDAO.GetAtivosBovespaVistaAsync();
        }

        /// <summary>
        /// Evento que será disparado ao se solicitar todos os ativos BMF(TODOS)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeTodosBMF_Click(object sender, C1.Silverlight.SourcedEventArgs e)
        {
            marketDataDAO.GetAtivosBMFTodosAsync();
        }

        /// <summary>
        /// Evento que será disparado ao se solicitar todos os ativos BMF(Principais)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treePrincipalBMF_Click(object sender, C1.Silverlight.SourcedEventArgs e)
        {
            marketDataDAO.GetAtivosBMFPrincipaisCheiosAsync();
        }

        /// <summary>
        /// Evento que será disparado ao se solicitar todos os ativos BMF(Mini)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeMiniContratos_Click(object sender, C1.Silverlight.SourcedEventArgs e)
        {
            marketDataDAO.GetAtivosBMFMiniContratosAsync();
        }
        #endregion

        #region Metodos

        /// <summary>
        /// Metodo que carrega o datagrid com o resultado obtido de marketdata
        /// </summary>
        /// <param name="Result"></param>
        private void CarregaDataGrid(List<AtivoDTO> Result)
        {
            var view = new PagedCollectionView(Result);
            _srchCompanies.View = view;
            var props = _srchCompanies.FilterProperties;
            props.Add(typeof(AtivoDTO).GetProperty("Codigo"));
            props.Add(typeof(AtivoDTO).GetProperty("Empresa"));
            _flexGridAtivos.ItemsSource = view;
        }

        #endregion

        
        
    }
}

