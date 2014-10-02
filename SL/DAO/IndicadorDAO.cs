using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Traderdata.Client.TerminalWEB.DTO;
using System.Collections.Generic;
using ModulusFE;
using ModulusFE.SL;
using ModulusFE.Indicators;

namespace Traderdata.Client.TerminalWEB.DAO
{
    public static class IndicadorDAO
    {
        #region Metodos publicos

        /// <summary>
        /// Metodo que retorna a listagem com todos os indicadores
        /// </summary>
        /// <returns></returns>
        public static void SetTodosIndicadoresInfo()
        {           
            try
            {
                #region Acumulação/Distribuição Wiliams

                IndicatorInfoDTO AcumulacaoDistribuicaoWiliams = new IndicatorInfoDTO();
                AcumulacaoDistribuicaoWiliams.Ajuda = @"O indicador define pontos de oscilação no curto prazo, indicando também a força e direção do mercado. 
Normalmente é usado para ter uma imagem melhor que o Swing Index para o longo prazo, que usa dados de duas barras somente. Se no longo prazo a tendência é de alta, o valor do accumulative swing index é positivo. Inversamente é negativo em tendência de baixa no longo prazo. Em mercados sem tendência, andando de lado, o ASI permanece variando entre valores positivos e negativos. O indicador é mais usado para futuros, mas também pode ser usado para ações. 
O ASI pode ser usado como uma alternativa de preço, aplicando nele indicadores, linhas de tendência e padrões gráficos. ";
                
                AcumulacaoDistribuicaoWiliams.NomePortugues = "Acumulação/Distribuição Wiliams";
                AcumulacaoDistribuicaoWiliams.TemSerieFilha1 = true;
                AcumulacaoDistribuicaoWiliams.TemSerieFilha2 = true;
                AcumulacaoDistribuicaoWiliams.TipoStockchart = IndicatorType.WilliamsAccumulationDistribution;
                AcumulacaoDistribuicaoWiliams.NovoPainel = true;
                AcumulacaoDistribuicaoWiliams.Propriedades = GetPropriedadesAcumulacaoDistribuicaoWiliams();
                AcumulacaoDistribuicaoWiliams.StrokeColor = Colors.Red;
                AcumulacaoDistribuicaoWiliams.StrokeThickness = 1;
                AcumulacaoDistribuicaoWiliams.StrokeType = 0;
                StaticData.listaIndicadores.Add(AcumulacaoDistribuicaoWiliams);

                #endregion

                #region Aroon

                IndicatorInfoDTO Aroon = new IndicatorInfoDTO();
                Aroon.Ajuda = @"O Aroon é usado para medir a presença e a força de uma tendência. Quando o Aroon(up) e Aroon(down) se movem na mesma direção juntos não há uma tendência definida (o preço está se movendo de lado, ou está próximo de se mover de lado). Quando o Aroon(up) está abaixo de 50, é uma indicação que a tendência de alta está perdendo seu momentum, enquanto o Aroon(down) está abaixo de 50, então a indicação é que a tendência de baixa está perdendo momentum. Quando o Aroon(up) ou o Aroon(down) estão acima de 70, indica uma forte tendência, enquanto se estão abaixo de 30 indicam baixa força e um sinal de uma nova tendência na direção oposta.";                
                Aroon.NomePortugues = "Aroon";
                Aroon.TemSerieFilha1 = false;
                Aroon.TemSerieFilha2 = false;
                Aroon.NovoPainel = true;
                Aroon.TipoStockchart = IndicatorType.Aroon;
                Aroon.Propriedades = GetPropriedadesAroon();
                Aroon.StrokeColor = Colors.Red;
                Aroon.StrokeThickness = 1;
                Aroon.StrokeType = 0;
                StaticData.listaIndicadores.Add(Aroon);

                #endregion

                #region BollingerBands

                IndicatorInfoDTO BollingerBands = new IndicatorInfoDTO();
                BollingerBands.Ajuda = @"São linhas plotadas no preço e em volta dele, em estrutura de envelope, em comparação são similares aos Envelopes de Média Móvel, porém as Bandas Bollinger são calculadas usando o desvio padrão em vez de bandas móveis por uma porcentagem fixa. 
O Bollinger Bands é um indicador que permite comparar a volatilidade e os níveis relativos de preços do ativo por um dado período. Pode ser combinado com a série de preço para gerar sinais de compra e venda nos cruzamentos e prever futuros movimentos significantes. 
Geralmente é usado para identificar períodos de alta ou baixa volatilidade (com a abertura ou fechamento das bandas), identificar quando os preços estão em extremos e possivelmente em um nível insustentável ou para atingir decisões rigorosas de compra e venda.";

                BollingerBands.NomePortugues = "Bandas de Bollinger";
                BollingerBands.NovoPainel = false;
                BollingerBands.TemSerieFilha1 = true;
                BollingerBands.TemSerieFilha2 = true;
                BollingerBands.TipoStockchart = IndicatorType.BollingerBands;
                BollingerBands.StrokeColor = Colors.Red;
                BollingerBands.StrokeThickness = 1;
                BollingerBands.StrokeType = 0;
                BollingerBands.Propriedades = GetPropriedadesBollingerBands();
                StaticData.listaIndicadores.Add(BollingerBands);

                #endregion

                #region Bandas Fractal Chaos

                IndicatorInfoDTO BandasFractalChaos = new IndicatorInfoDTO();
                BandasFractalChaos.Ajuda = @"O indicador procura no passado dependendo do número de períodos selecionado para plotar o indicador. 
Além de ser usado como uma banda determinando suporte e resistência, a inclinação das bandas é um bom indicador de agitação ou tendência do mercado. ";
                
                BandasFractalChaos.NomePortugues = "Bandas Fractal Chaos";
                BandasFractalChaos.NovoPainel = true;
                BandasFractalChaos.TemSerieFilha1 = false;
                BandasFractalChaos.TemSerieFilha2 = false;
                BandasFractalChaos.TipoStockchart = IndicatorType.FractalChaosBands;
                BandasFractalChaos.StrokeColor = Colors.Red;
                BandasFractalChaos.StrokeThickness = 1;
                BandasFractalChaos.StrokeType = 0;
                BandasFractalChaos.Propriedades = GetPropriedadesBandasFractalChaos();
                StaticData.listaIndicadores.Add(BandasFractalChaos);

                #endregion

                #region Banda de Números Primos

                IndicatorInfoDTO BandaNumerosPrimos = new IndicatorInfoDTO();
                BandaNumerosPrimos.Ajuda = @"Similar ao Prime Numbers Oscillator, o prime numbers bands (Bandas de Números Primos) foi desenvolvido pela Modulus Financial Engineering, Inc. Este indicador encontra os números prímos mais próximos das máximas e mínimas e plota duas sérias como bandas. ";
                
                BandaNumerosPrimos.NomePortugues = "Banda de Números Primos";
                BandaNumerosPrimos.NovoPainel = true;

                BandaNumerosPrimos.TemSerieFilha1 = true;
                BandaNumerosPrimos.TemSerieFilha2 = false;
                BandaNumerosPrimos.StrokeColor = Colors.Red;
                BandaNumerosPrimos.StrokeThickness = 1;
                BandaNumerosPrimos.StrokeType = 0;
                BandaNumerosPrimos.TipoStockchart = IndicatorType.PrimeNumberBands;
                BandaNumerosPrimos.Propriedades = GetPropriedadesBandaNumerosPrimos();
                StaticData.listaIndicadores.Add(BandaNumerosPrimos);

                #endregion

                #region Bandas Máxima/Mínimo

                IndicatorInfoDTO BandasMaximaMinimo = new IndicatorInfoDTO();
                BandasMaximaMinimo.Ajuda = @"As Bandas High Low consistem em uma média móvel triangular calculada a partir do preço, substituíndo a variação por uma porcentagem fixa e incluindo um valor médio. 
Quando os preços tocam a banda superior a banda inferior, uma mudança de direção no intraday pode ocorrer quando o preço voltar. ";

                BandasMaximaMinimo.NovoPainel = false;
                BandasMaximaMinimo.NomePortugues = "Bandas Máxima/Mínimo";
                BandasMaximaMinimo.TemSerieFilha1 = false;
                BandasMaximaMinimo.TemSerieFilha2 = false;
                BandasMaximaMinimo.StrokeColor = Colors.Red;
                BandasMaximaMinimo.StrokeThickness = 1;
                BandasMaximaMinimo.StrokeType = 0;
                BandasMaximaMinimo.TipoStockchart = IndicatorType.HighLowBands;
                BandasMaximaMinimo.Propriedades = GetPropriedadesBandasMaximaMinimo();
                StaticData.listaIndicadores.Add(BandasMaximaMinimo);

                #endregion

                #region Chaikin Fluxo Financeiro(Money Flow)

                IndicatorInfoDTO ChaikinFluxoFinanceiro = new IndicatorInfoDTO();
                ChaikinFluxoFinanceiro.Ajuda = @"O Oscilador Chaikin Money Flow, desenvolvido por Marc Chaikin, é um indicador de momentum que pontua compra e venda através do calculo de preço e volume juntos. Este indicador é baseado no Chaikin Accumulation/Distribution, que por sua vez é baseado na premissa que se uma ação fecha acima do ponto médio [(max+min)/2] do dia então então este dia está acumulando, caso contrário está distribuindo. 
Na pratica é usado para avaliar o fluxo acumulado de dinheiro para dentro ou para fora do ativo já que o preço se move dependendo da oferta e demanda, ele segue o volume.";

                ChaikinFluxoFinanceiro.NovoPainel = true;
                ChaikinFluxoFinanceiro.NomePortugues = "Chaikin Fluxo Financeiro(Money Flow)";
                ChaikinFluxoFinanceiro.TemSerieFilha1 = false;
                ChaikinFluxoFinanceiro.TemSerieFilha2 = false;
                ChaikinFluxoFinanceiro.StrokeColor = Colors.Red;
                ChaikinFluxoFinanceiro.StrokeThickness = 1;
                ChaikinFluxoFinanceiro.StrokeType = 0;
                ChaikinFluxoFinanceiro.TipoStockchart = IndicatorType.ChaikinMoneyFlow;
                ChaikinFluxoFinanceiro.Propriedades = GetPropriedadesChaikinFluxoFinanceiro();
                StaticData.listaIndicadores.Add(ChaikinFluxoFinanceiro);

                #endregion

                #region Chaikin Volatilidade

                IndicatorInfoDTO ChaikinVolatilidade = new IndicatorInfoDTO();
                ChaikinVolatilidade.Ajuda = @"O Chaikin Volatility Oscillator é uma Média Móvel derivada de um índice de Acumulação/Distribuição, é construído com a diferença entre uma média móvel dos preços máximo-mínimo do período atual, pela mesma medida de n períodos atrás. 
O indicador Chaikin Volatility representa a diferença entre os preços máximos e mínimos periódicos. O aumento do indicador reflete o aumento da volatilidade. A diminuição do indicador reflete a diminuição da volatilidade, que por sua vez é um potencial sinal de uma tendência do preço. O indicador formará picos quando os preços atingirem uma nova máxima ou nova mínima.";

                ChaikinVolatilidade.NovoPainel = true;
                ChaikinVolatilidade.NomePortugues = "Chaikin Volatilidade";
                ChaikinVolatilidade.TemSerieFilha1 = false;
                ChaikinVolatilidade.TemSerieFilha2 = false;
                ChaikinVolatilidade.StrokeColor = Colors.Red;
                ChaikinVolatilidade.StrokeThickness = 1;
                ChaikinVolatilidade.StrokeType = 0;
                ChaikinVolatilidade.TipoStockchart = IndicatorType.ChaikinVolatility;
                ChaikinVolatilidade.Propriedades = GetPropriedadesChaikinVolatilidade();
                StaticData.listaIndicadores.Add(ChaikinVolatilidade);

                #endregion

                #region Commodity Channel Index(CCI)

                IndicatorInfoDTO CommodityChannelIndex = new IndicatorInfoDTO();
                CommodityChannelIndex.Ajuda = @"O Commodity Channel Index é um indicador versátil, capaz de produzir um amplo arranjo de sinais de compra e venda. O CCI pode ser usado para identificar níveis sobre-comprados/sobre-vendidos. Um ativo pode ser considerado sobre-vendido quando o CCI mergulha abaixo de -100 e pode ser considerado sobre-comprado quando excede +100. A partir de um nível sobre-vendido, um sinal de compra pode ser disparado quando o CCI se move de volta acima de -100. Inversamente, o sinal de venda pode ser considerado quando o indicador retorna do nível sobre-comprado cruzando +100. 
O CCI também pode ajudar a identificar reversões no preço, preços extremos e força da tendência. O CCI se encaixa na categoria de osciladores momentum. ";

                CommodityChannelIndex.NovoPainel = true;
                CommodityChannelIndex.NomePortugues = "Commodity Channel Index(CCI)";
                CommodityChannelIndex.TemSerieFilha1 = false;
                CommodityChannelIndex.TemSerieFilha2 = false;
                CommodityChannelIndex.StrokeColor = Colors.Red;
                CommodityChannelIndex.StrokeThickness = 1;
                CommodityChannelIndex.StrokeType = 0;
                CommodityChannelIndex.TipoStockchart = IndicatorType.CommodityChannelIndex;
                CommodityChannelIndex.Propriedades = GetPropriedadesCommodityChannelIndex();
                StaticData.listaIndicadores.Add(CommodityChannelIndex);

                #endregion

                #region Ease of Movement

                IndicatorInfoDTO EaseMovement = new IndicatorInfoDTO();
                EaseMovement.Ajuda = @"O indicador Ease of Movement mostra a relação entre o volume e a alteração do preço sobre um período especificado pelo usuário. Foi desenvolvido por Richard W. Arms, Jr. e calcula a facilidade com que o preço se move. Quanto maior o movimento do preço e mais leve o volume, mais fácil é o movimento. 
Um valor alto do Ease of Movement ocorre quando os preços se movem subindo com um volume leve. Valores baixos do Ease of Movement ocorrem quando os preços se movem descendo com um volume leve. Se os preços não se estão se movendo, ou se um volume forte é necessário para mover o preço, então o indicador estará próximo de zero. 
O Oscilador Ease of Movement cai quando os preços estão com tendência altista sob um baixo volume e, igualmente, cai quando os preços estão com tendencia baixista sob um baixo volume. Não confirmando então o aparente movimento. ";
                EaseMovement.NomePortugues = "Ease of Movement";

                EaseMovement.NovoPainel = true;
                EaseMovement.TemSerieFilha1 = true;
                EaseMovement.TemSerieFilha2 = true;
                EaseMovement.StrokeColor = Colors.Red;
                EaseMovement.StrokeThickness = 1;
                EaseMovement.StrokeType = 0;
                EaseMovement.TipoStockchart = IndicatorType.EaseOfMovement;
                EaseMovement.Propriedades = GetPropriedadesEaseMovement();
                StaticData.listaIndicadores.Add(EaseMovement);

                #endregion

                #region Envelope de Médias Móveis

                IndicatorInfoDTO EnvelopeMediasMoveis = new IndicatorInfoDTO();
                EnvelopeMediasMoveis.Ajuda = @"O Envelope é usado para indicar níveis de sobre-compra/venda. e pode ser usado sozinho ou em conjunto com outros indicadores, especialmente indicadores de momentum. 
Quando os preços ultrapassam a banda superior ou caem abaixo da inferior, uma mudança na direção pode ocorrer, asim que o preço voltar a penetrar uma banda. 
Alguns traders usam envelopes para determinar quando os preços estão caros ou baratos e operar como bandas, porém é notável que em tendências fortes os preços tendem a ficar fora do envelope no sentido da tendência. ";
                EnvelopeMediasMoveis.NomePortugues = "Envelope de Médias Móveis";

                EnvelopeMediasMoveis.NovoPainel = true;
                EnvelopeMediasMoveis.TemSerieFilha1 = true;
                EnvelopeMediasMoveis.TemSerieFilha2 = false;
                EnvelopeMediasMoveis.StrokeColor = Colors.Red;
                EnvelopeMediasMoveis.StrokeThickness = 1;
                EnvelopeMediasMoveis.StrokeType = 0;
                EnvelopeMediasMoveis.TipoStockchart = IndicatorType.MovingAverageEnvelope;
                EnvelopeMediasMoveis.Propriedades = GetPropriedadesEnvelopeMediasMoveis();
                StaticData.listaIndicadores.Add(EnvelopeMediasMoveis);

                #endregion

                #region Fechamento Ponderado

                IndicatorInfoDTO FechamentoPonderado = new IndicatorInfoDTO();
                FechamentoPonderado.Ajuda = " Weighted Close é uma média de cada dia de preço. ";
                FechamentoPonderado.NomePortugues = "Fechamento Ponderado    ";
                FechamentoPonderado.TemSerieFilha1 = false;
                FechamentoPonderado.TemSerieFilha2 = false;
                FechamentoPonderado.NovoPainel = false;
                FechamentoPonderado.StrokeColor = Colors.Red;
                FechamentoPonderado.StrokeThickness = 1;
                FechamentoPonderado.StrokeType = 0;
                FechamentoPonderado.TipoStockchart = IndicatorType.WeightedClose;
                FechamentoPonderado.Propriedades = GetPropriedadesFechamentoPonderado();
                StaticData.listaIndicadores.Add(FechamentoPonderado);

                #endregion

                #region Filtro Vertical/Horizontal

                IndicatorInfoDTO FiltroVerticalHorizontal = new IndicatorInfoDTO();
                FiltroVerticalHorizontal.Ajuda = @"O Vertical Horizontal Filter (VHF) identifica se o mercado está com tendência ou em uma fase agitada. 
O indicador VHF é mais usado como um indicador de volatilidade. Também é normalmente usado como componente em outros indicadores.";
               
                FiltroVerticalHorizontal.NomePortugues = "Filtro Vertical/Horizontal";
                FiltroVerticalHorizontal.NovoPainel = true;
                FiltroVerticalHorizontal.TemSerieFilha1 = true;
                FiltroVerticalHorizontal.TemSerieFilha2 = true;
                FiltroVerticalHorizontal.StrokeColor = Colors.Red;
                FiltroVerticalHorizontal.StrokeThickness = 1;
                FiltroVerticalHorizontal.StrokeType = 0;
                FiltroVerticalHorizontal.TipoStockchart = IndicatorType.VerticalHorizontalFilter;
                FiltroVerticalHorizontal.Propriedades = GetPropriedadesFiltroVerticalHorizontal();
                StaticData.listaIndicadores.Add(FiltroVerticalHorizontal);

                #endregion

                #region High Low Activator

                IndicatorInfoDTO HighLowActivator = new IndicatorInfoDTO();
                HighLowActivator.Ajuda = @"Essa ferramenta foi introduzida por Robert Krausz em 1998, tendo sido inspirada num dos supostos métodos do trade W. D. Gann. O HiLo é um rastreador de tendência que trabalha com médias móveis e, por isso, funciona bem em mercados em tendências e não muito em mercados laterais. A principal utilidade dessa ferramenta é fornecer um stop móvel, mais também pode ser usado com uma ferramenta de entrada. A ferramente basicamente é a média das mínimas e máxima de um determinado período, excluindo o candle vigente. Ou seja, em um software que não tem o HiLo pode-se utilizar uma média móvel aritmética das mínimas e máximas com deslocamento de um período. Os períodos das mínimas e máximas pode ser ajustado, mais normalmente se usa 3 e 6 períodos (no gráfico semanal) dependendo do objetivo. Em alguns softwares como o Profit Chart, essa ferramenta é plotada em formato de escada, como mostrado na imagem acima. Quando os preços estão acima das médias das máximas, aparece no gráfico uma escala com as mínimas. E quando esses preços passam para baixo da mínima, o sistema plota uma escala com as médias das máximas. Muitos usam esse indicador como um setup de seguidor de tendência (trend following), comprando o ativo quando os preços fecham acima da máxima e vendendo quando fecham abaixo das mínimas.";
                HighLowActivator.NomePortugues = "High Low Activator";
                HighLowActivator.NovoPainel = false;
                HighLowActivator.TemSerieFilha1 = false;
                HighLowActivator.TemSerieFilha2 = false;
                HighLowActivator.StrokeColor = Colors.Red;
                HighLowActivator.StrokeThickness = 1;
                HighLowActivator.StrokeType = 0;
                HighLowActivator.TipoStockchart = IndicatorType.HighLowActivator;
                HighLowActivator.Propriedades = GetPropriedadesHighLowActivator();
                StaticData.listaIndicadores.Add(HighLowActivator);

                #endregion

                #region High Minus Low

                IndicatorInfoDTO HighMinusLow = new IndicatorInfoDTO();
                HighMinusLow.Ajuda = @"Retorna o preço máximo menos o preço mínimo, fornecendo uma boa idéia de range de variação dos preços em cada barra, muito útil para daytrades. ";
                
                HighMinusLow.NomePortugues = "High Minus Low";
                HighMinusLow.NovoPainel = true;
                HighMinusLow.TemSerieFilha1 = false;
                HighMinusLow.TemSerieFilha2 = false;
                HighMinusLow.StrokeColor = Colors.Red;
                HighMinusLow.StrokeThickness = 1;
                HighMinusLow.StrokeType = 0;
                HighMinusLow.TipoStockchart = IndicatorType.HighMinusLow;
                HighMinusLow.Propriedades = GetPropriedadesHighMinusLow();
                StaticData.listaIndicadores.Add(HighMinusLow);

                #endregion

                //Pendente
                #region Identificador de Agulha

                //IndicatorInfoDTO IdentificadorAgulha = new IndicatorInfoDTO();
                //IdentificadorAgulha.Ajuda = "";
                //IdentificadorAgulha.NomePortugues = "Identificador de Agulha";
                //IdentificadorAgulha.NovoPainel = true;
                //IdentificadorAgulha.TemSerieFilha1 = true;
                //IdentificadorAgulha.TemSerieFilha2 = true;
                //IdentificadorAgulha.StrokeColor = Colors.Red;
                //IdentificadorAgulha.StrokeThickness = 1;
                //IdentificadorAgulha.StrokeType = 0;
                //IdentificadorAgulha.TipoStockchart = IndicatorType.CustomIndicator;

                ////IdentificadorAgulha.TipoStockchart = IndicatorType.IdentificadorAgulha;
                //IdentificadorAgulha.Propriedades = GetPropriedadesIdentificadorAgulha();
                //StaticData.listaIndicadores.Add(IdentificadorAgulha);

                #endregion

                #region Índice de Fluxo Financeiro

                IndicatorInfoDTO IndiceFluxoFinanceiro = new IndicatorInfoDTO();
                IndiceFluxoFinanceiro.Ajuda = @"O Money Flow Index mede o fluxo de dinheiro de um ativo, usando o preço e o volume para os cálculos. 
São usados os valores 20 e 80 como gatilho e referência de movimento dos preços. Também são usadas divergência entre o preço e o Money Flow Index. 
O Money Flow Index ('MFI') é um indicador de momentum que mede a força do dinheiro que está entrando ou saindo de um ativo, calculando o volume. O Money flow é um indicador que calcula e indexa um valor baseado no preço e volume por um número de barras especificado. Os cálculos são feitos para cada barras com média de preço maior que a barra anterior e para cada barra com média de preço menor que a barra anterior. Estes valores são então indexados para calcular e plotar o Money Flow. O uso tanto do preço quadto do volume fornece uma perpectiva diferente do preço e volume sozinhos. O indicador MFI tende a mostrar uma oscilação mais dramática e pode ser muito útil para identificar condições de mercados sobre-comprados ou sobre-vendidos. ";
                IndiceFluxoFinanceiro.NomePortugues = "Índice de Fluxo Financeiro";
                IndiceFluxoFinanceiro.NovoPainel = true;
                IndiceFluxoFinanceiro.TemSerieFilha1 = false;
                IndiceFluxoFinanceiro.TemSerieFilha2 = false;
                IndiceFluxoFinanceiro.StrokeColor = Colors.Red;
                IndiceFluxoFinanceiro.StrokeThickness = 1;
                IndiceFluxoFinanceiro.StrokeType = 0;
                IndiceFluxoFinanceiro.TipoStockchart = IndicatorType.MoneyFlowIndex;
                IndiceFluxoFinanceiro.Propriedades = GetPropriedadesIndiceFluxoFinanceiro();
                StaticData.listaIndicadores.Add(IndiceFluxoFinanceiro);

                #endregion

                #region Índice de Força Relativa(IFR/RSI)

                IndicatorInfoDTO IndiceForcaRelativa = new IndicatorInfoDTO();
                IndiceForcaRelativa.Ajuda = @"O IFR é um indicador popular que mostra a força comparativa do preço de um ativo. Desenvolvido por J. Welles Wilder o Índice de Força Relativa ('IFR') compara a força interna de um único ativo. O IFR compara o valor que o ativo ganhou recentemente com o valor de suas recentes perdas e dispõe esta informação em um número que varia de 0 a 100. Ele usa um único parâmetro, o número de períodos para usar no cálculo. Wilder recomenda usar 14 períodos. O IFR compara movimentos ascendentes no preço de fechamento com os movimentos descendentes em um dado período. Wilder originalmente usou 14 períodos, mas é comum usar 7 e 9 períodos para períodos curtos. Também é formulado para oscilar entre 0 e 100, possibilitando níveis fixos de sobre-compra/sobre-venda.Os períodos 9, 14 e 25 do IFR são os mais populares. O método mais usual para interpretar o IFR é verificar divergências entre ele e o preço, bem como buscar níveis de suporte e resistência, ou padrões no gráfico IFR.";

                IndiceForcaRelativa.NomePortugues = "Índice de Força Relativa(IFR/RSI)";
                IndiceForcaRelativa.NovoPainel = true;
                IndiceForcaRelativa.TemSerieFilha1 = false;
                IndiceForcaRelativa.TemSerieFilha2 = false;
                IndiceForcaRelativa.StrokeColor = Colors.Red;
                IndiceForcaRelativa.StrokeThickness = 1;
                IndiceForcaRelativa.StrokeType = 0;
                IndiceForcaRelativa.TipoStockchart = IndicatorType.RelativeStrengthIndex;
                IndiceForcaRelativa.Propriedades = GetPropriedadesIndiceForcaRelativa();
                StaticData.listaIndicadores.Add(IndiceForcaRelativa);

                #endregion

                //Pendente
                #region Índice de Força Relativa Comparada

                //IndicatorInfoDTO IndiceForcaRelativaComparada = new IndicatorInfoDTO();
                //IndiceForcaRelativaComparada.Ajuda = "ALO";
                //IndiceForcaRelativaComparada.NovoPainel = true;
                //IndiceForcaRelativaComparada.NomePortugues = "Índice de Força Relativa Comparada";
                //IndiceForcaRelativaComparada.TemSerieFilha1 = false;
                //IndiceForcaRelativaComparada.TemSerieFilha2 = false;
                //IndiceForcaRelativaComparada.StrokeColor = Colors.Red;
                //IndiceForcaRelativaComparada.StrokeThickness = 1;
                //IndiceForcaRelativaComparada.StrokeType = 0;
                //IndiceForcaRelativaComparada.TipoStockchart = IndicatorType.ComparativeRelativeStrength;
                //IndiceForcaRelativaComparada.Propriedades = GetPropriedadesIndiceForcaRelativaComparada();
                //StaticData.listaIndicadores.Add(IndiceForcaRelativaComparada);

                #endregion

                #region Índice de Performance

                IndicatorInfoDTO IndicePerformance = new IndicatorInfoDTO();
                IndicePerformance.Ajuda = @"O indicador Performance calcula a performance dos preços como um valor de porcentagem normalizado. 
O indicador Performance mostra o preço de um ativo como um valor normalizado. Se o indicador mostra o valor 50, então o preço do ativo subiu 50% desde o início do cálculo do indicador. Ao contrário, se o indicador mostra -50, então o preço do ativo caiu 50% desde o início do cálculo do indicador.";
                
                IndicePerformance.NomePortugues = "Índice de Performance";
                IndicePerformance.NovoPainel = true;
                IndicePerformance.TemSerieFilha1 = false;
                IndicePerformance.TemSerieFilha2 = false;
                IndicePerformance.StrokeColor = Colors.Red;
                IndicePerformance.StrokeThickness = 1;
                IndicePerformance.StrokeType = 0;
                IndicePerformance.TipoStockchart = IndicatorType.PerformanceIndex;
                IndicePerformance.Propriedades = GetPropriedadesIndicePerformance();
                StaticData.listaIndicadores.Add(IndicePerformance);

                #endregion

                #region Índice de Volume Negativo

                IndicatorInfoDTO IndiceVolumeNegativo = new IndicatorInfoDTO();
                IndiceVolumeNegativo.Ajuda = @"O Negative Volume Index destaca-se em períodos que o volume cai em relação a períodos anteriores. 
O Negative Volume Index (NVI) está focado em períodos que o volume é decrescente em relação a periodos anteriores. A premissa é que o ""dinheiro inteligente"" toma posições quando o volume cai. Este 'indice tenta determinar oque os traders experientes estão fazendo, observando o período em que o volume negociado é decrescente em relação a períodos anteriores, baseado na idéia de que volume elevado é sinal de traders inexperientes tomando posições. ";
                
                IndiceVolumeNegativo.NomePortugues = "Índice de Volume Negativo";
                IndiceVolumeNegativo.NovoPainel = true;
                IndiceVolumeNegativo.TemSerieFilha1 = false;
                IndiceVolumeNegativo.TemSerieFilha2 = false;
                IndiceVolumeNegativo.StrokeColor = Colors.Red;
                IndiceVolumeNegativo.StrokeThickness = 1;
                IndiceVolumeNegativo.StrokeType = 0;
                IndiceVolumeNegativo.TipoStockchart = IndicatorType.NegativeVolumeIndex;
                IndiceVolumeNegativo.Propriedades = GetPropriedadesIndiceVolumeNegativo();
                StaticData.listaIndicadores.Add(IndiceVolumeNegativo);

                #endregion

                #region Índice de Volume Positivo

                IndicatorInfoDTO IndiceVolumePositivo = new IndicatorInfoDTO();
                IndiceVolumePositivo.Ajuda = @"O PVI assumes que nos períodos que o volume cresce, as massas desinformadas de traders estão no mercado. Inversamente em períodos de volume decrescente, o ""dinheiro inteligente"" está sorrateiramente tomando posições. É usual traçar a média móvel do PVI de 255 dias. Quando o PVI cruzar acima da média móvel traders menos informados (as massas) estão comprando, indicando que o preço pode continuar a subir. Quando o indicador cruza abaixo da média móvel, os traders desinformados normalmente estão vendendo, indicando queda. ";
                IndiceVolumePositivo.NomePortugues = "Índice de Volume Positivo";
                IndiceVolumePositivo.NovoPainel = true;
                IndiceVolumePositivo.TemSerieFilha1 = false;
                IndiceVolumePositivo.TemSerieFilha2 = false;
                IndiceVolumePositivo.StrokeColor = Colors.Red;
                IndiceVolumePositivo.StrokeThickness = 1;
                IndiceVolumePositivo.StrokeType = 0;
                IndiceVolumePositivo.TipoStockchart = IndicatorType.PositiveVolumeIndex;
                IndiceVolumePositivo.Propriedades = GetPropriedadesIndiceVolumePositivo();
                StaticData.listaIndicadores.Add(IndiceVolumePositivo);

                #endregion

                #region Índice Estocástico

                IndicatorInfoDTO  IndiceEstocastico = new IndicatorInfoDTO();
                IndiceEstocastico.Ajuda = @"O Stochastic Momentum Index, desenvolvido por William Blau, publicado pela primeira vez na edição de Janeiro de 1993 da revista “Stocks & Commodities”. Este indicador plota a proximidade relativa das recentes máximas e mínimas. 
O Stochastic Momentum Index tem dois componentes: %K e %D. %K é normalmente mostrado como uma linha sólida e %D como uma linha pontilhada. O método mais amplamente usado para interpretar o Stochastic Momentum Index é comprar quando as duas linhas estão acima de 40 e vender quando as duas linhas caem abaixo de 40. Outro caminho para interpretar o Stochastic Momentum Index é comprar quando %K cruza acima de %D, e inversamente, vender quando %K cruza abaixo %D. É mais comum usar os períodos 13 para %K, 25 para %K smoothing, 2 para %K double smoothing, e 9 para %D. ";

                IndiceEstocastico.NomePortugues = "Índice Estocástico";
                IndiceEstocastico.NovoPainel = true;
                IndiceEstocastico.TemSerieFilha1 = true;
                IndiceEstocastico.TemSerieFilha2 = true;
                IndiceEstocastico.StrokeColor = Colors.Red;
                IndiceEstocastico.StrokeThickness = 1;
                IndiceEstocastico.StrokeType = 0;
                IndiceEstocastico.TipoStockchart = IndicatorType.StochasticMomentumIndex;
                IndiceEstocastico.Propriedades = GetPropriedadesIndiceEstocastico();
                StaticData.listaIndicadores.Add(IndiceEstocastico);

                #endregion

                #region Índice Swing

                IndicatorInfoDTO IndiceSwing = new IndicatorInfoDTO();
                IndiceSwing.Ajuda = @"O Swing Index é um indicador popular que mostra comparativamente a força de um preço de um ativo comparando o último preço, máximo, mínimo e preço de fechamento, com os mesmos valores anteriores. Este indicador momentum é usado inicialmente como um componente do Accumulative Swing Index. O Swing Index sozinho não proporciona exatamente sinais. Deve ser usado em conjunto com outros indicadores para melhor uso. 
O Swing Index é um componente do Accumulation Swing Index. ";
                
                IndiceSwing.NomePortugues = "Índice Swing";
                IndiceSwing.NovoPainel = true;
                IndiceSwing.TemSerieFilha1 = false;
                IndiceSwing.TemSerieFilha2 = false;
                IndiceSwing.StrokeColor = Colors.Red;
                IndiceSwing.StrokeThickness = 1;
                IndiceSwing.StrokeType = 0;
                IndiceSwing.TipoStockchart = IndicatorType.SwingIndex;
                IndiceSwing.Propriedades = GetPropriedadesIndiceSwing();
                StaticData.listaIndicadores.Add(IndiceSwing);

                #endregion

                #region Índice Trade Volume

                IndicatorInfoDTO IndiceTradeVolume = new IndicatorInfoDTO();
                IndiceTradeVolume.Ajuda = @"O Trade Volume index mostra se um ativo está acumulando ou distribuindo. (similar ao Accumulation/Distribution index). 
Quando o indicador sobe, o ativo está acumulando. Inversamente, quando o indicador cai, o ativo está sendo distribuído. Os preços podem apresentar uma reversão quando o indicador converge junto com o o preço. ";
                
                IndiceTradeVolume.NomePortugues = "Índice Trade Volume";
                IndiceTradeVolume.NovoPainel = true;
                IndiceTradeVolume.TemSerieFilha1 = false;
                IndiceTradeVolume.TemSerieFilha2 = false;
                IndiceTradeVolume.StrokeColor = Colors.Red;
                IndiceTradeVolume.StrokeThickness = 1;
                IndiceTradeVolume.StrokeType = 0;
                IndiceTradeVolume.TipoStockchart = IndicatorType.TradeVolumeIndex;
                IndiceTradeVolume.Propriedades   = GetPropriedadesIndiceTradeVolume();
                StaticData.listaIndicadores.Add(IndiceTradeVolume);

                #endregion

                //Pendente
                #region Keltner

                IndicatorInfoDTO Keltner = new IndicatorInfoDTO();
                Keltner.Ajuda = "Indicador usado para predizer a tendência do mercado. Uma sobrecompra ocorre quando os preços se movem acima da faixa superior, e uma sobrevenda ocorre quando os preços se movem abaixo da faixa mais baixa.";
                Keltner.NomePortugues = "Keltner";
                Keltner.NovoPainel = false;
                Keltner.TemSerieFilha1 = true;
                Keltner.TemSerieFilha2 = true;
                Keltner.StrokeColor = Colors.Red;
                Keltner.StrokeThickness = 1;
                Keltner.StrokeType = 0;
                Keltner.TipoStockchart = IndicatorType.KeltnerChannel;
                Keltner.Propriedades = GetPropriedadesKeltner();
                StaticData.listaIndicadores.Add(Keltner);

                #endregion

                #region MACD

                IndicatorInfoDTO MACD = new IndicatorInfoDTO();
                MACD.Ajuda = @"O MACD ('Moving Average Convergence/Divergenc') é um indicador momentum e seguidor de tendência que mostra a relação entre duas médias móveis do preço. O MACD usa médias móveis exponenciais, que são indicadores lagging, para incluir as características de um seguidor de tendência. Estes indicadores lagging se transformam em um oscilador momentum quando subtraímos a média móvel longa pela média móvel curta. O resultado plotado forma uma linha que oscila acima e abaixo de zero, sem limites superiores ou inferiores. O MACD é um oscilador centralizado. 
O MACD é a diferença entre as médias móveis exponenciais de períodos 26 e 12. Uma média móvel exponencial de 9 períodos, chamada sinal, é plotada sobre o MACD para mostrar sinais de compra/venda. 
As interpretações de Compra/Venda podem derivar de cruzamentos (calculados a partir dos Períodos estabelecidos como parâmetro), Níveis Sobre-Comprados/Sobre-Vendidos do MACD e divergências entre o indicador e os preços. ";

                MACD.NomePortugues = "MACD";
                MACD.NovoPainel = true;
                MACD.TemSerieFilha1 = true;
                MACD.TemSerieFilha2 = false;
                MACD.StrokeColor = Colors.Red;
                MACD.StrokeThickness = 1;
                MACD.StrokeType = 0;
                MACD.TipoStockchart = IndicatorType.MACD;
                MACD.Propriedades = GetPropriedadesMACD();
                StaticData.listaIndicadores.Add(MACD);

                #endregion

                #region MACD Histograma

                IndicatorInfoDTO MACDHistograma = new IndicatorInfoDTO();
                MACDHistograma.NovoPainel = true;
                MACDHistograma.Ajuda = "Histograma do MACD.";
                MACDHistograma.NomePortugues = "MACD Histograma";
                MACDHistograma.TemSerieFilha1 = false;
                MACDHistograma.TemSerieFilha2 = false;
                MACDHistograma.StrokeColor = Colors.Red;
                MACDHistograma.StrokeThickness = 1;
                MACDHistograma.StrokeType = 0;
                MACDHistograma.TipoStockchart = IndicatorType.MACDHistogram;
                MACDHistograma.Propriedades = GetPropriedadesMACDHistograma();
                StaticData.listaIndicadores.Add(MACDHistograma);

                #endregion

                #region Mass Índex

                IndicatorInfoDTO MassIndex = new IndicatorInfoDTO();
                MassIndex.Ajuda = @"O Mass Index é um oscilador que usa as mudanças nos preços e fornece previsões únicas de reversões no mercado, que outros indicadores podem ignorar. O Mass Index de Donald Dorsey's é usado para sinalizar uma reversão de tendência se aproximando. O índice é uma soma móvel de 25 períodos de duas médias móveis. A primeira média móvel é uma média móvel exponencial suavizada do preço de fechamento, já a segunda média móvel é a primeira suavizada uma segunda vez. Usando 25 períodos, valores acima de 25 indicam uma grande variação, enquanto valores abaixo de 25 indicam pequena variação. 
De acordo com o desenvolvedor des índice, reversões podem ocorrer quando o indicador com um período configurado de 25, por exemplo, sobe acima de 27 e cai abaixo de 26,5.";

                MassIndex.NovoPainel = true;
                MassIndex.NomePortugues = "Mass Índex";
                MassIndex.TemSerieFilha1 = false;
                MassIndex.TemSerieFilha2 = false;
                MassIndex.StrokeColor = Colors.Red;
                MassIndex.StrokeThickness = 1;
                MassIndex.StrokeType = 0;
                MassIndex.TipoStockchart = IndicatorType.MassIndex;
                MassIndex.Propriedades = GetPropriedadesMassIndex();
                StaticData.listaIndicadores.Add(MassIndex);

                #endregion

                #region Média Móvel Exponencial(EMA)

                IndicatorInfoDTO MediaMoveExponencial = new IndicatorInfoDTO();
                MediaMoveExponencial.Ajuda = @"Uma Exponential Moving Average é similar a uma SMA. Uma MME é calculada aplicando uma pequena porcentagem do valor atual ao valor anterior. Aplicando maior peso ao valor atual. A média móvel exponencial reduz o atraso ao aplicar mais peso ao preços mais recentes em relação aos dados anteriores. Portando irá reagir mais rapidamente à alteração de preços que uma média móvel simples. 
Uma Média Móvel é frequentemente usada como uma representação suave do movimento do preço ou do indicador.";

                MediaMoveExponencial.NovoPainel = false;
                MediaMoveExponencial.NomePortugues  = "Média Móvel Exponencial(EMA)";
                MediaMoveExponencial.TemSerieFilha1 = false;
                MediaMoveExponencial.TemSerieFilha2 = false;
                MediaMoveExponencial.StrokeColor = Colors.Red;
                MediaMoveExponencial.StrokeThickness = 1;
                MediaMoveExponencial.StrokeType = 0;
                MediaMoveExponencial.TipoStockchart = IndicatorType.ExponentialMovingAverage;
                MediaMoveExponencial.Propriedades   = GetPropriedadesMediaMoveExponencial();
                StaticData.listaIndicadores.Add(MediaMoveExponencial);

                #endregion

                #region Média Móvel Ponderada

                IndicatorInfoDTO MediaMovelPonderada = new IndicatorInfoDTO();
                MediaMovelPonderada.NovoPainel = false;
                MediaMovelPonderada.Ajuda = "O Weighted Moving Average, ou Média Móvel Ponderada, coloca mais peso em valores recentes e menos peso em valores mais antigos. ";
                MediaMovelPonderada.NomePortugues = "Média Móvel Ponderada";
                MediaMovelPonderada.TemSerieFilha1 = false;
                MediaMovelPonderada.TemSerieFilha2 = false;
                MediaMovelPonderada.StrokeColor = Colors.Red;
                MediaMovelPonderada.StrokeThickness = 1;
                MediaMovelPonderada.StrokeType = 0;
                MediaMovelPonderada.TipoStockchart = IndicatorType.WeightedMovingAverage;
                MediaMovelPonderada.Propriedades = GetPropriedadesMediaMovelPonderada();
                StaticData.listaIndicadores.Add(MediaMovelPonderada);

                #endregion

                #region Média Móvel Simples (SMA)

                IndicatorInfoDTO MediaMovelSimples = new IndicatorInfoDTO();
                MediaMovelSimples.Ajuda = @"O indicador é o mais usado e facilita marcar tendências, suavizando uma série. Também é matéria prima de inúmeros outros indicadores e usado em conjunto com outros indicadores para formar sistemas e encontrar pontos de compra/venda. 
Mas o fato é que médias móveis são indicadores 'lagging' e portanto estarão sempre 'atrás' dos preços. Quando os preços apresentam tendência, este e outros indicadores lagging funcionam muito bem, quando a tendência não está definida podem apresentar muitos falsos sinais. ";
                MediaMovelSimples.NovoPainel = false;
                MediaMovelSimples.NomePortugues  = "Média Móvel Simples (SMA)";
                MediaMovelSimples.TemSerieFilha1 = false;
                MediaMovelSimples.TemSerieFilha2 = false;
                MediaMovelSimples.StrokeColor = Colors.Red;
                MediaMovelSimples.StrokeThickness = 1;
                MediaMovelSimples.StrokeType = 0;
                MediaMovelSimples.TipoStockchart = IndicatorType.SimpleMovingAverage;
                MediaMovelSimples.Propriedades   = GetPropriedadesSMA();
                StaticData.listaIndicadores.Add(MediaMovelSimples);

                #endregion

                #region Média Móvel Tempo Séries

                IndicatorInfoDTO MediaMovelTempoSeries = new IndicatorInfoDTO();
                MediaMovelTempoSeries.Ajuda = @"O Time Series Moving Average é similar a uma média móvel simples, exceto que seus valores são derivados de uma regressão linear no lugar de valores brutos. 
A Média Móvel é mais usada para conseguir uma média que mostra uma representação mais suave do preço ou de um indicador qualquer. ";

                MediaMovelTempoSeries.NovoPainel = false;
                MediaMovelTempoSeries.NomePortugues = "Média Móvel Tempo Séries";
                MediaMovelTempoSeries.TemSerieFilha1 = false;
                MediaMovelTempoSeries.TemSerieFilha2 = false;
                MediaMovelTempoSeries.StrokeColor = Colors.Red;
                MediaMovelTempoSeries.StrokeThickness = 1;
                MediaMovelTempoSeries.StrokeType = 0;
                MediaMovelTempoSeries.TipoStockchart = IndicatorType.TimeSeriesMovingAverage;
                MediaMovelTempoSeries.Propriedades = GetPropriedadesMediaMovelTempoSeries();
                StaticData.listaIndicadores.Add(MediaMovelTempoSeries);

                #endregion

                #region Média Móvel Triangular

                IndicatorInfoDTO  MediaMovelTriangular = new IndicatorInfoDTO();
                MediaMovelTriangular.Ajuda = @"A Média Móvel Triangular é similar à Média Móvel Simples, exceto que é dado um maior peso ao preço no período médio da média móvel. 
A Média Móvel é mais usada para conseguir uma média que mostra uma representação mais suave do preço ou de um indicador qualquer. ";

                MediaMovelTriangular.NovoPainel = false;
                MediaMovelTriangular.NomePortugues = "Média Móvel Triangular";
                MediaMovelTriangular.TemSerieFilha1 = false;
                MediaMovelTriangular.TemSerieFilha2 = false;
                MediaMovelTriangular.StrokeColor = Colors.Red;
                MediaMovelTriangular.StrokeThickness = 1;
                MediaMovelTriangular.StrokeType = 0;
                MediaMovelTriangular.TipoStockchart = IndicatorType.TriangularMovingAverage;
                MediaMovelTriangular.Propriedades = GetPropriedadesMediaMovelTriangula();
                StaticData.listaIndicadores.Add(MediaMovelTriangular);

                #endregion

                #region Média Móvel Variável

                IndicatorInfoDTO MediaMovelVariavel = new IndicatorInfoDTO();
                MediaMovelVariavel.Ajuda = @"A Média Móvel Variável é uma média móvel exponencial que se ajusta à volatilidade. 
A Média Móvel é mais usada para conseguir uma média que mostra uma representação mais suave do preço ou de um indicador qualquer. ";

                MediaMovelVariavel.NovoPainel = false;
                MediaMovelVariavel.NomePortugues = "Média Móvel Variável";
                MediaMovelVariavel.TemSerieFilha1 = false;
                MediaMovelVariavel.TemSerieFilha2 = false;
                MediaMovelVariavel.StrokeColor = Colors.Red;
                MediaMovelVariavel.StrokeThickness = 1;
                MediaMovelVariavel.StrokeType = 0;
                MediaMovelVariavel.TipoStockchart = IndicatorType.VariableMovingAverage;
                MediaMovelVariavel.Propriedades = GetPropriedadesMediaMovelVariavel();
                StaticData.listaIndicadores.Add(MediaMovelVariavel);

                #endregion

                #region Média Móvel VIDYA

                IndicatorInfoDTO MediaMovelVIDYA = new IndicatorInfoDTO();
                MediaMovelVIDYA.Ajuda = @"VIDYA (Volatility Index Dynamic Average), é uma média móvel derivada de uma regressão linear R2. 
A Média Móvel é mais usada para conseguir uma média que mostra uma representação mais suave do preço ou de um indicador qualquer. Como o VIDYA é derivado de uma regressão linear, rapidamente se adapta à volatilidade. ";

                MediaMovelVIDYA.NovoPainel = false;
                MediaMovelVIDYA.NomePortugues = "Média Móvel VIDYA";
                MediaMovelVIDYA.TemSerieFilha1 = true;
                MediaMovelVIDYA.TemSerieFilha2 = true;
                MediaMovelVIDYA.StrokeColor = Colors.Red;
                MediaMovelVIDYA.StrokeThickness = 1;
                MediaMovelVIDYA.StrokeType = 0;
                MediaMovelVIDYA.TipoStockchart = IndicatorType.VIDYA;
                MediaMovelVIDYA.Propriedades = GetPropriedadesMediaMovelVIDYA();
                StaticData.listaIndicadores.Add(MediaMovelVIDYA);

                #endregion

                #region Movimento Direcional

                IndicatorInfoDTO MovimentoDirecional = new IndicatorInfoDTO();
                MovimentoDirecional.Ajuda = @"O Directional Movement System de Welles Wilder contém 5 indicadores: ADX, DI+, DI-, DX e ADXR. 
O ADX (Average Directional Movement Index) é um indicador de o quanto de tendência apresenta o Mercado. Tanto para alta como para baixa. Quanto mais alto a linha ADX mais o Mercado apresenta tendência definida e mais sucetível a uma estratégia que acompanhe uma tendência. O indicador consiste de duas linhas: DI+ e DI-, a primeira mede tendencies de alta e a segunda tendências de baixa. 
O valor padrão do Directional Movement System é um período de 14 dias para DI+ e 14 dias para DI- no mesmo gráfico. ADX é também mostrado algumas vezes no mesmo gráfico. 
Um sinal de compra é dado quando DI+ cruza acima do DI-, e o sinal de compra quando DI- cruza acima do DI+. ";

                MovimentoDirecional.NovoPainel = true;
                MovimentoDirecional.NomePortugues = "Movimento Direcional";
                MovimentoDirecional.TemSerieFilha1 = true;
                MovimentoDirecional.TemSerieFilha2 = true;
                MovimentoDirecional.StrokeColor = Colors.Red;
                MovimentoDirecional.StrokeThickness = 1;
                MovimentoDirecional.StrokeType = 0;
                MovimentoDirecional.TipoStockchart = IndicatorType.DirectionalMovementSystem;
                MovimentoDirecional.Propriedades = GetPropriedadesMovimentoDirecional();
                StaticData.listaIndicadores.Add(MovimentoDirecional);

                #endregion

                #region On Balance Volume (OBV)

                IndicatorInfoDTO OnBalanceVolume = new IndicatorInfoDTO();
                OnBalanceVolume.Ajuda = @"O indicador On Balance Volume geralmente precede movimentos de preços, partindo da premissa que investidores bem informados compram quando o índice cai e que investidores desinformados compram quando o índice sobe. 
O Volume é o número de contratos ou cotas que estão trocando de proprietários em um período de tempo. O On Balance Volume ('OBV') é um indicador de momentum que relaciona o volume com a alteração do preço. Portanto é um bom indicador de oferta e demanda indepentente do preço e que pode relatar o entusiasmo relativo de comprados e vendidos no mercado. 
O On Balance Volume é um indicador desenhado para rastrear mudanças no volume durante a passagem do tempo. Supoe-se que mudanças no volume precedem mudanças na tendência do preço. O OBV foi criado por Joseph Granville e tem algumas qualidades interpretativas, deve ser usado em conjunto com outros indicadores de reversão de tendência. 
O OBV mostra se o volume (dinheiro) está entrando ou saindo do ativo. Quando o ativo fecha mais alto que o fechamento anterior, o volume é considerado volume de alta. Quando o ativo fecha abaixo do fechamento anterior, então é considerado volume de queda.";

                OnBalanceVolume.NovoPainel = true;
                OnBalanceVolume.NomePortugues = "On Balance Volume (OBV)";
                OnBalanceVolume.TemSerieFilha1 = false;
                OnBalanceVolume.TemSerieFilha2 = false;
                OnBalanceVolume.StrokeColor = Colors.Red;
                OnBalanceVolume.StrokeThickness = 1;
                OnBalanceVolume.StrokeType = 0;
                OnBalanceVolume.TipoStockchart = IndicatorType.OnBalanceVolume;
                OnBalanceVolume.Propriedades = GetPropriedadesOnBalanceVolume();
                StaticData.listaIndicadores.Add(OnBalanceVolume);

                #endregion

                #region Oscilador Aroon

                IndicatorInfoDTO OsciladorAroon = new IndicatorInfoDTO();
                OsciladorAroon.Ajuda = @"Para o Aroon Oscillator, o valor positivo indica uma tendência altista (ou se aproximando de uma), e valores negativos representam uma tendência baixista. Quanto maior o valor do indicador, mais forte é a indicação da tendência.";

                OsciladorAroon.NovoPainel = true;
                OsciladorAroon.NomePortugues = "Oscilador Aroon";
                OsciladorAroon.TemSerieFilha1 = false;
                OsciladorAroon.TemSerieFilha2 = false;
                OsciladorAroon.StrokeColor = Colors.Red;
                OsciladorAroon.StrokeThickness = 1;
                OsciladorAroon.StrokeType = 0;
                OsciladorAroon.TipoStockchart = IndicatorType.AroonOscillator;
                OsciladorAroon.Propriedades = GetPropriedadesOsciladorAroon();
                StaticData.listaIndicadores.Add(OsciladorAroon);

                #endregion

                #region Oscilador Change Momentum

                IndicatorInfoDTO OsciladorChangeMomentum = new IndicatorInfoDTO();
                OsciladorChangeMomentum.Ajuda = @"O Chande Momentum Oscillator é um oscilador avançado de momentum (Força Cinetica) derivado de uma regressão linear. O indicador determina o momentum do preço comparando o tamanho da recente alteração de preço negativa com o tamanho da recente alteração de preço positiva. 
O valor resultante é normalizado entre -100 e 100 onde valores negativos indicam maior acumulação negativa na alteração dos preços e e valores positivos indicam o inverso. 
O movimento crescente do valor do CMO pode indicar um movimento forte altista do preço. Ao contrário, quedas no valor do CMO podem indicar um movimento forte baixista. CMO está relacionado ao MACD e ao Price Rate of Change (Preço ROC). ";

                OsciladorChangeMomentum.NovoPainel = true;
                OsciladorChangeMomentum.NomePortugues = "Oscilador Change Momentum";
                OsciladorChangeMomentum.TemSerieFilha1 = false;
                OsciladorChangeMomentum.TemSerieFilha2 = false;
                OsciladorChangeMomentum.StrokeColor = Colors.Red;
                OsciladorChangeMomentum.StrokeThickness = 1;
                OsciladorChangeMomentum.StrokeType = 0;
                OsciladorChangeMomentum.TipoStockchart = IndicatorType.ChandeMomentumOscillator;
                OsciladorChangeMomentum.Propriedades = GetPropriedadesOsciladorChangeMomentum();
                StaticData.listaIndicadores.Add(OsciladorChangeMomentum);

                #endregion

                #region Oscilador de Fractal Chaos

                IndicatorInfoDTO OsciladorFractalChaos = new IndicatorInfoDTO();
                OsciladorFractalChaos.Ajuda = @"O indicador procura no passado dependendo do número de períodos selecionado para plotar o indicador. 
O valor do indicador é um bom indicador de agitação ou tendência do mercado. Ele essencialmente retorna a 0 em mercados agitados e tem valor positivo ou negativo em mercados com tendência.";

                OsciladorFractalChaos.NovoPainel = true;
                OsciladorFractalChaos.NomePortugues = "Oscilador de Fractal Chaos";
                OsciladorFractalChaos.TemSerieFilha1 = false;
                OsciladorFractalChaos.TemSerieFilha2 = false;
                OsciladorFractalChaos.StrokeColor = Colors.Red;
                OsciladorFractalChaos.StrokeThickness = 1;
                OsciladorFractalChaos.StrokeType = 0;
                OsciladorFractalChaos.TipoStockchart = IndicatorType.FractalChaosOscillator;
                OsciladorFractalChaos.Propriedades = GetPropriedadesOsciladorFractalChaos();
                StaticData.listaIndicadores.Add(OsciladorFractalChaos);

                #endregion

                #region Oscilador de Preços

                IndicatorInfoDTO OsciladorPrecos = new IndicatorInfoDTO();
                OsciladorPrecos.Ajuda = @"O Price Oscillator ilustra sinais cíclicos e frequentemente lucrativos. O cruzamento do PO no valor 0 representa o cruzamento das duas médias móveis. As médias móveis tipicamente geram sinais de compra e venda no cruzamento da média longa com a curta e vice-versa. Calcular esta diferença em um oscilador pode dar sinais de sobre-compra/sobre-venda e uma média móvel calculada sobre o PO pode gerar sinais rápidos de entrada e saída de posições.";

                OsciladorPrecos.NovoPainel = true;
                OsciladorPrecos.NomePortugues = "Oscilador de Preços";
                OsciladorPrecos.TemSerieFilha1 = false;
                OsciladorPrecos.TemSerieFilha2 = false;
                OsciladorPrecos.StrokeColor = Colors.Red;
                OsciladorPrecos.StrokeThickness = 1;
                OsciladorPrecos.StrokeType = 0;
                OsciladorPrecos.TipoStockchart = IndicatorType.PriceOscillator;
                OsciladorPrecos.Propriedades = GetPropriedadesOsciladorPrecos();
                StaticData.listaIndicadores.Add(OsciladorPrecos);

                #endregion

                #region Oscilador de Volume

                IndicatorInfoDTO OsciladorVolume = new IndicatorInfoDTO();
                BollingerBands.Ajuda = @"O Volume Oscillator mostra o spread de duas diferentes médias móveis do volume durante um período específico de tempo. 
O Volume Oscillator oferece uma visão clara se o volume está aumentando ou não, ou diminuindo ou não.";

                OsciladorVolume.NovoPainel = true;
                OsciladorVolume.NomePortugues = "Oscilador de Volume";
                OsciladorVolume.TemSerieFilha1 = false;
                OsciladorVolume.TemSerieFilha2 = false;
                OsciladorVolume.StrokeColor = Colors.Red;
                OsciladorVolume.StrokeThickness = 1;
                OsciladorVolume.StrokeType = 0;
                OsciladorVolume.TipoStockchart = IndicatorType.VolumeOscillator;
                OsciladorVolume.Propriedades = GetPropriedadesOsciladorVolume();
                StaticData.listaIndicadores.Add(OsciladorVolume);

                #endregion

                #region Oscilador Detrended Price

                IndicatorInfoDTO OsciladorDetrendedPrice = new IndicatorInfoDTO();
                OsciladorDetrendedPrice.Ajuda = @"O indicador ajuda a determinar ciclos e níveis de sobre-comprado/vendido. Pode ser usado para identificar pontos críticos no longo prazo, em conjunto com outros indicadores ou ainda como fonte para calcular novos indicadores. ";

                OsciladorDetrendedPrice.NovoPainel = true;
                OsciladorDetrendedPrice.NomePortugues = "Oscilador Detrended Price";
                OsciladorDetrendedPrice.TemSerieFilha1 = false;
                OsciladorDetrendedPrice.TemSerieFilha2 = false;
                OsciladorDetrendedPrice.StrokeColor = Colors.Red;
                OsciladorDetrendedPrice.StrokeThickness = 1;
                OsciladorDetrendedPrice.StrokeType = 0;
                OsciladorDetrendedPrice.TipoStockchart = IndicatorType.DetrendedPriceOscillator;
                OsciladorDetrendedPrice.Propriedades = GetPropriedadesOsciladorDetrendedPrice();
                StaticData.listaIndicadores.Add(OsciladorDetrendedPrice);

                #endregion

                #region Oscilador Estocástico

                IndicatorInfoDTO OsciladorEstocastico = new IndicatorInfoDTO();
                OsciladorEstocastico.Ajuda = @"O Stochastic Oscillator (Oscilador Estocástico) é um indicador momentum que mostra a localização do preço de fechamento atual em relação ao range da Máxima/Mínima durante um dado período. Valores próximos ao topo do range indicam acumulação (pressão compradora) enquanto valores próximos ao fundo do range demonstram distribuição (pressão vendedora). ";

                OsciladorEstocastico.NovoPainel = true;
                OsciladorEstocastico.NomePortugues = "Oscilador Estocástico";
                OsciladorEstocastico.TemSerieFilha1 = true;
                OsciladorEstocastico.TemSerieFilha2 = false;
                OsciladorEstocastico.StrokeColor = Colors.Red;
                OsciladorEstocastico.StrokeThickness = 1;
                OsciladorEstocastico.StrokeType = 0;
                OsciladorEstocastico.TipoStockchart = IndicatorType.StochasticOscillator;
                OsciladorEstocastico.Propriedades   = GetPropriedadesOsciladorEstocastico();
                StaticData.listaIndicadores.Add(OsciladorEstocastico);

                #endregion

                #region Oscilador Momentum

                IndicatorInfoDTO OsciladorMomentum = new IndicatorInfoDTO();
                OsciladorMomentum.Ajuda = @"O indicador Momentum mede o montante de alteração de um preço em um dado período. Ele mostra o PRICE ROC de um preço porém como uma taxa. É usado exatamente o mesmo cálculo, porém no PRICE ROC é mostrado como porcentagem. Este oscilador momentum mede a velocidade do movimento direcional dos preços. Quando o preço sobem até determinado ponto o mercado é considerado sobre-comprado e no contrário sobre-vendido. Em ambos os casos, a reação ou reversão é iminente. A inclinação do momentum é diretamente proporcional à velocidade do movimento. A distância percorrida acima ou abaixo pelo indicador é proporcional ao tamanho do movimento. O momentum é normalmente caracterizado em um gráfico de duas dimensões. O eixo Y (vertical) representa a magnitude ou a distância do movimento e o eixo X (horizontal) representa o tempo. Desenhado desta maneira o indicador é caracterizado pelo fato de mover-se rapidamente para pontos de reversão do mercado e diminuir a velocidade quando o mercado continua na mesma direção. 
O Aumento do valor do oscilador Momentum pode indicar um movimento forte altista dos preços. O Momentum está relacionado com o MACD e o Price Rate of Change (ROC). ";

                OsciladorEstocastico.NovoPainel = true;
                OsciladorMomentum.NomePortugues = "Oscilador Momentum";
                OsciladorMomentum.TemSerieFilha1 = false;
                OsciladorMomentum.TemSerieFilha2 = false;
                OsciladorMomentum.StrokeColor = Colors.Red;
                OsciladorMomentum.StrokeThickness = 1;
                OsciladorMomentum.StrokeType = 0;
                OsciladorMomentum.TipoStockchart = IndicatorType.MomentumOscillator;
                OsciladorMomentum.Propriedades = GetPropriedadesOsciladorMomentum();
                StaticData.listaIndicadores.Add(OsciladorMomentum);

                #endregion

                #region Oscilador Números Primos

                IndicatorInfoDTO OsciladorNumerosPrimos = new IndicatorInfoDTO();
                OsciladorNumerosPrimos.Ajuda = @"O prime numbers oscillator (Oscilador de Números Primos) foi desenvolvido pela Modulus Financial Engineering, Inc. Este indicador busca o número primo mais próximo tanto de um extremo quanto de outro da série, e plota a diferença entre o número primo e a série em si. 
Este indicador pode ser usado para encontrar pontos de reversão no mercado. Quando o oscilador permanence no mesmo ponto de alta em dois períodos consecutivos e no campo positivo, considere um sinal de venda. Inversamente, quando o oscilador permanence por dois períodos consecutivos em um ponto de baixa e com valor negativo, então considere um sinal de compra. ";

                OsciladorNumerosPrimos.NovoPainel = true;
                OsciladorNumerosPrimos.NomePortugues = "Oscilador Números Primos";
                OsciladorNumerosPrimos.TemSerieFilha1 = false;
                OsciladorNumerosPrimos.TemSerieFilha2 = false;
                OsciladorNumerosPrimos.StrokeColor = Colors.Red;
                OsciladorNumerosPrimos.StrokeThickness = 1;
                OsciladorNumerosPrimos.StrokeType = 0;
                OsciladorNumerosPrimos.TipoStockchart = IndicatorType.PrimeNumberOscillator;
                OsciladorNumerosPrimos.Propriedades = GetPropriedadesOsciladorNumerosPrimos();
                StaticData.listaIndicadores.Add(OsciladorNumerosPrimos);

                #endregion

                #region Oscilador Rainbow

                IndicatorInfoDTO OsciladorRainbow = new IndicatorInfoDTO();
                OsciladorRainbow.Ajuda = @"O Rainbow Oscillator é baseado em múltiplos time frames de uma média móvel. Quando os valores estão acima de 80 ou abaixo de 20 (ou a 80% / 20% do range), pode ocorrer uma reversão repentina na tendência. 
É usado para sinais de compra/venda, bem como para o reconhecimento de condições sobre-compradas / sobre-vendidas. ";

                OsciladorRainbow.NovoPainel = true;
                OsciladorRainbow.NomePortugues = "Oscilador Rainbow";
                OsciladorRainbow.TemSerieFilha1 = false;
                OsciladorRainbow.TemSerieFilha2 = false;
                OsciladorRainbow.StrokeColor = Colors.Red;
                OsciladorRainbow.StrokeThickness = 1;
                OsciladorRainbow.StrokeType = 0;
                OsciladorRainbow.TipoStockchart = IndicatorType.BollingerBands;
                OsciladorRainbow.Propriedades = GetPropriedadesBollingerBands();
                StaticData.listaIndicadores.Add(OsciladorRainbow);

                #endregion

                #region Oscilador Ultimate

                IndicatorInfoDTO OsciladorUltimate = new IndicatorInfoDTO();
                OsciladorUltimate.Ajuda = @"Como um leading, o objetivo do oscilador é prever o movimento futuro, e não dizer o que aconteceu com os preços (como fazem os seguidores de tendência - lagging). Os osciladores fazem isso baseado no suposto que um mercado sobre-vendido tem de subir, considerando que um mercado sobre-vendido sofre pressão de compra, e vice-versa. ";

                OsciladorUltimate.NovoPainel = true;
                OsciladorUltimate.NomePortugues = "Oscilador Ultimate";
                OsciladorUltimate.TemSerieFilha1 = false;
                OsciladorUltimate.TemSerieFilha2 = false;
                OsciladorUltimate.StrokeColor = Colors.Red;
                OsciladorUltimate.StrokeThickness = 1;
                OsciladorUltimate.StrokeType = 0;
                OsciladorUltimate.TipoStockchart = IndicatorType.UltimateOscillator;
                OsciladorUltimate.Propriedades = GetPropriedadesOsciladorUltimate();
                StaticData.listaIndicadores.Add(OsciladorUltimate);

                #endregion

                #region Preço-ROC

                IndicatorInfoDTO PrecoROC = new IndicatorInfoDTO();
                PrecoROC.Ajuda = @"O ROC é um indicador momentum que mede a velocidade e fornece pistas da ação dos preços. Se os preços sobem, o ROC sobe, se os preços caem, o ROC declina. Quanto maior a mudança nos preços, maior a mudança no indicador. O período usado para calcular o indicador pode variar entre 1 e 200 períodos (ou mais). 
Os períodos mais usados são 12 e 25 períodos. Quanto maior o ROC, mais sobre-comprado está o ativo, ou vice-versa. Entretanto, como qualquer indicador de sobre-comprado/sobre-vendido, é prudente esperar o sinal que vai começar a correção antes de montar ou desmontar uma posição. Um mercado que se mostra sobre-comprado pode permanecer nesta condição por algum tempo. De fato, níveis extremos de sobre-compra/sobre-venda tendem a permanecer continuando a tendência. 
O ROC de 12 períodos tende a ser muito cíclico, oscilando regularmente em ciclos. Por vezes, a alteração do preço pode ser antecipada por estudar ciclos anteriores do ROC e relacionando o ciclo anterior com o mercado atual. ";

                PrecoROC.NovoPainel = true;
                PrecoROC.NomePortugues = "Preço-ROC";
                PrecoROC.TemSerieFilha1 = false;
                PrecoROC.TemSerieFilha2 = false;
                PrecoROC.StrokeColor = Colors.Red;
                PrecoROC.StrokeThickness = 1;
                PrecoROC.StrokeType = 0;
                PrecoROC.TipoStockchart = IndicatorType.PriceROC;
                PrecoROC.Propriedades = GetPropriedadesPrecoROC();
                StaticData.listaIndicadores.Add(PrecoROC);

                #endregion

                #region Preço Médio

                IndicatorInfoDTO PrecoMedio = new IndicatorInfoDTO();
                PrecoMedio.Ajuda = @"O Preço Médio é simplesmente uma média dos valores máximos e mínimos durante um período. 
O Preço Médio é frequentemente usado como uma alternativa para visualizar o comportamento do preço, bem como um componente usado no cálculo de outros indicadores. ";

                PrecoMedio.NovoPainel = false;
                PrecoMedio.NomePortugues = "Preço Médio";
                PrecoMedio.TemSerieFilha1 = false;
                PrecoMedio.TemSerieFilha2 = false;
                PrecoMedio.StrokeColor = Colors.Red;
                PrecoMedio.StrokeThickness = 1;
                PrecoMedio.StrokeType = 0;
                PrecoMedio.TipoStockchart = IndicatorType.Median;
                PrecoMedio.Propriedades = GetPropriedadesPrecoMedio();
                StaticData.listaIndicadores.Add(PrecoMedio);

                #endregion

                #region Regressão Linear-Forecast

                IndicatorInfoDTO RegressaoLinearForecast = new IndicatorInfoDTO();
                RegressaoLinearForecast.Ajuda = @"O indicador Regressão Linear Forecast pode ser usado para suavisar o preço, realizar a regressão nos resultados, prever a linha de regressão se desejado, e então pode criar bandas de desvio padrão acima e abaixo da linha de regressão. 
É usado como uma linha de tendência estatística para o dado período, bem como encontrar estatísticamente níveis de suporte e resistência. ";

                RegressaoLinearForecast.NovoPainel = false;
                RegressaoLinearForecast.NomePortugues = "Regressão Linear-Forecast";
                RegressaoLinearForecast.TemSerieFilha1 = false;
                RegressaoLinearForecast.TemSerieFilha2 = false;
                RegressaoLinearForecast.StrokeColor = Colors.Red;
                RegressaoLinearForecast.StrokeThickness = 1;
                RegressaoLinearForecast.StrokeType = 0;
                RegressaoLinearForecast.TipoStockchart = IndicatorType.LinearRegressionForecast;
                RegressaoLinearForecast.Propriedades = GetPropriedadesRegressaoLinearForecast();
                StaticData.listaIndicadores.Add(RegressaoLinearForecast);

                #endregion

                #region Regressão Linear-Intercept

                IndicatorInfoDTO RegressaoLinearIntercept = new IndicatorInfoDTO();
                RegressaoLinearIntercept.Ajuda = "Pode ser usado para gerar sinais nos cruzamentos, bem como para compor outros indicadores e ainda ser um gerador de sinais de STOP. ";
                RegressaoLinearIntercept.NomePortugues = "Regressão Linear-Intercept";
                RegressaoLinearIntercept.NovoPainel = false;
                RegressaoLinearIntercept.TemSerieFilha1 = false;
                RegressaoLinearIntercept.TemSerieFilha2 = false;
                RegressaoLinearIntercept.StrokeColor = Colors.Red;
                RegressaoLinearIntercept.StrokeThickness = 1;
                RegressaoLinearIntercept.StrokeType = 0;
                RegressaoLinearIntercept.TipoStockchart = IndicatorType.LinearRegressionIntercept;
                RegressaoLinearIntercept.Propriedades = GetPropriedadesRegressaoLinearIntercept();
                StaticData.listaIndicadores.Add(RegressaoLinearIntercept);

                #endregion

                #region Regressão Linear-Raiz Quadrada

                IndicatorInfoDTO RegressaoLinearRaizQuadrada = new IndicatorInfoDTO();
                RegressaoLinearRaizQuadrada.Ajuda = @"A Regressão Linear é um método estatístico comum usado para prever valores usando os últimos ajustes de valores. Em Estatística, a regressão linear é um método de estimar o valor condicional esperado de uma variável Y fornecendo o valor de outra variável qualquer ou variável X. Há 4 tipos de Regressão Linear. 
A Regressão Linear Raiz-Quadrada determina a extensão de um relacionamento linear de um campo com o tempo durante um dado período ao quadrado, mostrando a força de uma tendência. Quanto mais próximo o preço se move em uma relação linear com a passagem do tempo, mais forte é a tendência. 
Os valores da Raiz-Quadrada mostram a porcentagem de movimento que pode ser explicada por uma regressão linear. Por exemplo, se a raiz-quadrada com 20 períodos está a 70%, significa que 70% do movimento do ativo é explicado por uma regressão linear. Os 30% restantes são inexplicáveis ruidos aleatórios. 
Um dos mais úteis métodos de usar a Raiz-Quadrada é como um indicador para confirmação. Indicadores de momentum (Ex.Estocastico, IFR, CCI, etc.) e médias móveis precisam de uma confirmação de tendência para apresentarem resultados consistentes. A Raiz-Quadrada fornece um meio de medir a tendência de preços.";

                RegressaoLinearRaizQuadrada.NovoPainel = true;
                RegressaoLinearRaizQuadrada.NomePortugues = "Regressão Linear-Raiz Quadrada";
                RegressaoLinearRaizQuadrada.TemSerieFilha1 = false;
                RegressaoLinearRaizQuadrada.TemSerieFilha2 = false;
                RegressaoLinearRaizQuadrada.StrokeColor = Colors.Red;
                RegressaoLinearRaizQuadrada.StrokeThickness = 1;
                RegressaoLinearRaizQuadrada.StrokeType = 0;
                RegressaoLinearRaizQuadrada.TipoStockchart = IndicatorType.LinearRegressionRSquared;
                RegressaoLinearRaizQuadrada.Propriedades = GetPropriedadesRegressaoLinearRaizQuadrada();
                StaticData.listaIndicadores.Add(RegressaoLinearRaizQuadrada);

                #endregion

                #region Regressão Linear-Slope

                IndicatorInfoDTO RegressaoLinearSlope = new IndicatorInfoDTO();
                RegressaoLinearSlope.Ajuda = @"O Slope mostra o quanto é esperado que se altere o preço por unidade de tempo. O Slope informa a direção geral da tendência (positiva ou negativa), enquanto a Raiz-quadrada informa a força da tendência. Um valor alto de raiz-quadrada pode ser associado com um valor alto (positivo ou negativo) do Slope. 
Quando o Slope começa a se tornar significantemente positivo, você poderá montar uma posição comprada. Você pode vender, ou montar uma posição vendida quando o Slope começara se tornar significaticamente negativo. 
Você pode também considerar montar uma posição no curto prazo oposta à tendência principal quando você observar Slope se mantendo em níveis extremos. Por exemplo, se o Slope está em um nível relativamente alto e começa a cair, você pode considerar vender ou montar uma posição vendida. 
O indicador Regressão Linear Slope fornece a inclinação de cada barra sobre a linha de regressão teorica, que esta barra e a anteriores N-1 barras (N sendo o período de regressão).";

                RegressaoLinearSlope.NovoPainel = true;
                RegressaoLinearSlope.NomePortugues = "Regressão Linear-Slope";
                RegressaoLinearSlope.TemSerieFilha1 = false;
                RegressaoLinearSlope.TemSerieFilha2 = false;
                RegressaoLinearSlope.StrokeColor = Colors.Red;
                RegressaoLinearSlope.StrokeThickness = 1;
                RegressaoLinearSlope.StrokeType = 0;
                RegressaoLinearSlope.TipoStockchart = IndicatorType.LinearRegressionSlope;
                RegressaoLinearSlope.Propriedades = GetPropriedadesRegressaoLinearSlope();
                StaticData.listaIndicadores.Add(RegressaoLinearSlope);

                #endregion

                #region SAR Parabólico(PSAR)

                IndicatorInfoDTO SARParabolico = new IndicatorInfoDTO();
                SARParabolico.Ajuda = @"O Parabolic SAR fornece excelentes pontos de saída de uma posição. Deve-se fechar uma posição comprada quando o preço cai abaixo do SAR e fechar uma posição vendida quando o preço cruzar acima do SAR. ";

                SARParabolico.NovoPainel = false;
                SARParabolico.NomePortugues = "SAR Parabólico(PSAR)";
                SARParabolico.TemSerieFilha1 = false;
                SARParabolico.TemSerieFilha2 = false;
                SARParabolico.StrokeColor = Colors.Red;
                SARParabolico.StrokeThickness = 1;
                SARParabolico.StrokeType = 0;
                SARParabolico.TipoStockchart = IndicatorType.ParabolicSAR;
                SARParabolico.Propriedades = GetPropriedadesSARParabolico();
                StaticData.listaIndicadores.Add(SARParabolico);

                #endregion

                //Pendente
                #region Tendência Preço/Volume

                //IndicatorInfoDTO TendenciaPrecoVolume = new IndicatorInfoDTO();
                //TendenciaPrecoVolume.Ajuda = "";

                //TendenciaPrecoVolume.NovoPainel = true;
                //TendenciaPrecoVolume.NomePortugues = "Tendência Preço/Volume";
                //TendenciaPrecoVolume.TemSerieFilha1 = false;
                //TendenciaPrecoVolume.TemSerieFilha2 = false;
                //TendenciaPrecoVolume.StrokeColor = Colors.Red;
                //TendenciaPrecoVolume.StrokeThickness = 1;
                //TendenciaPrecoVolume.StrokeType = 0;
                //TendenciaPrecoVolume.TipoStockchart = IndicatorType.PriceVolumeTrend;
                ////TendenciaPrecoVolume.Propriedades = GetPropriedadesTendenciaPrecoVolume();
                //StaticData.listaIndicadores.Add(TendenciaPrecoVolume);

                #endregion

                #region TRIX

                IndicatorInfoDTO TRIX = new IndicatorInfoDTO();
                TRIX.Ajuda = @"TRIX (Triple moving Average - ou média móvel tripla) é um oscilador momentum que mostra a taxa de variação do preço de fechamento aplicando a média exponência tripla. 
A interpretação mais comum do TRIX é comprar quando o oscilador subir e vender quando cair. Os períodos 3, 8 e 14 são frequentemente usados para suavisar o oscilador TRIX. ";

                TRIX.NovoPainel = true;
                TRIX.NomePortugues = "TRIX";
                TRIX.TemSerieFilha1 = false;
                TRIX.TemSerieFilha2 = false;
                TRIX.TipoStockchart = IndicatorType.TRIX;
                TRIX.StrokeColor = Colors.Red;
                TRIX.StrokeThickness = 1;
                TRIX.StrokeType = 0;
                TRIX.Propriedades = GetPropriedadesTRIX();
                StaticData.listaIndicadores.Add(TRIX);

                #endregion

                #region True Range

                IndicatorInfoDTO TrueRange = new IndicatorInfoDTO();
                TrueRange.Ajuda = @"O True Range (Wilder) mede a volatilidade do Mercado. 
Altos valores de TR podem significar um fundo/topo do mercado (gráfico) e baixos valores de TR podem significar mercado neutro. ";

                TrueRange.NovoPainel = true;
                TrueRange.NomePortugues = "True Range";
                TrueRange.TemSerieFilha1 = false;
                TrueRange.TemSerieFilha2 = false;
                TrueRange.StrokeColor = Colors.Red;
                TrueRange.StrokeThickness = 1;
                TrueRange.StrokeType = 0;
                TrueRange.TipoStockchart = IndicatorType.TrueRange;
                TrueRange.Propriedades = GetPropriedadesTrueRange();
                StaticData.listaIndicadores.Add(TrueRange);

                #endregion

                #region Typical Price

                IndicatorInfoDTO TypicalPrice = new IndicatorInfoDTO();
                TypicalPrice.Ajuda = @"O Typical Price é simplesmente uma média de um período sobre os valores máximo, mínimo e de fechamento. 
Typical Price é normalmente usado como uma alternativa para visualizar o comportamento do preço, bem como um componente para calcular outros indicadores.";

                TypicalPrice.NovoPainel = false;
                TypicalPrice.NomePortugues = "Typical Price";
                TypicalPrice.TemSerieFilha1 = false;
                TypicalPrice.TemSerieFilha2 = false;
                TypicalPrice.StrokeColor = Colors.Red;
                TypicalPrice.StrokeThickness = 1;
                TypicalPrice.StrokeType = 0;
                TypicalPrice.TipoStockchart = IndicatorType.TypicalPrice;
                TypicalPrice.Propriedades = GetPropriedadesTypicalPrice();
                StaticData.listaIndicadores.Add(TypicalPrice);

                #endregion
                
                #region Volatilidade Histórica

                IndicatorInfoDTO VolatilidadeHistorica = new IndicatorInfoDTO();
                VolatilidadeHistorica.Ajuda = @"O Historical volatility é um desvio padrão com escala logarítmica. O Historical Volatility Index é baseado no livro de Don Fishback, 'Odds: The Key to 90% Winners'. 
Esta formula irá resultar em um índice de volatilidade histórica de 30 dias, plotando valores entre 0 e 1: 
Stdev(Log(Close / Close Yesterday), 30) * Sqrt(365) 
Observe que alguns operadores de mercado usam 252 em vez de 365 de barhistory usado no calculo da raiz quadrada. O valor de Log é o natural (ie Log10). 
Quanto maior o valor do índice, maior a volatilidade da ação.";

                VolatilidadeHistorica.NovoPainel = true;
                VolatilidadeHistorica.NomePortugues = "Volatilidade Histórica";
                VolatilidadeHistorica.TemSerieFilha1 = false;
                VolatilidadeHistorica.TemSerieFilha2 = false;
                VolatilidadeHistorica.StrokeColor = Colors.Red;
                VolatilidadeHistorica.StrokeThickness = 1;
                VolatilidadeHistorica.StrokeType = 0;
                VolatilidadeHistorica.TipoStockchart = IndicatorType.HistoricalVolatility;
                VolatilidadeHistorica.Propriedades = GetPropriedadesVolatilidadeHistorica();
                StaticData.listaIndicadores.Add(VolatilidadeHistorica);

                #endregion

                #region Volume ROC

                IndicatorInfoDTO VolumeROC = new IndicatorInfoDTO();
                VolumeROC.Ajuda = @"O indicador Volume Rate of Change mostra claramente se o volume está operando em uma direção ou em outra. 
O V-ROC é um indicador que mostra quando está desenvolvendo uma tendência no volume ou altista ou baixista. O Volume ROC mostra a velocidade em que o volume está mudando. Isto pode ser uma informação útil, já que toda formação gráfica significante (topos, fundos, breakouts,etc.) são acompanhadas por forte alta no volume. ";

                VolumeROC.NovoPainel = false;
                VolumeROC.NomePortugues = "Volume ROC";
                VolumeROC.TemSerieFilha1 = false;
                VolumeROC.TemSerieFilha2 = false;
                VolumeROC.StrokeColor = Colors.Red;
                VolumeROC.StrokeThickness = 1;
                VolumeROC.StrokeType = 0;
                VolumeROC.TipoStockchart = IndicatorType.VolumeROC;
                VolumeROC.Propriedades = GetPropriedadesVolumeROC();
                StaticData.listaIndicadores.Add(VolumeROC);


                #endregion

                #region Welles Wider Smoothing

                IndicatorInfoDTO WellesWiderSmoothing = new IndicatorInfoDTO();
                WellesWiderSmoothing.Ajuda = @"O indicador Welles Wilder's Smoothing é similar a uma media móvel exponencial. O indicador não usa a formula padrão da média móvel exponencial. Welles Wilder é exposto como 1/14 of dos dados de hoje + 13/14 da média de ontem em uma média móvel exponencial de 14 dias. 
Este indicador é usado da mesma forma que qualquer outra média móvel. ";

                WellesWiderSmoothing.NovoPainel = false;
                WellesWiderSmoothing.NomePortugues = "Welles Wider Smoothing";
                WellesWiderSmoothing.TemSerieFilha1 = false;
                WellesWiderSmoothing.TemSerieFilha2 = false;
                WellesWiderSmoothing.StrokeColor = Colors.Red;
                WellesWiderSmoothing.StrokeThickness = 1;
                WellesWiderSmoothing.StrokeType = 0;
                WellesWiderSmoothing.TipoStockchart = IndicatorType.WellesWilderSmoothing;
                WellesWiderSmoothing.Propriedades = GetPropriedadesWellesWiderSmoothing();
                StaticData.listaIndicadores.Add(WellesWiderSmoothing);


                #endregion

                #region Willians %R

                IndicatorInfoDTO  WilliansR = new IndicatorInfoDTO();
                WilliansR.Ajuda = @"Williams’ %R mede os níveis overbought/oversold (mercado sobre-comprado/sobre-vendido). 
O Williams %R é um indicador de momentum. ESte oscilador foi desenvolvido por Larry Williams. O Larry Williams indica que a essência de seu trading system é baseado na interpretação do %R. William %R, também chamado simplesmente de %R, mostra o relacionamento do preço de fechamento em relação com o range de máxima e mínima durante um dado período de tempo. Quanto mais próximo o fechamento está do topo do range, mais próximo de zero (mais alto) estará o indicador. Quanto mais próximo está o preço de fechamento do fundo do range, mais próximo de -100 (fundo) estará o indicador. Se o preço de fechamento atinge o topo do range de máxima mínima do período escolhido, então o indicador estará em 0 (zero - o topo da escala). Se o fechamento atinge o fundo do range, então o resultado será de -100 (o menor valor da escala). 
O Williams's %R Tem se mostrado muito útil para antecipar reversões de tendência. Identifica mercado sobre-comprado ou sobre-vendido. É importante lembrar que um mercado sobre-comprado não necessariamente significa que é o momento de vender, e mercado sobre-vendido também não significa que é o momento de comprar. Um ativo pode estar em uma tendência de baixa, se torna sobre-vendido e permanece sobre-vendido enquanto a tendência de baixa persiste. Uma vez que o ativo apresenta níveis de sobre-compra ou sobre-venda, os investidores deverão esperar por um sinal de reversão de tendência para montar a posição. Um possível método seria esperar que o Williams %R cruze abaixo ou acima do valor -50 para uma confirmação. A confirmação da reversão de tendência pode ser feita também com uso de outros indicadores ou linhas de estudos em conjunto com o Williams %R. ";

                WilliansR.NovoPainel = true;
                WilliansR.NomePortugues = "Willians %R";
                WilliansR.TemSerieFilha1 = false;
                WilliansR.TemSerieFilha2 = false;
                WilliansR.StrokeColor = Colors.Red;
                WilliansR.StrokeThickness = 1;
                WilliansR.StrokeType = 0;
                WilliansR.TipoStockchart = IndicatorType.WilliamsPctR;
                WilliansR.Propriedades = GetPropriedadesWilliansR();
                StaticData.listaIndicadores.Add(WilliansR);


                #endregion
                
            }
            catch (Exception exc)
            {                
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna a validação de acordo com o indicador que esta sendo inserido
        /// </summary>
        /// <param name="indicador"></param>
        /// <param name="listaPropriedades"></param>
        /// <param name="numeroBarras"></param>
        /// <returns></returns>
        public static string ValidaIndicator(IndicatorType indicador, List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            switch ((indicador))
            {
                case IndicatorType.AccumulativeSwingIndex:
                    return ValidaSwingIndex(listaPropriedades, numeroBarras);
                case IndicatorType.Aroon:
                    return ValidaAroon(listaPropriedades, numeroBarras);
                case IndicatorType.SimpleMovingAverage:
                    return ValidaSimpleMovingAverage(listaPropriedades, numeroBarras);
                case IndicatorType.RelativeStrengthIndex:
                    return ValidaRelativeStrengthIndex(listaPropriedades, numeroBarras);
                default :
                    return "";
            }
        }

        /// <summary>
        /// Metodo que retorna a lista de propriedades default de cada indicador passado
        /// </summary>
        /// <param name="indicador"></param>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesDefault(IndicatorType indicador)
        {
            switch ((indicador))
            {
                case IndicatorType.AccumulativeSwingIndex:
                    return GetPropriedadesAccumulativeSwingIndex();
                case IndicatorType.SimpleMovingAverage:
                    return GetPropriedadesSMA();
                case IndicatorType.RelativeStrengthIndex:
                    return GetPropriedadesIndiceForcaRelativa();                    
                default:
                    return null;
            }
        }

        #region Validadores

        /// <summary>
        /// Metodo que faz a validação do indicador WilliamsAccumulationDistribution
        /// </summary>
        /// <returns></returns>
        public static string ValidaWilliamsAccumulationDistribution(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador Aroon
        /// </summary>
        /// <returns></returns>
        public static string ValidaAroon(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador BollingerBands
        /// </summary>
        /// <returns></returns>
        public static string ValidaBollingerBands(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador FractalChaosBands
        /// </summary>
        /// <returns></returns>
        public static string ValidaFractalChaosBands(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador PrimeNumberBands
        /// </summary>
        /// <returns></returns>
        public static string ValidaPrimeNumberBands(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador HighLowBands
        /// </summary>
        /// <returns></returns>
        public static string ValidaHighLowBands(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador ChaikinMoneyFlow
        /// </summary>
        /// <returns></returns>
        public static string ValidaChaikinMoneyFlow(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador ChaikinVolatility
        /// </summary>
        /// <returns></returns>
        public static string ValidaChaikinVolatility(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador CommodityChannelIndex
        /// </summary>
        /// <returns></returns>
        public static string ValidaCommodityChannelIndex(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador EaseOfMovement
        /// </summary>
        /// <returns></returns>
        public static string ValidaEaseOfMovement(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador MovingAverageEnvelope
        /// </summary>
        /// <returns></returns>
        public static string ValidaMovingAverageEnvelope(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador WeightedClose
        /// </summary>
        /// <returns></returns>
        public static string ValidaWeightedClose(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador VerticalHorizontalFilter
        /// </summary>
        /// <returns></returns>
        public static string ValidaVerticalHorizontalFilter(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador HighMinusLow
        /// </summary>
        /// <returns></returns>
        public static string ValidaHighMinusLow(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador IdentificadorAgulha
        /// </summary>
        /// <returns></returns>
        public static string ValidaIdentificadorAgulha(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador ArooMoneyFlowIndexn
        /// </summary>
        /// <returns></returns>
        public static string ValidaMoneyFlowIndex(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador RelativeStrengthIndex
        /// </summary>
        /// <returns></returns>
        public static string ValidaRelativeStrengthIndex(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            if (listaPropriedades.Count == 2)
            {
                if (Convert.ToInt32(listaPropriedades[1].Value) > numeroBarras)
                    return "Número de períodos é maior que o número de barras presentes no gráfico.";
            }
            else if (listaPropriedades.Count == 3)
            {
                if (Convert.ToInt32(listaPropriedades[2].Value) > numeroBarras)
                    return "Número de períodos é maior que o número de barras presentes no gráfico.";
            }

            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador ComparativeRelativeStrength
        /// </summary>
        /// <returns></returns>
        public static string ValidaComparativeRelativeStrength(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador PerformanceIndex
        /// </summary>
        /// <returns></returns>
        public static string ValidaPerformanceIndex(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }
             
       
        /// <summary>
        /// Metodo que faz a validação do indicador NegativeVolumeIndex
        /// </summary>
        /// <returns></returns>
        public static string ValidaNegativeVolumeIndex(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }
               
        /// <summary>
        /// Metodo que faz a validação do indicador PositiveVolumeIndex
        /// </summary>
        /// <returns></returns>
        public static string ValidaPositiveVolumeIndex(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }
      
        /// <summary>
        /// Metodo que faz a validação do indicador StochasticMomentumIndex
        /// </summary>
        /// <returns></returns>
        public static string ValidaStochasticMomentumIndex(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }
    
        /// <summary>
        /// Metodo que faz a validação do indicador SwingIndex
        /// </summary>
        /// <returns></returns>
        public static string ValidaSwingIndex(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador TradeVolumeIndex
        /// </summary>
        /// <returns></returns>
        public static string ValidaTradeVolumeIndex(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador KeltnerChannel
        /// </summary>
        /// <returns></returns>
        public static string ValidaKeltnerChannel(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }
        
        /// <summary>
        /// Metodo que faz a validação do indicador MACD
        /// </summary>
        /// <returns></returns>
        public static string ValidaMACD(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador MACDHistogram
        /// </summary>
        /// <returns></returns>
        public static string ValidaMACDHistogram(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador MassIndex
        /// </summary>
        /// <returns></returns>
        public static string ValidaMassIndex(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador ExponentialMovingAverage
        /// </summary>
        /// <returns></returns>
        public static string ValidaExponentialMovingAverage(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador WeightedMovingAverage
        /// </summary>
        /// <returns></returns>
        public static string ValidaWeightedMovingAverage(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador SimpleMovingAverage
        /// </summary>
        /// <returns></returns>
        public static string ValidaSimpleMovingAverage(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            if (listaPropriedades.Count == 2)
            {
                if (Convert.ToInt32(listaPropriedades[1].Value) > numeroBarras)
                    return "Número de períodos é maior que o número de barras presentes no gráfico.";
            }
            else
            {
                if (Convert.ToInt32(listaPropriedades[2].Value) > numeroBarras)
                    return "Número de períodos é maior que o número de barras presentes no gráfico.";
            }
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador TimeSeriesMovingAverage
        /// </summary>
        /// <returns></returns>
        public static string ValidaTimeSeriesMovingAverage(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador TriangularMovingAverage
        /// </summary>
        /// <returns></returns>
        public static string ValidaTriangularMovingAverage(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador VariableMovingAverage
        /// </summary>
        /// <returns></returns>
        public static string ValidaVariableMovingAverage(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador VIDYA
        /// </summary>
        /// <returns></returns>
        public static string ValidaVIDYA(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador DirectionalMovementSystem
        /// </summary>
        /// <returns></returns>
        public static string ValidaDirectionalMovementSystem(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        
        /// <summary>
        /// Metodo que faz a validação do indicador OnBalanceVolume
        /// </summary>
        /// <returns></returns>
        public static string ValidaOnBalanceVolume(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador AroonOscillator
        /// </summary>
        /// <returns></returns>
        public static string ValidaAroonOscillator(List<IndicatorPropertyDTO> listaPropriedades, int AroonOscillator)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador FractalChaosOscillator
        /// </summary>
        /// <returns></returns>
        public static string ValidaFractalChaosOscillator(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador VolumeOscillator
        /// </summary>
        /// <returns></returns>
        public static string ValidaVolumeOscillator(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador DetrendedPriceOscillator
        /// </summary>
        /// <returns></returns>
        public static string ValidaDetrendedPriceOscillatorl(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador StochasticOscillator
        /// </summary>
        /// <returns></returns>
        public static string ValidaStochasticOscillator(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador MomentumOscillator
        /// </summary>
        /// <returns></returns>
        public static string ValidaMomentumOscillator(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador PrimeNumberOscillator
        /// </summary>
        /// <returns></returns>
        public static string ValidaPrimeNumberOscillator(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        

        /// <summary>
        /// Metodo que faz a validação do indicador UltimateOscillator
        /// </summary>
        /// <returns></returns>
        public static string ValidaUltimateOscillator(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador PriceROC
        /// </summary>
        /// <returns></returns>
        public static string ValidaPriceROC(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador ValidaChandeMomentumOscillator
        /// </summary>
        /// <returns></returns>
        public static string ValidaChandeMomentumOscillator(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }
        
        /// <summary>
        /// Metodo que faz a validação do indicador ValidaPriceOscillator
        /// </summary>
        /// <returns></returns>
        public static string ValidaPriceOscillator(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador ValidaDetrendedPriceOscillator
        /// </summary>
        /// <returns></returns>
        public static string ValidaDetrendedPriceOscillator(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }
        
        
        
        /// <summary>
        /// Metodo que faz a validação do indicador ValidaRainbowOscillator
        /// </summary>
        /// <returns></returns>
        public static string ValidaRainbowOscillator(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador Median
        /// </summary>
        /// <returns></returns>
        public static string ValidaMedian(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador LinearRegressionForecast
        /// </summary>
        /// <returns></returns>
        public static string ValidaLinearRegressionForecast(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador LinearRegressionIntercept
        /// </summary>
        /// <returns></returns>
        public static string ValidaLinearRegressionIntercept(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador LinearRegressionRSquared
        /// </summary>
        /// <returns></returns>
        public static string ValidaLinearRegressionRSquared(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador LinearRegressionSlope
        /// </summary>
        /// <returns></returns>
        public static string ValidaLinearRegressionSlope(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador ParabolicSAR
        /// </summary>
        /// <returns></returns>
        public static string ValidaParabolicSAR(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador PriceVolumeTrend
        /// </summary>
        /// <returns></returns>
        public static string ValidaPriceVolumeTrend(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador TRIX
        /// </summary>
        /// <returns></returns>
        public static string ValidaTRIX(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador TrueRange
        /// </summary>
        /// <returns></returns>
        public static string ValidaTrueRange(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }
      
        /// <summary>
        /// Metodo que faz a validação do indicador TypicalPrice
        /// </summary>
        /// <returns></returns>
        public static string ValidaTypicalPrice(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador HistoricalVolatility
        /// </summary>
        /// <returns></returns>
        public static string ValidaHistoricalVolatility(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador VolumeROC
        /// </summary>
        /// <returns></returns>
        public static string ValidaVolumeROC(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }

        /// <summary>
        /// Metodo que faz a validação do indicador WellesWilderSmoothing
        /// </summary>
        /// <returns></returns>
        public static string ValidaWellesWilderSmoothing(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }
        
        /// <summary>
        /// Metodo que faz a validação do indicador WilliamsPctR
        /// </summary>
        /// <returns></returns>
        public static string ValidaWilliamsPctR(List<IndicatorPropertyDTO> listaPropriedades, int numeroBarras)
        {
            return "";
        }


        #endregion

        #endregion

        #region Metodos Privados

        #region Accumulative Swing Index

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Accumulative Swing Index
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesAccumulativeSwingIndex()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));                        
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null,  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Limite móvel", TipoField.NumericUpDownInteger, 12,  1));
            

            return listaPropriedades;
        }

        #endregion

        #region Acumulação/Distribuição Wiliams

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Acumulação/Distribuição Wiliams
        /// </summary>
        public static List<IndicatorPropertyDTO> GetPropriedadesAcumulacaoDistribuicaoWiliams()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));

            return listaPropriedades;
        }

        #endregion

        #region Aroon

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Aroon
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesAroon()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region BollingerBands

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em BollingerBands
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesBollingerBands()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Superior", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Blue, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Inferior", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Green, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE",  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Desvio Padrão", TipoField.NumericUpDownInteger, 2, 2));
            listaPropriedades.Add(new IndicatorPropertyDTO("Média Móvel", TipoField.Media, "Exponencial",  3));
            

            return listaPropriedades;
        }

        #endregion

        #region Bandas Fractal Chaos

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Bandas Fractal Chaos
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesBandasFractalChaos()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red,-1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Filha", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1,  -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null,  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14,  1));
            

            return listaPropriedades;
        }

        #endregion

        #region Banda Números Primos

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Banda Números Primos
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesBandaNumerosPrimos()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Filha", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));

            return listaPropriedades;
        }

        #endregion

        #region Bandas Máxima/Mínimo

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Bandas Máxima/Mínimo
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesBandasMaximaMinimo()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Banda Superior", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Banda Inferior", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null,  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));
            

            return listaPropriedades;
        }

        #endregion

        #region Chaikin Fluxo Financeiro(Money Flow)

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Chaikin Fluxo Financeiro(Money Flow)
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesChaikinFluxoFinanceiro()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null,  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Volume", TipoField.Serie, ".VOLUME",  1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 2));
            
            

            return listaPropriedades;
        }

        #endregion

        #region Chaikin Volatilidade

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Chaikin Volatilidade
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesChaikinVolatilidade()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null,  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Periodos", TipoField.NumericUpDownInteger, 14, 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Rate of Chg", TipoField.NumericUpDownInteger, 2, 2));
            listaPropriedades.Add(new IndicatorPropertyDTO("Média móvel", TipoField.Media, "Simples", 3));

            return listaPropriedades;
        }

        #endregion

        #region Commodity Channel Index(CCI)

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Commodity Channel Index(CCI)
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesCommodityChannelIndex()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null,  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));
            

            return listaPropriedades;
        }

        #endregion

        #region Ease of Movement

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em BollingerBands
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesEaseMovement()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null,  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Volume", TipoField.Serie, ".VOLUME",  1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Periodos", TipoField.NumericUpDownInteger, 14, 2));
            listaPropriedades.Add(new IndicatorPropertyDTO("Média móvel", TipoField.Media, "Simples", 3));
            

            return listaPropriedades;
        }

        #endregion

        #region Envelope de Médias Móveis

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Envelope de Médias Móveis
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesEnvelopeMediasMoveis()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Filha", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Média móvel", TipoField.Media, "Simples", 2));
            listaPropriedades.Add(new IndicatorPropertyDTO("Shift", TipoField.Double, 5, 3));

            return listaPropriedades;
        }

        #endregion

        #region Fechamento Ponderado

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Fechamento Ponderado
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesFechamentoPonderado()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));

            return listaPropriedades;
        }

        #endregion

        #region Filtro Vertical/Horizontal

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Filtro Vertical/Horizontal
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesFiltroVerticalHorizontal()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie,".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));


            return listaPropriedades;
        }

        #endregion

        #region High Minus Low

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em HighMinusLow
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesHighMinusLow()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));

            return listaPropriedades;
        }

        #endregion

        #region High Minus Low

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em HighMinusLow
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesHighLowActivator()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 3, 1));

            return listaPropriedades;
        }
        
        #endregion

        #region Identificador de Agulha

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Identificador de Agulha
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesIdentificadorAgulha()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série MM3", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série MM20", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE",  0));

            return listaPropriedades;
        }

        #endregion

        #region Índice de Fluxo Financeiro

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Índice de Fluxo Financeiro
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesIndiceFluxoFinanceiro()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Volume", TipoField.Serie, ".VOLUME", 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 2));
            

            return listaPropriedades;
        }

        #endregion

        #region Índice de Força Relativa(IFR/RSI)

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Índice de Força Relativa(IFR/RSI)
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesIndiceForcaRelativa()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region Índice de Força Relativa Comparada

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Índice de Força Relativa Comparada
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesIndiceForcaRelativaComparada()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série A", TipoField.Serie, ".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série B", TipoField.Serie, ".OPEN", 1));

            return listaPropriedades;
        }

        #endregion

        #region Índice de Performance

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Índice de Performance
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesIndicePerformance()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE", 0));

            return listaPropriedades;
        }

        #endregion

        #region Índice de Volume Negativo

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em BollingerBands
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesIndiceVolumeNegativo()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Volume", TipoField.Serie, ".VOLUME", 1));

            return listaPropriedades;
        }

        #endregion

        #region Índice de Volume Positivo

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Índice de Volume Positivo
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesIndiceVolumePositivo()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Volume", TipoField.Serie, ".VOLUME", 1));

            return listaPropriedades;
        }

        #endregion

        #region Índice Estocástico

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Índice Estocástico
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesIndiceEstocastico()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Filha", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));

            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("%K Periodos", TipoField.NumericUpDownInteger, 13, 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("%K Smooth", TipoField.NumericUpDownInteger, 25, 2));
            listaPropriedades.Add(new IndicatorPropertyDTO("%K Dbl Smooth", TipoField.NumericUpDownInteger, 2, 3));
            listaPropriedades.Add(new IndicatorPropertyDTO("%D Periodos", TipoField.NumericUpDownInteger, 9, 4));
            listaPropriedades.Add(new IndicatorPropertyDTO("Média móvel", TipoField.Media, "Simples", 5));
            listaPropriedades.Add(new IndicatorPropertyDTO("%D Tipo M. Móvel", TipoField.Media, "Simples", 6));

            return listaPropriedades;
        }

        #endregion

        #region Índice Swing

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Índice Swing
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesIndiceSwing()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Limite Móvel", TipoField.NumericUpDownInteger, 12, 1));
            

            return listaPropriedades;
        }

        #endregion

        #region Índice Trade Volume

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Índice Trade Volume
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesIndiceTradeVolume()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Volume", TipoField.Serie, ".VOLUME", 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Menor valor tick", TipoField.Double, 0.25, 2));

            return listaPropriedades;
        }

        #endregion

        #region Keltner

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Keltner
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesKeltner()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Periodos", TipoField.NumericUpDownInteger, 14, 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Shift", TipoField.Double, 1.1, 2));
            listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Média", TipoField.Media,"Simples" , 3));


            return listaPropriedades;
        }

        #endregion

        #region MACD

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em MACD
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesMACD()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Filha", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));

            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ciclo longo", TipoField.NumericUpDownInteger, 26, 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ciclo curto", TipoField.NumericUpDownInteger, 13, 2));
            listaPropriedades.Add(new IndicatorPropertyDTO("Sinal", TipoField.NumericUpDownInteger, 9, 3));


            return listaPropriedades;
        }

