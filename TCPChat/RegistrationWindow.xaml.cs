using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TCP_Chat_Library;

namespace TCPChat
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window, INotifyPropertyChanged
    {
        UserObj user;
        public UserObj User {
            get { return user; }
            set { user = value;
                OnPropertyChanged();
            }
        }


        public RegistrationWindow()
        {
            InitializeComponent();
            User = new UserObj();
            User.Age = 21;
            User.Name = string.Empty;
            User.Login = string.Empty;
            User.Password = string.Empty;
            this.DataContext = User;
            
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void RegIn_Click(object sender, RoutedEventArgs e)
        {
            if (Sex.SelectedIndex == 0) User.Sex = "male";
            else User.Sex = "female";
            if ((User.Name != string.Empty)
                && (User.Login != string.Empty)
                && (User.Password != string.Empty))
                this.DialogResult = true;
            else MessageBox.Show("Вы не заполнили все поля", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Stop);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
