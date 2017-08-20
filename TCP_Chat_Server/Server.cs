using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCP_Chat_Server
{
    class Server
    {

        TcpListener Listener; // Объект, принимающий TCP-клиентов

        // Запуск сервера
        public  Server(int Port)
        {
            // Создаем "слушателя" для указанного порта
            Listener = new TcpListener(IPAddress.Any, Port);
            Listener.Start(); // Запускаем его

            // В бесконечном цикле
            while (true)
            {
                //// Принимаем нового клиента
                //TcpClient Client = Listener.AcceptTcpClientAsync();
                //// Создаем поток
                //Thread thread = new Thread(new ParameterizedThreadStart(ClientThread));
                //// И запускаем этот поток, передавая ему принятого клиента
                //thread.Start(Client);
                ListenerRun();
            }
        }

        // Остановка сервера
        ~Server()
        {
            // Если "слушатель" был создан
            if (Listener != null)
            {
                // Остановим его
                Listener.Stop();
            }
        }

        public void Close()
        {
            Listener.Stop();
        }

        private async void ListenerRun()
        {
            // Принимаем нового клиента
            TcpClient Client = await Listener.AcceptTcpClientAsync();
            // Создаем поток
            Thread thread = new Thread(new ParameterizedThreadStart(ClientThread));
            // И запускаем этот поток, передавая ему принятого клиента
            thread.Start(Client);
        }


        static void ClientThread(Object StateInfo)
        {
            new Client((TcpClient)StateInfo);
        }
    }


}
