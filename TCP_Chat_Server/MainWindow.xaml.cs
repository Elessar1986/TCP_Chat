using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TCP_Chat_Server
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int port = 8888;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            TcpListener server = null;

            try
            {
                server = new TcpListener(IPAddress.Parse("192.168.0.102"), port);
                server.Start();
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    await  Task.Factory.StartNew( () => Listener(client),TaskCreationOptions.LongRunning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (server != null)
                    server.Stop();
            }
        }

        void Listener(Object o)
        {
            TcpClient client = (TcpClient)o;
            NetworkStream stream = client.GetStream();
            StreamReader sw = new StreamReader(stream);
            string answer = sw.ReadToEnd();
            lock (this)
            {
                Logins.Text += answer + "\n";
            }
            

            sw.Close();
            client.Close();
            
        }
    }
}
