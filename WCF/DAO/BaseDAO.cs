using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class BaseDAO:IDisposable
    {
        protected MySqlConnection readConnection;
        protected MySqlConnection writeConnection;

        public BaseDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
        {
            this.readConnection = readConnection;
            this.writeConnection = writeConnection;
        }

        public void Dispose()
        {
        }
    }
}
