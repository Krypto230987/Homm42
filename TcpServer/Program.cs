using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

RunServer();
void ServerWorker()
{
    int serverPort = 8888;
    IPAddress ipAddress = IPAddress.Loopback;
    EndPoint endPoint = new IPEndPoint(ipAddress, serverPort);
    using (Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
    {
        sender.Bind(endPoint);
        sender.Listen(backlog: 10);
        while (true)
        {
            using (Socket handler = sender.Accept())
            {
                Console.WriteLine("Client connected");
                byte[] getBytes = new byte[1024];
                int bytesCount = handler.Receive(getBytes);
                string request = Encoding.UTF8.GetString(getBytes, 0, bytesCount);
                Console.WriteLine($"Receive: {request}");
                string reply = "";
                switch (request)
                {
                    case "Give me the admin number":
                        reply = "Admin number - 123456789";
                        break;
                    case "Where can I buy a ticket":
                        reply = "Via this link - links";
                        break;
                    case "what time is it":
                        var currentTime = DateTime.Now;
                        reply = $"Сurrent time: {currentTime.Hour}:{currentTime.Minute}";
                        break;
                    case "When do you show the movie":
                        reply = "19:00";
                        break;
                    case "How much does a ticket cost":
                        reply = "The ticket price is $10";
                        break;
                    case "Is there a discount for children":
                        reply = "Yes, there is a 50% discount for children under 12";
                        break;
                    case "Do you have any special deals for groups":
                        reply = "Yes, for groups of 10 or more, we offer a group discount of 20%";
                        break;
                    default:
                        reply = $"your request string has length: {request.Length}";
                        break;
                }
                byte[] sendingBytes = Encoding.UTF8.GetBytes(reply);
                handler.Send(sendingBytes);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                if (request.Contains("Stop"))
                    break;
            }
        }
    }
}
void RunServer()
{
    try
    {
        ServerWorker();
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
    }
}