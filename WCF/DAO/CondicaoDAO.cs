using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class CondicaoDAO:BaseDAO
    {
        #region Construtor

        public CondicaoDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Metodos Read

        /// <summary>
        /// Metodo que retorna todas as condicoes
        /// </summary>
        /// <returns></returns>
        public List<CondicaoDTO> ReturnAll()
        {

            StringBuilder hql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Montando a query
                hql.Append(" SELECT * FROM CONDICAO ");

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), readConnection))
                {
                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<CondicaoDTO> listaAux = new List<CondicaoDTO>();

                        while (reader.Read())
                        {
                            CondicaoDTO condicaoDTO = new CondicaoDTO();

                            condicaoDTO.Id = reader.GetInt32("COND_CD_CONDICAO");
                            condicaoDTO.Comando = reader.GetString("COND_NM_COMANDO");
                            condicaoDTO.Nome = reader.GetString("COND_NM_CONDICAO");

                            listaAux.Add(condicaoDTO);
                        }

                        return listaAux;
                    }
                    else
                        return new List<CondicaoDTO>();
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

        #region Metodos Write
        #endregion
    }
}
