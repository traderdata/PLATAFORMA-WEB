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
using Traderdata.Server.App.TerminalWeb.DTO;
using Traderdata.Server.App.TerminalWeb.BusinessManager;
using System.Configuration;
using Traderdata.Server.Core.BusinessManager;


namespace Traderdata.Server.App.TerminalWeb
{
    public class TerminalWeb : ITerminalWeb
    {

        private string serviceGroup = "WCF";
        private string serviceName = "TWEB2";
        
        
        #region Usuario

        /// <summary>
        /// Metodo que faz a inserção de um novo usuairo
        /// </summary>
        /// <param name="usuario"></param>
        public UserDTO LoginOrInsertUser(string login)
        {
            try
            {
                using (UserBM userBM = new UserBM(true, true))
                {
                    UserDTO userTemp = userBM.GetUserByLogin(login);

                    if (userTemp == null)
                    {
                        //setando as datas de liberação do serviço
                        return userBM.InsertUser(login);
                    }
                    else
                        return userTemp;
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(serviceGroup, serviceName, "LoginOrInsertUser", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }
        
        #endregion

        #region Templates

        public TemplateDTO GetTemplateFake()
        {
            return new TemplateDTO();

        }

        public GraficoDTO GetGraficoFake()
        {
            return new GraficoDTO();
        }

        public ObjetoEstudoDTO GetObjetoFake()
        {
            return new ObjetoEstudoDTO();
        }

        
        public IndicadorDTO GetIndicatorFake()
        {
            return new IndicadorDTO();
        }
        #endregion

    }
}
