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
        public TemplateBM(bool leitura, bool escrita, string nomeServico)
            : base(leitura, escrita, nomeServico)
        {
        }

        #endregion         

        #region Metodos Read

        /// <summary>
        /// Retorna templates por id do cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<TemplateDTO> RetornaTemplatesPorUserId(int userId)
        {
            try
            {
                using (TemplateDAO templateDAO = new TemplateDAO(readConnection, writeConnection))
                {
                    return templateDAO.RetornaTemplatesPorUserId(userId);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que retorna template por id
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public TemplateDTO ReturnById(int id)
        {
            try
            {
                using (TemplateDAO templateDAO = new TemplateDAO(readConnection, writeConnection))
                {
                    return templateDAO.ReturnById(id);
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
        /// MEtodo que executa um save ou update
        /// </summary>
        /// <param name="templateDTO"></param>
        public void SaveOrUpdate(TemplateDTO templateDTO)
        {
            try
            {
                if (templateDTO.Id == 0)
                    InserirTemplate(templateDTO);
                else
                    EditarTemplate(templateDTO);
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        /// <summary>
        /// Metodo responsavel por salvar o template
        /// </summary>
        /// <param name="templateDTO"></param>
        public void InserirTemplate(TemplateDTO templateDTO)
        {
            try
            {
                using (TemplateDAO templateDAO = new TemplateDAO(readConnection, writeConnection))
                {
                    templateDAO.SalvarTemplate(templateDTO);
                }

                transaction.Commit();

            }
            catch (Exception exc)
            {
                transaction.Rollback();
                throw exc;
            }
        }

        /// <summary>
        /// Metodo responsavel por editar o template
        /// </summary>
        /// <param name="templateDTO"></param>
        public void EditarTemplate(TemplateDTO templateDTO)
        {
            try
            {
                using (TemplateDAO templateDAO = new TemplateDAO(readConnection, writeConnection))
                {
                    //Excluir o template antigo
                    templateDAO.ExcluirTemplate(templateDTO);

                    //Inserindo o novo template
                    templateDAO.SalvarTemplate(templateDTO);
                }

                transaction.Commit();
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                throw exc;
            }
        }

        /// <summary>
        /// Metodo responsavel por excluir o template
        /// </summary>
        /// <param name="templateDTO"></param>
        public void ExcluirTemplate(TemplateDTO templateDTO)
        {
            try
            {
                using (TemplateDAO templateDAO = new TemplateDAO(readConnection, writeConnection))
                {
                    //Excluir o template
                    templateDAO.ExcluirTemplate(templateDTO);
                }

                transaction.Commit();
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
