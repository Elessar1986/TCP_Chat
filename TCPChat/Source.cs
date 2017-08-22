using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TCPChat
{
    class Source : INotifyPropertyChanged
    {
        public Source()
        {
            mainText = "text";
            message = "Your message...";
        }

        string MainText;
        public string mainText
        {
            get { return MainText; }
            set
            {
                MainText = value;
                OnPropertyChanged();
            }
        }

        string Message;
        public string message
        {
            get { return Message; }
            set
            {
                Message = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
