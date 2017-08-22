using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPServer
{
    class Server
    {


        static TcpListener tcpListener; // сервер для прослушивания
        List<Client> clients = new List<Client>(); // все подключения

        protected internal void AddConnection(Client clientObject)
        {
            clients.Add(clientObject);
        }
        protected internal void RemoveConnection(string id)
        {
            // получаем по id закрытое подключение
            Client client = clients.FirstOrDefault(c => c.Id == id);
            // и удаляем его из списка подключений
            if (client != null)
                clients.Remove(client);
        }
        // прослушивание входящих подключений
        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 20000);
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    Client clientObject = new Client(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        // трансляция сообщения подключенным клиентам
        protected internal void BroadcastMessage(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id) // если id клиента не равно id отправляющего
                {
                    clients[i].Stream.Write(data, 0, data.Length); //передача данных
                }
            }
        }
        // отключение всех клиентов
        protected internal void Disconnect()
        {
            tcpListener.Stop(); //остановка сервера

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }
    }

    //TcpListener Listener; // Объект, принимающий TCP-клиентов

    //// Запуск сервера
    //public  Server(int Port)
    //{
    //    Console.WriteLine("Starting server...");
    //    // Создаем "слушателя" для указанного порта
    //    Listener = new TcpListener(IPAddress.Any, Port);
    //    Listener.Start(); // Запускаем его
    //    Console.WriteLine($"Server start {DateTime.Now.ToLongTimeString()}");
    //    Console.WriteLine("Waiting for clients...");
    //    // В бесконечном цикле
    //    while (true)
    //    {
    //        // Принимаем нового клиента
    //        //TcpClient Client = Listener.AcceptTcpClient();
    //        //Client client = new Client(Client);
    //        // Создаем поток
    //        //Thread thread = new Thread(new ThreadStart(client.ClientResponseToUpper));
    //        // И запускаем этот поток, передавая ему принятого клиента
    //        //thread.Start(Client);

    //        ListenerRun();
    //    }
    //}

    //// Остановка сервера
    //~Server()
    //{
    //    // Если "слушатель" был создан
    //    if (Listener != null)
    //    {
    //        // Остановим его
    //        Listener.Stop();
    //    }
    //}

    //public void Close()
    //{
    //    Listener.Stop();
    //}

    //private async void ListenerRun()
    //{
    //    // Принимаем нового клиента
    //    TcpClient Client = await Listener.AcceptTcpClientAsync();
    //    Client client = new Client(Client);
    //    // Создаем поток
    //    Thread thread = new Thread(new ThreadStart(client.ClientResponseToUpper));
    //    // И запускаем этот поток, передавая ему принятого клиента
    //    thread.Start();
    //}


    //static void ClientThread(Object StateInfo)
    //{
    //    new Client((TcpClient)StateInfo);
    //}
}



