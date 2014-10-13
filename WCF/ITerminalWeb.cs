using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;
using System.IO;
using Traderdata.Server.Core.DTO;


namespace Traderdata.Server.App.TerminalWeb
{
    [ServiceContract]
    public interface ITerminalWeb
    {
        
        #region Usuario

        [OperationContract]
        UserDTO LoginOrInsertUser(string login);

        #endregion

        #region Templates

        [OperationContract]
        List<TemplateDTO> GetTemplatesByUser(int user);

        [OperationContract]
        TemplateDTO SaveTemplate(TemplateDTO template);

        [OperationContract]
        void DeleteTemplate(TemplateDTO template);

        #endregion

        #region Grafico

        [OperationContract]
        GraficoDTO GetGrafico(string ativo, int userId, int periodicidade);

        [OperationContract]
        GraficoDTO SaveGrafico(GraficoDTO grafico);

        #endregion

        #region Fake

        

        [OperationContract]
        ObjetoEstudoDTO GetObjetoFake();

        [OperationContract]
        IndicadorDTO GetIndicatorFake();

        [OperationContract]
        LayoutDTO GetLIndicatorFake();

        #endregion

    }
}

