using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TCPClientConsoleTest
{
    class Program
    {


        static string userName;
        private const string host = "192.168.0.102";
        private const int port = 20000;
        static TcpClient client;
        static NetworkStream stream;

        static void Main(string[] args)
        {
            Console.Write("Введите свое имя: ");
            userName = Console.ReadLine();
            client = new TcpClient();
            try
            {
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); // получаем поток

                string message = userName;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока
                Console.WriteLine("Добро пожаловать, {0}", userName);
                SendMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }
        // отправка сообщений
        static void SendMessage()
        {
            Console.WriteLine("Введите сообщение: ");

            while (true)
            {
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
        }
        // получение сообщений
        static void ReceiveMessage()
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
                    Console.WriteLine(message);//вывод сообщения
                }
                catch
                {
                    Console.WriteLine("Подключение прервано!"); //соединение было прервано
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        static void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            Environment.Exit(0); //завершение процесса
        }

        //static int PORT = 20000;
        //static string IPaddress = "192.168.0.102";

        //static void Main(string[] args)
        //{

        //    Console.WriteLine("Enter name:");
        //    string name = Console.ReadLine();
        //    TcpClient client = null;
        //    try
        //    {
        //        client = new TcpClient(IPaddress, PORT);
        //        NetworkStream stream = client.GetStream();
        //        while (true)
        //        {
        //            byte[] data;
        //            string message;

        //            data = new byte[64]; // буфер для получаемых данных
        //            StringBuilder builder = new StringBuilder();
        //            int bytes = 0;
        //            do
        //            {
        //                bytes = stream.Read(data, 0, data.Length);
        //                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
        //            }
        //            while (stream.DataAvailable);

        //            message = builder.ToString();
        //            Console.WriteLine("Сервер: {0}", message);


        //            Console.Write(name + ": ");
        //            // ввод сообщения
        //            message = Console.ReadLine();
        //            //message = String.Format("{0}: {1}", name, message);
        //            // преобразуем сообщение в массив байтов

        //            data = Encoding.Unicode.GetBytes(message);
        //            // отправка сообщения
        //            stream.Write(data, 0, data.Length);
        //            if (message == "exit") break;
        //            // получаем ответ

        //        }

        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine("Error:" + ex.Message);
        //    }
        //    finally
        //    {
        //        client.Close();
        //    }



    }
    
}
