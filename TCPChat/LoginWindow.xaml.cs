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
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    /// 
    
    public partial class LoginWindow : Window, INotifyPropertyChanged
    {

        private Window mainWindow;
        public bool registration { get; set; }
        UserObj user;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public UserObj User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged();
            }
        }

        public LoginWindow(Window mainWin)
        {
            mainWindow = mainWin;
            InitializeComponent();
            registration = false;
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            this.DialogResult = true;
        }

        public string password
        {
            get { return Password.Password; }
        }

        public string login
        {
            get { return Login.Text; }
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow regWin = new RegistrationWindow();
            if (regWin.ShowDialog() == true)
            {
                registration = true;
                User = regWin.User;
                this.DialogResult = true;
            }
            

        }
    }
}