        #endregion

        #region MACD Histograma

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em MACD Histograma
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesMACDHistograma()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ciclo longo", TipoField.NumericUpDownInteger, 26, 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ciclo curto", TipoField.NumericUpDownInteger, 13, 2));
            listaPropriedades.Add(new IndicatorPropertyDTO("Sinal", TipoField.NumericUpDownInteger, 9, 3));

            return listaPropriedades;
        }

        #endregion

        #region Mass Índex

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Mass Índex
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesMassIndex()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));
            

            return listaPropriedades;
        }

        #endregion

        #region Media Móvel Exponencial

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Media Móvel Exponencial
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesMediaMoveExponencial()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, false, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region Média Móvel Ponderada

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Média Móvel Ponderada
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesMediaMovelPonderada()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, false, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region Média Móvel Simples

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Média Móvel Simples
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesSMA()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();
            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1,  -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parametros", TipoField.Header, null,  -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série ", TipoField.Serie, ".CLOSE",  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 5,  1));

            return listaPropriedades;
        }

        #endregion

        #region Média Móvel Tempo Séries

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Média Móvel Tempo Séries
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesMediaMovelTempoSeries()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, false, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region Média Móvel Triangula

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Média Móvel Triangula
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesMediaMovelTriangula()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, false, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region  Média Móvel Variável

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em  Média Móvel Variável
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesMediaMovelVariavel()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, false, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region Média Móvel VIDYA

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Média Móvel VIDYA
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesMediaMovelVIDYA()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, false, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE",  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Escala R2", TipoField.Double, 0.65, 2));

            return listaPropriedades;
        }

        #endregion

        #region  Movimento Direcional

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Movimento Direcional
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesMovimentoDirecional()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Filha 1", TipoField.Header, null, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Filha 2", TipoField.Header, null, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region On Balance Volume (OBV)

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em On Balance Volume (OBV)
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesOnBalanceVolume()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Volume", TipoField.Serie, ".VOLUME", 1));

            return listaPropriedades;
        }

        #endregion

        #region Oscilador Aroon

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Oscilador Aroon
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesOsciladorAroon()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));
            

            return listaPropriedades;
        }

        #endregion

        #region Oscilador Change Momentum

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Oscilador Change Momentum
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesOsciladorChangeMomentum()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region Oscilador de Fractal Chaos

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Oscilador de Fractal Chaos
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesOsciladorFractalChaos()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            ////listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));
            

            return listaPropriedades;
        }

        #endregion

        #region Oscilador de Preços

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Oscilador de Preços
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesOsciladorPrecos()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE",  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ciclo longo", TipoField.NumericUpDownInteger, 22, 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ciclo curto", TipoField.NumericUpDownInteger, 14, 2));
            listaPropriedades.Add(new IndicatorPropertyDTO("Média móvel", TipoField.Media, "Simples", 3));

            return listaPropriedades;
        }

        #endregion

        #region Oscilador de Volume

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Oscilador de Volume
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesOsciladorVolume()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Volume", TipoField.Serie, ".VOLUME", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Curto prazo", TipoField.NumericUpDownInteger, 9, 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Longo prazo", TipoField.NumericUpDownInteger, 21, 2));
            listaPropriedades.Add(new IndicatorPropertyDTO("Pontos ou Porc", TipoField.NumericUpDownInteger, 1, 3));
            

            return listaPropriedades;
        }

        #endregion

        #region Oscilador Detrended Price

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Oscilador Detrended Price
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesOsciladorDetrendedPrice()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE",  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Média móvel", TipoField.Media, "Simples", 2));

            return listaPropriedades;
        }

        #endregion

        #region Oscilador Estocástico

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Oscilador Estocástico
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesOsciladorEstocastico()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Filha", TipoField.Header, null, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));

            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));
            listaPropriedades.Add(new IndicatorPropertyDTO(" %K Periodos", TipoField.NumericUpDownInteger, 14, 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("%K Slowing", TipoField.NumericUpDownInteger, 3, 2));
            listaPropriedades.Add(new IndicatorPropertyDTO("%D Periodos", TipoField.NumericUpDownInteger, 5, 3));
            listaPropriedades.Add(new IndicatorPropertyDTO("Média móvel", TipoField.Media, "Simples", 4));

            return listaPropriedades;
        }

        #endregion

        #region Oscilador Momentum

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Oscilador Momentum
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesOsciladorMomentum()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE",  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region Oscilador Números Primos

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Oscilador Números Primos
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesOsciladorNumerosPrimos()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE",  0));

            return listaPropriedades;
        }

        #endregion

        #region Oscilador Rainbow

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Oscilador Rainbow
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesOsciladorRainbow()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE",  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Levels", TipoField.NumericUpDownInteger, 9,  1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Média Móvel", TipoField.Media, "Simple",  2));

            return listaPropriedades;
        }

        #endregion

        #region Oscilador Ultimate

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Oscilador Ultimate
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesOsciladorUltimate()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ciclo1", TipoField.NumericUpDownInteger, 7, 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ciclo2", TipoField.NumericUpDownInteger, 14, 2));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ciclo3", TipoField.NumericUpDownInteger, 28, 3));

            return listaPropriedades;
        }

        #endregion

        #region Preço-ROC

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Preço-ROC
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesPrecoROC()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE",  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region Preço Médio

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Preço Médio
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesPrecoMedio()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));

            return listaPropriedades;
        }

        #endregion

        #region Regressão Linear-Forecast

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Regressão Linear-Forecast
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesRegressaoLinearForecast()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE",  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region Regressão Linear-Intercept

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Regressão Linear-Intercept
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesRegressaoLinearIntercept()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE",  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region Regressão Linear-Raiz Quadrada

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Regressão Linear-Raiz Quadrada
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesRegressaoLinearRaizQuadrada()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE",  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14,  1));

            return listaPropriedades;
        }

        #endregion

        #region Regressão Linear-Slope

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Regressão Linear-Slope
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesRegressaoLinearSlope()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE",  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region SAR Parabólico(PSAR)

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em SAR Parabólico(PSAR)
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesSARParabolico()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null,  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Min Af", TipoField.Double, 0.02, 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Max Af", TipoField.Double, 0.2, 2));

            return listaPropriedades;
        }

        #endregion

        #region Tendência Preço/Volume

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Tendência Preço/Volume
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesTendênciaPrecoVolume()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Volume", TipoField.Serie, ".CLOSE", 1));

            return listaPropriedades;
        }

        #endregion

        #region TRIX

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em TRIX
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesTRIX()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE",  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Periodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region True Range

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em True Range
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesTrueRange()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null,  0));

            return listaPropriedades;
        }

        #endregion

        #region Typical Price

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Typical Price
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesTypicalPrice()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null,  0));

            return listaPropriedades;
        }

        #endregion

        #region Volatilidade Histórica

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Volatilidade Histórica
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesVolatilidadeHistorica()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE", 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Histórico de barras", TipoField.NumericUpDownInteger, 365, 2));
            listaPropriedades.Add(new IndicatorPropertyDTO("Desvio Padrão", TipoField.NumericUpDownInteger, 2, 3));

            return listaPropriedades;
        }

        #endregion

        #region Volume ROC

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em BollingerBands
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesVolumeROC()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Volume", TipoField.Serie, ".VOLUME",  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));
            
            return listaPropriedades;
        }

        #endregion

        #region Welles Wider Smoothing

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Welles Wider Smoothing
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesWellesWiderSmoothing()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Série", TipoField.Serie, ".CLOSE",  0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));

            return listaPropriedades;
        }

        #endregion

        #region Willians %R

        /// <summary>
        /// Metodo que retorna a lista de propriedades que devem ser apresentada em Willians %R
        /// </summary>
        /// <returns></returns>
        public static List<IndicatorPropertyDTO> GetPropriedadesWilliansR()
        {
            List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

            //listaPropriedades.Add(new IndicatorPropertyDTO("Características Série Principal", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Cor", TipoField.Cor, Brushes.Red, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Tipo de Linha", TipoField.StrokeType, 0, -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Grossura", TipoField.StrokeThicness, 1, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Parâmetros", TipoField.Header, null,  -1));
            //listaPropriedades.Add(new IndicatorPropertyDTO("Inserir em novo Painel", TipoField.CheckNewPanel, true, -1));
            listaPropriedades.Add(new IndicatorPropertyDTO("Ativo", TipoField.SymbolList, null, 0));
            listaPropriedades.Add(new IndicatorPropertyDTO("Períodos", TipoField.NumericUpDownInteger, 14, 1));
            

            return listaPropriedades;
        }

        #endregion

        #endregion
    }
}
