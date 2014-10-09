using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;
using Traderdata.Server.App.TerminalWeb.DAO;
using Traderdata.Server.Core.DTO;
using Traderdata.Server.Core.BusinessManager;

namespace Traderdata.Server.App.TerminalWeb.BusinessManager
{
    public class BackTestBM:BaseBM
    {
        /// <summary>
        /// Percentual de emolumentos
        /// </summary>
        private const double percentualEmolumentos = 0.0035;
        
        /// <summary>
        /// Variavel de calculo
        /// </summary>
        private CalculosEstatisticosBacktestBM calculosBM = new CalculosEstatisticosBacktestBM();

        #region Constructor
 
        //construtor basico que nao ira implementar o construtor do pai
        public BackTestBM(bool leitura, bool escrita, string nomeServico)
            : base(leitura, escrita, nomeServico)
        {
        }

        #endregion

        #region Metodos Read


        /// <summary>
        /// Retorna os testes de um usuário.
        /// </summary>
        /// <param name="cliente">Cliente desejado.</param>
        /// <param name="macroCliente">Macro cliente ao qual o cliente pertence.</param>
        /// <returns></returns>
        public List<BacktestDTO> RetornaTodosBacktestPorCliente(UsuarioDTO user)
        {
            try
            {
                using (BacktestDAO backTestDAO = new BacktestDAO(readConnection, writeConnection))
                {
                    return backTestDAO.RetornaTodosPorCliente(user);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Retorna um teste especiico
        /// </summary>
        /// <param name="id">codigo do teste desejado.</param>
        /// <returns></returns>
        public BacktestDTO RetornaBackTestPorId(int id)
        {
            try
            {
                using (BacktestDAO backTestDAO = new BacktestDAO(readConnection, writeConnection))
                {
                    List<BacktestDTO> listaBacktest = backTestDAO.RetornaBackTestPorId(id);
                    if (listaBacktest.Count > 0)
                        return listaBacktest[0];
                    else
                        return null;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /// <summary>
        /// Retorna o sumario de um teste.
        /// </summary>
        /// <param name="id">Id do teste.</param>
        /// <returns></returns>
        public SumarioDTO RetornaSumarioPorBacktesting(int id)
        {
            try
            {
                using (BacktestDAO backTestDAO = new BacktestDAO(readConnection, writeConnection))
                {
                    SumarioDTO sumario = backTestDAO.RetornaSumarioPorBacktesting(id);
                    sumario.Operacoes = (List<ResultadoBacktestDTO>)RetornaOperacoesPorBacktest(id);
                    return sumario;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /// <summary>
        /// Retorna o sumário gerado por um backtest.
        /// </summary>
        /// <param name="idBackTesting">ID do backtest desejado.</param>
        /// <returns></returns>
        public List<ResultadoBacktestDTO> RetornaOperacoesPorBacktest(int idBackTesting)
        {
            try
            {
                using (ResultadoBacktestDAO operacaoDAO = new ResultadoBacktestDAO(readConnection, writeConnection))
                {
                    return operacaoDAO.RetornaTodosPorBackTest(idBackTesting);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /// <summary>
        /// Retorna todos os templates de um usuário.
        /// </summary>
        /// <param name="cliente">Cliente desejado.</param>
        /// <param name="macroCliente">Macro cliente que contém este cliente.</param>
        /// <returns></returns>
        public List<TemplateBacktestDTO> RetornaTemplateBacktestPorCliente(UsuarioDTO user)
        {
            try
            {
                using (TemplateBacktestDAO templateDAO = new TemplateBacktestDAO(readConnection, writeConnection))
                {
                    return templateDAO.RetornaTodosPorCliente(user);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        #endregion

        #region Metodos Write

        /// <summary>
        /// Salva um novo back test, suas parcela-condições e gera os resultados.
        /// </summary>
        /// <param name="BacktestDTO">DTO contendo as informações sobre o teste.</param>
        /// <param name="msmqQueue">Fila do MSMQ para qual será enviada a solicitação de execução.</param>
        /// <returns></returns>
        public BacktestDTO IncluirBacktest(BacktestDTO BacktestDTO)
        {
            try
            {
                List<ResultadoBacktestDTO> listaOperacao = new List<ResultadoBacktestDTO>();

                ////Salvando test
                using (BacktestDAO backtestDAO = new BacktestDAO(readConnection, writeConnection))
                {
                    backtestDAO.InserirBackTest(BacktestDTO);
                }

                ////Atualizando condicoes com o id do backtesting
                BacktestDTO.CondicoesEntrada.ForEach(cond => cond.ListaParcelas.ForEach(parc => parc.IdBackTest = BacktestDTO.Id));
                BacktestDTO.CondicoesSaida.ForEach(cond => cond.ListaParcelas.ForEach(parc => parc.IdBackTest = BacktestDTO.Id));

                ////Salvando condições
                using (CondicaoValorBackTestDAO condValorDAO = new CondicaoValorBackTestDAO(readConnection, writeConnection))
                {
                    condValorDAO.IncluirAPartirCondicoes(BacktestDTO.CondicoesEntrada, BacktestDTO.Id, CondicaoParcelaDTO.TipoCondicaoEnum.Entrada, false);
                    condValorDAO.IncluirAPartirCondicoes(BacktestDTO.CondicoesSaida, BacktestDTO.Id, CondicaoParcelaDTO.TipoCondicaoEnum.Saida, false);
                }

                //executando o backtest
                ExecutaBackTest(ref BacktestDTO, ref listaOperacao);

                ////SETANDO o status
                BacktestDTO.Status = (int)BacktestDTO.StatusEnum.Executado;

                //salvando a lista de operações
                using (ResultadoBacktestDAO operacaoDAO = new ResultadoBacktestDAO(readConnection, writeConnection))
                {
                    foreach (ResultadoBacktestDTO obj in listaOperacao)
                    {
                        obj.IdBackTest = BacktestDTO.Id;
                        operacaoDAO.InserirOperacao(obj);
                    }
                }

                //Salvar o sumario
                AtualizarSumario(BacktestDTO);

                //comitando
                transaction.Commit();

                //retornando
                return BacktestDTO;
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                throw exc;
            }
        }

        /// <summary>
        /// Exclui o backtest desejado, inclusive seus valores de parcela-condicao e seus resultados.
        /// </summary>
        /// <param name="backTest">Backtest a ser deletado.</param>
        public void ExcluirBacktest(BacktestDTO backTest)
        {
            try
            {
                using (BacktestDAO backtestDAO = new BacktestDAO(readConnection, writeConnection))
                {
                    backtestDAO.ExcluirBackTest(backTest);
                }

                transaction.Commit();
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                throw exc;
            }

        }

        /// <summary>
        /// Salva um back test editado, suas parcela-condições e gera os resultados.
        /// </summary>
        /// <param name="BacktestDTO">DTO contendo as informações sobre o teste.</param>
        /// <param name="msmqQueue">Fila do MSMQ para qual será enviada a solicitação de execução.</param>
        public void AtualizaBacktest(BacktestDTO BacktestDTO)
        {
            List<ResultadoBacktestDTO> listaOperacao = new List<ResultadoBacktestDTO>();

            try
            {
                using (BacktestDAO backTestDAO = new BacktestDAO(readConnection, writeConnection))
                {
                    #region excluindo

                    //excluir backtest
                    backTestDAO.ExcluirBackTest(BacktestDTO);

                    #endregion

                    #region incluindo

                    ////Salvando test
                    using (BacktestDAO backtestDAO = new BacktestDAO(readConnection, writeConnection))
                    {
                        backtestDAO.InserirBackTest(BacktestDTO);
                    }

                    ////Atualizando condicoes com o id do backtesting
                    BacktestDTO.CondicoesEntrada.ForEach(cond => cond.ListaParcelas.ForEach(parc => parc.IdBackTest = BacktestDTO.Id));
                    BacktestDTO.CondicoesSaida.ForEach(cond => cond.ListaParcelas.ForEach(parc => parc.IdBackTest = BacktestDTO.Id));

                    ////Salvando condições
                    using (CondicaoValorBackTestDAO condValorDAO = new CondicaoValorBackTestDAO(readConnection, writeConnection))
                    {
                        condValorDAO.IncluirAPartirCondicoes(BacktestDTO.CondicoesEntrada, BacktestDTO.Id, CondicaoParcelaDTO.TipoCondicaoEnum.Entrada, false);
                        condValorDAO.IncluirAPartirCondicoes(BacktestDTO.CondicoesSaida, BacktestDTO.Id, CondicaoParcelaDTO.TipoCondicaoEnum.Saida, false);
                    }

                    //executando o backtest
                    ExecutaBackTest(ref BacktestDTO, ref listaOperacao);

                    ////SETANDO o status
                    BacktestDTO.Status = (int)BacktestDTO.StatusEnum.Executado;

                    //salvando a lista de operações
                    using (ResultadoBacktestDAO operacaoDAO = new ResultadoBacktestDAO(readConnection, writeConnection))
                    {
                        foreach (ResultadoBacktestDTO obj in listaOperacao)
                        {
                            obj.IdBackTest = BacktestDTO.Id;
                            operacaoDAO.InserirOperacao(obj);
                        }
                    }

                    //Salvar o sumario
                    AtualizarSumario(BacktestDTO);

                    #endregion
                }

                transaction.Commit();
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                throw exc;
            }

        }

        /// <summary>
        /// Metodo que salva a lista de operações
        /// </summary>
        /// <param name="listaOperacao"></param>
        private void SalvaListaOperacoes(List<ResultadoBacktestDTO> listaOperacao)
        {
            try
            {
                using (ResultadoBacktestDAO operacaDAO = new ResultadoBacktestDAO(readConnection, writeConnection))
                {
                    foreach (ResultadoBacktestDTO operacao in listaOperacao)
                    {
                        operacaDAO.InserirOperacao(operacao);
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /// <summary>
        /// Metodo que faz a atualização do sumario
        /// </summary>
        /// <param name="backtest"></param>
        private void AtualizarSumario(BacktestDTO backtest)
        {
            try
            {
                using (BacktestDAO backtestDAO = new BacktestDAO(readConnection, writeConnection))
                {
                    backtestDAO.AtualizarSumario(backtest);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        /// <summary>
        /// Inclui um novo template e seus valores-pacela.
        /// </summary>
        /// <param name="templateDTO">DTO do template a ser incluido.</param>
        /// <returns></returns>
        public TemplateBacktestDTO IncluirTemplateBacktest(TemplateBacktestDTO templateDTO)
        {
            try
            {
                //Inserir registro template
                using (TemplateBacktestDAO templateDAO = new TemplateBacktestDAO(readConnection, writeConnection))
                {
                    templateDAO.InserirTemplate(templateDTO);
                }

                //Atualizando condicoes com o id do template
                templateDTO.CondicoesEntrada.ForEach(cond => cond.ListaParcelas.ForEach(parc => parc.IdBackTest = templateDTO.Id));
                templateDTO.CondicoesSaida.ForEach(cond => cond.ListaParcelas.ForEach(parc => parc.IdBackTest = templateDTO.Id));

                //Salvando condições
                using (CondicaoValorBackTestDAO condicaoDAO = new CondicaoValorBackTestDAO(readConnection, writeConnection))
                {
                    condicaoDAO.IncluirAPartirCondicoes(templateDTO.CondicoesEntrada, templateDTO.Id, CondicaoParcelaDTO.TipoCondicaoEnum.Entrada, true);
                    condicaoDAO.IncluirAPartirCondicoes(templateDTO.CondicoesSaida, templateDTO.Id, CondicaoParcelaDTO.TipoCondicaoEnum.Saida, true);
                }

                transaction.Commit();
                return templateDTO;
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                throw exc;
            }
        }

        /// <summary>
        /// Exclui o template desejado e seus valores-pacela.
        /// </summary>
        /// <param name="templateDTO">Template a ser excluido.</param>
        public void ExcluirTemplateBacktest(TemplateBacktestDTO templateDTO)
        {
            try
            {
                //Excluir valor das parcelas das condicao deste template
                using (CondicaoValorBackTestDAO valorCondicaoDAO = new CondicaoValorBackTestDAO(readConnection, writeConnection))
                {
                    valorCondicaoDAO.ExcluirCondicoesValorTemplate(templateDTO.Id);
                }

                //Excluir template
                using (TemplateBacktestDAO templateDAO = new TemplateBacktestDAO(readConnection, writeConnection))
                {
                    templateDAO.ExcluirTemplate(templateDTO);
                }


                transaction.Commit();
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                throw exc;
            }

        }

        /// <summary>
        /// Atualiza o template desejado e seus valores-pacela.
        /// </summary>
        /// <param name="templateDTO">DTO do template a ser atualizado.</param>
        public void AtualizaTemplateBacktest(TemplateBacktestDTO templateDTO)
        {
            try
            {
                #region efetuando a exclusao

                //Excluir valor das parcelas das condicao deste template
                using (CondicaoValorBackTestDAO valorCondicaoDAO = new CondicaoValorBackTestDAO(readConnection, writeConnection))
                {
                    valorCondicaoDAO.ExcluirCondicoesValorTemplate(templateDTO.Id);
                }

                //Excluir template
                using (TemplateBacktestDAO templateDAO = new TemplateBacktestDAO(readConnection, writeConnection))
                {
                    templateDAO.ExcluirTemplate(templateDTO);
                }


                #endregion

                #region efetuando a inclusao

                //Inserir registro template
                using (TemplateBacktestDAO templateDAO = new TemplateBacktestDAO(readConnection, writeConnection))
                {
                    templateDAO.InserirTemplate(templateDTO);
                }

                //Atualizando condicoes com o id do template
                templateDTO.CondicoesEntrada.ForEach(cond => cond.ListaParcelas.ForEach(parc => parc.IdBackTest = templateDTO.Id));
                templateDTO.CondicoesSaida.ForEach(cond => cond.ListaParcelas.ForEach(parc => parc.IdBackTest = templateDTO.Id));

                //Salvando condições
                using (CondicaoValorBackTestDAO condicaoDAO = new CondicaoValorBackTestDAO(readConnection, writeConnection))
                {
                    condicaoDAO.IncluirAPartirCondicoes(templateDTO.CondicoesEntrada, templateDTO.Id, CondicaoParcelaDTO.TipoCondicaoEnum.Entrada, true);
                    condicaoDAO.IncluirAPartirCondicoes(templateDTO.CondicoesSaida, templateDTO.Id, CondicaoParcelaDTO.TipoCondicaoEnum.Saida, true);
                }


                #endregion

                transaction.Commit();
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                throw exc;
            }
        }

        #endregion

        #region Métodos de Calculo de Resultados


        /// <summary>
        /// Executa o teste desejado, gerando os resultados.
        /// </summary>
        /// <param name="backTest">Backtest a ser executado.</param>
        /// <returns></returns>
        public void ExecutaBackTest(ref BacktestDTO backTest, ref List<ResultadoBacktestDTO> listaOperacao)
        {
            try
            {
                //TODO: Checar se devemos ter flag para considerar ou não dados de aftermarket
                //TODO: Checar como pegaremos a bolsa qaue este ativo pertence

                double saldoLiquido = backTest.VolumeFinanceiroInicial;
                double saldoExposicaoDescoberto = backTest.ValorExposicaoMaxima;
                double preco = 0;
                double custodia = 0;
                double ultimoPrecoNegociado = 0;
                AtivoDTO ativoDTO = new AtivoDTO();
                List<CotacaoServerDTO> listaCotacao = new List<CotacaoServerDTO>();
                List<CotacaoServerDTO> listaCotacaoAuxiliar = new List<CotacaoServerDTO>();
                
                double saldoMaximo = saldoLiquido;
                double saldoMinimo = saldoLiquido;
                double saldoMedio = saldoLiquido;
                double saldoTotalAuxiliar = saldoLiquido;
                int qtdStopGain = 0;
                int qtdStopLoss = 0;
                int qtdOpBemsucedida = 0;
                int qtdOpMalSucedida = 0;
                int qtdTrades = 0;

                #region Fase 1
                //Resgatando 1 ativo
                using (AtivoBM ativoBM = new AtivoBM())
                {
                    ativoDTO = ativoBM.RetornaAtivoPorSymbol(backTest.Ativo);

                    if (ativoDTO == null)
                        throw new Exception("Ativo não encontrado");
                }


                //Resgatando as cotacoes
                using (CotacaoBM cotacaoServer = new CotacaoBM())
                {
                    int periodicidade = -1;
                    switch (backTest.PeriodicidadeEnumerado)
                    {
                        //case BacktestDTO.TipoPeriodicidadeEnum.CentoVinteMinutos:
                        //    periodicidade = 120;
                        //    break;
                        case BacktestDTO.TipoPeriodicidadeEnum.CincoMinutos:
                            periodicidade = 5;
                            break;
                        case BacktestDTO.TipoPeriodicidadeEnum.DezMinutos:
                            periodicidade = 10;
                            break;
                        case BacktestDTO.TipoPeriodicidadeEnum.Diario:
                            periodicidade = 1440;
                            break;
                        case BacktestDTO.TipoPeriodicidadeEnum.DoisMinutos:
                            periodicidade = 2;
                            break;
                        case BacktestDTO.TipoPeriodicidadeEnum.Mensal:
                            periodicidade = 43200;
                            break;
                        case BacktestDTO.TipoPeriodicidadeEnum.QuinzeMinutos:
                            periodicidade = 15;
                            break;
                        case BacktestDTO.TipoPeriodicidadeEnum.Semanal:
                            periodicidade = 10080;
                            break;
                        case BacktestDTO.TipoPeriodicidadeEnum.SessentaMinutos:
                            periodicidade = 60;
                            break;
                        case BacktestDTO.TipoPeriodicidadeEnum.TresMinutos:
                            periodicidade = 3;
                            break;
                        case BacktestDTO.TipoPeriodicidadeEnum.TrintaMinutos:
                            periodicidade = 30;
                            break;
                        case BacktestDTO.TipoPeriodicidadeEnum.UmMinuto:
                            periodicidade = 1;
                            break;

                    }

                    listaCotacao = cotacaoServer.GetCotacao(backTest.Ativo, periodicidade, backTest.DataInicio,
                                    backTest.DataTermino, true, false, false, General.DTO.EnumGeral.BolsaEnum.Bovespa);
                }

                #endregion

                #region Fase 2
                ///Perseguir as cotações obtidas verificando pela execução ou não da condição selecionada
                foreach (CotacaoServerDTO objCotacao in listaCotacao)
                {
                    #region Registrando os saldos que vao pro sumario

                    if (saldoLiquido > saldoMaximo)
                        saldoMaximo = saldoLiquido;

                    if (saldoLiquido < saldoMinimo)
                        saldoMinimo = saldoLiquido;

                    if (qtdTrades > 0)
                        saldoMedio = saldoTotalAuxiliar / qtdTrades;
                    else
                        saldoMedio = saldoTotalAuxiliar;

                    #endregion

                    #region Resgatando Preço
                    //setando o preço de acordo com o selecionado pelo cliente
                    preco = 0;
                    switch (backTest.TipoPrecoEnumerado)
                    {
                        case BacktestDTO.TipoPrecoEnum.Abertura:
                            preco = objCotacao.Abertura;
                            break;
                        case BacktestDTO.TipoPrecoEnum.Maximo:
                            preco = objCotacao.Maximo;
                            break;
                        case BacktestDTO.TipoPrecoEnum.Medio:
                            preco = (objCotacao.Maximo / objCotacao.Minimo) / 2;
                            break;
                        case BacktestDTO.TipoPrecoEnum.Minimo:
                            preco = objCotacao.Minimo;
                            break;
                        case BacktestDTO.TipoPrecoEnum.Ultimo:
                            preco = objCotacao.Ultimo;
                            break;
                    }
                    #endregion

                    bool condicaoCompraAceita = false;
                    bool condicaoVendaAceita = false;
                    ResultadoBacktestDTO ultimaOperacao = new ResultadoBacktestDTO();

                    //Setando a ultima operacao vindo da lista de operações
                    if (listaOperacao.Count > 0)
                        ultimaOperacao = listaOperacao[listaOperacao.Count - 1];
                    else
                        ultimaOperacao = null;

                    //montando a lista que deve ser analisada até o momento
                    listaCotacaoAuxiliar.Add(objCotacao);

                    //avaliando o disparo de condição de entrada e reversao
                    if (!backTest.PermitirOperacaoDescoberto)
                    {
                        if (custodia == 0)
                            condicaoCompraAceita = EvaluateFormula(listaCotacaoAuxiliar, backTest.CondicoesEntrada, false);
                        else
                            condicaoVendaAceita = EvaluateFormula(listaCotacaoAuxiliar, backTest.CondicoesEntrada, true);
                    }
                    else
                    {
                        //se eu posso operar descoberto tenho que verificar a condição de compra e a condição de venda
                        condicaoCompraAceita = EvaluateFormula(listaCotacaoAuxiliar, backTest.CondicoesEntrada, false);
                        condicaoVendaAceita = EvaluateFormula(listaCotacaoAuxiliar, backTest.CondicoesEntrada, true);
                    }

                    //dado que a condição foi aceita devemos verificar pelas outras condições se iremos do teste
                    if (condicaoCompraAceita)
                    {
                        //Se o preço de um lote é maior que o saldo líquido, não pode comprar
                        if (preco * ativoDTO.LotePadrao > saldoLiquido)
                            continue;

                        //realizando a compra
                        listaOperacao.Add(RealizaOperacao(backTest, objCotacao.Data, objCotacao.Hora, preco, ativoDTO.LotePadrao, ref saldoLiquido, ref custodia,
                            ResultadoBacktestDTO.OperacaoEnum.Compra, false, false, false, ultimaOperacao, ref qtdOpBemsucedida, ref qtdOpMalSucedida, false));

                        //alterando o saldo total
                        saldoTotalAuxiliar += saldoLiquido;

                        //aumentando a quantidade de trades
                        qtdTrades++;

                        //armazena o ultimo preço negociado
                        ultimoPrecoNegociado = preco;

                    }

                    if (condicaoVendaAceita)
                    {
                        //realizando a compra
                        listaOperacao.Add(RealizaOperacao(backTest, objCotacao.Data, objCotacao.Hora, preco, ativoDTO.LotePadrao, ref saldoLiquido, ref custodia,
                            ResultadoBacktestDTO.OperacaoEnum.Venda, false, false, true, ultimaOperacao, ref qtdOpBemsucedida, ref qtdOpMalSucedida, false));

                        //alterando o saldo total
                        saldoTotalAuxiliar += saldoLiquido;

                        //aumentando a quantidade de trades
                        qtdTrades++;

                        //armazena o ultimo preço negociado
                        ultimoPrecoNegociado = preco;

                    }


                    //avaliando se os stops foram atingidos para desfazer a operação
                    //avaliando stop gain para saida em compra
                    if (custodia > 0)
                    {
                        if (backTest.SairEmStopGain)
                        {
                            if (preco >= ultimoPrecoNegociado * (1 + backTest.PercentualStopGain / 100))
                            {
                                //devo reverter por conta do stop gain
                                //realizando a venda
                                listaOperacao.Add(RealizaOperacao(backTest, objCotacao.Data, objCotacao.Hora, preco, ativoDTO.LotePadrao, ref saldoLiquido, ref custodia,
                                    ResultadoBacktestDTO.OperacaoEnum.Venda, true, false, true, ultimaOperacao, ref qtdOpBemsucedida, ref qtdOpMalSucedida, false));

                                //alterando o saldo total
                                saldoTotalAuxiliar += saldoLiquido;

                                //aumentando a quantidade de trades
                                qtdTrades++;

                                //anotando os trades de stop gain
                                qtdStopGain++;

                            }
                        }
                        if (backTest.SairEmStopLoss)
                        {
                            if (preco <= ultimoPrecoNegociado * (1 - backTest.PercentualStopLoss / 100))
                            {
                                //devo reverter por conta do stop loss
                                //realizando a compra
                                listaOperacao.Add(RealizaOperacao(backTest, objCotacao.Data, objCotacao.Hora, preco, ativoDTO.LotePadrao, ref saldoLiquido, ref custodia,
                                    ResultadoBacktestDTO.OperacaoEnum.Venda, false, true, true, ultimaOperacao, ref qtdOpBemsucedida, ref qtdOpMalSucedida, false));

                                //alterando o saldo total
                                saldoTotalAuxiliar += saldoLiquido;

                                //aumentando a quantidade de trades
                                qtdTrades++;

                                //anotando os trades de stop loss
                                qtdStopLoss++;


                            }
                        }
                    }
                }

                //checando se deve encerrar o período liquido
                if (backTest.LiquidarPosicaoFinalPeriodo)
                {
                    #region Resgatando Preço
                    //setando o preço de acordo com o selecionado pelo cliente
                    preco = 0;
                    switch (backTest.TipoPrecoEnumerado)
                    {
                        case BacktestDTO.TipoPrecoEnum.Abertura:
                            preco = listaCotacao[listaCotacao.Count - 1].Abertura;
                            break;
                        case BacktestDTO.TipoPrecoEnum.Maximo:
                            preco = listaCotacao[listaCotacao.Count - 1].Maximo;
                            break;
                        case BacktestDTO.TipoPrecoEnum.Medio:
                            preco = (listaCotacao[listaCotacao.Count - 1].Maximo / listaCotacao[listaCotacao.Count - 1].Minimo) / 2;
                            break;
                        case BacktestDTO.TipoPrecoEnum.Minimo:
                            preco = listaCotacao[listaCotacao.Count - 1].Minimo;
                            break;
                        case BacktestDTO.TipoPrecoEnum.Ultimo:
                            preco = listaCotacao[listaCotacao.Count - 1].Ultimo;
                            break;
                    }
                    #endregion

                    if (custodia > 0)
                    {
                        if (listaOperacao.Count > 0)
                            listaOperacao.Add(RealizaOperacao(backTest, listaCotacao[listaCotacao.Count - 1].Data, listaCotacao[listaCotacao.Count - 1].Hora,
                                preco, ativoDTO.LotePadrao, ref saldoLiquido, ref custodia,
                                ResultadoBacktestDTO.OperacaoEnum.Venda, false, false, true, listaOperacao[listaOperacao.Count - 1], ref qtdOpBemsucedida, ref qtdOpMalSucedida,
                                true));
                        else
                            listaOperacao.Add(RealizaOperacao(backTest, listaCotacao[listaCotacao.Count - 1].Data, listaCotacao[listaCotacao.Count - 1].Hora,
                                preco, ativoDTO.LotePadrao, ref saldoLiquido, ref custodia,
                                ResultadoBacktestDTO.OperacaoEnum.Venda, false, false, true, null, ref qtdOpBemsucedida, ref qtdOpMalSucedida,
                                true));

                        //alterando o saldo total
                        saldoTotalAuxiliar += saldoLiquido;

                        //aumentando a quantidade de trades
                        qtdTrades++;

                        #region Registrando os saldos que vao pro sumario

                        if (saldoLiquido > saldoMaximo)
                            saldoMaximo = saldoLiquido;

                        if (saldoLiquido < saldoMinimo)
                            saldoMinimo = saldoLiquido;

                        if (qtdTrades > 0)
                            saldoMedio = saldoTotalAuxiliar / qtdTrades;
                        else
                            saldoMedio = saldoTotalAuxiliar;

                        #endregion

                    }
                    else
                    {
                        if (listaOperacao.Count > 0)
                            listaOperacao.Add(RealizaOperacao(backTest, listaCotacao[listaCotacao.Count - 1].Data, listaCotacao[listaCotacao.Count - 1].Hora,
                                preco, ativoDTO.LotePadrao, ref saldoLiquido, ref custodia,
                                ResultadoBacktestDTO.OperacaoEnum.Compra, false, false, true, listaOperacao[listaOperacao.Count - 1], ref qtdOpBemsucedida, ref qtdOpMalSucedida,
                                true));
                        else
                            listaOperacao.Add(RealizaOperacao(backTest, listaCotacao[listaCotacao.Count - 1].Data, listaCotacao[listaCotacao.Count - 1].Hora,
                                preco, ativoDTO.LotePadrao, ref saldoLiquido, ref custodia,
                                ResultadoBacktestDTO.OperacaoEnum.Compra, false, false, true, null, ref qtdOpBemsucedida, ref qtdOpMalSucedida,
                                true));

                        //alterando o saldo total
                        saldoTotalAuxiliar += saldoLiquido;

                        //aumentando a quantidade de trades
                        qtdTrades++;

                        #region Registrando os saldos que vao pro sumario

                        if (saldoLiquido > saldoMaximo)
                            saldoMaximo = saldoLiquido;

                        if (saldoLiquido < saldoMinimo)
                            saldoMinimo = saldoLiquido;

                        if (qtdTrades > 0)
                            saldoMedio = saldoTotalAuxiliar / qtdTrades;
                        else
                            saldoMedio = saldoTotalAuxiliar;

                        #endregion
                    }

                }

                #endregion

                //Salvando o backtesting com seu novo sumario
                backTest.QtdStopGain = qtdStopGain;
                backTest.QtdStopLoss = qtdStopLoss;
                backTest.QtdTrades = qtdTrades;
                backTest.ResultadoFinal = saldoLiquido;
                backTest.ResultadoMaximo = saldoMaximo;
                backTest.ResultadoMedio = saldoMedio;
                backTest.ResultadoMinimo = saldoMinimo;
                backTest.StatusEnumerado = BacktestDTO.StatusEnum.Executado;
                backTest.OpBemSucedidas = qtdOpBemsucedida;
                backTest.OpMalSucedidas = qtdOpMalSucedida;

                if (Double.IsNaN(custodia))
                    custodia = 0;
                backTest.PosicaoFinal = Convert.ToInt32(custodia);
                backTest.ResultadoTotal = saldoLiquido + (custodia * preco);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que realiza a operação
        /// </summary>
        /// <param name="backTest"></param>
        /// <param name="data"></param>
        /// <param name="preco"></param>
        /// <param name="hora"></param>
        /// <param name="lotePadrao"></param>
        /// <param name="saldoLiquido"></param>
        /// <param name="custodia"></param>
        /// <param name="tipoOperacao"></param>
        private ResultadoBacktestDTO RealizaOperacao(BacktestDTO backTest, DateTime data, string hora, double preco, int lotePadrao, ref double saldoLiquido,
            ref double custodia, ResultadoBacktestDTO.OperacaoEnum tipoOperacao, bool operacaoStopGain, bool operacaoStopLoss, bool operacaoReversao, ResultadoBacktestDTO ultimaOperacao,
            ref int qtdOpBemSucedida, ref int qtdOpMalSucedida, bool liquidar)
        {
            double valorBrutoOp = 0;
            double valorOperacao = 0;
            double valorEmolumento = 0;
            ResultadoBacktestDTO operacaoDTO = new ResultadoBacktestDTO();
            operacaoDTO.Preco = preco;
            operacaoDTO.IdBackTest = backTest.Id;
            operacaoDTO.Operacao = (int)tipoOperacao;

            //setando a data
            switch (backTest.PeriodicidadeEnumerado)
            {
                //case BacktestDTO.TipoPeriodicidadeEnum.CentoVinteMinutos:
                case BacktestDTO.TipoPeriodicidadeEnum.CincoMinutos:
                case BacktestDTO.TipoPeriodicidadeEnum.DezMinutos:
                case BacktestDTO.TipoPeriodicidadeEnum.DoisMinutos:
                case BacktestDTO.TipoPeriodicidadeEnum.QuinzeMinutos:
                case BacktestDTO.TipoPeriodicidadeEnum.SessentaMinutos:
                case BacktestDTO.TipoPeriodicidadeEnum.TresMinutos:
                case BacktestDTO.TipoPeriodicidadeEnum.TrintaMinutos:
                case BacktestDTO.TipoPeriodicidadeEnum.UmMinuto:
                    operacaoDTO.DataHora = new DateTime(data.Year, data.Month, data.Day, Convert.ToInt32(hora.Substring(0, 2)), Convert.ToInt32(hora.Substring(2, 2)), 0);
                    break;
                case BacktestDTO.TipoPeriodicidadeEnum.Diario:
                case BacktestDTO.TipoPeriodicidadeEnum.Mensal:
                case BacktestDTO.TipoPeriodicidadeEnum.Semanal:
                    operacaoDTO.DataHora = new DateTime(data.Year, data.Month, data.Day);
                    break;
            }

            //Calculando quantos lotes serão operados e a quantidade que serão comprados
            if (!backTest.PermitirOperacaoDescoberto)
            {
                if (!liquidar)
                {
                    if (operacaoDTO.OperacaoEnumerado == ResultadoBacktestDTO.OperacaoEnum.Compra)
                        operacaoDTO.Quantidade = lotePadrao * Math.Floor((saldoLiquido / preco) / lotePadrao);
                    else
                        operacaoDTO.Quantidade = custodia;
                }
                else
                {
                    operacaoDTO.Quantidade = custodia;
                }
            }
            else
            {
                if (!liquidar)
                {
                    if (operacaoDTO.OperacaoEnumerado == ResultadoBacktestDTO.OperacaoEnum.Venda)
                    {
                        //se for venda, eu posso vender o que tenho mais a exposição
                        //calcular a quantidade que a exposição maxima permite
                        double quantidadeExpostaMaxima = lotePadrao * Math.Floor((backTest.ValorExposicaoMaxima / preco) / lotePadrao);
                        operacaoDTO.Quantidade = custodia + quantidadeExpostaMaxima;
                    }
                    else
                    {
                        operacaoDTO.Quantidade = lotePadrao * Math.Floor((saldoLiquido / preco) / lotePadrao);
                    }
                }
                else
                    operacaoDTO.Quantidade = Math.Abs(custodia);

            }
            //Calculando o valor da operação no caso de compra
            if (operacaoDTO.OperacaoEnumerado == ResultadoBacktestDTO.OperacaoEnum.Compra)
            {
                valorBrutoOp = (-1) * (operacaoDTO.Quantidade * preco);

                //checando se deve levar em consideração corretagem + emolumentos
                if (backTest.ConsiderarCorretagemMaisEmolumento)
                {
                    valorEmolumento = percentualEmolumentos * valorBrutoOp;
                    valorOperacao = valorBrutoOp - Math.Abs(valorEmolumento) - backTest.ValorCorretagem;
                }
                else
                {
                    valorEmolumento = 0;
                    valorOperacao = valorBrutoOp;
                }
            }
            else
            {
                valorBrutoOp = (operacaoDTO.Quantidade * preco);

                //checando se deve levar em consideração corretagem + emolumentos
                if (backTest.ConsiderarCorretagemMaisEmolumento)
                {
                    valorEmolumento = percentualEmolumentos * valorBrutoOp;
                    valorOperacao = valorBrutoOp - valorEmolumento - backTest.ValorCorretagem;
                }
                else
                {
                    valorEmolumento = 0;
                    valorOperacao = valorBrutoOp;
                }
            }


            //Atualizando campos de acordo com o tipo da operação
            if (operacaoDTO.OperacaoEnumerado == ResultadoBacktestDTO.OperacaoEnum.Compra)
                custodia += operacaoDTO.Quantidade;
            else
                custodia -= operacaoDTO.Quantidade;

            //Atualizando saldo liquido
            saldoLiquido += valorOperacao;
            operacaoDTO.SaldoParcial = saldoLiquido;
            operacaoDTO.SaldoTotal = saldoLiquido + (custodia * preco);
            operacaoDTO.StopGainAtingido = operacaoStopGain;
            operacaoDTO.StopLossAtingido = operacaoStopLoss;
            operacaoDTO.CustodiaParcial = custodia;

            //calculando a rentabildiade acumulada = saldo total - saldo inicial / saldo total
            operacaoDTO.RentabilidadeAcumulada = ((operacaoDTO.SaldoTotal - backTest.VolumeFinanceiroInicial) / backTest.VolumeFinanceiroInicial) * 100;

            //comparando o saldo total da ultima operação com o desta operação
            if (operacaoReversao)
                if (ultimaOperacao != null)
                    if (ultimaOperacao.SaldoTotal > operacaoDTO.SaldoTotal)
                        qtdOpMalSucedida++;
                    else
                        qtdOpBemSucedida++;

            //Retornando a operação montada
            return operacaoDTO;
        }

        /* Nome: EvaluateFormula
         * Descrição: Avalia a formula de acordo com o ativo passado
         * Data: 27/10/2009
         */
        private bool EvaluateFormula(List<CotacaoServerDTO> listaCotacao, List<CondicaoDTO> listaCondicoes, bool reversao)
        {
            //variaveis auxiliares
            bool resultadoParcial = true;
            List<double> listaValorAbertura = new List<double>();
            List<double> listaValorMinimo = new List<double>();
            List<double> listaValorMaximo = new List<double>();
            List<double> listaValorUltimo = new List<double>();

            //Criando a lista de doubles que será usada nos calculos
            foreach (CotacaoServerDTO obj in listaCotacao)
            {
                listaValorAbertura.Add(obj.Abertura);
                listaValorMinimo.Add(obj.Minimo);
                listaValorMaximo.Add(obj.Maximo);
                listaValorUltimo.Add(obj.Ultimo);
            }

            //Percorrendo todos os valores presentes na formula
            foreach (CondicaoDTO objCondicao in listaCondicoes)
            {

                //verificando qual o comando que será executado
                switch (objCondicao.Comando)
                {
                    case "SMA":
                        if (!reversao)
                            resultadoParcial = calculosBM.EvaluateCruzamentoDeDuasSMA(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                0, "C");
                        else
                            resultadoParcial = calculosBM.EvaluateCruzamentoDeDuasSMA(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                0, "V");
                        break;
                    case "EMA":
                        if (!reversao)
                            resultadoParcial = calculosBM.EvaluateCruzamentoDeDuasEMA(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                0, "C");
                        else
                            resultadoParcial = calculosBM.EvaluateCruzamentoDeDuasEMA(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                0, "V");
                        break;
                    case "IFI":
                        if (!reversao)
                            resultadoParcial = calculosBM.EvaluateIFRInferior(listaValorUltimo,
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                0);
                        else
                            resultadoParcial = calculosBM.EvaluateIFRSuperior(listaValorUltimo,
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                0);
                        break;
                    case "IFS":
                        if (!reversao)
                            resultadoParcial = calculosBM.EvaluateIFRSuperior(listaValorUltimo,
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                0);
                        else
                            resultadoParcial = calculosBM.EvaluateIFRInferior(listaValorUltimo,
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                0);
                        break;
                    case "ULI":
                        if (!reversao)
                            resultadoParcial = calculosBM.EvaluateVariacaoInferior(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                0);
                        else
                            resultadoParcial = calculosBM.EvaluateVariacaoSuperior(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                0);
                        break;
                    case "ULS":
                        if (!reversao)
                            resultadoParcial = calculosBM.EvaluateVariacaoSuperior(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                0);
                        else
                            resultadoParcial = calculosBM.EvaluateVariacaoInferior(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                0);
                        break;
                    case "UMS":
                        if (!reversao)
                            resultadoParcial = calculosBM.EvaluateUltimoPrecoSuperiorAMediaSimples(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                0);
                        else
                            resultadoParcial = calculosBM.EvaluateUltimoPrecoInferiorAMediaSimples(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                0);
                        break;
                    case "UBS":
                        if (!reversao)
                            resultadoParcial = calculosBM.EvaluateUltimoPrecoInferiorAMediaSimples(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                0);
                        else
                            resultadoParcial = calculosBM.EvaluateUltimoPrecoSuperiorAMediaSimples(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                0);
                        break;
                    case "DIC":
                        resultadoParcial = calculosBM.EvaluateDIDICompra(listaValorUltimo,
                            0);
                        break;
                    case "DIV":
                        resultadoParcial = calculosBM.EvaluateDIDIVenda(listaValorUltimo,
                            0);
                        break;
                    case "TRC":
                        if (!reversao)
                            resultadoParcial = calculosBM.EvaluateTRIXCompra(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                0);
                        else
                            resultadoParcial = calculosBM.EvaluateTRIXVenda(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                0);
                        break;
                    case "TRB":
                        if (!reversao)
                            resultadoParcial = calculosBM.EvaluateTRIXVenda(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                0);
                        else
                            resultadoParcial = calculosBM.EvaluateTRIXCompra(listaValorUltimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                0);
                        break;
                    case "ESC":
                        if (!reversao)
                            resultadoParcial = calculosBM.EvaluateEstocasticoCompra(listaValorUltimo,
                                listaValorMaximo,
                                listaValorMinimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                3,//usando a velocidade/slowing 3
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                0);
                        else
                            resultadoParcial = calculosBM.EvaluateEstocasticoVenda(listaValorUltimo,
                            listaValorMaximo,
                            listaValorMinimo,
                            objCondicao.ListaParcelas[0].ValorInteiro,
                            3,//usando a velocidade/slowing 3
                            objCondicao.ListaParcelas[1].ValorInteiro,
                            0);
                        break;
                    case "ESV":
                        if (!reversao)
                            resultadoParcial = calculosBM.EvaluateEstocasticoVenda(listaValorUltimo,
                                listaValorMaximo,
                                listaValorMinimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                3,//usando a velocidade/slowing 3
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                0);
                        else
                            resultadoParcial = calculosBM.EvaluateEstocasticoCompra(listaValorUltimo,
                                listaValorMaximo,
                                listaValorMinimo,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                3,//usando a velocidade/slowing 3
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                0);
                        break;
                    case "MAC":
                        if (!reversao)
                            resultadoParcial = calculosBM.EvaluateMACDCompra(listaValorUltimo,
                                listaValorMaximo,
                                listaValorMinimo,
                                listaValorAbertura,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                objCondicao.ListaParcelas[2].ValorInteiro,
                                0);
                        else
                            resultadoParcial = calculosBM.EvaluateMACDVenda(listaValorUltimo,
                            listaValorMaximo,
                            listaValorMinimo,
                            listaValorAbertura,
                            objCondicao.ListaParcelas[0].ValorInteiro,
                            objCondicao.ListaParcelas[1].ValorInteiro,
                            objCondicao.ListaParcelas[2].ValorInteiro,
                            0);
                        break;
                    case "MAV":
                        if (!reversao)
                            resultadoParcial = calculosBM.EvaluateMACDVenda(listaValorUltimo,
                                listaValorMaximo,
                                listaValorMinimo,
                                listaValorAbertura,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                objCondicao.ListaParcelas[2].ValorInteiro,
                                0);
                        else
                            resultadoParcial = calculosBM.EvaluateMACDCompra(listaValorUltimo,
                                listaValorMaximo,
                                listaValorMinimo,
                                listaValorAbertura,
                                objCondicao.ListaParcelas[0].ValorInteiro,
                                objCondicao.ListaParcelas[1].ValorInteiro,
                                objCondicao.ListaParcelas[2].ValorInteiro,
                                0);
                        break;
                }

                if (!resultadoParcial)
                    return false;
            }


            return true;

        }

        #endregion Métodos de Calculo de Resultados
    }
}
