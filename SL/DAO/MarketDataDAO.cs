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
using System.Collections.Generic;
using Traderdata.Client.TerminalWEB.DTO;
using System.Threading;
using System.IO.IsolatedStorage;
using System.IO;
using System.Globalization;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using Traderdata.Client.TerminalWEB.Util;
using Traderdata.Server.Core.DTO;

namespace Traderdata.Client.TerminalWEB.DAO
{
    public class MarketDataDAO
    {
        #region Variaveis

        /// <summary>
        /// Variavel de acesso aos webservices
        /// </summary>
        private MDApiSVC.MarketDataWCFClient mdWebClient =
            new MDApiSVC.MarketDataWCFClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());

        #endregion

        #region Eventos


        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de cotação.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void CotacaoDiarioHandler(List<CotacaoDTO> Result);

        /// <summary>Evento disparado quando a ação de GetCotacaoDiaria é executada.</summary>
        public event CotacaoDiarioHandler GetCotacaoDiariaCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de set de cache
        /// </summary>
        /// <param name="tick"></param>
        public delegate void CotacaoDiarioCacheHandler(string Result);

        /// <summary>Evento disparado quando a ação de GetCotacaoDiaria é executada.</summary>
        public event CotacaoDiarioCacheHandler SetCotacaoDiariaCacheCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de set de cache
        /// </summary>
        /// <param name="tick"></param>
        public delegate void SetCotacaoIntradayCacheHandler(string Result);

        /// <summary>Evento disparado quando a ação de GetCotacaoDiaria é executada.</summary>
        public event SetCotacaoIntradayCacheHandler SetCotacaoIntradayCacheCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de cotação.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void CotacaoIntradayHandler(List<CotacaoDTO> Result);

        /// <summary>Evento disparado quando a ação de getCotacaoIntraday é executada.</summary>
        public event CotacaoIntradayHandler GetCotacaoIntradayCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de segmentos.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetSegmentosHandler(List<string> Result);

        /// <summary>Evento disparado quando a lista de segmentos é recebida.</summary>
        public event GetSegmentosHandler GetSegmentosCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de segmentos.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void SetCacheSegmentosHandler(List<string> Result);

        /// <summary>Evento disparado quando a lista de segmentos é recebida.</summary>
        public event SetCacheSegmentosHandler SetCacheSegmentosCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos por segmento.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void AtivosPorSegmentoHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a lista de segmentos é recebida.</summary>
        public event AtivosPorSegmentoHandler GetAtivosPorSegmentoCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de indices.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetIndicesHandler(List<string> Result);

        /// <summary>Evento disparado quando a ação de ping é executada.</summary>
        public event GetIndicesHandler GetIndicesCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de indices.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void SetCacheIndicesHandler(List<string> Result);

        /// <summary>Evento disparado quando a ação de ping é executada.</summary>
        public event SetCacheIndicesHandler SetCacheIndicesCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos por indice.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosPorIndiceHandler(List<AtivoDTO> Result, string Indice);

        /// <summary>Evento disparado quando a ação de ping é executada.</summary>
        public event GetAtivosPorIndiceHandler GetAtivosPorIndiceCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos que devem ser cacheados
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBovespaQueDevemSerCacheadosHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de ping é executada.</summary>
        public event GetAtivosBovespaQueDevemSerCacheadosHandler GetAtivosBovespaQueDevemSerCacheadosCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos por indice.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void SetCacheAtivosPorIndiceHandler(List<AtivoDTO> Result, string Indice);

        /// <summary>Evento disparado quando a ação de ping é executada.</summary>
        public event SetCacheAtivosPorIndiceHandler SetCacheAtivosPorIndiceCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos bovespa.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBovespaTodosHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoTodos é executada.</summary>
        public event GetAtivosBovespaTodosHandler GetAtivosBovespaTodosCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos bovespa.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void SetCacheAtivosBovespaHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoTodos é executada.</summary>
        public event SetCacheAtivosBovespaHandler SetCacheAtivosBovespaCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos bovespa.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBovespaVistaHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoVista é executada.</summary>
        public event GetAtivosBovespaVistaHandler GetAtivosBovespaVistaCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos bovespa.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBovespaOpcaoHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoOpcao é executada.</summary>
        public event GetAtivosBovespaOpcaoHandler GetAtivosBovespaOpcaoCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos bovespa.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBovespaTermoHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoTermo é executada.</summary>
        public event GetAtivosBovespaTermoHandler GetAtivosBovespaTermoCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos BMF.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBMFTodosHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoTermo é executada.</summary>
        public event GetAtivosBMFTodosHandler GetAtivosBMFTodosCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos BMF.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void SetCacheAtivosBMFTodosHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoTermo é executada.</summary>
        public event SetCacheAtivosBMFTodosHandler SetCacheAtivosBMFTodosCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos BMF principais.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBMFPrincpalCheioHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoTermo é executada.</summary>
        public event GetAtivosBMFPrincpalCheioHandler GetAtivosBMFPrincipalCheioCompleted;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de lista de ativos BMF - mini contratos
        /// </summary>
        /// <param name="tick"></param>
        public delegate void GetAtivosBMFMiniContratosHandler(List<AtivoDTO> Result);

