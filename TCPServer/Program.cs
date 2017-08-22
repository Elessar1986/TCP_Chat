using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TCPServer
{
   
    class Program
    {


        static Server server; // сервер
        static Thread listenThread; // потока для прослушивания



        static void Main(string[] args)
        {
            serverData data = new serverData();


            var a = from u in data.Messages
                    where u.FromID == (from w in data.User
                                       where w.Name == "Ivan"
                                       select w.UserId).FirstOrDefault()
                    select u;

            foreach (var item in a)
            {
                Console.WriteLine($"{item.FromID} {item.ToId} {item.Message}");
            }

           
            

            Console.ReadKey();
           
            try
            {

                server = new Server();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start(); //старт потока
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }


    }
}

    
