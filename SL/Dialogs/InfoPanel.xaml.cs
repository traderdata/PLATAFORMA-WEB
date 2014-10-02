using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ModulusFE.SL;
using Traderdata.Client.TerminalWEB.DTO;

namespace Traderdata.Client.TerminalWEB.Dialogs
{
    public partial class InfoPanel : UserControl
    {
        public InfoPanel()
        {
            InitializeComponent();
        }

        public void SetInfo(List<InfoPanelItemDTO> itens, DateTime data)
        {
            //setando as datas
            if (data.Hour == 0)
                txtDate.Text = data.ToString("dd-MM-yyyy");
            else
                txtDate.Text = data.ToString("dd-MM / HH:mm");


            stackItens.Children.Clear();
            foreach (InfoPanelItemDTO item in itens)
            {
                if (!item.Separator)
                {
                    StackPanel subItem = new StackPanel();
                    subItem.Height = 15;
                    Canvas canvasHorizontal = new Canvas();
                    
                    TextBlock textTitulo = new TextBlock();
                    textTitulo.Text = item.Titulo;
                    textTitulo.Foreground = Brushes.White;
                    textTitulo.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    canvasHorizontal.Children.Add(textTitulo);

                    TextBlock textValor = new TextBlock();
                    textValor.Text = item.Value;
                    textValor.Foreground = Brushes.White;
                    textValor.Width = 60;
                    textValor.TextAlignment = TextAlignment.Right;
                    textValor.Margin = new Thickness(70, 0, 0, 0);
                    canvasHorizontal.Children.Add(textValor);


                    //adicionando ao stackitens
                    subItem.Children.Add(canvasHorizontal);
                    stackItens.Children.Add(subItem);
                }
                else
                {
                    Line line = new Line();
                    line.Stroke = Brushes.Silver;
                    line.StrokeThickness = 2;
                    line.X1 = 0;
                    line.X2 = 150;
                    line.Y1 = 0;
                    line.Y2 = 0;
                    line.Margin = new Thickness(0, 10, 0, 5);
                    stackItens.Children.Add(line);
                }
            }
            
        }

        public void EnterBorder()
        {
            border.Opacity = 0.2;
        }

        public void ExitBorder()
        {
            border.Opacity = 1;
        }
    }
}
