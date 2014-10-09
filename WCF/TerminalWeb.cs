using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Net;

using Traderdata.Server.General.DTO;
using Traderdata.Server.App.TerminalWeb.DTO;
using Traderdata.Server.App.TerminalWeb.BusinessManager;
using System.Configuration;
using Traderdata.Server.Core.BusinessManager;
using Traderdata.Server.Core.DTO;
using Amazon.S3.Model;


namespace Traderdata.Server.App.TerminalWeb
{
    public class TerminalWeb : ITerminalWeb
    {

        private string nomeServico = ConfigurationSettings.AppSettings["CORRETORA"];


        #region Portfolio

        /// <summary>
        /// Metodo que retorna a lista de portfolios
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<PortfolioDTO> RetornaPortfolios(UsuarioDTO user)
        {
            try
            {
                using (PortfolioBM portfolioBM = new PortfolioBM(true,false, nomeServico))
                {
                    return portfolioBM.RetornaTodosPorClienteMaisPadroes(user);
                }

            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "RetornaPortfolios", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return new List<PortfolioDTO>();
            }
        }

        #endregion

        #region Usuario

        /// <summary>
        /// Metodo que registra o ping do usuario
        /// </summary>
        /// <param name="user"></param>
        public void Ping(string idCorretora)
        {
            try
            {
                //using (SegurancaBM usuarioLogadoBM = new SegurancaBM(false, true, nomeServico))
                //{
                //    UsuarioLogadoDTO temp = new UsuarioLogadoDTO();
                //    temp.Data = DateTime.Now;
                //    temp.Id = 0;
                //    temp.Usuario = idCorretora;
                //    usuarioLogadoBM.Ping(temp);
                //}

            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "Ping", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Metodo que faz a atualização do token de 1 usuario
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public UsuarioDTO EditarUsuario(UsuarioDTO user)
        {
            try
            {
                using (SegurancaBM usuarioBM = new SegurancaBM(true, true, nomeServico))
                {
                    return usuarioBM.EditarUsuario(user);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "Login", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que faz a confirmação pelo subrpoduto TERMINAL-WEB entre os produtos permissionados
        /// </summary>
        /// <param name="login"></param>
        /// <param name="senha"></param>
        /// <returns></returns>
        public UsuarioDTO Login(string login, string senha)
        {
            try
            {
                using (SegurancaBM usuarioBM = new SegurancaBM(true, true, nomeServico))
                {
                    UsuarioDTO usuario = usuarioBM.Login(login, senha);

                    usuario = CarregaHasSeguranca(usuario);

                    return usuario;
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "Login", login + " || "+ exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que verifica as seguranças e adiciona os comandos HAS something
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private UsuarioDTO CarregaHasSeguranca(UsuarioDTO usuario)
        {
            if (usuario != null)
            {
                if (usuario.BovespaRT >= DateTime.Today)
                {
                    usuario.HasBovespaRT = true;
                    usuario.HasSnapshotBovespaDiario = true;
                    usuario.HasSnapshotBovespaIntraday = true;
                }
                else
                {
                    usuario.HasBovespaRT = false;
                }

                if (usuario.BMFRT >= DateTime.Today)
                {
                    usuario.HasBMFRT = true;
                    usuario.HasSnapshotBMFDiario = true;
                    usuario.HasSnapshotBMFIntraday = true;
                }
                else
                {
                    usuario.HasBMFRT = false;
                }

                if (usuario.BovespaDELAY >= DateTime.Today)
                {
                    usuario.HasBovespaDELAY = true;
                    usuario.HasSnapshotBovespaDiario = true;
                    usuario.HasSnapshotBovespaIntraday = true;
                }
                else
                {
                    usuario.HasBovespaDELAY = false;
                }

                if (usuario.BMFDELAY >= DateTime.Today)
                {
                    usuario.HasBMFDELAY = true;
                    usuario.HasSnapshotBMFDiario = true;
                    usuario.HasSnapshotBMFIntraday = true;
                }
                else
                {
                    usuario.HasBMFDELAY = false;
                }

                if (usuario.BovespaEOD >= DateTime.Today)
                    usuario.HasSnapshotBovespaDiario = true;



                if (usuario.BMFEOD >= DateTime.Today)
                    usuario.HasSnapshotBMFDiario = true;


            }

            return usuario;
        }

        /// <summary>
        /// Metodo que faz a inserção de um novo usuairo
        /// </summary>
        /// <param name="usuario"></param>
        public UsuarioDTO InserirUsuario(UsuarioDTO usuario)
        {
            try
            {
                using (SegurancaBM usuarioBM = new SegurancaBM(true, true, nomeServico))
                {
                    //setando as datas de liberação do serviço
                    usuario.BovespaRT = DateTime.Today.AddDays(6);
                    usuario.BMFRT = DateTime.Today.AddDays(6);
                    usuario.BovespaDELAY = DateTime.Today.AddDays(15);
                    usuario.BMFDELAY = DateTime.Today.AddDays(15);
                    usuario.BovespaEOD = DateTime.Today.AddDays(30);
                    usuario.BMFEOD = DateTime.Today.AddDays(30);

                    UsuarioDTO usuarioTemp = usuarioBM.InserirUsuario(usuario);
                    if (usuarioTemp == null)
                        return null;

                    return CarregaHasSeguranca(usuarioTemp);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "InserirUsuario", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que verifica se usuario com o login existe, caso exista, retorna ele
        /// caso contrario deve cadastrar o usuairo e retorna-lo
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public UsuarioDTO LoginUserFacebook(string login, string token)
        {
            try
            {
                using (SegurancaBM usuarioBM = new SegurancaBM(true, true, nomeServico))
                {
                    return usuarioBM.LoginUserFacebook(login, token);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "LoginUserFacebook", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que verifica se usuario com o login existe no distribuidor em questão, caso exista, retorna ele
        /// caso contrario deve cadastrar o usuairo e retorna-lo
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public UsuarioDTO LoginUserDistribuidorIntegrado(string login, int distribuidor)
        {
            try
            {
                using (SegurancaBM usuarioBM = new SegurancaBM(true, true, nomeServico))
                {
                    return usuarioBM.LoginUserDistribuidorIntegrado(login, distribuidor);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "LoginUserDistribuidorIntegrado", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }
        #endregion

        #region MarketData

        /// <summary>
        /// Metodo que retorna os segmentos
        /// </summary>
        /// <returns></returns>
        public List<string> GetSegmentos()
        {
            try
            {
                using (AtivoBM ativoBM = new AtivoBM(true, false))
                {
                    return ativoBM.RetornaSegmentos();
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetSegmentos", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que retorna a lista de ativos por segmento
        /// </summary>
        /// <param name="segmento"></param>
        /// <returns></returns>
        public List<string> GetAtivosPorSegmento(string segmento)
        {
            try
            {
                using (AtivoBM ativoBM = new AtivoBM(true, false))
                {
                    List<string> listaAtivos = new List<string>();
                    foreach (AtivoDTO obj in ativoBM.RetornaAtivosPorSegmento(segmento))
                    {
                        listaAtivos.Add(obj.Codigo + ";" + obj.Nome);
                    }
                    return listaAtivos;
                }
                
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetAtivosPorSegmento", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que retorna os indices
        /// </summary>
        /// <returns></returns>
        public List<string> GetIndices()
        {
            try
            {
                using (IndiceBM indiceBM = new IndiceBM(true, false))
                {
                    return indiceBM.RetornaNomeIndices();
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetIndices", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que retorna todos os ativos que compoem determinado indice
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public List<string> GetAtivosPorIndice(string indice)
        {
            try
            {
                using (IndiceBM indiceBM = new IndiceBM(true, false))
                {
                    return indiceBM.RetornaAtivosPorIndice(indice);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetIndices", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que retorna todos os ativos BMF
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public List<string> GetAtivosBMFTodos()
        {
            try
            {
                using (AtivoBM ativoBM = new AtivoBM(true, false))
                {
                    List<string> listaAtivos = new List<string>();
                    foreach (AtivoDTO obj in ativoBM.RetornaAtivosReduzido(EnumGeral.BolsaEnum.BMF))
                    {
                        listaAtivos.Add(obj.Codigo + ";" + obj.Nome);
                    }
                    return listaAtivos;
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetAtivosBMFTodos", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que retorna todos os ativos BMF - MINI
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public List<string> GetAtivosBMFMini()
        {
            try
            {
                using (AtivoBM ativoBM = new AtivoBM(true, false))
                {
                    List<string> listaAtivos = new List<string>();
                    foreach (AtivoDTO obj in ativoBM.RetornaMiniContratos())
                    {
                        listaAtivos.Add(obj.Codigo + ";" + obj.Nome);
                    }
                    return listaAtivos;
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetAtivosBMFMini", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que retorna todos os ativos BMF - CHEIO
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public List<string> GetAtivosBMFCheio()
        {
            try
            {
                using (AtivoBM ativoBM = new AtivoBM(true, false))
                {
                    List<string> listaAtivos = new List<string>();
                    foreach (AtivoDTO obj in ativoBM.RetornaContratosPrincipais())
                    {
                        listaAtivos.Add(obj.Codigo + ";" + obj.Nome);
                    }
                    return listaAtivos;
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetAtivosBMFCheio", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }        

        /// <summary>
        /// Metodo que retorna todos os ativos que compoem determinado indice
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public List<string> GetAtivosBovespaTodos()
        {
            try
            {
                using (AtivoBM ativoBM = new AtivoBM(true, false))
                {
                    List<string> listaAtivos = new List<string>();
                    foreach (AtivoDTO obj in ativoBM.RetornaAtivosReduzido(EnumGeral.BolsaEnum.Bovespa))
                    {
                        listaAtivos.Add(obj.Codigo + ";" + obj.Nome);
                    }
                    return listaAtivos;
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetAtivosBovespa", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }                

        /// <summary>
        /// Metodo que retorna todos os ativos que compoem determinado indice
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public List<string> GetAtivosBovespaVista()
        {
            try
            {
                using (AtivoBM ativoBM = new AtivoBM(true, false))
                {
                    List<string> listaAtivos = new List<string>();
                    foreach (AtivoDTO obj in ativoBM.RetornaAtivosReduzido(EnumGeral.BolsaEnum.Bovespa))
                    {
                        if (obj.Tipo == "V")
                            listaAtivos.Add(obj.Codigo + ";" + obj.Nome);
                    }
                    return listaAtivos;
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetAtivosBovespaVista", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que retorna todos os ativos que compoem determinado indice
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public List<string> GetAtivosBovespaOpcao()
        {
            try
            {
                using (AtivoBM ativoBM = new AtivoBM(true, false))
                {
                    List<string> listaAtivos = new List<string>();
                    foreach (AtivoDTO obj in ativoBM.RetornaAtivosReduzido(EnumGeral.BolsaEnum.Bovespa))
                    {
                        if (obj.Tipo == "O")
                            listaAtivos.Add(obj.Codigo + ";" + obj.Nome);
                    }
                    return listaAtivos;
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetAtivosBovespaOpcao", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que retorna todos os ativos que compoem determinado indice
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public List<string> GetAtivosBovespaTermo()
        {
            try
            {
                using (AtivoBM ativoBM = new AtivoBM(true, false))
                {
                    List<string> listaAtivos = new List<string>();
                    foreach (AtivoDTO obj in ativoBM.RetornaAtivosReduzido(EnumGeral.BolsaEnum.Bovespa))
                    {
                        if (obj.Tipo == "T")
                            listaAtivos.Add(obj.Codigo + ";" + obj.Nome);
                    }
                    return listaAtivos;
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetAtivosBovespaTermo", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que retorna todas as cotações diarias de determinado ativo
        /// </summary>
        /// <param name="ativo"></param>
        /// <returns></returns>
        public List<string> GetCotacaoDiario(string ativo) 
        {
            try
            {
                //LogServicoBM.LogaEvento(nomeServico, "GetCotacaoDiario", DateTime.Now.ToString("HH:mm:ss.fff"), EventLogEntryType.Error);
                using (CotacaoBM cotacaoBM = new CotacaoBM(true, false))
                {
                    List<string> lista = new List<string>();
                    foreach (CotacaoServerDTO obj in cotacaoBM.GetCotacao(ativo, 1440, new DateTime(DateTime.Today.Year - 10, DateTime.Today.Month, DateTime.Today.Day), DateTime.Today,
                        true, false, false, EnumGeral.BolsaEnum.Bovespa))
                    {
                        lista.Add(obj.Abertura.ToString("0.00", Util.NumberProvider) + ";" + obj.Maximo.ToString("0.00", Util.NumberProvider) + ";"
                            + obj.Minimo.ToString("0.00", Util.NumberProvider) + ";" + obj.Ultimo.ToString("0.00", Util.NumberProvider) + ";" + obj.Quantidade.ToString("0")
                            + ";" + obj.Volume.ToString("0") + ";" + obj.Data.ToString("dd/MM/yyyy"));
                    }
                    
                    return lista;
                }

                
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetCotacaoDiario", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que retorna as cotações intraday
        /// </summary>
        /// <param name="ativo"></param>
        /// <returns></returns>
        public List<string> GetCotacaoIntraday(string ativo, int numeroDiasCorridos) 
        {
            try
            {
                using (CotacaoBM cotacaoBM = new CotacaoBM(true, false))
                {
                    List<string> lista = new List<string>();
                    foreach (CotacaoServerDTO obj in cotacaoBM.GetCotacao(ativo, 1, DateTime.Today.Subtract(new TimeSpan(numeroDiasCorridos,0,0,0)), 
                        DateTime.Now, true, false, false, EnumGeral.BolsaEnum.Bovespa))
                    {
                        lista.Add(obj.Abertura.ToString("0.00", Util.NumberProvider) + ";" + obj.Maximo.ToString("0.00", Util.NumberProvider) + ";"
                            + obj.Minimo.ToString("0.00", Util.NumberProvider) + ";" + obj.Ultimo.ToString("0.00", Util.NumberProvider) + ";" + obj.Quantidade.ToString("0")
                            + ";" + obj.Volume.ToString("0") + ";" + obj.Data.ToString("dd/MM/yyyy") + ";" + obj.Hora + ";" + obj.AfterMarket);
                    }
                    return lista;
                }


            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetCotacaoIntraday", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que retorna as cotações intraday
        /// </summary>
        /// <param name="ativo"></param>
        /// <returns></returns>
        public List<string> GetCotacaoIntradayDelayed(string ativo, int numeroDiasCorridos)
        {
            try
            {
                using (CotacaoBM cotacaoBM = new CotacaoBM(true, false))
                {
                    List<string> lista = new List<string>();
                    foreach (CotacaoServerDTO obj in cotacaoBM.GetCotacao(ativo, 1, DateTime.Today.Subtract(new TimeSpan(numeroDiasCorridos, 0, 0, 0)),
                        DateTime.Now, true, true, true, EnumGeral.BolsaEnum.Bovespa))
                    {
                        lista.Add(obj.Abertura.ToString("0.00", Util.NumberProvider) + ";" + obj.Maximo.ToString("0.00", Util.NumberProvider) + ";"
                            + obj.Minimo.ToString("0.00", Util.NumberProvider) + ";" + obj.Ultimo.ToString("0.00", Util.NumberProvider) + ";" + obj.Quantidade.ToString("0")
                            + ";" + obj.Volume.ToString("0") + ";" + obj.Data.ToString("dd/MM/yyyy") + ";" + obj.Hora + ";" + obj.AfterMarket);
                    }
                    return lista;
                }


            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetCotacaoIntraday", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Metodo que retorna as cotações intraday do dia corrente
        /// </summary>
        /// <param name="ativo"></param>
        /// <returns></returns>
        public List<string> GetCotacaoIntradayDoDia(string ativo)
        {
            try
            {
                using (CotacaoBM cotacaoBM = new CotacaoBM(true, false))
                {
                    List<string> lista = new List<string>();
                    foreach (CotacaoServerDTO obj in cotacaoBM.GetCotacao(ativo, 1, DateTime.Today,
                        DateTime.Today.AddDays(1), true, false, false, EnumGeral.BolsaEnum.Bovespa))
                    {
                        lista.Add(obj.Abertura.ToString("0.00", Util.NumberProvider) + ";" + obj.Maximo.ToString("0.00", Util.NumberProvider) + ";"
                            + obj.Minimo.ToString("0.00", Util.NumberProvider) + ";" + obj.Ultimo.ToString("0.00", Util.NumberProvider) + ";" + obj.Quantidade.ToString("0")
                            + ";" + obj.Volume.ToString("0") + ";" + obj.Data.ToString("dd/MM/yyyy") + ";" + obj.Hora + ";" + obj.AfterMarket);
                    }
                    return lista;
                }


            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetCotacaoIntradayDoDia", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        #endregion

        #region Templates

        /// <summary>
        /// Salva template DTO
        /// </summary>
        /// <param name="graficoTemplateDTO"></param>
        public void SalvaTemplate(TemplateDTO templateDTO)
        {
            try
            {
                using (TemplateBM templateBM = new TemplateBM(true, true, nomeServico))
                {
                    templateBM.SaveOrUpdate(templateDTO);
                }

            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "SalvaTemplate", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /// <summary>
        /// Exclui template
        /// </summary>
        public void ExcluiTemplate(TemplateDTO templateDTO)
        {
            try
            {
                using (TemplateBM templateBM = new TemplateBM(false, true, nomeServico))
                {
                    templateBM.ExcluirTemplate(templateDTO);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "ExcluiTemplate", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /// <summary>
        /// Retorna template por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TemplateDTO GetTemplatePorId(int id)
        {
            try
            {
                using (TemplateBM templateBM = new TemplateBM(true, false, nomeServico))
                {
                    return templateBM.ReturnById(id);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "RetornaTemplatePorId", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /// <summary>
        /// Retorna templates por login
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<TemplateDTO> GetTemplatesPorUserId(int userId)
        {
            try
            {
                using (TemplateBM templateBM = new TemplateBM(true, false, nomeServico))
                {
                    return templateBM.RetornaTemplatesPorUserId(userId);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "RetornaTemplatesPorClientId", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        #endregion

        #region Workspace

        /// <summary>
        /// Metodo que retorna o workspace default de acordo com o distribuidor passado
        /// </summary>
        /// <param name="distribuidorId"></param>
        /// <returns></returns>
        public WorkspaceDTO GetWorkspaceDefaultPorDistribuidor(int distribuidorId)
        {
            try
            {
                using (WorkspaceBM workspaceBM = new WorkspaceBM(true, false, nomeServico))
                {
                    return workspaceBM.GetWorkspaceDefaultPorDistribuidor(distribuidorId);
                }

            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetWorkspaceDefault", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna o workspace default(0) do cliente
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public WorkspaceDTO GetWorkspaceDefault(UsuarioDTO User)
        {
            try
            {
                using (WorkspaceBM workspaceBM = new WorkspaceBM(true,false, nomeServico))
                {
                    return workspaceBM.GetWorkspaceDefault(User);
                }

            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetWorkspaceDefault", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que salva o workspace passado como sendo o workspace default
        /// </summary>
        /// <param name="workspace"></param>
        public void SaveWorkspace(WorkspaceDTO workspace)
        {
            try
            {
                using (WorkspaceBM workspaceBM = new WorkspaceBM(true, true, nomeServico))
                {
                    workspaceBM.SaveWorkspaceDefault(workspace);
                }

            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "SaveWorkspaceDefault", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        #endregion

        #region Graficos

        /// <summary>
        /// Metodo que vai salvar o grafico no perfil do usuario
        /// </summary>
        /// <param name="grafico"></param>
        public void SaveGrafico(GraficoDTO grafico)
        {
            try
            {
                using (GraficoBM graficoBM = new GraficoBM(true, true, nomeServico))
                {
                    graficoBM.SaveGrafico(grafico);
                }

            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "SaveGrafico", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna o ativo e a periodicidade
        /// </summary>
        /// <param name="ativo"></param>
        /// <param name="periodicidade"></param>
        /// <returns></returns>
        public List<GraficoDTO> RetornaGraficoPorAtivoPeriodicidade(string ativo, int periodicidade, int userId)
        {
            try
            {
                using (GraficoBM graficoBM = new GraficoBM(true, true, nomeServico))
                {
                    return graficoBM.RetornaGraficoPorAtivoPeriodicidade(ativo, periodicidade, userId);
                }

            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "SaveGrafico", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        #endregion

        #region Util

        /// <summary>
        /// Metodo que vai salvar na pasta desejada o arquivo passado
        /// </summary>
        /// <param name="diretorio"></param>
        /// <param name="fileName"></param>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public void UploadFileS3(string bucket, string fileName, byte[] fileData)
        {
            try
            {
                Amazon.S3.AmazonS3Config config = new Amazon.S3.AmazonS3Config();
                config.ServiceURL = "s3-sa-east-1.amazonaws.com";

                using (Amazon.S3.AmazonS3Client client = new Amazon.S3.AmazonS3Client(config))
                {                    
                    PutObjectRequest request = new PutObjectRequest();
                    request.BucketName = bucket;
                    //request.ContentType = contentType;
                    request.Key = fileName;
                    //request.CannedACL = S3CannedACL.PublicRead;
                    MemoryStream stream = new MemoryStream(fileData);
                    request.InputStream = stream;
                    client.PutObject(request);
                }
                
            }
            catch (Exception exc)
            {                
                LogServicoBM.LogaEvento(nomeServico, "UploadFileS3", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                
            }
        }

        #endregion

        #region Scanner

        /// <summary>
        /// Metodo que reprocessa o scanner
        /// </summary>
        /// <param name="scanner"></param>
        public void ReProcessaScanner(ScannerDTO scanner)
        {
            using (ScannerBM scannerBM = new ScannerBM(true, true, nomeServico))
            {
                scannerBM.ProcessaScanner(ref scanner);
            }
        }

        /* Método: GetScannerById
         * Date: 20/06/2012
         * Description: Retorna um objeto scanner com a lista de condições e lista de parcelas populadas
         */
        public ScannerDTO GetScannerById(int idScanner)
        {
            try
            {
                //Instanciando a classe de scanner
                using (ScannerBM scannerBM = new ScannerBM(true, false, nomeServico))
                {
                    //Executando a query
                    return scannerBM.GetScannerDiarioById(idScanner);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetScannerById", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /* Método: RetornaScannersPorCliente
         * Date: 16/06/2012
         * Description: Retorna uma lista com todos os meus scanners validos 
         */
        public List<ScannerDTO> GetScanners(int userId)
        {
            try
            {
                //Instanciando a classe de scanner
                using (ScannerBM scannerBM = new ScannerBM(true, false, nomeServico))
                {
                    //Executando a query
                    return scannerBM.RetornaScannersDiarioPorCliente(userId);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetScanners", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /* Método: GetCondicoes
         * Date: 16/10/2009
         * Description: Retorna a listagem de condicoes
         */
        public List<CondicaoDTO> GetCondicoes()
        {
            try
            {
                //Instanciando a classe de scanner
                using (ScannerBM scannerBM = new ScannerBM(true, false, nomeServico))
                {
                    //Retorno
                    return scannerBM.RetornaTodasCondicoes();
                }

            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetCondicoes", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /* Método: SaveScanner
         * Date: 16/10/2009
         * Description: Armazena um scanner
         */
        public ScannerDTO SaveScanner(ScannerDTO scannerDTO)
        {
            try
            {
                using (ScannerBM scannerBM = new ScannerBM(true, true, nomeServico))
                {
                    return scannerBM.SaveScanner(scannerDTO);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "SaveScanner", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /* Método: DeleteScanner
         * Date: 16/10/2009
         * Description: Deleta um scanner
         */
        public void DeleteScanner(ScannerDTO scannerDTO)
        {
            try
            {
                //Instanciando a classe de scanner
                using (ScannerBM scannerBM = new ScannerBM(false, true, nomeServico))
                {
                    scannerBM.DeleteScanner(scannerDTO);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "SaveScanner", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /* Método: GetParcelaByCondicao
         * Date: 16/10/2009
         * Description: Retorna a listagem de parcelas de determinada cotacao
         */
        public List<CondicaoParcelaDTO> GetParcelaByCondicao(int condicaoId)
        {
            try
            {
                //Instanciando a classe de scanner
                using (ScannerBM condicaoParcelaBM = new ScannerBM(true, false, nomeServico))
                {
                    //Retorno
                    return condicaoParcelaBM.GetParcelaByCondicao(condicaoId);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetParcelaByCondicao", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /* Método:RetornaResultadosScanner
         * Descrição: Retorna a lista de ativos que foi disparada como resultado do scanner acionado
         * Data: 01/09/2008
         */
        public List<ResultadoScannerDTO> GetResultadoScanner(int codigoScanner)
        {
            try
            {
                using (ScannerBM resultadoBM = new ScannerBM(true, false, nomeServico))
                {
                    //Executando a query
                    return resultadoBM.RetornaResultadosScannerDiario(codigoScanner);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "GetResultadoScanner", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        #endregion

        #region Backtest

        /// <summary>
        /// Inclui um novo backtest.
        /// </summary>
        /// <param name="BacktestDTO">DTO do backtest a ser incluido.</param>
        /// <returns></returns>
        public BacktestDTO IncluirBackTest(BacktestDTO BacktestDTO)
        {
            BacktestDTO backTestAux = new BacktestDTO();

            try
            {
                using (BackTestBM backTestBM = new BackTestBM(true, true, nomeServico))
                {
                    backTestAux = backTestBM.IncluirBacktest(BacktestDTO);
                }

                return backTestAux;
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "IncluirBackTest", exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Atualiza backtest.
        /// </summary>
        /// <param name="BacktestDTO">DTO do backtest a ser atualizado.</param>
        /// <returns></returns>
        public void AtualizaBackTest(BacktestDTO BacktestDTO)
        {
            try
            {
                using (BackTestBM backTestBM = new BackTestBM(true, true, nomeServico))
                {
                    backTestBM.AtualizaBacktest(BacktestDTO);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "AtualizaBackTest", exc.ToString(), EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Exclui backtest desejado.
        /// </summary>
        /// <param name="backTest">Backtest a ser deletado.</param>
        public void ExcluiBackTest(BacktestDTO backTest)
        {
            try
            {
                using (BackTestBM backTestBM = new BackTestBM(true, true, nomeServico))
                {
                    backTestBM.ExcluirBacktest(backTest);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "ExcluiBackTest", exc.ToString(), EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Retorna todos os backtests de um cliente.
        /// </summary>
        /// <param name="cliente">Cliente proprietário dos backtests.</param>
        /// <param name="macroCliente">Macro cliente proprietário do cliente.</param>
        /// <returns></returns>
        public List<BacktestDTO> RetornaBackTests(UsuarioDTO user)
        {
            try
            {
                using (BackTestBM backTestBM = new BackTestBM(true, false, nomeServico))
                {
                    return backTestBM.RetornaTodosBacktestPorCliente(user);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "RetornaBackTests", exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Inclui um novo template.
        /// </summary>
        /// <param name="BacktestDTO">DTO do template a ser incluido.</param>
        /// <param name="cliente">Cliente proprietário do template.</param>
        /// <param name="macroCliente">Macro cliente proprietário do cliente,</param>
        /// <returns></returns>
        public void IncluirTemplateBacktest(TemplateBacktestDTO templateDTO)
        {
            try
            {
                using (BackTestBM templateBM = new BackTestBM(true, true, nomeServico))
                {
                    templateBM.IncluirBacktest(templateDTO);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "IncluirTemplate", exc.ToString(), EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Atualiza template.
        /// </summary>
        /// <param name="templateDTO">DTO do template a ser atualizado.</param>
        /// <returns></returns>
        public void AtualizaTemplateBacktest(TemplateBacktestDTO templateDTO)
        {
            try
            {
                using (BackTestBM templateBM = new BackTestBM(true, true, nomeServico))
                {
                    templateBM.AtualizaTemplateBacktest(templateDTO);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "AtualizaTemplate", exc.ToString(), EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Exclui tenmplate desejado.
        /// </summary>
        /// <param name="templateDTO">Tempalte a ser exlcuido.</param>
        public void ExcluiTemplateBacktest(TemplateBacktestDTO templateDTO)
        {
            try
            {
                using (BackTestBM templateBM = new BackTestBM(true, true, nomeServico))
                {
                    templateBM.ExcluirBacktest(templateDTO);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "ExcluiTemplateBacktest", exc.ToString(), EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Retorna todos os templates de um cliente.
        /// </summary>
        /// <param name="cliente">Cliente proprietário dos templates.</param>
        /// <param name="macroCliente">Macro cliente proprietário do cliente,</param>
        /// <returns></returns>
        public List<TemplateBacktestDTO> RetornaTemplatesBacktest(UsuarioDTO user)
        {
            try
            {
                using (BackTestBM templateBM = new BackTestBM(true, false, nomeServico))
                {
                    return templateBM.RetornaTemplateBacktestPorCliente(user);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "RetornaTemplatesBacktest", exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        /// <summary>
        /// Retorna o sumário gerado por um backtest.
        /// </summary>
        /// <param name="idBackTest">ID do backtest desejado.</param>
        /// <returns></returns>
        public SumarioDTO RetornaSumarioResultadoBacktest(int idBackTest)
        {
            try
            {
                using (BackTestBM backtestBM = new BackTestBM(true, false, nomeServico))
                {
                    return backtestBM.RetornaSumarioPorBacktesting(idBackTest);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "RetornaSumarioResultadoBacktest", exc.ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        #endregion

        #region Zona de Compartilhamento

        /// <summary>
        /// Metodo que retorna a ultima analise efetuada para este ativo
        /// </summary>
        /// <param name="ativo"></param>
        public AnaliseCompartilhadaDTO RetornaUltimaAnalise(string ativo)
        {
            try
            {
                using (AnaliseCompartilhadaBM analiseBM = new AnaliseCompartilhadaBM(true, false, nomeServico))
                {
                    //return analiseBM.RetornaUltimaAnalise(ativo);
                    return null;
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "RetornaUltimaAnalise", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna todas as analises
        /// </summary>
        /// <returns></returns>
        public List<AnaliseCompartilhadaDTO> RetornaTodasAnalises()
        {
            List<AnaliseCompartilhadaDTO> listaAnalises = new List<AnaliseCompartilhadaDTO>();
            List<string> ativosInseridos = new List<string>();
            try
            {
                using (AnaliseCompartilhadaBM analiseBM = new AnaliseCompartilhadaBM(true, false, nomeServico))
                {

                    //foreach (AnaliseCompartilhadaDTO obj in analiseBM.ReturnAll())
                    //{
                    //    if (!ativosInseridos.Contains(obj.Ativo))
                    //        listaAnalises.Add(obj);
                    //    ativosInseridos.Add(obj.Ativo);
                    //}

                }

                return listaAnalises;
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "RetornaTodasAnalises", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que armazena o array de bytes que compoem a imagem
        /// </summary>
        /// <param name="ImagemPrincipal"></param>
        /// <param name="ImagemLateral"></param>
        /// <param name="analiseDTO"></param>
        public void SalvarAnaliseCompartilhada(byte[] ImagemPrincipal, AnaliseCompartilhadaDTO analiseDTO, string macroCliente, GraficoDTO grafico)
        {
            UsuarioDTO usuarioDTO = new UsuarioDTO();
            try
            {
                //Salvando a analise compartilhada e a imagem
                using (AnaliseCompartilhadaBM analiseBM = new AnaliseCompartilhadaBM(true, true, nomeServico))
                {
                    //analiseBM.SalvarAnaliseCompartilhada(ImagemPrincipal, analiseDTO, macroCliente,
                    //    ConfigurationSettings.AppSettings["DIRETORIO-GRAFICO"], grafico);
                }

            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(nomeServico, "SalvarAnaliseCompartilhada", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        #endregion

        #region Loja Virtual

        #region Preços
        /// <summary>
        /// Metodo que retorna o preço de BVSP em RT
        /// </summary>
        /// <returns></returns>
        public double RetornaPrecoBVSPRT()
        {
            return Convert.ToDouble(ConfigurationSettings.AppSettings["PRECO-BVSP-RT"]);
        }
        /// <summary>
        /// Metodo que retorna o preço de BMF em RT
        /// </summary>
        /// <returns></returns>
        public double RetornaPrecoBMFRT()
        {
            return Convert.ToDouble(ConfigurationSettings.AppSettings["PRECO-BMF-RT"]);
        }
        /// <summary>
        /// Metodo que retorna o preço de BVSP em DELAY
        /// </summary>
        /// <returns></returns>
        public double RetornaPrecoBVSPDELAY()
        {
            return Convert.ToDouble(ConfigurationSettings.AppSettings["PRECO-BVSP-DELAY"]);
        }
        /// <summary>
        /// Metodo que retorna o preço de BVSP em RT
        /// </summary>
        /// <returns></returns>
        public double RetornaPrecoBMFDELAY()
        {
            return Convert.ToDouble(ConfigurationSettings.AppSettings["PRECO-BMF-DELAY"]);
        }

        /// <summary>
        /// Metodo que retorna o preço de BVSP em EOD
        /// </summary>
        /// <returns></returns>
        public double RetornaPrecoBVSPEOD()
        {
            return Convert.ToDouble(ConfigurationSettings.AppSettings["PRECO-BVSP-EOD"]);
        }

        /// <summary>
        /// Metodo que retorna o preço de BMF em EOD
        /// </summary>
        /// <returns></returns>
        public double RetornaPrecoBMFEOD()
        {
            return Convert.ToDouble(ConfigurationSettings.AppSettings["PRECO-BMF-EOD"]);
        }
        #endregion

        #region Links de Preços

        /// <summary>
        /// Metodo que retorna o link para pagamento de BVSP RT
        /// </summary>
        /// <returns></returns>
        public string RetornaLinkPagamentoBVSPRT()
        {
            return ConfigurationSettings.AppSettings["LINK-BVSP-RT"];
        }

        /// <summary>
        /// Metodo que retorna o link para pagamento de BVSP DELAY
        /// </summary>
        /// <returns></returns>
        public string RetornaLinkPagamentoBVSPDELAY()
        {
            return ConfigurationSettings.AppSettings["LINK-BVSP-DELAY"];
        }

        /// <summary>
        /// Metodo que retorna o link para pagamento de BVSP EOD
        /// </summary>
        /// <returns></returns>
        public string RetornaLinkPagamentoBVSPEOD()
        {
            return ConfigurationSettings.AppSettings["LINK-BVSP-EOD"];
        }

        /// <summary>
        /// Metodo que retorna o link para pagamento de BMF RT
        /// </summary>
        /// <returns></returns>
        public string RetornaLinkPagamentoBMFRT()
        {
            return ConfigurationSettings.AppSettings["LINK-BMF-RT"];
        }

        /// <summary>
        /// Metodo que retorna o link para pagamento de BMF DELAY
        /// </summary>
        /// <returns></returns>
        public string RetornaLinkPagamentoBMFDELAY()
        {
            return ConfigurationSettings.AppSettings["LINK-BMF-DELAY"];
        }


        /// <summary>
        /// Metodo que retorna o link para pagamento de BMF EOD
        /// </summary>
        /// <returns></returns>
        public string RetornaLinkPagamentoBMFEOD()
        {
            return ConfigurationSettings.AppSettings["LINK-BMF-EOD"];
        }

        /// <summary>
        /// Metodo que retorna o link para pagamento de BVSP RT + BMF RT
        /// </summary>
        /// <returns></returns>
        public string RetornaLinkPagamentoBVSPRTBMFRT()
        {
            return ConfigurationSettings.AppSettings["LINK-BVSP-RT-BMF-RT"];
        }


        /// <summary>
        /// Metodo que retorna o link para pagamento de BVSP RT BMF DELAY
        /// </summary>
        /// <returns></returns>
        public string RetornaLinkPagamentoBVSPRTBMFDELAY()
        {
            return ConfigurationSettings.AppSettings["LINK-BVSP-RT-BMF-DELAY"];
        }

        /// <summary>
        /// Metodo que retorna o link para pagamento de BVSP RT + BMF EOD
        /// </summary>
        /// <returns></returns>
        public string RetornaLinkPagamentoBVSPRTBMFEOD()
        {
            return ConfigurationSettings.AppSettings["LINK-BVSP-RT-BMF-EOD"];
        }

        /// <summary>
        /// Metodo que retorna o link para pagamento de BVSP DELAY + BMF RT
        /// </summary>
        /// <returns></returns>
        public string RetornaLinkPagamentoBVSPDELAYBMFRT()
        {
            return ConfigurationSettings.AppSettings["LINK-BVSP-DELAY-BMF-RT"];
        }

        /// <summary>
        /// Metodo que retorna o link para pagamento de BVSP EOD + BMF RT
        /// </summary>
        /// <returns></returns>
        public string RetornaLinkPagamentoBVSPEODBMFRT()
        {
            return ConfigurationSettings.AppSettings["LINK-BVSP-EOD-BMF-RT"];
        }

        /// <summary>
        /// Metodo que retorna o link para pagamento de BVSP DELAY + BMF DELAY
        /// </summary>
        /// <returns></returns>
        public string RetornaLinkPagamentoBVSPDELAYBMFDELAY()
        {
            return ConfigurationSettings.AppSettings["LINK-BVSP-DELAY-BMF-DELAY"];
        }

        /// <summary>
        /// Metodo que retorna o link para pagamento de BVSP DELAY + BMF EOD
        /// </summary>
        /// <returns></returns>
        public string RetornaLinkPagamentoBVSPDELAYBMFEOD()
        {
            return ConfigurationSettings.AppSettings["LINK-BVSP-DELAY-BMF-EOD"];
        }

        /// <summary>
        /// Metodo que retorna o link para pagamento de BVSP EOD + BMF DELAY
        /// </summary>
        /// <returns></returns>
        public string RetornaLinkPagamentoBVSPEODBMFDELAY()
        {
            return ConfigurationSettings.AppSettings["LINK-BVSP-EOD-BMF-DELAY"];
        }

        /// <summary>
        /// Metodo que retorna o link para pagamento de BVSP EOD + BMF EOD
        /// </summary>
        /// <returns></returns>
        public string RetornaLinkPagamentoBVSPEODBMFEOD()
        {
            return ConfigurationSettings.AppSettings["LINK-BVSP-EOD-BMF-EOD"];
        }


        #endregion

        #endregion

        #region Social

        /// <summary>
        /// Metodo que vai fazer a inserção de mensagem
        /// </summary>
        /// <param name="mensagem"></param>
        public void InsereMensagem(MensagemDTO mensagem)
        {
            using (SocialBM socialBM = new SocialBM(false, true, nomeServico))
            {
                socialBM.InsereMensagem(mensagem);
            }
        }

        #endregion
    }
}
