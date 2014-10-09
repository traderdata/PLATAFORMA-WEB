using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModulusFE.TASDK;

namespace Traderdata.Server.App.TerminalWeb.BusinessManager
{
    public class CalculosEstatisticosScannerBM:IDisposable
    {
        #region Construtor

        /// <summary>
        /// Construtor Padrão
        /// </summary>
        public CalculosEstatisticosScannerBM()
        {
        }

        /// <summary>
        /// Metodo de Dispose
        /// </summary>
        public void Dispose()
        {
        }

        #endregion

        #region Declarando as BMs de Calculo

        private MovingAverage movingAverageBM = new MovingAverage();
        private Index indexBM = new Index();
        private Oscillator oscilatorBM = new Oscillator();

        #endregion

        #region Calculos Estasticos

        /* Nome: CalculaSimpleMovingAverage
         * Descrição: Realiza os calculos estatísticos do TASDK
         * Data: 27/10/2009
         */
        private List<double> CalculaSimpleMovingAverage(List<double> listaCotacao, int periodos)
        {
            try
            {
                Recordset pRS = new Recordset();
                Navigator pNav = new Navigator();
                Field fieldValor = new Field(listaCotacao.Count, "Valor");
                List<double> listDouble = new List<double>();

                for (int j = 0; j <= listaCotacao.Count - 1; j++)
                {
                    fieldValor.Value(j, listaCotacao[j]);
                }

                pRS.AddField(fieldValor);
                pNav.Recordset_ = pRS;

                //Executando o calculo
                Recordset resultado = movingAverageBM.SimpleMovingAverage(pNav, fieldValor, periodos);

                //Percorrendo o recordset para montar a lista de doubles
                for (int i = 0; i <= listaCotacao.Count - 1; i++)
                {
                    listDouble.Add(resultado.ValueEx("Simple Moving Average", i));
                }

                return listDouble;

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /* Nome: CalculaExponentialMovingAverage
         * Descrição: Realiza os calculos estatísticos do TASDK
         * Data: 27/10/2009
         */
        private List<double> CalculaExponentialMovingAverage(List<double> listaCotacao, int periodos)
        {
            List<double> listDouble = new List<double>();
            try
            {
                Recordset pRS = new Recordset();
                Navigator pNav = new Navigator();
                Field fieldValor = new Field(listaCotacao.Count, "Valor");

                for (int j = 0; j <= listaCotacao.Count - 1; j++)
                {
                    fieldValor.Value(j, listaCotacao[j]);
                }

                pRS.AddField(fieldValor);
                pNav.Recordset_ = pRS;

                //Executando o calculo
                Recordset resultado = movingAverageBM.ExponentialMovingAverage(pNav, fieldValor, periodos);

                //Percorrendo o recordset para montar a lista de doubles
                for (int i = 0; i <= listaCotacao.Count - 1; i++)
                {
                    listDouble.Add(resultado.ValueEx("Exponential Moving Average", i));
                }

                return listDouble;

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /* Nome: CalculaIFR
         * Descrição: Realiza os calculos estatísticos do TASDK
         * Data: 27/10/2009
         */
        private List<double> CalculaIFR(List<double> listaCotacao, int periodos)
        {
            try
            {
                List<double> listaResultado = new List<double>();
                Recordset pRS = new Recordset();
                Navigator pNav = new Navigator();
                Field fieldValor = new Field(listaCotacao.Count, "Valor");

                for (int j = 0; j <= listaCotacao.Count - 1; j++)
                {
                    fieldValor.Value(j, Convert.ToDouble(listaCotacao[j]));
                }

                pRS.AddField(fieldValor);
                pNav.Recordset_ = pRS;

                //Executando o calculo
                Recordset resultado = indexBM.RelativeStrengthIndex(pNav, fieldValor, periodos);

                //Percorrendo a lista de resultados
                for (int i = 0; i <= listaCotacao.Count - 1; i++)
                {
                    listaResultado.Add(resultado.ValueEx("Relative Strength Index", i));
                }

                return listaResultado;

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /* Nome: CalculaTRIX
        * Descrição: Realiza os calculos estatísticos do TRIX
        * Data: 27/10/2009
        */
        private List<double> CalculaTRIX(List<double> listaCotacao, int periodos)
        {
            try
            {
                List<double> listaDouble = new List<double>();
                Recordset pRS = new Recordset();
                Navigator pNav = new Navigator();
                Field fieldValor = new Field(listaCotacao.Count, "Valor");

                for (int j = 0; j <= listaCotacao.Count - 1; j++)
                {
                    fieldValor.Value(j, Convert.ToDouble(listaCotacao[j]));
                }

                pRS.AddField(fieldValor);
                pNav.Recordset_ = pRS;

                //Executando o calculo
                Recordset resultado = oscilatorBM.TRIX(pNav, fieldValor, periodos);

                //Percorrendo o recordset para montar a lista de doubles
                for (int i = 0; i <= listaCotacao.Count - 1; i++)
                {
                    listaDouble.Add(resultado.ValueEx("TRIX", i));
                }

                return listaDouble;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /* Nome: CalculaEstocastico
        * Descrição: Realiza os calculos estatísticos do estocastico
        * Data: 27/10/2009
        */
        private List<double> CalculaEstocastico(List<double> listaUltimo, List<double> listaMax, List<double> listaMin, int kPeriodo, int kSlowing, int dPeriodo)
        {
            try
            {
                List<double> listaDouble = new List<double>();
                Recordset pRS = new Recordset();
                Navigator pNav = new Navigator();
                Field fieldHigh = new Field(listaMax.Count, "High");
                Field fieldLow = new Field(listaMin.Count, "Low");
                Field fieldClose = new Field(listaUltimo.Count, "Close");

                for (int j = 0; j <= listaUltimo.Count - 1; j++)
                {
                    fieldHigh.Value(j, listaMax[j]);
                    fieldLow.Value(j, listaMin[j]);
                    fieldClose.Value(j, listaUltimo[j]);
                }

                pRS.AddField(fieldHigh);
                pRS.AddField(fieldLow);
                pRS.AddField(fieldClose);
                pNav.Recordset_ = pRS;

                //Executando o calculo
                Recordset resultado = oscilatorBM.StochasticOscillator(pNav, pRS, kPeriodo, kSlowing, dPeriodo, ModulusFE.IndicatorType.SimpleMovingAverage);

                //Percorrendo o recordset para montar a lista de doubles
                for (int i = 0; i <= listaUltimo.Count - 1; i++)
                {
                    listaDouble.Add(resultado.ValueEx("%K", i));
                }

                return listaDouble;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /* Nome: CalculaMACD
        * Descrição: Realiza os calculos estatísticos do MACD
        * Data: 27/10/2009
        */
        private List<double> CalculaMACD(List<double> listaUltimo, List<double> listaMax, List<double> listaMin, List<double> listaAbertura, int signalPeriod, int cicloLongo, int cicloCurto)
        {
            try
            {
                List<double> listaDouble = new List<double>();
                Recordset pRS = new Recordset();
                Navigator pNav = new Navigator();
                Field fieldHigh = new Field(listaMax.Count, "High");
                Field fieldLow = new Field(listaMin.Count, "Low");
                Field fieldClose = new Field(listaUltimo.Count, "Close");
                Field fieldOpen = new Field(listaAbertura.Count, "Open");

                for (int j = 0; j <= listaUltimo.Count - 1; j++)
                {
                    fieldHigh.Value(j, listaMax[j]);
                    fieldLow.Value(j, listaMin[j]);
                    fieldClose.Value(j, listaUltimo[j]);
                    fieldOpen.Value(j, listaAbertura[j]);
                }

                pRS.AddField(fieldHigh);
                pRS.AddField(fieldLow);
                pRS.AddField(fieldClose);
                pRS.AddField(fieldOpen);
                pNav.Recordset_ = pRS;

                //Executando o calculo
                Recordset resultado = oscilatorBM.MACDHistogram(pNav, pRS, signalPeriod, cicloLongo, cicloCurto);

                //Percorrendo o recordset para montar a lista de doubles
                for (int i = 0; i <= listaUltimo.Count - 1; i++)
                {
                    listaDouble.Add(resultado.ValueEx("MACD", i));
                }

                return listaDouble;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        #endregion

        #region Evaluates

        /* Nome: EvaluateTRIXCompra
         * Descrição: Realiza os calculos para o TRIX na compra
         * Data: 27/10/2009
         */
        public bool EvaluateTRIXCompra(List<double> listaUltimo, int periodoTRIX, int periodoMM, double margemErro)
        {
            List<double> listValoresTRIX = new List<double>();
            List<double> listMediaMovel = new List<double>();

            try
            {

                //Calculando o valor da SMA para a primeira parcela
                listValoresTRIX = CalculaTRIX(listaUltimo, periodoTRIX);

                //Calculando o valor da SMA para a segunda parcela
                listMediaMovel = CalculaSimpleMovingAverage(listValoresTRIX, periodoMM);

                //Temos que verificar 2 situações
                //1a - TRIX em d-1 menor que a media movel
                //2a - TRIX em d igual ou maior que a media movel
                if ((listMediaMovel.Count > 2) && (listValoresTRIX.Count > 2))
                {
                    bool resultado;
                    resultado = IdentificaCruzamentoDeDoisValores(listValoresTRIX[listValoresTRIX.Count - 2],
                        listValoresTRIX[listValoresTRIX.Count - 1], listMediaMovel[listMediaMovel.Count - 2],
                        listMediaMovel[listMediaMovel.Count - 1], "C", margemErro);


                    return resultado;
                }
                else
                    return false;


            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /* Nome: EvaluateTRIXVenda
         * Descrição: Realiza os calculos para o TRIX na venda
         * Data: 27/10/2009
         */
        public bool EvaluateTRIXVenda(List<double> listaUltimo, int periodoTRIX, int periodoMM, double margemErro)
        {
            List<double> listValoresTRIX = new List<double>();
            List<double> listMediaMovel = new List<double>();

            try
            {

                //Calculando o valor da SMA para a primeira parcela
                listValoresTRIX = CalculaTRIX(listaUltimo, periodoTRIX);

                //Calculando o valor da SMA para a segunda parcela
                listMediaMovel = CalculaSimpleMovingAverage(listValoresTRIX, periodoMM);

                //Temos que verificar 2 situações
                //1a - TRIX em d-1 menor que a media movel
                //2a - TRIX em d igual ou maior que a media movel
                if ((listMediaMovel.Count > 2) && (listValoresTRIX.Count > 2))
                {
                    bool resultado = IdentificaCruzamentoDeDoisValores(listValoresTRIX[listValoresTRIX.Count - 2],
                        listValoresTRIX[listValoresTRIX.Count - 1], listMediaMovel[listMediaMovel.Count - 2],
                        listMediaMovel[listMediaMovel.Count - 1], "V", margemErro);


                    return resultado;

                }
                else
                    return false;


            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /* Nome: EvaluateEstocasticoCompra
         * Descrição: Realiza os calculos para o Estocastico na compra
         * Data: 27/10/2009
         */
        public bool EvaluateEstocasticoCompra(List<double> listaUltimo, List<double> listaMax, List<double> listaMin, int kPeriodo, int kSlowing, int dPeriodo, double margemErro)
        {
            List<double> listValoresEstocastico = new List<double>();
            List<double> listMediaMovel = new List<double>();

            try
            {

                //Calculando o valor da SMA para a primeira parcela
                listValoresEstocastico = CalculaEstocastico(listaUltimo, listaMax, listaMin, kPeriodo, kSlowing, dPeriodo);

                //Calculando o valor da SMA para a segunda parcela
                listMediaMovel = CalculaSimpleMovingAverage(listValoresEstocastico, dPeriodo);

                //Temos que verificar 2 situações
                //1a - TRIX em d-1 menor que a media movel
                //2a - TRIX em d igual ou maior que a media movel
                if ((listMediaMovel.Count > 2) && (listValoresEstocastico.Count > 2))
                {

                    return IdentificaCruzamentoDeDoisValores(listValoresEstocastico[listValoresEstocastico.Count - 2],
                        listValoresEstocastico[listValoresEstocastico.Count - 1], listMediaMovel[listMediaMovel.Count - 2],
                        listMediaMovel[listMediaMovel.Count - 1], "C", margemErro);

                }
                else
                    return false;


            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /* Nome: EvaluateEstocasticoVenda
         * Descrição: Realiza os calculos para o Estocastico na venda
         * Data: 27/10/2009
         */
        public bool EvaluateEstocasticoVenda(List<double> listaUltimo, List<double> listaMax, List<double> listaMin, int kPeriodo, int kSlowing, int dPeriodo, double margemErro)
        {
            List<double> listValoresEstocastico = new List<double>();
            List<double> listMediaMovel = new List<double>();

            try
            {

                //Calculando o valor da SMA para a primeira parcela
                listValoresEstocastico = CalculaEstocastico(listaUltimo, listaMax, listaMin, kPeriodo, kSlowing, dPeriodo);

                //Calculando o valor da SMA para a segunda parcela
                listMediaMovel = CalculaSimpleMovingAverage(listValoresEstocastico, dPeriodo);

                //Temos que verificar 2 situações
                //1a - TRIX em d-1 menor que a media movel
                //2a - TRIX em d igual ou maior que a media movel
                if ((listMediaMovel.Count > 2) && (listValoresEstocastico.Count > 2))
                {
                    return IdentificaCruzamentoDeDoisValores(listValoresEstocastico[listValoresEstocastico.Count - 2],
                        listValoresEstocastico[listValoresEstocastico.Count - 1], listMediaMovel[listMediaMovel.Count - 2],
                        listMediaMovel[listMediaMovel.Count - 1], "V", margemErro);

                }
                else
                    return false;


            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /* Nome: EvaluateMACDCompra
         * Descrição: Realiza os calculos para o MACD na compra
         * Data: 27/10/2009
         */
        public bool EvaluateMACDCompra(List<double> listaUltimo, List<double> listaMax, List<double> listaMin, List<double> listaAbertura, int cicloLongo, int cicloCurto, int signalPeriod, double margemErro)
        {
            List<double> listValoresMACD = new List<double>();
            List<double> listMediaMovel = new List<double>();

            try
            {

                //Calculando o valor da SMA para a primeira parcela
                listValoresMACD = CalculaMACD(listaUltimo, listaMax, listaMin, listaAbertura, signalPeriod, cicloLongo, cicloCurto);

                //Calculando o valor da SMA para a segunda parcela
                listMediaMovel = CalculaExponentialMovingAverage(listValoresMACD, signalPeriod);

                //Temos que verificar 2 situações
                //1a - TRIX em d-1 menor que a media movel
                //2a - TRIX em d igual ou maior que a media movel
                if ((listMediaMovel.Count > 2) && (listValoresMACD.Count > 2))
                {
                    //EventLog.WriteEntry("macd", "valor macd=" + listValoresMACD[listValoresMACD.Count - 2] .ToString()+ "signalPeriod=" + signalPeriod.ToString() + "cicloLongo=" + cicloLongo.ToString() + "cicloCurto=" + cicloCurto.ToString(), EventLogEntryType.Error);

                    return IdentificaCruzamentoDeDoisValores(listValoresMACD[listValoresMACD.Count - 2],
                            listValoresMACD[listValoresMACD.Count - 1],
                            listMediaMovel[listMediaMovel.Count - 2],
                            listMediaMovel[listMediaMovel.Count - 1],
                            "C",
                            margemErro);
                }
                else
                    return false;


            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /* Nome: EvaluateMACDVenda
         * Descrição: Realiza os calculos para o MACD na venda
         * Data: 27/10/2009
         */
        public bool EvaluateMACDVenda(List<double> listaUltimo, List<double> listaMax, List<double> listaMin, List<double> listaAbertura, int cicloLongo, int cicloCurto, int signalPeriod, double margemErro)
        {
            List<double> listValoresMACD = new List<double>();
            List<double> listMediaMovel = new List<double>();

            try
            {

                //Calculando o valor da SMA para a primeira parcela
                listValoresMACD = CalculaMACD(listaUltimo, listaMax, listaMin, listaAbertura, signalPeriod, cicloLongo, cicloCurto);

                //Calculando o valor da SMA para a segunda parcela
                listMediaMovel = CalculaExponentialMovingAverage(listValoresMACD, signalPeriod);

                //Temos que verificar 2 situações
                //1a - TRIX em d-1 menor que a media movel
                //2a - TRIX em d igual ou maior que a media movel
                if ((listMediaMovel.Count > 2) && (listValoresMACD.Count > 2))
                {
                    return IdentificaCruzamentoDeDoisValores(listValoresMACD[listValoresMACD.Count - 2],
                            listValoresMACD[listValoresMACD.Count - 1],
                            listMediaMovel[listMediaMovel.Count - 2],
                            listMediaMovel[listMediaMovel.Count - 1],
                            "V",
                            margemErro);
                }
                else
                    return false;


            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /* Nome: EvaluateCruzamentoDeDuasSMA
         * Descrição: Realiza os calculos para o SMA
         * Data: 27/10/2009
         */
        public bool EvaluateCruzamentoDeDuasSMA(List<double> listaValores, int periodoMedia1, int periodoMedia2, double margemErro)
        {
            List<double> listaMediaMovel1 = new List<double>();
            List<double> listaMediaMovel2 = new List<double>();

            try
            {
                //Calculando o valor da SMA para a primeira parcela
                listaMediaMovel1 = CalculaSimpleMovingAverage(listaValores, periodoMedia1);

                //Calculando o valor da SMA para a segunda parcela
                listaMediaMovel2 = CalculaSimpleMovingAverage(listaValores, periodoMedia2);

                if ((listaMediaMovel1.Count > 2) && (listaMediaMovel2.Count > 2))
                {
                    return IdentificaCruzamentoDeDoisValores(listaMediaMovel1[listaMediaMovel1.Count - 2],
                            listaMediaMovel1[listaMediaMovel1.Count - 1],
                            listaMediaMovel2[listaMediaMovel2.Count - 2],
                            listaMediaMovel2[listaMediaMovel2.Count - 1],
                            "A",
                            margemErro);
                }
                else
                    return false;
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /* Nome: EvaluateCruzamentoDeDuasEMA
         * Descrição: Realiza os calculos para o EMA
         * Data: 27/10/2009
         */
        public bool EvaluateCruzamentoDeDuasEMA(List<double> listaValores, int periodoMedia1, int periodoMedia2, double margemErro, string tipoCruzamento)
        {
            List<double> listaMediaMovel1 = new List<double>();
            List<double> listaMediaMovel2 = new List<double>();

            try
            {
                //Calculando o valor da SMA para a primeira parcela
                listaMediaMovel1 = CalculaExponentialMovingAverage(listaValores, periodoMedia1);

                //Calculando o valor da SMA para a segunda parcela
                listaMediaMovel2 = CalculaExponentialMovingAverage(listaValores, periodoMedia2);

                if ((listaMediaMovel1.Count > 2) && (listaMediaMovel2.Count > 2))
                {
                    return IdentificaCruzamentoDeDoisValores(listaMediaMovel1[listaMediaMovel1.Count - 2],
                            listaMediaMovel1[listaMediaMovel1.Count - 1],
                            listaMediaMovel2[listaMediaMovel2.Count - 2],
                            listaMediaMovel2[listaMediaMovel2.Count - 1],
                            tipoCruzamento,
                            margemErro);
                }
                else
                    return false;
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /* Nome: EvaluateIFRInferior
         * Descrição: Realiza os calculos para o IFR
         * Data: 27/10/2009
         */
        public bool EvaluateIFRInferior(List<double> listaUltimo, int periodos, int valorInferior, double margemErro)
        {
            List<double> listaValoresIFR = new List<double>();

            try
            {
                //Calculando o valor da SMA para a primeira parcela
                listaValoresIFR = CalculaIFR(listaUltimo, periodos);

                //Comparando se eles tem uma diferença inferior a margem de erro tolerada e retornar resultado
                if (listaValoresIFR[listaValoresIFR.Count - 1] <= valorInferior)
                    return true;
                else
                    return false;
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /* Nome: EvaluateIFRSuperior
         * Descrição: Realiza os calculos para o IFR
         * Data: 27/10/2009
         */
        public bool EvaluateIFRSuperior(List<double> listaUltimo, int periodos, int valorSuperior, double margemErro)
        {
            List<double> listaValoresIFR = new List<double>();

            try
            {
                //Calculando o valor da SMA para a primeira parcela
                listaValoresIFR = CalculaIFR(listaUltimo, periodos);

                //Comparando se eles tem uma diferença inferior a margem de erro tolerada e retornar resultado
                if (listaValoresIFR[listaValoresIFR.Count - 1] >= valorSuperior)
                {

                    return true;
                }
                else
                    return false;
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /* Nome: EvaluateVariacaoInferior
         * Descrição: Realiza a comparação entre a variação e o parametro passado
         * Data: 27/10/2009
         */
        public bool EvaluateVariacaoInferior(List<double> listaUltimo, int variacao, double margemErro)
        {
            double smaPrimeriaParcela = 0;

            try
            {
                if (listaUltimo.Count > 2)
                {
                    //Calculando a primeria parcela
                    smaPrimeriaParcela = Convert.ToDouble(
                        (listaUltimo[listaUltimo.Count - 1] - listaUltimo[listaUltimo.Count - 2]) / listaUltimo[listaUltimo.Count - 2]) * 100;
                }
                else
                    smaPrimeriaParcela = 0;

                //Comparando se eles tem uma diferença inferior a margem de erro tolerada e retornar resultado
                if (smaPrimeriaParcela <= variacao)
                    return true;
                else
                    return false;
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /* Nome: EvaluateVariacaoSuperior
        * Descrição: Realiza a comparação entre a variação e o parametro passado
        * Data: 27/10/2009
        */
        public bool EvaluateVariacaoSuperior(List<double> listaUltimo, int variacao, double margemErro)
        {
            double smaPrimeriaParcela = 0;

            try
            {
                if (listaUltimo.Count > 2)
                {
                    //Calculando a primeria parcela
                    smaPrimeriaParcela = Convert.ToDouble(
                        (listaUltimo[listaUltimo.Count - 1] - listaUltimo[listaUltimo.Count - 2]) / listaUltimo[listaUltimo.Count - 2]) * 100;
                }
                else
                    smaPrimeriaParcela = 0;

                //Comparando se eles tem uma diferença inferior a margem de erro tolerada e retornar resultado
                if (smaPrimeriaParcela >= variacao)
                    return true;
                else
                    return false;
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /* Nome: EvaluateUltimoPrecoSuperiorAMediaSimples
         * Descrição: Realiza os entre ultimo preço e a media simples
         * Data: 27/10/2009
         */
        public bool EvaluateUltimoPrecoSuperiorAMediaSimples(List<double> listaUltimo, int periodoMM, double margemErro)
        {
            List<double> listaValoresMM = new List<double>();


            try
            {
                //Calculando o valor da SMA para a primeira parcela
                listaValoresMM = CalculaSimpleMovingAverage(listaUltimo, periodoMM);

                //Comparando se eles tem uma diferença inferior a margem de erro tolerada e retornar resultado
                return (listaUltimo[listaUltimo.Count - 1] > listaValoresMM[listaValoresMM.Count - 1]);
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /* Nome: EvaluateUltimoPrecoInferiorAMediaSimples
         * Descrição: Realiza os entre ultimo preço e a media simples
         * Data: 27/10/2009
         */
        public bool EvaluateUltimoPrecoInferiorAMediaSimples(List<double> listaUltimo, int periodoMM, double margemErro)
        {
            List<double> listaValoresMM = new List<double>();


            try
            {
                //Calculando o valor da SMA para a primeira parcela
                listaValoresMM = CalculaSimpleMovingAverage(listaUltimo, periodoMM);

                //Comparando se eles tem uma diferença inferior a margem de erro tolerada e retornar resultado
                return (listaUltimo[listaUltimo.Count - 1] <= listaValoresMM[listaValoresMM.Count - 1]);
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /* Nome: EvaluateDIDICompra
         * Descrição: Realiza os calculos de DIDI avaliando para efetuar compra
         * Data: 27/10/2009
         */
        public bool EvaluateDIDICompra(List<double> listaUltimo, double margemErro)
        {
            List<double> listaValoresMM3 = new List<double>();
            List<double> listaValoresMM8 = new List<double>();
            List<double> listaValoresMM20 = new List<double>();

            bool cruzamentoMM3MM8 = false;
            bool cruzamentoMM20MM8 = false;


            try
            {
                //Calculando o valor da EMA para a primeira parcela
                listaValoresMM3 = CalculaSimpleMovingAverage(listaUltimo, 3);

                //Calculando o valor da EMA para a segunda parcela
                listaValoresMM8 = CalculaSimpleMovingAverage(listaUltimo, 8);

                //Calculando o valor da EMA para a terceira parcela
                listaValoresMM20 = CalculaSimpleMovingAverage(listaUltimo, 20);

                //identifica o cruzamento da media curta com a reta
                cruzamentoMM3MM8 = (IdentificaCruzamentoDeDoisValores(listaValoresMM3[listaValoresMM3.Count - 2], listaValoresMM3[listaValoresMM3.Count - 1],
                    listaValoresMM8[listaValoresMM8.Count - 2], listaValoresMM8[listaValoresMM8.Count - 1], "A", 0));

                //identifica o cruzamento da media curta com a reta
                cruzamentoMM20MM8 = (IdentificaCruzamentoDeDoisValores(listaValoresMM20[listaValoresMM20.Count - 2], listaValoresMM20[listaValoresMM20.Count - 1],
                    listaValoresMM8[listaValoresMM8.Count - 2], listaValoresMM8[listaValoresMM8.Count - 1], "A", 0));


                //Checando se houve agulhada
                if ((cruzamentoMM20MM8) && (cruzamentoMM3MM8))
                {
                    if ((listaValoresMM3[listaValoresMM3.Count - 1] > listaValoresMM8[listaValoresMM8.Count - 1]) &&
                         (listaValoresMM8[listaValoresMM8.Count - 1] > listaValoresMM20[listaValoresMM20.Count - 1]))
                        return true;
                    else
                        return false;
                }
                else
                    return false;

            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /* Nome: EvaluateDIDIVenda
         * Descrição: Realiza os calculos de DIDI avaliando para efetuar compra
         * Data: 27/10/2009
         */
        public bool EvaluateDIDIVenda(List<double> listaUltimo, double margemErro)
        {
            List<double> listaValoresMM3 = new List<double>();
            List<double> listaValoresMM8 = new List<double>();
            List<double> listaValoresMM20 = new List<double>();

            bool cruzamentoMM3MM8 = false;
            bool cruzamentoMM20MM8 = false;


            try
            {
                //Calculando o valor da EMA para a primeira parcela
                listaValoresMM3 = CalculaSimpleMovingAverage(listaUltimo, 3);

                //Calculando o valor da EMA para a segunda parcela
                listaValoresMM8 = CalculaSimpleMovingAverage(listaUltimo, 8);

                //Calculando o valor da EMA para a terceira parcela
                listaValoresMM20 = CalculaSimpleMovingAverage(listaUltimo, 20);

                //identifica o cruzamento da media curta com a reta
                cruzamentoMM3MM8 = (IdentificaCruzamentoDeDoisValores(listaValoresMM3[listaValoresMM3.Count - 2], listaValoresMM3[listaValoresMM3.Count - 1],
                    listaValoresMM8[listaValoresMM8.Count - 2], listaValoresMM8[listaValoresMM8.Count - 1], "A", 0));

                //identifica o cruzamento da media curta com a reta
                cruzamentoMM20MM8 = (IdentificaCruzamentoDeDoisValores(listaValoresMM20[listaValoresMM20.Count - 2], listaValoresMM20[listaValoresMM20.Count - 1],
                    listaValoresMM8[listaValoresMM8.Count - 2], listaValoresMM8[listaValoresMM8.Count - 1], "A", 0));


                //Checando se houve agulhada
                if ((cruzamentoMM20MM8) && (cruzamentoMM3MM8))
                {
                    if ((listaValoresMM3[listaValoresMM3.Count - 1] < listaValoresMM8[listaValoresMM8.Count - 1]) &&
                         (listaValoresMM8[listaValoresMM8.Count - 1] < listaValoresMM20[listaValoresMM20.Count - 1]))
                        return true;
                    else
                        return false;
                }
                else
                    return false;

            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        #endregion

        #region Metodos Auxiliares

        /* Nome: IdentificaCruzamentoDeDoisValores
         * Descrição: Avalia os dois valores passados de acordo com a margem de erro e diz se os valores se cruzaram ou não
         * Data: 27/10/2009
         * Parametros: O tipo deverá identificar como C - Compra / Venda - V ou A - Ambos
         */
        private bool IdentificaCruzamentoDeDoisValores(double valor1Anterior, double valor1Atual, double valor2Anterior, double valor2Atual, string tipo, double margemErro)
        {
            //tenho que verificar se o valor1Anterior for menor que o valor2Anterior e no período seguinte
            //valor1Atual for maior que valor2Atual, significa que houve cruzamento
            //O inverso total também vale.
            //ou o valor1atual é exatamente igual ao valor2atual, cruzamento ocorrendo neste exato momento

            if ((tipo == "V") || (tipo == "A"))
            {
                if ((valor1Anterior > valor2Anterior) &&
                      (valor1Atual <= valor2Atual))
                    return true;
            }
            if ((tipo == "C") || (tipo == "A"))
            {
                if ((valor1Anterior < valor2Anterior) &&
                    (valor1Atual >= valor2Atual))
                    return true;
            }

            //Se nenhuma das condições for disparada deve retornar falso
            return false;

        }

        #endregion
    }
}

