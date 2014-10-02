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
using ModulusFE.LineStudies;
using ModulusFE.Indicators;
using ModulusFE;
using System.ServiceModel;
using System.Collections.Generic;
using Traderdata.Client.TerminalWEB.DTO;
using ModulusFE.SL;
using System.IO.IsolatedStorage;

namespace Traderdata.Client.TerminalWEB
{
    public static class StaticData
    {
        #region AWS

        /// <summary>
        /// variavel que armazena o S3Endpoint
        /// </summary>
        public static string S3Endpoint = "https://s3-sa-east-1.amazonaws.com/";

        #endregion

        #region Ferramentas

        /// <summary>
        /// MacroAção selecionada na mainpage
        /// </summary>
        public static TipoAcao tipoAcao = TipoAcao.Seta;

        /// <summary>
        /// Ferramenta de desenho selecionada
        /// </summary>
        public static TipoFerramenta tipoFerramenta = TipoFerramenta.Nenhum;

        /// <summary>
        /// Variavel que controla a ferramenta selecionada
        /// </summary>
        //public static string ferramentaSelecionada = "SETA";

        /// <summary>
        /// Variavel que armazena o tipo de indicador que fora selecionado
        /// </summary>
        public static IndicatorType tipoIndicador = IndicatorType.Unknown;

        /// <summary>
        /// Cor selecionada na caixa de seleção de cores da MainPage
        /// </summary>
        public static Color corSelecionada = Colors.Black;

        /// <summary>
        /// Estilo de linha selecionao na toolbar na MainPage
        /// </summary>
        public static LinePattern estiloLinhaSelecionado = LinePattern.Solid;

        /// <summary>
        /// Grossura da linha selecionada
        /// </summary>
        public static int strokeThickness = 1;

        /// <summary>
        /// Faz a conversao entre a ferramenta selecionado e um objeto LineStudy
        /// </summary>
        /// <returns></returns>
        public static LineStudy.StudyTypeEnum LineStudySelecionado()
        {
            switch (tipoFerramenta)
            {
                case TipoFerramenta.RetaTendencia:
                    return LineStudy.StudyTypeEnum.TrendLine;                    
                case TipoFerramenta.Elipse:
                    return LineStudy.StudyTypeEnum.Ellipse;
                case TipoFerramenta.ErrorChannel:
                    return LineStudy.StudyTypeEnum.ErrorChannel;
                case TipoFerramenta.FiboArcs:
                    return LineStudy.StudyTypeEnum.FibonacciArcs;
                case TipoFerramenta.FiboFan:
                    return LineStudy.StudyTypeEnum.FibonacciFan;
                case TipoFerramenta.FiboRetracement:
                    return LineStudy.StudyTypeEnum.FibonacciRetracements;
                case TipoFerramenta.FiboTimezone:
                    return LineStudy.StudyTypeEnum.FibonacciTimeZones;
                case TipoFerramenta.GannFan:
                    return LineStudy.StudyTypeEnum.GannFan;
                case TipoFerramenta.RetaHorizontal:
                case TipoFerramenta.RetaSuporte:
                case TipoFerramenta.RetaResistencia:
                    return LineStudy.StudyTypeEnum.HorizontalLine;
                case TipoFerramenta.QuadrantLines:
                    return LineStudy.StudyTypeEnum.QuadrantLines;
                case TipoFerramenta.Image:
                    return LineStudy.StudyTypeEnum.ImageObject;
                case TipoFerramenta.RaffRegression:
                    return LineStudy.StudyTypeEnum.RaffRegression;
                case TipoFerramenta.Retangulo:
                    return LineStudy.StudyTypeEnum.Rectangle;
                case TipoFerramenta.SpeedLine:
                    return LineStudy.StudyTypeEnum.SpeedLines;
                case TipoFerramenta.Texto:
                case TipoFerramenta.ValorY:
                    return LineStudy.StudyTypeEnum.StaticText;
                case TipoFerramenta.Compra:
                case TipoFerramenta.Vende:
                case TipoFerramenta.Signal:
                    return LineStudy.StudyTypeEnum.SymbolObject;
                case TipoFerramenta.TironeLevels:
                    return LineStudy.StudyTypeEnum.TironeLevels;
                case TipoFerramenta.RetaVertical:
                    return LineStudy.StudyTypeEnum.VerticalLine;
                    
            }
            return LineStudy.StudyTypeEnum.Unknown;
        }

