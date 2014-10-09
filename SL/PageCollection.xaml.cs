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
using Traderdata.Client.TerminalWEB.DTO;
using C1.Silverlight;
using System.Windows.Media.Imaging;
using Traderdata.Client.TerminalWEB.Util;



namespace Traderdata.Client.TerminalWEB
{
    public partial class PageCollection : UserControl
    {
        #region Variaveis Privadas

        /// <summary>
        /// Variavel de controle de carregamento
        /// </summary>
        private bool Carregado = false;

        /// <summary>
        /// Variavel de acesso aos webservices
        /// </summary>
        private TerminalWebSVC.TerminalWebClient terminalWebClient =
            new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());

        /// <summary>
        /// Variavel de acesso ao marketdata
        /// </summary>
        private MarketDataDAO marketDataDAO = new MarketDataDAO();

        /// <summary>
        /// Variavel local que armazena os layouts
        /// </summary>
        private List<TerminalWebSVC.LayoutDTO> Layouts = new List<TerminalWebSVC.LayoutDTO>();

        /// <summary>
        /// Variavel que armazena o graficoDTO
        /// </summary>
        private TerminalWebSVC.GraficoDTO graficoDTO = new TerminalWebSVC.GraficoDTO();

        /// <summary>
        /// variavel que armazena o templateDTO
        /// </summary>
        private TerminalWebSVC.TemplateDTO templateDTO = new TerminalWebSVC.TemplateDTO();

        /// <summary>
        /// Variavel que armazena o ativo sendo aberto
        /// </summary>
        public string Ativo = "";

        /// <summary>
        /// Variavel que armazena a periodicidade do gráfico
        /// </summary>
        public Periodicidade Periodicidade = Periodicidade.Nenhum;


        #endregion

