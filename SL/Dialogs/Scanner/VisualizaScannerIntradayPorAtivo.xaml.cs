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
    public partial class VisualizaScannerIntradayPorAtivo : UserControl
    {
        private ObservableCollection<ScannerDTO> _scannerDataList = new ObservableCollection<ScannerDTO>();
        private string Ativo = "";

        public VisualizaScannerIntradayPorAtivo(string ativo)
        {
            InitializeComponent();
            this.Ativo = ativo;
            gridAlertas.ItemsSource = _scannerDataList;
            RealTimeDAO.ScannerReceived += RealTimeDAO_ScannerReceived;            
        }

        void RealTimeDAO_ScannerReceived(object Result)
        {
            if (((ScannerDTO)Result).Ativo==Ativo)
                _scannerDataList.Insert(0, (ScannerDTO)Result);
        }
    }
}
