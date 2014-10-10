using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Traderdata.Server.App.TerminalWeb.DTO;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class LayoutDAO:BaseDAO
    {
        #region Construtor

        public LayoutDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)
        { }

        #endregion

        #region Read

        #endregion

        #region Write

        /// <summary>
        /// Metodo que faz a inserção de layout
        /// </summary>
        /// <param name="layout"></param>
        public void InsertLayout(LayoutDTO layout)
        {
            try
            {
                //salvando o template
                StringBuilder hql = new StringBuilder();

                hql.Append(" INSERT INTO LAYOUT (LAYO_NM_COR_VOLUME, LAYO_NM_COR_FUNDO, LAYO_NM_COR_BORDA_ALTA,");
                hql.Append("LAYO_NM_COR_BORDA_BAIXA, LAYO_NM_COR_CANDLE_ALTA, LAYO_NM_COR_CANDLE_BAIXA, LAYO_IN_POSICAO_ESCALA,");
                hql.Append("LAYO_IN_TIPO_ESCALA, LAYO_NR_PRECISAO_ESCALA, LAYO_IN_GRADE_HORIZONTAL, LAYO_IN_GRADE_VERTICAL, LAYO_IN_PAINEL,");
                hql.Append("LAYO_VL_ESPACO_DIREITA, LAYO_IN_ESTILO_PRECO, LAYO_VL_PRECO_PARAM1, LAYO_VL_PRECO_PARAM2, LAYO_IN_ESTILO_BARRA,");
                hql.Append("LAYO_IN_DARVA_BOX, LAYO_IN_VOLUME, GRAF_CD_GRAFICO, TEMP_CD_TEMPLATE, LAYO_VL_ESPESSURA_VOLUME, ");
                hql.Append("LAYO_NM_COR_GRID, LAYO_NM_COR_ESCALA, LAYO_IN_USAR_COR_VOLUME,LAYO_VL_ESPACO_ESQUERDA, LAYO_NR_INDEX) ");

                hql.Append(" VALUES ( ?cor_volume, ?cor_fundo, ?cor_borda_alta, ?cor_borda_baixa, ?cor_candle_alta, ?cor_candle_baixa,");
                hql.Append("?in_posicao_escala, ?tipo_escala, ?precisao_escala, ?grade_horizontal, ?grade_vertical, ?in_painel,");
                hql.Append("?espaco_direita, ?estilo_preco, ?preco_param1, ?preco_param2, ?estilo_barra, ");
                hql.Append("?darva_box, ?in_volume, ?cd_grafico, ?cd_template,?espessuraVolume, ?corgrid, ?corescala, ?usarcorvolume,?espacoesquerda, ?index)");

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //Setando os parametros
                    command.Parameters.AddWithValue("?cor_volume", layout.CorVolume);
                    command.Parameters.AddWithValue("?cor_fundo", layout.CorFundo);
                    command.Parameters.AddWithValue("?cor_borda_alta", layout.CorBordaCandleAlta);
                    command.Parameters.AddWithValue("?cor_borda_baixa", layout.CorBordaCandleBaixa);
                    command.Parameters.AddWithValue("?cor_candle_alta", layout.CorCandleAlta);
                    command.Parameters.AddWithValue("?cor_candle_baixa", layout.CorCandleBaixa);
                    command.Parameters.AddWithValue("?in_posicao_escala", layout.PosicaoEscala);
                    command.Parameters.AddWithValue("?tipo_escala", layout.TipoEscala);
                    command.Parameters.AddWithValue("?precisao_escala", layout.PrecisaoEscala);
                    command.Parameters.AddWithValue("?grade_horizontal", layout.GradeHorizontal);
                    command.Parameters.AddWithValue("?grade_vertical", layout.GradeVertical);
                    command.Parameters.AddWithValue("?in_painel", layout.PainelInfo);
                    command.Parameters.AddWithValue("?espaco_direita", layout.EspacoADireitaDoGrafico);
                    command.Parameters.AddWithValue("?espacoesquerda", layout.EspacoAEsquerdaDoGrafico);
                    command.Parameters.AddWithValue("?estilo_preco", layout.EstiloPreco);
                    command.Parameters.AddWithValue("?preco_param1", layout.EstiloPrecoParam1);
                    command.Parameters.AddWithValue("?preco_param2", layout.EstiloPrecoParam2);
                    command.Parameters.AddWithValue("?estilo_barra", layout.EstiloBarra);
                    command.Parameters.AddWithValue("?darva_box", layout.DarvaBox);
                    command.Parameters.AddWithValue("?in_volume", layout.TipoVolume);
                    if (layout.GraficoId.HasValue)
                        command.Parameters.AddWithValue("?cd_grafico", layout.GraficoId.Value);
                    else
                        command.Parameters.AddWithValue("?cd_grafico", null);
                    if (layout.TemplateId.HasValue)
                        command.Parameters.AddWithValue("?cd_template", layout.TemplateId);
                    else
                        command.Parameters.AddWithValue("?cd_template", null);

                    command.Parameters.AddWithValue("?espessuraVolume", layout.VolumeStrokeThickness);
                    command.Parameters.AddWithValue("?corgrid", layout.CorGrid);
                    command.Parameters.AddWithValue("?corescala", layout.CorEscala);
                    command.Parameters.AddWithValue("?usarcorvolume", layout.UsarCoresAltaBaixaVolume);
                    command.Parameters.AddWithValue("?index", layout.Index);

                    command.ExecuteNonQuery();
                    layout.Id = Convert.ToInt32(command.LastInsertedId);
                }

                //inserindo paineis
                foreach (PainelDTO painel in layout.Paineis)
                {
                    using (PainelDAO painelDAO = new PainelDAO(readConnection, writeConnection))
                    {
                        painel.LayoutId = layout.Id;
                        painelDAO.InsertPainel(painel);
                    }
                }

                //inserir indicadores do layout
                foreach (IndicadorDTO indicador in layout.Indicadores)
                {
                    using (IndicadorDAO indicadorDAO = new IndicadorDAO(readConnection, writeConnection))
                    {
                        indicador.LayoutId = layout.Id;
                        indicadorDAO.SalvarIndicador(indicador);
                    }
                }

                ////inserir os objetos do layout
                //foreach (ObjetoEstudoDTO objeto in layout.Objetos)
                //{
                //    using (ObjetoEstudoDAO objetoDAO = new ObjetoEstudoDAO(readConnection, writeConnection))
                //    {
                //        objeto.LayoutId = layout.Id;
                //        objetoDAO.SalvarObjetoEstudo(objeto);
                //    }
                //}
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        #endregion
    }
}
