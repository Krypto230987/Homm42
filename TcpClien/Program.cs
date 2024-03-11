using System.Net;
using System.Net.Sockets;
using System.Text;
int serverPort = 8888;
string stopMessage = "Stop";
RunClient();
bool ClientWorker()
{
    IPAddress ipAddress = IPAddress.Loopback;
    EndPoint endPoint = new IPEndPoint(ipAddress, serverPort);
    using (Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
    {
       sender.Connect(endPoint);
           Console.WriteLine($"Client connected to {sender.RemoteEndPoint}");
           Console.WriteLine("Enter message:");
           string? message = Console.ReadLine();
            if (message != null)
           {
               byte[] byteMessage = Encoding.UTF8.GetBytes(message);
               sender.Send(byteMessage);
           }
           sender.Send(byteMessage);
           byte[] getMessage = new byte[1024];
           int bytesReceived = sender.Receive(getMessage);
           string response = Encoding.UTF8.GetString(getMessage, 0, bytesReceived);
           Console.WriteLine(response);
           sender.Shutdown(SocketShutdown.Both);
           sender.Close();
       
           return !message.Contains(stopMessage); 
    }
    

}
void RunClient()
{
    try
    {
        while (ClientWorker()) { }
        Console.WriteLine("Stop Client");
    }
    catch
    {
        Console.WriteLine("Error");
    }
}
