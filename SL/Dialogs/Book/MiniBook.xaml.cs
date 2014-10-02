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

namespace Traderdata.Client.TerminalWEB.Dialogs.Book
{
    public partial class MiniBook : UserControl
    {
        ObservableCollection<BookDTO> Book = new ObservableCollection<BookDTO>();
        string Ativo = "";

        public MiniBook(string ativo)
        {
            InitializeComponent();

            //setando o ativo local
            this.Ativo = ativo;

            //Criando ofertas
            Book.Add(new BookDTO(ativo, "Agora", 100, 5, "ATIVA", 100, 5));
            Book.Add(new BookDTO(ativo, "Agora", 100, 4, "ATIVA", 100, 6));
            Book.Add(new BookDTO(ativo, "Agora", 100, 3, "ATIVA", 100, 7));
            Book.Add(new BookDTO(ativo, "Agora", 100, 2, "ATIVA", 100, 8));
            Book.Add(new BookDTO(ativo, "Agora", 100, 1, "ATIVA", 100, 9));

            //associando ao evento de atualização de book
            RealTimeDAO.BookReceived += RealTimeDAO_BookReceived;
            RealTimeDAO.StartBookSubscription(ativo);
            
            //gridBook.CellFactory = new Cellf();
            gridBook.ItemsSource = Book;
            
        }

        void RealTimeDAO_BookReceived(object Result)
        {
            BookDTO bookTemp = (BookDTO)Result;
            if (bookTemp.Ativo == this.Ativo)
            {
                //popular observable collection
            }
        }
    }
}
