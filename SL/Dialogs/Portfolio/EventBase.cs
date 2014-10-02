using System.ComponentModel;
namespace Traderdata.Client.TerminalWEB.Dialogs.Portfolio
{
    public  abstract class EventBase : INotifyPropertyChanged 
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void PropertyChangedHandler(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