        /// <summary>
        /// SymbolType selecionado
        /// </summary>
        /// <returns></returns>
        public static SymbolType SymbolTypeSelecionado()
        {
            switch (tipoFerramenta)
            {
                case TipoFerramenta.Compra:
                    return SymbolType.Buy;
                case TipoFerramenta.Vende:
                    return SymbolType.Sell;
                case TipoFerramenta.Signal:
                    return SymbolType.Signal;
            }
            return SymbolType.Buy;
        }

        #endregion

        #region Indicadores

        public static List<IndicatorInfoDTO> listaIndicadores = new List<IndicatorInfoDTO>();

        public static List<IndicatorInfoDTO> GetListaIndicadores()
        {
            return listaIndicadores;
            //return ExtensionMethods.DeepCopy<List<IndicatorInfoDTO>>(listaIndicadores);
        }
        
        #endregion

        #region WCF

        /// <summary>
        /// String de URL para marketdata Service
        /// </summary>
        public static string UrlWebservice = "";
        
        /// <summary>
        /// Metodo que retorna o endpointAdress com a url passado
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static EndpointAddress MarketDataEndpoint()
        {
            return new EndpointAddress(UrlWebservice);
        }

        /// <summary>
        /// Metodo que retorna uma propriedade de basic bind
        /// </summary>
        /// <returns></returns>
        public static BasicHttpBinding BasicHttpBind()
        {
            BasicHttpBinding basicBind = new BasicHttpBinding();
            basicBind.SendTimeout = new TimeSpan(1, 0, 0);
            basicBind.ReceiveTimeout = new TimeSpan(1, 0, 0);
            basicBind.OpenTimeout = new TimeSpan(1, 0, 0);
            basicBind.MaxReceivedMessageSize = 9999999;            
            if (MarketDataEndpoint().Uri.OriginalString.Contains("https"))
                basicBind.Security.Mode = BasicHttpSecurityMode.Transport;

            return basicBind;

        }

        #endregion

        #region RealTime

        /// <summary>
        /// Variavel que recebe o caminho do chat server
        /// </summary>
        public static string URLChatServer = "";

        /// <summary>
        /// Variavel que guarda a string de conexao no scanner intraday
        /// </summary>
        public static string URLScannerIntraday = "";

        /// <summary>
        /// String de conexao ao servidor RT Bovespa
        /// </summary>
        public static string BVSPRTTickHost = "";

        /// <summary>
        /// String de conexao ao servidor RT Bovespa
        /// </summary>
        public static string BVSPRTTradeHost = "";

        /// <summary>
        /// String de conexao ao servidor RT Bovespa
        /// </summary>
        public static string BVSPRTBookHost = "";

        /// <summary>
        /// String de conexao ao servidor DELAY Bovespa
        /// </summary>
        public static string BVSPDelayHost = "";

        /// <summary>
        /// String de conexao ao servidor RT BMF
        /// </summary>
        public static string BMFRTTickHost = "";

        /// <summary>
        /// String de conexao ao servidor RT BMF
        /// </summary>
        public static string BMFRTTradeHost = "";

        /// <summary>
        /// String de conexao ao servidor RT BMF
        /// </summary>
        public static string BMFRTBookHost = "";

        /// <summary>
        /// String de conexao ao servidor DELAY BMF
        /// </summary>
        public static string BMFDelayTickHost = "";

        #endregion

        #region Cache Informações

