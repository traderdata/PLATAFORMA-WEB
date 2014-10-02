using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Traderdata.Client.TerminalWEB.Util;

namespace Traderdata.Client.TerminalWEB.Dialogs.Backtest
{
    public partial class VisualizaResultadoBacktest : UserControl
    {
        #region Campos e Construtores

        /// <summary>Guarda o sumário usado na página.</summary>
        private TerminalWebSVC.SumarioDTO sumario;
        private TerminalWebSVC.TerminalWebClient backTestWS;
        private bool intraday;
        
        /// <summary>
        /// Construtor padrão.
        /// </summary>
        /// <param name="backTestDTO"></param>
        /// <param name="sumario"></param>
        public VisualizaResultadoBacktest(TerminalWebSVC.BacktestDTO backTestDTO)
        {
            InitializeComponent();
            
            IniciaServico();

            switch (backTestDTO.Periodicidade)
            {
                case (int)TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.Diario:
                case (int)TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.Semanal:
                case (int)TerminalWebSVC.BacktestDTO.TipoPeriodicidadeEnum.Mensal:
                    intraday = false;
                    break;
                default:
                    intraday = true;
                    break;
            }

            //Setando o formato da hora
            if (intraday)
                (gridOperacoes.Columns[0] as DataGridBoundColumn).Binding.Converter = new DateTimeIntradayConverter();
            else
                (gridOperacoes.Columns[0] as DataGridBoundColumn).Binding.Converter = new DateTimeLongoPrazoConverter();
                        
            this.lblNomeTeste.Text += backTestDTO.Nome;
            
            //resgatando os resultados
            backTestWS.RetornaSumarioResultadoBacktestCompleted += backTestWS_RetornaSumarioResultadoBacktestCompleted;
            backTestWS.RetornaSumarioResultadoBacktestAsync(backTestDTO.Id, backTestDTO);

            
        }

        void backTestWS_RetornaSumarioResultadoBacktestCompleted(object sender, TerminalWebSVC.RetornaSumarioResultadoBacktestCompletedEventArgs e)
        {
            this.sumario = e.Result;
            this.PopulaCampos(e.Result, (TerminalWebSVC.BacktestDTO)e.UserState);
        }

        #endregion Campos e Construtores

        #region Métodos


        /// <summary>
        /// Popula campos de sumario e resultados.
        /// </summary>
        /// <param name="sumario"></param>
        private void PopulaCampos(TerminalWebSVC.SumarioDTO sumario, TerminalWebSVC.BacktestDTO backTest)
        {
            lblOpBemSucedidas.Text = sumario.OpBemSucedidas.ToString("N0");
            lblOpMalSucedidas.Text = sumario.OpMalSucedidas.ToString("N0");
            lblPosicaoFinal.Text = sumario.PosicaoFinal.ToString("N0");
            lblQteGain.Text = sumario.QtdStopGain.ToString("N0");
            lblQteLoss.Text = sumario.QtdStopLoss.ToString("N0");
            lblQteTrades.Text = sumario.QtdTrades.ToString("N0");
            lblResultadoFinal.Text = sumario.ResultadoFinal.ToString("N2");
            lblResultadoMax.Text = sumario.ResultadoMaximo.ToString("N2");
            lblResultadoMedio.Text = sumario.ResultadoMedio.ToString("N2");
            lblResultadoMin.Text = sumario.ResultadoMinimo.ToString("N2");
            lblSaldoTotal.Text = sumario.ResultadoTotal.ToString("N2");

            lblSaldoInicial.Text = backTest.VolumeFinanceiroInicial.ToString("N2");
            lblVolumeMaxExposicao.Text = backTest.ValorExposicaoMaxima.ToString("N2");
            chkOperacaoDescobertaPermitida.IsChecked = backTest.PermitirOperacaoDescoberto;



            gridOperacoes.ItemsSource = sumario.Operacoes;
        }

        /// <summary>
        /// Inicia serviço WS.
        /// </summary>
        private void IniciaServico()
        {
            backTestWS = new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());
            
        }


        #endregion Métodos

        #region Eventos UI


        /// <summary>
        /// Exporta operações para o excel.
        /// </summary>
        private void btnExportar_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog file = new SaveFileDialog();

            file.DefaultExt = "*.csv";
            file.Filter = "Excel Worksheets|*.csv";

            if (file.ShowDialog() == false)
                return;

            using (StreamWriter sw = new StreamWriter(file.OpenFile()))
            {
                sw.WriteLine("Data;Tipo Operacao;Quantidade;Preco;Saldo Parcial;Custodia Parcial; Saldo Total;Rentabilidade Acumulada; Stop Gain Atingido;Stop Loss Atingido");
                sw.Flush();

                foreach (TerminalWebSVC.ResultadoBacktestDTO obj in sumario.Operacoes)
                {
                    StringBuilder excel = new StringBuilder();
                    excel.Append(obj.DataHora.ToString() + ";");
                    if (obj.Operacao == 0)
                        excel.Append("Compra;");
                    else
                        excel.Append("Venda;");
                    excel.Append(obj.Quantidade + ";");
                    excel.Append(obj.Preco + ";");
                    excel.Append(obj.SaldoParcial + ";");
                    excel.Append(obj.CustodiaParcial + ";");
                    excel.Append(obj.SaldoTotal + ";");
                    excel.Append(obj.RentabilidadeAcumulada + ";");
                    if (obj.StopGainAtingido)
                        excel.Append("Sim" + ";");
                    else
                        excel.Append("Nao" + ";");

                    if (obj.StopLossAtingido)
                        excel.Append("Sim");
                    else
                        excel.Append("Nao");

                    sw.WriteLine(excel);
                    sw.Flush();
                }


            }

        }

        #endregion
    }
}
