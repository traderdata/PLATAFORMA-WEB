using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;
using Traderdata.Server.App.TerminalWeb.DAO;

namespace Traderdata.Server.App.TerminalWeb.BusinessManager
{
    public class PortfolioBM:BaseBM
    {
        #region Constructor
 
        //construtor basico que nao ira implementar o construtor do pai
        public PortfolioBM(bool leitura, bool escrita, string nomeServico)
            : base(leitura, escrita, nomeServico)
        {
        }

        #endregion

        #region  Metodos Read

        /// <summary>
        /// Metodo que retorna a lista de portfolios do cliente
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<PortfolioDTO> RetornaTodosPorClienteMaisPadroes(UsuarioDTO user)
        {
            try
            {
                using (PortfolioDAO portfolioDAO = new PortfolioDAO(readConnection, writeConnection))
                {
                    //Excluir o template antigo
                    return portfolioDAO.RetornaTodosPorClienteMaisPadroes(user);
                }
             
            }
            catch (Exception exc)
            {
                throw exc;
            }
         
        }

        #endregion
    }
}