        public static Dictionary<string, List<CotacaoDTO>> cacheCotacaoDiario = new Dictionary<string, List<CotacaoDTO>>();
        public static Dictionary<string, List<CotacaoDTO>> cacheCotacaoIntraday = new Dictionary<string, List<CotacaoDTO>>();
        public static Dictionary<string, List<CotacaoDTO>> cacheCotacaoIntradayS3 = new Dictionary<string, List<CotacaoDTO>>();
        public static Dictionary<string, List<AtivoDTO>> cacheAtivosPorIndice = new Dictionary<string, List<AtivoDTO>>();
        public static Dictionary<string, List<AtivoDTO>> cacheAtivosPorSegmento = new Dictionary<string, List<AtivoDTO>>();
        public static List<string> cacheIndices = new List<string>();
        public static List<AtivoDTO> cacheAtivosBMFTodos = new List<AtivoDTO>();
        public static List<AtivoDTO> cacheAtivosBMFPrincpalCheio = new List<AtivoDTO>();
        public static List<AtivoDTO> cacheAtivosBMFMiniContrato = new List<AtivoDTO>();
        public static List<AtivoDTO> cacheAtivosBovespaTodos = new List<AtivoDTO>();
        public static List<AtivoDTO> cacheAtivosBovespaVista = new List<AtivoDTO>();
        public static List<AtivoDTO> cacheAtivosBovespaOpcao = new List<AtivoDTO>();
        public static List<AtivoDTO> cacheAtivosBovespaTermo = new List<AtivoDTO>();
        public static List<AtivoDTO> cachePortfolioPadraoBovespa = new List<AtivoDTO>();        
        public static List<string> cacheSegmentos = new List<string>();

        public static bool CacheHabilitado = true;
        
        #endregion

        #region Usuario

        /// <summary>
        /// variavel que identifica se o usuario esta vindo por dentro do facebook
        /// </summary>
        public static bool FacebookIntegrationLogin = false;

        /// <summary>
        /// Variavel que armazena as configurações locais do usuario
        /// </summary>
        public static IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;


        /// <summary>
        /// Variavel que armazena quem é o distribuidor responsável por este cliente
        /// </summary>
        public static int DistribuidorId;

        /// <summary>
        /// Variavel que armazena o codigo do usuario dentro da Traderdata
        /// </summary>
        public static TerminalWebSVC.UsuarioDTO User;

        /// <summary>
        /// Variavel que controla qual o workspace
        /// </summary>
        public static TerminalWebSVC.WorkspaceDTO Workspace = new TerminalWebSVC.WorkspaceDTO();

        /// <summary>
        /// Variavel que controla o perfil do usuario localmente
        /// </summary>
        public static string Perfil = "B";

        public static bool BovespaRT { get; set; }
        public static bool BMFRT { get; set; }
        public static bool BovespaDelay { get; set; }
        public static bool BMFDelay { get; set; }
        public static bool BovespaEOD { get; set; }
        public static bool BMFEOD { get; set; }

        #endregion

        #region Log
        /// <summary>
        /// Variavel de log
        /// </summary>
        public static List<LogDTO> listaLog = new List<LogDTO>();

        /// <summary>
        /// Metodo que adiciona item no log
        /// </summary>
        /// <param name="texto"></param>
        public static void AddLog(string texto)
        {
            listaLog.Add(new LogDTO(DateTime.UtcNow, texto));
        }

        #endregion

        #region Outros

        public static bool FerramentasAuxiliaresVisiveis = true;

        public static Dictionary<int, string> CorretoraBovespa = new Dictionary<int, string>();

        public static List<TerminalWebSVC.PortfolioDTO> Portfolios = new List<TerminalWebSVC.PortfolioDTO>();

        public static int RefId = 0;

        public static int TempoDemo = 0;

        public static string WaterMark = "";

        public static bool WorkspaceSalvo { get; set; }

        #endregion

        #region Habilitacao Plugins Laterias

        public static bool PluginChat = true;
        public static bool PluginRastreadorEOD = true;
        public static bool PluginRastreadorRT = true;
        public static bool PluginVideoAula = true;
        public static bool PluginPortfolio = true;
        public static bool ContainerPlugins = true;

        #endregion

        #region Habilitação de itens de menu

        public static bool TimesTrades = true;
        public static bool Rastreador = true;

        #endregion

        public static bool DelayedVersion = false;
        public static bool SingleSignOn = false;
        public static string LoginIntegradoDistribuidor = "";
        public static string SymbolSolicitadonoDistribuidor = "";
        public static string Distribuidor = "";
        public static bool Backtest = false;

        public enum TipoAcao { Seta, Zoom, Ferramenta, Indicador, CROSS }
        public enum TipoFerramenta { Nenhum, RetaTendencia, RetaSuporte, RetaResistencia, RetaHorizontal, RetaVertical, Retangulo, Elipse, Texto, ValorY, Compra, Vende, Signal, RaffRegression, FiboRetracement, ErrorChannel, FiboFan, FiboArcs, SpeedLine, FiboTimezone, GannFan, TironeLevels, QuadrantLines, Image }
    }
}
