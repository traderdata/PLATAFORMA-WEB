using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;
using Traderdata.Server.App.TerminalWeb.DAO;

namespace Traderdata.Server.App.TerminalWeb.BusinessManager
{
    public class GraficoBM:BaseBM
    {
        #region Constructor
 
        //construtor basico que nao ira implementar o construtor do pai
        public GraficoBM()
            : base()
        {
        }

        //construtor basico que nao ira implementar o construtor do pai
        public GraficoBM(string connRead, string connWrite)
            : base(connRead, connWrite)
        {
        }

        //construtor basico que nao ira implementar o construtor do pai
        public GraficoBM(bool leitura, bool escrita)
            : base(leitura, escrita, BaseBM.BMType.Default)
        {
        }

        #endregion

        #region Read

        /// <summary>
        /// Metodo que retorna periodicidade
        /// </summary>
        /// <param name="ativo"></param>
        /// <param name="userId"></param>
        /// <param name="periodicidade"></param>
        /// <returns></returns>
        public GraficoDTO GetGrafico(string ativo, int userId, int periodicidade)
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

        /// <summary>
        /// Metodo que retorna periodicidade
        /// </summary>
        /// <param name="ativo"></param>
        /// <param name="userId"></param>
        /// <param name="periodicidade"></param>
        /// <returns></returns>
        public List<GraficoDTO> GetGraficoByUserId(int userId)
        {
            try
            {
                using (GraficoDAO graficoDAO = new GraficoDAO(readConnection, writeConnection))
                {
                    return graficoDAO.GetGraficoByUserId(userId);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        #endregion

        #region Write

        /// <summary>
        /// Insere grafico
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public GraficoDTO SaveGrafico(GraficoDTO grafico)
        {
            try
            {
                transaction = writeConnection.BeginTransaction();
                GraficoDTO graficoEdit = null;
                                
                //verificando se devo editar o grafico
                using (GraficoDAO graficoDAO = new GraficoDAO(readConnection, writeConnection))
                {
                    graficoEdit = graficoDAO.RetornaGraficoPorAtivoPeriodicidade(grafico.Ativo, grafico.Periodicidade, grafico.UsuarioId);                        
                }

                if (graficoEdit != null)
                {                    
                    //grafico
                    using (GraficoDAO graficoDAO = new GraficoDAO(readConnection, writeConnection))
                    {
                        graficoDAO.ExcluiGrafico(graficoEdit);
                    }

                    //layouts, indicadores, paineis, objetos serão excluidos por cascade
                }


                //inserindo o template
                using (GraficoDAO graficoDAO = new GraficoDAO(readConnection, writeConnection))
                {                    
                    grafico = graficoDAO.SaveGrafico(grafico);
                }

                //inserindo o layout
                using (LayoutDAO layoutDAO = new LayoutDAO(readConnection, writeConnection))
                {
                    grafico.Layout.GraficoId = grafico.Id;
                    layoutDAO.InsertLayout(grafico.Layout);
                }

                transaction.Commit();

                return grafico;
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                throw exc;
            }
        }

        #endregion
    }
}
