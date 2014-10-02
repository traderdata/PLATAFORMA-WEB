using System;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.Silverlight.FlexGrid;

namespace Traderdata.Client.TerminalWEB.Dialogs.Portfolio
{
    public class PortfolioCellFactory: CellFactory
    {
        static Thickness _thicknessEmpty = new Thickness(0);
        // bind cell to ticker
        public override void CreateCellContent(C1FlexGrid grid, Border bdr, CellRange range)
        {            
            // create binding for this cell
            var r = grid.Rows[range.Row];
            var c = grid.Columns[range.Column];
            var pi = c.PropertyInfo;
            if (r.DataItem is PortfolioDTO &&
               (pi.Name == "Ultimo" || pi.Name == "Variacao"))
            {
                StockTicker ticker = new StockTicker();
                bdr.Child = ticker;
                bdr.Padding = _thicknessEmpty;
                ticker.Value = (double)pi.GetValue(r.DataItem, null);
                ((PortfolioDTO)r.DataItem).PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == pi.Name)
                    {
                        ticker.Value = (double)pi.GetValue(sender, null);
                    }
                };
                
            }
            else if (r.DataItem is PortfolioDTO && pi.Name == "Ativo")
            {
                StockTickerSymbol ticker = new StockTickerSymbol();
                bdr.Child = ticker;
                bdr.Padding = _thicknessEmpty;
                ticker.ValueSymbolName = (string)pi.GetValue(r.DataItem, null);
                if (((PortfolioDTO)r.DataItem).Variacao > 0)
                    ticker.SetColorGreen();
                else if (((PortfolioDTO)r.DataItem).Variacao < 0)
                    ticker.SetColorRed();
                else

                    ticker.SetColorWhite();

                ((PortfolioDTO)r.DataItem).PropertyChanged += (sender, e) =>
                {
                    if (((PortfolioDTO)r.DataItem).Variacao > 0)
                        ticker.SetColorGreen();
                    else if (((PortfolioDTO)r.DataItem).Variacao < 0)
                        ticker.SetColorRed();
                    else

                        ticker.SetColorWhite();
                };
            }
            else if (r.DataItem is PortfolioDTO &&
               (pi.Name == "Volume" || pi.Name == "Maximo" || pi.Name == "Minimo" || pi.Name == "Abertura"))
            {
                StockTickerYellow ticker = null;
                if (pi.Name == "Volume")
                    ticker = new StockTickerYellow("n0");
                else
                    ticker = new StockTickerYellow("n2");

                bdr.Child = ticker;
                bdr.Padding = _thicknessEmpty;
                ticker.Value = (double)pi.GetValue(r.DataItem, null);
                ((PortfolioDTO)r.DataItem).PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == pi.Name)
                    {
                        ticker.Value = (double)pi.GetValue(sender, null);
                    }
                };
            }
            else
            {
                // use default implementation
                base.CreateCellContent(grid, bdr, range);
            }
        }

        // override alignment to make ticker control fill the whole cell
        public override void ApplyCellStyles(C1FlexGrid grid, CellType cellType, CellRange range, Border bdr)
        {
            //var ticker = bdr.Child as StockTicker;
            //if (ticker != null)
            //{
            //    ticker.HorizontalAlignment = HorizontalAlignment.Stretch;
            //    ticker.VerticalAlignment = VerticalAlignment.Stretch;
            //}
        }
    }
}
