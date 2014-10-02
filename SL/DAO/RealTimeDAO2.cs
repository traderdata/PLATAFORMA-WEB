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
    public static class RealTimeDAO2
    {
        #region Variaveis Privadas

        static System.Collections.Generic.List<string> lista = new System.Collections.Generic.List<string>();

        //variavel de controle de realtime
        private static FM.WebSync.Silverlight.Core.Client clientChat;

        //canal de publicação
        private static Publisher publisherChat;

        //variavel de controle de realtime
        private static FM.WebSync.Silverlight.Core.Client clientScanner;

        //variavel de controle de realtime
        private static FM.WebSync.Silverlight.Core.Client clientBVSPTick;

        //variavel de controle de realtime
        private static FM.WebSync.Silverlight.Core.Client clientBMFTick;

        //canal de publicação
        private static Publisher publisherBVSPTick;

        //canal de publicação
        private static Publisher publisherBMFTick;

        //variavel de controle de realtime
        private static FM.WebSync.Silverlight.Core.Client clientBVSPBook;

        //variavel de controle de realtime
        private static FM.WebSync.Silverlight.Core.Client clientBMFBook;

        //canal de publicação
        private static Publisher publisherBVSPBook;

        //canal de publicação
        private static Publisher publisherBMFBook;

        //variavel de controle de realtime
        private static FM.WebSync.Silverlight.Core.Client clientBVSPTrade;

        //variavel de controle de realtime
        private static FM.WebSync.Silverlight.Core.Client clientBMFTrade;
                
        /// <summary> Evento genérico, que participa do processo de alternativa ao uso de invoke.</summary>
        static internal event SendOrPostCallback GenericEventHandler;

        //Variável que permitirá executar um método no contexto da thread principal, evitando a necessidade de invoke pela aplicacao cliente
        static internal AsyncOperation asyncOperation;


        #endregion

        #region Eventos

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de mensganes do scanner
        /// </summary>
        /// <param name="tick"></param>
        public delegate void ChatHandler(object Result);

        /// <summary>Evento disparado quando a ação de StartTickSubscription é executada.</summary>
        public static event ChatHandler ChatMessageReceived;

        /// <summary>
        /// Representa o método que irá manipular o evento de recebimento de tick.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void ScannerHandler(object Result);

        /// <summary>Evento disparado quando a ação de StartTickSubscription é executada.</summary>
        public static event ScannerHandler ScannerReceived;

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
        /// Representa o método que irá manipular o evento de recebimento de book.
        /// </summary>
        /// <param name="tick"></param>
        public delegate void BookHandler(object Result);

        /// <summary>Evento disparado quando a ação de StartBookSubscription é executada.</summary>
        public static event BookHandler BookReceived;

        /// <summary>
        /// Representa o método que irá manipular o evento de conexao BVSP RT
        /// </summary>
        /// <param name="tick"></param>
        public delegate void ConnectBVSPHandler();

        /// <summary>Evento disparado quando a ação de ConnectBVSPRT é executada.</summary>
        public static event ConnectBVSPHandler OnConnectSuccessBVSP;

        /// <summary>
        /// Representa o método que irá manipular o evento de conexao BVSP RT
        /// </summary>
        /// <param name="tick"></param>
        public delegate void ConnectErrorBVSPHandler();

        /// <summary>Evento disparado quando a ação de ConnectBVSPRT é executada.</summary>
        public static event ConnectErrorBVSPHandler OnConnectErrorBVSP;

        /// <summary>
        /// Representa o método que irá manipular o evento de conexao BMF RT
        /// </summary>
        /// <param name="tick"></param>
        public delegate void ConnectBMFHandler();

        /// <summary>Evento disparado quando a ação de ConnectBMF é executada.</summary>
        public static event ConnectBMFHandler OnConnectSuccessBMF;

        /// <summary>
        /// Representa o método que irá manipular o evento de conexao BMF RT
        /// </summary>
        /// <param name="tick"></param>
        public delegate void ConnectErrorBMFHandler();

        /// <summary>Evento disparado quando a ação de ConnectBMFRT é executada.</summary>
        public static event ConnectErrorBMFHandler OnConnectErrorBMF;

        #endregion

        #region Metodos
        /// <summary>
        /// Metodo que faz a conexao no canal
        /// </summary>
        public static void ConnectChat()
        {
            try
            {
                //conectando o publicador
                publisherChat = new Publisher(new PublisherArgs
                {
                    RequestUrl = StaticData.URLChatServer
                });

                //conectando o hubd de dados em rt                
                clientChat = new FM.WebSync.Silverlight.Core.Client(new ClientArgs
                {
                    RequestUrl = StaticData.URLChatServer
                });

                //Conectando
                clientChat.Connect(new ConnectArgs
                {
                    //StayConnected = true,
                    OnSuccess = (args) =>
                    {
                        clientChat.Subscribe(new SubscribeArgs
                        {
                            Channel = "/open",
                            OnSuccess = (args2) =>
                            {

                            },
                            OnFailure = (args2) =>
                            {
                                //codigo de falha de assinatura
                            },
                            OnReceive = (args2) =>
                            {
                                if (ChatMessageReceived != null)
                                {
                                    //dispara evento para aqueles forms que estão aguardando o dado tick a tick
                                    //Assinando o evento de disparo de dados
                                    GenericEventHandler = new SendOrPostCallback(ChatMessageReceived);
                                    asyncOperation.Post(GenericEventHandler, JSON.Deserialize<MsgDTO>(args2.DataJson));

                                }
                            }
                        });
                    },
                    OnFailure = (args) =>
                    {
                        
                    },
                    OnStreamFailure = (args) =>
                    {
                        //codigo necessario no insucesso da conexao
                        clientChat.Reconnect();
                    }
                });

                
            }
            catch
            {
            }
        }

        /// <summary>
        /// Metodo que vai fazer a publicação de uma mensagem no canal
        /// </summary>
        /// <param name="msg"></param>
        public static void PublishMessageToChat(MsgDTO msg)
        {
            publisherChat.Publish(new PublicationArgs
            {
                Publication = new Publication
                {
                    Channel = "/open",
                    DataJson = JSON.Serialize(msg)
                },
                OnComplete = (completeArgs) =>
                {

                },
                OnException = (exceptionArgs) =>
                {

                }
            });
        }

        /// <summary>
        /// Metodo que faz a conexao no scanner intraday
        /// </summary>
        public static void ConnectScannerIntraday()
        {
            //conectando o hubd de dados em rt                
            clientScanner = new FM.WebSync.Silverlight.Core.Client(new ClientArgs
            {
                RequestUrl = StaticData.URLScannerIntraday
            });

                //Conectando
            clientScanner.Connect(new ConnectArgs
            {
                //StayConnected = true,
                OnSuccess = (args) =>
                {
                    clientScanner.Subscribe(new SubscribeArgs
                    {
                        Channel = "/sc-intraday",
                        OnSuccess = (args2) =>
                        {

                        },
                        OnFailure = (args2) =>
                        {
                            //codigo de falha de assinatura
                        },
                        OnReceive = (args2) =>
                        {
                            if (ScannerReceived != null)
                            {
                                //convertendo para tickDTO
                                ScannerDTO scanner = new ScannerDTO();
                                scanner.Ativo = args2.DataJson.Split(':')[1];
                                scanner.Hora = args2.DataJson.Split(':')[4].Substring(0, 2) + ":" + args2.DataJson.Split(':')[4].Substring(2, 2);
                                scanner.Ultimo = Convert.ToDouble(args2.DataJson.Split(':')[8], GeneralUtil.NumberProvider);
                                if (args2.DataJson.Split(':')[3] == "1")
                                    scanner.Periodicidade = args2.DataJson.Split(':')[3] + " minuto";
                                else
                                    scanner.Periodicidade = args2.DataJson.Split(':')[3] + " minutos";

                                switch (args2.DataJson.Split(':')[2])
                                {
                                    case "1":
                                        scanner.Estrategia = "Trix indicando compra  - 1 minuto";
                                        break;
                                    case "2":
                                        scanner.Estrategia = "Trix indicando compra  - 5 minutos";
                                        break;
                                    case "3":
                                        scanner.Estrategia = "Trix indicando compra  - 15 minutos";
                                        break;
                                        case "4":
                                        scanner.Estrategia = "Trix indicando compra  - 30 minutos";
                                        break;
                                        case "5":
                                        scanner.Estrategia = "Trix indicando compra  - 60 minutos";
                                        break;
                                        case "6":
                                        scanner.Estrategia = "Trix indicando venda  - 1 minuto";
                                        break;
                                        case "7":
                                        scanner.Estrategia = "Trix indicando venda  - 5 minutos";
                                        break;
                                        case "8":
                                        scanner.Estrategia = "Trix indicando venda  - 15 minutos";
                                        break;
                                        case "9":
                                        scanner.Estrategia = "Trix indicando venda  - 30 minutos";
                                        break;
                                        case "10":
                                        scanner.Estrategia = "Trix indicando venda  - 60 minutos";
                                        break;
                                        case "11":
                                        scanner.Estrategia = "MME 5 cruza MME 21 para cima  - 1 minuto";
                                        break;
                                    case "12":
                                        scanner.Estrategia = "MME 5 cruza MME 21 para cima  - 5 minutos";
                                        break;
                                    case "13":
                                        scanner.Estrategia = "MME 5 cruza MME 21 para cima  - 15 minutos";
                                        break;
                                    case "14":
                                        scanner.Estrategia = "MME 5 cruza MME 21 para cima  - 30 minutos";
                                        break;
                                    case "15":
                                        scanner.Estrategia = "MME 5 cruza MME 21 para cima  - 60 minutos";
                                        break;
                                    case "16":
                                        scanner.Estrategia = "MME 5 cruza MME 21 para baixo  - 1 minuto";
                                        break;
                                    case "17":
                                        scanner.Estrategia = "MME 5 cruza MME 21 para baixo  - 5 minutos";
                                        break;
                                    case "18":
                                        scanner.Estrategia = "MME 5 cruza MME 21 para baixo  - 15 minutos";
                                        break;
                                    case "19":
                                        scanner.Estrategia = "MME 5 cruza MME 21 para baixo  - 30 minutos";
                                        break;
                                    case "20":
                                        scanner.Estrategia = "MME 5 cruza MME 21 para baixo  - 60 minutos";
                                        break;
                                    case "21":
                                        scanner.Estrategia = "MME 30 cruza MME 60 para cima  - 1 minuto";
                                        break;
                                    case "22":
                                        scanner.Estrategia = "MME 30 cruza MME 60 para cima  - 5 minutos";
                                        break;
                                    case "23":
                                        scanner.Estrategia = "MME 30 cruza MME 60 para cima  - 15 minutos";
                                        break;
                                    case "24":
                                        scanner.Estrategia = "MME 30 cruza MME 60 para cima  - 30 minutos";
                                        break;
                                    case "25":
                                        scanner.Estrategia = "MME 30 cruza MME 60 para cima  - 60 minuto";
                                        break;
                                    case "26":
                                        scanner.Estrategia = "MME 30 cruza MME 60 para baixo - 1 minuto";
                                        break;
                                    case "27":
                                        scanner.Estrategia = "MME 30 cruza MME 60 para baixo - 5 minutos";
                                        break;
                                    case "28":
                                        scanner.Estrategia = "MME 30 cruza MME 60 para baixo - 15 minutos";
                                        break;
                                    case "29":
                                        scanner.Estrategia = "MME 30 cruza MME 60 para baixo - 30 minutos";
                                        break;
                                    case "30":
                                        scanner.Estrategia = "MME 30 cruza MME 60 para baixo - 60 minutos";
                                        break;
                                    case "31":
                                        scanner.Estrategia = "MME 5 cruza MME 21 e IFR abaixo de 30  - 1 minuto";
                                        break;
                                    case "32":
                                        scanner.Estrategia = "MME 5 cruza MME 21 e IFR abaixo de 30  - 5 minutoS";
                                        break;
                                    case "33":
                                        scanner.Estrategia = "MME 5 cruza MME 21 e IFR abaixo de 30  - 15 minutos";
                                        break;
                                    case "34":
                                        scanner.Estrategia = "MME 5 cruza MME 21 e IFR abaixo de 30  - 30 minutos";
                                        break;
                                    case "35":
                                        scanner.Estrategia = "MME 5 cruza MME 21 e IFR abaixo de 30  - 60 minutos";
                                        break;
                                    case "36":
                                        scanner.Estrategia = "MME 5 cruza MME 21 e IFR acima de 70 - 1 minuto";
                                        break;
                                    case "37":
                                        scanner.Estrategia = "MME 5 cruza MME 21 e IFR acima de 70 - 5 minutos";
                                        break;
                                    case "38":
                                        scanner.Estrategia = "MME 5 cruza MME 21 e IFR acima de 70 - 15 minutos";
                                        break;
                                    case "39":
                                        scanner.Estrategia = "MME 5 cruza MME 21 e IFR acima de 70 - 30 minutos";
                                        break;
                                    case "40":
                                        scanner.Estrategia = "MME 5 cruza MME 21 e IFR acima de 70 - 60 minutos";
                                        break;
                                    case "41":
                                        scanner.Estrategia = "Estocastico cruza sua própria media de 3 periodos indicando compra - 1 minuto";
                                        break;
                                    case "42":
                                        scanner.Estrategia = "Estocastico cruza sua própria media de 3 periodos indicando compra - 5 minutos";
                                        break;
                                    case "43":
                                        scanner.Estrategia = "Estocastico cruza sua própria media de 3 periodos indicando compra - 15 minutos";
                                        break;
                                    case "44":
                                        scanner.Estrategia = "Estocastico cruza sua própria media de 3 periodos indicando compra - 30 minutos";
                                        break;
                                    case "45":
                                        scanner.Estrategia = "Estocastico cruza sua própria media de 3 periodos indicando compra - 60 minutos";
                                        break;
                                    case "46":
                                        scanner.Estrategia = "Estocastico cruza sua própria media de 3 periodos indicando venda - 1 minuto";
                                        break;
                                    case "47":
                                        scanner.Estrategia = "Estocastico cruza sua própria media de 3 periodos indicando venda - 5 minutos";
                                        break;
                                    case "48":
                                        scanner.Estrategia = "Estocastico cruza sua própria media de 3 periodos indicando venda - 15 minutos";
                                        break;
                                    case "49":
                                        scanner.Estrategia = "Estocastico cruza sua própria media de 3 periodos indicando venda - 30 minutos";
                                        break;
                                    case "50":
                                        scanner.Estrategia = "Estocastico cruza sua própria media de 3 periodos indicando venda - 60 minutos";
                                        break;
                                    case "51":
                                        scanner.Estrategia = "Agulhada indicando compra - 1 minuto";
                                        break;
                                    case "52":
                                        scanner.Estrategia = "Agulhada indicando compra - 5 minutos";
                                        break;
                                    case "53":
                                        scanner.Estrategia = "Agulhada indicando compra - 15 minutos";
                                        break;
                                    case "54":
                                        scanner.Estrategia = "Agulhada indicando compra - 30 minutos";
                                        break;
                                    case "55":
                                        scanner.Estrategia = "Agulhada indicando compra - 60 minutos";
                                        break;
                                    case "56":
                                        scanner.Estrategia = "Agulhada indicando venda - 1 minuto";
                                        break;
                                    case "57":
                                        scanner.Estrategia = "Agulhada indicando venda - 5 minutos";
                                        break;
                                    case "58":
                                        scanner.Estrategia = "Agulhada indicando venda - 15 minutos";
                                        break;
                                    case "59":
                                        scanner.Estrategia = "Agulhada indicando venda - 30 minutos";
                                        break;
                                    case "60":
                                        scanner.Estrategia = "Agulhada indicando venda - 60 minutos";
                                        break;

                                }



                                //dispara evento para aqueles forms que estão aguardando o dado tick a tick
                                //Assinando o evento de disparo de dados
                                GenericEventHandler = new SendOrPostCallback(ScannerReceived);
                                asyncOperation.Post(GenericEventHandler, scanner);

                            }
                            

                        }
                    });
                },
                OnFailure = (args) =>
                {
                    
                },
                OnStreamFailure = (args) =>
                {
                    //codigo necessario no insucesso da conexao
                    clientScanner.Reconnect();                                        
                }
            });
        }

        /// <summary>
        /// Metodo que estabelece a conexao do sistema com o sistema de RT
        /// </summary>
        public static void ConnectBVSP(bool RT)
        {
            string urlBVSPTick = "";
            string urlBVSPTrade = "";
            string urlBVSPBook = "";

            if (RT)
            {
                urlBVSPTick = StaticData.BVSPRTTickHost;
                urlBVSPTrade = StaticData.BVSPRTTradeHost;
                urlBVSPBook = StaticData.BVSPRTBookHost;
            }
            else
                urlBVSPTick = StaticData.BVSPDelayHost;

            //conectando o publicador
            publisherBVSPTick = new Publisher(new PublisherArgs
            {
                RequestUrl = urlBVSPTick
            });

            //conectando o publicador
            publisherBVSPBook = new Publisher(new PublisherArgs
            {
                RequestUrl = urlBVSPBook
            });

            //conectando o hubd de dados em rt                
            clientBVSPTick = new FM.WebSync.Silverlight.Core.Client(new ClientArgs
            {
                RequestUrl = urlBVSPTick
            });

            //conectando o hubd de dados em rt                
            clientBVSPTrade = new FM.WebSync.Silverlight.Core.Client(new ClientArgs
            {
                RequestUrl = urlBVSPTrade
            });

            //conectando o hubd de dados em rt                
            clientBVSPBook = new FM.WebSync.Silverlight.Core.Client(new ClientArgs
            {
                RequestUrl = urlBVSPBook
            });

            //Conectando
            clientBVSPTick.Connect(new ConnectArgs
            {
                //StayConnected = true,
                OnSuccess = (args) =>
                {                    
                    //Disparar evento de sucesso
                    if (OnConnectSuccessBVSP != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            OnConnectSuccessBVSP();
                        });
                    }
                },
                OnFailure = (args) =>
                {
                    //Disparar evento de falha
                    if (OnConnectErrorBVSP != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            OnConnectErrorBVSP();
                        });
                    }
                },
                OnStreamFailure = (args) =>
                {
                    //codigo necessario no insucesso da conexao
                    clientBVSPTick.Reconnect();                                        
                }
            });

            //Conectando
            clientBVSPTrade.Connect(new ConnectArgs
            {
                //StayConnected = true,
                OnSuccess = (args) =>
                {
                    
                },
                OnFailure = (args) =>
                {
                    //Disparar evento de falha
                    if (OnConnectErrorBVSP != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            OnConnectErrorBVSP();
                        });
                    }
                },
                OnStreamFailure = (args) =>
                {
                    //codigo necessario no insucesso da conexao
                    clientBVSPTrade.Reconnect();
                }
            });

            //Conectando
            clientBVSPBook.Connect(new ConnectArgs
            {
                //StayConnected = true,
                OnSuccess = (args) =>
                {
                    //Disparar evento de sucesso
                    if (OnConnectSuccessBVSP != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            OnConnectSuccessBVSP();
                        });
                    }
                },
                OnFailure = (args) =>
                {
                    //Disparar evento de falha
                    if (OnConnectErrorBVSP != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            OnConnectErrorBVSP();
                        });
                    }
                },
                OnStreamFailure = (args) =>
                {
                    //codigo necessario no insucesso da conexao
                    clientBVSPBook.Reconnect();
                }
            });
            // Cria uma instância de uma AsyncOperation para gerenciar o contexto
            asyncOperation = AsyncOperationManager.CreateOperation(null);
        }

        /// <summary>
        /// Metodo que estabelece a conexao do sistema com o sistema de RT
        /// </summary>
        public static void ConnectBMF(bool RT)
        {
            string urlBMFTick = "";
            string urlBMFTrade = "";
            string urlBMFBook = "";
            if (RT)
            {
                urlBMFTick = StaticData.BMFRTTickHost;
                urlBMFBook = StaticData.BMFRTBookHost;
                urlBMFTrade = StaticData.BMFRTTradeHost;
            }
            else
            {
                urlBMFTick = StaticData.BMFDelayTickHost;
            }

            //conectando o publicador
            publisherBMFTick = new Publisher(new PublisherArgs
            {
                RequestUrl = urlBMFTick
            });
            publisherBMFBook = new Publisher(new PublisherArgs
            {
                RequestUrl = urlBMFBook
            });            

            //conectando o hubd de dados em rt                
            clientBMFTick = new FM.WebSync.Silverlight.Core.Client(new ClientArgs
            {
                RequestUrl = urlBMFTick
            });
            //conectando o hubd de dados em rt                
            clientBMFTrade = new FM.WebSync.Silverlight.Core.Client(new ClientArgs
            {
                RequestUrl = urlBMFTrade
            });
            //conectando o hubd de dados em rt                
            clientBMFBook = new FM.WebSync.Silverlight.Core.Client(new ClientArgs
            {
                RequestUrl = urlBMFBook
            });

            //Conectando
            clientBMFTick.Connect(new ConnectArgs
            {
                //StayConnected = true,
                OnSuccess = (args) =>
                {
             
                },
                OnFailure = (args) =>
                {
                    //Disparar evento de falha
                    if (OnConnectErrorBMF != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            OnConnectErrorBMF();
                        });
                    }
                },
                OnStreamFailure = (args) =>
                {
                    //codigo necessario no insucesso da conexao
                    clientBMFTick.Reconnect();
                    
                }
            });

            //Conectando
            clientBMFTrade.Connect(new ConnectArgs
            {
                //StayConnected = true,
                OnSuccess = (args) =>
                {
             
                },
                OnFailure = (args) =>
                {
                    //Disparar evento de falha
                    if (OnConnectErrorBMF != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            OnConnectErrorBMF();
                        });
                    }
                },
                OnStreamFailure = (args) =>
                {
                    //codigo necessario no insucesso da conexao
                    clientBMFTrade.Reconnect();                    
                }
            });

            //Conectando
            clientBMFBook.Connect(new ConnectArgs
            {
                //StayConnected = true,
                OnSuccess = (args) =>
                {
                    
                },
                OnFailure = (args) =>
                {
                    //Disparar evento de falha
                    if (OnConnectErrorBMF != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            OnConnectErrorBMF();
                        });
                    }
                },
                OnStreamFailure = (args) =>
                {
                    //codigo necessario no insucesso da conexao
                    clientBMFBook.Reconnect();                    
                }
            });

            //Disparar evento de sucesso
            if (OnConnectSuccessBMF != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    OnConnectSuccessBMF();
                });
            }
            // Cria uma instância de uma AsyncOperation para gerenciar o contexto
            //asyncOperation = AsyncOperationManager.CreateOperation(null);
        }

        /// <summary>
        /// Metodo usado para assinar um determinado ativo
        /// </summary>
        /// <param name="ativo"></param>
        public static void StartBookSubscription(string ativo)
        {
            if (MarketDataDAO.IsBovespa(ativo))
            {
                if (clientBVSPBook != null)
                {
                    clientBVSPBook.Subscribe(new SubscribeArgs
                    {
                        Channel = "/" + ativo,
                        OnSuccess = (args) =>
                        {
                            publisherBVSPBook.Publish(new PublicationArgs
                            {
                                Publication = new Publication
                                {
                                    Channel = "/incoming",
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
                            if (BookReceived != null)
                            {
                                //convertendo para tickDTO
                                BookDTO book = null;
                                
                                //dispara evento para aqueles forms que estão aguardando o dado tick a tick
                                //Assinando o evento de disparo de dados
                                GenericEventHandler = new SendOrPostCallback(BookReceived);
                                asyncOperation.Post(GenericEventHandler, book);

                            }
                        }
                    });
                }
            }
            else
            {
                if (clientBMFBook != null)
                {
                    clientBMFBook.Subscribe(new SubscribeArgs
                    {
                        Channel = "/" + ativo,
                        OnSuccess = (args) =>
                        {
                            publisherBMFBook.Publish(new PublicationArgs
                            {
                                Publication = new Publication
                                {
                                    Channel = "/incoming",
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
                            //throw new Exception("Erro ao assinar book de ativo");
                        },
                        OnReceive = (args) =>
                        {
                            if (BookReceived != null)
                            {
                                BookDTO book = null;

                                //dispara evento para aqueles forms que estão aguardando o dado tick a tick
                                //Assinando o evento de disparo de dados
                                GenericEventHandler = new SendOrPostCallback(BookReceived);
                                asyncOperation.Post(GenericEventHandler, book);

                            }
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Metodo usado para assinar um determinado ativo
        /// </summary>
        /// <param name="ativo"></param>
        public static void StartTickSubscription(string ativo)
        {
            if (MarketDataDAO.IsBovespa(ativo))
            {
                if (clientBVSPTick != null)
                {
                    clientBVSPTick.Subscribe(new SubscribeArgs
                    {
                        Channel = "/" + ativo,
                        OnSuccess = (args) =>
                        {
                            publisherBVSPTick.Publish(new PublicationArgs
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

                                if (tick.Ativo == "PETR4")
                                {
                                    lista.Add(JSON.Deserialize<string>(args.DataJson));                                   
                                }

                                //faço a atualização do cache intraday
                                AtualizaCacheIntraday(tick);

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
            else
            {
                if (clientBMFTick != null)
                {
                    clientBMFTick.Subscribe(new SubscribeArgs
                    {
                        Channel = "/" + ativo,
                        OnSuccess = (args) =>
                        {
                            publisherBMFTick.Publish(new PublicationArgs
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
                                AtualizaCacheIntraday(tick);

                                //dispara evento para aqueles forms que estão aguardando o dado tick a tick
                                //Assinando o evento de disparo de dados
                                GenericEventHandler = new SendOrPostCallback(TickReceived);
                                asyncOperation.Post(GenericEventHandler, tick);

                            }
                        }
                    });
                }
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
                if (clientBVSPTrade != null)
                {
                    clientBVSPTrade.Subscribe(new SubscribeArgs
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
            else
            {
                if (clientBMFTrade != null)
                {
                    clientBMFTrade.Subscribe(new SubscribeArgs
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
