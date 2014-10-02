using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Traderdata.Client.TerminalWEB.DTO;

namespace Traderdata.Client.TerminalWEB.Dialogs.Scanner
{
    public partial class CriaEditaScannerDiario : ChildWindow
    {
        #region Campos e Construtores

        /// <summary>
        /// Variavel que armazena os ativos selecionados
        /// </summary>
        private List<AtivoDTO> ativosSelecionados = new List<AtivoDTO>();

        /// <summary>
        /// Variavel que armazena um novo scanner a ser incluido
        /// </summary>
        public TerminalWebSVC.ScannerDTO Scanner = new TerminalWebSVC.ScannerDTO();

        /// <summary>
        /// Variavel de acesso a camada WCF
        /// </summary>
        private TerminalWebSVC.TerminalWebClient terminalWebClient;


        private List<TextBox> listaTextBox = new List<TextBox>();
        

        
        
        private int quantidadePaineis = 0;

        
        
        /// <summary>
        /// Construtor padrão.
        /// </summary>
        /// <param name="scanners">Scanners existentes.</param>
        /// <param name="isMonitorIntraday">True para o monitor intraday e False para o scanner.</param>
        public CriaEditaScannerDiario()
        {
            try
            {
                InitializeComponent();

                //Inicializando o webservice e os eventos assincronos de scanner
                terminalWebClient = new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());
                
                //assinando eventos
                terminalWebClient.SaveScannerCompleted += terminalWebClient_SaveScannerCompleted;
                
                //Iniciando combo de periodicidade
                cmbPeriodicidade.Items.Add("Diário");
                cmbPeriodicidade.Items.Add("Semanal");
                cmbPeriodicidade.Items.Add("Mensal");
                cmbPeriodicidade.SelectedIndex = 0;

                //iniciando o scanner que será inserido
                this.Scanner = new TerminalWebSVC.ScannerDTO();
                this.Scanner.ListaCondicoes = new List<TerminalWebSVC.CondicaoDTO>();
                
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        void terminalWebClient_SaveScannerCompleted(object sender, TerminalWebSVC.SaveScannerCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                    Scanner = e.Result;

                if (e.Cancelled)
                    MessageBox.Show("Não foi possível salvar o monitor, contate um dos administradores do sistema.", "Atenção", MessageBoxButton.OK);
                else if (e.Error != null)
                    MessageBox.Show("Ocorreu o seguinte erro ao salvar o monitor: " + e.Error.ToString(), "Erro", MessageBoxButton.OK);
                else
                {
                    //Encerrando o form caso positivo
                    this.DialogResult = true;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            
        }
        
        #endregion Campos e Construtores


        #region Eventos

        
        #region Eventos Aplicativo Silverlight

        /***********************************************************************************************
        * Evento: Botão Adicionar
        * Descrição: Adiciona uma condição ao monitor.
        ***********************************************************************************************/
        private void btnAdicionarCondicao_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {                
                Condicao cond = new Condicao();
                cond.Closing += (sender1, e1) =>
                {
                    if (cond.DialogResult == true)
                    {
                        if (cond.CondicaoSelecionada.ListaParcelas != null)
                        {
                            CriaPainelCondicao(cond.CondicaoSelecionada);
                        }
                    }   
                };

                cond.Show();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        /***********************************************************************************************
        * Evento: Botão Salvar
        * Descrição: Salva o monitor editado ou criado.
        ***********************************************************************************************/
        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busyIndicator.BusyContent = "Salvando/Processando Rastreador...";
                busyIndicator.IsBusy = true;

                string msgErro = ValidaPreenchimentoCampos();

                if (msgErro != string.Empty)
                    MessageBox.Show(msgErro, "Validação", MessageBoxButton.OK);
                else
                {
                    Scanner.Nome = txtNome.Text;
                    Scanner.Descricao = txtDescricao.Text;
                    Scanner.User = StaticData.User;
                    Scanner.Formula = "";
                    Scanner.ListaAtivos = "";
                    Scanner.PublicarFacebook = false;
                    Scanner.EnviarEmail = false;

                    //Obtendo ativos selecionados
                    foreach (AtivoDTO ativo in StaticData.cacheAtivosPorIndice["IBOV"])
                    {
                        Scanner.ListaAtivos += ativo.Codigo + ";";
                    }

                    //Retirando o ultimo ";"
                    if (ativosSelecionados.Count > 0)
                        Scanner.ListaAtivos = Scanner.ListaAtivos.Remove(Scanner.ListaAtivos.Length - 1);

                    //Obtendo periodicidade
                    switch (cmbPeriodicidade.SelectedIndex)
                    {
                        //Diário
                        case 0:
                            Scanner.Periodicidade = 1440;
                            break;

                        //Semanal
                        case 1:
                            Scanner.Periodicidade = 10080;
                            break;

                        //Mensal
                        case 2:
                            Scanner.Periodicidade = 43200;
                            break;
                    }

                    

                    terminalWebClient.SaveScannerAsync(Scanner);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        /***********************************************************************************************
        * Evento: Botão Cancelar
        * Descrição: Cancela a edição ou criação do monitor.
        ***********************************************************************************************/
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

   
        #endregion Eventos Aplicativo Silverlight

        #endregion Eventos

        #region Métodos

        #region CriaPainelCondicao()
        /// <summary>
		/// Cria painel com componentes para uma condição.
		/// </summary>
		private void CriaPainelCondicao(TerminalWebSVC.CondicaoDTO condicaoDTO)
		{
			try
			{
                //Adicionando condição à lista de condições
                Scanner.ListaCondicoes.Add(condicaoDTO);

				//Criando painel
				StackPanel stackPanel = new StackPanel();
				stackPanel.Height = 55;
				stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Background = ObtemGradienteDuasCores(Color.FromArgb(255, 223, 219, 219), Color.FromArgb(255, 250, 247, 247), 0, 1, new Point(0.5,1), new Point(0.5,0));
								
				//Criando borda
				Border borda = new Border();
				borda.BorderBrush = new SolidColorBrush(Colors.Black);
				borda.BorderThickness = new Thickness(1);
				borda.CornerRadius = new CornerRadius(3);
				borda.Child = stackPanel;
				borda.Margin = new Thickness(3,3,3,3);
				
				//Criando botão para fechar o painel
				Button btn = new Button();
				btn.Content = "X";
				btn.Width = 25;
				btn.VerticalAlignment = VerticalAlignment.Center;
				btn.Margin = new Thickness(10,0,0,0);
                btn.Click += (sender, e) => { stackPanelCondicoes.Children.Remove(borda); Scanner.ListaCondicoes.Remove(condicaoDTO); quantidadePaineis--; };
				btn.MouseEnter += (sender, e) =>{ ToolTipService.SetToolTip(btn, "Excluir condição");};
				
				//Adicionando o botao de fechamento ao painel
				stackPanel.Children.Add(btn);

                //Criando painel de parcelas
                StackPanel stackPanelAux = new StackPanel();
                stackPanel.Children.Add(stackPanelAux);

                //Criando título Condicao
                TextBlock lblTitulo = new TextBlock();
				lblTitulo.Text = "Condição: " + condicaoDTO.Nome;
				
                lblTitulo.Margin = new Thickness(10, 7, 0, 0);
				lblTitulo.Foreground = new SolidColorBrush(Colors.Black);

                stackPanelAux.Children.Add(lblTitulo);

                //Criando stackPanel de parcelas
                StackPanel stackPanelParcelas = new StackPanel();
                stackPanelParcelas.Orientation = Orientation.Horizontal;
                stackPanelParcelas.Tag = "P";

                stackPanelAux.Children.Add(stackPanelParcelas);

				//Criando labels e campos dos parâmetros
                foreach (TerminalWebSVC.CondicaoParcelaDTO obj in condicaoDTO.ListaParcelas)
                {
					//Criando descrição
					TextBlock lbl = new TextBlock();
					lbl.Text = obj.Nome;
					lbl.VerticalAlignment = VerticalAlignment.Center;
                    lbl.Foreground = new SolidColorBrush(Colors.Black);
                    lbl.Margin = new Thickness(10, 4, 0, 0);

                    //Adicionando campo de descrição ao painel
                    stackPanelParcelas.Children.Add(lbl);
					
					//Criando campo de preenchimento
                    switch (obj.TipoApresentacao)
                    {
                        //NumericUpDown
                        case "N":
                            NumericUpDown num = new NumericUpDown();
                            num.VerticalAlignment = VerticalAlignment.Center;
                            num.Margin = new Thickness(10, 4, 0, 0);
                            num.Tag = obj.TipoApresentacao + "\t" + obj.TipoFisico + "\t" + condicaoDTO.Nome;
                            num.Foreground = new SolidColorBrush(Colors.Black);

                            //Adicionando valor ao campo de preenchimento
                            switch (obj.TipoFisico)
                            {
                                case "I":
                                    num.DecimalPlaces = 0;
                                    num.Value = obj.ValorInteiro;
                                    break;

                                case "D":
                                    num.DecimalPlaces = 2;
                                    num.Value = obj.ValorDouble;
                                    break;
                            }

                            //Adicionando campo de preenchimento
                            stackPanelParcelas.Children.Add(num);
                            break;
                        case "C":
                        case "T":
                            TextBox txt = new TextBox();
                            txt.VerticalAlignment = VerticalAlignment.Center;
                            txt.Margin = new Thickness(10, 4, 0, 0);
                            txt.Tag = obj.TipoApresentacao + "\t" + obj.TipoFisico + "\t" + condicaoDTO.Nome;

                            //Adicionando valor ao campo de preenchimento
                            switch (obj.TipoFisico)
                            {
                                case "I":
                                    txt.Text = obj.ValorInteiro.ToString();
                                    break;

                                case "D":
                                    txt.Text = obj.ValorDouble.ToString();
                                    break;

                                case "S":
                                    txt.Text = obj.ValorString.ToString();
                                    break;
                            }

                            //Adicionando campo de preenchimento
                            stackPanelParcelas.Children.Add(txt);
                            break;
                    }
				}
				
				//Adicionando o painel ao painel principal
				stackPanelCondicoes.Children.Add(borda);
								
				//Incrementando variavel de controle de paineis
				quantidadePaineis++;
			}
			catch(Exception exc)
            {
                throw exc;
			}
        }
        #endregion CriaPainelCondicao()

        #region IniciaPainelCondicoes()
        /// <summary>
        /// Cria painel com componentes para uma condição. Usado geralmente quando a tela é aberta para uma edição.
        /// </summary>
        private void IniciaPainelCondicoesScannerSelecionado()
        {
            List<TerminalWebSVC.CondicaoDTO> roolBack = new List<TerminalWebSVC.CondicaoDTO>();

            try
            {
                roolBack = ClonaCondicoesScanner(Scanner.ListaCondicoes);
                Scanner.ListaCondicoes.Clear();

                if (roolBack.Count > 0)
                {
                    foreach (TerminalWebSVC.CondicaoDTO condicaoDTO in roolBack)
                    {
                        CriaPainelCondicao(condicaoDTO);
                    }
                }
                else
                {
                    LimpaPainelCondicoes();
                }
            }
            catch (Exception exc)
            {
                throw exc;
                Scanner.ListaCondicoes = roolBack;
            }
        }
        #endregion IniciaPainelCondicoes()

        #region ValidaPreenchimentoCampos()
        /// <summary>
        /// Valida preenchimento dos campos. Retorna string vazia se o preenchimento estiver correto. Este método também otbém os novos valores para as parcelas.
        /// </summary>
        /// <returns></returns>
        private string ValidaPreenchimentoCampos()
        {
            try
            {
                List<TerminalWebSVC.CondicaoDTO> antigasCondicoes = new List<TerminalWebSVC.CondicaoDTO>();
                List<string> condicoesPreenchidasIncorretamentes = new List<string>();
                string mensagemErro = string.Empty;
                int condIndex = 0;
                int condParcelaIndex = 0;

                //Obtendo condicoes antigas antes de obter novos dados
                foreach (TerminalWebSVC.CondicaoDTO obj in Scanner.ListaCondicoes)
                {
                    TerminalWebSVC.CondicaoDTO nova = new TerminalWebSVC.CondicaoDTO();
                    nova.Comando = obj.Comando;
                    nova.Id = obj.Id;
                    nova.Nome= obj.Nome;
                    nova.ListaParcelas = new List<TerminalWebSVC.CondicaoParcelaDTO>();

                    foreach (TerminalWebSVC.CondicaoParcelaDTO parcela in obj.ListaParcelas)
                    {
                        TerminalWebSVC.CondicaoParcelaDTO novaParcela = new TerminalWebSVC.CondicaoParcelaDTO();
                        novaParcela.Id = parcela.Id;
                        novaParcela.CondicaoId = parcela.CondicaoId;
                        novaParcela.Nome = parcela.Nome;
                        novaParcela.TipoApresentacao = parcela.TipoApresentacao;
                        novaParcela.TipoFisico = parcela.TipoFisico;
                        novaParcela.ValorDouble = parcela.ValorDouble;
                        novaParcela.ValorInteiro = parcela.ValorInteiro;
                        novaParcela.ValorString = parcela.ValorString;

                        nova.ListaParcelas.Add(novaParcela);
                    }

                    antigasCondicoes.Add(nova);
                }


                //Percorrendo os stackPanels
                foreach (object borda in stackPanelCondicoes.Children)
                {
                    //Se for uma borda, devo percorrer seus itens. Obs: Cada borda corresponde a uma condição
                    if ((borda != null) && (borda is Border))
                    {
                        //A borda guarda um stack panel, que guarda um botão e outro stackPanel
                        if ((((Border)borda).Child != null) && (((Border)borda).Child is StackPanel))
                        {
                            StackPanel stackPanel = (StackPanel)((Border)borda).Child;
                            StackPanel stackParcelas = null;

                            //Obtendo StackPanel que guarda as parcelas
                            foreach (object item in stackPanel.Children)
                            {
                                #region Localizando painel que guarda as parcelas

                                if ((item != null) && (item is StackPanel))
                                {
                                    StackPanel stp = (StackPanel)item;

                                    foreach (object aux in stp.Children)
                                    {
                                        if ((aux != null) && (aux is StackPanel))
                                        {
                                            StackPanel stpAux = (StackPanel)aux;

                                            if ((stpAux.Tag != null) && (stpAux.Tag.ToString() == "P"))
                                            {
                                                stackParcelas = stpAux;
                                                break;
                                            }
                                        }
                                    }
                                }
                                #endregion Localizando painel que guarda as parcelas
                            }

                            if (stackParcelas != null)
                            {
                                condParcelaIndex = 0;

                                //Percorrendo os campos de preenchimento do stackPanel. Obs: cada um desses stackPanel corresponde a uma parcela
                                foreach (object item in stackParcelas.Children)
                                {
                                    #region Validando preenchimento e obtendo valores para o DTO

                                    TextBox txt = null;
                                    NumericUpDown num = null;
                                    ComboBox cmb = null;
                                    string[] validacao = new string[3];
                                    string valor = "";

                                    if (item != null)
                                    {
                                        //Obtendo o campo de parametro
                                        if (item is TextBox)
                                        {
                                            txt = (TextBox)item;
                                            validacao = txt.Tag.ToString().Split('\t');
                                        }
                                        else if (item is NumericUpDown)
                                        {
                                            num = (NumericUpDown)item;
                                            validacao = num.Tag.ToString().Split('\t');
                                        }
                                        else if (item is ComboBox)
                                        {
                                            cmb = (ComboBox)item;
                                            validacao = cmb.Tag.ToString().Split('\t');
                                        }

                                        //Realizando a validacao
                                        switch (validacao[1])
                                        {
                                            case "I":
                                                int varInt = 0;

                                                valor = "0";

                                                if (txt != null)
                                                    valor = txt.Text;
                                                else if (num != null)
                                                    valor = num.Value.ToString();

                                                //Retirando casa decimal por conta de bug do componente
                                                if (num.DecimalPlaces == 0)
                                                {
                                                    if (valor.Contains(","))
                                                        valor = valor.Split(',')[0];
                                                    else if (valor.Contains("."))
                                                        valor = valor.Split('.')[0];
                                                }

                                                //Se a conversão for possível devo obter o novo valor no DTO, caso não, devo mostrar na mensagemm
                                                if (Int32.TryParse(valor, out varInt))
                                                    Scanner.ListaCondicoes[condIndex].ListaParcelas[condParcelaIndex].ValorInteiro = varInt;
                                                else if (!condicoesPreenchidasIncorretamentes.Contains(validacao[2]))
                                                    condicoesPreenchidasIncorretamentes.Add(validacao[2]);

                                                break;

                                            case "D":
                                                double varDouble = 0;

                                                if (txt != null)
                                                    valor = txt.Text;
                                                else if (num != null)
                                                    valor = num.Value.ToString();

                                                //Se a conversão for possível devo obter o novo valor no DTO, caso não, devo mostrar na mensagemm
                                                if (Double.TryParse(valor, out varDouble))
                                                    Scanner.ListaCondicoes[condIndex].ListaParcelas[condParcelaIndex].ValorDouble = varDouble;
                                                else if (!condicoesPreenchidasIncorretamentes.Contains(validacao[2]))
                                                    condicoesPreenchidasIncorretamentes.Add(validacao[2]);
                                                break;

                                            case "S":
                                                Scanner.ListaCondicoes[condIndex].ListaParcelas[condParcelaIndex].ValorString = txt.Text;
                                                break;
                                        }

                                        if ((txt != null) || (num != null) || (cmb != null))
                                            condParcelaIndex++;
                                    }

                                    #endregion Validando preenchimento e obtendo valores para o DTO
                                }

                                condIndex++;
                            }
                        }
                    }
                }

                //Montando mensagem de validação
                if (condicoesPreenchidasIncorretamentes.Count > 0)
                {
                    mensagemErro = "As seguintes condições não foram preenchidas corretamente:\n";

                    foreach (string obj in condicoesPreenchidasIncorretamentes)
                    {
                        mensagemErro += "\n- " + obj;
                    }
                }
                else
                {
                    if (txtNome.Text.Trim() == "")
                        mensagemErro = "O scanner deve possuir um nome.";
                    else if (Scanner.ListaCondicoes.Count == 0)
                        mensagemErro = "O scanner deve possuir pelo menos uma condição.";
                    
                }

                //Realizando roolBack de condicoes, se necessario
                if (mensagemErro.Length > 0)
                    Scanner.ListaCondicoes = antigasCondicoes;

                return mensagemErro;
            }
            catch (Exception exc)
            {
                throw exc;
                throw exc;
            }
        }
        #endregion ValidaPreenchimentoCampos()

        #region ObtemGradienteDuasCores()
        /// <summary>
		/// Obtém gradiente de duas cores.
		/// </summary>
		/// <param name="cor1">Primeira cor.</param>
		/// <param name="cor2">Segunda cor.</param>
		/// <param name="offset1">Distância para primeira cor.</param>
		/// <param name="offset2">Distância para segunda cor.</param>
		/// <param name="pontoInicial">Ponto inicial do gradiente.</param>
		/// <param name="pontoFinal">Ponto final do gradiente.</param>
		/// <returns></returns>
		private LinearGradientBrush ObtemGradienteDuasCores(Color cor1, Color cor2, double offset1, double offset2, Point pontoInicial, Point pontoFinal)
		{
			//Criando BackGround para o stackPanel
	    	GradientStop gs1 = new GradientStop();
        	gs1.Color = cor1;
			gs1.Offset = offset1;
			
        	GradientStop gs2 = new GradientStop();
        	gs2.Color = cor2;
			gs2.Offset = offset2;
			
			LinearGradientBrush lgb = new LinearGradientBrush();
			lgb.GradientStops.Add(gs1);
			lgb.GradientStops.Add(gs2);
			lgb.StartPoint = pontoInicial;
			lgb.EndPoint = pontoFinal;	
			
			return lgb;
        }
        #endregion ObtemGradienteDuasCores()

        #region WindowsVistaGradiente()
        /// <summary>
		/// Cria gradiente do Windows Vista.
		/// </summary>
		/// <returns></returns>
		private LinearGradientBrush WindowsVistaGradiente()
		{
			//Criando BackGround para o stackPanel
		    GradientStop gs1 = new GradientStop();
            gs1.Color = Color.FromArgb(155,76,76,76);
			
            GradientStop gs2 = new GradientStop();
            gs2.Color = Color.FromArgb(155,51,53,56);
			gs2.Offset = 1;
			
			GradientStop gs3 = new GradientStop();
            gs3.Color = Color.FromArgb(155,60,61,63);
			gs3.Offset = 0.394;
			
			GradientStop gs4 = new GradientStop();
            gs4.Color = Color.FromArgb(155,21,21,22);
			gs4.Offset = 0.417;

            GradientStopCollection gsc = new GradientStopCollection();
            gsc.Add(gs1);
            gsc.Add(gs2);
			gsc.Add(gs3);
			gsc.Add(gs4);
			
			LinearGradientBrush lgb = new LinearGradientBrush();	
			
			lgb.GradientStops = gsc;
			lgb.StartPoint = new Point(0.5, 0);
			lgb.EndPoint = new Point(0.5, 1);
			
			return lgb;
        }
        
        #endregion WindowsVistaGradiente()
        
        #region LimpaPainelCondicoes()
        /// <summary>
        /// Limpa o painel de condições.
        /// </summary>
        private void LimpaPainelCondicoes()
        {
            stackPanelCondicoes.Children.Clear();
        }
        #endregion LimpaPainelCondicoes()

        #region ClonaCondicoesScanner()
        /// <summary>
        /// Obtem cópia não referenciada da lista de condicoes.
        /// </summary>
        /// <returns></returns>
        private List<TerminalWebSVC.CondicaoDTO> ClonaCondicoesScanner(List<TerminalWebSVC.CondicaoDTO> condicoesASeremClonadas)
        {
            try
            {
                List<TerminalWebSVC.CondicaoDTO> condicoes = new List<TerminalWebSVC.CondicaoDTO>();

                //Obtendo condicoes antigas antes de obter novos dados
                foreach (TerminalWebSVC.CondicaoDTO obj in condicoesASeremClonadas)
                {
                    TerminalWebSVC.CondicaoDTO nova = new TerminalWebSVC.CondicaoDTO();
                    nova.Comando = obj.Comando;
                    nova.Id = obj.Id;
                    nova.Nome = obj.Nome;
                    nova.ListaParcelas = new List<TerminalWebSVC.CondicaoParcelaDTO>();

                    foreach (TerminalWebSVC.CondicaoParcelaDTO parcela in obj.ListaParcelas)
                    {
                        TerminalWebSVC.CondicaoParcelaDTO novaParcela = new TerminalWebSVC.CondicaoParcelaDTO();
                        novaParcela.Id = parcela.Id;
                        novaParcela.CondicaoId = parcela.CondicaoId;
                        novaParcela.Nome = parcela.Nome;
                        novaParcela.TipoApresentacao = parcela.TipoApresentacao;
                        novaParcela.TipoFisico = parcela.TipoFisico;
                        novaParcela.ValorDouble = parcela.ValorDouble;
                        novaParcela.ValorInteiro = parcela.ValorInteiro;
                        novaParcela.ValorString = parcela.ValorString;

                        nova.ListaParcelas.Add(novaParcela);
                    }

                    condicoes.Add(nova);
                }

                return condicoes;
            }
            catch (Exception exc)
            {
                throw exc;
                return null;
            }
        }
        #endregion ClonaCondicoesScanner()

              

        #endregion Métodos
    }
}

