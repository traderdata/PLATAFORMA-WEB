using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class BaseDAO : IDisposable
    {
        protected MySqlConnection readConnection;
        protected MySqlConnection writeConnection;

        #region Construtor

        public BaseDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
        {
            this.readConnection = readConnection;
            this.writeConnection = writeConnection;
        }
        public void Dispose()
        {
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Executa as queries externas
        /// </summary>
        /// <param name="query"></param>
        public void ExecuteExternalQuery(string query)
        {
            try
            {

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(query, writeConnection))
                {
                    command.ExecuteNonQuery();
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
