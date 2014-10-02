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
using FM.WebSync.Silverlight.Core;
using Traderdata.Client.TerminalWEB.DTO;
using System.Threading;
using System.ComponentModel;
using Traderdata.Client.TerminalWEB.Util;

namespace Traderdata.Client.TerminalWEB.DAO
{
    public static class RealTimeDAO
    {
        #region Variaveis Privadas

        static System.Collections.Generic.List<string> lista = new System.Collections.Generic.List<string>();

        //variavel de controle de realtime
        private static FM.WebSync.Silverlight.Core.Client clientBMFBVSPTick;

        //canal de publicação
        private static Publisher publisherBMFBVSPTick;

        //variavel de controle de realtime
        private static FM.WebSync.Silverlight.Core.Client clientBMFBVSPTrade;
        
        /// <summary> Evento genérico, que participa do processo de alternativa ao uso de invoke.</summary>
        static internal event SendOrPostCallback GenericEventHandler;

        //Variável que permitirá executar um método no contexto da thread principal, evitando a necessidade de invoke pela aplicacao cliente
        static internal AsyncOperation asyncOperation;


        #endregion

        #region Eventos

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de tick.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void TickHandler(object Result);

        /// <summary>Evento disparado quando a ação de StartTickSubscription é executada.</summary>
        public static event TickHandler TickReceived;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de trade.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void TradeHandler(object Result);

        /// <summary>Evento disparado quando a ação de StartTradeSubscription é executada.</summary>
        public static event TradeHandler TradeReceived;

        /// <summary>
        /// Representa o método que irá manipular o evento de conexao BVSP RT
        /// </summary>
        /// <param name="tick"></param>
        public delegate void ConnectBVSPHandler();

        /// <summary>Evento disparado quando a ação de ConnectBVSPRT é executada.</summary>
        public static event ConnectBVSPHandler OnConnectSuccessTick;

        /// <summary>
        /// Representa o método que irá manipular o evento de conexao BVSP RT
        /// </summary>
        /// <param name="tick"></param>
        public delegate void ConnectErrorBVSPHandler();

        /// <summary>Evento disparado quando a ação de ConnectBVSPRT é executada.</summary>
        public static event ConnectErrorBVSPHandler OnConnectErrorTick;

        #endregion

        #region Metodos
    
