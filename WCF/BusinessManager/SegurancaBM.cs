using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;
using Traderdata.Server.App.TerminalWeb.DAO;
using Traderdata.Server.Core.BusinessManager;

namespace Traderdata.Server.App.TerminalWeb.BusinessManager
{
    public class SegurancaBM:BaseBM
    {
        #region Constructor
 
        //construtor basico que nao ira implementar o construtor do pai
        public SegurancaBM(bool leitura, bool escrita, string nomeServico)
            : base(leitura, escrita, nomeServico)
        {
        }

        #endregion         

        #region Metodos Read

        /// <summary>
        /// Metodo que retorna todos os usuário que já tiveram publicações públicas
        /// </summary>
        /// <returns></returns>
        public List<UsuarioDTO> RetornaTodosPublicadores()
        {
            try
            {
                using (UsuarioDAO usuarioDAO = new UsuarioDAO(readConnection, writeConnection))
                {
                    return usuarioDAO.RetornaTodosPublicadores();
                }
            }
            catch (Exception exc)
            {                
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que faz a inserção de um usuario logado
        /// </summary>
        /// <param name="usuario"></param>
        public void Ping(UsuarioLogadoDTO usuario)
        {
            try
            {
                using (UsuarioLogadoDAO usuarioDAO = new UsuarioLogadoDAO(readConnection, writeConnection))
                {
                    usuarioDAO.Ping(usuario);
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
        /// Metodo que valida o usuário para permitir o login
        /// </summary>
        /// <param name="login"></param>
        /// <param name="senha"></param>
        /// <returns></returns>
        public UsuarioDTO Login(string login, string senha)
        {
            try
            {
                using (UsuarioDAO usuarioDAO = new UsuarioDAO(readConnection, writeConnection))
                {
                    UsuarioDTO usuario = usuarioDAO.Login(login, senha);
                    usuario.Token = Guid.NewGuid().ToString();
                    transaction.Commit();
                    return usuarioDAO.CreateNewToken(usuario);
                }

            }
            catch (Exception exc)
            {
                transaction.Rollback();
                throw exc;
            }
        }

        /// <summary>
        /// Retorna todos os usuarios cadastrados naquela data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<UsuarioDTO> RetornaUsuariosPorDataCadastro(DateTime data)
        {
            using (UsuarioDAO usuarioDAO = new UsuarioDAO(readConnection, writeConnection))
            {
                return usuarioDAO.RetornaUsuariosPorDataCadastro(data);
            }
        }

        /// <summary>
        /// Retorna todos os usuarios 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<UsuarioDTO> RetornaUsuarios()
        {
            using (UsuarioDAO usuarioDAO = new UsuarioDAO(readConnection, writeConnection))
            {
                return usuarioDAO.RetornaUsuarios();
            }
        }

        #endregion

        #region Metodos Write

        /// <summary>
        /// Metodo que verifica se usuario com o login existe, caso exista, retorna ele
        /// caso contrario deve cadastrar o usuairo e retorna-lo
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public UsuarioDTO LoginUserFacebook(string login, string token)
        {
            UsuarioDTO userFacebook = null;
            try
            {
                using (UsuarioDAO usuarioDAO = new UsuarioDAO(readConnection, writeConnection))
                {
                    userFacebook = usuarioDAO.RetornaUsuarioPorLogin(login);

                    if (userFacebook != null)
                    {
                        //atualiza o token
                        userFacebook.FBToken = token;
                        userFacebook.Token = Guid.NewGuid().ToString();
                        usuarioDAO.EditarUsuario(userFacebook);

                    }                    
                    else
                    {
                        userFacebook = new UsuarioDTO();
                        userFacebook.BMFRT = DateTime.Today.AddDays(6);
                        userFacebook.BovespaRT = DateTime.Today.AddDays(6);
                        userFacebook.BMFDELAY = DateTime.Today.AddDays(15);
                        userFacebook.BovespaDELAY = DateTime.Today.AddDays(15);
                        userFacebook.BMFEOD = DateTime.Today.AddDays(30);
                        userFacebook.BovespaEOD = DateTime.Today.AddDays(30);
                        userFacebook.Perfil = "U";
                        userFacebook.Login = login;
                        userFacebook.DistribuidorId = 1;
                        userFacebook.FBToken = token;
                        userFacebook.Token = Guid.NewGuid().ToString();
                        userFacebook = usuarioDAO.InserirUsuario(userFacebook);
                    }

                }

                transaction.Commit();

                //retornando usuario
                return userFacebook;
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                return null;
            }
        }

        /// <summary>
        /// Metodo que verifica se usuario com o login existe, caso exista, retorna ele
        /// caso contrario deve cadastrar o usuairo e retorna-lo
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public UsuarioDTO LoginUserDistribuidorIntegrado(string login, int distribuidor)
        {
            UsuarioDTO user = null;
            try
            {
                using (UsuarioDAO usuarioDAO = new UsuarioDAO(readConnection, writeConnection))
                {
                    user = usuarioDAO.RetornaUsuarioPorLoginEDistribuidor(login, distribuidor);

                    if (user != null)
                    {
                        //atualiza o token
                        return user;
                    }
                    else
                    {
                        user = new UsuarioDTO();
                        user.Perfil = "U";
                        user.Login = login;
                        user.Nome = login;
                        user.CPF = login;
                        user.DistribuidorId = distribuidor;
                        user.Token = Guid.NewGuid().ToString();
                        user = usuarioDAO.InserirUsuario(user);
                        user.HasSnapshotBMFDiario = true;
                        user.HasSnapshotBMFIntraday = true;
                        user.HasSnapshotBovespaDiario = true;
                        user.HasSnapshotBovespaIntraday = true;
                        user.HasBovespaRT = true;
                        user.HasBMFRT = true;
                    }

                }

                transaction.Commit();

                //retornando usuario
                return user;
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                return null;
            }
        }

        /// <summary>
        /// Metodo que faz a inserção de um novo usuairo
        /// </summary>
        /// <param name="usuario"></param>
        public UsuarioDTO InserirUsuario(UsuarioDTO usuario)
        {
            UsuarioDTO usuarioTemp = null;
            try
            {
                
                using (UsuarioDAO usuarioDAO = new UsuarioDAO(readConnection, writeConnection))
                {
                    UsuarioDTO usuaAux = usuarioDAO.RetornaUsuarioPorLogin(usuario.Login);

                    if (usuaAux != null)
                        return null;

                    usuaAux = usuarioDAO.RetornaUsuarioPorCPF(usuario.CPF);
                    if (usuaAux != null)
                        return null;

                    usuario.Token = Guid.NewGuid().ToString();
                    usuarioTemp = usuarioDAO.InserirUsuario(usuario);
                }

                transaction.Commit();

                return usuarioTemp;
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que faz a inserção de um novo usuairo
        /// </summary>
        /// <param name="usuario"></param>
        public UsuarioDTO EditarUsuario(UsuarioDTO usuario)
        {
            UsuarioDTO usuarioTemp = null;
            try
            {                
                using (UsuarioDAO usuarioDAO = new UsuarioDAO(readConnection, writeConnection))
                {
                    usuarioTemp = usuarioDAO.EditarUsuario(usuario);
                }

                transaction.Commit();
                return usuarioTemp;
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
