using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;
using Traderdata.Server.App.TerminalWeb.DAO;

namespace Traderdata.Server.App.TerminalWeb.BusinessManager
{
    public class BaseBM : IDisposable
    {
        #region Variaveis privadas

        protected MySqlConnection readConnection = new MySqlConnection();
        protected MySqlConnection writeConnection = new MySqlConnection();
        protected MySqlTransaction transaction;

        #endregion

        #region Construtor

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="transacionado"></param>
        public BaseBM(bool leitura, bool escrita, BMType readType)
        {
            try
            {
                if (leitura)
                {
                    switch (readType)
                    {
                        case BMType.Security:
                            readConnection = new MySqlConnection(ConfigurationSettings.AppSettings["CONNECTION-SECURITY-READ"]);
                            break;
                        case BMType.Default:
                            readConnection = new MySqlConnection(ConfigurationSettings.AppSettings["CONNECTION-READ"]);
                            break;
                    }

                    readConnection.Open();
                }

                if (escrita)
                {
                    switch (readType)
                    {
                        case BMType.Security:
                            writeConnection = new MySqlConnection(ConfigurationSettings.AppSettings["CONNECTION-SECURITY-SAVE"]);
                            break;
                        case BMType.Default:
                            writeConnection = new MySqlConnection(ConfigurationSettings.AppSettings["CONNECTION-SAVE"]);
                            break;
                    }

                    writeConnection.Open();
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="transacionado"></param>
        public BaseBM()
        {
            try
            {
                readConnection = new MySqlConnection(ConfigurationSettings.AppSettings["CONNECTION-READ"]);
                readConnection.Open();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="transacionado"></param>
        public BaseBM(string connRead, string connWrite)
        {
            try
            {
                if (connRead != "")
                {
                    readConnection = new MySqlConnection(connRead);
                    readConnection.Open();
                }
                if (connWrite != "")
                {
                    writeConnection = new MySqlConnection(connWrite);
                    writeConnection.Open();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Metodo que fara o dispose do objeto
        /// </summary>
        public void Dispose()
        {
            if (writeConnection.State == System.Data.ConnectionState.Open)
            {
                //fechando e liberando recursos
                writeConnection.Close();
                writeConnection.Dispose();
            }

            if (readConnection.State == System.Data.ConnectionState.Open)
            {
                //fechando e liberando recursos
                readConnection.Close();
                readConnection.Dispose();
            }
        }

        #endregion

        public enum BMType { Security, Default }

        #region Metodos

        /// <summary>
        /// Metodo que executa uma query generica no banco
        /// </summary>
        /// <param name="query"></param>
        public void ExecuteExternalQuery(string query)
        {
            try
            {
                using (BaseDAO baseDAO = new BaseDAO(this.readConnection, this.writeConnection))
                {
                    baseDAO.ExecuteExternalQuery(query);
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
