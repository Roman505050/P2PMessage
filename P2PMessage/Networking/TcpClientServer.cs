using System.Net.Sockets;
using System.Text;

namespace P2PMessage.Networking
{
    public class TcpClientServer(string serverIp, int serverPort)
    {
        private readonly TcpClient _client = new();

        public async Task ConnectAsync()
        {
            try
            {
                await _client.ConnectAsync(serverIp, serverPort);
                Console.WriteLine($"Connected to {serverIp}:{serverPort}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }

        }

        public async Task SendMessageAsync(string? message)
        {
            if (_client.Connected)
            {
                NetworkStream stream = _client.GetStream();
                if (message != null)
                {
                    byte[] messageBytes = Encoding.ASCII.GetBytes(message);

                    try
                    {
                        // Send message to server
                        await stream.WriteAsync(messageBytes);
                        Console.WriteLine($"Sent message: {message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Connection not established");
            }
        }

        public void Disconnect()
        {
            if (_client.Connected)
            {
                _client.Close();
                Console.WriteLine($"Disconnected from {serverIp}:{serverPort}");
            }
            else
            {
                Console.WriteLine($"Connection not established");
            }
        }
    }
}