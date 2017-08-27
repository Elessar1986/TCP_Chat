using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
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
using TCP_Chat_Library;


namespace TCPChat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        User user;

        Source source;
        static string userName;
        //private const string host = "176.38.92.22";
        private const string host = "127.0.0.1";

        private const int port = 20000;
        static TcpClient client;
        static NetworkStream stream;

        public MainWindow()
        {
            
            InitializeComponent();
            
            source = new Source();
            this.DataContext = source;
            Conect();
            Login();
        }

        public void Conect()
        {
            client = new TcpClient();
            try
            {
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); // получаем поток
                userName = Guid.NewGuid().ToString();
                string message = userName;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                WriteMessage("Error" + ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();  
        }

        private void Login()
        {
            LoginWindow passwordWindow = new LoginWindow(this);

            if (passwordWindow.ShowDialog() == true)
            {
                if (passwordWindow.password == "nimana")
                {
                    WriteMessage("Авторизация пройдена");
                    WriteMessage($"Добро пожалывать в чат {passwordWindow.login}");
                    NewUser(passwordWindow.login, passwordWindow.password);
                }
                else 
                {
                    NewUser(passwordWindow.login, passwordWindow.password);


                   
                }
            }
            else
            {
               WriteMessage("Авторизация не пройдена");
            }
        }

        private void NewUser(string name, string password)
        {
            user = new User();
            user.Login = name;
            user.Password = password;
             
        }

        private void WriteMessage(string message)
        {
            source.mainText = source.mainText.Insert(0, message + Environment.NewLine);
        }

        private void SendMessage()
        {
            string message = source.message;
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
            WriteMessage(message);
            source.message = "";
        }

        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    source.mainText = source.mainText.Insert(0, message + Environment.NewLine);
                }
                catch
                {
                    MessageBox.Show("Подключение прервано!"); //соединение было прервано
                    
                    Disconnect();
                }
            }
        }

        private void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            Environment.Exit(0); //завершение процесса
        }

        


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Conect();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            WriteMessage("Test");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (stream != null)
            {
                string message = "exit";
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            Disconnect();
            Environment.Exit(0);
        }
    }

    
}
