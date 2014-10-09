using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class PortfolioDAO : BaseDAO
    {
        #region Construtor

        public PortfolioDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Metodos Read

        /// <summary>
        /// Metodo que retorna a lista de portfolios do cliente mais os padroes
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<PortfolioDTO> RetornaTodosPorClienteMaisPadroes(UsuarioDTO user)
        {
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                string query =
                    @"
                 SELECT * FROM PORTFOLIO WHERE (USUA_CD_USUARIO = ?usuario OR PORT_IN_PUBLICO = 1)";
                    

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(query, readConnection))
                {
                    command.Parameters.AddWithValue("?usuario", user.Id);

                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<PortfolioDTO> listaPortfolio = new List<PortfolioDTO>();
                        while (reader.Read())
                        {
                            PortfolioDTO portfolio = new PortfolioDTO();
                            portfolio.Ativos = reader.GetString("PORT_NM_ATIVOS");
                            portfolio.Nome = reader.GetString("PORT_NM_PORTFOLIO");
                            portfolio.Colunas = reader.GetString("PORT_NM_COLUNAS");
                            portfolio.Id = reader.GetInt32("PORT_CD_PORTFOLIO");
                            portfolio.Publico = reader.GetBoolean("PORT_IN_PUBLICO");
                            portfolio.TamanhoColunas = reader.GetString("PORT_NM_TAMANHO_COLUNAS");
                            if (!reader.IsDBNull(reader.GetOrdinal("USUA_CD_USUARIO")))
                                portfolio.UserId = reader.GetInt32("USUA_CD_USUARIO");

                            listaPortfolio.Add(portfolio);
                        }

                        return listaPortfolio;
                    }
                    else
                        return new List<PortfolioDTO>();
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

        #region Metodos write
        #endregion
    }
}
