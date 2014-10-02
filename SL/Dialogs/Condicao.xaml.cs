using System;
using System.Windows;
using System.Windows.Controls;
using System.ServiceModel;


namespace Traderdata.Client.TerminalWEB.Dialogs
{
    public partial class Condicao : ChildWindow
    {
        #region Campos e Construtores

        public TerminalWebSVC.CondicaoDTO CondicaoSelecionada = null;
        private TerminalWebSVC.TerminalWebClient baseService;
		private TerminalWebSVC.CondicaoDTO[] lista;
        private EndpointAddress endpoint;


        public Condicao()
        {
            InitializeComponent();

            //Inicializando o webservice
            baseService = new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());
            baseService.GetCondicoesCompleted += baseService_GetCondicoesCompleted;
            baseService.GetParcelaByCondicaoCompleted += baseService_GetParcelaByCondicaoCompleted;

        }

        
        #endregion Campos e Construtores

        #region Eventos

        /*******************************************************************************************************************
        * Evento: Load
        * Descricao: Inicia o recbimento assincrono das condicoes.
        *******************************************************************************************************************/
        private void Condicoes_Loaded(object sender, RoutedEventArgs e)
        {
            //Executando o metodo de forma assincrona
            baseService.GetCondicoesAsync();
        }

        /// <summary>
        /// Evento disparado após se carregar as condições
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void baseService_GetCondicoesCompleted(object sender, TerminalWebSVC.GetCondicoesCompletedEventArgs e)
        {
            try
            {
                gridCondicoes.ItemsSource = null;
                gridCondicoes.ItemsSource = e.Result;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

                

        /// <summary>
        /// Evento disparado ao se trocar o item selecionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridCondicoes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            baseService.GetParcelaByCondicaoAsync(((TerminalWebSVC.CondicaoDTO)gridCondicoes.SelectedItem).Id);
        }


        /// <summary>
        /// Evento disparado ao se carregar as parcelas de uma condicao
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void baseService_GetParcelaByCondicaoCompleted(object sender, TerminalWebSVC.GetParcelaByCondicaoCompletedEventArgs e)
        {
            try
            {
                //Obtendo a condição selecionada
                CondicaoSelecionada = (TerminalWebSVC.CondicaoDTO)gridCondicoes.SelectedItem;

                CondicaoSelecionada.ListaParcelas = e.Result;

                this.DialogResult = true;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        #endregion Eventos


        
    }
}
