using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class ObjetoEstudoDAO:BaseDAO
    {
        #region Construtor

        public ObjetoEstudoDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Read

        #endregion

        #region Write

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
                    command.Parameters.AddWithValue("?indexPainel", objetoEstudoObj.IndexPainel);
                    command.Parameters.AddWithValue("?recordInicial", objetoEstudoObj.RecordInicial);
                    command.Parameters.AddWithValue("?recordFinal", objetoEstudoObj.RecordFinal);
                    command.Parameters.AddWithValue("?parametros", objetoEstudoObj.Parametros);
                    command.Parameters.AddWithValue("?valorInicial", objetoEstudoObj.ValorInicial);
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
