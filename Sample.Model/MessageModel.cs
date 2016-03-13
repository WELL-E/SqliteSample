
using System.ComponentModel;

namespace Sample.Model
{
    public class MessageModel : INotifyPropertyChanged
    {
        public string MsgId { get; set; }

        public string Title { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public string RecvTime { get; set; }

        public string Body { get; set; }

        private bool _isRead;

        public bool IsRead
        {
            get { return _isRead; }
            set
            {
                _isRead = value;
                //OnPropertyChanged("IsRead");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) return;

            var handler = PropertyChanged;
            handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
