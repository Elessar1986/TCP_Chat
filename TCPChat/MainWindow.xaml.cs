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
using System.Windows.Threading;

namespace TCPChat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserObj user;
        MessageObj newMessage = new MessageObj();
        Source source;
        //static string userName;
        //private const string host = "176.38.92.22";
        private const string host = "127.0.0.1";

        private const int port = 20000;
        static TcpClient client;
        static NetworkStream stream;
        Command ResCommand;
        ErrorObj error = new ErrorObj();

        public MainWindow()
        {
            
            InitializeComponent();
            PropertiesButton.IsEnabled = false;
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
            SendMessage();  
        }

        private void Login()
        {
            LoginWindow passwordWindow = new LoginWindow(this);

            if (passwordWindow.ShowDialog() == true)
            {
                if (passwordWindow.password == "nimana")
                {
                    PropertiesButton.IsEnabled = true;
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
            
            BinaryFormatter bf = new BinaryFormatter();
            Command newCom = new Command();
            newCom.command = command;
            bf.Serialize(stream, newCom);
            stream.Flush();

        }

        private void NewUser(string name, string password)
        {
            user = new UserObj();
            user.Login = name;
            user.Password = password;
             
        }

        private void WriteMessage(string message)
        {
            source.mainText = source.mainText.Insert(0, message + Environment.NewLine);
        }

        private void SendUserData(UserObj user)
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
            newMessage = new MessageObj();
            newMessage.message = source.message;
            source.message = "";
            WriteMessage(newMessage.message);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, newMessage);
            

        }

        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    ResCommand = new Command();
                    ResCommand.command = GetCommandFromServer();
                    
                    switch (ResCommand.command)
                    {

                        case Commands.MyCommands.Message:
                            {
                                
                                //GetMessage();
                                break;
                            }
                        case Commands.MyCommands.UserData:
                            {
                                //GetUserData();

                                

                                break;
                            }
                        case Commands.MyCommands.Error:
                            {
                                GetError();
                                WriteMessage($"Error: {error.errorCode}");
                                switch (error.errorCode)
                                {
                                    case ErrorCodeEnum.ErrorCode.WrongLoginOrPass:
                                        {
                                            WriteMessage("Try one more time.");
                                            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                                                        (ThreadStart)delegate ()
                                                                {
                                                                    Login();
                                                                }
                                                            );
                                            
                                        }
                                        break;
                                    case ErrorCodeEnum.ErrorCode.ServerShotdown:
                                        {

                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            }
                        default:
                            break;
                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show($"ERROR: {ex.Message}"); //соединение было прервано
                    
                    //Disconnect();
                }
            }
        }

        private void GetError()
        {
            BinaryFormatter bf = new BinaryFormatter();

            error = bf.Deserialize(stream) as ErrorObj;

        }

        private Commands.MyCommands GetCommandFromServer()
        {
            BinaryFormatter bf = new BinaryFormatter();

            Command com = bf.Deserialize(stream) as Command;

            if (com.command < 0) throw new Exception("Неверная комманда");
            //reader.Close();
            return com.command; ;
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
