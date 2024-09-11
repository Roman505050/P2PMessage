using P2PMessage.Networking;

namespace P2PMessage;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Choose mode: (1) Server (2) Client");
        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
            {
                // Server mode
                const int port = 5000; // Port for server
                var server = new TcpServerService(port);

                server.Start();

                Console.WriteLine("Press Enter to stop the server...");
                Console.ReadLine();

                server.Stop();
                break;
            }
            case "2":
            {
                // Client mode
                const string serverIp = "127.0.0.1"; // IP address of the server
                const int port = 5000; // Port for server

                var client = new TcpClientServer(serverIp, port);

                await client.ConnectAsync();

                Console.WriteLine("Enter messages to send to the server. Type 'exit' to quit.");
                string? message;
                while ((message = Console.ReadLine()) != "exit")
                {
                    await client.SendMessageAsync(message);
                }

                client.Disconnect();
                break;
            }
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}