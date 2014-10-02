using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Traderdata.Client.TerminalWEB.DTO;

namespace Traderdata.Client.TerminalWEB.Dialogs.Backtest
{
    /// <summary>
    /// Child window para adicionar ou editar um backtest.
    /// </summary>
    public partial class AdicionarEditarTesteUI : ChildWindow
    {
        #region Campos e Construtores

        public enum AcaoBackTestEnum { Adicionar, Editar }

        private TerminalWebSVC.TerminalWebClient backTestWS;
        private AcaoBackTestEnum acaoBackTest;
        private List<TerminalWebSVC.CondicaoDTO> condicoesEntrada = new List<TerminalWebSVC.CondicaoDTO>();
        private List<TerminalWebSVC.CondicaoDTO> condicoesSaida = new List<TerminalWebSVC.CondicaoDTO>();
        private int quantidadePaineis;

        /// <summary>
        /// Construtor para inclusão
        /// </summary>
        public AdicionarEditarTesteUI()
        {
            this.acaoBackTest = AcaoBackTestEnum.Adicionar;

            InitializeComponent();
            IniciaServico();

            dtptDataAte.SelectedDate = DateTime.Today;
            dtptDataDe.SelectedDate = DateTime.Today;
        }

        /// <summary>
        /// Construtor para edição.
        /// </summary>
        /// <param name="backTestEdicao"></param>
        public AdicionarEditarTesteUI(TerminalWebSVC.BacktestDTO backTestEdicao)
        {
            this.acaoBackTest = AcaoBackTestEnum.Editar;
            this.BackTestEdicao = backTestEdicao;

            InitializeComponent();
            IniciaServico();

            PreencheCamposEdicao(backTestEdicao);
        }

        #endregion Campos e Construtores

        #region Propriedades

        /// <summary>Backtest criado/editado.</summary>
        public TerminalWebSVC.BacktestDTO BackTestCriadoEditado { get; set; }

        /// <summary>Guarda o backtest de edição. Após ser salvo, usar a propriedade BackTestSalvo.</summary>
        private TerminalWebSVC.BacktestDTO BackTestEdicao { get; set; }

        /// <summary>Ação que está sendo executada: Criação ou Edição</summary>
        public AcaoBackTestEnum AcaoBackTest { get { return acaoBackTest; } }

        #endregion Propriedades

        #region Eventos IU

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.BackTestEdicao != null)
                this.Title = "Edição: " + this.BackTestEdicao.Nome;

