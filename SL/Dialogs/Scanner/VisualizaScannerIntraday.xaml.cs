using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Traderdata.Client.TerminalWEB.DAO;
using Traderdata.Client.TerminalWEB.DTO;

namespace Traderdata.Client.TerminalWEB.Dialogs.Scanner
{
    public partial class VisualizaScannerIntraday : UserControl
    {

        private ObservableCollection<ScannerDTO> _scannerDataList = new ObservableCollection<ScannerDTO>();



        public VisualizaScannerIntraday()
        {
            InitializeComponent();

            _gridAtivos.ItemsSource = _scannerDataList;
            RealTimeDAO.ScannerReceived += RealTimeDAO_ScannerReceived;
            RealTimeDAO.ConnectScannerIntraday();
        }

        void RealTimeDAO_ScannerReceived(object Result)
        {
            if (Result.GetType().ToString().Contains("ScannerDTO"))
            {
                if (StaticData.FerramentasAuxiliaresVisiveis)
                    _scannerDataList.Insert(0, (ScannerDTO)Result);
            }
        }


        /// <summary>
        /// Evento de double click sobre a grid de resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _gridAtivos_DoubleClick_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((MainPage)((BusyIndicator)((Grid)((Grid)((Grid)((ContainerDireita)((Grid)((C1.Silverlight.C1TabControl)((C1.Silverlight.C1TabItem)this.Parent).Parent).Parent).Parent)
                .Parent).Parent).Parent).Parent).Parent).NovoGraficoAtalho(((ScannerDTO)_gridAtivos.SelectedItem).Ativo);
        }
    }
}
