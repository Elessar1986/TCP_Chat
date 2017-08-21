using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
   
    class Program
    {


        static void Main(string[] args)
        {

            new Server(20000);

            Console.ReadKey();
        }

       
    }
}

    
