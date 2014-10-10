using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;
using Traderdata.Server.App.TerminalWeb.DAO;

namespace Traderdata.Server.App.TerminalWeb.BusinessManager
{
    public class TemplateBM:BaseBM
    {
        #region Constructor
 
        //construtor basico que nao ira implementar o construtor do pai
        public TemplateBM()
            : base()
        {
        }

        //construtor basico que nao ira implementar o construtor do pai
        public TemplateBM(string connRead, string connWrite)
            : base(connRead, connWrite)
        {
        }

        //construtor basico que nao ira implementar o construtor do pai
        public TemplateBM(bool leitura, bool escrita)
            : base(leitura, escrita, BaseBM.BMType.Default)
        {
        }

        #endregion

        #region Read

        /// <summary>
        /// Return user by it´s login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public List<TemplateDTO> GetTemplatesByUser(int userId)
        {
            try
            {
                using (TemplateDAO templateDAO = new TemplateDAO(readConnection, writeConnection))
                {
                    return templateDAO.GetTemplatesByUser(userId);
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
        /// Method that will insert a user in database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public TemplateDTO SaveTemplate(TemplateDTO template)
        {
            try
            {
                transaction = writeConnection.BeginTransaction();

                //inserindo o template
                using (TemplateDAO templateDAO = new TemplateDAO(readConnection, writeConnection))
                {
                    template = templateDAO.InsertTemplate(template);
                }

                //inserindo o layout
                using (LayoutDAO layoutDAO = new LayoutDAO(readConnection, writeConnection))
                {
                    template.Layout.TemplateId = template.Id;
                    layoutDAO.InsertLayout(template.Layout);
                }

                transaction.Commit();

                return template;
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
