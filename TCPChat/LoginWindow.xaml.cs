using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TCPChat
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    /// 
    
    public partial class LoginWindow : Window
    {

        private Window mainWindow;
        public LoginWindow(Window mainWin)
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show($"Pas: {Password.Password} \nLog: {Login.Text}");
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
    }
}
