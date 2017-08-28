using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using TCP_Chat_Library;
using System.Runtime.Serialization.Formatters.Binary;
using TCPServer_DataBase;

namespace TCPServer
{
    class Client
    {
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }

        protected internal UserObj user = new UserObj();
        protected internal MessageObj newMessage = new MessageObj();
        protected internal ErrorObj errorCode = new ErrorObj();
        TcpClient client;
        Server server; // объект сервера
        Command ResCommand;
        

        public Client(TcpClient tcpClient, Server serverObject)
        {
            Id = Guid.NewGuid().ToString();
            ResCommand = new Command();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {

                Stream = client.GetStream();

                // в бесконечном цикле получаем сообщения от клиента
                do
                {

                    try
                    {
                        ResCommand.command = GetCommandFromClient();
                        switch (ResCommand.command)
                        {

                            case Commands.MyCommands.Message:
                                {
                                    GetMessage();
                                    break;
                                }
                            case Commands.MyCommands.UserData:
                                {
                                    GetUserData();
                                    if (server.GetUserId(user) != -1)
                                        Console.WriteLine($"{user.Login} ({client.Client.RemoteEndPoint}) conected");
                                    else
                                        SendError(ErrorCodeEnum.ErrorCode.WrongLoginOrPass);
                                        
                                    break;
                                }
                            case Commands.MyCommands.Exit:
                                {
                                    Console.WriteLine($"{user.Login} покинул чат");
                                    WriteToLog($"{user.Login}  покинул чат");
                                    server.RemoveConnection(user.UserID);
                                    Close();
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("ERROR1: " + ex.Message);
                    }

                } while (ResCommand.command != Commands.MyCommands.Exit);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR2: " + e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(user.UserID);
                Close();
            }
        }

        private void SendError(ErrorCodeEnum.ErrorCode code)
        {
            SendCommand(Commands.MyCommands.Error);
            BinaryFormatter bf = new BinaryFormatter();
            errorCode = new ErrorObj();
            errorCode.errorCode = code;
            bf.Serialize(Stream, errorCode);

        }

        private void SendCommand(Commands.MyCommands command)
        {

            BinaryFormatter bf = new BinaryFormatter();
            Command newCom = new Command();
            newCom.command = command;
            bf.Serialize(Stream, newCom);
            //Stream.Flush();

        }

        private void GetUserData()
        {
            BinaryFormatter bf = new BinaryFormatter();

            user = bf.Deserialize(Stream) as TCP_Chat_Library.UserObj;
            

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
        private void GetMessage()
        {
            BinaryFormatter bf = new BinaryFormatter();

            newMessage = bf.Deserialize(Stream) as MessageObj;
            Console.WriteLine($"({user.Login}): {newMessage.message}");
        }

        private Commands.MyCommands GetCommandFromClient()
        {
           
            BinaryFormatter bf = new BinaryFormatter();

            Command com = bf.Deserialize(Stream) as Command;
            
            if (com.command < 0) throw new Exception("Неверная комманда");
            //reader.Close();
            return com.command; ;
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

    

}