        #region Construtor

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="ativo"></param>
        public PageCollection(string ativo, TerminalWebSVC.TemplateDTO template, Periodicidade periodicidade)
        {
            InitializeComponent();

            //setando a periodicidade
            this.Periodicidade = periodicidade;

            //assinando eventos
            terminalWebClient.SaveGraficoCompleted += terminalWebClient_SaveGraficoCompleted;
                        
            if (template == null)
            {
                template = new TerminalWebSVC.TemplateDTO();                
                List<TerminalWebSVC.LayoutDTO> layout = new List<TerminalWebSVC.LayoutDTO>();
                layout.Add(GeneralUtil.LayoutFake());
                template.Layouts = layout;
                template.Periodicidade = GeneralUtil.GetIntPeriodicidade(this.Periodicidade);
            }


            this.Ativo = ativo;            
            this.templateDTO = template;

            //setando o cache intraday
            //marketDataDAO.SetCacheCotacaoIntradayAsync(ativo);

            //assinando os eventos de interface
            c1TabControl1.SelectionChanged += new SelectionChangedEventHandler(c1TabControl1_SelectionChanged);
            c1TabControl1.TabItemClosed += new EventHandler<C1.Silverlight.SourcedEventArgs>(c1TabControl1_TabItemClosed);

            //assinando os eventos de Realtime Data
            RealTimeDAO.TickReceived += new RealTimeDAO.TickHandler(RealTimeDAO_TickReceived);
            
            RealTimeDAO.StartTickSubscription(ativo);
            
            
        }

        

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="ativo"></param>
        public PageCollection(TerminalWebSVC.GraficoDTO grafico)
        {
            InitializeComponent();

            //assinando eventos
            terminalWebClient.SaveGraficoCompleted += terminalWebClient_SaveGraficoCompleted;

            this.templateDTO = null;
            this.Ativo = grafico.Ativo;
            this.Periodicidade = GeneralUtil.GetPeriodicidadeFromInt(grafico.Periodicidade);
            this.graficoDTO = grafico;

            //setando o cache intraday
            //marketDataDAO.SetCacheCotacaoIntradayAsync(grafico.Ativo);
            
            //assinando os eventos de interface
            c1TabControl1.SelectionChanged += new SelectionChangedEventHandler(c1TabControl1_SelectionChanged);
            c1TabControl1.TabItemClosed += new EventHandler<C1.Silverlight.SourcedEventArgs>(c1TabControl1_TabItemClosed);

            //setando a lista de layouts local
            Layouts = grafico.Layouts;

            //assinando os eventos de Realtime Data
            RealTimeDAO.TickReceived += new RealTimeDAO.TickHandler(RealTimeDAO_TickReceived);
            RealTimeDAO.StartTickSubscription(grafico.Ativo);
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Metodo que faz a troca do ativo
        /// </summary>
        /// <param name="ativo"></param>
        public void ChangeAtivo(string ativo)
        {
            this.Ativo = ativo;
            RealTimeDAO.StartTickSubscription(ativo);
            SetHeaderTitle(Ativo + " (" + GeneralUtil.GetPeriodicidadeFromIntToString(GeneralUtil.GetIntPeriodicidade(this.Periodicidade)) + ") ", null);
        }


        /// <summary>
        /// Metodo que altera o header do form
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sourceImagem"></param>
        private void SetHeaderTitle(string message, Uri sourceImagem)
        {
            StackPanel stackHeader = new StackPanel();
            stackHeader.Orientation = Orientation.Horizontal;
            stackHeader.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            stackHeader.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            if (sourceImagem != null)
            {
                Image imagem = new Image();
                imagem.Source = new BitmapImage(sourceImagem);
                imagem.Margin = new Thickness(3, 0, 10, 0);
                stackHeader.Children.Add(imagem);
            }

            TextBlock textTitle = new TextBlock();
            textTitle.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            textTitle.Text = message;
            textTitle.Margin = new Thickness(0, 0, 20, 0);            
            stackHeader.Children.Add(textTitle);

            //Button btnTrades = new Button();
            //btnTrades.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            //btnTrades.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            //btnTrades.Width = 80;
            //btnTrades.Content = "Trades";
            //btnTrades.Click += new RoutedEventHandler(btnTrades_Click);
            //stackHeader.Children.Add(btnTrades);

            //Button btnRastreador = new Button();
            //btnRastreador.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            //btnRastreador.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            //btnRastreador.Width = 80;
            //btnRastreador.Content = "Rastreador";
            //btnRastreador.Click += new RoutedEventHandler(btnRastreador_Click);
            //stackHeader.Children.Add(btnRastreador);

            //alterando o header
        //    ((C1Window)this.Parent).Header = stackHeader;
        }

        /// <summary>
        /// Metodo que retorna um objeto de template
        /// </summary>
        /// <returns></returns>
        public TerminalWebSVC.TemplateDTO GetTemplate()
        {
            List<TerminalWebSVC.LayoutDTO> Layouts = new List<TerminalWebSVC.LayoutDTO>();
            TerminalWebSVC.TemplateDTO templateDTO = new TerminalWebSVC.TemplateDTO();
            
            foreach (C1TabItem tabItem in c1TabControl1.Items)
            {
                if (tabItem.Header != "+")
                {
                    TerminalWebSVC.LayoutDTO layoutDTO = ((Grafico)tabItem.Content).GetLayoutDTOFromStockchart();
                    layoutDTO.TemplateId = templateDTO.Id;
                    Layouts.Add(layoutDTO);
                }
                    
            }

            templateDTO.Layouts = Layouts;
            templateDTO.UsuarioId = StaticData.User.Id;
            templateDTO.Periodicidade = GeneralUtil.GetIntPeriodicidade(Periodicidade);
            return templateDTO;
        }

        /// <summary>
        /// Metodo que retorna os layouts envolvidos no gráficos
        /// </summary>
        /// <returns></returns>
        public List<TerminalWebSVC.LayoutDTO> GetLayouts()
        {
            List<TerminalWebSVC.LayoutDTO> Layouts = new List<TerminalWebSVC.LayoutDTO>();
            int j = 0;

            foreach (C1TabItem tabItem in c1TabControl1.Items)
            {
                if (tabItem.Header != "+")
                {                    
                    try
                    {                        
                        Layouts.Add(((Grafico)tabItem.Content).GetLayoutDTOFromStockchart());

                    }
                    catch
                    {
                        //nesse caso devo pegar o layout da lista que foi carregada inicialmente
                        TerminalWebSVC.LayoutDTO layoutTemp = (TerminalWebSVC.LayoutDTO)tabItem.Tag;
                        if (layoutTemp.Indicadores == null)
                            layoutTemp.Indicadores = new List<TerminalWebSVC.IndicadorDTO>();
                        if (layoutTemp.Objetos == null)
                            layoutTemp.Objetos = new List<TerminalWebSVC.ObjetoEstudoDTO>();

                        Layouts.Add(layoutTemp);
                    }
                    
                }

                Layouts.Reverse();
            }

            return Layouts;
        }

        /// <summary>
        /// Metodo que aplica um template
        /// </summary>
        /// <param name="template"></param>
        public void AplicarTemplate(List<TerminalWebSVC.LayoutDTO> listaLayout, int periodicidade)
        {
            c1TabControl1.Items.Clear();
            this.Periodicidade = GeneralUtil.GetPeriodicidadeFromInt(periodicidade);
            bool layoutum = true;
            c1TabControl1.SelectionChanged -= c1TabControl1_SelectionChanged;
            C1.Silverlight.C1TabItem tabItemMais = new C1.Silverlight.C1TabItem();
            tabItemMais.Header = "+";
            tabItemMais.CanUserClose = false;
            //c1TabControl1.Items.Add(tabItemMais);

            foreach (TerminalWebSVC.LayoutDTO layout in listaLayout)
            {
                Grafico pagina = new Grafico(Ativo, layout, layoutum, this.Periodicidade);
                

                C1.Silverlight.C1TabItem tabItem = new C1.Silverlight.C1TabItem();
                tabItem.Content = pagina;
                tabItem.Tag = layout;
                tabItem.BorderThickness = new Thickness(1);
                tabItem.BorderBrush = new SolidColorBrush(Colors.Gray);
                tabItem.Header = "Layout " + c1TabControl1.Items.Count;
                if (layoutum)
                    tabItem.CanUserClose = false;
                //c1TabControl1.Items.Insert(c1TabControl1.Items.Count - 1, tabItem); //com a retirada do + trocamos pela linha de baixo
                c1TabControl1.Items.Insert(c1TabControl1.Items.Count, tabItem);
                
                layoutum = false;

            }
            c1TabControl1.SelectionChanged += c1TabControl1_SelectionChanged;
        }

        /// <summary>
        /// Metodo que altera a periodicidade
        /// </summary>
        /// <param name="periodicidade"></param>
        public void ChangePeriodicity(Periodicidade periodicidade)
        {
            this.Periodicidade = periodicidade;
            foreach (C1TabItem tabItem in c1TabControl1.Items)
            {
                if ((string)tabItem.Header != "+")
                    ((Grafico)tabItem.Content).SetPeriodicidade(periodicidade, false);
            }
            //setando o header
            SetHeaderTitle(Ativo + " (" + GeneralUtil.GetPeriodicidadeFromIntToString(GeneralUtil.GetIntPeriodicidade(this.Periodicidade)) + ") ", null);
        }

        /// <summary>
        /// Metodo que salva o grafico 
        /// </summary>
        public void SalvarGrafico()
        {
            TerminalWebSVC.GraficoDTO graficoDTO = new TerminalWebSVC.GraficoDTO();
            graficoDTO.Ativo = this.Ativo;
            graficoDTO.Height = 0;
            graficoDTO.Layouts = this.GetLayouts();
            graficoDTO.Left = 0;
            graficoDTO.Periodicidade = GeneralUtil.GetIntPeriodicidade(this.Periodicidade);
            graficoDTO.Top = 0;
            graficoDTO.Width = 0;
            graficoDTO.UsuarioId = StaticData.User.Id;

            //enviando para servidor
            terminalWebClient.SaveGraficoAsync(graficoDTO);            
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Controle do slider do times & trades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void rangeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((C1Window)((StackPanel)((Slider)sender).Parent).Tag).Opacity = e.NewValue;
        }

        /// <summary>
        /// Controle do slider do book
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void rangeSliderBook_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((C1Window)((StackPanel)((Slider)sender).Parent).Tag).Opacity = e.NewValue;
        }

