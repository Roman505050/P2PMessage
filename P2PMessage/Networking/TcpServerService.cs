using System.Net;
using System.Net.Sockets;
using System.Text;

namespace P2PMessage.Networking
{
    public class TcpServerService
    {
        private readonly TcpListener _tcpListener;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly int _port;

        public TcpServerService(int port)
        {
            _port = port;
            _tcpListener = new TcpListener(IPAddress.Any, _port);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            _tcpListener.Start();
            Console.WriteLine($"Server started on port {_port}.");

            Task.Run(() => AcceptClientsAsync(_cancellationTokenSource.Token));
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _tcpListener.Stop();
            Console.WriteLine("Server stopped.");
        }

        private async Task AcceptClientsAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var tcpClient = await _tcpListener.AcceptTcpClientAsync(cancellationToken);
                    Console.WriteLine("Client connected.");
                    await Task.Run(() => HandleClientAsync(tcpClient, cancellationToken));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private async Task HandleClientAsync(TcpClient tcpClient, CancellationToken cancellationToken)
        {
            using (tcpClient)
            {
                NetworkStream networkStream = tcpClient.GetStream();

                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await networkStream.ReadAsync(buffer, cancellationToken)) != 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received: {message}");
                }
            }

            Console.WriteLine("Client disconnected.");
        }
    }
}