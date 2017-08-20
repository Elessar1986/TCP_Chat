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
using System.Windows.Threading;

namespace TCP_Chat_Server
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int port = 8888;

        Server server;

        public MainWindow()
        {
            InitializeComponent();
        }

        private  void Button_Click(object sender, RoutedEventArgs e)
        {
            

            try
            {
                Logins.Text += "Сервер запускаеться...";
                server = new Server(port);
                //while (true)
                //{
                    
                //    Thread newThread = new Thread(new ThreadStart(Listener));
                //    newThread.Start();
                //}
                Logins.Text += "Сервер стартовал." + DateTime.Now.ToLongTimeString();
                Logins.Text += "Ожидание подключений...";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (server != null)
                    server.Close();
            }
            
        }

        async void Listener()
        {
            
            //NetworkStream stream = client.GetStream();
            //StreamReader sw = new StreamReader(stream);
            //string answer = sw.ReadToEnd();
            //await Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    Logins.Text += answer + "\n";
            //}));
            
            
            
            //sw.Close();
            
        }
    }
}