        /// <summary>Evento disparado quando a ação de getAtivoTermo é executada.</summary>
        public event GetAtivosBMFMiniContratosHandler GetAtivosBMFMiniContratosCompleted;
        #endregion

        #region Construtor

        /// <summary>
        /// Metodo que deve ser executado para se iniciar a classe estatica de marketdata
        /// </summary>        
        public MarketDataDAO()
        {
            //assinando eventos WCF
            mdWebClient.GetCotacaoCompleted += new EventHandler<MDApiSVC.GetCotacaoCompletedEventArgs>(mdWebClient_GetCotacaoCompleted);
        
            //assinando eventos de realtime
            RealTimeDAO.TickReceived += new RealTimeDAO.TickHandler(RealTimeDAO_TickReceived);
        }


        #endregion

        #region Eventos WCF

        void mdWebClient_GetCotacaoCompleted(object sender, MDApiSVC.GetCotacaoCompletedEventArgs e)
        {
            //recuperando parametros enviados
            List<object> args = (List<object>)e.UserState;
            string quoteType = Convert.ToString(args[0]);
            string symbol = Convert.ToString(args[1]);

            if (quoteType == "I")
            {
                List<CotacaoDTO> listaCotacao = new List<CotacaoDTO>();
                foreach (string obj in e.Result)
                {
                    CotacaoDTO quote = new CotacaoDTO();
                    CotacaoDTO.TryParse(obj, out quote);
                    quote.Data = quote.Data.ToLocalTime();
                    listaCotacao.Add(quote);
                }


                //armazenando no cache
                StaticData.cacheCotacaoIntraday[symbol] = listaCotacao;

                //Disparanbdo o evento
                if ((bool)args[2])
                {
                    if (GetCotacaoIntradayCompleted != null)
                        GetCotacaoIntradayCompleted(listaCotacao);
                }
            }
            else if (quoteType == "EOD")
            {
                if (GetCotacaoDiariaCompleted != null)
                {
                    List<CotacaoDTO> listaCotacao = new List<CotacaoDTO>();
                    foreach (string obj in e.Result)
                    {
                        CotacaoDTO quote = new CotacaoDTO();
                        CotacaoDTO.TryParse(obj, out quote);
                        listaCotacao.Add(quote);
                    }

                    //armazenando no cache
                    StaticData.cacheCotacaoDiario[symbol] = listaCotacao;

                    //assinando a atualização para o ativo resgatado
                    RealTimeDAO.StartTickSubscription(symbol);

                    //Disparanbdo o evento
                    GetCotacaoDiariaCompleted(listaCotacao);
                }
            }
            
        }

        

        #endregion

        #region Eventos Realtime

        /// <summary>
        /// Evento disparado ao se receber um novo tick de qualquer ativo assinado
        /// </summary>
        /// <param name="Result"></param>
        void RealTimeDAO_TickReceived(object Result)
        {
            //atualizando cache diario
            //if (StaticData.cacheCotacaoDiario.ContainsKey(((TickDTO)Result).Ativo))
            //{                
            //    CotacaoDTO cotacaoAux = new CotacaoDTO(((TickDTO)Result).Abertura, ((TickDTO)Result).Maximo,
            //        ((TickDTO)Result).Minimo, ((TickDTO)Result).Ultimo, ((TickDTO)Result).Quantidade, ((TickDTO)Result).Volume,
            //        ((TickDTO)Result).Data.Date, false, ((TickDTO)Result).Hora);

            //    if (StaticData.cacheCotacaoDiario[((TickDTO)Result).Ativo][StaticData.cacheCotacaoDiario[((TickDTO)Result).Ativo].Count - 1].Data.Date == DateTime.Today)
            //        StaticData.cacheCotacaoDiario[((TickDTO)Result).Ativo][StaticData.cacheCotacaoDiario[((TickDTO)Result).Ativo].Count - 1] = cotacaoAux;
            //    else
            //        StaticData.cacheCotacaoDiario[((TickDTO)Result).Ativo].Add(cotacaoAux);
            //}

        }

        #endregion

