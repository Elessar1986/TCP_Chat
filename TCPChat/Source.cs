using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TCP_Chat_Library;

namespace TCPChat
{
    class Source : INotifyPropertyChanged
    {
        UserObj selectedUser;
        public UserObj SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged();
            }
        }

        List<UserObj> usersOnline;
        public List<UserObj> UsersOnline { get
            {
                return usersOnline;
            }
            set
            {
                usersOnline = value;
                OnPropertyChanged();
            }
        }
        
        

        public Source()
        {
            mainText = "";
            message = "Your message...";
            UsersOnline = new List<UserObj>();

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
