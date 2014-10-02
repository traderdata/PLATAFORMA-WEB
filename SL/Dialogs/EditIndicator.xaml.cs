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
using ModulusFE;
using Traderdata.Client.TerminalWEB.DTO;

namespace Traderdata.Client.TerminalWEB.Dialogs
{
    public partial class EditIndicator : ChildWindow
    {
        #region Variaveis

        /// <summary>
        /// Lista de propriedades do´indicador
        /// </summary>
        public List<IndicatorPropertyDTO> listaPropriedades = new List<IndicatorPropertyDTO>();

        #endregion


        #region Construtor

        public EditIndicator(IndicatorInfoDTO indicador, List<Series> listaSeries, List<string> listaAtivos, int numeroBarras)            
        {
            InitializeComponent();

            #region Criando os itens a serem editados

            //limpando o canvas de parametros
            stackParametros.Children.Clear();
            
            //montar o canvas de acordo com as propriedades
            foreach (IndicatorPropertyDTO obj in indicador.Propriedades)
            {
                StackPanel painelInterno = new StackPanel();
                painelInterno.Orientation = Orientation.Horizontal;
                painelInterno.Margin = new Thickness(0, 5, 0, 0);
                painelInterno.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                painelInterno.Tag = obj.TipoDoCampo;
                TextBlock textBlock = new TextBlock();
                textBlock.Text = obj.Label;
                textBlock.Width = 100;
                textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                painelInterno.Children.Add(textBlock);

                switch (obj.TipoDoCampo)
                {
                    case TipoField.Header:
                        textBlock.Margin = new Thickness(10, 0, 0, 0);
                        textBlock.FontWeight = FontWeights.Bold;
                        textBlock.FontSize = 11;
                        break;
                    case TipoField.NumericUpDownInteger:
                        textBlock.Margin = new Thickness(30, 0, 0, 0);
                        NumericUpDown numberField = new NumericUpDown();
                        numberField.Width = 60;
                        numberField.DecimalPlaces = 0;
                        numberField.Tag = obj;
                        numberField.Maximum = numeroBarras;
                        numberField.Value = Convert.ToDouble(obj.Value);
                        painelInterno.Children.Add(numberField);
                        break;
                    case TipoField.Double:
                        textBlock.Margin = new Thickness(30, 0, 0, 0);
                        NumericUpDown numberDouble = new NumericUpDown();
                        numberDouble.DecimalPlaces = 2;
                        numberDouble.Maximum = 10000;
                        numberDouble.Width = 60;
                        numberDouble.Tag = obj;
                        numberDouble.Value = Convert.ToDouble(obj.Value);
                        painelInterno.Children.Add(numberDouble);
                        break;
                    case TipoField.Media:
                        textBlock.Margin = new Thickness(30, 0, 0, 0);
                        ComboBox cmbTipoMedia = new ComboBox();
                        cmbTipoMedia.Margin = new Thickness(00, 0, 0, 10);
                        cmbTipoMedia.Items.Add("Simples");
                        cmbTipoMedia.Items.Add("Exponencial");
                        cmbTipoMedia.Items.Add("TimeSeries");
                        cmbTipoMedia.Items.Add("Triangular");
                        cmbTipoMedia.Items.Add("Variável");
                        cmbTipoMedia.Items.Add("VYDIA");
                        cmbTipoMedia.Items.Add("Ponderada");
                        cmbTipoMedia.Width = 100;
                        cmbTipoMedia.Height = 20;
                        cmbTipoMedia.Tag = obj;
                        switch((int)obj.Value)
                        {
                            case 0:
                                cmbTipoMedia.SelectedIndex = 0;
                                break;
                            case 1:
                                cmbTipoMedia.SelectedIndex = 1;
                                break;
                            case 2:
                                cmbTipoMedia.SelectedIndex = 2;
                                break;
                            case 3:
                                cmbTipoMedia.SelectedIndex = 3;
                                break;
                            case 4:
                                cmbTipoMedia.SelectedIndex = 4;
                                break;
                            case 5:
                                cmbTipoMedia.SelectedIndex = 5;
                                break;
                            case 7:
                                cmbTipoMedia.SelectedIndex = 6;
                                break;
                        }
                        painelInterno.Children.Add(cmbTipoMedia);
                        break;
                    case TipoField.Serie:
                        textBlock.Margin = new Thickness(30, 0, 0, 0);
                        ComboBox cmbSerie = new ComboBox();
                        foreach (Series serie in listaSeries)
                        {
                            cmbSerie.Items.Add(serie.FullName.ToUpper().Trim());
                        }

                        cmbSerie.Margin = new Thickness(00, 0, 0, 0);
                        cmbSerie.Width = 130;
                        cmbSerie.Height = 20;
                        cmbSerie.Tag = obj;
                        cmbSerie.SelectedItem = obj.Value.ToString().ToUpper().Trim();
                        painelInterno.Children.Add(cmbSerie);
                        break;
                    case TipoField.SymbolList:
                        textBlock.Margin = new Thickness(30, 0, 0, 0);
                        ComboBox cmbSymbolList = new ComboBox();
                        foreach (string ativo in listaAtivos)
                        {
                            cmbSymbolList.Items.Add(ativo);
                        }

                        cmbSymbolList.Margin = new Thickness(00, 0, 0, 0);
                        cmbSymbolList.Width = 130;
                        cmbSymbolList.Height = 20;
                        cmbSymbolList.Tag = obj;
                        //cmbSymbolList.SelectedItem = defaultAtivo;
                        painelInterno.Children.Add(cmbSymbolList);
                        break;

                }

                if ((obj.TipoDoCampo == TipoField.SymbolList))
                {
                    painelInterno.Visibility = System.Windows.Visibility.Collapsed;
                }

                stackParametros.Children.Add(painelInterno);


            }
            
            #endregion
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento disparado ao se clicar em Ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //setando as propriedades
            foreach (StackPanel obj in stackParametros.Children)
            {
                switch ((TipoField)obj.Tag)
                {
                    case TipoField.NumericUpDownInteger:
                        foreach (object objInterno in obj.Children)
                        {
                            if (objInterno.GetType().ToString().Contains("NumericUpDown"))
                            {
                                if (((NumericUpDown)objInterno).Tag != null)
                                {
                                    if (((NumericUpDown)objInterno).Tag.GetType().ToString().Contains("IndicatorPropertyDTO"))
                                    {
                                        ((IndicatorPropertyDTO)((NumericUpDown)objInterno).Tag).Value = ((NumericUpDown)objInterno).Value;
                                        listaPropriedades.Add((IndicatorPropertyDTO)((NumericUpDown)objInterno).Tag);
                                    }
                                }
                            }

                        }
                        break;
                    case TipoField.Double:
                        foreach (object objInterno in obj.Children)
                        {
                            if (objInterno.GetType().ToString().Contains("NumericUpDown"))
                            {
                                if (((NumericUpDown)objInterno).Tag != null)
                                {
                                    if (((NumericUpDown)objInterno).Tag.GetType().ToString().Contains("IndicatorPropertyDTO"))
                                    {
                                        ((IndicatorPropertyDTO)((NumericUpDown)objInterno).Tag).Value = ((NumericUpDown)objInterno).Value;
                                        listaPropriedades.Add((IndicatorPropertyDTO)((NumericUpDown)objInterno).Tag);
                                    }
                                }
                            }

                        }
                        break;
                    case TipoField.Serie:
                        foreach (object objInterno in obj.Children)
                        {
                            if (objInterno.GetType().ToString().Contains("ComboBox"))
                            {
                                if (((ComboBox)objInterno).Tag.GetType().ToString().Contains("IndicatorPropertyDTO"))
                                {
                                    ((IndicatorPropertyDTO)((ComboBox)objInterno).Tag).Value = ((ComboBox)objInterno).SelectedItem;
                                    listaPropriedades.Add((IndicatorPropertyDTO)((ComboBox)objInterno).Tag);
                                }
                            }

                        }
                        break;
                    case TipoField.Media:
                        foreach (object objInterno in obj.Children)
                        {
                            if (objInterno.GetType().ToString().Contains("ComboBox"))
                            {
                                if (((ComboBox)objInterno).Tag.GetType().ToString().Contains("IndicatorPropertyDTO"))
                                {
                                    switch (Convert.ToString(((ComboBox)objInterno).SelectedItem))
                                    {
                                        case "Simples":
                                            ((IndicatorPropertyDTO)((ComboBox)objInterno).Tag).Value = 0;
                                            break;
                                        case "Exponencial":
                                            ((IndicatorPropertyDTO)((ComboBox)objInterno).Tag).Value = 1;
                                            break;
                                        case "TimeSeries":
                                            ((IndicatorPropertyDTO)((ComboBox)objInterno).Tag).Value = 2;
                                            break;
                                        case "Triangular":
                                            ((IndicatorPropertyDTO)((ComboBox)objInterno).Tag).Value = 3;
                                            break;
                                        case "Variável":
                                            ((IndicatorPropertyDTO)((ComboBox)objInterno).Tag).Value = 4;
                                            break;
                                        case "VYDIA":
                                            ((IndicatorPropertyDTO)((ComboBox)objInterno).Tag).Value = 5;
                                            break;
                                        case "Ponderada":
                                            ((IndicatorPropertyDTO)((ComboBox)objInterno).Tag).Value = 7;
                                            break;
                                    }
                                    
                                    listaPropriedades.Add((IndicatorPropertyDTO)((ComboBox)objInterno).Tag);
                                }
                            }

                        }
                        break;
                    case TipoField.SymbolList:
                        foreach (object objInterno in obj.Children)
                        {
                            if (objInterno.GetType().ToString().Contains("ComboBox"))
                            {
                                if (((ComboBox)objInterno).Tag.GetType().ToString().Contains("IndicatorPropertyDTO"))
                                {
                                    ((IndicatorPropertyDTO)((ComboBox)objInterno).Tag).Value = ((ComboBox)objInterno).SelectedItem;
                                    listaPropriedades.Add((IndicatorPropertyDTO)((ComboBox)objInterno).Tag);
                                }
                            }

                        }
                        break;

                }


            }

            this.DialogResult = true;
        }

        /// <summary>
        /// Evento disparado ao se clicar em Cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #endregion
    }
}