        #region Eventos Cache

        
        #endregion

        #region Metodos

        /// <summary>
        /// Metodo que retorna as cotações diarias
        /// </summary>
        /// <param name="ativo"></param>
        public void GetCotacaoDiariaAsync(string ativo)
        {
            try
            {
                List<object> args = new List<object>();
                args.Add("EOD");
                args.Add(ativo);
                
                if (!StaticData.cacheCotacaoDiario.ContainsKey(ativo))
                {
                    mdWebClient.GetCotacaoAsync(ativo, CotacaoDTO.BasePeriodicity.DailyEOD, 1, null, null, StaticData.DelayedVersion, null, false, false, args);
                }
                else
                {
                    if (GetCotacaoDiariaCompleted != null)
                        GetCotacaoDiariaCompleted(StaticData.cacheCotacaoDiario[ativo]);
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
                        
        }

        /// <summary>
        /// Metodo que retorna as cotações intraday
        /// </summary>
        /// <param name="ativo"></param>
        public void GetCotacaoIntradayAsync(string ativo, bool disparaEvento, bool forceRefresh)
        {
            try
            {
                List<object> args = new List<object>();
                args.Add("I");
                args.Add(ativo);
                args.Add(disparaEvento);

                if (!StaticData.cacheCotacaoIntraday.ContainsKey(ativo))
                {
                    mdWebClient.GetCotacaoAsync(ativo,CotacaoDTO.BasePeriodicity.Minute, 1, null, null, StaticData.DelayedVersion, 10000, false, false, args);                
                }
                else
                {
                    if (GetCotacaoIntradayCompleted != null)
                        GetCotacaoIntradayCompleted(StaticData.cacheCotacaoIntraday[ativo]);
                }

                
                
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
          

        /// <summary>
        /// Metodo que espera receber a lista de cotação intraday em barras de minuto e devolve uma serie na periodicidade solicitada
        /// </summary>
        /// <returns></returns>
        public List<CotacaoDTO> ConvertPeriodicidadeIntraday(List<CotacaoDTO> listaCotacao1Minuto, int periodicidade)
        {
            try
            {
                //se a periodicidade for igual a um nao tem que converter
                if (periodicidade == 1)
                    return listaCotacao1Minuto;

                return AplicaPeriodicidadeGenericaPorMinuto(listaCotacao1Minuto, periodicidade);
            }
            catch (Exception exc)
            {                
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que espera receber a lista de cotação intraday em barras diarias e devolve uma serie na periodicidade solicitada
        /// </summary>
        /// <returns></returns>
        public List<CotacaoDTO> ConvertPeriodicidadeDiaria(List<CotacaoDTO> listaCotacaoDiaria, int periodicidade)
        {
            try
            {
                //se a periodicidade for igual a um nao tem que converter
                switch (periodicidade)
                {
                    case 1440:
                        return listaCotacaoDiaria;
                    case 10080:
                        return AplicaPeriodicidadeSemanal(listaCotacaoDiaria);
                    case 43200:
                        return AplicaPeriodicidadeMensal(listaCotacaoDiaria);
                    default:
                        return listaCotacaoDiaria;
                }
                    
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
                
        #region Ajuste de Periodicidade

        /// <summary>
        /// Metodo retornas as cotações organizadas em periodicidade semanal, de acordo com a semana brasileira
        /// </summary>
        /// <param name="listaTemp"></param>
        /// <returns></returns>
        private List<CotacaoDTO> AplicaPeriodicidadeSemanal(List<CotacaoDTO> listaCotacao)
        {
            List<CotacaoDTO> listaResultado = new List<CotacaoDTO>();
            List<CotacaoDTO> listaTemp = new List<CotacaoDTO>();
            int numeroSemana = -1;
            int numeroSemanaAnterior = -1;
            CultureInfo myCI = new CultureInfo("pt-br");
            System.Globalization.Calendar myCal = myCI.Calendar;

            try
            {
                //Percorrer a lista de cotações, verificando pelo quebra do número da semana
                foreach (CotacaoDTO objTemp in listaCotacao)
                {
                    numeroSemana = myCal.GetWeekOfYear(objTemp.Data, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
                    if (numeroSemana != numeroSemanaAnterior)
                    {
                        if (listaTemp.Count > 0)
                        {
                            //nesse caso deve gerar a barra e inserir no resultado
                            listaResultado.Add(GeraBarraDiaria(listaTemp));
                            listaTemp.Clear();
                        }
                        listaTemp.Add(objTemp);
                        numeroSemanaAnterior = numeroSemana;
                    }
                    else
                        listaTemp.Add(objTemp);
                }
                //Verifico se ficou dado no hub de cotações e caso tenha ficado submeto
                if (listaTemp.Count > 0)
                    listaResultado.Add(GeraBarraDiaria(listaTemp));

                //retornando o resultado
                return listaResultado;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo retornas as cotações organizadas em periodicidade mensal, de acordo com a semana brasileira
        /// </summary>
        /// <param name="listaTemp"></param>
        /// <returns></returns>
        private List<CotacaoDTO> AplicaPeriodicidadeMensal(List<CotacaoDTO> listaCotacao)
        {
            List<CotacaoDTO> listaResultado = new List<CotacaoDTO>();
            List<CotacaoDTO> listaTemp = new List<CotacaoDTO>();
            int numeroMes = -1;
            int numeroMesAnterior = -1;
            CultureInfo myCI = new CultureInfo("pt-br");
            System.Globalization.Calendar myCal = myCI.Calendar;

            try
            {
                //Percorrer a lista de cotações, verificando pelo quebra do número da semana
                foreach (CotacaoDTO objTemp in listaCotacao)
                {
                    numeroMes = myCal.GetMonth(objTemp.Data);
                    if (numeroMes != numeroMesAnterior)
                    {
                        if (listaTemp.Count > 0)
                        {
                            //nesse caso deve gerar a barra e inserir no resultado
                            listaResultado.Add(GeraBarraDiaria(listaTemp));
                            listaTemp.Clear();
                        }
                        listaTemp.Add(objTemp);
                        numeroMesAnterior = numeroMes;
                    }
                    else
                        listaTemp.Add(objTemp);
                }

                //Verifico se ficou dado no hub de cotações e caso tenha ficado submeto
                if (listaTemp.Count > 0)
                    listaResultado.Add(GeraBarraDiaria(listaTemp));

                //retornando o resultado
                return listaResultado;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que recebe a lista de cotações em barras de minuto e converte para a periodicidade
        /// </summary>
        /// <param name="listaCotacao"></param>
        /// <param name="periodicidade"></param>
        /// <returns></returns>
        private List<CotacaoDTO> AplicaPeriodicidadeGenericaPorMinuto(List<CotacaoDTO> listaCotacao, int periodicidade)
        {
            List<BarraComprimida> listaBarrasComprimidas = new List<BarraComprimida>();
            List<CotacaoDTO> listaFinal = new List<CotacaoDTO>();
            string horaNormalizada = "";
            try
            {
                //Percorro a lista com barras de 1 minuto
                foreach (CotacaoDTO obj in listaCotacao)
                {
                    //Efetuo a normalização da barra
                    horaNormalizada = NormalizaPeriodo(obj.Data.ToString("HHmm"), periodicidade);
                    DateTime dataHoraBarraNormalizada = new DateTime(obj.Data.Year,
                        obj.Data.Month, obj.Data.Day,
                        Convert.ToInt16(horaNormalizada.Substring(0, 2)),
                        Convert.ToInt16(horaNormalizada.Substring(2, 2)), 0);

                    //Verifico a data da ultima normalização                    
                    if ((listaBarrasComprimidas.Count > 0) && (dataHoraBarraNormalizada == listaBarrasComprimidas[listaBarrasComprimidas.Count - 1].data))
                        listaBarrasComprimidas[listaBarrasComprimidas.Count - 1].listaCotacoes.Add(obj);
                    else
                    {
                        BarraComprimida novaBarra = new BarraComprimida();
                        novaBarra.data = dataHoraBarraNormalizada;
                        novaBarra.listaCotacoes = new List<CotacaoDTO>();
                        novaBarra.listaCotacoes.Add(obj);

                        //incluindo no hub de datas
                        listaBarrasComprimidas.Add(novaBarra);
                    }
                }

                //Devo percorrer os ubconuntos de barras, gerando as barras finais
                foreach (BarraComprimida obj in listaBarrasComprimidas)
                {
                    listaFinal.Add(GeraBarraIntraday(obj.listaCotacoes, periodicidade));
                }

                return listaFinal;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo deve retornar a barra consolidada de acordo com a lista de cotações
        /// </summary>
        /// <param name="listaCotacao"></param>
        /// <returns></returns>
        private CotacaoDTO GeraBarraIntraday(List<CotacaoDTO> listaCotacao, int periodicidade)
        {
            CotacaoDTO barraResultado = new CotacaoDTO();

            try
            {
                //Setando dados iniciais da barra
                barraResultado.Abertura = listaCotacao[0].Abertura;
                barraResultado.AfterMarket = listaCotacao[0].AfterMarket;                
                barraResultado.Data = listaCotacao[0].Data;
                barraResultado.Maximo = Double.MinValue;
                barraResultado.Minimo = Double.MaxValue;
                string Hora = NormalizaPeriodo(listaCotacao[0].Data.ToString("HHmm"), periodicidade);
                barraResultado.Quantidade = 0;
                barraResultado.Ultimo = 0;
                barraResultado.Volume = 0;


                foreach (CotacaoDTO obj in listaCotacao)
                {
                    if (obj.Maximo > barraResultado.Maximo)
                        barraResultado.Maximo = obj.Maximo;
                    if (obj.Minimo < barraResultado.Minimo)
                        barraResultado.Minimo = obj.Minimo;
                    barraResultado.Volume += obj.Volume;
                    barraResultado.Quantidade += obj.Quantidade;
                }

                //setando o fechamento
                barraResultado.Ultimo = listaCotacao[listaCotacao.Count - 1].Ultimo;

                //Retornando a barra gerada
                return barraResultado;
            }
            catch (Exception exc)
            {
                throw exc;
            }


        }

        /// <summary>
        /// Metodo deve retornar a barra consolidada de acordo com a lista de cotações
        /// </summary>
        /// <param name="listaCotacao"></param>
        /// <returns></returns>
        private CotacaoDTO GeraBarraDiaria(List<CotacaoDTO> listaCotacao)
        {
            CotacaoDTO barraResultado = new CotacaoDTO();

            try
            {
                //Setando dados iniciais da barra
                barraResultado.Abertura = listaCotacao[0].Abertura;                
                barraResultado.Data = listaCotacao[0].Data;
                barraResultado.Maximo = Double.MinValue;
                barraResultado.Minimo = Double.MaxValue;
                barraResultado.Quantidade = 0;
                barraResultado.Ultimo = 0;
                barraResultado.Volume = 0;


                foreach (CotacaoDTO obj in listaCotacao)
                {
                    if (obj.Maximo > barraResultado.Maximo)
                        barraResultado.Maximo = obj.Maximo;
                    if (obj.Minimo < barraResultado.Minimo)
                        barraResultado.Minimo = obj.Minimo;
                    barraResultado.Volume += obj.Volume;
                    barraResultado.Quantidade += obj.Quantidade;
                }

                //setando o fechamento
                barraResultado.Ultimo = listaCotacao[listaCotacao.Count - 1].Ultimo;

                //Retornando a barra gerada
                return barraResultado;
            }
            catch (Exception exc)
            {
                throw exc;
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Hora"></param>
        /// <param name="periodicidade"></param>
        private string NormalizaPeriodo(string Hora, int periodicidade)
        {
            try
            {
                //ajustando a hora de acordo com a periodicidade
                if (Convert.ToInt16(Hora.Substring(2, 2)) % periodicidade == 0)
                    return Hora;
                else
                {
                    //nesse caso tenho que percorrer a hora para baixo até encontrar algum divisivel
                    for (int i = Convert.ToInt16(Hora); i >= 0; i--)
                    {
                        string aux = Convert.ToString(i);
                        if (aux.Length < 4)
                            aux = "0" + aux;
                        if (Convert.ToUInt16(aux.Substring(2, 2)) % periodicidade == 0)
                        {
                            return Convert.ToString(aux);
                        }
                    }

                    throw new Exception("Hora passada não é normalizavel");
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

     

        #endregion

        /// <summary>
        /// Metodo que recebe um stream compactado e retorna uma string
        /// </summary>
        /// <param name="Result"></param>
        /// <returns></returns>
        private string Uncompress(Stream Result)
        {
            ZipInputStream zipInputStream = new ZipInputStream(Result);
            ZipEntry zipEntry = zipInputStream.GetNextEntry();
            MemoryStream streamWriter = new MemoryStream();
            while (zipEntry != null)
            {
                String entryFileName = zipEntry.Name;
                // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                // Optionally match entrynames against a selection list here to skip as desired.
                // The unpacked length is available in the zipEntry.Size property.

                byte[] buffer = new byte[4096];		// 4K is optimum

                // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                // of the file, but does not waste memory.
                // The "using" will close the stream even if an exception occurs.
                StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                zipEntry = zipInputStream.GetNextEntry();
            }



            //convertendo MemoryStream to string
            string text = "";
            streamWriter.Position = 0;
            using (StreamReader sr = new StreamReader(streamWriter))
            {
                text = sr.ReadToEnd();
            }

            return text.Replace('\r', ' ').Replace('\n', '|');
        }


        #endregion

        private struct BarraComprimida
        {
            public DateTime data;
            public List<CotacaoDTO> listaCotacoes;
        }
    }

    
}


