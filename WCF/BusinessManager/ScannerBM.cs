using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;
using Traderdata.Server.App.TerminalWeb.DAO;
using System.Messaging;
using System.Threading;
using Traderdata.Server.Core.DTO;
using System.Diagnostics;
using Traderdata.Server.Core.DAO;
using Traderdata.Server.Core.BusinessManager;
using Traderdata.Server.General.DTO;

namespace Traderdata.Server.App.TerminalWeb.BusinessManager
{
    public class ScannerBM:BaseBM
    {
        #region Constructor
 
        //construtor basico que nao ira implementar o construtor do pai
        public ScannerBM(bool leitura, bool escrita, string nomeServico)
            : base(leitura, escrita, nomeServico)
        {
        }

        #endregion

        #region Metodos de processamento de scanner

        /// <summary>
        /// Metodo que faz o processamento de um scanner após ele já ter sido salvo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="conexaoRead"></param>
        /// <param name="conexaoWrite"></param>
        public void ProcessaScanner(ref ScannerDTO scannerDTO)
        {

            //Declarando variaveis auxiliares
            List<ResultadoScannerDTO> listaResultados = new List<ResultadoScannerDTO>();
            
            try
            {
                string[] ativos = scannerDTO.ListaAtivos.Split(';');
                                
                using (ResultadoScannerDAO resultadoScannerDAO = new ResultadoScannerDAO(readConnection, writeConnection))
                {
                    //Limpando os resultados presentes para este scanner
                    resultadoScannerDAO.DeleteResultadoScannerByScannerId(scannerDTO.Id);
                }
                

                //Percorrendo todos os ativos para o determinado scanner
                foreach (string ativo in ativos)
                {
                    if (ativo.Trim().Length > 0)
                    {
                        try
                        {
                            ResultadoScannerDTO resultado = new ResultadoScannerDTO();
                            resultado = EvaluateFormula(ativo.Trim().ToUpper(), scannerDTO);

                            if (resultado != null)
                                listaResultados.Add(resultado);
                        }
                        catch
                        { }
                    }
                }
                
                //Salvando a lista
                if (listaResultados.Count > 0)
                {
                    //Salvando o resultado
                    using (ResultadoScannerDAO resultadoDAO = new ResultadoScannerDAO(readConnection, writeConnection))
                    {
                        foreach (ResultadoScannerDTO obj in listaResultados)
                        {
                            resultadoDAO.SalvarListaResultadoScannerDiario(obj);
                        }
                    }

                    //fazendo o envio por email
                    if (scannerDTO.EnviarEmail)
                    {
                        UtilBM.EnviaEmailSES("noreply@traderdata.com.br", scannerDTO.User.Login, "Rastreador de oportunidades faceTrader",
                            MontaEmail(listaResultados, scannerDTO), true, nomeServico);
                    }

                    //setando os resulyados
                    scannerDTO.Resultados = listaResultados;
   
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /// <summary>
        /// Metodo auxiliar que monta o email
        /// </summary>
        /// <param name="listaResultado"></param>
        /// <param name="scanner"></param>
        /// <param name="macrocliente"></param>
        /// <returns></returns>
        private string MontaEmail(List<ResultadoScannerDTO> listaResultado, ScannerDTO scanner)
        {
            StringBuilder email = new StringBuilder();
            
            try
            {
                email.Append("<html><body>Prezado Cliente<BR><BR><BR>");
                email.Append("Confira os papéis que alcançaram os parâmetros definidos por você no Rastreador " + scanner.Nome + "<BR><BR>");
                email.Append("Lista de ativos:" + "<BR><BR>");

                email.Append("<table width=100% style='border:1px Solid'><tr>");
                email.Append("<TD style='border:1px Solid'>Papel</td><td style='border:1px Solid'>Cotação</td><td style='border:1px Solid'>Variação</td><td style='border:1px Solid'>Abertura</td><td style='border:1px Solid'>Mínima</td><td style='border:1px Solid'>Máxima</td><td style='border:1px Solid'>Volume</td></tr>");
                foreach (ResultadoScannerDTO obj in listaResultado)
                {
                    email.Append("<tr style='border:1px Solid'>");

                    email.Append("<td style='border:1px Solid'>" + obj.Ativo + "</td>");
                    email.Append("<td style='border:1px Solid'>" + obj.Fechamento + "</td>");
                    email.Append("<td style='border:1px Solid'>" + obj.Variacao.ToString("N2") + "</td>");
                    email.Append("<td style='border:1px Solid'>" + obj.Abertura + "</td>");
                    email.Append("<td style='border:1px Solid'>" + obj.Minimo + "</td>");
                    email.Append("<td style='border:1px Solid'>" + obj.Maximo + "</td>");
                    email.Append("<td style='border:1px Solid'>" + obj.Volume + "</td>");

                    email.Append("</tr>");
                }

                email.Append("</table><br><br><br>");
                email.Append("Atenciosamente,<br><br>");
                email.Append("Equipe faceTrader<br>");
                email.Append("https://www.facebook.com/webstockchart");
                
                return email.ToString();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /* Nome: EvaluateFormula
         * Descrição: Avalia a formula de acordo com o ativo passado
         * Data: 27/10/2009
         */
        public ResultadoScannerDTO EvaluateFormula(string ativo, ScannerDTO scannerDTO)
        {
            //Declarando variaveis auxiliares
            List<CotacaoServerDTO> listaCotacoes = new List<CotacaoServerDTO>();
            List<double> listaUltimo = new List<double>();
            List<double> listaMax = new List<double>();
            List<double> listaMin = new List<double>();
            List<double> listaAbertura = new List<double>();
            ResultadoScannerDTO objResultadoScanner = new ResultadoScannerDTO();
            bool resultadoParcial = false;

            try
            {
                using (CotacaoBM cotacaoBM = new CotacaoBM(true, false))
                {
                    //Resgatando os valores que serão calculados
                    switch (scannerDTO.Periodicidade)
                    {
                        case 1440:
                            listaCotacoes = cotacaoBM.GetCotacao(ativo, 1440, new DateTime(DateTime.Today.Year - 10, DateTime.Today.Month, DateTime.Today.Day), DateTime.Today,
                                    true, false, false, EnumGeral.BolsaEnum.Bovespa);
                            break;
                        case 10080:
                            listaCotacoes = cotacaoBM.GetCotacao(ativo, 10080, new DateTime(DateTime.Today.Year - 10, DateTime.Today.Month, DateTime.Today.Day), DateTime.Today,
                                    true, false, false, EnumGeral.BolsaEnum.Bovespa);
                            break;
                        case 43200:
                            listaCotacoes = cotacaoBM.GetCotacao(ativo, 43200, new DateTime(DateTime.Today.Year - 10, DateTime.Today.Month, DateTime.Today.Day), DateTime.Today,
                                    true, false, false, EnumGeral.BolsaEnum.Bovespa);
                            break;
                    }
                }

                //Populando listaUltimo
                foreach (CotacaoServerDTO obj in listaCotacoes)
                {
                    listaUltimo.Add(Convert.ToDouble(obj.Ultimo));
                    listaMax.Add(Convert.ToDouble(obj.Maximo));
                    listaMin.Add(Convert.ToDouble(obj.Minimo));
                    listaAbertura.Add(Convert.ToDouble(obj.Abertura));
                }

                using (CalculosEstatisticosScannerBM calculosBM = new CalculosEstatisticosScannerBM())
                {
                    #region Verificando as parcelas

                    //Percorrendo todos os valores presentes na formula
                    foreach (CondicaoDTO objCondicao in scannerDTO.ListaCondicoes)
                    {
                        //verificando qual o comando que será executado
                        switch (objCondicao.Comando)
                        {
                            case "SMA":
                                resultadoParcial = calculosBM.EvaluateCruzamentoDeDuasSMA(listaUltimo,
                                    objCondicao.ListaParcelas[0].ValorInteiro,
                                    objCondicao.ListaParcelas[1].ValorInteiro,
                                    0);
                                break;
                            case "EMA":
                                resultadoParcial = calculosBM.EvaluateCruzamentoDeDuasEMA(listaUltimo,
                                    objCondicao.ListaParcelas[0].ValorInteiro,
                                    objCondicao.ListaParcelas[1].ValorInteiro,
                                    0, "A");
                                break;
                            case "IFI":
                                resultadoParcial = calculosBM.EvaluateIFRInferior(listaUltimo,
                                    objCondicao.ListaParcelas[1].ValorInteiro,
                                    objCondicao.ListaParcelas[0].ValorInteiro,
                                    0);
                                break;
                            case "IFS":
                                resultadoParcial = calculosBM.EvaluateIFRSuperior(listaUltimo,
                                    objCondicao.ListaParcelas[1].ValorInteiro,
                                    objCondicao.ListaParcelas[0].ValorInteiro,
                                    0);
                                break;
                            case "ULI":
                                resultadoParcial = calculosBM.EvaluateVariacaoInferior(listaUltimo,
                                    objCondicao.ListaParcelas[0].ValorInteiro,
                                    0);
                                break;
                            case "ULS":
                                resultadoParcial = calculosBM.EvaluateVariacaoSuperior(listaUltimo,
                                    objCondicao.ListaParcelas[0].ValorInteiro,
                                    0);
                                break;
                            case "UMS":
                                resultadoParcial = calculosBM.EvaluateUltimoPrecoSuperiorAMediaSimples(listaUltimo,
                                    objCondicao.ListaParcelas[0].ValorInteiro,
                                    0);
                                break;
                            case "UBS":
                                resultadoParcial = calculosBM.EvaluateUltimoPrecoInferiorAMediaSimples(listaUltimo,
                                    objCondicao.ListaParcelas[0].ValorInteiro,
                                    0);
                                break;
                            case "DIC":
                                resultadoParcial = calculosBM.EvaluateDIDICompra(listaUltimo,
                                    0);
                                break;
                            case "DIV":
                                resultadoParcial = calculosBM.EvaluateDIDIVenda(listaUltimo,
                                    0);
                                break;
                            case "TRC":
                                resultadoParcial = calculosBM.EvaluateTRIXCompra(listaUltimo,
                                    objCondicao.ListaParcelas[0].ValorInteiro,
                                    objCondicao.ListaParcelas[1].ValorInteiro,
                                    0);
                                break;
                            case "TRB":
                                resultadoParcial = calculosBM.EvaluateTRIXVenda(listaUltimo,
                                    objCondicao.ListaParcelas[0].ValorInteiro,
                                    objCondicao.ListaParcelas[1].ValorInteiro,
                                    0);
                                break;
                            case "ESC":
                                resultadoParcial = calculosBM.EvaluateEstocasticoCompra(listaUltimo,
                                    listaMax,
                                    listaMin,
                                    objCondicao.ListaParcelas[0].ValorInteiro,
                                    3,//usando a velocidade/slowing 3
                                    objCondicao.ListaParcelas[1].ValorInteiro,
                                    0);
                                break;
                            case "ESV":
                                resultadoParcial = calculosBM.EvaluateEstocasticoVenda(listaUltimo,
                                    listaMax,
                                    listaMin,
                                    objCondicao.ListaParcelas[0].ValorInteiro,
                                    3,//usando a velocidade/slowing 3
                                    objCondicao.ListaParcelas[1].ValorInteiro,
                                    0);
                                break;
                            case "MAC":
                                resultadoParcial = calculosBM.EvaluateMACDCompra(listaUltimo,
                                    listaMax,
                                    listaMin,
                                    listaAbertura,
                                    objCondicao.ListaParcelas[0].ValorInteiro,
                                    objCondicao.ListaParcelas[1].ValorInteiro,
                                    objCondicao.ListaParcelas[2].ValorInteiro,
                                    0);
                                break;
                            case "MAV":
                                resultadoParcial = calculosBM.EvaluateMACDVenda(listaUltimo,
                                    listaMax,
                                    listaMin,
                                    listaAbertura,
                                    objCondicao.ListaParcelas[0].ValorInteiro,
                                    objCondicao.ListaParcelas[1].ValorInteiro,
                                    objCondicao.ListaParcelas[2].ValorInteiro,
                                    0);
                                break;
                        }

                        if (!resultadoParcial)
                            return null;

                    }

                    #endregion
                }

                //Montando o objeto resultado
                objResultadoScanner.Abertura = Convert.ToDouble(listaAbertura[listaAbertura.Count - 1]);
                objResultadoScanner.Ativo = ativo;
                objResultadoScanner.ScannerId = scannerDTO.Id;
                objResultadoScanner.Data = listaCotacoes[listaCotacoes.Count - 1].Data;
                objResultadoScanner.Fechamento = Convert.ToDouble(listaUltimo[listaUltimo.Count - 1]);
                objResultadoScanner.Id = 0;
                objResultadoScanner.Maximo = Convert.ToDouble(listaMax[listaMax.Count - 1]);
                objResultadoScanner.Minimo = Convert.ToDouble(listaMin[listaMin.Count - 1]);

                if (listaUltimo.Count > 2)
                    objResultadoScanner.Variacao = Convert.ToDouble(((listaUltimo[listaUltimo.Count - 1] - listaUltimo[listaUltimo.Count - 2]) / listaUltimo[listaUltimo.Count - 2]) * 100);
                else
                    objResultadoScanner.Variacao = 0;

                objResultadoScanner.Volume = Convert.ToDouble(listaCotacoes[listaCotacoes.Count - 1].Volume);

                //retornando true ou false para o escaneamento
                return objResultadoScanner;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        #endregion

        #region Metodos Read

        /// <summary>
        /// Metodo que retorna todas as condições
        /// </summary>
        /// <returns></returns>
        public List<CondicaoDTO> RetornaTodasCondicoes()
        {
            try
            {
                using (CondicaoDAO condicaoDAO = new CondicaoDAO(readConnection, writeConnection))
                {
                    return condicaoDAO.ReturnAll();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        /// <summary>
        /// Metodo que retorna a lista de scanners by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ScannerDTO GetScannerDiarioById(int id)
        {
            try
            {
                using (ScannerDAO scannerDAO = new ScannerDAO(readConnection, writeConnection))
                {
                    return scannerDAO.GetScannerById(id);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /* Método: RetornaScannersPorCliente
         * Date: 16/10/2009
         * Description: Retorna uma lista com todos os meus scanners validos e todos os scanners publicos do macro
         * cliente
         */
        public List<ScannerDTO> RetornaScannersDiarioPorCliente(int userId)
        {
            try
            {
                using (ScannerDAO scannerDAO = new ScannerDAO(readConnection, writeConnection))
                {
                    return scannerDAO.GetScanners(userId);
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /* Método:RetornaResultadosScanner
         * Descrição: Retorna a lista de ativos que foi disparada como resultado do scanner acionado
         * Data: 01/09/2008
         */
        public List<ResultadoScannerDTO> RetornaResultadosScannerDiario(int codigoScanner)
        {
            try
            {
                using (ResultadoScannerDAO resultadoDAO = new ResultadoScannerDAO(readConnection, writeConnection))
                {
                    //Executando a query
                    return resultadoDAO.GetResultadoScanner(codigoScanner);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /* Metodo: GetParcelaByCondicao
         * Descricao: Traz uma lista de parcelas que deverão ser apresentadas ao usuário para que possam
         * ser informados os parametros
         * Data: 31/01/2012
         */
        public List<CondicaoParcelaDTO> GetParcelaByCondicao(int condicaoId)
        {
            try
            {
                using (CondicaoParcelaDAO condicaoDAO = new CondicaoParcelaDAO(readConnection, writeConnection))
                {
                    return condicaoDAO.GetParcelaByCondicao(condicaoId);
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
        /// Metodo que retorna o objeto scanner que fora salvo
        /// </summary>
        /// <param name="scannerDTO"></param>
        /// <returns></returns>
        public ScannerDTO SaveScanner(ScannerDTO scannerDTO)
        {
            //Variaveis auxiliares
            bool bFirst = false;
            string formulaAux = "";

            try
            {


                #region Limpando as condições valor pre-existentes caso seja uma alteração

                //verificando se o scanner é novo ou edição
                if (scannerDTO.Id != 0)
                {
                    //Excluindo os resultados ja existentes
                    using (ResultadoScannerDAO resultadoScannerDAO = new ResultadoScannerDAO(readConnection, writeConnection))
                    {
                        //Apagando todas as parcelas
                        resultadoScannerDAO.DeleteResultadoScannerByScannerId(scannerDTO.Id);
                    }

                    //Excluindo as condições pre-existentes para este scanner
                    using (ScannerCondicaoValorDAO scannerCondicaoDAO = new ScannerCondicaoValorDAO(readConnection, writeConnection))
                    {
                        //Apagando todas as parcelas
                        scannerCondicaoDAO.DeleteCondicaoValorByScannerId(scannerDTO.Id);
                    }

                    //Excluindo as condições pre-existentes para este scanner
                    using (ScannerDAO scannerDAO = new ScannerDAO(readConnection, writeConnection))
                    {
                        //Apagando todas as parcelas
                        scannerDAO.ExcluirScanner(scannerDTO);
                    }
                }

                #endregion

                #region Gerando a Formula

                //Gerando a formula e armazenando a mesma
                foreach (CondicaoDTO objCondicao in scannerDTO.ListaCondicoes)
                {
                    if (!bFirst)
                        formulaAux += " AND ";

                    string sParcelaAux = "";
                    foreach (CondicaoParcelaDTO condicaoParcela in objCondicao.ListaParcelas)
                    {
                        switch (condicaoParcela.TipoFisico)
                        {
                            case "I":
                                sParcelaAux += condicaoParcela.ValorInteiro.ToString();
                                break;
                            case "S":
                                sParcelaAux += condicaoParcela.ValorString;
                                break;
                            case "D":
                                sParcelaAux += condicaoParcela.ValorDouble.ToString();
                                break;
                        }

                        sParcelaAux += ";";
                    }
                    formulaAux += objCondicao.Comando + "[" + sParcelaAux + "]";
                    bFirst = false;
                }

                //Armazenando o scanner com a formula parseada;
                scannerDTO.Formula = formulaAux;

                #endregion

                #region Salvando o scanner

                //Salvando o scanner
                using (ScannerDAO scannerDAO = new ScannerDAO(readConnection, writeConnection))
                {
                    scannerDAO.InserirScanner(scannerDTO);
                }

                #endregion

                #region Salvando a condição
                //Salvando os valoers de cada condição
                using (ScannerCondicaoValorDAO scannerCondicaoValorDAO = new ScannerCondicaoValorDAO(readConnection, writeConnection))
                {
                    //Percorrendo as condicoes e parcelas para salvar as condicao parcelas e valores
                    foreach (CondicaoDTO condicao in scannerDTO.ListaCondicoes)
                    {
                        foreach (CondicaoParcelaDTO condicaoParcela in condicao.ListaParcelas)
                        {
                            ScannerCondicaoValorDTO scannerCondicaoValorDTO = new ScannerCondicaoValorDTO();
                            scannerCondicaoValorDTO.Id = 0;
                            scannerCondicaoValorDTO.CondicaoId = condicao.Id;
                            scannerCondicaoValorDTO.ParcelaId = condicaoParcela.Id;
                            scannerCondicaoValorDTO.ScannerId = scannerDTO.Id;
                            scannerCondicaoValorDTO.ValorDouble = condicaoParcela.ValorDouble;
                            scannerCondicaoValorDTO.ValorInteiro = condicaoParcela.ValorInteiro;
                            scannerCondicaoValorDTO.ValorString = condicaoParcela.ValorString;

                            //Salvando
                            scannerCondicaoValorDAO.InsereCondicaoValor(scannerCondicaoValorDTO);
                        }

                    }
                }
                #endregion
                
                //processar scanner
                ProcessaScanner(ref scannerDTO);

                //comitando
                transaction.Commit();

                //Retornando o scanner
                return scannerDTO;
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                throw exc;
            }
        }

        /* Método: DeleteScanner
         * Date: 16/10/2009
         * Description: Apaga um scanner
         */
        public void DeleteScanner(ScannerDTO scannerDTO)
        {
            //Variaveis 
            List<ScannerCondicaoValorDTO> listaDelete = new List<ScannerCondicaoValorDTO>();

            try
            {
                #region Apagando os resultados existentes
                //Resgatando as parcelas que devem ser excluidas
                using (ResultadoScannerDAO resultadoScannerDAO
                    = new ResultadoScannerDAO(readConnection, writeConnection))
                {
                    resultadoScannerDAO.DeleteResultadoScannerByScannerId(scannerDTO.Id);
                }
                #endregion

                #region Apagando as condições
                //Resgatando as parcelas que devem ser excluidas
                using (ScannerCondicaoValorDAO scannerValorCondicaoDAO
                    = new ScannerCondicaoValorDAO(readConnection, writeConnection))
                {
                    scannerValorCondicaoDAO.DeleteCondicaoValorByScannerId(scannerDTO.Id);
                }
                #endregion

                #region Excluindo o Scanner

                using (ScannerDAO scannerDAO = new ScannerDAO(readConnection, writeConnection))
                {
                    //Excluindo o scanner
                    scannerDAO.ExcluirScanner(scannerDTO);
                }
                #endregion

                transaction.Commit();
            }
            catch (Exception exc)
            {
                //Dando rollback na transação
                transaction.Rollback();
                throw exc;
            }
        }

        #endregion
    }
}
