using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;
using Traderdata.Server.App.TerminalWeb.DAO;

namespace Traderdata.Server.App.TerminalWeb.BusinessManager
{
    public class SocialBM:BaseBM
    {
        #region Constructor
 
        //construtor basico que nao ira implementar o construtor do pai
        public SocialBM(bool leitura, bool escrita, string nomeServico)
            : base(leitura, escrita, nomeServico)
        {
        }

        #endregion

        #region Metodos Write

        /// <summary>
        /// Metodo que faz a insrção de uma mensagem de chat
        /// </summary>
        /// <param name="mensagem"></param>
        public void InsereMensagem(MensagemDTO mensagem)
        {
            try
            {
                using (MensagemDAO mensagemDAO = new MensagemDAO(readConnection, writeConnection))
                {
                    mensagemDAO.InsereMensagem(mensagem);
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        #endregion
    }
}
