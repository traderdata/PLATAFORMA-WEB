using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Traderdata.Client.TerminalWEB.Dialogs.Portfolio
{
    public class PortfolioDTO:EventBase
    {
        private string ativo;
        private double abertura;
        private double maximo;
        private double minimo;
        private double ultimo;
        private double variacao;
        private double volume;
        private string hora;

        public string Ativo
        {
            get
            {
                return ativo;
            }
            set
            {
                ativo = value;
                PropertyChangedHandler("Ativo");
            }
        }

        public double Abertura
        {
            get
            {
                return abertura;
            }
            set
            {
                abertura = value;
                PropertyChangedHandler("Abertura");
            }
        }

        public double Maximo
        {
            get
            {
                return maximo;
            }
            set
            {
                maximo = value;
                PropertyChangedHandler("Maximo");
            }
        }

        public double Minimo
        {
            get
            {
                return minimo;
            }
            set
            {
                minimo = value;
                PropertyChangedHandler("Minimo");
            }
        }

        public double Ultimo
        {
            get
            {
                return ultimo;
            }
            set
            {
                ultimo = value;
                PropertyChangedHandler("Ultimo");
            }
        }

        public double Variacao
        {
            get
            {
                return variacao;
            }
            set
            {
                variacao = value;
                PropertyChangedHandler("Variacao");
            }
        }

        public double Volume
        {
            get
            {
                return volume;
            }
            set
            {
                volume = value;
                PropertyChangedHandler("Volume");
            }
        }

        public string Hora
        {
            get
            {
                return hora;
            }
            set
            {
                hora = value;
                PropertyChangedHandler("Hora");
            }
        }
    }
}
