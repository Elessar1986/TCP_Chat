using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCPClientConsoleTest
{
    class Program
    {

        static int PORT = 20000;
        static string IPaddress = "192.168.0.102";

        static void Main(string[] args)
        {
            Console.WriteLine("Enter name:");
            string name = Console.ReadLine();
            TcpClient client = null;
            try
            {
                client = new TcpClient(IPaddress, PORT);
                NetworkStream stream = client.GetStream();
                while (true)
                {
                    byte[] data;
                    string message;

                    data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    message = builder.ToString();
                    Console.WriteLine("Сервер: {0}", message);


                    Console.Write(name + ": ");
                    // ввод сообщения
                    message = Console.ReadLine();
                    //message = String.Format("{0}: {1}", name, message);
                    // преобразуем сообщение в массив байтов
                    
                    data = Encoding.Unicode.GetBytes(message);
                    // отправка сообщения
                    stream.Write(data, 0, data.Length);
                    if (message == "exit") break;
                    // получаем ответ

                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
            }
            finally
            {
                client.Close();
            }



        }
    }
}
