using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server_2_1
{
    public class ClientHandler
    {
        public TcpClient clientSocket;

        public void RunClient()
        {
            StreamReader readStream = new StreamReader(clientSocket.GetStream());
            NetworkStream writeStream = clientSocket.GetStream();
            string returnData = readStream.ReadLine();
            string username = returnData;
            Console.WriteLine("Добро пожаловать на сервер " + username);
            while (true)
            {
                returnData = readStream.ReadLine();
                if (returnData.IndexOf("QUIT") > -1)
                {
                    Console.WriteLine("До свидания " + username);
                    break;
                }

                Console.WriteLine(username + ": " + returnData);
                returnData += "\r\n";
                byte[] dataWrite = Encoding.ASCII.GetBytes(returnData);
                writeStream.Write(dataWrite, 0, dataWrite.Length);
            }

            clientSocket.Close();
        }

    }

    class Program
    {
        private const int ECHO_PORT = 8080;
        public static int nClients = 0;

        static void Main(string[] args)
        {
            try
            {
                TcpListener clientListener = new TcpListener(ECHO_PORT);
                clientListener.Start();
                Console.WriteLine("Ожидание соединения...");
                while (true)
                {
                    TcpClient client = clientListener.AcceptTcpClient();
                    ClientHandler cHandler = new ClientHandler();
                    cHandler.clientSocket = client;
                    Thread clientThread = new Thread(new ThreadStart(cHandler.RunClient));
                    clientThread.Start();
                }

                clientListener.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