        /// <summary>
        /// Evento recebido apos salvar grafico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_SaveGraficoCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Gráfico de " + this.Ativo + " com periodicidade " + 
                GeneralUtil.GetPeriodicidadeFromIntToString(GeneralUtil.GetIntPeriodicidade(this.Periodicidade)) + " salvo com sucesso.");
        }

        /// <summary>
        /// Evento disparado ao receber novo tick
        /// </summary>
        /// <param name="Result"></param>
        void RealTimeDAO_TickReceived(object Result)
        {
            if (((TickDTO)Result).Ativo == this.Ativo)
            {
                if (((TickDTO)Result).Variacao > 0)
                    SetHeaderTitle(((TickDTO)Result).Ativo + "(" + GeneralUtil.GetPeriodicidadeFromIntToString(GeneralUtil.GetIntPeriodicidade(this.Periodicidade)) + ") [" + ((TickDTO)Result).Hora.Substring(0, 2) + ":" + ((TickDTO)Result).Hora.Substring(2, 2) + "/" + ((TickDTO)Result).Ultimo.ToString("0.00") + "/" +
                        ((TickDTO)Result).Variacao.ToString("0.00") + "%]", new Uri("/TerminalWeb;component/images/buy.png", UriKind.RelativeOrAbsolute));
                else
                    SetHeaderTitle(((TickDTO)Result).Ativo + "(" + GeneralUtil.GetPeriodicidadeFromIntToString(GeneralUtil.GetIntPeriodicidade(this.Periodicidade)) + ") [" + ((TickDTO)Result).Hora.Substring(0, 2) + ":" + ((TickDTO)Result).Hora.Substring(2, 2) + "/" + ((TickDTO)Result).Ultimo.ToString("0.00") + "/" +
                        ((TickDTO)Result).Variacao.ToString("0.00") + "%]", new Uri("/TerminalWeb;component/images/sell.png", UriKind.RelativeOrAbsolute));
            }
        }

        /// <summary>
        /// Evento disparado ao se fechar um item de tabcontrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void c1TabControl1_TabItemClosed(object sender, C1.Silverlight.SourcedEventArgs e)
        {
            if (c1TabControl1.SelectedIndex == c1TabControl1.Items.Count - 1)
                c1TabControl1.SelectedIndex--;

            for (int i = 0; i < c1TabControl1.Items.Count-1; i++)
            {
                ((C1TabItem)c1TabControl1.Items[i]).Header = "Layout " + (i+1).ToString();
            }

        }

        /// <summary>
        /// Evento disparado ao se alterar a seleção do tabcontrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void c1TabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)((C1.Silverlight.C1TabItem)e.AddedItems[0]).Header == "+")
            {
                TerminalWebSVC.LayoutDTO layout = ((Grafico)((C1TabItem)e.RemovedItems[0]).Content).GetLayoutDTOFromStockchart();
                layout.Indicadores = new List<TerminalWebSVC.IndicadorDTO>();
                layout.Objetos = new List<TerminalWebSVC.ObjetoEstudoDTO>();
                layout.Periodicidade = GeneralUtil.GetIntPeriodicidade(this.Periodicidade);
                
                Grafico pagina = new Grafico(Ativo,layout, false, this.Periodicidade);
                    
                C1.Silverlight.C1TabItem tabItem = new C1.Silverlight.C1TabItem();
                tabItem.Content = pagina;
                tabItem.BorderThickness = new Thickness(1);
                tabItem.BorderBrush = new SolidColorBrush(Colors.Gray);
                tabItem.Header = "Layout " + c1TabControl1.Items.Count;
                c1TabControl1.Items.Insert(c1TabControl1.Items.Count - 1, tabItem);

            }
            else
            {
                ((Grafico)((C1TabItem)e.AddedItems[0]).Content).AtualizaCores();
            }
        }

        /// <summary>
        /// Evento executado ao se abrir o form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Carregado)
            {
                Carregado = true;
                if (this.templateDTO != null)
                {
                    //associando ao form pai
                    AplicarTemplate(this.templateDTO.Layouts, this.templateDTO.Periodicidade);
                }
                else
                    AplicarTemplate(this.graficoDTO.Layouts, this.graficoDTO.Periodicidade);

                //setando o header
                SetHeaderTitle(Ativo + " (" + GeneralUtil.GetPeriodicidadeFromIntToString(GeneralUtil.GetIntPeriodicidade(this.Periodicidade)) + ") ", null);

            }
            else
            {
                //atualizar layout
                foreach (C1TabItem tabItem in c1TabControl1.Items)
                {
                    if ((string)tabItem.Header != "+")
                        ((Grafico)tabItem.Content).Refresh();
                }
                //AplicarTemplate(this.graficoDTO.Layouts, GeneralUtil.GetIntPeriodicidade(this.Periodicidade));
            }
        }

        /// <summary>
        /// Evento que ocorre a cada vez qaue o layout é alterado (resize por exmeplo)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_LayoutUpdated_1(object sender, EventArgs e)
        {
            c1TabControl1.Width = LayoutRoot.ActualWidth;
            c1TabControl1.Height = LayoutRoot.ActualHeight;
        }

        #endregion

        private void LayoutRoot_Unloaded(object sender, RoutedEventArgs e)
        {

        }


        
    }
}