            //ldgLoadingControl.StartLoading();
            backTestWS.RetornaTemplatesBacktestAsync(StaticData.User);
        }

        /// <summary>
        /// Evento que dispara ao pressionamento de uma tecla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Util.GeneralUtil.ValidaNumeroDecimal(sender as TextBox, e);
        }

        /// <summary>
        /// Salva o backtest.
        /// </summary>
        private void SalvarButton_Click(object sender, RoutedEventArgs e)
        {
            if (VerificaPreenchimento())
            {
                switch (acaoBackTest)
                {
                    case AcaoBackTestEnum.Adicionar:
                        TerminalWebSVC.BacktestDTO backTest = new TerminalWebSVC.BacktestDTO();
                        AtualizaBackTestDTO(backTest);



                        if (VerificaCondicoes())
                        {
                            BackTestCriadoEditado = backTest;
                            BackTestCriadoEditado.Usuario = StaticData.User;
                            busyIndicator.BusyContent = "Processando Teste...";
                            busyIndicator.IsBusy = true;
                            //enviando para os servidores
                            backTestWS.IncluirBackTestAsync(BackTestCriadoEditado, BackTestCriadoEditado);
                        }
                        else
                            MessageBox.Show("Insira pelo menos uma condição de entrada.");
                        break;
                    case AcaoBackTestEnum.Editar:
                        AtualizaBackTestDTO(BackTestEdicao);

                        if (VerificaCondicoes())
                        {
                            BackTestCriadoEditado = BackTestEdicao;
                            BackTestCriadoEditado.Usuario = StaticData.User;
                            busyIndicator.BusyContent = "Processando Teste...";
                            busyIndicator.IsBusy = true;
                            //enviando para os servidores
                            backTestWS.IncluirBackTestAsync(BackTestCriadoEditado, BackTestCriadoEditado);
                            
                        }
                        else
                            MessageBox.Show("Insira pelo menos uma condição de entrada.");
                        break;
                }
            }

            
        }

        /// <summary>
        /// Cancela a criação ou edição do backtest.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Habilita campos quando o usuário clica em um checkBox.
        /// </summary>
        private void chkHabilitaCampos_CheckChanged(object sender, RoutedEventArgs e)
        {
            HabilitaCamposCheckBox();
        }

        /// <summary>
        /// Abre a tela de condições para adicionar uma condição de entrada.
        /// </summary>
        private void btnAdicionarCondicaoEntrada_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                Condicao condicaoUI = new Condicao();
                condicaoUI.Show();

                condicaoUI.Closing += (sender1, e1) =>
                {
                    if ((condicaoUI.DialogResult == true) && (condicaoUI.CondicaoSelecionada.ListaParcelas != null))
                    {
                        if ((condicaoUI.DialogResult == true) && (condicaoUI.CondicaoSelecionada.ListaParcelas != null))
                        {
                            condicoesEntrada.Add(condicaoUI.CondicaoSelecionada);
                            CriaPainelCondicao(condicaoUI.CondicaoSelecionada);
                        }
                    }
                };
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Preenche campos com o template selecionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTemplate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTemplate.SelectedIndex == 0)
                LimpaCampos();
            else
                PreencheCamposTemplate(cmbTemplate.SelectedItem as TerminalWebSVC.TemplateBacktestDTO);
        }

        /// <summary>
        /// Abre pesquisa de ativo.
        /// </summary>
        private void btnPesquisaAtivo_Click(object sender, RoutedEventArgs e)
        {
            //PesquisaAtivoUI pesqAtivos = new PesquisaAtivoUI(txtAtivo.Text.ToUpper());
            //pesqAtivos.Show();
            //pesqAtivos.Closing += (sender1, e1) =>
            //{
            //    AtivoReduzidoDTO ativo = pesqAtivos.AtivosSelecionados.FirstOrDefault();

            //    if (ativo != null)
            //        txtAtivo.Text = ativo.Codigo.ToUpper();
            //};
        }

        #endregion Eventos IU

        #region Eventos WS

        /// <summary>
        /// Resposta de retorno de templates.
        /// </summary>
        private void backTestWS_RetornaTemplatesBacktestCompleted(object sender, TerminalWebSVC.RetornaTemplatesBacktestCompletedEventArgs e)
        {
            cmbTemplate.SelectionChanged -= cmbTemplate_SelectionChanged;

            cmbTemplate.Items.Clear();
            cmbTemplate.DisplayMemberPath = "Nome";
            cmbTemplate.SelectedValuePath = "Id";

            cmbTemplate.Items.Add(new TerminalWebSVC.TemplateBacktestDTO() { Id = -1, Nome = "-" });

            if (e.Result != null)
                e.Result.ForEach(item => cmbTemplate.Items.Add(item));

            //ldgLoadingControl.StopLoading();

            cmbTemplate.SelectedIndex = 0;

            cmbTemplate.SelectionChanged += cmbTemplate_SelectionChanged;
        }

        /// <summary>
        /// Evento disparado ao se incluir um teste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void backTestWS_IncluirBackTestCompleted(object sender, TerminalWebSVC.IncluirBackTestCompletedEventArgs e)
        {
            busyIndicator.IsBusy = false;
            this.DialogResult = true;
        }

        #endregion Eventos WS

        #region Métodos

        /// <summary>
        /// Inicia serviço de dados.
        /// </summary>
        private void IniciaServico()
        {
            //criando canal de comunicação WCF
            backTestWS = new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());

            //assinando eventos
            backTestWS.RetornaTemplatesBacktestCompleted += backTestWS_RetornaTemplatesBacktestCompleted;
            backTestWS.IncluirBackTestCompleted += backTestWS_IncluirBackTestCompleted;
        }

       

        
        /// <summary>
        /// Habilita campos de checkbox.
        /// </summary>
        private void HabilitaCamposCheckBox()
        {
            txtPrcMontante.IsEnabled = (bool)chkPemitirDescoberto.IsChecked;
            txtCorretagem.IsEnabled = (bool)chkEMolumento.IsChecked;
            txtPercStopGain.IsEnabled = (bool)chkSairStopGain.IsChecked;
            txtPercStopLoss.IsEnabled = (bool)chkSairStopLoss.IsChecked;
        }

        /// <summary>
        /// Cria painel com componentes para uma condição.
        /// </summary>
        private void CriaPainelCondicao(TerminalWebSVC.CondicaoDTO condicaoDTO)
        {
            try
            {
                //Criando painel
                StackPanel stackPanel = new StackPanel();
                stackPanel.Height = 55;
                stackPanel.Orientation = Orientation.Horizontal;

                //Criando borda
                Border borda = new Border();
                borda.BorderBrush = new SolidColorBrush(Colors.Black);
                borda.BorderThickness = new Thickness(1);
                borda.CornerRadius = new CornerRadius(4);
                borda.Child = stackPanel;
                borda.Margin = new Thickness(3);
                borda.Background = Application.Current.Resources["CondicaoBoxBackground"] as LinearGradientBrush;

                //Criando botão para fechar o painel
                Button btn = new Button();
                btn.Content = "X";
                btn.Width = 25;
                btn.VerticalAlignment = VerticalAlignment.Center;
                btn.Margin = new Thickness(10, 0, 0, 0);
                btn.Click += (sender, e) => { stackPanelCondicoes.Children.Remove(borda); condicoesEntrada.Remove(condicaoDTO); quantidadePaineis--; };
                btn.MouseEnter += (sender, e) => { ToolTipService.SetToolTip(btn, "Excluir condição"); };

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
                            switch (obj.TipoFisicoEnumerado)
                            {
                                case TerminalWebSVC.CondicaoParcelaDTO.TipoFisicoEnum.Int:
                                    num.DecimalPlaces = 0;
                                    num.Value = obj.ValorInteiro;
                                    break;

                                case TerminalWebSVC.CondicaoParcelaDTO.TipoFisicoEnum.Double:
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
                            switch (obj.TipoFisicoEnumerado)
                            {
                                case TerminalWebSVC.CondicaoParcelaDTO.TipoFisicoEnum.Int:
                                    txt.Text = obj.ValorInteiro.ToString();
                                    break;

                                case TerminalWebSVC.CondicaoParcelaDTO.TipoFisicoEnum.Double:
                                    txt.Text = obj.ValorDouble.ToString();
                                    break;

                                case TerminalWebSVC.CondicaoParcelaDTO.TipoFisicoEnum.String:
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
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Verifica se o formulario foi preenchido corretamente.
        /// </summary>
        /// <returns></returns>
        private bool VerificaPreenchimento()
        {
            string msgErro = "";

            if (condicoesEntrada.Count == 0)
                msgErro += "- Deve haver pelo menos uma condição.\r\n";

            if (txtNome.Text.Trim() == "")
                msgErro += "- Nome inválido.\r\n";

            bool ativoOk = false;
            foreach (AtivoDTO obj in StaticData.cacheAtivosBovespaTodos)
            {
                if (txtAtivo.Text.Trim().ToUpper() == obj.Codigo)
                {
                    ativoOk = true;
                    break;
                }
            }

            if ((txtAtivo.Text.Trim() == "") || (!ativoOk))
                msgErro += "- Ativo inválido.\r\n";

            if (txtCapitalFinInicial.Text.Trim() == "")
                msgErro += "- Capital Inicial inválido.\r\n";

            if (dtptDataDe.Text.Trim() == "" || dtptDataAte.Text.Trim() == "")
                msgErro += "- Período inválido.\r\n";

            if (chkEMolumento.IsChecked == true)
            {
                if (txtCorretagem.Text.Trim() == "")
                    msgErro += "- Corretagem inválida.\r\n";
            }

            if (chkPemitirDescoberto.IsChecked == true)
            {
                if (txtPrcMontante.Text.Trim() == "")
                    msgErro += "- Exposição Máxima inválida.\r\n";
            }

            if (chkSairStopGain.IsChecked == true)
            {
                if (txtPercStopGain.Text.Trim() == "")
                    msgErro += "- Percentual Stop Gain inválido.\r\n";
            }

            if (chkSairStopLoss.IsChecked == true)
            {
                if (txtPercStopLoss.Text.Trim() == "")
                    msgErro += "- Percentual Stop Loss inválido.\r\n";
            }


            DateTime inicio = (DateTime)dtptDataDe.SelectedDate;
            DateTime termino = (DateTime)dtptDataAte.SelectedDate;

            TimeSpan dif = termino.Subtract(inicio);

            switch (cmbPeriodicidade.SelectedIndex)
            {
                //1Minuto
                case 0:
                //2Minutos
                case 1:
                //3Minutos
                case 2:
                    if (dif.TotalDays > 30)
                        msgErro += "- Para a periodicidade desejada, o período máximo permitido é de 1 mês.\r\n";
                    break;

                //5Minutos
                case 3:
                //10Minutos
                case 4:
                    if (dif.TotalDays > 90)
                        msgErro += "- Para a periodicidade desejada, o período máximo permitido é de 3 meses.\r\n";
                    break;

                //15minutos
                case 5:
                //30Minutos
                case 6:
                //60Minutos
                case 7:
                    if (dif.TotalDays > 180)
                        msgErro += "- Para a periodicidade desejada, o período máximo permitido é de 6 meses.\r\n";
                    break;

                //Diario
                case 8:
                //Semanal
                case 9:
                //Mensal
                case 10:
                    if (dif.TotalDays > 3650)
                        msgErro += "- Para a periodicidade desejada, o período máximo permitido é de 10 anos.\r\n";
                    break;
            }


            if (msgErro == "")
                return true;
            else
            {
                msgErro = "Considerações sobre o preenchimento:\r\n\n" + msgErro;

                MessageBox.Show(msgErro, "Preenchimento", MessageBoxButton.OK);

                return false;
            }
        }

        /// <summary>
        /// Atualiza um BackTestDTO com os valores do formulario.
        /// </summary>
        /// <param name="backTest"></param>
        private void AtualizaBackTestDTO(TerminalWebSVC.BacktestDTO backTest)
        {
            backTest.Usuario = StaticData.User;
            
            backTest.Ativo = txtAtivo.Text.ToUpper();
            backTest.CondicoesEntrada = condicoesEntrada;
            backTest.CondicoesSaida = condicoesSaida;
            backTest.TipoPrecoEnumerado = (TerminalWebSVC.BacktestDTO.TipoPrecoEnum)cmbPreco.SelectedIndex;

            DateTime inicio = (DateTime)dtptDataDe.SelectedDate;
            DateTime inicioSpan = Convert.ToDateTime("00:00");
            backTest.DataInicio = new DateTime(inicio.Year, inicio.Month, inicio.Day, inicioSpan.Hour, inicioSpan.Minute, 0);

            DateTime termino = (DateTime)dtptDataAte.SelectedDate;
            DateTime terminoSpan = Convert.ToDateTime("00:00");
            backTest.DataTermino = new DateTime(termino.Year, termino.Month, termino.Day, terminoSpan.Hour, terminoSpan.Minute, 0);

            backTest.LiquidarPosicaoFinalPeriodo = (bool)chkLiquidarPosicaoFimPeriodo.IsChecked;

            backTest.Nome = txtNome.Text;
            backTest.Observacao = txtDescricao.Text;
            backTest.PermitirOperacaoDescoberto = (bool)chkPemitirDescoberto.IsChecked;
            backTest.SairEmStopGain = (bool)chkSairStopGain.IsChecked;
            backTest.SairEmStopLoss = (bool)chkSairStopLoss.IsChecked;
            backTest.ConsiderarCorretagemMaisEmolumento = (bool)chkEMolumento.IsChecked;

            switch (cmbPeriodicidade.SelectedIndex)
            {
                case 0:
                    backTest.PeriodicidadeEnumerado = TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.UmMinuto;
                    break;
                case 1:
                    backTest.PeriodicidadeEnumerado = TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.DoisMinutos;
                    break;
                case 2:
                    backTest.PeriodicidadeEnumerado = TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.TresMinutos;
                    break;
                case 3:
                    backTest.PeriodicidadeEnumerado = TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.CincoMinutos;
                    break;
                case 4:
                    backTest.PeriodicidadeEnumerado = TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.DezMinutos;
                    break;
                case 5:
                    backTest.PeriodicidadeEnumerado = TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.QuinzeMinutos;
                    break;
                case 6:
                    backTest.PeriodicidadeEnumerado = TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.TrintaMinutos;
                    break;
                case 7:
                    backTest.PeriodicidadeEnumerado = TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.SessentaMinutos;
                    break;
                case 8:
                    backTest.PeriodicidadeEnumerado = TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.Diario;
                    break;
                case 9:
                    backTest.PeriodicidadeEnumerado = TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.Semanal;
                    break;
                case 10:
                    backTest.PeriodicidadeEnumerado = TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.Mensal;
                    break;
            }


            if (backTest.SairEmStopGain)
                backTest.PercentualStopGain = Convert.ToDouble(txtPercStopGain.Text);

            if (backTest.SairEmStopLoss)
                backTest.PercentualStopLoss = Convert.ToDouble(txtPercStopLoss.Text);

            if (backTest.ConsiderarCorretagemMaisEmolumento)
                backTest.ValorCorretagem = Convert.ToDouble(txtCorretagem.Text);

            if (backTest.PermitirOperacaoDescoberto)
                backTest.ValorExposicaoMaxima = Convert.ToDouble(txtPrcMontante.Text);

            backTest.VolumeFinanceiroInicial = Convert.ToDouble(txtCapitalFinInicial.Text);

            AtualizaCondicoesEntrada();

            foreach (TerminalWebSVC.CondicaoDTO condicao in condicoesEntrada)
            {
                foreach (TerminalWebSVC.CondicaoParcelaDTO parcela in condicao.ListaParcelas)
                {
                    if ((parcela.TipoFisicoEnumerado == TerminalWebSVC.CondicaoParcelaDTO.TipoFisicoEnum.String) && (parcela.ValorString == null))
                        parcela.ValorString = "";
                }
            }
        }

        /// <summary>
        /// Preenche os campos do formulário com dados do DTO.
        /// </summary>
        /// <param name="backTestEdicao"></param>
        private void PreencheCamposEdicao(TerminalWebSVC.BacktestDTO backTestEdicao)
        {
            txtAtivo.Text = backTestEdicao.Ativo.ToUpper();
            chkEMolumento.IsChecked = backTestEdicao.ConsiderarCorretagemMaisEmolumento;
            dtptDataDe.SelectedDate = backTestEdicao.DataInicio;
            dtptDataAte.SelectedDate = backTestEdicao.DataTermino;
            chkLiquidarPosicaoFimPeriodo.IsChecked = backTestEdicao.LiquidarPosicaoFinalPeriodo;
            txtNome.Text = backTestEdicao.Nome;
            txtDescricao.Text = backTestEdicao.Observacao;
            txtPercStopGain.Text = backTestEdicao.PercentualStopGain.ToString();
            txtPercStopLoss.Text = backTestEdicao.PercentualStopLoss.ToString();
            chkPemitirDescoberto.IsChecked = backTestEdicao.PermitirOperacaoDescoberto;
            chkSairStopGain.IsChecked = backTestEdicao.SairEmStopGain;
            chkSairStopLoss.IsChecked = backTestEdicao.SairEmStopLoss;
            txtCorretagem.Text = backTestEdicao.ValorCorretagem.ToString();
            txtPrcMontante.Text = backTestEdicao.ValorExposicaoMaxima.ToString();
            txtCapitalFinInicial.Text = backTestEdicao.VolumeFinanceiroInicial.ToString();
            cmbPreco.SelectedIndex = (int)backTestEdicao.TipoPreco;

            switch (backTestEdicao.PeriodicidadeEnumerado)
            {
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.UmMinuto:
                    cmbPeriodicidade.SelectedIndex = 0;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.DoisMinutos:
                    cmbPeriodicidade.SelectedIndex = 1;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.TresMinutos:
                    cmbPeriodicidade.SelectedIndex = 2;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.CincoMinutos:
                    cmbPeriodicidade.SelectedIndex = 3;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.DezMinutos:
                    cmbPeriodicidade.SelectedIndex = 4;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.QuinzeMinutos:
                    cmbPeriodicidade.SelectedIndex = 5;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.TrintaMinutos:
                    cmbPeriodicidade.SelectedIndex = 6;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.SessentaMinutos:
                    cmbPeriodicidade.SelectedIndex = 7;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.Diario:
                    cmbPeriodicidade.SelectedIndex = 8;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.Semanal:
                    cmbPeriodicidade.SelectedIndex = 9;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.Mensal:
                    cmbPeriodicidade.SelectedIndex = 10;
                    break;
                default:
                    break;
            }

            TerminalWebSVC.CondicaoDTO[] condicoes = new TerminalWebSVC.CondicaoDTO[backTestEdicao.CondicoesEntrada.Count];
            backTestEdicao.CondicoesEntrada.CopyTo(condicoes);

            condicoesEntrada = condicoes.ToList();

            //Construindo paineis
            backTestEdicao.CondicoesEntrada.ForEach(cond => CriaPainelCondicao(cond));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void AtualizaCondicoesEntrada()
        {
            try
            {
                List<TerminalWebSVC.CondicaoDTO> antigasCondicoes = new List<TerminalWebSVC.CondicaoDTO>();
                List<string> condicoesPreenchidasIncorretamentes = new List<string>();
                int condIndex = 0;
                int condParcelaIndex = 0;

                //Obtendo condicoes antigas antes de obter novos dados
                foreach (TerminalWebSVC.CondicaoDTO obj in condicoesEntrada)
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
                                                    condicoesEntrada[condIndex].ListaParcelas[condParcelaIndex].ValorInteiro = varInt;
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
                                                    condicoesEntrada[condIndex].ListaParcelas[condParcelaIndex].ValorDouble = varDouble;
                                                else if (!condicoesPreenchidasIncorretamentes.Contains(validacao[2]))
                                                    condicoesPreenchidasIncorretamentes.Add(validacao[2]);
                                                break;

                                            case "S":
                                                condicoesEntrada[condIndex].ListaParcelas[condParcelaIndex].ValorString = txt.Text;
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

                //Realizando roolBack de condicoes, se necessario
                //condicoesEntrada = antigasCondicoes;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Verifico se há condições de entrada
        /// </summary>
        /// <returns></returns>
        private bool VerificaCondicoes()
        {
            if (condicoesEntrada.Count != 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Reseta os campos da tela
        /// </summary>
        private void LimpaCampos()
        {
            txtAtivo.Text = "";
            txtCapitalFinInicial.Text = "";
            txtCorretagem.Text = "";
            txtDescricao.Text = "";
            txtNome.Text = "";
            txtPercStopGain.Text = "";
            txtPercStopLoss.Text = "";
            txtPrcMontante.Text = "";
            cmbPeriodicidade.SelectedIndex = 0;
            cmbPreco.SelectedIndex = 0;
            dtptDataAte.Text = "<dd/MM/yyyy>";
            dtptDataDe.Text = "<dd/MM/yyyy>";
            chkEMolumento.IsChecked = false;
            chkLiquidarPosicaoFimPeriodo.IsChecked = false;
            chkPemitirDescoberto.IsChecked = false;
            chkSairStopGain.IsChecked = false;
            chkSairStopLoss.IsChecked = false;

            stackPanelCondicoes.Children.Clear();
            condicoesEntrada.Clear();
        }

        /// <summary>
        /// Preenche os campos do formulário com dados do DTO.
        /// </summary>
        /// <param name="template"></param>
        private void PreencheCamposTemplate(TerminalWebSVC.TemplateBacktestDTO template)
        {
            if (template == null)
                return;

            txtAtivo.Text = template.Ativo.ToUpper();
            chkEMolumento.IsChecked = template.ConsiderarCorretagemMaisEmolumento;
            dtptDataDe.SelectedDate = template.DataInicio;
            dtptDataAte.SelectedDate = template.DataTermino;
            chkLiquidarPosicaoFimPeriodo.IsChecked = template.LiquidarPosicaoFinalPeriodo;
            txtDescricao.Text = template.Observacao;
            txtPercStopGain.Text = template.PercentualStopGain.ToString();
            txtPercStopLoss.Text = template.PercentualStopLoss.ToString();
            chkPemitirDescoberto.IsChecked = template.PermitirOperacaoDescoberto;
            chkSairStopGain.IsChecked = template.SairEmStopGain;
            chkSairStopLoss.IsChecked = template.SairEmStopLoss;
            txtCorretagem.Text = template.ValorCorretagem.ToString();
            txtPrcMontante.Text = template.ValorExposicaoMaxima.ToString();
            txtCapitalFinInicial.Text = template.VolumeFinanceiroInicial.ToString();
            cmbPreco.SelectedIndex = (int)template.TipoPreco;

            switch (template.PeriodicidadeEnumerado)
            {
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.UmMinuto:
                    cmbPeriodicidade.SelectedIndex = 0;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.DoisMinutos:
                    cmbPeriodicidade.SelectedIndex = 1;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.TresMinutos:
                    cmbPeriodicidade.SelectedIndex = 2;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.CincoMinutos:
                    cmbPeriodicidade.SelectedIndex = 3;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.DezMinutos:
                    cmbPeriodicidade.SelectedIndex = 4;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.QuinzeMinutos:
                    cmbPeriodicidade.SelectedIndex = 5;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.TrintaMinutos:
                    cmbPeriodicidade.SelectedIndex = 6;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.SessentaMinutos:
                    cmbPeriodicidade.SelectedIndex = 7;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.Diario:
                    cmbPeriodicidade.SelectedIndex = 8;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.Semanal:
                    cmbPeriodicidade.SelectedIndex = 9;
                    break;
                case TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.Mensal:
                    cmbPeriodicidade.SelectedIndex = 10;
                    break;
                default:
                    break;
            }


            TerminalWebSVC.CondicaoDTO[] condicoes = new TerminalWebSVC.CondicaoDTO[template.CondicoesEntrada.Count];
            template.CondicoesEntrada.CopyTo(condicoes);

            condicoesEntrada = condicoes.ToList();

            stackPanelCondicoes.Children.Clear();

            //Construindo paineis
            template.CondicoesEntrada.ForEach(cond => CriaPainelCondicao(cond));
        }

        #endregion Métodos
    }
}