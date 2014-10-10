using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;
using Traderdata.Server.App.TerminalWeb.DAO;

namespace Traderdata.Server.App.TerminalWeb.BusinessManager
{
    public class UserBM:BaseBM
    {
        #region Constructor
 
        //construtor basico que nao ira implementar o construtor do pai
        public UserBM()
            : base()
        {
        }

        //construtor basico que nao ira implementar o construtor do pai
        public UserBM(string connRead, string connWrite)
            : base(connRead, connWrite)
        {
        }

        //construtor basico que nao ira implementar o construtor do pai
        public UserBM(bool leitura, bool escrita)
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
        public UserDTO GetUserByLogin(string login)
        {
            try
            {
                using (UserDAO userDAO = new UserDAO(readConnection, writeConnection))
                {
                    return userDAO.GetUserByLogin(login);
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
        public UserDTO InsertUser(string login)
        {
            try
            {
                UserDTO user = new UserDTO();
                transaction = writeConnection.BeginTransaction();

                using (UserDAO userDAO = new UserDAO(readConnection, writeConnection))
                {                    
                    user.Login = login;
                    userDAO.InsertUser(user);
                }

                transaction.Commit();

                return user;
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                return null;
                throw exc;
            }
        }

        #endregion
    }
}
