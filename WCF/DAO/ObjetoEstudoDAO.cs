using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;
using MySql.Data.MySqlClient;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class ObjetoEstudoDAO:BaseDAO
    {
        #region Construtor

        public ObjetoEstudoDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Metodos Read

        /// <summary>
        /// Metodo que retorna a lista de objetos de um grafico
        /// </summary>
        /// <param name="graficoId"></param>
        /// <returns></returns>
        public List<ObjetoEstudoDTO> RetornaObjetosPorGraficoId(int graficoId)
        {
            StringBuilder hql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Montando a query
                hql.Append(" SELECT * FROM OBJETO_ESTUDO O");
                hql.Append(" WHERE O.GRAF_CD_GRAFICO = " + graficoId);

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), readConnection))
                {
                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<ObjetoEstudoDTO> listaAux = new List<ObjetoEstudoDTO>();
                        while (reader.Read())
                        {
                            ObjetoEstudoDTO objetoEstudo = new ObjetoEstudoDTO();
                            objetoEstudo.CorObjeto = reader.GetString("OBES_NM_COR");
                            objetoEstudo.Espessura = reader.GetInt32("OBES_NR_ESPESSURA");
                            objetoEstudo.Id = reader.GetInt32("OBES_CD_OBJETO_ESTUDO");
                            objetoEstudo.IndexPainel = reader.GetInt32("OBES_NR_INDEXPAINEL");
                            objetoEstudo.InfinitaADireita = reader.GetBoolean("OBES_IN_INFINITA");
                            objetoEstudo.Magnetica = reader.GetBoolean("OBES_IN_MAGNETICA");
                            objetoEstudo.Parametros = reader.GetString("OBES_NM_PARAMETRO");
                            objetoEstudo.RecordFinal = reader.GetInt32("OBES_NR_RECORDFINAL");
                            objetoEstudo.RecordInicial = reader.GetInt32("OBES_NR_RECORDINICIAL");
                            objetoEstudo.TamanhoTexto = reader.GetInt32("OBES_NR_TAMANHOTEXTO");
                            objetoEstudo.Texto = reader.GetString("OBES_NM_TEXTO");
                            objetoEstudo.TipoLinha = reader.GetInt32("OBES_IN_TIPO_LINHA");
                            objetoEstudo.TipoObjeto = reader.GetInt32("OBES_IN_TIPOOBJETO");
                            objetoEstudo.ValorErrorChannel = reader.GetDecimal("OBES_VL_ERRORCHANNEL");
                            objetoEstudo.ValorFinal = reader.GetDouble("OBES_VL_FINAL");
                            objetoEstudo.ValorInicial = reader.GetDouble("OBES_VL_INICIAL");
                            
                            //adicionando a lista
                            listaAux.Add(objetoEstudo);
                        }

                        return listaAux;
                    }
                    else
                        return new List<ObjetoEstudoDTO>();
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
        /// Metodo que exclui indicador de acordo com o id
        /// </summary>
        /// <param name="id"></param>
        public void ExcluiObjetosDeAcordoComLayoutId(int layoutId)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                hql = new StringBuilder();
                hql.Append(" DELETE FROM ");
                hql.Append(" OBJETO_ESTUDO ");
                hql.Append(" WHERE LAYO_CD_LAYOUT = " + layoutId);

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    command.ExecuteNonQuery();
                }
                
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que exclui indicador de acordo com o id
        /// </summary>
        /// <param name="id"></param>
        public void ExcluiObjetoEstudoPorId(int id)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                hql = new StringBuilder();
                hql.Append(" DELETE FROM ");
                hql.Append(" OBJETO_ESTUDO ");
                hql.Append(" WHERE OBES_CD_OBJETO_ESTUDO = " + id);

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    command.ExecuteNonQuery();
                }
                                
            }
            catch (Exception exc)
            {                
                throw exc;
            }
        }

        /// <summary>
        /// MEtodo que salva um objeto de estudo
        /// </summary>
        /// <param name="objetoEstudoObj"></param>
        public void SalvarObjetoEstudo(ObjetoEstudoDTO objetoEstudoObj)
        {
            StringBuilder hql = new StringBuilder();
            try
            {
                //Salvando os graficos
                hql = new StringBuilder();
                hql.Append(" INSERT INTO OBJETO_ESTUDO (OBES_NM_COR, OBES_NR_ESPESSURA, OBES_IN_TIPO_LINHA, OBES_IN_MAGNETICA, OBES_IN_INFINITA, ");
                hql.Append(" OBES_VL_ERRORCHANNEL, OBES_NM_TEXTO, OBES_NR_TAMANHOTEXTO, OBES_NR_INDEXPAINEL, ");
                hql.Append(" OBES_NR_RECORDINICIAL, OBES_NR_RECORDFINAL, OBES_VL_INICIAL, OBES_VL_FINAL, OBES_IN_TIPOOBJETO, ");
                hql.Append(" OBES_NM_PARAMETRO, LAYO_CD_LAYOUT) VALUES (");
                hql.Append(" ?cor, ?espessura, ?tipoLinha, ?magnetica, ");
                hql.Append(" ?infinita, ?errorChannel, ?texto, ?tamanhoTexto, ");
                hql.Append(" ?indexPainel, ?recordInicial, ?recordFinal, ");
                hql.Append(" ?valorInicial, ?valorFinal, ?tipoObjeto, ?parametros, ?layout)");

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //Setando os parametros
                    command.Parameters.AddWithValue("?cor", objetoEstudoObj.CorObjeto);
                    command.Parameters.AddWithValue("?espessura", objetoEstudoObj.Espessura);
                    command.Parameters.AddWithValue("?tipoLinha", objetoEstudoObj.TipoLinha);
                    command.Parameters.AddWithValue("?magnetica", objetoEstudoObj.Magnetica);
                    command.Parameters.AddWithValue("?infinita", objetoEstudoObj.InfinitaADireita);
                    command.Parameters.AddWithValue("?errorChannel", objetoEstudoObj.ValorErrorChannel);
                    command.Parameters.AddWithValue("?texto", objetoEstudoObj.Texto);
                    command.Parameters.AddWithValue("?tamanhoTexto", objetoEstudoObj.TamanhoTexto);
                    command.Parameters.AddWithValue("?indexPainel",objetoEstudoObj.IndexPainel);
                    command.Parameters.AddWithValue("?recordInicial", objetoEstudoObj.RecordInicial);
                    command.Parameters.AddWithValue("?recordFinal", objetoEstudoObj.RecordFinal);
                    command.Parameters.AddWithValue("?parametros", objetoEstudoObj.Parametros);
                    command.Parameters.AddWithValue("?valorInicial",objetoEstudoObj.ValorInicial);
                    command.Parameters.AddWithValue("?valorFinal", objetoEstudoObj.ValorFinal);
                    command.Parameters.AddWithValue("?tipoObjeto", objetoEstudoObj.TipoObjeto);
                    command.Parameters.AddWithValue("?layout", objetoEstudoObj.LayoutId);
                    
                    //executando
                    command.ExecuteNonQuery();
                    objetoEstudoObj.Id = Convert.ToInt32(command.LastInsertedId);
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
