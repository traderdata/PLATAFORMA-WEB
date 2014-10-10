using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class IndicadorDAO:BaseDAO
    {
        #region Construtor

        public IndicadorDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Read

        #endregion

        #region Write

        /// <summary>
        /// Metodo que salva o indicador
        /// </summary>
        /// <param name="indicadorObj"></param>
        public void SalvarIndicador(IndicadorDTO indicadorObj)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //Salvando os graficos
                hql = new StringBuilder();
                hql.Append(" INSERT INTO INDICADOR (INDI_NM_COR, INDI_NM_COR_FILHA1, INDI_NM_COR_FILHA2, INDI_IN_TIPO_LINHA,");
                hql.Append(" INDI_IN_TIPO_LINHA_FILHA1, INDI_IN_TIPO_LINHA_FILHA2, INDI_NR_ESPESSURA, INDI_NR_ESPESSURA_FILHA1, ");
                hql.Append(" INDI_NR_ESPESSURA_FILHA2, INDI_NM_PARAMETRO, INDI_IN_TIPO_INDICADOR, INDI_CD_INDEX_PAINEL, ");
                hql.Append(" LAYO_CD_LAYOUT) VALUES (");
                hql.Append(" ?cor, ?corFilha1, ?corFilha2, ?tipoLinha, ?tipoLinhaFilha1, ");
                hql.Append(" ?tipoLinhaFilha2, ?espessura, ?espessuraFilha1, ?espessuraFilha2, ");
                hql.Append(" ?parametro, ?tipo, ?indexPainel, ?layout)");

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //Setando os parametros
                    command.Parameters.AddWithValue("?cor", indicadorObj.Cor);
                    command.Parameters.AddWithValue("?corFilha1", indicadorObj.CorFilha1);
                    command.Parameters.AddWithValue("?corFilha2", indicadorObj.CorFilha2);

                    command.Parameters.AddWithValue("?tipoLinha", indicadorObj.TipoLinha);
                    command.Parameters.AddWithValue("?tipoLinhaFilha1", indicadorObj.TipoLinhaFilha1);
                    command.Parameters.AddWithValue("?tipoLinhaFilha2", indicadorObj.TipoLinhaFilha2);

                    command.Parameters.AddWithValue("?espessura", indicadorObj.Espessura);
                    command.Parameters.AddWithValue("?espessuraFilha1", indicadorObj.EspessuraFilha1);
                    command.Parameters.AddWithValue("?espessuraFilha2", indicadorObj.EspessuraFilha2);

                    command.Parameters.AddWithValue("?parametro", indicadorObj.Parametros);
                    command.Parameters.AddWithValue("?tipo", indicadorObj.TipoIndicador);
                    command.Parameters.AddWithValue("?indexPainel", indicadorObj.IndexPainel);

                    //command.Parameters.AddWithValue("?valorAltura", indicadorObj.AlturaPainel);
                    //command.Parameters.AddWithValue("?statusPainel", indicadorObj.StatusPainel);
                    //command.Parameters.AddWithValue("?painelIndicador", indicadorObj.PainelIndicador);

                    //command.Parameters.AddWithValue("?painelPreco", indicadorObj.PainelPreco);
                    //command.Parameters.AddWithValue("?painelVolume", indicadorObj.PainelVolume);
                    //command.Parameters.AddWithValue("?painelAbaixoPreco", indicadorObj.PainelAbaixoPreco);

                    if (indicadorObj.LayoutId.HasValue)
                        command.Parameters.AddWithValue("?layout", indicadorObj.LayoutId.Value);
                    else
                        command.Parameters.AddWithValue("?layout", null);


                    //executando
                    command.ExecuteNonQuery();
                    indicadorObj.Id = Convert.ToInt32(command.LastInsertedId);
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
