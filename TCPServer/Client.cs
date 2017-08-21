using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    class Client
    {
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        TcpClient client;
        Server server; // объект сервера

        public Client(TcpClient tcpClient, Server serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                // получаем имя пользователя
                string message = GetMessage();
                userName = message;

                message = userName + " вошел в чат";
                // посылаем сообщение о входе в чат всем подключенным пользователям
                server.BroadcastMessage(message, this.Id);
                Console.WriteLine(message);
                WriteToLog(message);

                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        message = String.Format("{0}: {1}", userName, message);
                        Console.WriteLine(message);
                        WriteToLog(message);
                        server.BroadcastMessage(message, this.Id);
                    }
                    catch
                    {
                        message = String.Format("{0}: покинул чат", userName);
                        Console.WriteLine(message);
                        WriteToLog(message);
                        server.BroadcastMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        private void WriteToLog(string message)
        {
            string logstr = $"{DateTime.Now.ToLongDateString()}/{DateTime.Now.ToLongTimeString()}: {message}" + Environment.NewLine;
            byte[] toLog = Encoding.Unicode.GetBytes(logstr);
            FileStream log = new FileStream("log.txt", FileMode.Append, FileAccess.Write);
            log.Write(toLog, 0, toLog.Length);
            log.Close();
        }

        // чтение входящего сообщения и преобразование в строку
        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }

    //TcpClient client;

    //public Client(TcpClient Client)
    //{
    //    this.client = Client;
    //}


    //public void ClientResponseToUpper()
    //{
    //    Console.WriteLine($"{client.Client.RemoteEndPoint.ToString()} conected");
    //    // Код простой HTML-странички
    //    //string Html = "<html><body><h1>It works!</h1></body></html>";
    //    // Необходимые заголовки: ответ сервера, тип и длина содержимого. После двух пустых строк - само содержимое
    //    //string Str = "HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;
    //    string Str = "Hello, " + client.Client.RemoteEndPoint.ToString() + "!";

    //    // Приведем строку к виду массива байт
    //    byte[] Buffer = Encoding.Unicode.GetBytes(Str);
    //    // Отправим его клиенту
    //    client.GetStream().Write(Buffer, 0, Buffer.Length);

    //    NetworkStream stream = client.GetStream();

    //    while (true)
    //    {
    //        byte[] data;
    //        string message;

    //        data = new byte[64]; // буфер для получаемых данных
    //        StringBuilder builder = new StringBuilder();
    //        int bytes = 0;
    //        do
    //        {
    //            bytes = stream.Read(data, 0, data.Length);
    //            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
    //        }
    //        while (stream.DataAvailable);

    //        message = builder.ToString();

    //        if (message.Contains("exit"))
    //        {
    //            Console.WriteLine(client.Client.RemoteEndPoint.ToString() + " disconected");
    //            break;
    //        }

    //        Console.WriteLine(client.Client.RemoteEndPoint.ToString() + ": " + message);


    //        //Console.Write("Me: ");
    //        // ввод сообщения
    //        //message = Console.ReadLine();
    //        //message = String.Format("{0}: {1}", name, message);
    //        // преобразуем сообщение в массив байтов
    //        //data = Encoding.Unicode.GetBytes(message);
    //        // отправка сообщения
    //        //stream.Write(data, 0, data.Length);

    //        // получаем ответ
    //        message = message.Substring(message.IndexOf(':') + 1).Trim().ToUpper();
    //        data = Encoding.Unicode.GetBytes(message);
    //        stream.Write(data, 0, data.Length);

    //    }

    //    stream.Close();
    //    client.Close();
    //}

}

