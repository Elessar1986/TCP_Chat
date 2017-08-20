using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    class Client : IDisposable
    {
        NetworkStream s;

        public Client(TcpClient client)
        {
            s = client.GetStream();
        }

        public void Dispose()
        {
            s.Dispose();
        }

        async Task<string> ReadFromStreamAsync()
        {
            var buf = new byte[s.Length];
            var readpos = 0;
            //while (readpos < nbytes)
            //    readpos += await s.ReadAsync(buf, readpos, nbytes - readpos);
            await s.ReadAsync(buf, 0, Convert.ToInt32(s.Length));
            string str = Encoding.UTF8.GetString(buf);
            return str;
        }

        public async Task ProcessAsync()
        {
            while (true)
            {
                string res = await ReadFromStreamAsync();
                Console.WriteLine(res + "\n");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting server...");
            RunServerAsync();

            Console.ReadKey();
        }

        public static async void RunServerAsync()
        {

            var tcpListener = new TcpListener(IPAddress.Any,20000);
            tcpListener.Start();
            Console.WriteLine("server start " + DateTime.Now.ToLongDateString());
            while (true) // тут какое-то разумное условие выхода
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                processClientTearOff(tcpClient); // await не нужен
            }
        }

        async static Task processClientTearOff(TcpClient c)
        {
            using (var client = new Client(c))
                await client.ProcessAsync();
        }
    }
}

    