        /// <summary>
        /// Metodo que estabelece a conexao do sistema com o sistema de RT
        /// </summary>
        public static void ConnectBMFBVSP()
        {
            string urlbmfBVSPTick = "";
            string urlBMFBVSPTrade = "";
            
            urlbmfBVSPTick = StaticData.BVSPRTTickHost;
            urlBMFBVSPTrade = StaticData.BVSPRTTradeHost;
            

            //conectando o publicador
            publisherBMFBVSPTick = new Publisher(new PublisherArgs
            {
                RequestUrl = urlbmfBVSPTick
            });

            //conectando o hubd de dados em rt                
            clientBMFBVSPTick = new FM.WebSync.Silverlight.Core.Client(new ClientArgs
            {
                RequestUrl = urlbmfBVSPTick
            });

            //conectando o hubd de dados em rt                
            clientBMFBVSPTrade = new FM.WebSync.Silverlight.Core.Client(new ClientArgs
            {
                RequestUrl = urlBMFBVSPTrade
            });

            //Conectando
            clientBMFBVSPTick.Connect(new ConnectArgs
            {
                //StayConnected = true,
                OnSuccess = (args) =>
                {                    
                    //Disparar evento de sucesso
                    if (OnConnectSuccessTick != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            OnConnectSuccessTick();
                        });
                    }
                },
                OnFailure = (args) =>
                {
                    //Disparar evento de falha
                    if (OnConnectErrorTick != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            OnConnectErrorTick();
                        });
                    }
                },
                OnStreamFailure = (args) =>
                {
                    //codigo necessario no insucesso da conexao
                    clientBMFBVSPTick.Reconnect();                                        
                }
            });

            ////Conectando
            //clientBMFBVSPTrade.Connect(new ConnectArgs
            //{
            //    //StayConnected = true,
            //    OnSuccess = (args) =>
            //    {
                    
            //    },
            //    OnFailure = (args) =>
            //    {
            //        //Disparar evento de falha
            //        if (OnConnectErrorTick != null)
            //        {
            //            Deployment.Current.Dispatcher.BeginInvoke(() =>
            //            {
            //                OnConnectErrorTick();
            //            });
            //        }
            //    },
            //    OnStreamFailure = (args) =>
            //    {
            //        //codigo necessario no insucesso da conexao
            //        clientBMFBVSPTrade.Reconnect();
            //    }
            //});

            // Cria uma instância de uma AsyncOperation para gerenciar o contexto
            asyncOperation = AsyncOperationManager.CreateOperation(null);
        }

        

        /// <summary>
        /// Metodo usado para assinar um determinado ativo
        /// </summary>
        /// <param name="ativo"></param>
        public static void StartTickSubscription(string ativo)
        {
            if (clientBMFBVSPTick != null)
            {
                clientBMFBVSPTick.Subscribe(new SubscribeArgs
                {
                    Channel = "/" + ativo,
                    OnSuccess = (args) =>
                    {
                        publisherBMFBVSPTick.Publish(new PublicationArgs
                        {
                            Publication = new Publication
                            {
                                Channel = "/incomingticksubscription",
                                DataJson = JSON.Serialize(ativo)
                            },
                            OnComplete = (completeArgs) =>
                            {

                            },
                            OnException = (exceptionArgs) =>
                            {

                            }
                        });

                    },
                    OnFailure = (args) =>
                    {
                        //codigo de falha de assinatura
                    },
                    OnReceive = (args) =>
                    {
                        if (TickReceived != null)
                        {
                            //convertendo para tickDTO
                            TickDTO tick = ConverteStringParaTick(JSON.Deserialize<string>(args.DataJson));
                            
                            //faço a atualização do cache intraday
                            //AtualizaCacheIntraday(tick);

                            //AtualizaCacheDiario(tick);

                            //dispara evento para aqueles forms que estão aguardando o dado tick a tick
                            //Assinando o evento de disparo de dados
                            GenericEventHandler = new SendOrPostCallback(TickReceived);
                            asyncOperation.Post(GenericEventHandler, tick);

                        }
                    }
                });
            }

        }
        

        /// <summary>
        /// Metodo usado para assinar um determinado ativo
        /// </summary>
        /// <param name="ativo"></param>
        public static void StartTradeSubscription(string ativo)
        {
            if (MarketDataDAO.IsBovespa(ativo))
            {
                if (clientBMFBVSPTrade != null)
                {
                    clientBMFBVSPTrade.Subscribe(new SubscribeArgs
                    {
                        Channel = "/" + ativo,
                        OnSuccess = (args) =>
                        {

                        },
                        OnFailure = (args) =>
                        {
                            //codigo de falha de assinatura
                        },
                        OnReceive = (args) =>
                        {
                            if (TradeReceived != null)
                            {
                                TradeDTO trade = ParseiaNegocio(JSON.Deserialize<string>(args.DataJson));
                                //dispara evento para aqueles forms que estão aguardando o dado tick a tick
                                //Assinando o evento de disparo de dados
                                GenericEventHandler = new SendOrPostCallback(TradeReceived);
                                asyncOperation.Post(GenericEventHandler, trade);

                            }
                        }
                    });
                }
            }
            
        }
        
        /// <summary>
        /// Parseia pacote de negócio em formato TraderData. Atenção: Os DTOs de corretoras gerados não refletem as corretora do banco.
        /// Formato: N:[1-Ativo]:[2-Bolsa]:[3-Hora]:[4-Valor]:[5-Quantidade]:[6-Numero]:[7-Corretora Compra]:[8-Corretora Venda]:[9-Tipo registro]
        /// </summary>
        /// <param name="negocioString"></param>
        /// <returns></returns>
        private static TradeDTO ParseiaNegocio(string negocioString)
        {
            TradeDTO negocioDTO = new TradeDTO();
            string[] negocio = negocioString.Split(':');

            
            try
            {
                negocioDTO.Id = 0;
                negocioDTO.Ativo = negocio[1];
                negocioDTO.Bolsa = Convert.ToInt32(negocio[2]);
                negocioDTO.HoraBolsa = negocio[3];
                negocioDTO.Valor = Convert.ToDouble(negocio[4], GeneralUtil.NumberProvider);
                negocioDTO.Quantidade = Convert.ToDouble(negocio[5], GeneralUtil.NumberProvider);
                negocioDTO.Numero = Convert.ToInt32(negocio[6], GeneralUtil.NumberProvider);

                try
                {
                    negocioDTO.CorretoraCompradora = StaticData.CorretoraBovespa[Convert.ToInt32(negocio[7])];
                }
                catch
                {
                    negocioDTO.CorretoraCompradora = negocio[7];
                }
                try
                {
                    negocioDTO.CorretoraVendedora = StaticData.CorretoraBovespa[Convert.ToInt32(negocio[8])];
                }
                catch
                {
                    negocioDTO.CorretoraVendedora = negocio[8];
                }

                //Acertando hora se necessario
                if (negocioDTO.HoraBolsa.Length == 3)
                    negocioDTO.HoraBolsa = "0" + negocioDTO.HoraBolsa;

                negocioDTO.TipoRegistro = negocio[9];
                negocioDTO.TimeStamp = DateTime.Now;//new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, Convert.ToInt32(negocioDTO.HoraBolsa.Substring(0, 2)), Convert.ToInt32(negocioDTO.HoraBolsa.Substring(2, 2)), 0);

                return negocioDTO;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        

        /// <summary>
        /// Parsea o dados de um pacote tick e dispara o evento OnTick.
        /// </summary>
        /// <param name="tickString">Dado a ser parseado.</param>
        private static TickDTO ConverteStringParaTick(string tickString)
        {
            try
            {
                tickString = tickString.Replace("\"", "");
                string[] tick = tickString.Split(':');

                TickDTO tickDTO = new TickDTO();
                if (tick.Length > 2)
                {
                    if (tick[0] == "T")
                    {
                        tickDTO.Ativo = tick[1];
                        tickDTO.Bolsa = Convert.ToInt32(tick[2]);
                        tickDTO.Hora = tick[3];
                        tickDTO.Abertura = Convert.ToDouble(tick[4], GeneralUtil.NumberProvider);
                        tickDTO.FechamentoAnterior = Convert.ToDouble(tick[5], GeneralUtil.NumberProvider);
                        tickDTO.Ultimo = Convert.ToDouble(tick[6], GeneralUtil.NumberProvider);
                        tickDTO.Variacao = Convert.ToDouble(tick[7], GeneralUtil.NumberProvider);
                        tickDTO.Maximo = Convert.ToDouble(tick[8], GeneralUtil.NumberProvider);
                        tickDTO.Minimo = Convert.ToDouble(tick[9], GeneralUtil.NumberProvider);
                        tickDTO.Media = Convert.ToDouble(tick[10], GeneralUtil.NumberProvider);
                        tickDTO.NumeroNegocio = Convert.ToInt32(tick[11], GeneralUtil.NumberProvider);
                        tickDTO.QuantidadeUltimoNegocio = Convert.ToDouble(tick[12], GeneralUtil.NumberProvider);
                        tickDTO.Quantidade = Convert.ToDouble(tick[13], GeneralUtil.NumberProvider);
                        tickDTO.MelhorOfertaCompra = Convert.ToDouble(tick[14], GeneralUtil.NumberProvider);
                        tickDTO.QuantidadeMelhorOfertaCompra = Convert.ToDouble(tick[15], GeneralUtil.NumberProvider);
                        tickDTO.MelhorOfertaVenda = Convert.ToDouble(tick[16], GeneralUtil.NumberProvider);
                        tickDTO.QuantidadeMelhorOfertaVenda = Convert.ToDouble(tick[17], GeneralUtil.NumberProvider);
                        tickDTO.VolumeUltimoMinuto = Convert.ToDouble(tick[18], GeneralUtil.NumberProvider);

                        

                        //Acertando hora se necessario
                        if (tickDTO.Hora.Length == 3)
                            tickDTO.Hora = "0" + tickDTO.Hora;

                        //Demais dados
                        tickDTO.Volume = Convert.ToDouble(tickDTO.Quantidade) * tickDTO.Media;
                        if (tickDTO.Hora.Length == 4)
                            tickDTO.Data = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, Convert.ToInt32(tickDTO.Hora.Substring(0, 2)), Convert.ToInt32(tickDTO.Hora.Substring(2, 2)), 0);
                        else
                            tickDTO.Data = DateTime.Now;
                    }
                    else if (tick[0] == "I")
                    {
                        tickDTO.Ativo = tick[1];
                        tickDTO.Bolsa = 1;
                        tickDTO.Hora = tick[12];
                        tickDTO.Abertura = Convert.ToDouble(tick[15], GeneralUtil.NumberProvider);
                        tickDTO.FechamentoAnterior = Convert.ToDouble(tick[18], GeneralUtil.NumberProvider);
                        tickDTO.Ultimo = Convert.ToDouble(tick[3], GeneralUtil.NumberProvider);
                        tickDTO.Variacao = Convert.ToDouble(tick[7], GeneralUtil.NumberProvider);
                        tickDTO.Maximo = Convert.ToDouble(tick[4], GeneralUtil.NumberProvider);
                        tickDTO.Minimo = Convert.ToDouble(tick[6], GeneralUtil.NumberProvider);
                        tickDTO.Media = Convert.ToDouble(tick[16], GeneralUtil.NumberProvider);
                        tickDTO.NumeroNegocio = 0;
                        tickDTO.QuantidadeUltimoNegocio = 0;
                        tickDTO.Quantidade = 0;
                        tickDTO.MelhorOfertaCompra = 0;
                        tickDTO.QuantidadeMelhorOfertaCompra = 0;
                        tickDTO.MelhorOfertaVenda = 0;
                        tickDTO.QuantidadeMelhorOfertaVenda = 0;
                        //tickDTO.VolumeMinuto = Convert.ToDouble(tick[17], Util.NumberProvider);

                        //Acertando hora se necessario
                        if (tickDTO.Hora.Length == 3)
                            tickDTO.Hora = "0" + tickDTO.Hora;

                        //Demais dados
                        tickDTO.Volume = Convert.ToDouble(tick[2]);
                        if (tickDTO.Hora.Length == 4)
                            tickDTO.Data = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, Convert.ToInt32(tickDTO.Hora.Substring(0, 2)), Convert.ToInt32(tickDTO.Hora.Substring(2, 2)), 0);
                        else
                            tickDTO.Data = DateTime.Now;
                    }
                }


                return tickDTO;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que vai receber um tick e vai atualizar o cache do ativo caso o mesmo já esteja cacheado
        /// </summary>
        /// <param name="tickDTO"></param>
        private static void AtualizaCacheDiario(TickDTO tickDTO)
        {
            try
            {
                if (StaticData.cacheCotacaoDiario.ContainsKey(tickDTO.Ativo))
                {
                    if (StaticData.cacheCotacaoDiario[tickDTO.Ativo][StaticData.cacheCotacaoDiario[tickDTO.Ativo].Count - 1].Data.Date == tickDTO.Data.Date)
                    {
                        StaticData.cacheCotacaoDiario[tickDTO.Ativo][StaticData.cacheCotacaoDiario[tickDTO.Ativo].Count - 1].Abertura = tickDTO.Abertura;
                        StaticData.cacheCotacaoDiario[tickDTO.Ativo][StaticData.cacheCotacaoDiario[tickDTO.Ativo].Count - 1].Maximo = tickDTO.Maximo;
                        StaticData.cacheCotacaoDiario[tickDTO.Ativo][StaticData.cacheCotacaoDiario[tickDTO.Ativo].Count - 1].Minimo = tickDTO.Minimo;
                        StaticData.cacheCotacaoDiario[tickDTO.Ativo][StaticData.cacheCotacaoDiario[tickDTO.Ativo].Count - 1].Quantidade = tickDTO.Quantidade;
                        StaticData.cacheCotacaoDiario[tickDTO.Ativo][StaticData.cacheCotacaoDiario[tickDTO.Ativo].Count - 1].Ultimo = tickDTO.Ultimo;
                        StaticData.cacheCotacaoDiario[tickDTO.Ativo][StaticData.cacheCotacaoDiario[tickDTO.Ativo].Count - 1].Volume = tickDTO.Volume;
                    }
                    else if (StaticData.cacheCotacaoDiario[tickDTO.Ativo][StaticData.cacheCotacaoDiario[tickDTO.Ativo].Count - 1].Data.Date < tickDTO.Data.Date)
                    {
                        StaticData.cacheCotacaoDiario[tickDTO.Ativo].Add(new CotacaoDTO(tickDTO.Abertura, tickDTO.Maximo, tickDTO.Minimo, tickDTO.Ultimo,
                            tickDTO.Quantidade, tickDTO.Volume, tickDTO.Data, false, tickDTO.Hora));
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Metodo que faz a atualização do cache intraday
        /// </summary>
        /// <param name="tickDTO"></param>
        private static void AtualizaCacheIntraday(TickDTO tickDTO)
        {
            try
            {
                if (StaticData.cacheCotacaoIntraday.ContainsKey(tickDTO.Ativo))
                {
                    if (StaticData.cacheCotacaoIntraday[tickDTO.Ativo][StaticData.cacheCotacaoIntraday[tickDTO.Ativo].Count - 1].Data == tickDTO.Data)
                    {
                        StaticData.cacheCotacaoIntraday[tickDTO.Ativo][StaticData.cacheCotacaoIntraday[tickDTO.Ativo].Count - 1].Maximo =
                            Math.Max(StaticData.cacheCotacaoIntraday[tickDTO.Ativo][StaticData.cacheCotacaoIntraday[tickDTO.Ativo].Count - 1].Maximo, tickDTO.Ultimo);
                        StaticData.cacheCotacaoIntraday[tickDTO.Ativo][StaticData.cacheCotacaoIntraday[tickDTO.Ativo].Count - 1].Minimo = 
                            Math.Min(StaticData.cacheCotacaoIntraday[tickDTO.Ativo][StaticData.cacheCotacaoIntraday[tickDTO.Ativo].Count - 1].Minimo,tickDTO.Ultimo);
                        StaticData.cacheCotacaoIntraday[tickDTO.Ativo][StaticData.cacheCotacaoIntraday[tickDTO.Ativo].Count - 1].Ultimo = tickDTO.Ultimo;
                        StaticData.cacheCotacaoIntraday[tickDTO.Ativo][StaticData.cacheCotacaoIntraday[tickDTO.Ativo].Count - 1].Volume = tickDTO.VolumeUltimoMinuto;

                        //StaticData.cacheCotacaoDiario[tickDTO.Ativo][StaticData.cacheCotacaoIntraday[tickDTO.Ativo].Count - 1].Quantidade = tickDTO.Quantidade;                        
                        
                    }
                    else 
                    {
                        StaticData.cacheCotacaoIntraday[tickDTO.Ativo].Add(new CotacaoDTO(tickDTO.Ultimo, tickDTO.Ultimo, tickDTO.Ultimo, tickDTO.Ultimo,
                            0, tickDTO.VolumeUltimoMinuto, tickDTO.Data, false, tickDTO.Hora));
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        

        #endregion
    }
}
