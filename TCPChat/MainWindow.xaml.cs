using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using System.Runtime.Serialization.Formatters.Binary;


namespace TCPChat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        User user;
        Source source;
        //static string userName;
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
                //userName = Guid.NewGuid().ToString();
                //string message = userName;
                //byte[] data = Encoding.Unicode.GetBytes(message);
                //stream.Write(data, 0, data.Length);

                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                WriteMessage("Error: " + ex.Message);
            }
        }

        private void Button_SendMessage(object sender, RoutedEventArgs e)
        {
            SendCommand(Commands.MyCommands.Message);
            //SendMessage();  
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
                    SendCommand(Commands.MyCommands.UserData);
                    SendUserData(user);
                }
                else 
                {
                    NewUser(passwordWindow.login, passwordWindow.password);
                    WriteMessage($"Добро пожалывать в чат {passwordWindow.login}");
                    SendCommand(Commands.MyCommands.UserData);
                    SendUserData(user);


                }
            }
            else
            {
               WriteMessage("Авторизация не пройдена");
            }
        }

        private void SendCommand(Commands.MyCommands command)
        {
            //BinaryWriter writer = new BinaryWriter(stream);
            //writer.Write((int)command);
            //writer.Flush();
            //writer.Close();
            BinaryFormatter bf = new BinaryFormatter();
            Command newCom = new Command();
            newCom.command = command;
            bf.Serialize(stream, newCom);
            stream.Flush();

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

        private void SendUserData(User user)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, user);
            
        }

        private void SendMessage()
        {
            //string message = source.message;
            //byte[] data = Encoding.Unicode.GetBytes(message);
            //stream.Write(data, 0, data.Length);
            //WriteMessage(message);
            //source.message = "";
        }

        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                   

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

        


        private void Button_ConectToServer(object sender, RoutedEventArgs e)
        {
            Conect();

        }

        

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            
            SendCommand(Commands.MyCommands.Exit);
            Thread.Sleep(100);
            Disconnect();
            Environment.Exit(0);
        }

        private void com1_Click(object sender, RoutedEventArgs e)
        {
            SendCommand(Commands.MyCommands.UserData);
            Thread.Sleep(100);
            SendUserData(user);

        }

        private void com2_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }

    
}
