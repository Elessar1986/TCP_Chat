﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TCPServer_DataBase;

namespace TCPServer
{
   
    class Program
    {


        static Server server; // сервер
        static Thread listenThread; // потока для прослушивания



        static void Main(string[] args)
        {
            

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

    
