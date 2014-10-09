using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Traderdata.Server.App.TerminalWeb.BusinessManager
{
    public class BaseBM : IDisposable
    {
        #region Variaveis privadas

        protected MySqlConnection readConnection = new MySqlConnection();
        protected MySqlConnection writeConnection = new MySqlConnection();
        protected MySqlTransaction transaction;
        protected string nomeServico = "";

        #endregion

        #region Construtor

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="transacionado"></param>
        public BaseBM(bool leitura, bool escrita, string nomeServico)
        {
            try
            {
                if (leitura)
                {
                    readConnection = new MySqlConnection(ConfigurationSettings.AppSettings["CONNECTION-READ"]);
                    readConnection.Open();
                }

                if (escrita)
                {
                    writeConnection = new MySqlConnection(ConfigurationSettings.AppSettings["CONNECTION-SAVE"]);
                    writeConnection.Open();
                    transaction = writeConnection.BeginTransaction();
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
    }
}
