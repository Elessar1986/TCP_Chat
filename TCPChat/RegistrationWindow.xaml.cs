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
using TCP_Chat_Library;

namespace TCPChat
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        UserObj user;
        public UserObj User { get; set; }


        public RegistrationWindow()
        {
            InitializeComponent();
            this.DataContext = User;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void RegIn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
