using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class UserDAO:BaseDAO
    {
        #region Construtor

        public UserDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Read

        /// <summary>
        /// Get user by it´s login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public UserDTO GetUserByLogin(string login)
        {
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {

                string query = "SELECT * FROM USUARIO WHERE USUA_NM_LOGIN = ?login  ";
                
                //Executando a query
                using (MySqlCommand command = new MySqlCommand(query, readConnection))
                {
                    //setando os parametros
                    command.Parameters.AddWithValue("?login", login);

                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        UserDTO user = new UserDTO();
                        reader.Read();
                        
                        user.Id = reader.GetInt32("USUA_CD_USUARIO");
                        user.Login = reader.GetString("USUA_NM_LOGIN");

                        return user;

                    }
                    else
                        return null;
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                reader.Close();
            }

        }

        #endregion

        #region Write

        /// <summary>
        /// Insert new user
        /// </summary>
        /// <param name="cotacao"></param>
        public UserDTO InsertUser(UserDTO user)
        {
            StringBuilder sql = new StringBuilder();

            try
            {


                sql.Append("INSERT INTO USUARIO ( USUA_NM_LOGIN ) VALUES ");
                sql.Append("( ?login )");


                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(sql.ToString(), writeConnection))
                {
                    //adicionando parametros
                    command.Parameters.AddWithValue("?login", user.Login);
                    command.ExecuteNonQuery();
                    user.Id = Convert.ToInt32(command.LastInsertedId);
                    return user;
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
