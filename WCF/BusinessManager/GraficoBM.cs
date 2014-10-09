using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;
using Traderdata.Server.App.TerminalWeb.DAO;


namespace Traderdata.Server.App.TerminalWeb.BusinessManager
{
    public class GraficoBM : BaseBM
    {
        #region Constructor
 
        //construtor basico que nao ira implementar o construtor do pai
        public GraficoBM(bool leitura, bool escrita, string nomeServico)
            : base(leitura, escrita, nomeServico)
        {
        }

        #endregion

        #region Metodos Write

        /// <summary>
        /// Metodo responsavel por salvar um grafico no perfil do usuario
        /// </summary>
        /// <param name="grafico"></param>
        public void SaveGrafico(GraficoDTO grafico)
        {
            try
            {
                using (GraficoDAO graficoDAO = new GraficoDAO(readConnection, writeConnection))
                {
                    graficoDAO.InserirGrafico(grafico);
                    transaction.Commit();
                }
            }
            catch (Exception exc)
            {
                transaction.Rollback();
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
                using (GraficoDAO graficoDAO = new GraficoDAO(readConnection, writeConnection))
                {
                    return graficoDAO.RetornaGraficoPorAtivoPeriodicidade(ativo, periodicidade, userId);                 
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        #endregion

        #region Metodos Read

   

        #endregion
    }
}
