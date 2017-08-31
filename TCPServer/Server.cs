using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCPServer_DataBase;
using TCP_Chat_Library;

namespace TCPServer
{
    class Server
    {


        static TcpListener tcpListener; // сервер для прослушивания
        List<Client> clients = new List<Client>(); // все подключения

        public List<UserObj> usersOnline = new List<UserObj>();

        serverData data = new serverData();

        public Server()
        {

        }

        protected internal long GetUserId(UserObj user)
        {
            var res = from us in data.User
                      where us.Login == user.Login && us.Password == user.Password
                      select us.UserId;
            if (res.Count() == 0)
                return -1;
            else
                return res.FirstOrDefault();
        }

        protected internal void AddUserOnline(UserObj user)
        {
            usersOnline.Add(user);
        }

        protected internal void AddConnection(Client clientObject)
        {
            clients.Add(clientObject);
            
        }
        protected internal void RemoveConnection(long id)
        {
            // получаем по id закрытое подключение
            Client client = clients.FirstOrDefault(c => c.user.UserID == id);
            // и удаляем его из списка подключений
            if (client != null)
                clients.Remove(client);

            UserObj user = usersOnline.FirstOrDefault(x => x.UserID == id);

            if(usersOnline != null)
            {
                usersOnline.Remove(user);
            }
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

        protected internal void GetUserInfo(ref UserObj user)
        {
            long uId = user.UserID;
            var res = from us in data.User
                      where us.UserId == uId
                      select us;
            user = MyConverter.UserToUserObj(res.FirstOrDefault());
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

        internal void AddUserData(UserObj user)
        {
            data.User.Add(MyConverter.UserObjToUser(user));
            data.SaveChanges();
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



