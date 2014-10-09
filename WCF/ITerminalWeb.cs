using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;
using System.IO;
using Traderdata.Server.Core.DTO;


namespace Traderdata.Server.App.TerminalWeb
{
    [ServiceContract]
    public interface ITerminalWeb
    {
        #region Social

        [OperationContract]
        void InsereMensagem(MensagemDTO mensagem);

        #endregion

        #region Portfolio
        [OperationContract]
        List<PortfolioDTO> RetornaPortfolios(UsuarioDTO user);

        #endregion

        #region Usuario

        [OperationContract]
        void Ping(string user);

        [OperationContract]
        UsuarioDTO Login(string login, string senha);

        [OperationContract]
        UsuarioDTO LoginUserDistribuidorIntegrado(string login, int distribuidor);

        [OperationContract]
        UsuarioDTO InserirUsuario(UsuarioDTO usuario);

        [OperationContract]
        UsuarioDTO LoginUserFacebook(string login, string token);

        [OperationContract]
        UsuarioDTO EditarUsuario(UsuarioDTO user);

        #endregion

        #region MarketData

        [OperationContract]
        List<string> GetSegmentos();

        [OperationContract]
        List<string> GetAtivosPorSegmento(string segmento);

        [OperationContract]
        List<string> GetIndices();

        [OperationContract]
        List<string> GetAtivosPorIndice(string indice);

        [OperationContract]
        List<string> GetAtivosBMFTodos();

        [OperationContract]
        List<string> GetAtivosBMFMini();

        [OperationContract]
        List<string> GetAtivosBMFCheio();

        [OperationContract]
        List<string> GetAtivosBovespaTodos();

        
        [OperationContract]
        List<string> GetAtivosBovespaVista();

        [OperationContract]
        List<string> GetAtivosBovespaOpcao();

        [OperationContract]
        List<string> GetAtivosBovespaTermo();

        [OperationContract]
        List<string> GetCotacaoDiario(string ativo);

        [OperationContract]
        List<string> GetCotacaoIntraday(string ativo, int numeroDiasCorridos);

        [OperationContract]
        List<string> GetCotacaoIntradayDelayed(string ativo, int numeroDiasCorridos);

        [OperationContract]
        List<string> GetCotacaoIntradayDoDia(string ativo);

        #endregion

        #region Templates

        [OperationContract]
        void SalvaTemplate(TemplateDTO TemplateDTO);

        [OperationContract]
        void ExcluiTemplate(TemplateDTO templateDTO);

        [OperationContract]
        TemplateDTO GetTemplatePorId(int id);

        [OperationContract]
        List<TemplateDTO> GetTemplatesPorUserId(int userId);

        #endregion

        #region Workspace
        [OperationContract]
        WorkspaceDTO GetWorkspaceDefaultPorDistribuidor(int distribuidorId);

        [OperationContract]
        WorkspaceDTO GetWorkspaceDefault(UsuarioDTO User);

        [OperationContract]
        void SaveWorkspace(WorkspaceDTO workspace);

        #endregion

        #region Graficos

        [OperationContract]
        void SaveGrafico(GraficoDTO grafico);

        [OperationContract]
        List<GraficoDTO> RetornaGraficoPorAtivoPeriodicidade(string ativo, int periodicidade, int userId);

        #endregion

        #region Util

        [OperationContract]
        void UploadFileS3(string bucket, string fileName, byte[] fileData);

        #endregion

        #region Scanner

        [OperationContract]
        ScannerDTO GetScannerById(int idScanner);

        [OperationContract]
        List<ScannerDTO> GetScanners(int userId);

        [OperationContract]
        List<CondicaoDTO> GetCondicoes();

        [OperationContract]
        ScannerDTO SaveScanner(ScannerDTO scannerDTO);

        [OperationContract]
        void DeleteScanner(ScannerDTO scannerDTO);

        [OperationContract]
        List<CondicaoParcelaDTO> GetParcelaByCondicao(int condicaoId);

        [OperationContract]
        List<ResultadoScannerDTO> GetResultadoScanner(int scannerId);

        [OperationContract]
        void ReProcessaScanner(ScannerDTO scanner);
        #endregion

        #region Backtest
        
        [OperationContract]
        BacktestDTO IncluirBackTest(BacktestDTO BacktestDTO);

        [OperationContract]
        void AtualizaBackTest(BacktestDTO BacktestDTO);

        [OperationContract]
        void ExcluiBackTest(BacktestDTO backTest);

        [OperationContract]
        List<BacktestDTO> RetornaBackTests(UsuarioDTO user);

        //Template
        [OperationContract]
        void IncluirTemplateBacktest(TemplateBacktestDTO templateDTO);

        [OperationContract]
        void AtualizaTemplateBacktest(TemplateBacktestDTO templateDTO);

        [OperationContract]
        void ExcluiTemplateBacktest(TemplateBacktestDTO templateDTO);

        [OperationContract]
        List<TemplateBacktestDTO> RetornaTemplatesBacktest(UsuarioDTO user);
        
        //Resultado do backtest
        [OperationContract]
        SumarioDTO RetornaSumarioResultadoBacktest(int idBackTest);
        
        #endregion

        #region Zona de Compartilhamento

        //[OperationContract]
        //void SalvarAnaliseCompartilhada(byte[] ImagemPrincipal, AnaliseCompartilhadaDTO analiseDTO, 
        //    GraficoDTO grafico);

        [OperationContract]
        List<AnaliseCompartilhadaDTO> RetornaTodasAnalises();
        

        [OperationContract]
        AnaliseCompartilhadaDTO RetornaUltimaAnalise(string ativo);

        #endregion

        #region Loja Virtual

        #region Preços
        [OperationContract]
        double RetornaPrecoBVSPRT();

        [OperationContract]
        double RetornaPrecoBMFRT();

        [OperationContract]
        double RetornaPrecoBVSPDELAY();

        [OperationContract]
        double RetornaPrecoBMFDELAY();

        [OperationContract]
        double RetornaPrecoBVSPEOD();

        [OperationContract]
        double RetornaPrecoBMFEOD();

        #endregion

        #region Links de Preços

        [OperationContract]
        string RetornaLinkPagamentoBVSPRT();

        [OperationContract]
        string RetornaLinkPagamentoBVSPDELAY();

        [OperationContract]
        string RetornaLinkPagamentoBVSPEOD();

        [OperationContract]
        string RetornaLinkPagamentoBMFRT();

        [OperationContract]
        string RetornaLinkPagamentoBMFDELAY();

        [OperationContract]
        string RetornaLinkPagamentoBMFEOD();

        [OperationContract]
        string RetornaLinkPagamentoBVSPRTBMFRT();

        [OperationContract]
        string RetornaLinkPagamentoBVSPRTBMFDELAY();

        [OperationContract]
        string RetornaLinkPagamentoBVSPRTBMFEOD();

        [OperationContract]
        string RetornaLinkPagamentoBVSPDELAYBMFRT();

        [OperationContract]
        string RetornaLinkPagamentoBVSPEODBMFRT();

        [OperationContract]
        string RetornaLinkPagamentoBVSPDELAYBMFDELAY();

        [OperationContract]
        string RetornaLinkPagamentoBVSPDELAYBMFEOD();

        [OperationContract]
        string RetornaLinkPagamentoBVSPEODBMFDELAY();

        [OperationContract]
        string RetornaLinkPagamentoBVSPEODBMFEOD();


        #endregion

        #endregion
    }
}

