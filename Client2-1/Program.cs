using System;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace Lab_2
{
    class Program
    {
        private const int ECHO_PORT = 8080;

        static void Main(string[] args)
        {
            Console.WriteLine("Ваше имя пользователя?");
            string userName = Console.ReadLine();
            try
            {
                TcpClient eClient = new TcpClient("127.0.0.1", ECHO_PORT);
                StreamReader readerStream = new StreamReader(eClient.GetStream());
                NetworkStream writerStream = eClient.GetStream();
                string dataToSend = userName;
                dataToSend += "\r\n";
                byte[] data = Encoding.ASCII.GetBytes(dataToSend);
                writerStream.Write(data, 0, data.Length);

                while (true)
                {
                    Console.WriteLine(userName + ":");
                    dataToSend = Console.ReadLine();
                    dataToSend += "\r\n";
                    data = Encoding.ASCII.GetBytes(dataToSend);
                    writerStream.Write(data, 0, data.Length);
                    if (dataToSend.IndexOf("QUIT") > -1) break;
                    string returnData = readerStream.ReadLine();
                    Console.WriteLine("сервер: " + returnData);
                }

                eClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}