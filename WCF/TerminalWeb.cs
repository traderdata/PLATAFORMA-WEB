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

        /// <summary>
        /// Lista de templates
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TemplateDTO> GetTemplatesByUser(int userId)
        {
            try
            {
                using (TemplateBM templateBM = new TemplateBM(true, true))
                {
                    return templateBM.GetTemplatesByUser(userId);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(serviceGroup, serviceName, "GetTemplatesByUser", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que salva o template
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public TemplateDTO SaveTemplate(TemplateDTO template)
        {
            try
            {
                using (TemplateBM templateBM = new TemplateBM(true, true))
                {
                    return templateBM.SaveTemplate(template);                    
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(serviceGroup, serviceName, "SaveTemplate", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que apaga o template
        /// </summary>
        /// <param name="template"></param>
        public void DeleteTemplate(TemplateDTO template)
        {
            try
            {
                using (TemplateBM templateBM = new TemplateBM(true, true))
                {
                    templateBM.DeleteTemplate(template);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(serviceGroup, serviceName, "DeleteTemplate", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        #endregion

        #region Grafico

        /// <summary>
        /// Metodo que retorna um grafico
        /// </summary>
        /// <param name="ativo"></param>
        /// <param name="userId"></param>
        /// <param name="periodicidade"></param>
        /// <returns></returns>
        public GraficoDTO GetGrafico(string ativo, int userId, int periodicidade)
        {
            try
            {
                using (GraficoBM graficoBM = new GraficoBM(true, true))
                {
                    return graficoBM.GetGrafico(ativo, userId, periodicidade);                        
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(serviceGroup, serviceName, "SaveGrafico", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }


        /// <summary>
        /// Metodo que salva o template
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public GraficoDTO SaveGrafico(GraficoDTO grafico)
        {
            try
            {
                using (GraficoBM graficoBM = new GraficoBM(true, true))
                {
                    return graficoBM.SaveGrafico(grafico);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(serviceGroup, serviceName, "SaveGrafico", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna a lista de gráficos que o usuario possui
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<GraficoDTO> GetGraficosByUserId(int userId)
        {
            try
            {
                using (GraficoBM graficoBM = new GraficoBM(true, true))
                {
                    return graficoBM.GetGraficoByUserId(userId);
                }
            }
            catch (Exception exc)
            {
                LogServicoBM.LogaEvento(serviceGroup, serviceName, "SaveGrafico", exc.StackTrace + " || " + exc.ToString(), EventLogEntryType.Error);
                throw exc;
            }
        }

        #endregion
        
        
        #region fake

        
        public ObjetoEstudoDTO GetObjetoFake()
        {
            return new ObjetoEstudoDTO();
        }

        public LayoutDTO GetLIndicatorFake()
        {
            return new LayoutDTO();
        }

        public IndicadorDTO GetIndicatorFake()
        {
            return new IndicadorDTO();
        }
        #endregion

    }
}
