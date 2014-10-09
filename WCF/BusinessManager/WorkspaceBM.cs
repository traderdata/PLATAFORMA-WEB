using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;
using Traderdata.Server.App.TerminalWeb.DAO;

namespace Traderdata.Server.App.TerminalWeb.BusinessManager
{
    public class WorkspaceBM:BaseBM
    {
        #region Constructor
 
        //construtor basico que nao ira implementar o construtor do pai
        public WorkspaceBM(bool leitura, bool escrita, string nomeServico)
            : base(leitura, escrita, nomeServico)
        {
        }

        #endregion         

        #region Metodos Read

        /// <summary>
        /// Metodo que retorna o workspace default do distribuidor
        /// </summary>
        /// <param name="distribuidorId"></param>
        /// <returns></returns>
        public WorkspaceDTO GetWorkspaceDefaultPorDistribuidor(int distribuidorId)
        {
            try
            {
                using (WorkspaceDAO workspaceDAO = new WorkspaceDAO(readConnection, writeConnection))
                {
                    return workspaceDAO.GetWorkspaceDefaultPorDistribuidor(distribuidorId)[0];
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        /// <summary>
        /// Metodo que retorna o workspace 0 de determinado usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public WorkspaceDTO GetWorkspaceDefault(UsuarioDTO User)
        {
            try
            {
                using (WorkspaceDAO workspaceDAO = new WorkspaceDAO(readConnection, writeConnection))
                {
                    return workspaceDAO.GetWorkspacesByUser(User)[0];
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
        /// Metodo que salva o workspace passado sobre o workspace existente
        /// </summary>
        /// <param name="workspace"></param>
        public void SaveWorkspaceDefault(WorkspaceDTO workspace)
        {
            try
            {
                using (WorkspaceDAO workspaceDAO = new WorkspaceDAO(readConnection, writeConnection))
                {
                    //Excluir o template antigo
                    workspaceDAO.ExcluirWorkspacesUsuario(workspace.UsuarioId);

                    //Inserindo o novo template
                    workspaceDAO.InsereWorkspace(workspace);
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
